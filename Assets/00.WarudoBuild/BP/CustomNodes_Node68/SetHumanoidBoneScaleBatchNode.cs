using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Core.Utils;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 여러 휴머노이드 본을 한 노드에서 선택하고, 동일한 XYZ 스케일을 한 번에 적용합니다 (본 그룹 + 공유 트랜스폼).
    /// 본 입력은 문자열 배열 자동완성으로 제공되어 에디터에서 이름 검색이 가능합니다.
    /// </summary>
    [NodeType(
        Id = "a1b2c3d4-e5f6-4789-a012-3456789abcde",
        Title = "Bone Group Scale Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.3f
    )]
    public sealed class SetHumanoidBoneScaleBatchNode : Node
    {
        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataInput]
        [Label("캐릭터")]
        public CharacterAsset Character;

        /// <summary>
        /// 구버전 블루프린트용. 예전 노드가 <c>Bones</c> (HumanBodyBones[]) 키로 저장했을 때 로드된다.
        /// 인스펙터에는 안 보인다 — 편집은 <see cref="BoneNames"/> 로 한다.
        /// </summary>
        [DataInput]
        [Hidden]
        public HumanBodyBones[] Bones;

        /// <summary>자동완성 표시는 HumanBody 별로 한 줄(띄어 쓴 영문)만 씁니다.</summary>
        [DataInput]
        [Label("본 목록")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "자동완성을 연 뒤 글자를 입력하면 목록이 필터됩니다. 저장된 예전 형식은 자동으로 읽습니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        [AutoComplete(nameof(AutoCompleteBoneNames), forceSelection: true)]
        public string[] BoneNames;

        [DataInput]
        [Label("스케일")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Vector3 Scale = Vector3.one;

        [DataInput]
        [Label("트랜지션 시간 (초)")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float TransitionTime = 1.2f;

        [DataInput]
        [Label("트랜지션 이징")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Ease TransitionEasing = Ease.OutCubic;

        private Sequence _sequence;

        private UniTask<AutoCompleteList> AutoCompleteBoneNames()
        {
            if (Node68BuildRuntime.IsShareBuild())
                return UniTask.FromResult(new List<AutoCompleteEntry>().ToAutoCompleteList());

            var enumType = typeof(HumanBodyBones);
            var serialized = Context.TypeRegistry.GetSerializedEnumType(enumType);
            var entries = serialized
                .members.Where(m => m.value != (long)HumanBodyBones.LastBone)
                .Select(m =>
                {
                    var bone = (HumanBodyBones)Enum.ToObject(enumType, m.value);
                    var enumName = bone.ToString();
                    var label = enumName.ToSpacedPascalCase();
                    return new AutoCompleteEntry { label = label, value = enumName };
                })
                .OrderBy(e => e.label, StringComparer.OrdinalIgnoreCase)
                .ToList();
            return UniTask.FromResult(entries.ToAutoCompleteList());
        }

        private static IEnumerable<HumanBodyBones> EnumerateLegacyBones(HumanBodyBones[] bones)
        {
            if (bones == null || bones.Length == 0)
                yield break;

            var seen = new HashSet<HumanBodyBones>();
            foreach (var bone in bones)
            {
                if (bone == HumanBodyBones.LastBone)
                    continue;
                if (!seen.Add(bone))
                    continue;
                yield return bone;
            }
        }

        private List<HumanBodyBones> ResolveTargetBones()
        {
            var fromStrings = EnumerateResolvedBoneNames(BoneNames).ToList();
            if (fromStrings.Count > 0)
                return fromStrings;

            var legacy = EnumerateLegacyBones(Bones).ToList();
            return legacy;
        }

        private static IEnumerable<HumanBodyBones> EnumerateResolvedBoneNames(string[] boneNames)
        {
            if (boneNames == null)
                yield break;

            var seen = new HashSet<HumanBodyBones>();
            foreach (var raw in boneNames)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                if (!Enum.TryParse<HumanBodyBones>(raw.Trim(), ignoreCase: true, out var bone))
                    continue;
                if (bone == HumanBodyBones.LastBone)
                    continue;
                if (!seen.Add(bone))
                    continue;
                yield return bone;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyShareSuffixToDisplayName();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyShareSuffixToDisplayName();
            if (EnumerateResolvedBoneNames(BoneNames).Any())
                return;

            var legacyNames = EnumerateLegacyBones(Bones).Select(b => b.ToString()).ToArray();
            if (legacyNames.Length == 0)
                return;

            SetDataInput(nameof(BoneNames), legacyNames, broadcast: true);
            SetDataInput(nameof(Bones), Array.Empty<HumanBodyBones>());
        }

        private const string ShareDisplayNameSuffix = " Shr";

        /// <summary>
        /// 쉐어 빌드: 카테고리는 📁Node68 Share, 캔버스 노드 이름은 <c> Shr</c> 접미사.
        /// </summary>
        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Bone Group Scale Node68";

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

        [FlowInput]
        public Continuation Enter()
        {
            ApplyBatch();
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

        private void ApplyBatch()
        {
            _sequence?.Kill(false);
            _sequence = null;

            List<HumanBodyBones> bones = ResolveTargetBones();
            var scaleVec = Scale;
            var transitionTime = TransitionTime;
            var transitionEasing = TransitionEasing;

            if (Character == null || bones.Count == 0)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var map = Character.HumanBodyBoneToBodyTransforms;
            if (map == null || map.Count == 0)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var duration = Mathf.Max(0f, transitionTime);

            if (duration <= 0f)
            {
                foreach (var bone in bones)
                {
                    if (!map.TryGetValue(bone, out var tr) || tr == null)
                        continue;
                    tr.localScale = scaleVec;
                }

                InvokeFlow(nameof(OnTransitionEnd));
                return;
            }

            var seq = DOTween.Sequence();
            var any = false;

            foreach (var bone in bones)
            {
                if (!map.TryGetValue(bone, out var tr) || tr == null)
                    continue;

                var tween = tr.DOScale(scaleVec, duration).SetEase(transitionEasing);
                seq.Join(tween);
                any = true;
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
    }
}
