using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Scenes;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// Poiyomi Base Color Dim · Max Brightness를 Potatoon Volume처럼 **전역(셰이더 GPU)** 으로 적용합니다.
    /// Throw Prop·캐릭터·프롭 구분 없이 Poiyomi 셰이더가 그릴 때 자동 반영됩니다.
    /// </summary>
    [AssetType(
        Id = "f7b1c2d3-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
        Title = "Node Poiyomi Volume_Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        CategoryOrder = -56
    )]
    public sealed class Node68PoiyomiGlobalVolumeAsset : Asset
    {
        [Section(BpToolkitUiLabels.WarudoSection)]
        [DataInput(10)]
        [Label("Volume 활성")]
        [Description(
            "켜면 아래 전역 조명 설정이 Poiyomi 셰이더에 적용됩니다. "
                + "Potatoon Volume과 같이 머티리얼을 하나씩 건드리지 않습니다."
        )]
        public bool ControlEnabled = true;

        [Section("Base Color Dim (전역)")]
        [DataInput(100)]
        [Label("Base Color Dim 적용")]
        [Description(
            "Poiyomi 베이스 색·텍스처를 전역 비율로 어둡게 합니다. "
                + "Light Setting_Node68의 Base Color Dim과 같은 목적이지만 GPU 전역 방식입니다."
        )]
        public bool ApplyBaseColorDim = true;

        [DataInput(101)]
        [Label("Base Color Dim")]
        [FloatSlider(0f, 1f)]
        [Description("1=원본, 0.65=65% 밝기, 0.5=절반. 0.2~0.65부터 시도하세요.")]
        public float BaseColorDim = 0.65f;

        [Section("Max Brightness (전역)")]
        [DataInput(200)]
        [Label("밝기 제한 (Limit Brightness)")]
        [Description(
            "Poiyomi `_LightingCap` 전역 상한. Light Setting_Node68 Max Brightness와 같은 목적이지만 GPU 전역 방식입니다."
        )]
        public bool LimitBrightness = true;

        [DataInput(201)]
        [Label("Max Brightness")]
        [FloatSlider(0f, 10f)]
        [Description(
            "Poiyomi `_LightingCap`. 직접광·간접광 상한(0~10). 0.2~0.5부터 시도하세요."
        )]
        public float MaxBrightness = 0.35f;

        [Markdown]
        [Hidden]
        public string _note =
            "Poiyomi Pro URP 셰이더에 Node68 전역 include가 패치되어 있어야 합니다. "
            + "Unity 메뉴: Node68 DevKit → Poiyomi Volume → Patch Shaders. "
            + "PotaToon 캐릭터는 Potatoon Volume, Poiyomi 프롭/Throw Prop은 이 에셋을 사용하세요.";

        [Trigger]
        [Label("지금 적용")]
        public void ApplyNow()
        {
            PushToShader();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            SetActive(true);

            Watch(nameof(ControlEnabled), OnInputChanged);
            Watch(nameof(ApplyBaseColorDim), OnInputChanged);
            Watch(nameof(BaseColorDim), OnInputChanged);
            Watch(nameof(LimitBrightness), OnInputChanged);
            Watch(nameof(MaxBrightness), OnInputChanged);

            PushToShader();
        }

        private void OnInputChanged()
        {
            PushToShader();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (ControlEnabled)
                PushToShader();
        }

        protected override void OnDestroy()
        {
            Node68PoiyomiGlobalVolumeBridge.SetFromAsset(false, false, 1f, false, 10f);
            base.OnDestroy();
        }

        private void PushToShader()
        {
            Node68PoiyomiGlobalVolumeBridge.SetFromAsset(
                ControlEnabled,
                ApplyBaseColorDim,
                BaseColorDim,
                LimitBrightness,
                MaxBrightness
            );
        }
    }
}
