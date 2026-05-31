using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 피벗 오프셋 = 지정한 Transform의 로컬 위치.
    /// 「부모(씬 루트)」는 에디터 지즈모가 붙는 쪽과 같을 때가 많습니다. 「메인」은 모델 앵커입니다.
    /// </summary>
    public enum CharacterPivotTransformTarget
    {
        /// <summary>Character.ParentTransform — 씬에서 이동·회전되는 루트에 가깝습니다.</summary>
        [Label("부모 트랜스폼 (씬 루트 · 지즈모 기준 추천)")]
        ParentSceneRoot = 0,

        /// <summary>Character.MainTransform — 리그/모델 쪽 메인 앵커.</summary>
        [Label("메인 트랜스폼 (모델 앵커)")]
        MainModelAnchor = 1,
    }

    [NodeType(
        Id = "72c4eb91-53f9-4312-b8a8-1194f2c6d881",
        Title = "Character Pivot Offset Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1f
    )]
    public sealed class SetCharacterMainTransformPivotOffsetNode : Node
    {
        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataInput]
        [Label("캐릭터")]
        public CharacterAsset Character;

        /// <summary>적용 대상: 지즈모로 보이는 위치와 맞추려면 보통 「부모」.</summary>
        [DataInput]
        [Label("어디에 적용할지")]
        [HiddenIf(nameof(HideInShareBuild))]
        public CharacterPivotTransformTarget ApplyTo =
            CharacterPivotTransformTarget.ParentSceneRoot;

        [DataInput]
        [Label("로컬 위치 (피벗 오프셋)")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Vector3 MainLocalPosition = Vector3.zero;

        [DataInput]
        [Label("트랜지션 시간 (초, 0이면 즉시)")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float TransitionTime;

        [DataInput]
        [Label("트랜지션 이징")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Ease TransitionEasing = Ease.OutCubic;

        /// <summary>
        /// 체크 시 그래프가 켜져 있는 동안 매 LateUpdate 로 값을 즉시 유지합니다.
        /// 「값 유지」가 켜져 있으면 트랜지션은 사용하지 않습니다.
        /// </summary>
        [DataInput]
        [Label("값 유지 매 프레임 적용")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool MaintainEveryFrame;

        private bool _pendingInstantApplyAfterSystems;
        private bool _pendingTweenNextLateUpdate;
        private Tweener _pivotTween;

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
                baseName = "Character Pivot Offset Node68";

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

        private void GetEffectivePivotInputs(
            out CharacterPivotTransformTarget applyTo,
            out Vector3 mainLocalPosition,
            out float transitionTime,
            out Ease transitionEasing,
            out bool maintainEveryFrame
        )
        {
            applyTo = ApplyTo;
            mainLocalPosition = MainLocalPosition;
            transitionTime = TransitionTime;
            transitionEasing = TransitionEasing;
            maintainEveryFrame = MaintainEveryFrame;
        }

        private Transform ResolveTarget(CharacterPivotTransformTarget applyTo)
        {
            if (Character == null)
                return null;

            if (applyTo == CharacterPivotTransformTarget.ParentSceneRoot)
            {
                var p = Character.ParentTransform;
                if (p != null)
                    return p;
            }

            return Character.MainTransform;
        }

        private void ApplyPivotImmediate(
            Vector3 localPosition,
            CharacterPivotTransformTarget applyTo
        )
        {
            var tr = ResolveTarget(applyTo);
            if (tr != null)
                tr.localPosition = localPosition;
        }

        private void KillActiveTween()
        {
            _pivotTween?.Kill(false);
            _pivotTween = null;
        }

        [FlowInput]
        public Continuation Enter()
        {
            KillActiveTween();

            GetEffectivePivotInputs(
                out var applyTo,
                out _,
                out var transitionTime,
                out _,
                out var maintainEveryFrame
            );

            if (maintainEveryFrame)
                return Exit;

            var tr = ResolveTarget(applyTo);
            if (tr == null)
            {
                InvokeFlow(nameof(OnTransitionEnd));
                return Exit;
            }

            var duration = Mathf.Max(0f, transitionTime);
            if (duration <= 0f)
                _pendingInstantApplyAfterSystems = true;
            else
                _pendingTweenNextLateUpdate = true;

            return Exit;
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            GetEffectivePivotInputs(
                out var applyTo,
                out var mainLocalPosition,
                out var transitionTime,
                out var transitionEasing,
                out var maintainEveryFrame
            );

            if (maintainEveryFrame)
            {
                KillActiveTween();
                _pendingInstantApplyAfterSystems = false;
                _pendingTweenNextLateUpdate = false;
                ApplyPivotImmediate(mainLocalPosition, applyTo);
                return;
            }

            if (_pendingTweenNextLateUpdate)
            {
                _pendingTweenNextLateUpdate = false;
                var tr = ResolveTarget(applyTo);
                if (tr == null)
                {
                    InvokeFlow(nameof(OnTransitionEnd));
                    return;
                }

                var duration = Mathf.Max(0f, transitionTime);
                if (duration <= 0f)
                {
                    ApplyPivotImmediate(mainLocalPosition, applyTo);
                    InvokeFlow(nameof(OnTransitionEnd));
                    return;
                }

                _pivotTween = tr
                    .DOLocalMove(mainLocalPosition, duration)
                    .SetEase(transitionEasing)
                    .OnComplete(() =>
                    {
                        _pivotTween = null;
                        InvokeFlow(nameof(OnTransitionEnd));
                    });

                return;
            }

            if (_pendingInstantApplyAfterSystems)
            {
                ApplyPivotImmediate(mainLocalPosition, applyTo);
                _pendingInstantApplyAfterSystems = false;
                InvokeFlow(nameof(OnTransitionEnd));
            }
        }

        protected override void OnDestroy()
        {
            KillActiveTween();
            base.OnDestroy();
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("트랜지션 종료 시")]
        public Continuation OnTransitionEnd;
    }
}
