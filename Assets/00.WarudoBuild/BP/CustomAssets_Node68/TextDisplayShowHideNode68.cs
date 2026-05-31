using DG.Tweening;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    /// <summary>
    /// 보이기/숨기기 시 오버레이 애니(페이드·슬라이드·팝). 끄면 Toggle 과 같이 즉시 반영됩니다.
    /// Warudo 기본 Toggle 은 GameObject 를 바로 비활성해 페이드 없이 바로 사라집니다.
    /// </summary>
    [NodeType(
        Id = "c3f5a7b9-1d2e-4f5a-8c0b-819293a4d6e7",
        Title = "Text Display Show/Hide Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.32f
    )]
    public sealed class TextDisplayShowHideNode68 : Node
    {
        public override long GetVersion() => 2;

        private bool HideInShareBuild() => CustomAssetsBuildRuntime.IsShareBuild();

        [DataInput]
        [Label("TextDisplay Node68")]
        public CharacterBoneAttachedTextAsset Display;

        [DataInput]
        [Label("동작")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "「보이기」는 항상 Visible 과 Enabled 를 켭니다. 아래 「숨긴 뒤」 목록은 「숨기기」일 때만 나타납니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public TextDisplayShowHideIntent68 Intent = TextDisplayShowHideIntent68.Hide;

        [DataInput]
        [Label("등장·퇴장 애니")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "끄면 보이기/숨기기 모두 즉시. 켜면 아래 프리셋·시간·이징으로 재생합니다 (TMP 오버레이)."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool UseFade = true;

        [DataInput]
        [Label("보일 때")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "동작이 「보이기」일 때만 사용됩니다.")]
        [HiddenIf(nameof(HideShowAnimField))]
        public TextDisplayShowAnim68 ShowAnim = TextDisplayShowAnim68.FadeIn;

        [DataInput]
        [Label("숨길 때")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "동작이 「숨기기」일 때만 사용됩니다.")]
        [HiddenIf(nameof(HideHideAnimField))]
        public TextDisplayHideAnim68 HideAnim = TextDisplayHideAnim68.FadeOut;

        [DataInput]
        [Label("슬라이드 거리 (본 로컬)")]
        [FloatSlider(0.02f, 1.5f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "슬라이드류 프리셋에서만 사용.")]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public float SlideDistance = 0.14f;

        [DataInput]
        [Label("팝 스케일 시작 비율")]
        [FloatSlider(0.05f, 0.95f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "팝 인/아웃 프리셋에서만 사용.")]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public float PopScaleStart = 0.22f;

        [DataInput]
        [Label("스핀·큐브 회전 바퀴 (Y)")]
        [FloatSlider(0.25f, 4f)]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public float SpinTurns = 1f;

        [DataInput]
        [Label("튀기기 세기")]
        [FloatSlider(0f, 2.5f)]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public float BounceStrength = 1f;

        [DataInput]
        [Label("타자기 단위")]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public TextDisplayTypewriterGranularity68 TypewriterGranularity =
            TextDisplayTypewriterGranularity68.Character;

        [DataInput]
        [Label("컬러 플래시 세기")]
        [FloatSlider(0f, 2f)]
        [HiddenIf(nameof(HideAnimPresetFields))]
        public float ColorFlashIntensity = 0.85f;

        [DataInput]
        [Label("애니 길이 (초)")]
        [FloatSlider(0.02f, 4f)]
        [HiddenIf(nameof(HideFadeFields))]
        public float FadeDuration = 0.35f;

        [DataInput]
        [Label("이징")]
        [HiddenIf(nameof(HideFadeFields))]
        public Ease FadeEasing = Ease.InOutQuad;

        [DataInput]
        [Label("숨긴 뒤")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "숨기기일 때만 사용합니다. 목록은 전부 「끄는」 방식입니다. 켜기(활성화)는 위 「동작」을 「보이기」로 두면 됩니다 (Enabled·Visible 모두 true)."
        )]
        [HiddenIf(nameof(HideAfterFields))]
        public TextDisplayHideAfter68 AfterHide = TextDisplayHideAfter68.VisibleOff;

        private Tween _tween;

        private bool HideFadeFields() => HideInShareBuild() || !UseFade;

        private bool HideAnimPresetFields() => HideInShareBuild() || !UseFade;

        private bool HideShowAnimField() =>
            HideInShareBuild() || !UseFade || Intent != TextDisplayShowHideIntent68.Show;

        private bool HideHideAnimField() =>
            HideInShareBuild() || !UseFade || Intent != TextDisplayShowHideIntent68.Hide;

        private bool HideAfterFields() =>
            HideInShareBuild() || Intent != TextDisplayShowHideIntent68.Hide;

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
                baseName = "Text Display Show/Hide Node68";

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

        private void KillTween()
        {
            if (_tween != null && _tween.IsActive())
                _tween.Kill(false);
            _tween = null;
        }

        [FlowInput]
        public Continuation Enter()
        {
            KillTween();

            if (Display == null)
            {
                InvokeFlow(nameof(OnFinished));
                return Exit;
            }

            if (Intent == TextDisplayShowHideIntent68.Show)
                RunShow();
            else
                RunHide();

            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("처리 끝 (애니·즉시)")]
        public Continuation OnFinished;

        private void RunShow()
        {
            Display.SetDataInput("Enabled", true, true);
            Display.SetDataInput(nameof(CharacterBoneAttachedTextAsset.Visible), true, true);
            Display.ClearTextDisplayOverlayAnimation();

            if (!UseFade)
            {
                InvokeFlow(nameof(OnFinished));
                return;
            }

            var dur = Mathf.Max(0.02f, FadeDuration);
            var ease = FadeEasing;
            var target = Display;
            var slide = SlideDistance;
            var popLo = Mathf.Clamp(PopScaleStart, 0.05f, 0.95f);
            var preset = ShowAnim;
            var bodyTw = target.BodyText ?? "";

            SampleShowOverlay(
                target,
                preset,
                0f,
                slide,
                popLo,
                SpinTurns,
                BounceStrength,
                TypewriterGranularity,
                ColorFlashIntensity,
                bodyTw
            );

            float p = 0f;
            _tween = DOTween
                .To(() => p, x => p = x, 1f, dur)
                .SetEase(ease)
                .OnUpdate(() =>
                    SampleShowOverlay(
                        target,
                        preset,
                        p,
                        slide,
                        popLo,
                        SpinTurns,
                        BounceStrength,
                        TypewriterGranularity,
                        ColorFlashIntensity,
                        bodyTw
                    )
                )
                .OnKill(() => _tween = null)
                .OnComplete(() =>
                {
                    _tween = null;
                    target.ClearTextDisplayOverlayAnimation();
                    InvokeFlow(nameof(OnFinished));
                });
        }

        private void RunHide()
        {
            if (!UseFade)
            {
                Display.ClearTextDisplayOverlayAnimation();
                ApplyAfterHide(Display);
                InvokeFlow(nameof(OnFinished));
                return;
            }

            var dur = Mathf.Max(0.02f, FadeDuration);
            var ease = FadeEasing;
            var target = Display;
            var slide = SlideDistance;
            var popLo = Mathf.Clamp(PopScaleStart, 0.05f, 0.95f);
            var preset = HideAnim;
            var bodyTw = target.BodyText ?? "";

            SampleHideOverlay(
                target,
                preset,
                0f,
                slide,
                popLo,
                SpinTurns,
                BounceStrength,
                TypewriterGranularity,
                ColorFlashIntensity,
                bodyTw
            );

            float p = 0f;
            _tween = DOTween
                .To(() => p, x => p = x, 1f, dur)
                .SetEase(ease)
                .OnUpdate(() =>
                    SampleHideOverlay(
                        target,
                        preset,
                        p,
                        slide,
                        popLo,
                        SpinTurns,
                        BounceStrength,
                        TypewriterGranularity,
                        ColorFlashIntensity,
                        bodyTw
                    )
                )
                .OnKill(() => _tween = null)
                .OnComplete(() =>
                {
                    _tween = null;
                    target.ClearTextDisplayOverlayAnimation();
                    ApplyAfterHide(target);
                    InvokeFlow(nameof(OnFinished));
                });
        }

        private static void SampleShowOverlay(
            CharacterBoneAttachedTextAsset d,
            TextDisplayShowAnim68 preset,
            float p01,
            float slide,
            float popLo,
            float spinTurns,
            float bounceStrength,
            TextDisplayTypewriterGranularity68 twGran,
            float colorFlash,
            string bodyText
        )
        {
            var ap = Node68AnimateOverlaySamples68.MapShowAnim(preset);
            Node68AnimateOverlaySamples68.ApplyOverlay(
                d,
                ap,
                p01,
                slide,
                popLo,
                spinTurns,
                bounceStrength,
                twGran,
                colorFlash,
                bodyText
            );
        }

        private static void SampleHideOverlay(
            CharacterBoneAttachedTextAsset d,
            TextDisplayHideAnim68 preset,
            float p01,
            float slide,
            float popLo,
            float spinTurns,
            float bounceStrength,
            TextDisplayTypewriterGranularity68 twGran,
            float colorFlash,
            string bodyText
        )
        {
            var ap = Node68AnimateOverlaySamples68.MapHideAnim(preset);
            Node68AnimateOverlaySamples68.ApplyOverlay(
                d,
                ap,
                p01,
                slide,
                popLo,
                spinTurns,
                bounceStrength,
                twGran,
                colorFlash,
                bodyText
            );
        }

        private void ApplyAfterHide(CharacterBoneAttachedTextAsset d)
        {
            switch (AfterHide)
            {
                case TextDisplayHideAfter68.VisibleOff:
                    d.SetDataInput(nameof(CharacterBoneAttachedTextAsset.Visible), false, true);
                    break;
                case TextDisplayHideAfter68.EnabledOff:
                    d.SetDataInput("Enabled", false, true);
                    break;
                case TextDisplayHideAfter68.BothOff:
                    d.SetDataInput(nameof(CharacterBoneAttachedTextAsset.Visible), false, true);
                    d.SetDataInput("Enabled", false, true);
                    break;
            }
        }
    }
}
