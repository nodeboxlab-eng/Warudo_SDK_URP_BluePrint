using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Prop;

namespace Node68.CustomAssets
{
    /// <summary>
    /// <see cref="PoiyomiMaxBrightnessControlPropAsset"/> 전역·단일 대상 시 적용 범위.
    /// 에셋을 두 개 두고(씬 Prop / 던진) 서로 다른 Max Brightness 를 줄 때 사용합니다.
    /// </summary>
    public enum PoiyomiPropBrightnessTarget68
    {
        [Label("씬 PropAsset만")]
        ScenePropAssets = 0,

        [Label("던진 프롭만")]
        ThrownPropsOnly = 1,

        [Label("둘 다")]
        Both = 2,

        [Label("프롭별 목록")]
        PerPropList = 3,
    }

    /// <summary>머티리얼에 쓸 Poiyomi 밝기·발광 값 묶음.</summary>
    public struct PoiyomiBrightnessApplySettings
    {
        public bool LimitBrightness;
        public float MaxBrightness;
        public float MinLightBrightness;
        public bool ApplyMainBrightness;
        public float MainBrightness;
        public bool ApplyBaseColorDim;
        public float BaseColorDim;
        public bool LimitEmissionStrength;
        public float EmissionStrengthMax;
        public bool ApplyForceLightColor;
        public Color ForcedLightColor;
        public bool ApplyUnlitIntensity;
        public float UnlitIntensity;
        public bool ApplyDepthRimWidth;
        public float DepthRimWidth;

        public static PoiyomiBrightnessApplySettings FromAsset(PoiyomiMaxBrightnessControlBase asset) =>
            new PoiyomiBrightnessApplySettings
            {
                LimitBrightness = asset.LimitBrightness,
                MaxBrightness = asset.MaxBrightness,
                MinLightBrightness = asset.MinLightBrightness,
                ApplyMainBrightness = asset.ApplyMainBrightness,
                MainBrightness = asset.MainBrightness,
                ApplyBaseColorDim = asset.ApplyBaseColorDim,
                BaseColorDim = asset.BaseColorDim,
                LimitEmissionStrength = asset.LimitEmissionStrength,
                EmissionStrengthMax = asset.EmissionStrengthMax,
                ApplyForceLightColor = asset.ApplyForceLightColor,
                ForcedLightColor = asset.ForcedLightColor,
                ApplyUnlitIntensity = asset.ApplyUnlitIntensity,
                UnlitIntensity = asset.UnlitIntensity,
                ApplyDepthRimWidth = asset.ApplyDepthRimWidth,
                DepthRimWidth = asset.DepthRimWidth,
            };
    }

    /// <summary>
    /// Poiyomi Light Data의 Max Brightness(<c>_LightingCap</c>) 등 밝기 상한을 머티리얼에 적용합니다.
    /// PotaToon Volume의 Max Toon Brightness와 유사한 역할입니다(머티리얼 단위).
    /// </summary>
    public abstract class PoiyomiMaxBrightnessControlBase : Asset
    {
        private static readonly int LightingCapEnabledProp = Shader.PropertyToID(
            "_LightingCapEnabled"
        );
        private static readonly int LightingCapProp = Shader.PropertyToID("_LightingCap");
        private static readonly int LightingMinLightBrightnessProp = Shader.PropertyToID(
            "_LightingMinLightBrightness"
        );
        private static readonly int MainColorAdjustToggleProp = Shader.PropertyToID(
            "_MainColorAdjustToggle"
        );
        private static readonly int MainBrightnessProp = Shader.PropertyToID("_MainBrightness");
        private static readonly int EmissionStrengthProp = Shader.PropertyToID("_EmissionStrength");
        private static readonly int EmissionStrength1Prop = Shader.PropertyToID("_EmissionStrength1");
        private static readonly int EmissionStrength2Prop = Shader.PropertyToID("_EmissionStrength2");
        private static readonly int EmissionStrength3Prop = Shader.PropertyToID("_EmissionStrength3");
        private static readonly int ColorProp = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorProp = Shader.PropertyToID("_EmissionColor");
        private static readonly int EmissionColor1Prop = Shader.PropertyToID("_EmissionColor1");
        private static readonly int EmissionColor2Prop = Shader.PropertyToID("_EmissionColor2");
        private static readonly int EmissionColor3Prop = Shader.PropertyToID("_EmissionColor3");
        private static readonly int LightingForceColorEnabledProp = Shader.PropertyToID(
            "_LightingForceColorEnabled"
        );
        private static readonly int LightingForcedColorProp = Shader.PropertyToID(
            "_LightingForcedColor"
        );
        private static readonly int LightingForcedColorThemeIndexProp = Shader.PropertyToID(
            "_LightingForcedColorThemeIndex"
        );
        private static readonly int UnlitIntensityProp = Shader.PropertyToID("_Unlit_Intensity");
        private static readonly int EnableDepthRimLightingProp = Shader.PropertyToID(
            "_EnableDepthRimLighting"
        );
        private static readonly int DepthRimWidthProp = Shader.PropertyToID("_DepthRimWidth");

        internal const string ColorGradingHdrKeyword = "COLOR_GRADING_HDR";
        internal const string DepthRimKeyword = "_POI_DEPTH_RIMLIGHT";

        private readonly Dictionary<int, Color> _baseColorCache = new();

        [Section(CustomAssetsUiLabels.WarudoSection)]
        [DataInput(30)]
        [Label("에셋 활성")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "끄면 이 에셋이 머티리얼에 아무 값도 쓰지 않습니다.")]
        public bool ControlEnabled = true;

        [DataInput(50)]
        [Label("SkinnedMeshRenderer")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "지정하면 해당 스킨드 메시 렌더러만 대상입니다.")]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput(51)]
        [Label("MeshRenderer")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "스킨드 메시 렌더러가 비어 있을 때만 사용합니다.")]
        public MeshRenderer StaticMeshRenderer;

        [DataInput(52)]
        [Label("메시 키 (스킨/메시)")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "비어 있으면 대상(캐릭터/프롭)의 모든 SkinnedMesh·MeshRenderer를 순회합니다. "
                + "전역 모드에서는 각 캐릭터/프롭에서 같은 키를 찾습니다. "
                + "값이 있으면 해당 키의 스킨드 메시를 먼저 찾고, 없으면 MeshRenderer를 찾습니다."
        )]
        [HiddenIf(nameof(HideMeshKeyAutocomplete))]
        [AutoComplete(nameof(CollectMeshKeysForAutocomplete))]
        public string TargetSkinnedMeshKey = "";

        /// <summary><see cref="PoiyomiMaxBrightnessControlPropAsset"/> 프롭별 목록 모드일 때 에셋 공통 슬라이더 숨김.</summary>
        protected virtual bool HidePerPropListMode() => false;

        protected bool HideShareDevOnlyFields() => CustomAssetsFlavorEmbedded.ShareBuild;

        protected bool HideAssetLevelLightingInputs() =>
            HidePerPropListMode();

        [DataInput(53)]
        [Label("매 프레임 유지 (LateUpdate)")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 매 LateUpdate마다 다시 적용합니다. 다른 시스템(애니메이션·프리셋 등)이 머티리얼을 덮어써도 이 값이 유지되도록 합니다. "
                + "씬 PropAsset 은 Warudo MeshUpdater가 머티리얼을 되돌리므로 **프롭 Light Control** 은 활성화 시 PostLateUpdate에서 자동 재적용됩니다."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool MaintainEveryFrame = true;

        /// <summary>
        /// <see cref="PropAsset"/> MeshUpdater가 LateUpdate 이후 머티리얼을 되돌리므로
        /// 프롭 전용 에셋은 PostLateUpdate에서 적용합니다.
        /// </summary>
        protected virtual bool ApplyAfterPropMeshUpdater => false;

        [DataInput(54)]
        [Label("인스턴스 머티리얼에 쓰기")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 `Renderer.materials`(실제로 그려지는 인스턴스)를 수정합니다. Warudo에서 권장합니다. "
                + "끄면 `sharedMaterials`만 수정합니다."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool WriteInstanceMaterials = true;

        [Section("Light Data / Base Pass")]
        [DataInput(100)]
        [Label("밝기 제한 (Limit Brightness)")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi `_LightingCapEnabled`. 켜면 Max Brightness 상한을 적용합니다.")]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool LimitBrightness = true;

        [DataInput(101)]
        [Label("Max Brightness")]
        [FloatSlider(0f, 10f)]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "Poiyomi `_LightingCap`. ★가장 중요★ 직접광·간접광 상한(0~10). 0.2~0.5부터 시도하세요."
        )]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public float MaxBrightness = 0.35f;

        [DataInput(102)]
        [Label("Min Brightness")]
        [FloatSlider(0f, 1f)]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "Poiyomi `_LightingMinLightBrightness`. 라이팅 최소 밝기(0~1)."
        )]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public float MinLightBrightness;

        [Section("Color Adjust (추가 어둡게)")]
        [DataInput(120)]
        [Label("Brightness 적용")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi Color Adjust `_MainBrightness`. 발광 0 이후에도 밝으면 켜세요.")]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool ApplyMainBrightness = true;

        [DataInput(121)]
        [Label("Brightness")]
        [FloatSlider(-1f, 0f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi `_MainBrightness`. -1에 가까울수록 더 어두움. -0.5 ~ -0.8 추천.")]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public float MainBrightness = -0.5f;

        [DataInput(122)]
        [Label(CustomAssetsFlavorEmbedded.ShareBuild ? "밝기 조절" : "Base Color Dim 적용")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi `_Color` RGB를 원본 대비 비율로 줄입니다. 텍스처 전체를 어둡게.")]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool ApplyBaseColorDim;

        [DataInput(123)]
        [Label(CustomAssetsFlavorEmbedded.ShareBuild ? "밝기 조절" : "Base Color Dim")]
        [FloatSlider(0f, 1f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "1=원본, 0.5=절반 밝기, 0.2=매우 어두움.")]
        [HiddenIf(nameof(HideAssetLevelBaseColorDimSlider))]
        public float BaseColorDim = 0.65f;

        [Section("Emission (발광 억제)")]
        [DataInput(130)]
        [Label("Emission Strength 제한")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "날개·악세 등 발광(_EmissionStrength*)이 밝을 때 켜세요. 0이면 Strength·Color 모두 OFF. "
                + "애니·Poiyomi·캐릭터 MeshUpdater가 LateUpdate 이후 값을 덮어쓰므로, 켜면 **프레임 말(EndOfFrame)** 에 재적용됩니다. "
                + "Max보다 이미 낮은 Strength면 변화가 없습니다 — 더 줄이려면 Max를 **0~1** 로 낮추세요."
        )]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool LimitEmissionStrength;

        [DataInput(131)]
        [Label("Emission Strength Max")]
        [FloatSlider(0f, 20f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi `_EmissionStrength` 계열 상한. 0=완전 OFF.")]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public float EmissionStrengthMax;

        [Section("Lighting / Force Light Color")]
        [DataInput(132)]
        [Label("Force Light Color 적용")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "Poiyomi `_LightingForceColorEnabled`. 켜면 직접광 색을 `_LightingForcedColor`로 고정합니다. "
                + "테마 드롭다운은 **Off(0)** 로 맞춥니다."
        )]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool ApplyForceLightColor = true;

        [DataInput(133)]
        [Label("Forced Color")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Poiyomi `_LightingForcedColor`. 인스펙터 Forced Color 와 동일.")]
        [HiddenIf(nameof(HideForceLightColorFields))]
        public Color ForcedLightColor = Color.white;

        [DataInput(134)]
        [Label("Unlit Intensity 적용")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "Poiyomi `_Unlit_Intensity` (Lighting Color Mode = Unlit 일 때). "
                + "켜면 매 적용 시 아래 값으로 맞춥니다."
        )]
        [HiddenIf(nameof(HideAssetLevelLightingInputs))]
        public bool ApplyUnlitIntensity = true;

        [DataInput(135)]
        [Label("Unlit Intensity")]
        [FloatSlider(0.001f, 4f)]
        [HiddenIf(nameof(HideUnlitIntensityValue))]
        public float UnlitIntensity = 4f;

        protected bool HideForceLightColorFields() =>
            HideAssetLevelLightingInputs() || !ApplyForceLightColor;

        protected bool HideUnlitIntensityValue() =>
            HideAssetLevelLightingInputs() || !ApplyUnlitIntensity;

        [Section("Depth Rim Lighting")]
        [DataInput(136)]
        [Label("Depth Rim Width 적용")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "Poiyomi `_DepthRimWidth` (0~1). 켜면 Depth Rim을 켜고(`_EnableDepthRimLighting`) Width를 맞춥니다. "
                + "Lock 시 Depth Rim이 꺼져 있으면 셰이더에 Rim 코드가 없을 수 있습니다."
        )]
        [HiddenIf(nameof(HideDepthRimLightingInputs))]
        [SectionHiddenIf(nameof(HideShareDevOnlyFields))]
        public bool ApplyDepthRimWidth;

        [DataInput(137)]
        [Label("Depth Rim Width")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideDepthRimWidthValue))]
        public float DepthRimWidth = 0.04f;

        protected bool HideDepthRimWidthValue() =>
            HideDepthRimLightingInputs() || !ApplyDepthRimWidth;

        protected bool HideDepthRimLightingInputs() =>
            HideAssetLevelLightingInputs() || HideShareDevOnlyFields();

        protected bool HideAssetLevelBaseColorDimSlider() =>
            HideAssetLevelLightingInputs()
            || (CustomAssetsFlavorEmbedded.ShareBuild && !ApplyBaseColorDim);

        protected abstract void WatchMeshParentAsset();

        protected abstract bool IsGlobalMode();

        protected abstract IEnumerable<(Dictionary<string, SkinnedMeshRenderer> Smrs, Dictionary<string, MeshRenderer> Mrs)> EnumerateMeshParents();

        protected abstract object GetMeshAutocompleteParentRef();

        protected abstract string GetParentRequiredAutocompleteMessage();

        protected virtual bool TryCollectGlobalMeshKeys(List<string> keys) => false;

        private AutoCompleteList _acCachedList;
        private object _acCacheMeshParentRef;
        private Dictionary<string, SkinnedMeshRenderer> _acCacheSmrsDictRef;
        private Dictionary<string, MeshRenderer> _acCacheMrsDictRef;

        protected virtual bool HideMeshKeyAutocomplete() =>
            SkinnedMesh != null || StaticMeshRenderer != null;

        public async UniTask<AutoCompleteList> CollectMeshKeysForAutocomplete()
        {
            await UniTask.CompletedTask;

            if (IsGlobalMode())
            {
                var globalKeys = new List<string>();
                if (!TryCollectGlobalMeshKeys(globalKeys))
                    return AutoCompleteList.Message("씬에 대상이 없습니다.");

                if (globalKeys.Count == 0)
                    return AutoCompleteList.Message("스킨드 메시·MeshRenderer가 없습니다.");

                globalKeys.Sort(StringComparer.OrdinalIgnoreCase);
                var globalEntries = new List<AutoCompleteEntry>(globalKeys.Count);
                foreach (var key in globalKeys)
                    globalEntries.Add(new AutoCompleteEntry { label = key, value = key });
                return globalEntries.ToAutoCompleteList();
            }

            if (GetMeshAutocompleteParentRef() == null)
                return AutoCompleteList.Message(GetParentRequiredAutocompleteMessage());

            var seen = new HashSet<string>(StringComparer.Ordinal);
            var keysBuffer = new List<string>();
            foreach (var parent in EnumerateMeshParents())
                CollectMeshKeysFromParent(parent.Smrs, parent.Mrs, seen, keysBuffer);

            if (keysBuffer.Count == 0)
                return AutoCompleteList.Message("스킨드 메시·MeshRenderer가 없습니다.");

            var firstParent = GetMeshAutocompleteParentRef();
            var firstSmrs = default(Dictionary<string, SkinnedMeshRenderer>);
            var firstMrs = default(Dictionary<string, MeshRenderer>);
            foreach (var parent in EnumerateMeshParents())
            {
                firstSmrs = parent.Smrs;
                firstMrs = parent.Mrs;
                break;
            }

            if (
                _acCachedList != null
                && ReferenceEquals(_acCacheMeshParentRef, firstParent)
                && ReferenceEquals(_acCacheSmrsDictRef, firstSmrs)
                && ReferenceEquals(_acCacheMrsDictRef, firstMrs)
            )
                return _acCachedList;

            keysBuffer.Sort(StringComparer.OrdinalIgnoreCase);

            var entries = new List<AutoCompleteEntry>(keysBuffer.Count);
            foreach (var key in keysBuffer)
                entries.Add(new AutoCompleteEntry { label = key, value = key });

            _acCachedList = entries.ToAutoCompleteList();
            _acCacheMeshParentRef = firstParent;
            _acCacheSmrsDictRef = firstSmrs;
            _acCacheMrsDictRef = firstMrs;
            return _acCachedList;
        }

        [Trigger]
        [Label("지금 적용")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public void ApplyNow()
        {
            ApplyOnce();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            SetActive(true);

            Watch(nameof(ControlEnabled), OnInputChanged);
            Watch(nameof(SkinnedMesh), OnInputChanged);
            Watch(nameof(StaticMeshRenderer), OnInputChanged);
            WatchMeshParentAsset();
            Watch(nameof(TargetSkinnedMeshKey), OnInputChanged);
            Watch(nameof(MaintainEveryFrame), OnInputChanged);
            Watch(nameof(WriteInstanceMaterials), OnInputChanged);

            Watch(nameof(LimitBrightness), OnInputChanged);
            Watch(nameof(MaxBrightness), OnInputChanged);
            Watch(nameof(MinLightBrightness), OnInputChanged);
            Watch(nameof(ApplyMainBrightness), OnInputChanged);
            Watch(nameof(MainBrightness), OnInputChanged);
            Watch(nameof(ApplyBaseColorDim), OnInputChanged);
            Watch(nameof(BaseColorDim), OnInputChanged);
            Watch(nameof(LimitEmissionStrength), OnInputChanged);
            Watch(nameof(EmissionStrengthMax), OnInputChanged);
            Watch(nameof(ApplyForceLightColor), OnInputChanged);
            Watch(nameof(ForcedLightColor), OnInputChanged);
            Watch(nameof(ApplyUnlitIntensity), OnInputChanged);
            Watch(nameof(UnlitIntensity), OnInputChanged);
            Watch(nameof(ApplyDepthRimWidth), OnInputChanged);
            Watch(nameof(DepthRimWidth), OnInputChanged);

            ApplyOnce();
        }

        protected void OnInputChanged()
        {
            ApplyOnce();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (ApplyAfterPropMeshUpdater)
                return;
            if (MaintainEveryFrame && ControlEnabled)
                ApplyOnce(applyEmission: !DeferEmissionToEndOfFrame());
        }

        public override void OnPostLateUpdate()
        {
            base.OnPostLateUpdate();
            if (!ControlEnabled)
                return;

            if (ApplyAfterPropMeshUpdater)
            {
                if (MaintainEveryFrame)
                    ApplyOnce(applyEmission: !DeferEmissionToEndOfFrame());
                return;
            }

            // 캐릭터·파티클: MeshUpdater/애니가 PostLateUpdate에서 머티리얼을 되돌릴 수 있음 → 발광은 EndOfFrame
            if (MaintainEveryFrame && !DeferEmissionToEndOfFrame())
                ApplyOnce();
        }

        public override void OnEndOfFrame()
        {
            base.OnEndOfFrame();
            if (!ControlEnabled || !UsesEmissionReapplyThisFrame())
                return;

            ApplyOnce(applyLighting: false, applyEmission: true);
        }

        protected virtual bool DeferEmissionToEndOfFrame() => LimitEmissionStrength;

        protected virtual bool UsesEmissionReapplyThisFrame() => DeferEmissionToEndOfFrame();

        protected virtual IEnumerable<(
            IEnumerable<Renderer> renderers,
            PoiyomiBrightnessApplySettings settings
        )> EnumerateApplyPasses()
        {
            yield return (ResolveTargetRenderers(), PoiyomiBrightnessApplySettings.FromAsset(this));
        }

        private void ApplyOnce(bool applyLighting = true, bool applyEmission = true)
        {
            if (!ControlEnabled)
                return;

            if (!applyLighting && !applyEmission)
                return;

            foreach (var (renderers, settings) in EnumerateApplyPasses())
            {
                foreach (var r in renderers)
                {
                    if (r == null)
                        continue;

                    ApplyToRenderer(r, settings, applyLighting, applyEmission);
                }
            }
        }

        private void ApplyToRenderer(
            Renderer r,
            PoiyomiBrightnessApplySettings settings,
            bool applyLighting,
            bool applyEmission
        )
        {
            if (WriteInstanceMaterials)
            {
                var inst = r.materials;
                if (inst == null)
                    return;
                for (var i = 0; i < inst.Length; i++)
                {
                    var m = inst[i];
                    if (m != null)
                        TryApplyPoiyomiMaxBrightnessBlock(m, settings, applyLighting, applyEmission);
                }

                r.materials = inst;
            }
            else
            {
                var mats = r.sharedMaterials;
                if (mats == null)
                    return;
                for (var i = 0; i < mats.Length; i++)
                {
                    var m = mats[i];
                    if (m != null)
                        TryApplyPoiyomiMaxBrightnessBlock(m, settings, applyLighting, applyEmission);
                }
            }
        }

        private void TryApplyPoiyomiMaxBrightnessBlock(
            Material m,
            PoiyomiBrightnessApplySettings settings,
            bool applyLighting = true,
            bool applyEmission = true
        )
        {
            if (!IsPoiyomiMaterial(m))
                return;

            if (applyLighting)
            {
                if (m.HasProperty(LightingCapEnabledProp))
                    m.SetFloat(LightingCapEnabledProp, settings.LimitBrightness ? 1f : 0f);
                if (m.HasProperty(LightingCapProp))
                    m.SetFloat(
                        LightingCapProp,
                        Mathf.Clamp(settings.MaxBrightness, 0f, 10f)
                    );

                if (m.HasProperty(LightingMinLightBrightnessProp))
                    m.SetFloat(
                        LightingMinLightBrightnessProp,
                        Mathf.Clamp01(settings.MinLightBrightness)
                    );

                if (settings.ApplyMainBrightness && m.HasProperty(MainBrightnessProp))
                {
                    if (m.HasProperty(MainColorAdjustToggleProp))
                        m.SetFloat(MainColorAdjustToggleProp, 1f);
                    m.EnableKeyword(ColorGradingHdrKeyword);
                    m.SetFloat(
                        MainBrightnessProp,
                        Mathf.Clamp(settings.MainBrightness, -1f, 2f)
                    );
                }

                TryApplyBaseColorDim(m, settings);
                TryApplyForceLightColor(m, settings);
                TryApplyUnlitIntensity(m, settings);
                TryApplyDepthRimWidth(m, settings);
            }

            if (applyEmission && settings.LimitEmissionStrength)
            {
                var cap = Mathf.Clamp(settings.EmissionStrengthMax, 0f, 20f);
                SetEmissionStrengthCap(m, EmissionStrengthProp, cap);
                SetEmissionStrengthCap(m, EmissionStrength1Prop, cap);
                SetEmissionStrengthCap(m, EmissionStrength2Prop, cap);
                SetEmissionStrengthCap(m, EmissionStrength3Prop, cap);
                if (cap <= 0.001f)
                    ZeroEmissionColors(m);
            }
        }

        private void TryApplyBaseColorDim(Material m, PoiyomiBrightnessApplySettings settings)
        {
            if (!m.HasProperty(ColorProp))
                return;

            var id = m.GetInstanceID();
            if (!_baseColorCache.TryGetValue(id, out var original))
            {
                original = m.GetColor(ColorProp);
                _baseColorCache[id] = original;
            }

            if (!settings.ApplyBaseColorDim)
            {
                m.SetColor(ColorProp, original);
                return;
            }

            var dim = Mathf.Clamp01(settings.BaseColorDim);
            var dimmed = original;
            dimmed.r *= dim;
            dimmed.g *= dim;
            dimmed.b *= dim;
            m.SetColor(ColorProp, dimmed);
        }

        private static void TryApplyForceLightColor(
            Material m,
            PoiyomiBrightnessApplySettings settings
        )
        {
            if (!m.HasProperty(LightingForceColorEnabledProp))
                return;

            if (!settings.ApplyForceLightColor)
                return;

            m.SetFloat(LightingForceColorEnabledProp, 1f);
            if (m.HasProperty(LightingForcedColorThemeIndexProp))
                m.SetFloat(LightingForcedColorThemeIndexProp, 0f);
            if (m.HasProperty(LightingForcedColorProp))
                m.SetColor(LightingForcedColorProp, settings.ForcedLightColor);
        }

        private static void TryApplyUnlitIntensity(
            Material m,
            PoiyomiBrightnessApplySettings settings
        )
        {
            if (!settings.ApplyUnlitIntensity || !m.HasProperty(UnlitIntensityProp))
                return;

            m.SetFloat(
                UnlitIntensityProp,
                Mathf.Clamp(settings.UnlitIntensity, 0.001f, 4f)
            );
        }

        private static void TryApplyDepthRimWidth(
            Material m,
            PoiyomiBrightnessApplySettings settings
        )
        {
            if (!settings.ApplyDepthRimWidth || !m.HasProperty(DepthRimWidthProp))
                return;

            if (m.HasProperty(EnableDepthRimLightingProp))
            {
                m.SetFloat(EnableDepthRimLightingProp, 1f);
                m.EnableKeyword(DepthRimKeyword);
            }

            m.SetFloat(DepthRimWidthProp, Mathf.Clamp01(settings.DepthRimWidth));
        }

        private static void ZeroEmissionColors(Material m)
        {
            SetColorIfBlack(m, EmissionColorProp);
            SetColorIfBlack(m, EmissionColor1Prop);
            SetColorIfBlack(m, EmissionColor2Prop);
            SetColorIfBlack(m, EmissionColor3Prop);
        }

        private static void SetColorIfBlack(Material m, int propId)
        {
            if (m.HasProperty(propId))
                m.SetColor(propId, Color.black);
        }

        private static bool IsPoiyomiMaterial(Material m) =>
            m.HasProperty(LightingCapProp)
            || m.HasProperty(MainBrightnessProp)
            || m.HasProperty(EmissionStrengthProp);

        private static void SetEmissionStrengthCap(Material m, int propId, float cap)
        {
            if (!m.HasProperty(propId))
                return;
            var current = m.GetFloat(propId);
            m.SetFloat(propId, cap <= 0.001f ? 0f : Mathf.Min(current, cap));
        }

        protected virtual IEnumerable<Renderer> ResolveTargetRenderers() =>
            ResolveMeshRenderers();

        private IEnumerable<Renderer> ResolveMeshRenderers()
        {
            if (SkinnedMesh != null)
            {
                yield return SkinnedMesh;
                yield break;
            }

            if (StaticMeshRenderer != null)
            {
                yield return StaticMeshRenderer;
                yield break;
            }

            var hasParent = false;
            foreach (var parent in EnumerateMeshParents())
            {
                hasParent = true;
                foreach (
                    var r in ResolveRenderersForParent(
                        parent.Smrs,
                        parent.Mrs,
                        TargetSkinnedMeshKey
                    )
                )
                    yield return r;
            }

            if (!hasParent && !IsGlobalMode())
                yield break;
        }

        protected static IEnumerable<Renderer> ResolveRenderersForParent(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            Dictionary<string, MeshRenderer> mrs,
            string meshKey
        )
        {
            if (!string.IsNullOrWhiteSpace(meshKey))
            {
                var key = meshKey.Trim();
                if (smrs != null && smrs.TryGetValue(key, out var smr) && smr != null)
                {
                    yield return smr;
                    yield break;
                }

                if (mrs != null && mrs.TryGetValue(key, out var mr) && mr != null)
                {
                    yield return mr;
                    yield break;
                }

                yield break;
            }

            if (smrs != null)
            {
                foreach (var kv in smrs)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }

            if (mrs != null)
            {
                foreach (var kv in mrs)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }
        }

        protected static void CollectMeshKeysFromParent(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            Dictionary<string, MeshRenderer> mrs,
            HashSet<string> seen,
            List<string> keysBuffer
        )
        {
            if (smrs != null)
            {
                foreach (var kv in smrs)
                {
                    if (kv.Value != null && seen.Add(kv.Key))
                        keysBuffer.Add(kv.Key);
                }
            }

            if (mrs != null)
            {
                foreach (var kv in mrs)
                {
                    if (kv.Value != null && seen.Add(kv.Key))
                        keysBuffer.Add(kv.Key);
                }
            }
        }

        protected static IEnumerable<ParticleSystemRenderer> CollectParticleRenderersFromGameObject(
            GameObject root
        )
        {
            if (root == null)
                yield break;

            var renderers = root.GetComponentsInChildren<ParticleSystemRenderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var psr = renderers[i];
                if (psr != null)
                    yield return psr;
            }
        }

        protected HashSet<Transform> CollectCharacterRoots()
        {
            var roots = new HashSet<Transform>();
            if (Scene == null)
                return roots;

            foreach (var character in Scene.GetAssets<CharacterAsset>())
            {
                if (character?.GameObject != null)
                    roots.Add(character.GameObject.transform);
            }

            return roots;
        }

        protected HashSet<Transform> CollectPropAssetRoots()
        {
            var roots = new HashSet<Transform>();
            if (Scene == null)
                return roots;

            foreach (var prop in Scene.GetAssets<PropAsset>())
            {
                if (prop?.GameObject != null)
                    roots.Add(prop.GameObject.transform);
            }

            return roots;
        }

        protected static bool IsTransformUnderAnyRoot(Transform t, ICollection<Transform> roots)
        {
            if (t == null || roots == null || roots.Count == 0)
                return false;

            foreach (var root in roots)
            {
                if (root == null)
                    continue;
                if (t == root || t.IsChildOf(root))
                    return true;
            }

            return false;
        }

        protected IEnumerable<Renderer> EnumerateMeshRenderersOutsideAssetHierarchies(
            bool excludePropAssetHierarchies
        )
        {
            var characterRoots = CollectCharacterRoots();
            var propRoots = excludePropAssetHierarchies ? CollectPropAssetRoots() : null;

            var smrs = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
            for (var i = 0; i < smrs.Length; i++)
            {
                var smr = smrs[i];
                if (smr == null)
                    continue;
                if (IsTransformUnderAnyRoot(smr.transform, characterRoots))
                    continue;
                if (excludePropAssetHierarchies && IsTransformUnderAnyRoot(smr.transform, propRoots))
                    continue;
                yield return smr;
            }

            var mrs = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
            for (var i = 0; i < mrs.Length; i++)
            {
                var mr = mrs[i];
                if (mr == null)
                    continue;
                if (IsTransformUnderAnyRoot(mr.transform, characterRoots))
                    continue;
                if (excludePropAssetHierarchies && IsTransformUnderAnyRoot(mr.transform, propRoots))
                    continue;
                yield return mr;
            }
        }

        protected IEnumerable<ParticleSystemRenderer> EnumerateParticleRenderersOutsideAssetHierarchies()
        {
            var characterRoots = CollectCharacterRoots();
            var propRoots = CollectPropAssetRoots();
            var psrs = UnityEngine.Object.FindObjectsOfType<ParticleSystemRenderer>();
            for (var i = 0; i < psrs.Length; i++)
            {
                var psr = psrs[i];
                if (psr == null)
                    continue;
                if (IsTransformUnderAnyRoot(psr.transform, characterRoots))
                    continue;
                if (IsTransformUnderAnyRoot(psr.transform, propRoots))
                    continue;
                yield return psr;
            }
        }
    }

#if false // Light Setting_Node68 (캐릭터) — 프롭만 사용 중
    [AssetType(
        Id = "c4e8f1a2-6b3d-4f9e-a1c7-2d5e8b0f3a61",
        Title = "Light Setting_Node68 (캐릭터)",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.LightControlShare
            : CustomAssetsNode68Categories.LightControlDev,
        CategoryOrder = -59
    )]
    public sealed class PoiyomiMaxBrightnessControlAsset : PoiyomiMaxBrightnessControlBase
    {
        [Section(CustomAssetsUiLabels.WarudoSection)]
        [DataInput(38)]
        [Label("씬 전체 캐릭터")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 씬에 스폰된 **모든 캐릭터**에 적용합니다(Potatoon Volume처럼 전역). "
                + "끄면 아래 캐릭터 하나만 대상입니다."
        )]
        public bool ApplyGlobally;

        [DataInput(39)]
        [Label("캐릭터")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "「씬 전체 캐릭터」가 꺼져 있을 때만 사용합니다.")]
        [Section("대상")]
        [HiddenIf(nameof(HideSingleTargetInInspector))]
        public CharacterAsset Character;

        protected bool HideSingleTargetInInspector() =>
            ApplyGlobally;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(ApplyGlobally), OnInputChanged);
            Watch(nameof(Character), OnInputChanged);
        }

        protected override bool IsGlobalMode() => ApplyGlobally;

        protected override IEnumerable<(Dictionary<string, SkinnedMeshRenderer>, Dictionary<string, MeshRenderer>)> EnumerateMeshParents()
        {
            if (ApplyGlobally)
            {
                if (Scene == null)
                    yield break;

                foreach (var character in Scene.GetAssets<CharacterAsset>())
                {
                    if (character == null)
                        continue;
                    yield return (character.SkinnedMeshRenderers, character.MeshRenderers);
                }

                yield break;
            }

            if (Character != null)
                yield return (Character.SkinnedMeshRenderers, Character.MeshRenderers);
        }

        protected override bool TryCollectGlobalMeshKeys(List<string> keys)
        {
            if (!ApplyGlobally || Scene == null)
                return false;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            var found = false;
            foreach (var character in Scene.GetAssets<CharacterAsset>())
            {
                if (character == null)
                    continue;
                found = true;
                CollectMeshKeysFromParent(
                    character.SkinnedMeshRenderers,
                    character.MeshRenderers,
                    seen,
                    keys
                );
            }

            return found;
        }

        protected override object GetMeshAutocompleteParentRef() =>
            ApplyGlobally ? Scene : Character;

        protected override string GetParentRequiredAutocompleteMessage() =>
            ApplyGlobally ? "씬에 캐릭터가 없습니다." : "캐릭터 에셋을 먼저 선택하세요.";
    }
#endif

    [AssetType(
        Id = "d5f9a2b3-7c4e-5a0f-b2d8-3e6f9c1a4b72",
        Title = "Light Setting_Node68 (프롭)",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.LightControlShare
            : CustomAssetsNode68Categories.LightControlDev,
        CategoryOrder = -58
    )]
    public sealed class PoiyomiMaxBrightnessControlPropAsset : PoiyomiMaxBrightnessControlBase
    {
        [Section(CustomAssetsUiLabels.WarudoSection)]
        [DataInput(36)]
        [Label("대상 범위")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "**프롭별 목록** · 아래 표에 Prop마다 Max/Emission 등을 각각 지정(10개 서로 다를 때). "
                + "**씬 PropAsset만** · 전역 또는 프롭 1개. "
                + "**던진 프롭만** · Throw 클론만(별도 에셋 권장). "
                + "**둘 다** · 씬 Prop + 던진. "
                + "목록 모드와 **씬 전체 프롭**은 동시에 쓰지 마세요."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public PoiyomiPropBrightnessTarget68 TargetScope = PoiyomiPropBrightnessTarget68.Both;

        [Section("프롭별 밝기")]
        [DataInput(34)]
        [Label("프롭별 설정")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild
                ? "프롭 선택·줄별 활성·밝기 조절만 바꿀 수 있습니다. 그 외 밝기·발광 등은 제작자가 미리 넣어 둔 값이 적용됩니다."
                : "대상 범위가 **프롭별 목록**일 때만 사용합니다. Prop마다 Max Brightness·Emission 등을 다르게 둡니다. "
                    + "같은 Prop이 두 줄이면 **위쪽만** 적용됩니다."
        )]
        [HiddenIf(nameof(HidePropOverrideList))]
        public PropBrightnessOverrideEntry[] PropOverrides = Array.Empty<PropBrightnessOverrideEntry>();

        [DataInput(38)]
        [Label("씬 전체 프롭")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 씬의 **모든 PropAsset**(대상 범위에 따라) 또는 던진 프롭 전역 스캔에 적용합니다. "
                + "끄면 아래 「프롭」 하나만(범위가 씬 PropAsset·둘 다일 때). **프롭별 목록**과 함께 쓰지 마세요."
        )]
        [HiddenIf(nameof(HideLegacyPropTargeting))]
        public bool ApplyGlobally;

        [DataInput(39)]
        [Label("프롭")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "「씬 전체 프롭」이 꺼져 있고 대상 범위가 **던진 프롭만**·**프롭별 목록**이 아닐 때 사용합니다.")]
        [Section("대상")]
        [SectionHiddenIf(nameof(HideShareDevOnlyFields))]
        [HiddenIf(nameof(HideSingleTargetInInspector))]
        public PropAsset Prop;

        protected bool HidePropOverrideList() =>
            TargetScope != PoiyomiPropBrightnessTarget68.PerPropList;

        protected bool HideLegacyPropTargeting() =>
            TargetScope == PoiyomiPropBrightnessTarget68.PerPropList;

        protected override bool HidePerPropListMode() =>
            TargetScope == PoiyomiPropBrightnessTarget68.PerPropList;

        protected bool HideSingleTargetInInspector() =>
            ApplyGlobally
            || TargetScope == PoiyomiPropBrightnessTarget68.ThrownPropsOnly
            || TargetScope == PoiyomiPropBrightnessTarget68.PerPropList;

        private bool IncludeScenePropTargets() =>
            TargetScope != PoiyomiPropBrightnessTarget68.ThrownPropsOnly
            && TargetScope != PoiyomiPropBrightnessTarget68.PerPropList;

        private bool IncludeThrownPropTargets() =>
            TargetScope != PoiyomiPropBrightnessTarget68.ScenePropAssets;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(TargetScope), OnInputChanged);
            Watch(nameof(PropOverrides), OnInputChanged);
            Watch(nameof(ApplyGlobally), OnInputChanged);
            Watch(nameof(Prop), OnInputChanged);
        }

        protected override bool IsGlobalMode() =>
            ApplyGlobally && TargetScope != PoiyomiPropBrightnessTarget68.PerPropList;

        protected override bool DeferEmissionToEndOfFrame()
        {
            if (TargetScope != PoiyomiPropBrightnessTarget68.PerPropList)
                return base.DeferEmissionToEndOfFrame();

            if (PropOverrides == null)
                return false;

            for (var i = 0; i < PropOverrides.Length; i++)
            {
                var entry = PropOverrides[i];
                if (entry != null && entry.Enabled && entry.LimitEmissionStrength)
                    return true;
            }

            return false;
        }

        protected override IEnumerable<(
            IEnumerable<Renderer> renderers,
            PoiyomiBrightnessApplySettings settings
        )> EnumerateApplyPasses()
        {
            if (TargetScope != PoiyomiPropBrightnessTarget68.PerPropList)
            {
                foreach (var pass in base.EnumerateApplyPasses())
                    yield return pass;
                yield break;
            }

            if (PropOverrides == null || PropOverrides.Length == 0)
                yield break;

            var seenProps = new HashSet<PropAsset>();
            for (var i = 0; i < PropOverrides.Length; i++)
            {
                var entry = PropOverrides[i];
                if (entry == null || !entry.Enabled || entry.Prop == null)
                    continue;

                if (!seenProps.Add(entry.Prop))
                    continue;

                var batch = new List<Renderer>();
                foreach (var r in EnumerateRenderersForOverrideEntry(entry))
                {
                    if (r != null)
                        batch.Add(r);
                }

                if (batch.Count == 0)
                    continue;

                yield return (batch, entry.ToApplySettings());
            }
        }

        protected override IEnumerable<(Dictionary<string, SkinnedMeshRenderer>, Dictionary<string, MeshRenderer>)> EnumerateMeshParents()
        {
            if (!IncludeScenePropTargets())
                yield break;

            if (ApplyGlobally)
            {
                if (Scene == null)
                    yield break;

                foreach (var prop in Scene.GetAssets<PropAsset>())
                {
                    if (prop == null)
                        continue;
                    yield return (prop.SkinnedMeshRenderers, prop.MeshRenderers);
                }

                yield break;
            }

            if (Prop != null)
                yield return (Prop.SkinnedMeshRenderers, Prop.MeshRenderers);
        }

        protected override bool TryCollectGlobalMeshKeys(List<string> keys)
        {
            if (!IncludeScenePropTargets() || !ApplyGlobally || Scene == null)
                return false;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            var found = false;
            foreach (var prop in Scene.GetAssets<PropAsset>())
            {
                if (prop == null)
                    continue;
                found = true;
                CollectMeshKeysFromParent(prop.SkinnedMeshRenderers, prop.MeshRenderers, seen, keys);
            }

            return found;
        }

        protected override object GetMeshAutocompleteParentRef() => ApplyGlobally ? Scene : Prop;

        protected override string GetParentRequiredAutocompleteMessage() =>
            ApplyGlobally ? "씬에 프롭이 없습니다." : "프롭 에셋을 먼저 선택하세요.";

        protected override bool ApplyAfterPropMeshUpdater => true;

        protected override bool HideMeshKeyAutocomplete() =>
            HidePerPropListMode() || base.HideMeshKeyAutocomplete();

        protected override IEnumerable<Renderer> ResolveTargetRenderers()
        {
            var seen = new HashSet<int>();

            if (IncludeScenePropTargets())
            {
                foreach (var renderer in base.ResolveTargetRenderers())
                {
                    if (renderer != null && seen.Add(renderer.GetInstanceID()))
                        yield return renderer;
                }

                foreach (var renderer in EnumeratePropAssetHierarchyRenderers())
                {
                    if (renderer != null && seen.Add(renderer.GetInstanceID()))
                        yield return renderer;
                }
            }

            if (!IncludeThrownPropTargets())
                yield break;

            foreach (
                var renderer in EnumerateMeshRenderersOutsideAssetHierarchies(
                    excludePropAssetHierarchies: true
                )
            )
            {
                if (renderer != null && seen.Add(renderer.GetInstanceID()))
                    yield return renderer;
            }
        }

        /// <summary>
        /// PropAsset 딕셔너리가 비어 있거나 갱신 전일 때 GameObject 계층에서 Renderer를 수집합니다.
        /// </summary>
        private IEnumerable<Renderer> EnumeratePropAssetHierarchyRenderers()
        {
            if (!IncludeScenePropTargets())
                yield break;

            if (ApplyGlobally)
            {
                if (Scene == null)
                    yield break;

                foreach (var prop in Scene.GetAssets<PropAsset>())
                {
                    if (prop?.GameObject == null)
                        continue;
                    foreach (var renderer in CollectMeshRenderersFromGameObject(prop.GameObject))
                        yield return renderer;
                }

                yield break;
            }

            if (Prop?.GameObject == null)
                yield break;

            foreach (var renderer in CollectMeshRenderersFromGameObject(Prop.GameObject))
                yield return renderer;
        }

        private IEnumerable<Renderer> EnumerateRenderersForOverrideEntry(
            PropBrightnessOverrideEntry entry
        )
        {
            if (entry?.Prop == null)
                yield break;

            var smrs = entry.Prop.SkinnedMeshRenderers;
            var mrs = entry.Prop.MeshRenderers;
            var any = false;
            foreach (var r in ResolveRenderersForParent(smrs, mrs, entry.TargetSkinnedMeshKey))
            {
                any = true;
                yield return r;
            }

            if (any || entry.Prop.GameObject == null)
                yield break;

            foreach (var renderer in CollectMeshRenderersFromGameObject(entry.Prop.GameObject))
            {
                if (string.IsNullOrWhiteSpace(entry.TargetSkinnedMeshKey))
                {
                    yield return renderer;
                    continue;
                }

                if (
                    string.Equals(
                        renderer.name,
                        entry.TargetSkinnedMeshKey.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                    yield return renderer;
            }
        }

        private static IEnumerable<Renderer> CollectMeshRenderersFromGameObject(GameObject root)
        {
            if (root == null)
                yield break;

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null || renderer is ParticleSystemRenderer)
                    continue;
                if (renderer is SkinnedMeshRenderer or MeshRenderer)
                    yield return renderer;
            }
        }

        public sealed class PropBrightnessOverrideEntry
            : StructuredData<PoiyomiMaxBrightnessControlPropAsset>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("활성")]
            public bool Enabled = true;

            [DataInput]
            [Label("프롭")]
            public PropAsset Prop;

            [DataInput]
            [Label("메시 키 (비우면 전체)")]
            [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "해당 Prop의 SkinnedMesh/Mesh 키. 비우면 이 Prop의 모든 메시.")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public string TargetSkinnedMeshKey = "";

            [Section("Light Data")]
            [DataInput]
            [Label("밝기 제한")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool LimitBrightness = true;

            [DataInput]
            [Label("Max Brightness")]
            [FloatSlider(0f, 10f)]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public float MaxBrightness = 0.35f;

            [DataInput]
            [Label("Min Brightness")]
            [FloatSlider(0f, 1f)]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public float MinLightBrightness;

            [Section(CustomAssetsFlavorEmbedded.ShareBuild ? "밝기 조절" : "Color Adjust")]
            [DataInput]
            [Label("Brightness 적용")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool ApplyMainBrightness = true;

            [DataInput]
            [Label("Brightness")]
            [FloatSlider(-1f, 0f)]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public float MainBrightness = -0.5f;

            [DataInput]
            [Label(CustomAssetsFlavorEmbedded.ShareBuild ? "밝기 조절" : "Base Color Dim 적용")]
            public bool ApplyBaseColorDim;

            [DataInput]
            [Label(CustomAssetsFlavorEmbedded.ShareBuild ? "밝기 조절" : "Base Color Dim")]
            [FloatSlider(0f, 1f)]
            [HiddenIf(nameof(HideEntryBaseColorDimSlider))]
            public float BaseColorDim = 0.65f;

            [Section("Emission")]
            [DataInput]
            [Label("Emission Strength 제한")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool LimitEmissionStrength;

            [DataInput]
            [Label("Emission Strength Max")]
            [FloatSlider(0f, 20f)]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public float EmissionStrengthMax;

            [Section("Force Light Color")]
            [DataInput]
            [Label("Force Light Color 적용")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool ApplyForceLightColor = true;

            [DataInput]
            [Label("Forced Color")]
            [HiddenIf(nameof(HideEntryForceLightColor))]
            public Color ForcedLightColor = Color.white;

            [DataInput]
            [Label("Unlit Intensity 적용")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool ApplyUnlitIntensity = true;

            [DataInput]
            [Label("Unlit Intensity")]
            [FloatSlider(0.001f, 4f)]
            [HiddenIf(nameof(HideEntryUnlitIntensity))]
            public float UnlitIntensity = 4f;

            [Section("Depth Rim Lighting")]
            [DataInput]
            [Label("Depth Rim Width 적용")]
            [HiddenIf(nameof(HideEntryShareDevFields))]
            public bool ApplyDepthRimWidth;

            [DataInput]
            [Label("Depth Rim Width")]
            [FloatSlider(0f, 1f)]
            [HiddenIf(nameof(HideEntryDepthRimWidth))]
            public float DepthRimWidth = 0.04f;

            private bool HideEntryShareDevFields() => CustomAssetsFlavorEmbedded.ShareBuild;

            private bool HideEntryBaseColorDimSlider() =>
                CustomAssetsFlavorEmbedded.ShareBuild && !ApplyBaseColorDim;

            private bool HideEntryForceLightColor() =>
                HideEntryShareDevFields() || !ApplyForceLightColor;

            private bool HideEntryUnlitIntensity() =>
                HideEntryShareDevFields() || !ApplyUnlitIntensity;

            private bool HideEntryDepthRimWidth() =>
                HideEntryShareDevFields() || !ApplyDepthRimWidth;

            public PoiyomiBrightnessApplySettings ToApplySettings() =>
                new PoiyomiBrightnessApplySettings
                {
                    LimitBrightness = LimitBrightness,
                    MaxBrightness = MaxBrightness,
                    MinLightBrightness = MinLightBrightness,
                    ApplyMainBrightness = ApplyMainBrightness,
                    MainBrightness = MainBrightness,
                    ApplyBaseColorDim = ApplyBaseColorDim,
                    BaseColorDim = BaseColorDim,
                    LimitEmissionStrength = LimitEmissionStrength,
                    EmissionStrengthMax = EmissionStrengthMax,
                    ApplyForceLightColor = ApplyForceLightColor,
                    ForcedLightColor = ForcedLightColor,
                    ApplyUnlitIntensity = ApplyUnlitIntensity,
                    UnlitIntensity = UnlitIntensity,
                    ApplyDepthRimWidth = ApplyDepthRimWidth,
                    DepthRimWidth = DepthRimWidth,
                };

            public string GetHeader()
            {
                var name = Prop != null ? Prop.Name : "(프롭 없음)";
                if (CustomAssetsFlavorEmbedded.ShareBuild)
                    return name;

                var max = Mathf.Clamp(MaxBrightness, 0f, 10f);
                var em = LimitEmissionStrength
                    ? $" · Em≤{Mathf.Clamp(EmissionStrengthMax, 0f, 20f):0.##}"
                    : "";
                return $"{name} · Max {max:0.##}{em}";
            }
        }
    }

#if false // Light Setting_Node68 (파티클) — 프롭만 사용 중
    [AssetType(
        Id = "e6a0b3c4-5d6e-7f8a-9b0c-1d2e3f4a5b83",
        Title = "Light Setting_Node68 (파티클)",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.LightControlShare
            : CustomAssetsNode68Categories.LightControlDev,
        CategoryOrder = -57
    )]
    public sealed class PoiyomiMaxBrightnessControlParticleAsset : PoiyomiMaxBrightnessControlBase
    {
        [Section(CustomAssetsUiLabels.WarudoSection)]
        [DataInput(38)]
        [Label("씬 전체 파티클")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 씬의 캐릭터·프롭 계층 아래 ParticleSystemRenderer + "
                + "Throw Prop 충돌 이펙트 등 동적 파티클을 포함합니다."
        )]
        public bool ApplyGlobally = true;

        [DataInput(37)]
        [Label("동적 파티클 포함")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Throw Prop **Impact Particle** 등, 캐릭터·PropAsset 계층 밖에 생성된 파티클을 포함합니다.")]
        [HiddenIf(nameof(HideDynamicParticleOption))]
        public bool IncludeDynamicParticles = true;

        protected bool HideDynamicParticleOption() => !ApplyGlobally;

        [Section("대상")]
        [DataInput(39)]
        [Label("프롭 (파티클)")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "「씬 전체 파티클」이 꺼져 있을 때, 해당 프롭 아래 파티클 렌더러만 대상입니다.")]
        [HiddenIf(nameof(HideSingleTargetInInspector))]
        public PropAsset Prop;

        [DataInput(40)]
        [Label("ParticleSystemRenderer")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "지정하면 이 렌더러만 대상입니다(전역·프롭 설정보다 우선)."
        )]
        public ParticleSystemRenderer ParticleRenderer;

        protected bool HideSingleTargetInInspector() =>
            ApplyGlobally;

        protected override bool HideMeshKeyAutocomplete() => true;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(ApplyGlobally), OnInputChanged);
            Watch(nameof(IncludeDynamicParticles), OnInputChanged);
            Watch(nameof(Prop), OnInputChanged);
            Watch(nameof(ParticleRenderer), OnInputChanged);
        }

        protected override bool IsGlobalMode() => ApplyGlobally;

        protected override IEnumerable<(Dictionary<string, SkinnedMeshRenderer>, Dictionary<string, MeshRenderer>)> EnumerateMeshParents()
        {
            yield break;
        }

        protected override object GetMeshAutocompleteParentRef() => null;

        protected override string GetParentRequiredAutocompleteMessage() =>
            "파티클 에셋은 메시 키를 사용하지 않습니다.";

        protected override IEnumerable<Renderer> ResolveTargetRenderers()
        {
            if (ParticleRenderer != null)
            {
                yield return ParticleRenderer;
                yield break;
            }

            if (ApplyGlobally)
            {
                if (Scene == null)
                    yield break;

                var seen = new HashSet<int>();
                foreach (var prop in Scene.GetAssets<PropAsset>())
                {
                    if (prop?.GameObject == null)
                        continue;
                    foreach (var psr in CollectParticleRenderersFromGameObject(prop.GameObject))
                    {
                        if (seen.Add(psr.GetInstanceID()))
                            yield return psr;
                    }
                }

                foreach (var character in Scene.GetAssets<CharacterAsset>())
                {
                    if (character?.GameObject == null)
                        continue;
                    foreach (var psr in CollectParticleRenderersFromGameObject(character.GameObject))
                    {
                        if (seen.Add(psr.GetInstanceID()))
                            yield return psr;
                    }
                }

                if (IncludeDynamicParticles)
                {
                    foreach (var psr in EnumerateParticleRenderersOutsideAssetHierarchies())
                    {
                        if (seen.Add(psr.GetInstanceID()))
                            yield return psr;
                    }
                }

                yield break;
            }

            if (Prop?.GameObject != null)
            {
                foreach (var psr in CollectParticleRenderersFromGameObject(Prop.GameObject))
                    yield return psr;
            }
        }
    }
#endif
}
