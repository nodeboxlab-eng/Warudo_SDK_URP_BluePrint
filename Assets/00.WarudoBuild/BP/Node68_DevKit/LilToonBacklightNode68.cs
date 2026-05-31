#if !NODE68_SHARE_BUILD

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// 캐릭터 렌더러의 머티리얼에 lilToon 백라이트 속성이 있으면 수치를 씁니다.
    /// 해당 프로퍼티가 없는 셰이더(변형)·슬롯은 건너뜁니다.
    /// 개발 빌드 전용 DevKit 노드입니다.
    /// </summary>
    [NodeType(
        Id = "c7e91a43-61b2-4f8e-9c1d-a4b03e82751d",
        Title = "LilToon Backlight Node68",
        Category = BpToolkitCategories.Toolkit,
        Width = 1.35f
    )]
    public sealed class LilToonBacklightNode68 : Node
    {
        private static readonly int UseBacklight = Shader.PropertyToID("_UseBacklight");
        private static readonly int BacklightColor = Shader.PropertyToID("_BacklightColor");
        private static readonly int BacklightMainStrength = Shader.PropertyToID("_BacklightMainStrength");
        private static readonly int BacklightNormalStrength = Shader.PropertyToID("_BacklightNormalStrength");
        private static readonly int BacklightBorder = Shader.PropertyToID("_BacklightBorder");
        private static readonly int BacklightBlur = Shader.PropertyToID("_BacklightBlur");
        private static readonly int BacklightDirectivity = Shader.PropertyToID("_BacklightDirectivity");
        private static readonly int BacklightViewStrength = Shader.PropertyToID("_BacklightViewStrength");
        private static readonly int BacklightReceiveShadow = Shader.PropertyToID("_BacklightReceiveShadow");
        private static readonly int BacklightBackfaceMask = Shader.PropertyToID("_BacklightBackfaceMask");

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        [Description("있으면 이 렌더러만 대상입니다.")]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("MeshRenderer")]
        [Description("스킨이 아닌 메시만 지정할 때. SkinnedMesh가 비었을 때만 사용됩니다.")]
        public MeshRenderer StaticMeshRenderer;

        [DataInput]
        [Label("캐릭터")]
        [Description("위 둘이 비었을 때. 스킨 메시 키로 한 개만 고르거나 비우면 모든 SMR/MR.")]
        public CharacterAsset Character;

        [DataInput]
        [Label("스킨 메시 키")]
        [Description("비우면 전체 순회.")]
        [HiddenIf(nameof(HideMeshKeyAutocomplete))]
        [AutoComplete(nameof(AutoCompleteSkinnedMeshKeys))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [Label("백라이트 켜기")]
        public bool UseBacklightToggle = true;

        [DataInput]
        [Label("백라이트 색")]
        public Color BacklightColorValue = new Color(0.85f, 0.8f, 0.7f, 1f);

        [DataInput]
        [Label("메인색 영향")]
        [FloatSlider(0f, 1f)]
        public float BacklightMainStrengthValue;

        [DataInput]
        [Label("노멀 영향")]
        [FloatSlider(0f, 1f)]
        public float BacklightNormalStrengthValue = 1f;

        [DataInput]
        [Label("보더")]
        [FloatSlider(0f, 1f)]
        public float BacklightBorderValue = 0.35f;

        [DataInput]
        [Label("블러")]
        [FloatSlider(0f, 1f)]
        public float BacklightBlurValue = 0.05f;

        [DataInput]
        [Label("지향성 (Directivity)")]
        public float BacklightDirectivityValue = 5f;

        [DataInput]
        [Label("뷰 방향 영향")]
        [FloatSlider(0f, 1f)]
        public float BacklightViewStrengthValue = 1f;

        [DataInput]
        [Label("그림자 수신")]
        public bool BacklightReceiveShadowToggle = true;

        [DataInput]
        [Label("뒷면 마스크")]
        public bool BacklightBackfaceMaskToggle = true;

        [DataInput]
        [Label("매 프레임 유지")]
        [Description("켜면 그래프가 켜진 동안 LateUpdate에서 값을 다시 씁니다.")]
        public bool MaintainEveryFrame;

        [Markdown]
        [Hidden]
        public string _note =
            "lilToon 머티리얼에 `_UseBacklight` 등이 있을 때만 적용됩니다. 공유 머티리얼(`sharedMaterials`)을 직접 수정하므로, 에셋 원본까지 바뀔 수 있습니다.";

        private bool HideMeshKeyAutocomplete() =>
            SkinnedMesh != null || StaticMeshRenderer != null;

        private AutoCompleteList _acCachedList;
        private CharacterAsset _acCacheCharacterRef;
        private Dictionary<string, SkinnedMeshRenderer> _acCacheSmrsDictRef;

        public async UniTask<AutoCompleteList> AutoCompleteSkinnedMeshKeys()
        {
            await UniTask.CompletedTask;

            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return AutoCompleteList.Message("스킨드 메시가 없습니다");

            if (_acCachedList != null
                && ReferenceEquals(_acCacheCharacterRef, Character)
                && ReferenceEquals(_acCacheSmrsDictRef, smrs))
                return _acCachedList;

            var keysBuffer = new List<string>(smrs.Count);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in smrs)
            {
                if (kv.Value == null || !seen.Add(kv.Key))
                    continue;
                keysBuffer.Add(kv.Key);
            }

            keysBuffer.Sort(System.StringComparer.OrdinalIgnoreCase);

            var entries = new List<AutoCompleteEntry>(keysBuffer.Count);
            foreach (var key in keysBuffer)
                entries.Add(new AutoCompleteEntry { label = key, value = key });

            _acCachedList = entries.ToAutoCompleteList();
            _acCacheCharacterRef = Character;
            _acCacheSmrsDictRef = smrs;
            return _acCachedList;
        }

        [FlowInput]
        public Continuation Enter()
        {
            ApplyOnce();
            return Exit;
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (MaintainEveryFrame)
                ApplyOnce();
        }

        private void ApplyOnce()
        {
            foreach (var r in ResolveRenderers())
            {
                if (r == null)
                    continue;
                var mats = r.sharedMaterials;
                if (mats == null)
                    continue;

                for (var i = 0; i < mats.Length; i++)
                {
                    var m = mats[i];
                    if (m == null || !m.HasProperty(UseBacklight))
                        continue;

                    m.SetFloat(UseBacklight, UseBacklightToggle ? 1f : 0f);

                    if (m.HasProperty(BacklightColor))
                        m.SetColor(BacklightColor, BacklightColorValue);

                    if (m.HasProperty(BacklightMainStrength))
                        m.SetFloat(BacklightMainStrength, Mathf.Clamp01(BacklightMainStrengthValue));

                    if (m.HasProperty(BacklightNormalStrength))
                        m.SetFloat(BacklightNormalStrength, Mathf.Clamp01(BacklightNormalStrengthValue));

                    if (m.HasProperty(BacklightBorder))
                        m.SetFloat(BacklightBorder, Mathf.Clamp01(BacklightBorderValue));

                    if (m.HasProperty(BacklightBlur))
                        m.SetFloat(BacklightBlur, Mathf.Clamp01(BacklightBlurValue));

                    if (m.HasProperty(BacklightDirectivity))
                        m.SetFloat(BacklightDirectivity, Mathf.Max(0f, BacklightDirectivityValue));

                    if (m.HasProperty(BacklightViewStrength))
                        m.SetFloat(BacklightViewStrength, Mathf.Clamp01(BacklightViewStrengthValue));

                    if (m.HasProperty(BacklightReceiveShadow))
                        m.SetFloat(BacklightReceiveShadow, BacklightReceiveShadowToggle ? 1f : 0f);

                    if (m.HasProperty(BacklightBackfaceMask))
                        m.SetFloat(BacklightBackfaceMask, BacklightBackfaceMaskToggle ? 1f : 0f);
                }
            }
        }

        private IEnumerable<Renderer> ResolveRenderers()
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

            if (Character == null)
                yield break;

            if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
            {
                var key = TargetSkinnedMeshKey.Trim();
                var smrs = Character.SkinnedMeshRenderers;
                if (smrs != null && smrs.TryGetValue(key, out var smr) && smr != null)
                    yield return smr;
                yield break;
            }

            var dictSm = Character.SkinnedMeshRenderers;
            if (dictSm != null)
            {
                foreach (var kv in dictSm)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }

            var dictMr = Character.MeshRenderers;
            if (dictMr != null)
            {
                foreach (var kv in dictMr)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }
        }

        [FlowOutput]
        public Continuation Exit;
    }
}

#endif
