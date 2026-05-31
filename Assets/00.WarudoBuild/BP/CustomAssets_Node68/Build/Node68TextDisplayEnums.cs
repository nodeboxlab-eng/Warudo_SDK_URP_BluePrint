using Warudo.Core.Attributes;

namespace Node68.CustomAssets
{
    /// <summary>TextDisplay Node68 위치·회전 좌표계.</summary>
    public enum TextDisplayTransformSpace68
    {
        [Label("부착 로컬 (본·에셋)")]
        AttachLocal = 0,

        [Label("월드")]
        World = 1,
    }

    /// <summary>TextDisplay Node68 위치·스케일 애니메이션 곡선.</summary>
    public enum TextDisplayMotionShape
    {
        [Label("사인 (연속 부드러운 루프)")]
        Sine = 0,

        [Label("이징 핑퐁 (끝에서 살살 멈추는 느낌)")]
        SmoothPingPong = 1,
    }

    public enum TextDisplayTypewriterGranularity68
    {
        [Label("글자 단위")]
        Character = 0,

        [Label("단어 단위")]
        Word = 1,

        [Label("줄 단위")]
        Line = 2,
    }

    public enum TextDisplayShowHideIntent68
    {
        [Label("보이기 (Visible·Enabled 켜기)")]
        Show = 0,

        [Label("숨기기")]
        Hide = 1,
    }

    public enum TextDisplayHideAfter68
    {
        [Label("표시만 끄기 (Visible=false)")]
        VisibleOff = 0,

        [Label("에셋·오브젝트 끄기 (Enabled=false)")]
        EnabledOff = 1,

        [Label("Visible·Enabled 모두 끄기")]
        BothOff = 2,
    }

    public enum TextDisplayShowAnim68
    {
        [Label("페이드 인")]
        FadeIn = 0,

        [Label("슬라이드 업 + 페이드 인")]
        SlideUpFadeIn = 1,

        [Label("슬라이드 왼쪽 + 페이드 인")]
        SlideLeftFadeIn = 2,

        [Label("슬라이드 오른쪽 + 페이드 인")]
        SlideRightFadeIn = 3,

        [Label("팝 인")]
        PopIn = 4,

        [Label("슬라이드 다운 + 페이드 인")]
        SlideDownFadeIn = 5,

        [Label("뒤집기 인 (X)")]
        FlipInX = 6,

        [Label("스핀 인 (Y)")]
        SpinInY = 7,

        [Label("큐브 인")]
        CubeIn = 8,

        [Label("튀기기 인")]
        BounceIn = 9,

        [Label("타자기 인")]
        TypewriterIn = 10,

        [Label("컬러 플래시 인")]
        ColorFlashIn = 11,
    }

    public enum TextDisplayHideAnim68
    {
        [Label("페이드 아웃")]
        FadeOut = 0,

        [Label("슬라이드 다운 + 페이드 아웃")]
        SlideDownFadeOut = 1,

        [Label("팝 아웃")]
        PopOut = 2,

        [Label("슬라이드 업 + 페이드 아웃")]
        SlideUpFadeOut = 3,

        [Label("슬라이드 왼쪽 + 페이드 아웃")]
        SlideLeftFadeOut = 4,

        [Label("슬라이드 오른쪽 + 페이드 아웃")]
        SlideRightFadeOut = 5,

        [Label("뒤집기 아웃 (X)")]
        FlipOutX = 6,

        [Label("스핀 아웃 (Y)")]
        SpinOutY = 7,

        [Label("큐브 아웃")]
        CubeOut = 8,

        [Label("튀기기 아웃")]
        BounceOut = 9,

        [Label("타자기 아웃")]
        TypewriterOut = 10,

        [Label("컬러 플래시 아웃")]
        ColorFlashOut = 11,
    }

    public enum TextDisplayAnimatePreset68
    {
        [Label("페이드 인")]
        FadeIn = 0,

        [Label("페이드 아웃")]
        FadeOut = 1,

        [Label("팝 인 (작게→원래 + 페이드)")]
        PopIn = 2,

        [Label("팝 아웃")]
        PopOut = 3,

        [Label("슬라이드 업 + 페이드 인")]
        SlideUpFadeIn = 4,

        [Label("슬라이드 다운 + 페이드 아웃")]
        SlideDownFadeOut = 5,

        [Label("슬라이드 왼쪽 + 페이드 인")]
        SlideLeftFadeIn = 6,

        [Label("슬라이드 오른쪽 + 페이드 인")]
        SlideRightFadeIn = 7,

        [Label("슬라이드 다운 + 페이드 인")]
        SlideDownFadeIn = 8,

        [Label("슬라이드 업 + 페이드 아웃")]
        SlideUpFadeOut = 9,

        [Label("슬라이드 왼쪽 + 페이드 아웃")]
        SlideLeftFadeOut = 10,

        [Label("슬라이드 오른쪽 + 페이드 아웃")]
        SlideRightFadeOut = 11,

        [Label("뒤집기 인 (X)")]
        FlipInX = 12,

        [Label("뒤집기 아웃 (X)")]
        FlipOutX = 13,

        [Label("스핀 인 (Y)")]
        SpinInY = 14,

        [Label("스핀 아웃 (Y)")]
        SpinOutY = 15,

        [Label("큐브 인 (X+Y)")]
        CubeIn = 16,

        [Label("큐브 아웃")]
        CubeOut = 17,

        [Label("튀기기 인")]
        BounceIn = 18,

        [Label("튀기기 아웃")]
        BounceOut = 19,

        [Label("타자기 인")]
        TypewriterIn = 20,

        [Label("타자기 아웃")]
        TypewriterOut = 21,

        [Label("컬러 플래시 인")]
        ColorFlashIn = 22,

        [Label("컬러 플래시 아웃")]
        ColorFlashOut = 23,
    }

    public enum TextDisplayAnimateEndAction68
    {
        [Label("없음")]
        None = 0,

        [Label("표시 끄기 (Visible)")]
        HideVisible = 1,

        [Label("에셋 비활성 (Enabled)")]
        DisableAsset = 2,
    }
}
