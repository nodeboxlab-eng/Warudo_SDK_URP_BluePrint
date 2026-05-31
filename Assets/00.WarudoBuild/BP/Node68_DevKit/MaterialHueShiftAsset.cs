using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Node68.ToolkitMods.LightingControl;
using Node68.ToolkitMods.Node68DevKit;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Prop;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// Poiyomi Color Adjust → Main Hue Shift(<c>_MainHueShift*</c>)를 머티리얼에 적용합니다.
    /// <c>COLOR_GRADING_HDR</c> 키워드가 필요하므로 Hue Shift 사용 시 자동 활성화합니다.
    /// </summary>
    public abstract class MaterialHueShiftBase : Asset
    {
        private static readonly int MainColorAdjustToggleProp = Shader.PropertyToID(
            "_MainColorAdjustToggle"
        );
        private static readonly int MainHueShiftToggleProp = Shader.PropertyToID(
            "_MainHueShiftToggle"
        );
        private static readonly int MainHueShiftProp = Shader.PropertyToID("_MainHueShift");
        private static readonly int MainHueShiftSpeedProp = Shader.PropertyToID("_MainHueShiftSpeed");
        private static readonly int MainHueShiftColorSpaceProp = Shader.PropertyToID(
            "_MainHueShiftColorSpace"
        );
        private static readonly int MainHueShiftSelectOrShiftProp = Shader.PropertyToID(
            "_MainHueShiftSelectOrShift"
        );
        private static readonly int MainHueShiftReplaceProp = Shader.PropertyToID(
            "_MainHueShiftReplace"
        );

        internal const string ColorGradingHdrKeyword = "COLOR_GRADING_HDR";

        [Section(BpToolkitUiLabels.WarudoSection)]
        [DataInput]
        [Label("에셋 활성")]
        public bool ControlEnabled = true;

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("MeshRenderer")]
        public MeshRenderer StaticMeshRenderer;

        [DataInput]
        [Label("메시 키 (스킨/메시)")]
        [HiddenIf(nameof(HideMeshKeyAutocomplete))]
        [AutoComplete(nameof(CollectMeshKeysForAutocomplete))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [Label("매 프레임 유지 (LateUpdate)")]
        public bool MaintainEveryFrame = true;

        [DataInput]
        [Label("인스턴스 머티리얼에 쓰기")]
        public bool WriteInstanceMaterials = true;

        [Section("Color Adjust / Hue Shift")]
        [DataInput]
        [Label("Hue Shift")]
        [Description(
            "Poiyomi `_MainHueShiftToggle`. 셰이더에서 `if(_MainHueShiftToggle == 1)` 분기에 필요합니다."
        )]
        public bool HueShiftEnabled = true;

        [DataInput]
        [Label("Hue Shift 값")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_MainHueShift`.")]
        public float HueShift;

        [DataInput]
        [Label("Hue Shift Speed")]
        [Description("Poiyomi `_MainHueShiftSpeed`. 자동 회전 속도.")]
        public float HueShiftSpeed;

        [DataInput]
        [Label("Color Space")]
        [Description("Poiyomi `_MainHueShiftColorSpace`. OKLab(0) / HSV(1).")]
        public PoiHueColorSpace HueShiftColorSpace = PoiHueColorSpace.OKLab;

        [DataInput]
        [Label("Select or Shift")]
        [Description("Poiyomi `_MainHueShiftSelectOrShift`.")]
        public PoiHueSelectOrShift HueSelectOrShift = PoiHueSelectOrShift.HueShift;

        [DataInput]
        [Label("Hue Replace")]
        [Description("Poiyomi `_MainHueShiftReplace`.")]
        public bool HueReplace = true;

        protected abstract void WatchMeshParentAsset();

        protected abstract bool IsGlobalMode();

        protected abstract IEnumerable<(Dictionary<string, SkinnedMeshRenderer> Smrs, Dictionary<string, MeshRenderer> Mrs)> EnumerateMeshParents();

        protected abstract object GetMeshAutocompleteParentRef();

        protected abstract string GetParentRequiredAutocompleteMessage();

        protected virtual bool TryCollectGlobalMeshKeys(List<string> keys) => false;

        protected virtual bool ApplyAfterPropMeshUpdater => false;

        private AutoCompleteList _acCachedList;
        private object _acCacheMeshParentRef;
        private Dictionary<string, SkinnedMeshRenderer> _acCacheSmrsDictRef;
        private Dictionary<string, MeshRenderer> _acCacheMrsDictRef;

        protected bool HideMeshKeyAutocomplete() =>
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
            Dictionary<string, SkinnedMeshRenderer> firstSmrs = null;
            Dictionary<string, MeshRenderer> firstMrs = null;
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
        public void ApplyNow() => ApplyOnce();

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
            Watch(nameof(HueShiftEnabled), OnInputChanged);
            Watch(nameof(HueShift), OnInputChanged);
            Watch(nameof(HueShiftSpeed), OnInputChanged);
            Watch(nameof(HueShiftColorSpace), OnInputChanged);
            Watch(nameof(HueSelectOrShift), OnInputChanged);
            Watch(nameof(HueReplace), OnInputChanged);

            ApplyOnce();
        }

        protected void OnInputChanged() => ApplyOnce();

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (ApplyAfterPropMeshUpdater)
                return;
            if (MaintainEveryFrame && ControlEnabled)
                ApplyOnce();
        }

        public override void OnPostLateUpdate()
        {
            base.OnPostLateUpdate();
            if (!ApplyAfterPropMeshUpdater || !ControlEnabled)
                return;
            ApplyOnce();
        }

        internal void ApplyOnce()
        {
            if (!ControlEnabled)
                return;

            foreach (var r in ResolveTargetRenderers())
            {
                if (r == null)
                    continue;

                if (WriteInstanceMaterials)
                {
                    var inst = r.materials;
                    if (inst == null)
                        continue;
                    for (var i = 0; i < inst.Length; i++)
                    {
                        if (inst[i] != null)
                            TryApplyMainHueShiftBlock(inst[i]);
                    }

                    r.materials = inst;
                }
                else
                {
                    var mats = r.sharedMaterials;
                    if (mats == null)
                        continue;
                    for (var i = 0; i < mats.Length; i++)
                    {
                        if (mats[i] != null)
                            TryApplyMainHueShiftBlock(mats[i]);
                    }
                }
            }
        }

        internal void TryApplyMainHueShiftBlock(Material m) =>
            ApplyMainHueShiftToMaterial(
                m,
                HueShiftEnabled,
                HueShift,
                HueShiftSpeed,
                HueShiftColorSpace,
                HueSelectOrShift,
                HueReplace
            );

        internal static void ApplyMainHueShiftToMaterial(
            Material m,
            bool hueShiftEnabled,
            float hueShift,
            float hueShiftSpeed,
            PoiHueColorSpace colorSpace = PoiHueColorSpace.OKLab,
            PoiHueSelectOrShift selectOrShift = PoiHueSelectOrShift.HueShift,
            bool hueReplace = true
        )
        {
            if (m == null || !IsPoiyomiMaterial(m))
                return;
            if (!m.HasProperty(MainHueShiftToggleProp))
                return;

            var needsColorGrading =
                hueShiftEnabled
                || hueShift != 0f
                || !Mathf.Approximately(hueShiftSpeed, 0f);
            if (needsColorGrading && m.HasProperty(MainColorAdjustToggleProp))
            {
                m.SetFloat(MainColorAdjustToggleProp, 1f);
                m.EnableKeyword(ColorGradingHdrKeyword);
            }

            m.SetFloat(MainHueShiftToggleProp, hueShiftEnabled ? 1f : 0f);
            if (m.HasProperty(MainHueShiftProp))
                m.SetFloat(MainHueShiftProp, Mathf.Clamp01(hueShift));
            if (m.HasProperty(MainHueShiftSpeedProp))
                m.SetFloat(MainHueShiftSpeedProp, hueShiftSpeed);
            if (m.HasProperty(MainHueShiftColorSpaceProp))
                m.SetFloat(MainHueShiftColorSpaceProp, (int)colorSpace);
            if (m.HasProperty(MainHueShiftSelectOrShiftProp))
                m.SetFloat(MainHueShiftSelectOrShiftProp, (int)selectOrShift);
            if (m.HasProperty(MainHueShiftReplaceProp))
                m.SetFloat(MainHueShiftReplaceProp, hueReplace ? 1f : 0f);
        }

        private static bool IsPoiyomiMaterial(Material m) =>
            m.HasProperty(MainHueShiftToggleProp) || m.HasProperty(MainColorAdjustToggleProp);

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
                foreach (var r in ResolveRenderersForParent(parent.Smrs, parent.Mrs))
                    yield return r;
            }

            if (!hasParent && !IsGlobalMode())
                yield break;
        }

        private IEnumerable<Renderer> ResolveRenderersForParent(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            Dictionary<string, MeshRenderer> mrs
        )
        {
            if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
            {
                var key = TargetSkinnedMeshKey.Trim();
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
    }

    [AssetType(
        Id = "a3f8c1d2-4e5b-4a6c-9d0e-1f2a3b4c5d6e",
        Title = "Material Hue Shift Node68 (캐릭터)",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        CategoryOrder = -57
    )]
    public sealed class MaterialHueShiftAsset : MaterialHueShiftBase
    {
        [DataInput]
        [Label("씬 전체 캐릭터")]
        public bool ApplyGlobally;

        [DataInput]
        [Label("캐릭터")]
        [HiddenIf(nameof(HideSingleTarget))]
        public CharacterAsset Character;

        private bool HideSingleTarget() => ApplyGlobally;

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

    [AssetType(
        Id = "b4a9d2e3-5f6c-4b7d-0e1f-2a3b4c5d6e7f",
        Title = "Material Hue Shift Node68 (프롭)",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        CategoryOrder = -56
    )]
    public sealed class MaterialHueShiftPropAsset : MaterialHueShiftBase
    {
        [DataInput]
        [Label("씬 전체 프롭")]
        public bool ApplyGlobally;

        [DataInput]
        [Label("프롭")]
        [HiddenIf(nameof(HideSingleTarget))]
        public PropAsset Prop;

        private bool HideSingleTarget() => ApplyGlobally;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(ApplyGlobally), OnInputChanged);
            Watch(nameof(Prop), OnInputChanged);
        }

        protected override bool IsGlobalMode() => ApplyGlobally;

        protected override bool ApplyAfterPropMeshUpdater => true;

        protected override IEnumerable<(Dictionary<string, SkinnedMeshRenderer>, Dictionary<string, MeshRenderer>)> EnumerateMeshParents()
        {
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
            if (!ApplyGlobally || Scene == null)
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
    }

    /// <summary>
    /// 플로우에서 Poiyomi Main Hue Shift 값을 한 번 적용합니다.
    /// </summary>
    [NodeType(
        Id = "c5e0e3f4-6a7d-4c8e-1f2a-3b4c5d6e7f8a",
        Title = "Material Hue Shift Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.3f
    )]
    public sealed class MaterialHueShiftNode68 : Node
    {
        [DataInput]
        [Label("Hue Shift 에셋")]
        [Description("비우면 아래 캐릭터·수치만으로 1회 적용합니다.")]
        public MaterialHueShiftAsset HueShiftAsset;

        [DataInput]
        [Label("캐릭터")]
        [HiddenIf(nameof(HideCharacterWhenAssetSet))]
        public CharacterAsset Character;

        [DataInput]
        [Label("Hue Shift")]
        [HiddenIf(nameof(HideOverridesWhenAssetSet))]
        public bool HueShiftEnabled = true;

        [DataInput]
        [Label("Hue Shift 값")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideOverridesWhenAssetSet))]
        public float HueShift;

        [DataInput]
        [Label("Hue Shift Speed")]
        [HiddenIf(nameof(HideOverridesWhenAssetSet))]
        public float HueShiftSpeed;

        private bool HideCharacterWhenAssetSet() => HueShiftAsset != null;
        private bool HideOverridesWhenAssetSet() => HueShiftAsset != null;

        [FlowInput]
        [Label("적용")]
        public Continuation Enter()
        {
            if (HueShiftAsset != null)
            {
                HueShiftAsset.ApplyOnce();
                return Exit;
            }

            if (Character != null)
                ApplyToCharacter(Character, HueShiftEnabled, HueShift, HueShiftSpeed);

            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        private static void ApplyToCharacter(
            CharacterAsset character,
            bool enabled,
            float hue,
            float speed
        )
        {
            if (character?.SkinnedMeshRenderers != null)
            {
                foreach (var smr in character.SkinnedMeshRenderers.Values)
                    ApplyToRenderer(smr, enabled, hue, speed);
            }

            if (character?.MeshRenderers != null)
            {
                foreach (var mr in character.MeshRenderers.Values)
                    ApplyToRenderer(mr, enabled, hue, speed);
            }
        }

        private static void ApplyToRenderer(Renderer renderer, bool enabled, float hue, float speed)
        {
            if (renderer == null)
                return;

            var mats = renderer.materials;
            if (mats == null)
                return;

            for (var i = 0; i < mats.Length; i++)
            {
                if (mats[i] == null)
                    continue;
                MaterialHueShiftBase.ApplyMainHueShiftToMaterial(mats[i], enabled, hue, speed);
            }

            renderer.materials = mats;
        }
    }
}
