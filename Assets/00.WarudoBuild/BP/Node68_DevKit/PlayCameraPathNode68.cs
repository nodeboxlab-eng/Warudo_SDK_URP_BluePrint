#if !NODE68_SHARE_BUILD

using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Data.Models;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// SET_ASSET_TRANSFORM 체인을 하나의 노드로 재생합니다.
    /// Keyframes 는 에셋 Inspector 「변형(Transform)」의 Position / Rotation / Scale 과 동일한 값입니다.
    /// 개발 빌드 전용 DevKit 노드입니다.
    /// </summary>
    [NodeType(
        Id = "a7c3e891-4f2b-4d6e-9c81-2b5f8e3d1a04",
        Title = "Play Camera Path Node68",
        Category = BpToolkitCategories.Toolkit,
        Width = 1.42f
    )]
    public sealed class PlayCameraPathNode68 : Node
    {
        public override long GetVersion() => 13;

        private const float InstantDurationThreshold = 0.015f;
        private const string TransformPositionKey =
            nameof(GameObjectAsset.Transform) + "." + nameof(TransformData.Position);
        private const string TransformRotationKey =
            nameof(GameObjectAsset.Transform) + "." + nameof(TransformData.Rotation);
        private const string TransformScaleKey =
            nameof(GameObjectAsset.Transform) + "." + nameof(TransformData.Scale);

        [DataInput]
        [Section("에셋")]
        [Label("에셋")]
        public GameObjectAsset Asset;

        [DataInput]
        [Section("경로")]
        [Label("키프레임")]
        public CameraKeyframe68[] Keyframes = Array.Empty<CameraKeyframe68>();

        [DataInput]
        [Section("경로")]
        [Label("재생 중 구간")]
        [Disabled]
        public string PlaybackStatus = "—";

        [DataInput]
        [Section("테스트")]
        [Label("시작 키프레임 (#)")]
        [Description("1 = 처음부터. 2 = #2 구간부터 (#1 끝 위치에서 즉시 이동 후 재생).")]
        [FloatSlider(1f, 32f)]
        public float StartKeyframeNumber = 1f;

        [DataInput]
        [Section("옵션")]
        [Label("시작 시 카메라 전환")]
        public bool ToggleCameraOnStart;

        [DataInput]
        [Section("옵션")]
        [Label("완료 후 시작 위치로 복귀")]
        public bool ReturnToOriginalTransform;

        [DataInput]
        [Section("옵션")]
        [Label("복귀 시간 (초)")]
        [FloatSlider(0f, 120f)]
        [DisabledIf(nameof(ReturnToOriginalTransform), Is.False)]
        public float ReturnDuration = 1f;

        [DataInput]
        [Section("옵션")]
        [Label("복귀 이징")]
        [DisabledIf(nameof(ReturnToOriginalTransform), Is.False)]
        public Ease ReturnEasing = Ease.OutCubic;

        [DataInput]
        [Section("옵션")]
        [Label("반복 재생")]
        public bool Loop;

        /// <summary>구버전 그래프 호환용. 재생 중 항상 None 으로 전환합니다.</summary>
        [DataInput]
        [Hidden]
        public bool DisableCameraControlOnPlay = true;

        /// <summary>구버전 그래프 호환용. 종료·중단 시 항상 이전 모드로 복구합니다.</summary>
        [DataInput]
        [Hidden]
        public bool RestoreCameraControlOnFinish = true;

        [DataInput]
        [Section("캡처")]
        [Label("캡처 기본 시간 (초)")]
        public float CaptureDuration = 1f;

        [DataInput]
        [Section("캡처")]
        [Label("캡처 기본 이징")]
        public Ease CaptureEasing = Ease.OutCubic;

        /// <summary>구버전 그래프 호환용. <see cref="Asset"/> 로 마이그레이션됩니다.</summary>
        [DataInput]
        [Hidden]
        public CameraAsset Camera;

        private Sequence _sequence;
        private AssetTransformSnapshot _originalTransform;
        private GameObjectAsset _pathAsset;
        private AssetTransformProxy _pathProxy;
        private bool _pathPlaying;
        private bool _pathPaused;
        private bool _pathDirty;
        private int _activeKeyframeIndex = -1;
        private bool _activeSegmentIsReturn;
        private readonly List<SavedCameraControlState> _savedCameraControlStates =
            new List<SavedCameraControlState>();

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            MigrateLegacyCameraField();
        }

        private void MigrateLegacyCameraField()
        {
            if (Asset != null || Camera == null)
                return;

            SetDataInput(nameof(Asset), Camera, broadcast: true);
        }

        protected override void OnDestroy()
        {
            KillSequence(invokeCancelledFlow: false);
            RestoreCameraControlIfNeeded();
            base.OnDestroy();
        }

        public override void OnPostLateUpdate()
        {
            base.OnPostLateUpdate();

            if (!_pathPlaying || _pathAsset == null)
                return;

            ForceDisableCameraControlForPlayback(_pathAsset);
        }

        public override void OnEndOfFrame()
        {
            base.OnEndOfFrame();

            if (!_pathPlaying || _pathPaused || _pathAsset == null || _pathProxy == null || !_pathDirty)
                return;

            ForceDisableCameraControlForPlayback(_pathAsset);

            ApplyAssetTransform(_pathAsset, _pathProxy.Snapshot);
            _pathDirty = false;
        }

        [FlowInput]
        public Continuation Enter()
        {
            KillSequence(invokeCancelledFlow: false);
            PlayPath();
            return Exit;
        }

        [FlowInput]
        [Label("Pause")]
        public Continuation Pause()
        {
            if (!TryPausePath())
                Debug.LogWarning("[Play Camera Path Node68] 일시정지할 재생이 없습니다.");

            return Exit;
        }

        [FlowInput]
        [Label("Resume")]
        public Continuation Resume()
        {
            if (!TryResumePath())
                Debug.LogWarning("[Play Camera Path Node68] 재개할 일시정지 상태가 없습니다.");

            return Exit;
        }

        [FlowInput]
        [Label("Stop")]
        public Continuation Stop()
        {
            StopPath(invokeCancelled: true);
            return Exit;
        }

        [Trigger]
        [Label("Pause")]
        public void PausePathTrigger()
        {
            if (!TryPausePath())
                Debug.LogWarning("[Play Camera Path Node68] 일시정지할 재생이 없습니다.");
        }

        [Trigger]
        [Label("Resume")]
        public void ResumePathTrigger()
        {
            if (!TryResumePath())
                Debug.LogWarning("[Play Camera Path Node68] 재개할 일시정지 상태가 없습니다.");
        }

        [FlowInput]
        [Label("CaptureKeyframe")]
        public Continuation CaptureKeyframe()
        {
            if (!TryAppendCapturedKeyframe(out var message))
                Debug.LogWarning(message);
            else
                Debug.Log(message);

            return OnCaptured;
        }

        [Trigger]
        [Label("Capture Keyframe")]
        public void CaptureCurrentKeyframe()
        {
            if (!TryAppendCapturedKeyframe(out var message))
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("OnFinished")]
        public Continuation OnFinished;

        [FlowOutput]
        [Label("OnCancelled")]
        public Continuation OnCancelled;

        [FlowOutput]
        [Label("OnPaused")]
        public Continuation OnPaused;

        [FlowOutput]
        [Label("OnCaptured")]
        public Continuation OnCaptured;

        private GameObjectAsset ResolveAsset() => Asset != null ? Asset : Camera;

        private bool TryAppendCapturedKeyframe(out string message)
        {
            message = string.Empty;

            var asset = ResolveAsset();
            if (asset == null)
            {
                message = "[Play Camera Path Node68] Asset이 지정되지 않았습니다.";
                return false;
            }

            if (!TryReadAssetTransform(asset, out var snapshot))
            {
                message = "[Play Camera Path Node68] Asset Transform을 읽을 수 없습니다.";
                return false;
            }

            var list = new List<CameraKeyframe68>(Keyframes ?? Array.Empty<CameraKeyframe68>());
            var keyframe = StructuredData.Create<CameraKeyframe68, PlayCameraPathNode68>(
                this,
                item =>
                {
                    item.Position = snapshot.Position;
                    item.Rotation = snapshot.RotationEuler;
                    item.Scale = snapshot.Scale;
                    item.Duration = CaptureDuration;
                    item.Easing = CaptureEasing;
                }
            );

            list.Add(keyframe);
            SetDataInput(nameof(Keyframes), list.ToArray(), broadcast: true);

            message =
                "[Play Camera Path Node68] Keyframe 추가 (#"
                + list.Count
                + ") · "
                + FormatTransform(snapshot);
            return true;
        }

        private void PlayPath()
        {
            var asset = ResolveAsset();
            if (asset == null)
            {
                Debug.LogWarning("[Play Camera Path Node68] Asset이 지정되지 않았습니다.");
                InvokeFlow(nameof(OnFinished));
                return;
            }

            if (Keyframes == null || Keyframes.Length == 0)
            {
                Debug.LogWarning("[Play Camera Path Node68] Keyframes가 비어 있습니다.");
                InvokeFlow(nameof(OnFinished));
                return;
            }

            if (!TryReadAssetTransform(asset, out var currentTransform))
            {
                Debug.LogWarning("[Play Camera Path Node68] Asset Transform을 읽을 수 없습니다.");
                InvokeFlow(nameof(OnFinished));
                return;
            }

            if (ToggleCameraOnStart && asset is CameraAsset cameraForToggle)
                TryToggleCamera(cameraForToggle);

            BeginPlayback(asset);

            _originalTransform = currentTransform;
            var startIndex = ResolveStartKeyframeIndex(Keyframes);
            var startSnapshot = ResolveSegmentStartSnapshot(startIndex, currentTransform);
            _pathProxy = new AssetTransformProxy(startSnapshot);
            _sequence = DOTween.Sequence().SetUpdate(UpdateType.Late);
            AppendPathSegments(_sequence, _pathProxy, Keyframes, startIndex);

            if (startIndex > 0)
            {
                Debug.Log(
                    "[Play Camera Path Node68] #"
                        + (startIndex + 1)
                        + " 구간부터 재생합니다."
                );
            }

            if (ReturnToOriginalTransform && !Loop)
            {
                AppendTransformSegment(
                    _sequence,
                    _pathProxy,
                    _originalTransform,
                    ReturnDuration,
                    ReturnEasing,
                    keyframeIndex: -1,
                    isReturnSegment: true
                );
            }

            MarkPathTransformDirty();
            ApplyAssetTransform(_pathAsset, _pathProxy.Snapshot);

            if (Loop)
            {
                _sequence.SetLoops(-1);
                return;
            }

            _sequence.OnComplete(() =>
            {
                CommitCurrentPathTransform();
                FinishPlayback(invokeFinished: true, restoreCameraControl: true);
            });
        }

        private void BeginPlayback(GameObjectAsset asset)
        {
            _pathAsset = asset;
            _pathPlaying = true;
            _pathPaused = false;
            _pathDirty = true;
            _savedCameraControlStates.Clear();
            ForceDisableCameraControlForPlayback(asset);
        }

        private void SaveAndDisableCameraControl(CameraAsset camera)
        {
            if (camera == null)
                return;

            for (var i = 0; i < _savedCameraControlStates.Count; i++)
            {
                if (_savedCameraControlStates[i].Camera != camera)
                    continue;

                ForceDisableCameraControl(camera);
                return;
            }

            _savedCameraControlStates.Add(
                new SavedCameraControlState(
                    camera,
                    GetCameraControlMode(camera),
                    camera.ShouldUpdateFreeLook
                )
            );
            ForceDisableCameraControl(camera);
        }

        private static void ForceDisableCameraControl(CameraAsset camera)
        {
            if (camera == null)
                return;

            try
            {
                camera.ShouldUpdateFreeLook = false;
                camera.ControlMode = CameraAsset.CameraControlMode.None;
                camera.SetDataInput(
                    nameof(CameraAsset.ControlMode),
                    CameraAsset.CameraControlMode.None,
                    broadcast: true
                );
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[Play Camera Path Node68] Control Mode None 적용 실패: " + ex.Message
                );
            }
        }

        private void ForceDisableCameraControlForPlayback(GameObjectAsset asset)
        {
            SaveAndDisableCameraControl(asset as CameraAsset);

            var mainCamera = TryGetMainCamera();
            if (mainCamera != null && mainCamera != asset)
                SaveAndDisableCameraControl(mainCamera);
        }

        private void FinishPlayback(bool invokeFinished, bool restoreCameraControl)
        {
            _sequence = null;
            _pathPlaying = false;
            _pathPaused = false;
            _pathAsset = null;
            _pathProxy = null;
            _pathDirty = false;
            ClearPlaybackSegmentDisplay();

            if (restoreCameraControl)
                RestoreCameraControlIfNeeded();

            if (invokeFinished)
                InvokeFlow(nameof(OnFinished));
        }

        private bool TryPausePath()
        {
            if (!_pathPlaying || _pathPaused || _sequence == null || !_sequence.IsActive())
                return false;

            _sequence.Pause();
            _pathPaused = true;
            CommitCurrentPathTransform();
            RefreshPlaybackStatusDisplay();
            InvokeFlow(nameof(OnPaused));
            return true;
        }

        private bool TryResumePath()
        {
            if (!_pathPlaying || !_pathPaused || _sequence == null)
                return false;

            _sequence.Play();
            _pathPaused = false;
            RefreshPlaybackStatusDisplay();
            return true;
        }

        private void StopPath(bool invokeCancelled)
        {
            KillSequence(invokeCancelledFlow: invokeCancelled);
        }

        internal bool IsActiveKeyframe(CameraKeyframe68 keyframe)
        {
            if (!_pathPlaying || _activeSegmentIsReturn || keyframe == null)
                return false;

            if (_activeKeyframeIndex < 0 || Keyframes == null)
                return false;

            return _activeKeyframeIndex < Keyframes.Length
                && Keyframes[_activeKeyframeIndex] == keyframe;
        }

        internal int GetKeyframeDisplayIndex(CameraKeyframe68 keyframe)
        {
            if (Keyframes == null || keyframe == null)
                return 0;

            for (var i = 0; i < Keyframes.Length; i++)
            {
                if (Keyframes[i] == keyframe)
                    return i + 1;
            }

            return 0;
        }

        private void SetActiveSegment(int keyframeIndex, bool isReturnSegment)
        {
            _activeKeyframeIndex = keyframeIndex;
            _activeSegmentIsReturn = isReturnSegment;
            RefreshPlaybackStatusDisplay();
        }

        private void RefreshPlaybackStatusDisplay()
        {
            PlaybackStatus = BuildPlaybackStatus();
            BroadcastDataInput(nameof(PlaybackStatus));
            BroadcastDataInput(nameof(Keyframes));
        }

        private void ClearPlaybackSegmentDisplay()
        {
            _activeKeyframeIndex = -1;
            _activeSegmentIsReturn = false;
            PlaybackStatus = "—";
            BroadcastDataInput(nameof(PlaybackStatus));
            BroadcastDataInput(nameof(Keyframes));
        }

        private string BuildPlaybackStatus()
        {
            if (!_pathPlaying)
                return "—";

            var prefix = _pathPaused ? "⏸ " : "▶ ";

            if (_activeSegmentIsReturn)
            {
                return
                    prefix
                    + "복귀 · 시작 위치 · "
                    + ReturnDuration.ToString("0.##")
                    + "s · "
                    + ReturnEasing;
            }

            if (_activeKeyframeIndex < 0 || Keyframes == null)
                return prefix + "재생 중";

            if (_activeKeyframeIndex >= Keyframes.Length)
                return prefix + "#" + (_activeKeyframeIndex + 1);

            var keyframe = Keyframes[_activeKeyframeIndex];
            if (keyframe == null)
                return prefix + "#" + (_activeKeyframeIndex + 1);

            return
                prefix
                + "#"
                + (_activeKeyframeIndex + 1)
                + " · P ("
                + keyframe.Position.x.ToString("0.##")
                + ", "
                + keyframe.Position.y.ToString("0.##")
                + ", "
                + keyframe.Position.z.ToString("0.##")
                + ") · "
                + keyframe.Duration.ToString("0.##")
                + "s · "
                + keyframe.Easing;
        }

        private void RestoreCameraControlIfNeeded()
        {
            for (var i = 0; i < _savedCameraControlStates.Count; i++)
            {
                var saved = _savedCameraControlStates[i];
                var camera = saved.Camera;
                if (camera == null)
                    continue;

                try
                {
                    camera.ControlMode = saved.ControlMode;
                    camera.SetDataInput(
                        nameof(CameraAsset.ControlMode),
                        saved.ControlMode,
                        broadcast: true
                    );
                    camera.ShouldUpdateFreeLook = saved.ShouldUpdateFreeLook;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        "[Play Camera Path Node68] Control Mode 복구 실패: " + ex.Message
                    );
                }
            }

            _savedCameraControlStates.Clear();
        }

        private readonly struct SavedCameraControlState
        {
            public readonly CameraAsset Camera;
            public readonly CameraAsset.CameraControlMode ControlMode;
            public readonly bool ShouldUpdateFreeLook;

            public SavedCameraControlState(
                CameraAsset camera,
                CameraAsset.CameraControlMode controlMode,
                bool shouldUpdateFreeLook
            )
            {
                Camera = camera;
                ControlMode = controlMode;
                ShouldUpdateFreeLook = shouldUpdateFreeLook;
            }
        }

        private int ResolveStartKeyframeIndex(CameraKeyframe68[] keyframes)
        {
            if (keyframes == null || keyframes.Length == 0)
                return 0;

            var number = Mathf.RoundToInt(StartKeyframeNumber);
            number = Mathf.Clamp(number, 1, keyframes.Length);
            return number - 1;
        }

        private AssetTransformSnapshot ResolveSegmentStartSnapshot(
            int startIndex,
            AssetTransformSnapshot enterTransform
        )
        {
            if (startIndex <= 0 || Keyframes == null)
                return enterTransform;

            for (var i = startIndex - 1; i >= 0; i--)
            {
                var keyframe = Keyframes[i];
                if (keyframe == null)
                    continue;

                return new AssetTransformSnapshot(
                    keyframe.Position,
                    keyframe.Rotation,
                    keyframe.Scale
                );
            }

            return enterTransform;
        }

        private void MarkPathTransformDirty()
        {
            _pathDirty = true;
        }

        private void AppendPathSegments(
            Sequence sequence,
            AssetTransformProxy proxy,
            CameraKeyframe68[] keyframes,
            int startIndex
        )
        {
            if (sequence == null || proxy == null || keyframes == null)
                return;

            startIndex = Mathf.Clamp(startIndex, 0, keyframes.Length);

            for (var i = startIndex; i < keyframes.Length; i++)
            {
                var keyframe = keyframes[i];
                if (keyframe == null)
                    continue;

                AppendTransformSegment(
                    sequence,
                    proxy,
                    new AssetTransformSnapshot(
                        keyframe.Position,
                        keyframe.Rotation,
                        keyframe.Scale
                    ),
                    keyframe.Duration,
                    keyframe.Easing,
                    keyframeIndex: i,
                    isReturnSegment: false
                );
            }
        }

        private void AppendTransformSegment(
            Sequence sequence,
            AssetTransformProxy proxy,
            AssetTransformSnapshot target,
            float duration,
            Ease easing,
            int keyframeIndex,
            bool isReturnSegment
        )
        {
            sequence.AppendCallback(() => SetActiveSegment(keyframeIndex, isReturnSegment));

            if (IsInstantDuration(duration))
            {
                sequence.AppendCallback(() =>
                {
                    proxy.CopyFrom(target);
                    MarkPathTransformDirty();
                });
                return;
            }

            var tweenDuration = Mathf.Max(0.02f, duration);
            var step = DOTween.Sequence().SetUpdate(UpdateType.Late);
            var progress = 0f;
            AssetTransformSnapshot segmentStart = default;
            var hasSegmentStart = false;

            step.Append(
                DOTween
                    .To(
                        () => progress,
                        value =>
                        {
                            if (!hasSegmentStart)
                            {
                                segmentStart = proxy.Snapshot;
                                hasSegmentStart = true;
                            }

                            progress = value;
                            proxy.Snapshot = AssetTransformSnapshot.Lerp(
                                segmentStart,
                                target,
                                progress
                            );
                            MarkPathTransformDirty();
                        },
                        1f,
                        tweenDuration
                    )
                    .SetEase(easing)
            );
            sequence.Append(step);
        }

        private static bool IsInstantDuration(float duration) =>
            duration <= InstantDurationThreshold;

        private static bool TryReadAssetTransform(
            GameObjectAsset asset,
            out AssetTransformSnapshot snapshot
        )
        {
            snapshot = default;

            if (asset == null)
                return false;

            try
            {
                var transformData = asset.Transform;
                if (transformData == null)
                    return false;

                snapshot = new AssetTransformSnapshot(
                    transformData.Position,
                    transformData.Rotation,
                    transformData.Scale
                );
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ApplyAssetTransform(
            GameObjectAsset asset,
            AssetTransformSnapshot snapshot
        )
        {
            if (asset == null)
                return;

            try
            {
                var transformData = asset.Transform;
                if (transformData != null)
                {
                    transformData.Position = snapshot.Position;
                    transformData.Rotation = snapshot.RotationEuler;
                    transformData.Scale = snapshot.Scale;
                }

                asset.SetDataInput(TransformPositionKey, snapshot.Position, broadcast: true);
                asset.SetDataInput(
                    TransformRotationKey,
                    snapshot.RotationEuler,
                    broadcast: true
                );
                asset.SetDataInput(TransformScaleKey, snapshot.Scale, broadcast: true);

                if (asset is CameraAsset camera)
                {
                    camera.TeleportTo(snapshot.Position, snapshot.Rotation, broadcast: true);
                    return;
                }

                asset.BroadcastTransformOptimized();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[Play Camera Path Node68] Transform 적용 실패: " + ex.Message
                );
            }
        }

        private static string FormatTransform(AssetTransformSnapshot snapshot) =>
            "P ("
            + snapshot.Position.x.ToString("0.###")
            + ", "
            + snapshot.Position.y.ToString("0.###")
            + ", "
            + snapshot.Position.z.ToString("0.###")
            + ") · R ("
            + snapshot.RotationEuler.x.ToString("0.###")
            + ", "
            + snapshot.RotationEuler.y.ToString("0.###")
            + ", "
            + snapshot.RotationEuler.z.ToString("0.###")
            + ") · S ("
            + snapshot.Scale.x.ToString("0.###")
            + ", "
            + snapshot.Scale.y.ToString("0.###")
            + ", "
            + snapshot.Scale.z.ToString("0.###")
            + ")";

        private static void TryToggleCamera(CameraAsset camera)
        {
            if (camera == null)
                return;

            try
            {
                var core = Context.PluginManager.GetPlugin<CorePlugin>();
                core?.SetMainCamera(camera);
            }
            catch
            {
                // CorePlugin 이 없거나 전환에 실패한 경우 무시합니다.
            }
        }

        private void KillSequence(bool invokeCancelledFlow)
        {
            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill(false);
            _sequence = null;
            _pathPaused = false;

            if (_pathPlaying)
            {
                CommitCurrentPathTransform();
                FinishPlayback(
                    invokeFinished: false,
                    restoreCameraControl: invokeCancelledFlow
                );
            }

            if (invokeCancelledFlow)
                InvokeFlow(nameof(OnCancelled));
        }

        private void CommitCurrentPathTransform()
        {
            if (_pathAsset == null || _pathProxy == null)
                return;

            ApplyAssetTransform(_pathAsset, _pathProxy.Snapshot);
        }

        private readonly struct AssetTransformSnapshot
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;
            public readonly Vector3 Scale;

            public AssetTransformSnapshot(Vector3 position, Vector3 rotationEuler, Vector3 scale)
            {
                Position = position;
                Rotation = Quaternion.Euler(rotationEuler);
                Scale = scale;
            }

            public Vector3 RotationEuler => Rotation.eulerAngles;

            public static AssetTransformSnapshot Lerp(
                AssetTransformSnapshot from,
                AssetTransformSnapshot to,
                float t
            )
            {
                var clamped = Mathf.Clamp01(t);
                return new AssetTransformSnapshot(
                    Vector3.Lerp(from.Position, to.Position, clamped),
                    Quaternion.Slerp(from.Rotation, to.Rotation, clamped),
                    Vector3.Lerp(from.Scale, to.Scale, clamped)
                );
            }

            private AssetTransformSnapshot(
                Vector3 position,
                Quaternion rotation,
                Vector3 scale
            )
            {
                Position = position;
                Rotation = rotation;
                Scale = scale;
            }
        }

        private sealed class AssetTransformProxy
        {
            public AssetTransformSnapshot Snapshot;

            public AssetTransformProxy(AssetTransformSnapshot snapshot)
            {
                Snapshot = snapshot;
            }

            public void CopyFrom(AssetTransformSnapshot snapshot)
            {
                Snapshot = snapshot;
            }
        }

        public sealed class CameraKeyframe68
            : StructuredData<PlayCameraPathNode68>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("Position")]
            public Vector3 Position;

            [DataInput]
            [Label("Rotation")]
            public Vector3 Rotation;

            [DataInput]
            [Label("Scale")]
            public Vector3 Scale = Vector3.one;

            [DataInput]
            [Label("Duration")]
            [FloatSlider(0f, 120f)]
            public float Duration = 1f;

            [DataInput]
            [Label("Easing")]
            public Ease Easing = Ease.OutCubic;

            public string GetHeader()
            {
                var node = Parent as PlayCameraPathNode68;
                var index = node != null ? node.GetKeyframeDisplayIndex(this) : 0;
                var prefix =
                    node != null && node.IsActiveKeyframe(this)
                        ? node._pathPaused
                            ? "⏸ "
                            : "▶ "
                        : string.Empty;
                return
                    prefix
                    + "#"
                    + index
                    + " P ("
                    + Position.x.ToString("0.##")
                    + ", "
                    + Position.y.ToString("0.##")
                    + ", "
                    + Position.z.ToString("0.##")
                    + ") · "
                    + Duration.ToString("0.##")
                    + "s";
            }
        }

        private static CameraAsset TryGetMainCamera()
        {
            try
            {
                var core = Context.PluginManager.GetPlugin<CorePlugin>();
                return core?.MainCameraAsset as CameraAsset;
            }
            catch
            {
                return null;
            }
        }

        private static CameraAsset.CameraControlMode GetCameraControlMode(CameraAsset camera)
        {
            if (camera == null)
                return CameraAsset.CameraControlMode.Orbit;

            try
            {
                return camera.ControlMode;
            }
            catch
            {
                return CameraAsset.CameraControlMode.Orbit;
            }
        }
    }
}

#endif
