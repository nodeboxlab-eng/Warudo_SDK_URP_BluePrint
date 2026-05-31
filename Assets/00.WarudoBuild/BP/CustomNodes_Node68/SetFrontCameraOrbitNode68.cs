using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Core.Server;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 캐릭터 Y 회전을 보정한 뒤, 코어 CAMERA_ORBIT_CHARACTER 와 동일 경로로 정면 Orbit 구도로 이동합니다.
    /// </summary>
    [NodeType(
        Id = "b7e4a1c9-3f62-4d85-9a1e-6c8f2d0b4e71",
        Title = "Set Front Camera Orbit Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareCamera
            : Node68NodeCategories.ToolkitCamera,
        Width = 1.35f
    )]
    public sealed class SetFrontCameraOrbitNode68 : Node
    {
        [DataInput]
        [Label("Camera")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "비우면 메인 카메라. 코어 Camera Orbit Character 와 동일하게 Orbit 으로 전환됩니다."
        )]
        public CameraAsset Camera;

        [DataInput]
        [Label("Character")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "비우면 Camera 의 FocusCharacter 를 사용합니다."
        )]
        public CharacterAsset Character;

        [DataInput]
        [Label("저장된 구도")]
        [Markdown(primary: true)]
        public string SavedOrbitStatus = "—";

        [DataInput]
        [Label("Transition Time")]
        [FloatSlider(0f, 120f)]
        public float TransitionTime = 1f;

        [DataInput]
        [Label("Transition Easing")]
        public Ease TransitionEasing = Ease.OutCubic;

        [DataInput]
        [Label("Front X Offset")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "Orbit X = 캐릭터 World Rotation Y + 이 값. API 또는 구도마다 다르면 파라미터로 연결합니다."
        )]
        public float FrontXOffset = -180f;

        [DataInput]
        [Label("Orbit X")]
        [HiddenIf(nameof(HideSavedOrbitDetails))]
        public float OrbitX = 180f;

        [DataInput]
        [Label("Orbit Y")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "카메라 위/아래 Orbit 각도입니다. API 또는 구도마다 다르면 파라미터로 연결합니다."
        )]
        public float OrbitY = 0.5f;

        [DataInput]
        [Label("Orbit Offset")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "카메라 거리, 높이, 좌우 위치 보정입니다. API 또는 구도마다 다르면 파라미터로 연결합니다."
        )]
        public Vector3 OrbitOffset = new Vector3(0.0120055005f, -0.294999868f, -3.84000087f);

        [DataInput]
        [Label("Rotation Source")]
        [HiddenIf(nameof(HideInShareBuild))]
        public CharacterPivotTransformTarget RotationSource =
            CharacterPivotTransformTarget.ParentSceneRoot;

        private readonly Node68CameraOrbitCharacterBridge _orbitBridge = new();

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();
        private bool HideSavedOrbitDetails() => true;

        protected override void OnCreate()
        {
            base.OnCreate();
            NormalizeSavedOrbitXFromLegacyField();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Set Front Camera Orbit Node68"
            );
            RefreshSavedOrbitStatusDisplay();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            NormalizeSavedOrbitXFromLegacyField();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Set Front Camera Orbit Node68"
            );
            RefreshSavedOrbitStatusDisplay();
        }

        protected override void OnDestroy()
        {
            _orbitBridge.Stop();
            base.OnDestroy();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!_orbitBridge.IsRunning)
                return;

            _orbitBridge.OnUpdate();

            if (_orbitBridge.IsTransitionComplete())
                FinishOrbit(invokeTransitionEnd: true);
        }

        [FlowInput]
        public Continuation Enter()
        {
            _orbitBridge.Stop();
            PlayFrontOrbit();
            return Exit;
        }

        [FlowInput]
        [Label("Stop")]
        public Continuation Stop()
        {
            _orbitBridge.Stop();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("On Transition End")]
        public Continuation OnTransitionEnd;

        [Trigger]
        [Label("Align Target With Main Camera")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "현재 메인 카메라 Orbit 구도에서 정면 보정값을 저장합니다."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public void AlignTargetWithMainCamera()
        {
            var camera = Node68CameraOrbitHelper.ResolveCamera(Camera);
            if (camera == null)
            {
                Debug.LogWarning(
                    "[Node68/Camera] Align Target With Main Camera: 메인 카메라를 찾을 수 없습니다."
                );
                return;
            }

            if (
                !Node68CameraOrbitHelper.TryCaptureFrontOrbitFromCamera(
                    camera,
                    Character,
                    RotationSource,
                    true,
                    out var orbitY,
                    out var orbitOffset,
                    out var frontXOffset
                )
            )
            {
                Debug.LogWarning(
                    "[Node68/Camera] Align Target With Main Camera: 캐릭터 회전을 읽을 수 없습니다."
                );
                return;
            }

            OrbitX = frontXOffset;
            OrbitY = camera.OrbitRotation.y;
            OrbitOffset = camera.OrbitOffset;
            FrontXOffset = frontXOffset;
            SetDataInput(nameof(OrbitX), OrbitX, broadcast: true);
            SetDataInput(nameof(OrbitY), OrbitY, broadcast: true);
            SetDataInput(nameof(OrbitOffset), OrbitOffset, broadcast: true);
            SetDataInput(nameof(FrontXOffset), FrontXOffset, broadcast: true);
            RefreshSavedOrbitStatusDisplay();

            Context.Service?.Toast(
                ToastSeverity.Success,
                "Front Orbit",
                "구도 저장됨",
                SavedOrbitStatus
            );
        }

        [Trigger]
        [Label("Align Main Camera With Target")]
        [Description(
            Node68FlavorEmbedded.ShareBuild
                ? ""
                : "저장된 정면 구도로 즉시 카메라를 맞춥니다."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public void AlignMainCameraWithTarget()
        {
            if (Graph == null)
            {
                Debug.LogWarning("[Node68/Camera] Align Main Camera With Target: Graph 가 없습니다.");
                return;
            }

            var camera = Node68CameraOrbitHelper.ResolveCamera(Camera);
            if (camera == null)
            {
                Debug.LogWarning(
                    "[Node68/Camera] Align Main Camera With Target: 메인 카메라를 찾을 수 없습니다."
                );
                return;
            }

            try
            {
                _orbitBridge.Stop();
                _orbitBridge.Start(
                    Graph,
                    camera,
                    GetTargetOrbitX(camera),
                    OrbitY,
                    OrbitOffset,
                    0f,
                    Ease.Linear
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[Node68/Camera] Align Main Camera With Target: " + ex.Message);
            }
        }

        private void PlayFrontOrbit()
        {
            const string title = "Set Front Camera Orbit";
            var camera = Node68CameraOrbitHelper.ResolveCamera(Camera);
            if (camera == null)
            {
                Debug.LogWarning($"[Node68/Camera] {title}: 메인 카메라를 찾을 수 없습니다.");
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            if (Graph == null)
            {
                Debug.LogWarning($"[Node68/Camera] {title}: Graph 가 없습니다.");
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            try
            {
                _orbitBridge.Start(
                    Graph,
                    camera,
                    GetTargetOrbitX(camera),
                    OrbitY,
                    OrbitOffset,
                    TransitionTime,
                    TransitionEasing
                );

                if (TransitionTime <= 0f || _orbitBridge.IsTransitionComplete())
                    FinishOrbit(invokeTransitionEnd: true);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[Node68/Camera] {title}: {ex.Message}");
                InvokeFlow(nameof(OnTransitionEnd));
            }
        }

        private void FinishOrbit(bool invokeTransitionEnd)
        {
            RefreshFinalOrbitTarget();
            _orbitBridge.Complete();

            if (invokeTransitionEnd)
                InvokeFlow(nameof(OnTransitionEnd));
        }

        private void RefreshFinalOrbitTarget()
        {
            var camera = Node68CameraOrbitHelper.ResolveCamera(Camera);
            if (camera == null)
                return;

            _orbitBridge.UpdateTarget(GetTargetOrbitX(camera), OrbitY, OrbitOffset);
        }

        private void RefreshSavedOrbitStatusDisplay()
        {
            SavedOrbitStatus = Node68CameraOrbitHelper.FormatSavedFrontOrbitStatus(
                FrontXOffset
            );
            SetDataInput(nameof(SavedOrbitStatus), SavedOrbitStatus, broadcast: true);
        }

        private float GetTargetOrbitX(CameraAsset camera)
        {
            var character = Character ?? camera?.FocusCharacter;
            if (
                !Node68CameraOrbitHelper.TryGetCharacterEulerY(
                    character,
                    RotationSource,
                    true,
                    out var characterEulerY
                )
            )
                return Node68CameraOrbitHelper.NormalizeOrbitX(FrontXOffset);

            return Node68CameraOrbitHelper.ComputeFrontOrbitX(characterEulerY, FrontXOffset);
        }

        private void NormalizeSavedOrbitXFromLegacyField()
        {
            if (Mathf.Approximately(FrontXOffset, -180f))
                FrontXOffset = OrbitX;

            OrbitX = FrontXOffset;
        }
    }
}
