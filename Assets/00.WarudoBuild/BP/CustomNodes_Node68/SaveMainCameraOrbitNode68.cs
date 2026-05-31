using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "c4a1f8e2-6b3d-4f90-a7c2-1e5d9b0f3a68",
        Title = "Save Main Camera Orbit Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareCamera
            : Node68NodeCategories.ToolkitCamera,
        Width = 1.35f
    )]
    public sealed class SaveMainCameraOrbitNode68 : Node
    {
        private const bool CreateVariablesIfMissing = true;

        [DataInput]
        [Label("TargetGraph")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 이 노드가 있는 현재 블루프린트 Graph Variable 을 사용합니다.")]
        [AutoComplete(nameof(AutoCompleteTargetGraph))]
        public string TargetGraph;

        [DataInput]
        [Label("Camera")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Save·CAMERA_ORBIT_CHARACTER 와 같은 카메라를 지정하세요. 비우면 메인 카메라만 사용합니다.")]
        public CameraAsset Camera;

        [DataInput]
        [Label("PreOrbitX")]
        [AutoComplete(nameof(AutoCompleteFloatVariableName))]
        public string PreOrbitX = "PreOrbitX";

        [DataInput]
        [Label("PreOrbitY")]
        [AutoComplete(nameof(AutoCompleteFloatVariableName))]
        public string PreOrbitY = "PreOrbitY";

        [DataInput]
        [Label("PreOrbitOffset")]
        [AutoComplete(nameof(AutoCompleteVector3VariableName))]
        public string PreOrbitOffset = "PreOrbitOffset";

        [DataInput]
        [Label("PreFOV")]
        [AutoComplete(nameof(AutoCompleteFloatVariableName))]
        public string PreFov = "PreFOV";

        [DataInput]
        [Label("PreControlMode")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 ControlMode 는 저장하지 않습니다.")]
        [AutoComplete(nameof(AutoCompleteStringVariableName))]
        public string PreControlMode = "PreControlMode";

        [DataInput]
        [Label("PreLookAtTarget")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 Look At 상태는 저장하지 않습니다.")]
        [AutoComplete(nameof(AutoCompleteBooleanVariableName))]
        public string PreLookAtTarget = "PreLookAtTarget";

        [DataInput]
        [Label("PreTargetCamera")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 Look At 대상 에셋 이름은 저장하지 않습니다. ApplyLookAtAfterSave 전 FocusCharacter 의 LookAtTarget 이름을 String 으로 저장합니다.")]
        [AutoComplete(nameof(AutoCompleteStringVariableName))]
        public string PreTargetCamera = "PreTargetCamera";

        [DataInput]
        [Label("ApplyLookAtAfterSave")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Yes: 저장 직후 FocusCharacter LookAt ON + Camera 를 LookAtTarget 으로 설정합니다. No: LookAt OFF.")]
        public bool ApplyLookAtAfterSave = true;

        [DataInput]
        [Label("ApplyFovAfterSave")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Yes: 저장 직후 FOV 를 AfterSaveFieldOfView 로 변경합니다.")]
        public bool ApplyFovAfterSave = true;

        [DataInput]
        [Label("AfterSaveFieldOfView")]
        [HiddenIf(nameof(HideFovAfterSaveFields))]
        public float AfterSaveFieldOfView = 16.2f;

        [DataInput]
        [Label("FovTransitionTime")]
        [HiddenIf(nameof(HideFovAfterSaveFields))]
        public float FovTransitionTime = 0.5f;

        [DataInput]
        [Label("FovTransitionEase")]
        [HiddenIf(nameof(HideFovAfterSaveFields))]
        public Ease FovTransitionEase = Ease.OutCubic;

        private Tween _fovTween;

        private bool HideFovAfterSaveFields() => !ApplyFovAfterSave;

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Save Main Camera Orbit Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitNodeMigration.MigrateSaveFields(this, serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Save Main Camera Orbit Node68"
            );
        }

        protected override void OnDestroy()
        {
            KillFovTween();
            base.OnDestroy();
        }

        [FlowInput]
        public Continuation Enter()
        {
            SaveCurrentOrbit();
            return OnSaved;
        }

        [FlowOutput]
        public Continuation OnSaved;

        private void KillFovTween()
        {
            _fovTween?.Kill(false);
            _fovTween = null;
        }

        private void SaveCurrentOrbit()
        {
            const string title = "Save Main Camera Orbit";
            if (
                Node68GraphVariableHelper.WarnMissingVariable(title, PreOrbitX)
                || Node68GraphVariableHelper.WarnMissingVariable(title, PreOrbitY)
                || Node68GraphVariableHelper.WarnMissingVariable(title, PreOrbitOffset)
                || Node68GraphVariableHelper.WarnMissingVariable(title, PreFov)
            )
                return;

            var camera = Node68CameraOrbitHelper.ResolveCamera(Camera);
            if (camera == null)
            {
                Debug.LogWarning($"[Node68/Camera] {title}: 메인 카메라를 찾을 수 없습니다.");
                return;
            }

            if (
                !Node68GraphVariableHelper.TrySetFloat(
                    this,
                    TargetGraph,
                    PreOrbitX,
                    camera.OrbitRotation.x,
                    CreateVariablesIfMissing
                )
            )
                Node68GraphVariableHelper.WarnVariableNotFound(title, PreOrbitX);

            if (
                !Node68GraphVariableHelper.TrySetFloat(
                    this,
                    TargetGraph,
                    PreOrbitY,
                    camera.OrbitRotation.y,
                    CreateVariablesIfMissing
                )
            )
                Node68GraphVariableHelper.WarnVariableNotFound(title, PreOrbitY);

            if (
                !Node68GraphVariableHelper.TrySetVector3(
                    this,
                    TargetGraph,
                    PreOrbitOffset,
                    camera.OrbitOffset,
                    CreateVariablesIfMissing
                )
            )
                Node68GraphVariableHelper.WarnVariableNotFound(title, PreOrbitOffset);

            if (
                !Node68GraphVariableHelper.TrySetFloat(
                    this,
                    TargetGraph,
                    PreFov,
                    camera.FieldOfView,
                    CreateVariablesIfMissing
                )
            )
                Node68GraphVariableHelper.WarnVariableNotFound(title, PreFov);

            if (!string.IsNullOrWhiteSpace(PreControlMode))
            {
                if (
                    !Node68GraphVariableHelper.TrySetString(
                        this,
                        TargetGraph,
                        PreControlMode,
                        Node68CameraOrbitHelper.FormatControlMode(
                            Node68CameraOrbitHelper.GetControlMode(camera)
                        ),
                        CreateVariablesIfMissing
                    )
                )
                    Node68GraphVariableHelper.WarnVariableNotFound(title, PreControlMode);
            }

            if (!string.IsNullOrWhiteSpace(PreLookAtTarget))
            {
                if (
                    !Node68GraphVariableHelper.TrySetBoolean(
                        this,
                        TargetGraph,
                        PreLookAtTarget,
                        Node68CameraOrbitHelper.GetCharacterLookAtEnabled(camera),
                        CreateVariablesIfMissing
                    )
                )
                    Node68GraphVariableHelper.WarnVariableNotFound(title, PreLookAtTarget);
            }

            if (!string.IsNullOrWhiteSpace(PreTargetCamera))
            {
                var lookAtCharacter = Node68CameraOrbitHelper.ResolveLookAtCharacter(camera, null);
                var lookAtTargetName = Node68CameraOrbitHelper.GetCharacterLookAtTargetAssetName(
                    lookAtCharacter
                );

                if (
                    !Node68GraphVariableHelper.TrySetString(
                        this,
                        TargetGraph,
                        PreTargetCamera,
                        lookAtTargetName,
                        CreateVariablesIfMissing
                    )
                )
                    Node68GraphVariableHelper.WarnVariableNotFound(title, PreTargetCamera);
            }

            ApplyAfterSaveEffects(camera);
        }

        private void ApplyAfterSaveEffects(CameraAsset camera)
        {
            var lookAtCharacter = Node68CameraOrbitHelper.ResolveLookAtCharacter(camera, null);
            if (ApplyLookAtAfterSave)
                Node68CameraOrbitHelper.ApplyCharacterLookAt(lookAtCharacter, true, camera);
            else
                Node68CameraOrbitHelper.ApplyCharacterLookAt(lookAtCharacter, false, null);

            if (!ApplyFovAfterSave)
                return;

            KillFovTween();
            var tween = Node68CameraOrbitHelper.CreateFovTransitionSequence(
                camera,
                AfterSaveFieldOfView,
                FovTransitionTime,
                FovTransitionEase
            );
            if (tween != null)
            {
                tween.OnComplete(() => _fovTween = null);
                _fovTween = tween;
            }
        }

        private async UniTask<AutoCompleteList> AutoCompleteFloatVariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.Float
            );

        private async UniTask<AutoCompleteList> AutoCompleteVector3VariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.Vector3
            );

        private async UniTask<AutoCompleteList> AutoCompleteStringVariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.String
            );

        private async UniTask<AutoCompleteList> AutoCompleteBooleanVariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.Boolean
            );

        private async UniTask<AutoCompleteList> AutoCompleteTargetGraph() =>
            await Node68GraphVariableHelper.AutoCompleteTargetGraph(this);
    }
}
