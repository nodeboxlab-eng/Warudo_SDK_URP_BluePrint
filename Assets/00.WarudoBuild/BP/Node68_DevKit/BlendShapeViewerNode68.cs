using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Persistence;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Utils;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// 확인 전용 — 선택한 스킨드 메시의 BlendShape **이름**과 현재 **weight(0~100)** 만 멀티라인 필드에 나열합니다.
    /// 그래프가 켜져 있으면 일정 간격으로만 갱신합니다(브라우즈 UI·드롭다운 렉 방지).
    /// </summary>
    [NodeType(
        Id = "e8f7a6b5-c4d3-41e9-8acb-9d716f2e5840",
        Title = "BlendShape Viewer Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.35f
    )]
    public sealed class BlendShapeViewerNode68 : Node
    {
        /// <summary>SkinnedMesh 지정 시 메시 키는 불필요하므로 숨김.</summary>
        private bool HideTargetSkinnedMeshKeyInInspector() => SkinnedMesh != null;

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        [Description("있으면 이 렌더러만 봅니다.")]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("캐릭터")]
        [Description("위가 비었을 때. 메시 키로 렌더러 고르거나 비우면 블렌드가 있는 첫 메시.")]
        public CharacterAsset Character;

        [DataInput]
        [Label("메시 키")]
        [HiddenIf(nameof(HideTargetSkinnedMeshKeyInInspector))]
        [AutoComplete(nameof(AutoCompleteSkinnedMeshKeys))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [MultilineInput]
        [Label("목록")]
        [Description(
            "이름 ↔ weight (0~100). 수백 개 쉐이프도 드랍다운 버벅임을 줄이기 위해 약 0.12초마다 새로 고칩니다."
        )]
        public string BlendShapeListText = "";

        [DataOutput]
        [Label("이름")]
        public string[] OutputBlendShapeNames()
        {
            return _cachedNames ?? Array.Empty<string>();
        }

        [DataOutput]
        [Label("값 (0~100)")]
        public float[] OutputBlendShapeWeights()
        {
            return _cachedWeights ?? Array.Empty<float>();
        }

        private string[] _cachedNames = Array.Empty<string>();
        private float[] _cachedWeights = Array.Empty<float>();
        private string _lastListTextSerialized = "";

        /// <summary>촘촘한 갱신은 멀티라인·DataOutput 브로드캐스트로 메시 키 UI가 렉 걸림.</summary>
        private const float ViewerRefreshSeconds = 0.12f;

        private float _nextViewerRebuildTime = float.NegativeInfinity;

        private CharacterAsset _acCacheCharacterRef;
        private Dictionary<string, SkinnedMeshRenderer> _acCacheSmrsDictRef;
        private AutoCompleteList _acCachedList;

        public override void OnUpdate()
        {
            base.OnUpdate();
            var now = Time.time;
            if (now < _nextViewerRebuildTime)
                return;
            _nextViewerRebuildTime = now + ViewerRefreshSeconds;
            RefreshViewer();
        }

        private void RefreshViewer()
        {
            var smr = ResolveSkinnedMeshRenderer(out var failReason);
            if (smr == null)
            {
                PushList(
                    string.IsNullOrEmpty(failReason)
                        ? "(SkinnedMeshRenderer 또는 캐릭터를 지정하세요)"
                        : $"({failReason})",
                    Array.Empty<string>(),
                    Array.Empty<float>()
                );
                return;
            }

            var mesh = smr.sharedMesh;
            if (mesh == null)
            {
                PushList(
                    "(sharedMesh 가 null 입니다)",
                    Array.Empty<string>(),
                    Array.Empty<float>()
                );
                return;
            }

            var n = mesh.blendShapeCount;
            if (n == 0)
            {
                PushList("(BlendShape 없음)", Array.Empty<string>(), Array.Empty<float>());
                return;
            }

            var names = new string[n];
            var weights = new float[n];

            var sb = new StringBuilder(capacity: n * 48);
            for (var i = 0; i < n; i++)
            {
                var shapeName = mesh.GetBlendShapeName(i);
                var w = smr.GetBlendShapeWeight(i);
                names[i] = shapeName;
                weights[i] = w;

                sb.Append(SafeOneLine(shapeName));
                sb.Append('\t');
                sb.Append(w.ToString("0.###", CultureInfo.InvariantCulture));
                sb.Append('\n');
            }

            PushList(sb.ToString().TrimEnd(), names, weights);
        }

        private void PushList(string text, string[] names, float[] weights)
        {
            var textChanged = _lastListTextSerialized != text;
            var namesChanged = !NamesSequenceEqual(_cachedNames, names);
            var weightsChanged = !WeightsSequenceApproxEqual(_cachedWeights, weights);

            _cachedNames = names;
            _cachedWeights = weights;

            if (textChanged)
            {
                _lastListTextSerialized = text;
                BlendShapeListText = text;
                BroadcastDataInput(nameof(BlendShapeListText));
            }

            if (textChanged || namesChanged || weightsChanged)
                Broadcast();
        }

        private static bool NamesSequenceEqual(string[] a, string[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            var n = a?.Length ?? 0;
            if ((b?.Length ?? 0) != n)
                return false;

            for (var i = 0; i < n; i++)
            {
                if (!string.Equals(a[i], b[i], StringComparison.Ordinal))
                    return false;
            }

            return true;
        }

        private static bool WeightsSequenceApproxEqual(float[] a, float[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            var n = a?.Length ?? 0;
            if ((b?.Length ?? 0) != n)
                return false;

            for (var i = 0; i < n; i++)
            {
                if (!Mathf.Approximately(a[i], b[i]))
                    return false;
            }

            return true;
        }

        private SkinnedMeshRenderer ResolveSkinnedMeshRenderer(out string failReason)
        {
            failReason = null;

            if (SkinnedMesh != null)
                return SkinnedMesh;

            if (Character == null)
            {
                failReason = "Character 없음.";
                return null;
            }

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
            {
                failReason = "등록된 SkinnedMesh 없음.";
                return null;
            }

            if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
            {
                if (smrs.TryGetValue(TargetSkinnedMeshKey.Trim(), out var byKey) && byKey != null)
                    return byKey;
                failReason = $"메시 키를 찾을 수 없음: {TargetSkinnedMeshKey}";
                return null;
            }

            SkinnedMeshRenderer firstWithShapes = null;
            SkinnedMeshRenderer firstAny = null;

            foreach (var kv in smrs)
            {
                var smr = kv.Value;
                if (smr == null)
                    continue;

                firstAny ??= smr;
                var m = smr.sharedMesh;
                if (m != null && m.blendShapeCount > 0)
                    firstWithShapes ??= smr;
            }

            return firstWithShapes ?? firstAny;
        }

        public async UniTask<AutoCompleteList> AutoCompleteSkinnedMeshKeys()
        {
            await UniTask.CompletedTask;

            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return AutoCompleteList.Message("스킨드 메시가 없습니다");

            if (
                _acCachedList != null
                && ReferenceEquals(_acCacheCharacterRef, Character)
                && ReferenceEquals(_acCacheSmrsDictRef, smrs)
            )
                return _acCachedList;

            var keysBuffer = new List<string>(smrs.Count);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in smrs)
            {
                if (kv.Value == null || !seen.Add(kv.Key))
                    continue;

                keysBuffer.Add(kv.Key);
            }

            keysBuffer.Sort(StringComparer.OrdinalIgnoreCase);

            var entries = new List<AutoCompleteEntry>(keysBuffer.Count);
            foreach (var key in keysBuffer)
            {
                var smrRow = smrs[key];
                var m = smrRow != null ? smrRow.sharedMesh : null;
                var bs = m != null ? m.blendShapeCount : 0;
                entries.Add(
                    new AutoCompleteEntry
                    {
                        label = bs >= 1 ? $"{key} ({bs} 블렌드)" : key,
                        value = key,
                    }
                );
            }

            var result = entries.ToAutoCompleteList();
            _acCacheCharacterRef = Character;
            _acCacheSmrsDictRef = smrs;
            _acCachedList = result;

            return result;
        }

        private static string SafeOneLine(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            return s.Replace('\r', ' ').Replace('\n', ' ');
        }
    }

    /// <summary>
    /// <see cref="CharacterAsset"/> 의 <see cref="SkinnedMeshRenderer"/> / <see cref="MeshRenderer"/> 에서
    /// <see cref="Renderer.sharedMaterials"/> 를 모아 머티리얼·셰이더 프로퍼티를 열람합니다.
    /// 프로퍼티 메타는 Unity 2021 런타임 <see cref="Shader"/> API 로 수집합니다(Editor 전용 <c>ShaderUtil</c> 미사용).
    /// </summary>
    [NodeType(
        Id = "91c3d2fd-5846-4735-9137-82d4edb6c3a9",
        Title = "Character Material Shader Probe Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.45f
    )]
    public sealed class CharacterMaterialShaderProbeNode68 : Node
    {
        private sealed class SlotEntry
        {
            public string ValueKey;
            public string DropdownLabel;
            public Renderer Renderer;
            public int Slot;
            public Material SharedMaterialRef;
            public Shader LastShaderSeen;
            public Dictionary<string, PropMeta> Props = new Dictionary<string, PropMeta>(
                StringComparer.Ordinal
            );
        }

        private readonly struct PropMeta
        {
            public readonly ShaderPropertyType UnityType;
            public readonly string Name;
            public readonly Vector2? Range;
            public readonly ShaderPropertyFlags Flags;

            public PropMeta(
                ShaderPropertyType unityType,
                string name,
                Vector2? range,
                ShaderPropertyFlags flags
            )
            {
                UnityType = unityType;
                Name = name;
                Range = range;
                Flags = flags;
            }
        }

        [DataInput]
        [Label("캐릭터")]
        [Description("스폰 후 렌더러가 채워져야 머티리얼 목록이 나타납니다.")]
        public CharacterAsset Character;

        [DataInput]
        [Label("머티리얼")]
        [AutoComplete(nameof(AutoCompleteMaterialSlots))]
        [Description("렌더러 키·슬롯·인스턴스 ID 로 구분됩니다(동일 이름 중복 방지).")]
        public string SelectedMaterialSlotKey = "";

        [DataInput]
        [Label("프로퍼티")]
        [AutoComplete(nameof(AutoCompleteShaderPropertiesFiltered))]
        [Description("Float / Range / Color / Vector / Texture 계열만.")]
        public string SelectedPropertyName = "";

        [DataInput]
        [Label("이름 검색")]
        [Description("프로퍼티 자동완성을 부분 문자열로 좁힙니다.")]
        public string PropertyNameFilter = "";

        [DataInput]
        [Label("빠른 태그")]
        [AutoComplete(nameof(AutoCompleteKeywordTags))]
        [Description("Emission / Rim / Matcap 등 키워드로 후보를 필터합니다.")]
        public string KeywordQuickTag = "";

        [DataInput]
        [MultilineInput]
        [Label("값 (노드가 갱신)")]
        [Description("읽기 전용 표시용 문자열입니다.")]
        public string DisplayCurrentValueHint = "";

        [DataInput]
        [Label("타입 표시")]
        public string DisplayPropertyKind = "";

        [DataInput]
        [Label("[편집] Float")]
        public float EditFloatTarget;

        [DataInput]
        [Label("[편집] Color")]
        public Color EditColorTarget = Color.white;

        [DataInput]
        [Label("Transition Time")]
        [Description("Apply Flow 시 DOTween 길이(초). 0이면 즉시.")]
        public float TransitionSeconds;

        [DataInput]
        [Label("Ease")]
        public Ease TransitionEase = Ease.OutQuad;

        [Markdown]
        [Hidden]
        public string InfoNote =
            "<b>주의</b> <c>sharedMaterials</c> 에 직접 씁니다. 공유 머티리얼 에셋이면 원본까지 바뀔 수 있습니다.";

        [FlowInput]
        [Label("즉시 갱신")]
        public Continuation RefreshNow()
        {
            InvalidateCaches();
            RebuildSlotsFromCharacter();
            PushDisplayStrings(immediate: true);
            return Exit;
        }

        [FlowInput]
        [Label("적용 (Float/Color)")]
        public Continuation ApplyEditedValueTween()
        {
            ApplyTweenOrImmediate();
            PushDisplayStrings(immediate: true);
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("머티리얼 InstanceId")]
        public int OutputMaterialInstanceId()
        {
            if (!TryResolveSlot(out var slot, out _))
                return 0;
            return slot.SharedMaterialRef != null ? slot.SharedMaterialRef.GetInstanceID() : 0;
        }

        [DataOutput]
        [Label("값 한 줄")]
        public string OutputValueOneLine() => BuildCompactValue();

        [DataOutput]
        [Label("Float")]
        public float OutputFloatOrZero()
        {
            if (!TryResolveSlot(out var slot, out _) || slot.SharedMaterialRef == null)
                return 0f;
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue)
                return 0f;
            if (
                meta.Value.UnityType != ShaderPropertyType.Float
                && meta.Value.UnityType != ShaderPropertyType.Range
            )
                return 0f;
            var id = Shader.PropertyToID(meta.Value.Name);
            return slot.SharedMaterialRef.HasProperty(id)
                ? slot.SharedMaterialRef.GetFloat(id)
                : 0f;
        }

        [DataOutput]
        [Label("Color")]
        public Color OutputColorOrWhite()
        {
            if (!TryResolveSlot(out var slot, out _) || slot.SharedMaterialRef == null)
                return Color.white;
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue || meta.Value.UnityType != ShaderPropertyType.Color)
                return Color.white;
            var id = Shader.PropertyToID(meta.Value.Name);
            return slot.SharedMaterialRef.HasProperty(id)
                ? slot.SharedMaterialRef.GetColor(id)
                : Color.white;
        }

        private readonly List<SlotEntry> _slots = new List<SlotEntry>(48);
        private readonly Dictionary<string, SlotEntry> _slotByKey = new Dictionary<
            string,
            SlotEntry
        >(StringComparer.Ordinal);

        private CharacterAsset _cachedCharacter;
        private int _characterLayoutStamp;

        private Tweener _propertyTween;

        private const float InspectorThrottleSec = 0.14f;
        private float _nextInspectorPushTime = float.NegativeInfinity;

        private string _lastPushedValue = "";
        private string _lastPushedKind = "";

        private AutoCompleteList _acMaterials;
        private CharacterAsset _acMatCharacter;
        private string _acMatStampKey;

        private AutoCompleteList _acProps;
        private string _acPropSlotKey;

        private AutoCompleteList _acTags;

        private string _liveMaterialsSignature = "";

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!ReferenceEquals(Character, _cachedCharacter))
                InvalidateCaches();

            DetectLiveMaterialArrayChanges();
            RebuildSlotsFromCharacter();
            DetectShaderSwaps();

            var due = Time.time >= _nextInspectorPushTime;
            if (due && DisplayDirty())
            {
                PushDisplayStrings(immediate: false);
                _nextInspectorPushTime = Time.time + InspectorThrottleSec;
            }
        }

        protected override void OnDestroy()
        {
            KillPropertyTween();
            base.OnDestroy();
        }

        private void InvalidateCaches()
        {
            _cachedCharacter = Character;
            _characterLayoutStamp = -1;
            _slots.Clear();
            _slotByKey.Clear();
            _acMaterials = null;
            _acMatCharacter = null;
            _acMatStampKey = null;
            _acProps = null;
            _acPropSlotKey = null;
            _nextInspectorPushTime = float.NegativeInfinity;
            _liveMaterialsSignature = "";
        }

        private void DetectLiveMaterialArrayChanges()
        {
            if (Character == null)
            {
                _liveMaterialsSignature = "";
                return;
            }

            var sig = BuildLiveMaterialsSignature(Character);
            if (string.Equals(sig, _liveMaterialsSignature, StringComparison.Ordinal))
                return;
            _liveMaterialsSignature = sig;
            _characterLayoutStamp = -1;
            InvalidateMaterialAutocomplete();
        }

        private static string BuildLiveMaterialsSignature(CharacterAsset ch)
        {
            var parts = new List<string>(48);
            AppendRendererMaterialRows(ch.SkinnedMeshRenderers, "SMR", parts);
            AppendRendererMaterialRows(ch.MeshRenderers, "MR", parts);
            parts.Sort(StringComparer.Ordinal);
            return string.Join('\u241F', parts);
        }

        private static void AppendRendererMaterialRows<T>(
            Dictionary<string, T> dict,
            string kind,
            List<string> sink
        )
            where T : Renderer
        {
            if (dict == null)
                return;
            foreach (var kv in dict)
            {
                var r = kv.Value;
                if (r == null)
                    continue;
                var mats = r.sharedMaterials;
                if (mats == null)
                    continue;
                for (var si = 0; si < mats.Length; si++)
                {
                    var m = mats[si];
                    sink.Add($"{kind}:{kv.Key}:{si}:{(m != null ? m.GetInstanceID() : 0)}");
                }
            }
        }

        private int ComputeLayoutStamp(CharacterAsset ch)
        {
            if (ch == null)
                return 0;
            unchecked
            {
                var h = HashCode.Combine(
                    ch.SkinnedMeshRenderers?.GetHashCode() ?? 0,
                    ch.MeshRenderers?.GetHashCode() ?? 0,
                    ch.SkinnedMeshRenderers?.Count ?? 0,
                    ch.MeshRenderers?.Count ?? 0
                );
                return h;
            }
        }

        private void RebuildSlotsFromCharacter()
        {
            if (Character == null)
            {
                if (_slots.Count > 0)
                {
                    _slots.Clear();
                    _slotByKey.Clear();
                    SanitizeSelectionAfterRebuild();
                }

                return;
            }

            var stamp = ComputeLayoutStamp(Character);
            if (
                ReferenceEquals(Character, _cachedCharacter)
                && stamp == _characterLayoutStamp
                && _slots.Count > 0
            )
                return;

            _cachedCharacter = Character;
            _characterLayoutStamp = stamp;
            _slots.Clear();
            _slotByKey.Clear();

            var nameCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            void Collect<T>(Dictionary<string, T> dict, string kind)
                where T : Renderer
            {
                if (dict == null)
                    return;
                foreach (var kv in dict)
                {
                    var r = kv.Value;
                    if (r == null)
                        continue;
                    var mats = r.sharedMaterials;
                    if (mats == null || mats.Length == 0)
                        continue;
                    for (var i = 0; i < mats.Length; i++)
                    {
                        var m = mats[i];
                        if (m == null)
                            continue;
                        nameCount.TryGetValue(m.name, out var dup);
                        nameCount[m.name] = dup + 1;
                        var suffix = dup > 0 ? $" #{dup + 1}" : "";
                        var key = $"{kind}\u241F{kv.Key}\u241F{i}\u241F{m.GetInstanceID()}";
                        var label = $"{kind} [{kv.Key}] · {i}: {SafeOneLine(m.name)}{suffix}";
                        var entry = new SlotEntry
                        {
                            ValueKey = key,
                            DropdownLabel = label,
                            Renderer = r,
                            Slot = i,
                            SharedMaterialRef = m,
                            LastShaderSeen = m.shader,
                            Props = BuildProps(m.shader),
                        };
                        _slots.Add(entry);
                        _slotByKey[key] = entry;
                    }
                }
            }

            Collect(Character.SkinnedMeshRenderers, "SMR");
            Collect(Character.MeshRenderers, "MR");

            _slots.Sort(
                (a, b) =>
                    string.Compare(
                        a.DropdownLabel,
                        b.DropdownLabel,
                        StringComparison.OrdinalIgnoreCase
                    )
            );
            SanitizeSelectionAfterRebuild();
            InvalidateMaterialAutocomplete();
        }

        private void DetectShaderSwaps()
        {
            var any = false;
            foreach (var e in _slots)
            {
                var m = e.SharedMaterialRef;
                if (m == null)
                    continue;
                if (m.shader != e.LastShaderSeen)
                {
                    e.LastShaderSeen = m.shader;
                    e.Props = BuildProps(m.shader);
                    any = true;
                }
            }

            if (any)
                InvalidatePropertyAutocomplete();
        }

        private static Dictionary<string, PropMeta> BuildProps(Shader shader)
        {
            var map = new Dictionary<string, PropMeta>(StringComparer.Ordinal);
            if (shader == null)
                return map;
            try
            {
                var n = shader.GetPropertyCount();
                for (var i = 0; i < n; i++)
                {
                    var name = shader.GetPropertyName(i);
                    if (string.IsNullOrEmpty(name))
                        continue;
                    var t = shader.GetPropertyType(i);
                    if (!IsSupportedType(t))
                        continue;
                    var flags = shader.GetPropertyFlags(i);
                    Vector2? range = null;
                    if (t == ShaderPropertyType.Range)
                    {
                        try
                        {
                            range = shader.GetPropertyRangeLimits(i);
                        }
                        catch
                        {
                            range = null;
                        }
                    }

                    map[name] = new PropMeta(t, name, range, flags);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ShaderProbe68] shader={shader.name} 스캔 실패: {ex.Message}");
            }

            return map;
        }

        private static bool IsSupportedType(ShaderPropertyType t)
        {
            switch (t)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                case ShaderPropertyType.Color:
                case ShaderPropertyType.Vector:
                case ShaderPropertyType.Texture:
                    return true;
                default:
                {
                    // SRP 변형 또는 상위 패치에서 추가된 텍스처류 enum 도 자동 허용
                    var label = t.ToString();
                    return label.IndexOf("Texture", StringComparison.Ordinal) >= 0
                        || label.IndexOf("Cube", StringComparison.Ordinal) >= 0;
                }
            }
        }

        private static bool IsTextureFamily(ShaderPropertyType t)
        {
            if (t == ShaderPropertyType.Texture)
                return true;
            var label = t.ToString();
            return label.IndexOf("Texture", StringComparison.Ordinal) >= 0
                || label.IndexOf("Cube", StringComparison.Ordinal) >= 0;
        }

        private void SanitizeSelectionAfterRebuild()
        {
            if (_slotByKey.Count == 0)
            {
                SelectedMaterialSlotKey = "";
                SelectedPropertyName = "";
                BroadcastDataInput(nameof(SelectedMaterialSlotKey));
                BroadcastDataInput(nameof(SelectedPropertyName));
                return;
            }

            if (
                string.IsNullOrEmpty(SelectedMaterialSlotKey)
                || !_slotByKey.ContainsKey(SelectedMaterialSlotKey)
            )
            {
                SelectedMaterialSlotKey = _slots[0].ValueKey;
                BroadcastDataInput(nameof(SelectedMaterialSlotKey));
            }

            if (!TryResolveSlot(out var slot, out _))
            {
                SelectedPropertyName = "";
                BroadcastDataInput(nameof(SelectedPropertyName));
                return;
            }

            if (string.IsNullOrEmpty(SelectedPropertyName))
            {
                PickFirstProperty(slot);
                return;
            }

            if (!slot.Props.ContainsKey(SelectedPropertyName))
            {
                var ignoreCase = slot.Props.Keys.FirstOrDefault(k =>
                    string.Equals(k, SelectedPropertyName, StringComparison.OrdinalIgnoreCase)
                );
                if (ignoreCase != null)
                    SelectedPropertyName = ignoreCase;
                else
                    PickFirstProperty(slot);
            }
        }

        private void PickFirstProperty(SlotEntry slot)
        {
            if (slot.Props.Count == 0)
            {
                if (SelectedPropertyName != "")
                {
                    SelectedPropertyName = "";
                    BroadcastDataInput(nameof(SelectedPropertyName));
                }

                return;
            }

            var first = slot.Props.Keys.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).First();
            if (SelectedPropertyName != first)
            {
                SelectedPropertyName = first;
                BroadcastDataInput(nameof(SelectedPropertyName));
            }
        }

        private bool TryResolveSlot(out SlotEntry slot, out Material mat)
        {
            slot = null;
            mat = null;
            if (Character == null || string.IsNullOrEmpty(SelectedMaterialSlotKey))
                return false;
            if (
                !_slotByKey.TryGetValue(SelectedMaterialSlotKey, out var e)
                || e?.SharedMaterialRef == null
            )
                return false;
            slot = e;
            mat = e.SharedMaterialRef;
            return true;
        }

        private PropMeta? GetSelectedMeta(SlotEntry slot)
        {
            if (slot == null || string.IsNullOrEmpty(SelectedPropertyName))
                return null;
            return slot.Props.TryGetValue(SelectedPropertyName, out var m) ? m : null;
        }

        private string BuildReadableValue()
        {
            if (!TryResolveSlot(out var slot, out var mat) || mat == null)
                return "(캐릭터/머티리얼 없음)";
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue)
                return "(프로퍼티 없음)";
            var id = Shader.PropertyToID(meta.Value.Name);
            if (!mat.HasProperty(id))
                return $"{FriendlyType(meta.Value.UnityType)} · HasProperty=false";

            try
            {
                if (meta.Value.UnityType is ShaderPropertyType.Float or ShaderPropertyType.Range)
                {
                    var v = mat.GetFloat(id);
                    return meta.Value.Range is Vector2 rg
                        ? $"Float [{rg.x:0.###} .. {rg.y:0.###}] → {v:0.############}"
                        : $"Float → {v:0.############}";
                }

                if (meta.Value.UnityType == ShaderPropertyType.Color)
                {
                    var c = mat.GetColor(id);
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "RGBA({0:0.###}, {1:0.###}, {2:0.###}, {3:0.###})",
                        c.r,
                        c.g,
                        c.b,
                        c.a
                    );
                }

                if (meta.Value.UnityType == ShaderPropertyType.Vector)
                {
                    var v = mat.GetVector(id);
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "Vector4({0:0.###}, {1:0.###}, {2:0.###}, {3:0.###})",
                        v.x,
                        v.y,
                        v.z,
                        v.w
                    );
                }

                if (IsTextureFamily(meta.Value.UnityType))
                {
                    var tex = mat.GetTexture(id);
                    return tex != null ? $"Texture → {tex.name}" : "Texture → (none)";
                }

                return meta.Value.UnityType.ToString();
            }
            catch (Exception ex)
            {
                return $"읽기 오류: {ex.Message}";
            }
        }

        private string BuildKindLine()
        {
            if (!TryResolveSlot(out var slot, out _))
                return "";
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue)
                return "";
            return $"{FriendlyType(meta.Value.UnityType)} · {meta.Value.Name}";
        }

        private string BuildCompactValue()
        {
            if (!TryResolveSlot(out var slot, out var mat) || mat == null)
                return "";
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue)
                return "";
            var id = Shader.PropertyToID(meta.Value.Name);
            if (!mat.HasProperty(id))
                return "";
            try
            {
                var ut = meta.Value.UnityType;
                if (ut is ShaderPropertyType.Float or ShaderPropertyType.Range)
                    return mat.GetFloat(id).ToString(CultureInfo.InvariantCulture);
                if (ut == ShaderPropertyType.Color)
                {
                    var c = mat.GetColor(id);
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "RGBA({0:0.###},{1:0.###},{2:0.###},{3:0.###})",
                        c.r,
                        c.g,
                        c.b,
                        c.a
                    );
                }

                if (ut == ShaderPropertyType.Vector)
                    return mat.GetVector(id).ToString();
                if (IsTextureFamily(ut))
                {
                    var tex = mat.GetTexture(id);
                    return tex != null ? tex.name : "";
                }

                return "";
            }
            catch
            {
                return "";
            }
        }

        private static string FriendlyType(ShaderPropertyType t) =>
            IsTextureFamily(t) && t != ShaderPropertyType.Texture
                ? $"Texture ({t})"
                : t switch
                {
                    ShaderPropertyType.Range => "Range (float)",
                    _ => t.ToString(),
                };

        private bool DisplayDirty()
        {
            var v = BuildReadableValue();
            var k = BuildKindLine();
            return v != _lastPushedValue || k != _lastPushedKind;
        }

        private void PushDisplayStrings(bool immediate)
        {
            if (!immediate && !DisplayDirty())
                return;

            var v = BuildReadableValue();
            var k = BuildKindLine();
            var changed = false;
            if (DisplayCurrentValueHint != v)
            {
                DisplayCurrentValueHint = v;
                BroadcastDataInput(nameof(DisplayCurrentValueHint));
                changed = true;
            }

            if (DisplayPropertyKind != k)
            {
                DisplayPropertyKind = k;
                BroadcastDataInput(nameof(DisplayPropertyKind));
                changed = true;
            }

            if (changed)
            {
                _lastPushedValue = v;
                _lastPushedKind = k;
                Broadcast();
            }
        }

        private void ApplyTweenOrImmediate()
        {
            if (!TryResolveSlot(out var slot, out var mat) || mat == null)
                return;
            var meta = GetSelectedMeta(slot);
            if (!meta.HasValue)
                return;
            var id = Shader.PropertyToID(meta.Value.Name);
            if (!mat.HasProperty(id))
                return;

            KillPropertyTween();
            var dur = Mathf.Max(0f, TransitionSeconds);

            switch (meta.Value.UnityType)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                {
                    var end = EditFloatTarget;
                    if (meta.Value.Range is Vector2 rg)
                    {
                        var lo = Mathf.Min(rg.x, rg.y);
                        var hi = Mathf.Max(rg.x, rg.y);
                        end = Mathf.Clamp(end, lo, hi);
                    }

                    if (dur <= 1e-5f)
                        mat.SetFloat(id, end);
                    else
                        _propertyTween = DOTween
                            .To(() => mat.GetFloat(id), x => mat.SetFloat(id, x), end, dur)
                            .SetEase(TransitionEase)
                            .SetTarget(mat);
                    break;
                }
                case ShaderPropertyType.Color:
                {
                    if (dur <= 1e-5f)
                        mat.SetColor(id, EditColorTarget);
                    else
                        _propertyTween = DOTween
                            .To(
                                () => mat.GetColor(id),
                                c => mat.SetColor(id, c),
                                EditColorTarget,
                                dur
                            )
                            .SetEase(TransitionEase)
                            .SetTarget(mat);
                    break;
                }
                default:
                    break;
            }
        }

        private void KillPropertyTween()
        {
            if (_propertyTween != null && _propertyTween.IsActive())
                _propertyTween.Kill();
            _propertyTween = null;
        }

        public async UniTask<AutoCompleteList> AutoCompleteMaterialSlots()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 지정하세요");

            RebuildSlotsFromCharacter();
            if (_slots.Count == 0)
                return AutoCompleteList.Message("머티리얼이 없습니다(스폰·렌더러 확인)");

            var stamp = $"{Character.Id}\u241F{_characterLayoutStamp}\u241F{_slots.Count}";
            if (
                _acMaterials != null
                && ReferenceEquals(_acMatCharacter, Character)
                && _acMatStampKey == stamp
            )
                return _acMaterials;

            var entries = _slots
                .Select(s => new AutoCompleteEntry { label = s.DropdownLabel, value = s.ValueKey })
                .ToList();

            _acMaterials = entries.ToAutoCompleteList();
            _acMatCharacter = Character;
            _acMatStampKey = stamp;
            return _acMaterials;
        }

        public async UniTask<AutoCompleteList> AutoCompleteShaderPropertiesFiltered()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터 필요");
            if (
                string.IsNullOrEmpty(SelectedMaterialSlotKey)
                || !_slotByKey.TryGetValue(SelectedMaterialSlotKey, out var slot)
            )
                return AutoCompleteList.Message("머티리얼 슬롯을 먼저 고르세요");

            var f = PropertyNameFilter?.Trim() ?? "";
            var tag = KeywordQuickTag?.Trim() ?? "";
            var key =
                $"{SelectedMaterialSlotKey}\u241F{f}\u241F{tag}\u241F{slot.Props.Count}\u241F{slot.LastShaderSeen?.GetInstanceID() ?? 0}";
            if (_acProps != null && _acPropSlotKey == key)
                return _acProps;

            IEnumerable<KeyValuePair<string, PropMeta>> rows = slot.Props;
            if (!string.IsNullOrEmpty(f))
                rows = rows.Where(kv => kv.Key.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!string.IsNullOrEmpty(tag) && tag != "(모두)")
                rows = rows.Where(kv => MatchQuickTag(kv.Key, tag));

            var list = rows.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
                .Select(kv => new AutoCompleteEntry
                {
                    label = $"{kv.Key} [{FriendlyType(kv.Value.UnityType)}]",
                    value = kv.Key,
                })
                .ToList();

            _acProps =
                list.Count > 0
                    ? list.ToAutoCompleteList()
                    : AutoCompleteList.Message("조건에 맞는 프로퍼티가 없습니다");
            _acPropSlotKey = key;
            return _acProps;
        }

        public async UniTask<AutoCompleteList> AutoCompleteKeywordTags()
        {
            await UniTask.CompletedTask;
            if (_acTags != null)
                return _acTags;

            var tags = new List<AutoCompleteEntry>
            {
                new AutoCompleteEntry { label = "(모두)", value = "(모두)" },
                new AutoCompleteEntry { label = "Emission / Glow", value = "_Emission" },
                new AutoCompleteEntry { label = "Rim / 림", value = "rim" },
                new AutoCompleteEntry { label = "Matcap", value = "matcap" },
                new AutoCompleteEntry { label = "Poiyomi (keyword)", value = "poiyomi" },
                new AutoCompleteEntry { label = "lilToon prefix", value = "_lil" },
                new AutoCompleteEntry { label = "Specular", value = "spec" },
                new AutoCompleteEntry { label = "Outline", value = "outline" },
            };
            _acTags = tags.ToAutoCompleteList();
            return _acTags;
        }

        private static bool MatchQuickTag(string propNameLowerSource, string tag)
        {
            var p = propNameLowerSource ?? "";
            var t = tag.Trim();
            if (string.Equals(t, "(모두)", StringComparison.Ordinal))
                return true;
            return p.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void InvalidateMaterialAutocomplete()
        {
            _acMaterials = null;
            _acMatCharacter = null;
            _acMatStampKey = null;
            InvalidatePropertyAutocomplete();
        }

        private void InvalidatePropertyAutocomplete()
        {
            _acProps = null;
            _acPropSlotKey = null;
        }

        private static string SafeOneLine(string s) =>
            string.IsNullOrEmpty(s) ? "" : s.Replace('\r', ' ').Replace('\n', ' ');
    }

    /// <summary>
    /// 캐릭터 머티리얼의 Texture 슬롯을 PNG 로보냅니다.
    /// 읽기 불가(isReadable=false) 텍스처도 RenderTexture 경유로 복사합니다.
    /// </summary>
    [NodeType(
        Id = "c4a8e2f1-9b3d-4e7c-a1f6-8d2e5b0c3a71",
        Title = "Extract Material Texture Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.45f
    )]
    public sealed class ExtractMaterialTextureNode68 : Node
    {
        private const string ExportRelativeBaseDir = "Node68TextureExport";

        private sealed class SlotEntry
        {
            public string ValueKey;
            public string DropdownLabel;
            public Renderer Renderer;
            public int Slot;
            public Material SharedMaterialRef;
            public Dictionary<string, ShaderPropertyType> TextureProps = new Dictionary<
                string,
                ShaderPropertyType
            >(StringComparer.Ordinal);
        }

        private static readonly string[] MainTextureFallbackPropertyNames =
        {
            "_MainTex",
            "_BaseMap",
            "_BaseColorMap",
            "_BaseColorTex",
        };

        /// <summary>NiloToon / Nilo 예제 쉐이더에서 자주 쓰는 Texture 슬롯 (우선 시도).</summary>
        private static readonly string[] NiloToonPriorityTexturePropertyNames =
        {
            "_BaseMap",
            "_MainTex",
            "_EmissionMap",
            "_OcclusionMap",
            "_OutlineZOffsetMaskTex",
            "_BumpMap",
            "_NormalMap",
            "_MaskMap",
            "_MetallicGlossMap",
            "_SpecGlossMap",
        };

        [DataInput]
        [Label("캐릭터")]
        [Description("스폰 후 렌더러·머티리얼이 채워져야 합니다.")]
        public CharacterAsset Character;

        [DataInput]
        [Label("머티리얼 슬롯")]
        [AutoComplete(nameof(AutoCompleteMaterialSlots))]
        [Description("「이 슬롯만」 사용 시 대상. 「전부보내기」는 모든 슬롯을 자동 처리합니다.")]
        public string SelectedMaterialSlotKey = "";

        [DataInput]
        [Hidden]
        [Label("텍스처 프로퍼티")]
        [AutoComplete(nameof(AutoCompleteTextureProperties))]
        [Description("고급: 단일 추출 시만 사용")]
        public string SelectedPropertyName = "";

        [DataInput]
        [Label("하위 폴더")]
        [Description(
            "폴더 이름만 입력 (예: MyAvatar). 전체 경로를 넣지 마세요. 비우면 시간 폴더 자동."
        )]
        public string SubfolderName = "";

        [DataInput]
        [Label("파일 접두사")]
        [Description("PNG 파일명 앞에 붙입니다.")]
        public string FilePrefix = "";

        [DataInput]
        [Label("최대 변 길이")]
        [Description("0이면 원본 해상도. 0보다 크면 긴 변을 이 값으로 축소 후 저장.")]
        public int MaxLongEdge;

        [DataInput]
        [Label("저장 후 폴더 열기")]
        [Description("PNG가 1개 이상 저장되면 파일 탐색기로 출력 폴더를 엽니다.")]
        public bool OpenFolderAfterSave = true;

        [Markdown]
        public string InfoNote =
            "**저장 위치 (Warudo URP)**\n\n"
            + "- 상대: `StreamingAssets/"
            + ExportRelativeBaseDir
            + "/{하위폴더}/`\n"
            + "- Steam 예: `Warudo/Warudo_Data/StreamingAssets/"
            + ExportRelativeBaseDir
            + "/{하위폴더}/`\n"
            + "- 실행 후 **저장 폴더** 출력에 PC 전체 경로 표시\n\n"
            + "**모든 텍스처보내기** = 캐릭터 전 머티리얼·슬롯에서 연결된 Texture를 찾아 PNG 저장 (같은 이미지는 1번만).\n"
            + "슬롯·쉐이더 종류를 고를 필요 없습니다.";

        [FlowInput]
        [Label("모든 텍스처보내기")]
        [Description(
            "캐릭터의 모든 SkinnedMesh/Mesh 렌더러·머티리얼 슬롯을 스캔해, 연결된 Texture2D·RenderTexture를 PNG로 저장합니다. 동일 텍스처는 한 번만 저장합니다."
        )]
        public Continuation ExportAllTextures()
        {
            RunExport(ExportScope.AllUniqueTexturesOnCharacter);
            return Exit;
        }

        [FlowInput]
        [Hidden]
        [Label("전부보내기 (구버전 호환)")]
        public Continuation ExportAllAssigned()
        {
            RunExport(ExportScope.AllUniqueTexturesOnCharacter);
            return Exit;
        }

        [FlowInput]
        [Hidden]
        [Label("NiloToon 전부보내기")]
        [Description("NiloToon 계열 머티리얼만 저장 (고급).")]
        public Continuation ExportNiloToonAssigned()
        {
            RunExport(ExportScope.AllAssignedNiloToonOnCharacter);
            return Exit;
        }

        [FlowInput]
        [Label("이 슬롯만")]
        [Description("선택한 머티리얼 슬롯에서 연결된 Texture만 저장합니다.")]
        public Continuation ExportSlotAssigned()
        {
            RunExport(ExportScope.AllAssignedOnSlot);
            return Exit;
        }

        [FlowInput]
        [Hidden]
        [Label("선택 프로퍼티 1장")]
        public Continuation ExportSelected()
        {
            RunExport(ExportScope.SelectedProperty);
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("저장 폴더")]
        public string OutputFolder() => _lastFolder;

        [DataOutput]
        [Label("저장 파일 수")]
        public int OutputSavedCount() => _lastSaved;

        [DataOutput]
        [Label("빈 슬롯 수")]
        public int OutputEmptySlotCount() => _lastEmptySlots;

        [DataOutput]
        [Label("실패 수")]
        public int OutputFailedCount() => _lastFailed;

        [DataOutput]
        [Label("결과")]
        public string OutputMessage() => _lastMessage;

        [DataOutput]
        [Label("경로 목록")]
        public string OutputPathsMultiline() => _lastPathsText;

        private readonly List<SlotEntry> _slots = new List<SlotEntry>(48);
        private readonly Dictionary<string, SlotEntry> _slotByKey = new Dictionary<
            string,
            SlotEntry
        >(StringComparer.Ordinal);

        private CharacterAsset _cachedCharacter;
        private int _characterLayoutStamp;

        private string _lastFolder = "";
        private int _lastSaved;
        private int _lastEmptySlots;
        private int _lastFailed;
        private int _lastUniqueTextureCount;
        private string _lastMessage = "";
        private string _lastPathsText = "";
        private string _lastFailureHint = "";

        private AutoCompleteList _acMaterials;
        private CharacterAsset _acMatCharacter;
        private string _acMatStampKey;

        private AutoCompleteList _acProps;
        private string _acPropSlotKey;

        private enum ExportScope
        {
            SelectedProperty,
            AllAssignedOnSlot,
            AllAssignedOnCharacter,
            AllUniqueTexturesOnCharacter,
            AllAssignedNiloToonOnCharacter,
        }

        private void RunExport(ExportScope scope)
        {
            _lastSaved = 0;
            _lastEmptySlots = 0;
            _lastFailed = 0;
            _lastUniqueTextureCount = 0;
            _lastFolder = "";
            _lastMessage = "";
            _lastPathsText = "";
            _lastFailureHint = "";

            if (Character == null)
            {
                _lastMessage = "(캐릭터 없음)";
                Broadcast();
                return;
            }

            RebuildSlotsFromCharacter();
            if (_slots.Count == 0)
            {
                _lastMessage = "(머티리얼 없음 — 캐릭터 스폰 후 다시 시도)";
                Broadcast();
                return;
            }

            var exportRelDir = BuildExportRelativeDirectory();
            var pdm = Context.PersistentDataManager;
            _lastFolder = pdm.GetFullPath(exportRelDir);
            EnsureExportDirectory(pdm, exportRelDir);
            var paths = new List<string>(64);
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var exportedTextureIds = new HashSet<int>();

            switch (scope)
            {
                case ExportScope.SelectedProperty:
                    if (!TryResolveSlot(out var singleSlot))
                    {
                        _lastMessage = "(머티리얼 슬롯을 고르세요)";
                        Broadcast();
                        return;
                    }

                    if (string.IsNullOrEmpty(SelectedPropertyName))
                    {
                        _lastMessage = "(텍스처 프로퍼티를 고르세요)";
                        Broadcast();
                        return;
                    }

                    TryExportOne(
                        singleSlot,
                        SelectedPropertyName,
                        exportRelDir,
                        pdm,
                        paths,
                        usedNames,
                        exportedTextureIds,
                        treatNullAsEmpty: false,
                        ref _lastSaved,
                        ref _lastEmptySlots,
                        ref _lastFailed
                    );
                    break;

                case ExportScope.AllAssignedOnSlot:
                    if (!TryResolveSlot(out var slotOnly))
                    {
                        _lastMessage = "(머티리얼 슬롯을 고르세요)";
                        Broadcast();
                        return;
                    }

                    ExportAssignedTexturesOnSlot(
                        slotOnly,
                        exportRelDir,
                        pdm,
                        paths,
                        usedNames,
                        exportedTextureIds,
                        ref _lastSaved,
                        ref _lastEmptySlots,
                        ref _lastFailed
                    );
                    break;

                case ExportScope.AllAssignedOnCharacter:
                case ExportScope.AllUniqueTexturesOnCharacter:
                    ExportAllUniqueTexturesOnCharacter(
                        exportRelDir,
                        pdm,
                        paths,
                        usedNames,
                        exportedTextureIds,
                        ref _lastSaved,
                        ref _lastEmptySlots,
                        ref _lastFailed,
                        out var uniqueFound
                    );
                    _lastUniqueTextureCount = uniqueFound;
                    break;

                case ExportScope.AllAssignedNiloToonOnCharacter:
                {
                    var niloMatCount = 0;
                    foreach (var slot in _slots)
                    {
                        var mat = ResolveLiveMaterial(slot);
                        if (!IsNiloToonMaterial(mat))
                            continue;
                        niloMatCount++;
                        ExportAssignedTexturesOnSlot(
                            slot,
                            exportRelDir,
                            pdm,
                            paths,
                            usedNames,
                            exportedTextureIds,
                            ref _lastSaved,
                            ref _lastEmptySlots,
                            ref _lastFailed
                        );
                    }

                    if (niloMatCount == 0)
                    {
                        _lastMessage =
                            "NiloToon 쉐이더 슬롯 없음. 「전부보내기」를 쓰거나 캐릭터 스폰 후 재시도.\n· "
                            + BuildZeroSaveDiagnostic();
                        Broadcast();
                        return;
                    }
                    break;
                }
            }

            _lastPathsText = paths.Count > 0 ? string.Join("\n", paths) : "";
            _lastMessage = scope switch
            {
                ExportScope.SelectedProperty =>
                    $"단일: 저장 {_lastSaved}, 실패 {_lastFailed}",
                ExportScope.AllAssignedOnSlot =>
                    $"슬롯: 저장 {_lastSaved}, 실패 {_lastFailed}",
                ExportScope.AllAssignedOnCharacter or ExportScope.AllUniqueTexturesOnCharacter =>
                    $"모든 텍스처: 고유 {_lastUniqueTextureCount}장 · 저장 {_lastSaved}, 실패 {_lastFailed}",
                ExportScope.AllAssignedNiloToonOnCharacter =>
                    $"NiloToon: 저장 {_lastSaved}, 실패 {_lastFailed}",
                _ => $"저장 {_lastSaved}",
            };
            if (_lastSaved > 0)
                _lastMessage += $"\n→ {_lastFolder}";
            else
            {
                if (!string.IsNullOrEmpty(_lastFolder))
                    _lastMessage += $"\n· 출력 경로(시작 시 생성): {_lastFolder}";
                _lastMessage +=
                    "\n\n[확인]"
                    + "\n· 초록 Flow를 눌렀는지 (Inspect만 보면 0일 수 있음)"
                    + "\n· DevKit UMod 재빌드 후 Warudo 재시작 (not readable 오류는 구버전 증상)"
                    + "\n· 캐릭터 스폰 후인지"
                    + "\n· 하위 폴더는 MyAvatar 처럼 이름만"
                    + "\n· 다른 머티리얼 슬롯(의상·얼굴·헤어)도 시도";
                if (_lastEmptySlots > 0 && _lastSaved == 0)
                    _lastMessage +=
                        "\n· Poiyomi 등은 빈 Texture 슬롯이 수십 개 — 연결된 텍스처가 없으면 0건이 정상일 수 있음";
                _lastMessage += "\n· " + BuildZeroSaveDiagnostic();
                if (!string.IsNullOrEmpty(_lastFailureHint))
                    _lastMessage += "\n· 상세: " + _lastFailureHint;
            }

            if (_lastSaved > 0 && OpenFolderAfterSave)
                TryOpenExportFolder(_lastFolder);

            Broadcast();
        }

        private static void TryOpenExportFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                return;
            try
            {
                Application.OpenURL("file:///" + folderPath.Replace("\\", "/"));
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[TextureExtract68] 폴더 열기 실패: " + ex.Message);
            }
        }

        private static bool IsNiloToonMaterial(Material mat)
        {
            if (mat?.shader == null)
                return false;
            var name = mat.shader.name ?? "";
            if (name.IndexOf("NiloToon", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (name.IndexOf("SimpleURPToonLit", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (name.IndexOf("Nilo", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return name.IndexOf("Toon", StringComparison.OrdinalIgnoreCase) >= 0
                && name.IndexOf("lil", StringComparison.OrdinalIgnoreCase) < 0
                && name.IndexOf("Poiyomi", StringComparison.OrdinalIgnoreCase) < 0;
        }

        private readonly struct TextureBinding
        {
            public readonly string PropName;
            public readonly Texture Texture;

            public TextureBinding(string propName, Texture texture)
            {
                PropName = propName;
                Texture = texture;
            }
        }

        private static void AddUniquePropertyName(List<string> result, string propName)
        {
            if (string.IsNullOrEmpty(propName))
                return;
            for (var i = 0; i < result.Count; i++)
            {
                if (string.Equals(result[i], propName, StringComparison.Ordinal))
                    return;
            }

            result.Add(propName);
        }

        private static Texture GetTextureDirect(Material mat, string propName)
        {
            if (mat == null || string.IsNullOrEmpty(propName))
                return null;
            try
            {
                var t = mat.GetTexture(propName);
                if (t != null)
                    return t;
            }
            catch
            {
                // ignore
            }

            try
            {
                return mat.GetTexture(Shader.PropertyToID(propName));
            }
            catch
            {
                return null;
            }
        }

        private static bool IsExportableTexture(Texture t)
        {
            if (t == null)
                return false;
            if (t.width < 1 || t.height < 1)
                return false;
            // Cubemap 은 Blit·ReadPixels 호환이 불안정해 제외
            return t is Texture2D or RenderTexture;
        }

        private static List<TextureBinding> CollectNonNullTextureBindings(SlotEntry slot, Material mat)
        {
            var list = new List<TextureBinding>(48);
            if (mat == null)
                return list;

            void TryAdd(string propName, Texture tex)
            {
                if (!IsExportableTexture(tex))
                    return;
                for (var i = 0; i < list.Count; i++)
                {
                    if (
                        string.Equals(list[i].PropName, propName, StringComparison.Ordinal)
                        && ReferenceEquals(list[i].Texture, tex)
                    )
                        return;
                }

                list.Add(new TextureBinding(propName, tex));
            }

            var nilo = IsNiloToonMaterial(mat);
            if (nilo)
            {
                for (var i = 0; i < NiloToonPriorityTexturePropertyNames.Length; i++)
                    TryAdd(NiloToonPriorityTexturePropertyNames[i], GetTextureDirect(mat, NiloToonPriorityTexturePropertyNames[i]));
            }

            try
            {
                var names = mat.GetTexturePropertyNames();
                if (names != null)
                {
                    for (var i = 0; i < names.Length; i++)
                        TryAdd(names[i], GetTextureDirect(mat, names[i]));
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] GetTexturePropertyNames: {ex.Message}");
            }

            if (slot?.TextureProps != null)
            {
                foreach (var prop in slot.TextureProps.Keys)
                    TryAdd(prop, GetTextureDirect(mat, prop));
            }

            if (mat.mainTexture != null)
                TryAdd("mainTexture", mat.mainTexture);

            AppendTexturesFromMaterialPropertyBlock(slot, mat, TryAdd);
            AppendTexturesFromInstanceMaterialIfDifferent(slot, mat, TryAdd);

            return list;
        }

        private static void AppendTexturesFromMaterialPropertyBlock(
            SlotEntry slot,
            Material mat,
            Action<string, Texture> tryAdd
        )
        {
            if (slot?.Renderer == null || mat == null)
                return;

            var block = new MaterialPropertyBlock();
            try
            {
                slot.Renderer.GetPropertyBlock(block, slot.Slot);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] GetPropertyBlock: {ex.Message}");
                return;
            }

            try
            {
                var names = mat.GetTexturePropertyNames();
                if (names == null)
                    return;
                for (var i = 0; i < names.Length; i++)
                {
                    var prop = names[i];
                    if (string.IsNullOrEmpty(prop))
                        continue;
                    tryAdd(prop, block.GetTexture(Shader.PropertyToID(prop)));
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] MPB 텍스처: {ex.Message}");
            }
        }

        private static void AppendTexturesFromInstanceMaterialIfDifferent(
            SlotEntry slot,
            Material sharedMat,
            Action<string, Texture> tryAdd
        )
        {
            if (slot?.Renderer == null || sharedMat == null)
                return;

            Material instMat = null;
            try
            {
                var inst = slot.Renderer.materials;
                if (inst == null || slot.Slot < 0 || slot.Slot >= inst.Length)
                    return;
                instMat = inst[slot.Slot];
            }
            catch
            {
                return;
            }

            if (instMat == null || ReferenceEquals(instMat, sharedMat))
                return;

            try
            {
                var names = instMat.GetTexturePropertyNames();
                if (names == null)
                    return;
                for (var i = 0; i < names.Length; i++)
                    tryAdd(names[i], GetTextureDirect(instMat, names[i]));
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] instance mat: {ex.Message}");
            }
        }

        private void ExportAllUniqueTexturesOnCharacter(
            string exportRelDir,
            PersistentDataManager pdm,
            List<string> paths,
            HashSet<string> usedNames,
            HashSet<int> exportedTextureIds,
            ref int saved,
            ref int emptySlots,
            ref int failed,
            out int uniqueFound
        )
        {
            uniqueFound = 0;
            var unique = new Dictionary<int, (SlotEntry slot, Material mat, string prop, Texture tex)>();

            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var mat = ResolveLiveMaterial(slot);
                var bindings = CollectNonNullTextureBindings(slot, mat);
                if (bindings.Count == 0)
                    emptySlots++;

                for (var b = 0; b < bindings.Count; b++)
                {
                    var binding = bindings[b];
                    var tex = binding.Texture;
                    if (!IsExportableTexture(tex))
                        continue;

                    var id = tex.GetInstanceID();
                    if (!unique.ContainsKey(id))
                        unique[id] = (slot, mat, binding.PropName, tex);
                }
            }

            uniqueFound = unique.Count;

            foreach (var entry in unique.Values)
            {
                TryExportTexture(
                    entry.slot,
                    entry.mat,
                    entry.prop,
                    entry.tex,
                    exportRelDir,
                    pdm,
                    paths,
                    usedNames,
                    exportedTextureIds,
                    ref saved,
                    ref failed
                );
            }
        }

        private void ExportAssignedTexturesOnSlot(
            SlotEntry slot,
            string exportRelDir,
            PersistentDataManager pdm,
            List<string> paths,
            HashSet<string> usedNames,
            HashSet<int> exportedTextureIds,
            ref int saved,
            ref int emptySlots,
            ref int failed
        )
        {
            var mat = ResolveLiveMaterial(slot);
            var bindings = CollectNonNullTextureBindings(slot, mat);
            if (bindings.Count == 0)
            {
                emptySlots++;
                return;
            }

            for (var i = 0; i < bindings.Count; i++)
            {
                var b = bindings[i];
                TryExportTexture(
                    slot,
                    mat,
                    b.PropName,
                    b.Texture,
                    exportRelDir,
                    pdm,
                    paths,
                    usedNames,
                    exportedTextureIds,
                    ref saved,
                    ref failed
                );
            }
        }

        private static void EnsureExportDirectory(PersistentDataManager pdm, string exportRelDir)
        {
            try
            {
                var stampPath = JoinRelativePath(exportRelDir, "_export_started.txt");
                pdm.WriteFileBytes(
                    stampPath,
                    Encoding.UTF8.GetBytes("Node68 Texture Export\n")
                );
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[TextureExtract68] 출력 폴더 생성 실패: " + ex.Message);
            }
        }

        /// <summary>텍스처 읽기는 sharedMaterials 우선 (인스턴스 머티리얼이 비어 있는 경우 방지).</summary>
        private static Material ResolveLiveMaterial(SlotEntry slot)
        {
            if (slot?.Renderer != null)
            {
                try
                {
                    var shared = slot.Renderer.sharedMaterials;
                    if (
                        shared != null
                        && slot.Slot >= 0
                        && slot.Slot < shared.Length
                        && shared[slot.Slot] != null
                    )
                        return shared[slot.Slot];
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[TextureExtract68] sharedMaterials: {ex.Message}");
                }

                try
                {
                    var inst = slot.Renderer.materials;
                    if (slot.Slot >= 0 && slot.Slot < inst.Length && inst[slot.Slot] != null)
                        return inst[slot.Slot];
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[TextureExtract68] materials: {ex.Message}");
                }
            }

            return slot?.SharedMaterialRef;
        }

        private string BuildZeroSaveDiagnostic()
        {
            var parts = new List<string>(8) { $"슬롯 {_slots.Count}개" };
            var nilo = 0;
            var withBase = 0;
            var sampleShaders = new List<string>(4);

            for (var i = 0; i < _slots.Count; i++)
            {
                var s = _slots[i];
                var m = ResolveLiveMaterial(s);
                if (m == null)
                    continue;
                if (IsNiloToonMaterial(m))
                    nilo++;
                if (GetTextureDirect(m, "_BaseMap") != null || GetTextureDirect(m, "_MainTex") != null)
                    withBase++;
                if (sampleShaders.Count < 3 && m.shader != null)
                    sampleShaders.Add(SafeOneLine(m.shader.name));
            }

            parts.Add($"NiloToon슬롯 {nilo}");
            parts.Add($"_BaseMap있음 {withBase}");
            if (sampleShaders.Count > 0)
                parts.Add("쉐이더예: " + string.Join(" | ", sampleShaders));
            return string.Join(", ", parts);
        }

        private static List<string> BuildTexturePropertyCandidates(string propName)
        {
            var candidates = new List<string>(8);
            if (!string.IsNullOrEmpty(propName))
                candidates.Add(propName);

            if (string.Equals(propName, "_MainTex", StringComparison.Ordinal))
            {
                for (var i = 0; i < MainTextureFallbackPropertyNames.Length; i++)
                {
                    var p = MainTextureFallbackPropertyNames[i];
                    if (!candidates.Contains(p))
                        candidates.Add(p);
                }
            }

            return candidates;
        }

        private static bool TryResolveTextureFromMaterial(
            Material mat,
            string propName,
            out Texture texture,
            out string resolvedPropName
        )
        {
            texture = null;
            resolvedPropName = propName;
            if (mat == null)
                return false;

            var candidates = BuildTexturePropertyCandidates(propName);
            for (var i = 0; i < candidates.Count; i++)
            {
                var name = candidates[i];
                var t = GetTextureDirect(mat, name);
                if (t != null)
                {
                    texture = t;
                    resolvedPropName = name;
                    return true;
                }
            }

            if (mat.mainTexture != null)
            {
                texture = mat.mainTexture;
                resolvedPropName = "mainTexture";
                return true;
            }

            return false;
        }

        private static bool TryResolveTexture(
            SlotEntry slot,
            Material mat,
            string propName,
            out Texture texture,
            out string resolvedPropName
        )
        {
            if (TryResolveTextureFromMaterial(mat, propName, out texture, out resolvedPropName))
                return true;

            if (slot?.Renderer == null)
                return false;

            var candidates = BuildTexturePropertyCandidates(propName);
            var block = new MaterialPropertyBlock();
            try
            {
                slot.Renderer.GetPropertyBlock(block, slot.Slot);
                for (var i = 0; i < candidates.Count; i++)
                {
                    var id = Shader.PropertyToID(candidates[i]);
                    var t = block.GetTexture(id);
                    if (t == null)
                        continue;
                    texture = t;
                    resolvedPropName = candidates[i];
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] PropertyBlock 읽기 실패: {ex.Message}");
            }

            return false;
        }

        private void NoteSkip(string hint)
        {
            if (string.IsNullOrEmpty(_lastFailureHint))
                _lastFailureHint = hint;
        }

        private void TryExportOne(
            SlotEntry slot,
            string propName,
            string exportRelDir,
            PersistentDataManager pdm,
            List<string> paths,
            HashSet<string> usedNames,
            HashSet<int> exportedTextureIds,
            bool treatNullAsEmpty,
            ref int saved,
            ref int emptySlots,
            ref int failed
        )
        {
            var mat = ResolveLiveMaterial(slot);
            if (mat == null || string.IsNullOrEmpty(propName))
            {
                failed++;
                NoteSkip("머티리얼 없음");
                return;
            }

            if (!TryResolveTexture(slot, mat, propName, out var src, out var resolvedProp))
            {
                if (treatNullAsEmpty)
                    emptySlots++;
                else
                {
                    failed++;
                    NoteSkip($"{mat.name} · {propName} 텍스처 없음");
                }
                return;
            }

            TryExportTexture(
                slot,
                mat,
                resolvedProp,
                src,
                exportRelDir,
                pdm,
                paths,
                usedNames,
                exportedTextureIds,
                ref saved,
                ref failed
            );
        }

        private void TryExportTexture(
            SlotEntry slot,
            Material mat,
            string propName,
            Texture src,
            string exportRelDir,
            PersistentDataManager pdm,
            List<string> paths,
            HashSet<string> usedNames,
            HashSet<int> exportedTextureIds,
            ref int saved,
            ref int failed
        )
        {
            if (mat == null || src == null || !IsExportableTexture(src))
            {
                failed++;
                return;
            }

            var texId = src.GetInstanceID();
            if (!exportedTextureIds.Add(texId))
                return;

            try
            {
                var baseName = BuildBaseFileName(slot, propName, src, mat);
                var fileName = MakeUniqueFileName(baseName, ".png", usedNames);
                var relPath = JoinRelativePath(exportRelDir, fileName);
                var png = CaptureTextureAsPng(src, MaxLongEdge);
                if (png == null || png.Length == 0)
                {
                    failed++;
                    NoteSkip($"{mat.name} · {propName} PNG 변환 실패");
                    return;
                }

                pdm.WriteFileBytes(relPath, png);
                paths.Add(pdm.GetFullPath(relPath));
                saved++;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    $"[TextureExtract68] 저장 실패: mat={mat.name}, prop={propName}, tex={src.name}, err={ex.Message}"
                );
                failed++;
                NoteSkip("저장 실패: " + ex.Message);
            }
        }

        private string BuildBaseFileName(SlotEntry slot, string propName, Texture tex, Material mat)
        {
            var prefix = SanitizeFileToken(FilePrefix);
            var matPart = SanitizeFileToken(mat != null ? mat.name : "mat");
            var propPart = SanitizeFileToken(propName);
            var texPart = SanitizeFileToken(tex != null ? tex.name : "tex");
            var slotHint = SanitizeFileToken(ShortSlotHint(slot.ValueKey));
            var core = $"{matPart}__{propPart}__{texPart}__{slotHint}";
            return string.IsNullOrEmpty(prefix) ? core : $"{prefix}_{core}";
        }

        private static string ShortSlotHint(string valueKey)
        {
            if (string.IsNullOrEmpty(valueKey))
                return "slot";
            var parts = valueKey.Split('\u241F');
            if (parts.Length < 3)
                return valueKey;
            return $"{parts[0]}_{parts[1]}_s{parts[2]}";
        }

        private static string MakeUniqueFileName(string baseName, string ext, HashSet<string> used)
        {
            var candidate = baseName + ext;
            if (used.Add(candidate))
                return candidate;

            for (var i = 2; i < 10000; i++)
            {
                candidate = $"{baseName}_{i}{ext}";
                if (used.Add(candidate))
                    return candidate;
            }

            candidate = $"{baseName}_{Guid.NewGuid():N}{ext}";
            used.Add(candidate);
            return candidate;
        }

        private static string SanitizeFileToken(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "x";
            var sb = new StringBuilder(raw.Length);
            foreach (var ch in raw.Trim())
            {
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                    sb.Append(ch);
                else
                    sb.Append('_');
            }

            var s = sb.ToString().Trim('_');
            if (s.Length == 0)
                s = "x";
            if (s.Length > 80)
                s = s.Substring(0, 80);
            return s;
        }

        private string BuildExportRelativeDirectory()
        {
            var sub = ResolveSubfolderToken();
            return JoinRelativePath(ExportRelativeBaseDir, sub) + "/";
        }

        /// <summary>하위 폴더 필드에 경로 전체를 붙여넣은 경우 마지막 폴더명만 사용합니다.</summary>
        private string ResolveSubfolderToken()
        {
            var raw = SubfolderName?.Trim() ?? "";
            if (string.IsNullOrEmpty(raw))
                return DateTime.Now.ToString("yyyyMMdd_HHmmss");

            var normalized = raw.Replace('\\', '/');
            if (
                normalized.IndexOf('/') >= 0
                || normalized.IndexOf(':') >= 0
                || normalized.IndexOf("StreamingAssets", StringComparison.OrdinalIgnoreCase) >= 0
                || normalized.IndexOf("Warudo_Data", StringComparison.OrdinalIgnoreCase) >= 0
            )
            {
                var trimmed = normalized.TrimEnd('/');
                var slash = trimmed.LastIndexOf('/');
                if (slash >= 0 && slash < trimmed.Length - 1)
                    raw = trimmed.Substring(slash + 1);
                Debug.LogWarning(
                    "[TextureExtract68] 하위 폴더에 경로가 들어있어 마지막 이름만 사용합니다: "
                        + raw
                );
            }

            return SanitizeFileToken(raw);
        }

        private static string JoinRelativePath(string left, string right)
        {
            left = (left ?? "").Trim().TrimEnd('/', '\\');
            right = (right ?? "").Trim().TrimStart('/', '\\');
            if (left.Length == 0)
                return right;
            if (right.Length == 0)
                return left;
            return left + "/" + right;
        }

        /// <summary>
        /// Read/Write 꺼진 VRChat·NiloToon 텍스처 — 소스에 EncodeToPNG/GetPixels 를 호출하지 않고 RT 경유.
        /// </summary>
        private static byte[] CaptureTextureAsPng(Texture src, int maxLongEdge)
        {
            if (src == null)
                return null;

            if (src is Texture2D readable2d && readable2d.isReadable)
            {
                try
                {
                    return EncodeReadableTexture2D(readable2d, maxLongEdge);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        "[TextureExtract68] readable 직접 인코딩 실패, Blit 경유: " + ex.Message
                    );
                }
            }

            var w = Mathf.Max(1, src.width);
            var h = Mathf.Max(1, src.height);
            if (maxLongEdge > 0)
            {
                var scale = Mathf.Min((float)maxLongEdge / w, (float)maxLongEdge / h);
                if (scale < 1f)
                {
                    w = Mathf.Max(1, Mathf.RoundToInt(w * scale));
                    h = Mathf.Max(1, Mathf.RoundToInt(h * scale));
                }
            }

            var rt = RenderTexture.GetTemporary(
                w,
                h,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.sRGB
            );
            try
            {
                var prev = RenderTexture.active;
                try
                {
                    Graphics.Blit(src, rt);
                }
                finally
                {
                    RenderTexture.active = prev;
                }

                return EncodeRenderTextureToPng(rt);
            }
            finally
            {
                RenderTexture.ReleaseTemporary(rt);
            }
        }

        private static byte[] EncodeReadableTexture2D(Texture2D src, int maxLongEdge)
        {
            if (src == null)
                return null;

            var w = src.width;
            var h = src.height;
            if (maxLongEdge > 0)
            {
                var scale = Mathf.Min((float)maxLongEdge / w, (float)maxLongEdge / h);
                if (scale < 1f)
                {
                    w = Mathf.Max(1, Mathf.RoundToInt(w * scale));
                    h = Mathf.Max(1, Mathf.RoundToInt(h * scale));
                    var scaled = new Texture2D(w, h, TextureFormat.RGBA32, false);
                    try
                    {
                        Graphics.ConvertTexture(src, scaled);
                        return scaled.EncodeToPNG();
                    }
                    finally
                    {
                        UnityEngine.Object.Destroy(scaled);
                    }
                }
            }

            return src.EncodeToPNG();
        }

        private static byte[] EncodeRenderTextureToPng(RenderTexture rt)
        {
            if (rt == null)
                return null;

            Texture2D scratch = null;
            var prev = RenderTexture.active;
            try
            {
                RenderTexture.active = rt;
                scratch = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
                scratch.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, false);
                scratch.Apply(false, false);
                return scratch.EncodeToPNG();
            }
            catch (Exception readEx)
            {
                Debug.LogWarning(
                    "[TextureExtract68] ReadPixels 실패, AsyncGPUReadback 시도: " + readEx.Message
                );
            }
            finally
            {
                RenderTexture.active = prev;
                if (scratch != null)
                    UnityEngine.Object.Destroy(scratch);
            }

            if (!SystemInfo.supportsAsyncGPUReadback)
                return null;

            try
            {
                var request = AsyncGPUReadback.Request(rt, 0, TextureFormat.RGBA32);
                request.WaitForCompletion();
                if (request.hasError)
                    return null;

                var data = request.GetData<byte>();
                var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
                try
                {
                    tex.LoadRawTextureData(data);
                    tex.Apply(false, false);
                    return tex.EncodeToPNG();
                }
                finally
                {
                    UnityEngine.Object.Destroy(tex);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[TextureExtract68] AsyncGPUReadback 실패: " + ex.Message);
                return null;
            }
        }

        private bool TryResolveSlot(out SlotEntry slot)
        {
            slot = null;
            if (Character == null || string.IsNullOrEmpty(SelectedMaterialSlotKey))
                return false;
            return _slotByKey.TryGetValue(SelectedMaterialSlotKey, out slot)
                && slot?.SharedMaterialRef != null;
        }

        private void RebuildSlotsFromCharacter()
        {
            if (Character == null)
            {
                _slots.Clear();
                _slotByKey.Clear();
                return;
            }

            var stamp = ComputeLayoutStamp(Character);
            if (
                ReferenceEquals(Character, _cachedCharacter)
                && stamp == _characterLayoutStamp
                && _slots.Count > 0
            )
                return;

            _cachedCharacter = Character;
            _characterLayoutStamp = stamp;
            _slots.Clear();
            _slotByKey.Clear();

            var nameCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            void Collect<T>(Dictionary<string, T> dict, string kind)
                where T : Renderer
            {
                if (dict == null)
                    return;
                foreach (var kv in dict)
                {
                    var r = kv.Value;
                    if (r == null)
                        continue;
                    var mats = r.sharedMaterials;
                    if (mats == null || mats.Length == 0)
                        continue;
                    for (var i = 0; i < mats.Length; i++)
                    {
                        var m = mats[i];
                        if (m == null)
                            continue;
                        nameCount.TryGetValue(m.name, out var dup);
                        nameCount[m.name] = dup + 1;
                        var suffix = dup > 0 ? $" #{dup + 1}" : "";
                        var key = $"{kind}\u241F{kv.Key}\u241F{i}\u241F{m.GetInstanceID()}";
                        var label = $"{kind} [{kv.Key}] · {i}: {SafeOneLine(m.name)}{suffix}";
                        var entry = new SlotEntry
                        {
                            ValueKey = key,
                            DropdownLabel = label,
                            Renderer = r,
                            Slot = i,
                            SharedMaterialRef = m,
                            TextureProps = BuildTextureProps(m.shader),
                        };
                        _slots.Add(entry);
                        _slotByKey[key] = entry;
                    }
                }
            }

            Collect(Character.SkinnedMeshRenderers, "SMR");
            Collect(Character.MeshRenderers, "MR");

            _slots.Sort(
                (a, b) =>
                    string.Compare(
                        a.DropdownLabel,
                        b.DropdownLabel,
                        StringComparison.OrdinalIgnoreCase
                    )
            );
            SanitizeSelectionAfterRebuild();
            InvalidateMaterialAutocomplete();
        }

        private static Dictionary<string, ShaderPropertyType> BuildTextureProps(Shader shader)
        {
            var map = new Dictionary<string, ShaderPropertyType>(StringComparer.Ordinal);
            if (shader == null)
                return map;
            try
            {
                var n = shader.GetPropertyCount();
                for (var i = 0; i < n; i++)
                {
                    var name = shader.GetPropertyName(i);
                    if (string.IsNullOrEmpty(name))
                        continue;
                    var t = shader.GetPropertyType(i);
                    if (!IsTextureFamily(t))
                        continue;
                    map[name] = t;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TextureExtract68] shader={shader.name} 스캔 실패: {ex.Message}");
            }

            return map;
        }

        private static bool IsTextureFamily(ShaderPropertyType t)
        {
            if (t == ShaderPropertyType.Texture)
                return true;
            var label = t.ToString();
            return label.IndexOf("Texture", StringComparison.Ordinal) >= 0
                || label.IndexOf("Cube", StringComparison.Ordinal) >= 0;
        }

        private static int ComputeLayoutStamp(CharacterAsset ch)
        {
            if (ch == null)
                return 0;
            unchecked
            {
                return HashCode.Combine(
                    ch.SkinnedMeshRenderers?.GetHashCode() ?? 0,
                    ch.MeshRenderers?.GetHashCode() ?? 0,
                    ch.SkinnedMeshRenderers?.Count ?? 0,
                    ch.MeshRenderers?.Count ?? 0
                );
            }
        }

        private void SanitizeSelectionAfterRebuild()
        {
            if (_slotByKey.Count == 0)
            {
                SelectedMaterialSlotKey = "";
                SelectedPropertyName = "";
                return;
            }

            if (
                string.IsNullOrEmpty(SelectedMaterialSlotKey)
                || !_slotByKey.ContainsKey(SelectedMaterialSlotKey)
            )
                SelectedMaterialSlotKey = _slots[0].ValueKey;

            if (!TryResolveSlot(out var slot))
            {
                SelectedPropertyName = "";
                return;
            }

            if (string.IsNullOrEmpty(SelectedPropertyName) || !slot.TextureProps.ContainsKey(SelectedPropertyName))
            {
                var first = slot.TextureProps.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase).FirstOrDefault();
                SelectedPropertyName = first ?? "";
            }
        }

        public async UniTask<AutoCompleteList> AutoCompleteMaterialSlots()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 지정하세요");

            RebuildSlotsFromCharacter();
            if (_slots.Count == 0)
                return AutoCompleteList.Message("머티리얼이 없습니다(스폰·렌더러 확인)");

            var stamp = $"{Character.Id}\u241F{_characterLayoutStamp}\u241F{_slots.Count}";
            if (
                _acMaterials != null
                && ReferenceEquals(_acMatCharacter, Character)
                && _acMatStampKey == stamp
            )
                return _acMaterials;

            var entries = _slots
                .Select(s => new AutoCompleteEntry { label = s.DropdownLabel, value = s.ValueKey })
                .ToList();

            _acMaterials = entries.ToAutoCompleteList();
            _acMatCharacter = Character;
            _acMatStampKey = stamp;
            return _acMaterials;
        }

        public async UniTask<AutoCompleteList> AutoCompleteTextureProperties()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터 필요");
            if (
                string.IsNullOrEmpty(SelectedMaterialSlotKey)
                || !_slotByKey.TryGetValue(SelectedMaterialSlotKey, out var slot)
            )
                return AutoCompleteList.Message("머티리얼 슬롯을 먼저 고르세요");

            var key = $"{SelectedMaterialSlotKey}\u241F{slot.TextureProps.Count}";
            if (_acProps != null && _acPropSlotKey == key)
                return _acProps;

            var list = slot.TextureProps.Keys
                .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
                .Select(k => new AutoCompleteEntry
                {
                    label = $"{k} [{slot.TextureProps[k]}]",
                    value = k,
                })
                .ToList();

            _acProps =
                list.Count > 0
                    ? list.ToAutoCompleteList()
                    : AutoCompleteList.Message("Texture 프로퍼티가 없습니다");
            _acPropSlotKey = key;
            return _acProps;
        }

        private void InvalidateMaterialAutocomplete()
        {
            _acMaterials = null;
            _acMatCharacter = null;
            _acMatStampKey = null;
            _acProps = null;
            _acPropSlotKey = null;
        }

        private static string SafeOneLine(string s) =>
            string.IsNullOrEmpty(s) ? "" : s.Replace('\r', ' ').Replace('\n', ' ');
    }

}
