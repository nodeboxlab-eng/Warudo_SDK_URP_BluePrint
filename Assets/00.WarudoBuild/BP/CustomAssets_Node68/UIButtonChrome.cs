using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UICanvasRenderer 버튼 배경(Bg)만 (테두리 Border 레이어 없음)
/// </summary>
public static class UIButtonChrome
{
    public const string BorderName = "Border";
    public const string BgName = "Bg";

    public static void CreateBorderAndBg(
        Transform parent,
        Sprite roundedSprite,
        bool stateOn,
        AccentColor accent,
        out Image borderImage,
        out Image bgImage
    )
    {
        borderImage = null;

        var bgGo = new GameObject(BgName);
        bgGo.transform.SetParent(parent, false);
        var bgRt = bgGo.AddComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;

        bgImage = bgGo.AddComponent<Image>();
        bgImage.sprite = roundedSprite;
        bgImage.type = Image.Type.Sliced;
        bgImage.color = UITheme.ButtonBgForState(stateOn, accent);
        bgImage.raycastTarget = true;
        bgImage.preserveAspect = false;
        bgImage.maskable = true;
    }

    public static void ApplyBorderAndBgColors(
        Image borderImage,
        Image bgImage,
        bool stateOn,
        AccentColor accent
    )
    {
        if (borderImage != null)
            borderImage.color = UITheme.ButtonBorderStrokeColor(stateOn, accent);
        if (bgImage != null)
            bgImage.color = UITheme.ButtonBgForState(stateOn, accent);
    }
}
