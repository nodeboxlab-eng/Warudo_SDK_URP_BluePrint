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
    /// <see cref="SkinnedMeshRenderer"/> 에 바인드된 본(<c>bones</c>)의 <see cref="Transform.localScale"/> 을 변경합니다.
    /// 휴머노이드 매핑이 없는 추가 본이나 특정 메시 계층 조절용입니다.
    /// </summary>
    [NodeType(
        Id = "8f2a4c1e-3b7d-4a9e-bc12-0d1e2f3a4b5d",
        Title = "Skinned Mesh Bone Scale Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.38f
    )]
    public sealed class SkinnedMeshBoneScaleNode68 : Node
    {
        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "직접 지정하면 해당 SMR만 사용합니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("캐릭터")]
        public CharacterAsset Character;

        [DataInput]
        [Label("스킨 메시 키")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "SkinnedMesh와 둘 다 비었을 때는 선택할 수 없습니다. 하나의 SMR만 정해 진단 자동완성이 활성화됩니다.")]
        [HiddenIf(nameof(HideTargetSkinnedMeshKeyField))]
        [AutoComplete(nameof(AutoCompleteSkinnedMeshKeys))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [Label("본 경로 목록")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "자동완성 값은 해당 SMR의 rootBone 기준 계층 경로입니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        [AutoComplete(nameof(AutoCompleteBonePaths), forceSelection: true)]
        public string[] BoneHierarchyPaths = Array.Empty<string>();

        [DataInput]
        [Label("로컬 스케일")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Vector3 LocalScale = new Vector3(0.85f, 0.85f, 0.85f);

        [DataInput]
        [Label("트랜지션 시간 (초)")]
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

        private CharacterAsset _acBoneCharacterRef;
        private Dictionary<string, SkinnedMeshRenderer> _acBoneSmrsDictRef;
        private string _acBoneMeshFilterKey;
        private AutoCompleteList _acBoneCachedList;

        private Sequence _sequence;

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
                baseName = "Skinned Mesh Bone Scale Node68";

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
                entries.Add(new AutoCompleteEntry { label = key, value = key });

            _acMeshKeysCachedList = entries.ToAutoCompleteList();
            _acMeshKeysCharacterRef = Character;
            _acMeshKeysDictRef = smrs;
            return _acMeshKeysCachedList;
        }

        public async UniTask<AutoCompleteList> AutoCompleteBonePaths()
        {
            await UniTask.CompletedTask;
            if (Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0)
                return AutoCompleteList.Message("스킨드 메시가 없습니다");

            SkinnedMeshRenderer targetSmr;
            if (!TryResolvePrimarySkinnedMesh(smrs, out targetSmr) || targetSmr == null)
                return AutoCompleteList.Message(
                    "SkinnedMeshRenderer 지정 또는 스킨 메시 키를 선택하세요."
                );

            var meshFilter = "";
            if (SkinnedMesh != null)
            {
                if (!TryGetMeshKey(smrs, SkinnedMesh, out meshFilter))
                    return AutoCompleteList.Message(
                        "SkinnedMesh 에 해당하는 메시 키를 찾을 수 없습니다"
                    );
            }
            else if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
                meshFilter = TargetSkinnedMeshKey.Trim();

            if (_acBoneCachedList != null
                && ReferenceEquals(_acBoneCharacterRef, Character)
                && ReferenceEquals(_acBoneSmrsDictRef, smrs)
                && string.Equals(_acBoneMeshFilterKey, meshFilter, StringComparison.Ordinal))
                return _acBoneCachedList;

            var bones = targetSmr.bones;
            var rootBone = targetSmr.rootBone;
            if (rootBone == null || bones == null || bones.Length == 0)
                return AutoCompleteList.Message(
                    "바인드 본 또는 rootBone 이 없습니다"
                );

            var pathSeen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in bones)
            {
                if (TryGetHierarchyPathFromRoot(b, rootBone, out var p))
                    pathSeen.Add(p);
            }

            var pathList = new List<string>(pathSeen);
            pathList.Sort(StringComparer.OrdinalIgnoreCase);

            var boneEntries = new List<AutoCompleteEntry>(pathList.Count);
            foreach (var p in pathList)
            {
                boneEntries.Add(new AutoCompleteEntry { label = p, value = p });
            }

            var result = boneEntries.Count > 0
                ? boneEntries.ToAutoCompleteList()
                : AutoCompleteList.Message("바인드 본 경로가 없습니다");

            _acBoneCachedList = result;
            _acBoneCharacterRef = Character;
            _acBoneSmrsDictRef = smrs;
            _acBoneMeshFilterKey = meshFilter;
            return result;
        }

        private bool TryResolvePrimarySkinnedMesh(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            out SkinnedMeshRenderer smr
        )
        {
            smr = null;
            if (SkinnedMesh != null)
            {
                smr = SkinnedMesh;
                return true;
            }

            var keyTrim = TargetSkinnedMeshKey != null ? TargetSkinnedMeshKey.Trim() : "";
            if (keyTrim.Length != 0 && smrs.TryGetValue(keyTrim, out var byKey) && byKey != null)
            {
                smr = byKey;
                return true;
            }

            return false;
        }

        private static bool TryGetMeshKey(
            Dictionary<string, SkinnedMeshRenderer> smrs,
            SkinnedMeshRenderer needle,
            out string key
        )
        {
            foreach (var kv in smrs)
            {
                if (ReferenceEquals(kv.Value, needle))
                {
                    key = kv.Key;
                    return true;
                }
            }

            key = null;
            return false;
        }

        private static bool TryGetHierarchyPathFromRoot(
            Transform bone,
            Transform rootBone,
            out string path
        )
        {
            path = null;
            if (bone == null || rootBone == null)
                return false;
            if (bone != rootBone && !bone.IsChildOf(rootBone))
                return false;

            var segments = new List<string>();
            for (Transform t = bone; t != null; t = t.parent)
            {
                segments.Add(t.name);
                if (t == rootBone)
                    break;
            }

            segments.Reverse();
            path = string.Join("/", segments);
            return path.Length > 0;
        }

        private Transform ResolveBoneByHierarchyPath(SkinnedMeshRenderer smr, string rawPath)
        {
            var rootBone = smr != null ? smr.rootBone : null;
            if (smr == null || rootBone == null || string.IsNullOrWhiteSpace(rawPath))
                return null;

            var path = rawPath.Trim();
            var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            Transform current = rootBone;
            var idx = 0;
            if (string.Equals(parts[0], rootBone.name, StringComparison.Ordinal))
            {
                idx = 1;
            }

            for (; idx < parts.Length; idx++)
            {
                var need = parts[idx];
                Transform next = null;
                for (var c = 0; c < current.childCount; c++)
                {
                    var ch = current.GetChild(c);
                    if (string.Equals(ch.name, need, StringComparison.Ordinal))
                    {
                        next = ch;
                        break;
                    }
                }

                if (next == null)
                    return null;
                current = next;
            }

            return current;
        }

        [FlowInput]
        public Continuation Enter()
        {
            ApplyNow();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("트랜지션 종료 시")]
        public Continuation OnTransitionEnd;

        protected override void OnDestroy()
        {
            _sequence?.Kill(false);
            _sequence = null;
            base.OnDestroy();
        }

        private void ApplyNow()
        {
            _sequence?.Kill(false);
            _sequence = null;

            if (Character == null)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var smrs = Character.SkinnedMeshRenderers;
            if (smrs == null || smrs.Count == 0
                || !TryResolvePrimarySkinnedMesh(smrs, out var smr)
                || smr == null)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var paths = BoneHierarchyPaths;
            if (paths == null || paths.Length == 0)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var transforms = new List<Transform>(paths.Length);
            var seen = new HashSet<Transform>();

            foreach (var raw in paths)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                var tr = ResolveBoneByHierarchyPath(smr, raw);
                if (tr == null || !seen.Add(tr))
                    continue;
                transforms.Add(tr);
            }

            if (transforms.Count == 0)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var duration = Mathf.Max(0f, TransitionTime);

            if (duration <= 0f)
            {
                foreach (var tr in transforms)
                    tr.localScale = LocalScale;
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var seq = DOTween.Sequence();
            foreach (var tr in transforms)
            {
                seq.Join(tr.DOScale(LocalScale, duration).SetEase(TransitionEasing));
            }

            seq.OnComplete(() =>
            {
                _sequence = null;
                InvokeFlow(nameof(OnTransitionEnd));
            });

            _sequence = seq;
        }
    }
}
