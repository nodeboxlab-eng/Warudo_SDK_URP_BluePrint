using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.CustomNodes
{
    /// <summary>
    /// <see cref="CharacterAsset"/> 의 스킨드 메시에 대해 BlendShape 가중치(0~100)를 설정합니다.
    /// 자동완성으로 메시 키·블렌드쉐이프 이름을 고릅니다.
    /// </summary>
    [NodeType(
        Id = "f3e2d1c0-b5a4-4938-8f7e-1a2b3c4d5e6f",
        Title = "Character BlendShape Weight Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.35f
    )]
    public sealed class CharacterBlendShapeWeightNode68 : Node
    {
        /// <summary>메시 키와 블렌드쉐이프 이름 구분자(값에 포함되지 않도록 제어 문자 사용).</summary>
        private const char TargetPairSeparator = '\u001f';

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "있으면 이 렌더러만 대상입니다. 캐릭터와 함께 쓰면 해당 SMR의 키로 자동완성이 맞춰집니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("캐릭터")]
        public CharacterAsset Character;

        [DataInput]
        [Label("스킨 메시 키")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 캐릭터의 모든 SkinnedMeshRenderer. SkinnedMesh가 있으면 숨깁니다.")]
        [HiddenIf(nameof(HideTargetSkinnedMeshKeyField))]
        [AutoComplete(nameof(AutoCompleteSkinnedMeshKeys))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [Label("적용할 블렌드 (자동완성)")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "항목을 고를 때마다 배열에 한 줄씩 추가됩니다. 형식: 메시키+제어 문자+쉐이프명.")]
        [HiddenIf(nameof(HideInShareBuild))]
        [AutoComplete(nameof(AutoCompleteBlendShapeRows), forceSelection: true)]
        public string[] BlendShapeTargets = Array.Empty<string>();

        [DataInput]
        [Label("가중치 (0~100)")]
        [FloatSlider(0f, 100f)]
        [HiddenIf(nameof(HideInShareBuild))]
        public float Weight = 0f;

        [DataInput]
        [Label("동일 이름 전체 메시 적용")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "켜면 각 항목의 블렌드 이름이 붙은 모든 SkinnedMesh에 적용합니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool MatchNameAcrossRenderers;

        [DataInput]
        [Label("트랜지션 시간 (초, 0이면 즉시)")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float TransitionTime;

        [DataInput]
        [Label("트랜지션 이징")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Ease TransitionEasing = Ease.OutCubic;

        private bool HideMeshKeyAutocomplete() => SkinnedMesh != null;

        private bool HideTargetSkinnedMeshKeyField() =>
            HideInShareBuild() || HideMeshKeyAutocomplete();

        private CharacterAsset _acMeshKeysCharacterRef;
        private Dictionary<string, SkinnedMeshRenderer> _acMeshKeysDictRef;
        private AutoCompleteList _acMeshKeysCachedList;

        private CharacterAsset _acBlendCharacterRef;
        private Dictionary<string, SkinnedMeshRenderer> _acBlendDictRef;
        private string _acBlendMeshFilterKey;
        private AutoCompleteList _acBlendCachedList;

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyShareSuffixToDisplayName();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyShareSuffixToDisplayName();
        }

        private const string ShareDisplayNameSuffix = " Shr";

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Character BlendShape Weight Node68";

            if (Node68BuildRuntime.IsShareBuild())
            {
                var core = string.IsNullOrEmpty(Name)
                    ? baseName
                    : Node68ShareNodeTypeTitles.StripToBaseDisplayName(Name, ShareDisplayNameSuffix);
                if (string.IsNullOrEmpty(core))
                    core = baseName;
                Name = core + ShareDisplayNameSuffix;
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                    Name = baseName;
                else
                {
                    var cleaned = Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
                    Name = string.IsNullOrEmpty(cleaned) ? baseName : cleaned;
                }
            }
        }

        public async UniTask<AutoCompleteList> AutoCompleteSkinnedMeshKeys()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return AutoCompleteList.Message("스킨드 메시가 없습니다");

            if (_acMeshKeysCachedList != null
                && ReferenceEquals(_acMeshKeysCharacterRef, Character)
                && ReferenceEquals(_acMeshKeysDictRef, smrs))
                return _acMeshKeysCachedList;

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
                entries.Add(new AutoCompleteEntry
                {
                    label = bs >= 1 ? $"{key} ({bs} 블렌드)" : key,
                    value = key
                });
            }

            _acMeshKeysCachedList = entries.ToAutoCompleteList();
            _acMeshKeysCharacterRef = Character;
            _acMeshKeysDictRef = smrs;
            return _acMeshKeysCachedList;
        }

        public async UniTask<AutoCompleteList> AutoCompleteBlendShapeRows()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return AutoCompleteList.Message("스킨드 메시가 없습니다");

            var meshFilter = "";
            if (SkinnedMesh != null)
            {
                if (!TryGetMeshKey(smrs, SkinnedMesh, out meshFilter))
                    return AutoCompleteList.Message("SkinnedMesh에 해당하는 메시 키를 찾을 수 없습니다");
            }
            else if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
                meshFilter = TargetSkinnedMeshKey.Trim();

            if (_acBlendCachedList != null
                && ReferenceEquals(_acBlendCharacterRef, Character)
                && ReferenceEquals(_acBlendDictRef, smrs)
                && string.Equals(_acBlendMeshFilterKey, meshFilter, StringComparison.Ordinal))
                return _acBlendCachedList;

            var entries = new List<AutoCompleteEntry>(64);

            foreach (var kv in smrs)
            {
                var meshKey = kv.Key;
                var smr = kv.Value;

                if (smr == null)
                    continue;
                if (meshFilter.Length > 0 && !string.Equals(meshKey, meshFilter, StringComparison.Ordinal))
                    continue;

                var mesh = smr.sharedMesh;
                if (mesh == null)
                    continue;

                var n = mesh.blendShapeCount;
                for (var i = 0; i < n; i++)
                {
                    var shapeName = mesh.GetBlendShapeName(i);

                    var value = meshKey + TargetPairSeparator + shapeName;
                    entries.Add(new AutoCompleteEntry
                    {
                        label = $"[{meshKey}] {shapeName}",
                        value = value
                    });
                }
            }

            entries.Sort((a, b) =>
                string.Compare(a.label, b.label, StringComparison.OrdinalIgnoreCase));

            var result = entries.Count > 0
                ? entries.ToAutoCompleteList()
                : AutoCompleteList.Message("블렌드쉐이프가 없습니다");

            _acBlendCachedList = result;
            _acBlendCharacterRef = Character;
            _acBlendDictRef = smrs;
            _acBlendMeshFilterKey = meshFilter;
            return result;
        }

        private static bool TryGetMeshKey(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            SkinnedMeshRenderer smr,
            out string key
        )
        {
            foreach (var kv in smrs)
            {
                if (ReferenceEquals(kv.Value, smr))
                {
                    key = kv.Key;
                    return true;
                }
            }

            key = null;
            return false;
        }

        private Sequence _sequence;

        [FlowInput]
        public Continuation Enter()
        {
            KillSequence();

            ApplyNow(useTween: Mathf.Max(0f, TransitionTime) > 0f);
            return Exit;
        }

        private void KillSequence()
        {
            _sequence?.Kill(false);
            _sequence = null;
        }

        protected override void OnDestroy()
        {
            KillSequence();
            base.OnDestroy();
        }

        private void ApplyNow(bool useTween)
        {
            if (Character == null)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var rows = CollectApplyRows();
            if (rows.Count == 0)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var duration = Mathf.Max(0f, TransitionTime);
            if (!useTween || duration <= 0f)
            {
                ApplyImmediateRows(rows);
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var seq = DOTween.Sequence();
            var any = false;

            foreach (var row in rows)
            {
                if (row.Smr == null || row.Mesh == null || row.ShapeIndex < 0)
                    continue;

                var smr = row.Smr;
                var idx = row.ShapeIndex;
                var end = Mathf.Clamp(Weight, 0f, 100f);
                var start = smr.GetBlendShapeWeight(idx);

                any = true;

                var tween = DOVirtual.Float(
                        start,
                        end,
                        duration,
                        w => smr.SetBlendShapeWeight(idx, w))
                    .SetEase(TransitionEasing);

                seq.Join(tween);
            }

            if (!any)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            seq.OnComplete(() =>
            {
                _sequence = null;
                InvokeFlow(nameof(OnTransitionEnd));
            });

            _sequence = seq;
        }

        private void ApplyImmediateRows(List<ApplyRow> rows)
        {
            var w = Mathf.Clamp(Weight, 0f, 100f);
            foreach (var row in rows)
            {
                if (row.Smr == null || row.Mesh == null || row.ShapeIndex < 0)
                    continue;
                row.Smr.SetBlendShapeWeight(row.ShapeIndex, w);
            }
        }

        private readonly List<ApplyRow> _rowBuffer = new List<ApplyRow>(32);

        private List<ApplyRow> CollectApplyRows()
        {
            _rowBuffer.Clear();

            if (Character == null)
                return _rowBuffer;

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return _rowBuffer;

            var seen = new HashSet<ulong>();
            var targets = BlendShapeTargets;
            if (targets == null || targets.Length == 0)
                return _rowBuffer;

            foreach (var raw in targets)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;

                if (!TryParseTargetRow(raw, out var meshKey, out var shapeName))
                    continue;

                if (MatchNameAcrossRenderers)
                {
                    foreach (var kv in smrs)
                    {
                        var smr = kv.Value;
                        if (smr == null)
                            continue;

                        var mesh = smr.sharedMesh;
                        if (mesh == null)
                            continue;

                        var idx = FindBlendShapeIndex(mesh, shapeName);
                        if (idx < 0)
                            continue;

                        TryAddRow(seen, smr, mesh, idx);
                    }
                }
                else
                {
                    if (!smrs.TryGetValue(meshKey, out var smr) || smr == null)
                    {
                        if (SkinnedMesh != null
                            && TryGetMeshKey(smrs, SkinnedMesh, out var k)
                            && string.Equals(meshKey, k, StringComparison.Ordinal))
                            smr = SkinnedMesh;
                        else
                            continue;
                    }

                    var mesh = smr.sharedMesh;
                    if (mesh == null)
                        continue;

                    var idx = FindBlendShapeIndex(mesh, shapeName);
                    if (idx < 0)
                        continue;

                    TryAddRow(seen, smr, mesh, idx);
                }
            }

            return _rowBuffer;
        }

        private void TryAddRow(HashSet<ulong> seen, SkinnedMeshRenderer smr, Mesh mesh, int shapeIndex)
        {
            var key = PackSmrShape(smr, shapeIndex);
            if (!seen.Add(key))
                return;

            _rowBuffer.Add(new ApplyRow { Smr = smr, Mesh = mesh, ShapeIndex = shapeIndex });
        }

        private static ulong PackSmrShape(SkinnedMeshRenderer smr, int shapeIndex)
        {
            unchecked
            {
                return ((ulong)(uint)smr.GetInstanceID() << 32) | (uint)shapeIndex;
            }
        }

        private static bool TryParseTargetRow(string raw, out string meshKey, out string shapeName)
        {
            meshKey = null;
            shapeName = null;

            var sep = raw.IndexOf(TargetPairSeparator);
            if (sep <= 0 || sep >= raw.Length - 1)
                return false;

            meshKey = raw.Substring(0, sep);
            shapeName = raw.Substring(sep + 1);
            return !string.IsNullOrEmpty(shapeName);
        }

        private static int FindBlendShapeIndex(Mesh mesh, string shapeName)
        {
            if (mesh == null || string.IsNullOrEmpty(shapeName))
                return -1;

            var n = mesh.blendShapeCount;
            for (var i = 0; i < n; i++)
            {
                if (string.Equals(mesh.GetBlendShapeName(i), shapeName, StringComparison.Ordinal))
                    return i;
            }

            for (var i = 0; i < n; i++)
            {
                if (string.Equals(mesh.GetBlendShapeName(i), shapeName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        private struct ApplyRow
        {
            public SkinnedMeshRenderer Smr;
            public Mesh Mesh;
            public int ShapeIndex;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("트랜지션 종료 시")]
        public Continuation OnTransitionEnd;
    }
}
