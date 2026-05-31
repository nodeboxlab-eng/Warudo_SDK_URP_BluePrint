using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    [NodeType(
        Id = "b2e4c6d8-0a1b-4c3d-9e5f-708192a3b4c5",
        Title = "Text Display Animate Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.38f
    )]
    public sealed class TextDisplayAnimateNode68 : Node
    {
        /// <summary>프리셋 enum 확장 시 올려 Warudo 가 직렬화 버전을 구분합니다.</summary>
        public override long GetVersion() => 2;

        private bool HideInShareBuild() => CustomAssetsBuildRuntime.IsShareBuild();

        [DataInput]
        [Label("TextDisplay Node68")]
        public CharacterBoneAttachedTextAsset Display;

        [DataInput]
        [Label("먼저 텍스트 바꾸기")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool SetBodyText;

        [DataInput]
        [Label("새 내용")]
        [MultilineInput]
        [HiddenIf(nameof(HideBodyTextField))]
        public string NewBodyText = "";

        [DataInput]
        [Label("애니 프리셋")]
        [HiddenIf(nameof(HideInShareBuild))]
        public TextDisplayAnimatePreset68 Preset = TextDisplayAnimatePreset68.SlideUpFadeIn;

        [DataInput]
        [Label("길이 (초)")]
        [FloatSlider(0.02f, 5f)]
        [HiddenIf(nameof(HideInShareBuild))]
        public float Duration = 0.45f;

        [DataInput]
        [Label("이징 (DOTween)")]
        [HiddenIf(nameof(HideInShareBuild))]
        public Ease Easing = Ease.OutCubic;

        [DataInput]
        [Label("슬라이드 거리 (본 로컬)")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "슬라이드류 프리셋에서만 사용 (미터 단위 근사)."
        )]
        [FloatSlider(0.02f, 1.5f)]
        [HiddenIf(nameof(HideInShareBuild))]
        public float SlideDistance = 0.14f;

        [DataInput]
        [Label("팝 인 시작 스케일 비율")]
        [FloatSlider(0.05f, 0.95f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "팝 인/아웃 시 곱해 넣는 최소 스케일 비율.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float PopScaleStart = 0.22f;

        [DataInput]
        [Label("스핀·큐브 회전 바퀴 (Y)")]
        [FloatSlider(0.25f, 4f)]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "스핀·큐브 프리셋에서 Y 축 회전량 배율 (1≈한 바퀴)."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public float SpinTurns = 1f;

        [DataInput]
        [Label("튀기기 세기")]
        [FloatSlider(0f, 2.5f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "튀기기 프리셋에서 슬라이드 거리에 곱하는 위아래 튐.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float BounceStrength = 1f;

        [DataInput]
        [Label("타자기 단위")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "타자기 인/아웃 프리셋에서 글자 또는 단어 단위로 잘라 냅니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public TextDisplayTypewriterGranularity68 TypewriterGranularity =
            TextDisplayTypewriterGranularity68.Character;

        [DataInput]
        [Label("컬러 플래시 세기")]
        [FloatSlider(0f, 2f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "컬러 플래시 프리셋에서 중간에 RGB 를 키우는 배율.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public float ColorFlashIntensity = 0.85f;

        [DataInput]
        [Label("시작 전 표시 켜기")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "켜면 Enter 에서 Visible 을 true 로 둡니다 (잠깐 메시지용)."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool EnsureVisibleOnStart = true;

        [DataInput]
        [Label("종료 후 동작")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "애니가 끝난 뒤 에셋을 숨기거나 비활성할 수 있습니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public TextDisplayAnimateEndAction68 EndAction = TextDisplayAnimateEndAction68.None;

        private Tween _tweenAnim;

        private bool HideBodyTextField() => HideInShareBuild() || !SetBodyText;

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

        protected override void OnDestroy()
        {
            KillTween();
            base.OnDestroy();
        }

        private const string ShareDisplayNameSuffix = " Shr";

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Text Display Animate Node68";

            if (CustomAssetsBuildRuntime.IsShareBuild())
            {
                var core = string.IsNullOrEmpty(Name)
                    ? baseName
                    : Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
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
            PlayAnimation();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("애니 종료 시")]
        public Continuation OnAnimationComplete;

        private void KillTween()
        {
            if (_tweenAnim != null && _tweenAnim.IsActive())
                _tweenAnim.Kill(false);
            _tweenAnim = null;
        }

        private void PlayAnimation()
        {
            KillTween();

            if (Display == null)
            {
                InvokeFlow(nameof(OnAnimationComplete));
                return;
            }

            if (SetBodyText)
                Display.SetDataInput(
                    nameof(CharacterBoneAttachedTextAsset.BodyText),
                    NewBodyText ?? "",
                    true
                );

            if (EnsureVisibleOnStart)
                Display.SetDataInput(nameof(CharacterBoneAttachedTextAsset.Visible), true, true);

            Display.ClearTextDisplayOverlayAnimation();

            var dur = Mathf.Max(0.02f, Duration);
            var slide = SlideDistance;
            var popLo = Mathf.Clamp(PopScaleStart, 0.05f, 0.95f);
            var preset = Preset;
            var ease = Easing;
            var target = Display;

            var bodyForTw = SetBodyText ? (NewBodyText ?? "") : (target.BodyText ?? "");

            float p = 0f;
            Node68AnimateOverlaySamples68.ApplyOverlay(
                target,
                preset,
                0f,
                slide,
                popLo,
                SpinTurns,
                BounceStrength,
                TypewriterGranularity,
                ColorFlashIntensity,
                bodyForTw
            );

            _tweenAnim = DOTween
                .To(() => p, x => p = x, 1f, dur)
                .SetEase(ease)
                .OnUpdate(() =>
                    Node68AnimateOverlaySamples68.ApplyOverlay(
                        target,
                        preset,
                        p,
                        slide,
                        popLo,
                        SpinTurns,
                        BounceStrength,
                        TypewriterGranularity,
                        ColorFlashIntensity,
                        bodyForTw
                    )
                )
                .OnKill(() => _tweenAnim = null)
                .OnComplete(() =>
                {
                    _tweenAnim = null;
                    target.ClearTextDisplayOverlayAnimation();
                    ApplyEndAction(target);
                    InvokeFlow(nameof(OnAnimationComplete));
                });
        }

        private void ApplyEndAction(CharacterBoneAttachedTextAsset d)
        {
            switch (EndAction)
            {
                case TextDisplayAnimateEndAction68.HideVisible:
                    d.SetDataInput(nameof(CharacterBoneAttachedTextAsset.Visible), false, true);
                    break;
                case TextDisplayAnimateEndAction68.DisableAsset:
                    d.SetDataInput("Enabled", false, true);
                    break;
            }
        }
    }
}
