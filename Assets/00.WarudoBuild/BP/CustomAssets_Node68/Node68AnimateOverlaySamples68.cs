using UnityEngine;

namespace Node68.CustomAssets
{
    internal static class Node68AnimateOverlaySamples68
    {
        internal static Color ColorFlashMultiplier(float p01, float intensity)
        {
            var k = Mathf.Max(0f, intensity);
            var x = 1f + k * Mathf.Sin(Mathf.Clamp01(p01) * Mathf.PI);
            return new Color(x, x, x, 1f);
        }

        internal static void ApplyOverlay(
            CharacterBoneAttachedTextAsset d,
            TextDisplayAnimatePreset68 preset,
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
            p01 = Mathf.Clamp01(p01);
            var off = Vector3.zero;
            var sc = Vector3.one;
            float a = 1f;
            var euler = Vector3.zero;
            var tw = -1;
            var cMul = Color.white;
            var st = Mathf.Max(0.05f, spinTurns);
            var bb = Mathf.Max(0f, bounceStrength);

            switch (preset)
            {
                case TextDisplayAnimatePreset68.FadeIn:
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.FadeOut:
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.PopIn:
                    a = p01;
                    var pin = Mathf.Lerp(popLo, 1f, p01);
                    sc = new Vector3(pin, pin, pin);
                    break;
                case TextDisplayAnimatePreset68.PopOut:
                    a = 1f - p01;
                    var pout = Mathf.Lerp(1f, popLo, p01);
                    sc = new Vector3(pout, pout, pout);
                    break;
                case TextDisplayAnimatePreset68.SlideUpFadeIn:
                    off = Vector3.Lerp(new Vector3(0f, -slide, 0f), Vector3.zero, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.SlideDownFadeOut:
                    off = Vector3.Lerp(Vector3.zero, new Vector3(0f, -slide, 0f), p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.SlideLeftFadeIn:
                    off = Vector3.Lerp(new Vector3(-slide, 0f, 0f), Vector3.zero, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.SlideRightFadeIn:
                    off = Vector3.Lerp(new Vector3(slide, 0f, 0f), Vector3.zero, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.SlideDownFadeIn:
                    off = Vector3.Lerp(new Vector3(0f, slide, 0f), Vector3.zero, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.SlideUpFadeOut:
                    off = Vector3.Lerp(Vector3.zero, new Vector3(0f, slide, 0f), p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.SlideLeftFadeOut:
                    off = Vector3.Lerp(Vector3.zero, new Vector3(-slide, 0f, 0f), p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.SlideRightFadeOut:
                    off = Vector3.Lerp(Vector3.zero, new Vector3(slide, 0f, 0f), p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.FlipInX:
                    euler.x = Mathf.Lerp(88f, 0f, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.FlipOutX:
                    euler.x = Mathf.Lerp(0f, -88f, p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.SpinInY:
                    euler.y = Mathf.Lerp(360f * st, 0f, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.SpinOutY:
                    euler.y = Mathf.Lerp(0f, 360f * st, p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.CubeIn:
                    euler.x = Mathf.Lerp(82f, 0f, p01);
                    euler.y = Mathf.Lerp(-72f * st, 0f, p01);
                    a = p01;
                    break;
                case TextDisplayAnimatePreset68.CubeOut:
                    euler.x = Mathf.Lerp(0f, 82f, p01);
                    euler.y = Mathf.Lerp(0f, -72f * st, p01);
                    a = 1f - p01;
                    break;
                case TextDisplayAnimatePreset68.BounceIn:
                    a = p01;
                    off.y =
                        bb
                        * slide
                        * Mathf.Abs(Mathf.Sin(p01 * Mathf.PI * 3.2f))
                        * (1f - 0.4f * p01);
                    off.y += bb * slide * 0.12f * Mathf.Sin(p01 * Mathf.PI * 6f);
                    break;
                case TextDisplayAnimatePreset68.BounceOut:
                    a = 1f - p01;
                    var q = 1f - p01;
                    off.y =
                        bb * slide * Mathf.Abs(Mathf.Sin(q * Mathf.PI * 3.2f)) * (1f - 0.4f * q);
                    off.y += bb * slide * 0.12f * Mathf.Sin(q * Mathf.PI * 6f);
                    break;
                case TextDisplayAnimatePreset68.TypewriterIn:
                    a = 1f;
                    tw = Node68TypewriterText68.VisibleCharsReveal(bodyText, p01, twGran);
                    break;
                case TextDisplayAnimatePreset68.TypewriterOut:
                    a = Mathf.Lerp(1f, 0.35f, p01);
                    tw = Node68TypewriterText68.VisibleCharsHide(bodyText, p01, twGran);
                    break;
                case TextDisplayAnimatePreset68.ColorFlashIn:
                    a = p01;
                    cMul = ColorFlashMultiplier(p01, colorFlash);
                    break;
                case TextDisplayAnimatePreset68.ColorFlashOut:
                    a = 1f - p01;
                    cMul = ColorFlashMultiplier(1f - p01, colorFlash);
                    break;
                default:
                    a = p01;
                    break;
            }

            d.SetTextDisplayOverlayAnimation(off, sc, a, euler, tw, cMul);
        }

        internal static TextDisplayAnimatePreset68 MapShowAnim(TextDisplayShowAnim68 s)
        {
            switch (s)
            {
                case TextDisplayShowAnim68.SlideUpFadeIn:
                    return TextDisplayAnimatePreset68.SlideUpFadeIn;
                case TextDisplayShowAnim68.SlideLeftFadeIn:
                    return TextDisplayAnimatePreset68.SlideLeftFadeIn;
                case TextDisplayShowAnim68.SlideRightFadeIn:
                    return TextDisplayAnimatePreset68.SlideRightFadeIn;
                case TextDisplayShowAnim68.PopIn:
                    return TextDisplayAnimatePreset68.PopIn;
                case TextDisplayShowAnim68.SlideDownFadeIn:
                    return TextDisplayAnimatePreset68.SlideDownFadeIn;
                case TextDisplayShowAnim68.FlipInX:
                    return TextDisplayAnimatePreset68.FlipInX;
                case TextDisplayShowAnim68.SpinInY:
                    return TextDisplayAnimatePreset68.SpinInY;
                case TextDisplayShowAnim68.CubeIn:
                    return TextDisplayAnimatePreset68.CubeIn;
                case TextDisplayShowAnim68.BounceIn:
                    return TextDisplayAnimatePreset68.BounceIn;
                case TextDisplayShowAnim68.TypewriterIn:
                    return TextDisplayAnimatePreset68.TypewriterIn;
                case TextDisplayShowAnim68.ColorFlashIn:
                    return TextDisplayAnimatePreset68.ColorFlashIn;
                default:
                    return TextDisplayAnimatePreset68.FadeIn;
            }
        }

        internal static TextDisplayAnimatePreset68 MapHideAnim(TextDisplayHideAnim68 h)
        {
            switch (h)
            {
                case TextDisplayHideAnim68.SlideDownFadeOut:
                    return TextDisplayAnimatePreset68.SlideDownFadeOut;
                case TextDisplayHideAnim68.PopOut:
                    return TextDisplayAnimatePreset68.PopOut;
                case TextDisplayHideAnim68.SlideUpFadeOut:
                    return TextDisplayAnimatePreset68.SlideUpFadeOut;
                case TextDisplayHideAnim68.SlideLeftFadeOut:
                    return TextDisplayAnimatePreset68.SlideLeftFadeOut;
                case TextDisplayHideAnim68.SlideRightFadeOut:
                    return TextDisplayAnimatePreset68.SlideRightFadeOut;
                case TextDisplayHideAnim68.FlipOutX:
                    return TextDisplayAnimatePreset68.FlipOutX;
                case TextDisplayHideAnim68.SpinOutY:
                    return TextDisplayAnimatePreset68.SpinOutY;
                case TextDisplayHideAnim68.CubeOut:
                    return TextDisplayAnimatePreset68.CubeOut;
                case TextDisplayHideAnim68.BounceOut:
                    return TextDisplayAnimatePreset68.BounceOut;
                case TextDisplayHideAnim68.TypewriterOut:
                    return TextDisplayAnimatePreset68.TypewriterOut;
                case TextDisplayHideAnim68.ColorFlashOut:
                    return TextDisplayAnimatePreset68.ColorFlashOut;
                default:
                    return TextDisplayAnimatePreset68.FadeOut;
            }
        }
    }
}
