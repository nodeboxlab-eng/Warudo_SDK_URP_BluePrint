using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼형 면과 구분되는 스위치 토글 시각 (직사각 트랙 + 사각 노브 + 라벨).
/// </summary>
public static class UIToggleSwitchChrome
{
    public const string LabelName = "SwitchLabel";
    public const string HitAreaName = "SwitchHitArea";
    public const string TrackName = "SwitchTrack";
    public const string KnobName = "SwitchKnob";

    public const float HitWidth = 54f;
    public const float HitHeight = 26f;
    public const float KnobSize = 18f;
    public const float KnobPadding = 2f;
    public const float SwitchRightInset = 10f;

    static Sprite _squareSprite;

    /// <summary>모서리 없는 단색 사각 스프라이트 — 트랙·노브 공통 (네모 형태)</summary>
    public static Sprite GetSquareSprite()
    {
        if (_squareSprite != null)
            return _squareSprite;

        const int n = 8;
        var tex = new Texture2D(n, n, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
                tex.SetPixel(x, y, Color.white);
        }
        tex.Apply();

        _squareSprite = Sprite.Create(
            tex,
            new Rect(0, 0, n, n),
            new Vector2(0.5f, 0.5f),
            100f,
            0,
            SpriteMeshType.FullRect
        );
        return _squareSprite;
    }

    public static Color TrackOffColor(ThemeMode mode)
    {
        return mode == ThemeMode.Dark
            ? new Color(0.28f, 0.28f, 0.33f, 1f)
            : new Color(0.82f, 0.84f, 0.88f, 1f);
    }

    public static Color TrackOnColor(AccentColor accent)
    {
        var c = UITheme.ButtonStateOnColor(accent);
        return new Color(c.r, c.g, c.b, 0.98f);
    }

    public static void ApplyTrackAndKnob(
        Image trackImage,
        RectTransform knobRt,
        bool isOn,
        AccentColor accent,
        ThemeMode theme
    )
    {
        if (trackImage != null)
            trackImage.color = isOn ? TrackOnColor(accent) : TrackOffColor(theme);
        if (knobRt != null)
            ApplyKnobAnchoredX(knobRt, HitWidth, isOn);
    }

    /// <summary>히트 영역 가로가 HitWidth와 다를 때는 knobRt 의 너비를 쓴다.</summary>
    public static void ApplyKnobAnchoredX(RectTransform knobRt, float hitWidth, bool isOn)
    {
        if (knobRt == null)
            return;
        float w = knobRt.rect.width > 1f ? knobRt.rect.width : KnobSize;
        float pad = KnobPadding;
        float xLeft = pad + w * 0.5f;
        float xRight = hitWidth - pad - w * 0.5f;
        knobRt.anchoredPosition = new Vector2(isOn ? xRight : xLeft, 0f);
    }

    /// <summary>
    /// 생성 직후 호출 — 라벨은 왼쪽, 오른쪽에 스위치. WarudoUIToggle 은 Track 의 Image 에 붙입니다.
    /// </summary>
    public static Image BuildSwitchRow(
        Transform root,
        bool isOn,
        AccentColor accent,
        ThemeMode theme,
        Font font,
        string labelText,
        int fontSize,
        out RectTransform knobRt
    )
    {
        knobRt = null;
        var quad = GetSquareSprite();

        var labelGo = new GameObject(LabelName);
        labelGo.transform.SetParent(root, false);
        var labelRt = labelGo.AddComponent<RectTransform>();
        labelRt.anchorMin = Vector2.zero;
        labelRt.anchorMax = Vector2.one;
        labelRt.offsetMin = new Vector2(12f, 4f);
        labelRt.offsetMax = new Vector2(-(HitWidth + SwitchRightInset + 4f), -4f);

        var label = labelGo.AddComponent<Text>();
        label.font = font;
        label.text = string.IsNullOrEmpty(labelText) ? "토글" : labelText;
        label.fontSize = fontSize;
        label.alignment = TextAnchor.MiddleLeft;
        label.horizontalOverflow = HorizontalWrapMode.Wrap;
        label.verticalOverflow = VerticalWrapMode.Truncate;
        label.raycastTarget = false;
        label.supportRichText = false;
        label.color = UITheme.TextPrimary(theme);

        var outline = labelGo.AddComponent<Outline>();
        outline.effectColor = UITheme.TextOutlineEffectColor(theme);
        outline.effectDistance = new Vector2(1, -1);

        var hitGo = new GameObject(HitAreaName);
        hitGo.transform.SetParent(root, false);
        var hitRt = hitGo.AddComponent<RectTransform>();
        hitRt.anchorMin = new Vector2(1f, 0.5f);
        hitRt.anchorMax = new Vector2(1f, 0.5f);
        hitRt.pivot = new Vector2(1f, 0.5f);
        hitRt.sizeDelta = new Vector2(HitWidth, HitHeight);
        hitRt.anchoredPosition = new Vector2(-SwitchRightInset, 0f);

        var trackGo = new GameObject(TrackName);
        trackGo.transform.SetParent(hitGo.transform, false);
        var trackRt = trackGo.AddComponent<RectTransform>();
        trackRt.anchorMin = Vector2.zero;
        trackRt.anchorMax = Vector2.one;
        trackRt.offsetMin = Vector2.zero;
        trackRt.offsetMax = Vector2.zero;

        var trackImage = trackGo.AddComponent<Image>();
        trackImage.sprite = quad;
        trackImage.type = Image.Type.Simple;
        trackImage.raycastTarget = true;
        trackImage.preserveAspect = false;

        var knobGo = new GameObject(KnobName);
        knobGo.transform.SetParent(hitGo.transform, false);
        knobRt = knobGo.AddComponent<RectTransform>();
        knobRt.anchorMin = new Vector2(0f, 0.5f);
        knobRt.anchorMax = new Vector2(0f, 0.5f);
        knobRt.pivot = new Vector2(0.5f, 0.5f);
        knobRt.sizeDelta = new Vector2(KnobSize, KnobSize);

        var knobImg = knobGo.AddComponent<Image>();
        knobImg.sprite = quad;
        knobImg.type = Image.Type.Simple;
        knobImg.color = new Color(0.96f, 0.96f, 0.98f, 1f);
        knobImg.raycastTarget = false;

        ApplyTrackAndKnob(trackImage, knobRt, isOn, accent, theme);
        return trackImage;
    }
}
