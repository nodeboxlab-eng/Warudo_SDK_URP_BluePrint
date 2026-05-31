using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Scenes;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "ed49f598-947a-44d8-9b41-499b3966c426",
        Title = "Restore Asset State Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.45f
    )]
    public sealed class RestoreAssetStateNode68 : Node
    {
        [DataInput]
        [Label("TargetGraph")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 이 노드가 있는 현재 블루프린트 Graph Variable 을 사용합니다.")]
        [AutoComplete(nameof(AutoCompleteTargetGraph))]
        [HiddenIf(nameof(HideInShareBuild))]
        public string TargetGraph;

        [DataInput]
        [Label("Asset")]
        public Asset Asset;

        [DataInput]
        [Label("StateJson")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "직접 연결된 상태 JSON 입니다. 비어 있으면 StateVariable 에서 읽습니다.")]
        public string StateJson = "";

        [DataInput]
        [Label("StateVariable")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "StateJson 이 비었을 때 읽을 String Graph Variable 이름입니다.")]
        [AutoComplete(nameof(AutoCompleteStringVariableName))]
        [HiddenIf(nameof(HideInShareBuild))]
        public string StateVariable = "AssetState";

        [DataInput]
        [Label("RestoreEnabled")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "꺼두면 Enabled 항목은 복구하지 않습니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool RestoreEnabled = false;

        [DataInput]
        [Label("RestoreAssetReferences")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "꺼두면 FocusCharacter 같은 Asset 참조 항목은 복구하지 않습니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool RestoreAssetReferences = false;

        [DataInput]
        [Label("Use Transition")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "CameraAsset 복구에만 사용하세요. Orbit/FOV/Transform 값에만 적용됩니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool UseTransition = false;

        [DataInput]
        [Label("Transition Time")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "카메라에만 사용하세요. CameraAsset 의 Orbit/FOV/Transform 값에만 적용됩니다.")]
        [FloatSlider(0f, 30f)]
        [HiddenIf(nameof(HideTransitionInputs))]
        public float TransitionTime = 0f;

        [DataInput]
        [Label("Transition Easing")]
        [HiddenIf(nameof(HideTransitionInputs))]
        public Ease TransitionEasing = Ease.OutCubic;

        [DataInput]
        [Label("SkipMissingPaths")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool SkipMissingPaths = true;

        [DataOutput]
        [Label("RestoredCount")]
        public int OutputRestoredCount() => _lastRestoredCount;

        [DataOutput]
        [Label("Result")]
        public string OutputResult() => _lastResult;

        private int _lastRestoredCount;
        private string _lastResult = "-";
        private Sequence _sequence;

        private static readonly HashSet<string> TweenableCameraPaths = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase
        )
        {
            "OrbitRotation",
            "OrbitOffset",
            "FieldOfView",
            "Transform.Position",
            "Transform.Rotation",
        };

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();
        private bool HideTransitionInputs() => HideInShareBuild() || !UseTransition;

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Restore Asset State Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Restore Asset State Node68"
            );
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            KillSequence();
        }

        [FlowInput]
        public Continuation Enter()
        {
            RestoreState();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("On Restored")]
        public Continuation OnRestored;

        private void RestoreState()
        {
            const string title = "Restore Asset State";
            KillSequence();
            _lastRestoredCount = 0;
            _lastResult = "-";

            if (Asset == null)
            {
                _lastResult = "Asset is empty.";
                Debug.LogWarning($"[Node68/State] {title}: Asset 이 비어 있습니다.");
                Broadcast();
                InvokeFlow(nameof(OnRestored));
                return;
            }

            if (!TryResolveStateData(out var state, out var resolveMessage))
            {
                _lastResult = resolveMessage;
                Debug.LogWarning($"[Node68/State] {title}: {resolveMessage}");
                Broadcast();
                InvokeFlow(nameof(OnRestored));
                return;
            }

            var options = new Node68StateApplyOptions
            {
                RestoreEnabled = RestoreEnabled,
                RestoreAssetReferences = RestoreAssetReferences,
                SkipMissingPaths = SkipMissingPaths,
                Broadcast = true,
            };

            var useCameraTransition = ShouldUseCameraTransition();
            if (!ApplyImmediateEntries(state, options, useCameraTransition))
                return;

            if (useCameraTransition && TryBuildCameraTweenSequence(state, out var sequence))
            {
                _sequence = sequence;
                _sequence.OnComplete(FinishSuccess);
                _sequence.OnKill(() => _sequence = null);
                _sequence.Play();
                return;
            }

            if (useCameraTransition)
                ApplyTweenableCameraEntriesImmediate(state);

            FinishSuccess();
        }

        private bool ApplyImmediateEntries(
            Node68StateData state,
            Node68StateApplyOptions options,
            bool skipTweenableCameraPaths
        )
        {
            const string title = "Restore Asset State";

            foreach (var entry in state.Entries ?? new List<Node68StateEntryData>())
            {
                var path = entry?.Path?.Trim();
                if (
                    skipTweenableCameraPaths
                    && !string.IsNullOrWhiteSpace(path)
                    && TweenableCameraPaths.Contains(path)
                )
                    continue;

                if (Node68StateValueCodec.TryApplyEntry(Asset, entry, options, out var message))
                {
                    if (message == null)
                        _lastRestoredCount++;
                    continue;
                }

                if (!SkipMissingPaths)
                {
                    _lastResult = message;
                    Debug.LogWarning($"[Node68/State] {title}: {message}");
                    Broadcast();
                    InvokeFlow(nameof(OnRestored));
                    return false;
                }

                Debug.LogWarning($"[Node68/State] {title}: {message} 건너뜁니다.");
            }

            return true;
        }

        private void FinishSuccess()
        {
            _lastResult = $"Restored {_lastRestoredCount} entries.";
            Broadcast();
            InvokeFlow(nameof(OnRestored));
        }

        private bool ShouldUseCameraTransition()
        {
            return UseTransition
                && Asset is CameraAsset
                && Mathf.Max(0f, TransitionTime) > 1e-5f;
        }

        private bool TryBuildCameraTweenSequence(Node68StateData state, out Sequence sequence)
        {
            sequence = DOTween.Sequence().SetUpdate(UpdateType.Late);
            var duration = Mathf.Max(0f, TransitionTime);
            var camera = (CameraAsset)Asset;
            var hasTween = false;

            if (TryGetValue<Vector2>(state, "OrbitRotation", out var orbitRotation))
            {
                hasTween = true;
                _lastRestoredCount++;
                sequence.Join(
                    DOTween
                        .To(() => camera.OrbitRotation, v => ApplyOrbitRotation(camera, v), orbitRotation, duration)
                        .SetEase(TransitionEasing)
                );
            }

            if (TryGetValue<Vector3>(state, "OrbitOffset", out var orbitOffset))
            {
                hasTween = true;
                _lastRestoredCount++;
                sequence.Join(
                    DOTween
                        .To(() => camera.OrbitOffset, v => ApplyOrbitOffset(camera, v), orbitOffset, duration)
                        .SetEase(TransitionEasing)
                );
            }

            if (TryGetValue<float>(state, "FieldOfView", out var fov))
            {
                hasTween = true;
                _lastRestoredCount++;
                sequence.Join(
                    DOTween
                        .To(() => camera.FieldOfView, v => ApplyFieldOfView(camera, v), fov, duration)
                        .SetEase(TransitionEasing)
                );
            }

            if (camera is GameObjectAsset gameObjectAsset && gameObjectAsset.Transform != null)
            {
                if (TryGetValue<Vector3>(state, "Transform.Position", out var position))
                {
                    hasTween = true;
                    _lastRestoredCount++;
                    sequence.Join(
                        DOTween
                            .To(
                                () => gameObjectAsset.Transform.Position,
                                v => ApplyTransformPosition(gameObjectAsset, v),
                                position,
                                duration
                            )
                            .SetEase(TransitionEasing)
                    );
                }

                if (TryGetValue<Vector3>(state, "Transform.Rotation", out var rotation))
                {
                    hasTween = true;
                    _lastRestoredCount++;
                    sequence.Join(
                        DOTween
                            .To(
                                () => gameObjectAsset.Transform.Rotation,
                                v => ApplyTransformRotation(gameObjectAsset, v),
                                rotation,
                                duration
                            )
                            .SetEase(TransitionEasing)
                    );
                }
            }

            if (hasTween)
                return true;

            sequence.Kill(false);
            sequence = null;
            return false;
        }

        private void ApplyTweenableCameraEntriesImmediate(Node68StateData state)
        {
            var camera = Asset as CameraAsset;
            if (camera == null)
                return;

            if (TryGetValue<Vector2>(state, "OrbitRotation", out var orbitRotation))
            {
                ApplyOrbitRotation(camera, orbitRotation);
                _lastRestoredCount++;
            }

            if (TryGetValue<Vector3>(state, "OrbitOffset", out var orbitOffset))
            {
                ApplyOrbitOffset(camera, orbitOffset);
                _lastRestoredCount++;
            }

            if (TryGetValue<float>(state, "FieldOfView", out var fov))
            {
                ApplyFieldOfView(camera, fov);
                _lastRestoredCount++;
            }

            if (camera is GameObjectAsset gameObjectAsset && gameObjectAsset.Transform != null)
            {
                if (TryGetValue<Vector3>(state, "Transform.Position", out var position))
                {
                    ApplyTransformPosition(gameObjectAsset, position);
                    _lastRestoredCount++;
                }

                if (TryGetValue<Vector3>(state, "Transform.Rotation", out var rotation))
                {
                    ApplyTransformRotation(gameObjectAsset, rotation);
                    _lastRestoredCount++;
                }
            }
        }

        private static void ApplyOrbitRotation(CameraAsset camera, Vector2 value)
        {
            camera.OrbitRotation = value;
            camera.SetDataInput(nameof(CameraAsset.OrbitRotation), value, broadcast: true);
        }

        private static void ApplyOrbitOffset(CameraAsset camera, Vector3 value)
        {
            camera.OrbitOffset = value;
            camera.SetDataInput(nameof(CameraAsset.OrbitOffset), value, broadcast: true);
        }

        private static void ApplyFieldOfView(CameraAsset camera, float value)
        {
            camera.FieldOfView = value;
            camera.SetDataInput(nameof(CameraAsset.FieldOfView), value, broadcast: true);
        }

        private static void ApplyTransformPosition(GameObjectAsset asset, Vector3 value)
        {
            asset.Transform.Position = value;
            asset.BroadcastDataInput(nameof(GameObjectAsset.Transform));
        }

        private static void ApplyTransformRotation(GameObjectAsset asset, Vector3 value)
        {
            asset.Transform.Rotation = value;
            asset.BroadcastDataInput(nameof(GameObjectAsset.Transform));
        }

        private bool TryGetValue<T>(Node68StateData state, string path, out T result)
        {
            result = default;
            if (!Node68StateValueCodec.TryFindEntry(state, path, out var entry))
                return false;

            if (!Node68StateValueCodec.TryDeserializeValue(entry, out var value, out _))
                return false;

            if (value is T typed)
            {
                result = typed;
                return true;
            }

            return false;
        }

        private void KillSequence()
        {
            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill(false);
            _sequence = null;
        }

        private bool TryResolveStateData(out Node68StateData state, out string message)
        {
            state = null;
            message = "State JSON is empty or invalid.";

            if (!string.IsNullOrWhiteSpace(StateJson))
            {
                if (TryParseUsableStateData(StateJson, out state))
                    return true;

                Debug.LogWarning(
                    "[Node68/State] Restore Asset State: StateJson 이 상태값 JSON 이 아닙니다. StateVariable 을 대신 읽습니다."
                );
            }

            if (string.IsNullOrWhiteSpace(StateVariable))
                return false;

            if (!Node68GraphVariableHelper.TryGetString(
                this,
                TargetGraph,
                StateVariable,
                out var value
            ))
            {
                message = $"Graph 변수 '{StateVariable}' 를 읽을 수 없습니다.";
                return false;
            }

            if (TryParseUsableStateData(value, out state))
                return true;

            message = $"Graph 변수 '{StateVariable}' 의 상태 JSON 이 비어 있거나 올바르지 않습니다.";
            return false;
        }

        private static bool TryParseUsableStateData(string json, out Node68StateData state)
        {
            if (!Node68StateData.TryFromJson(json, out state))
                return false;

            if (state?.Entries == null || state.Entries.Count == 0)
                return false;

            foreach (var entry in state.Entries)
            {
                if (
                    entry != null
                    && !string.IsNullOrWhiteSpace(entry.Path)
                    && !string.IsNullOrWhiteSpace(entry.Type)
                    && entry.ValueJson != null
                )
                    return true;
            }

            return false;
        }

        private async UniTask<AutoCompleteList> AutoCompleteTargetGraph() =>
            await Node68GraphVariableHelper.AutoCompleteTargetGraph(this);

        private async UniTask<AutoCompleteList> AutoCompleteStringVariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.String
            );
    }
}
