using UnityEngine;

public enum ThemeMode
{
    Dark,
    Light,
}

public enum AccentColor
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet,
}

public static class UITheme
{
    private static Sprite _roundedSprite;

    private static Color Rgb(byte r, byte g, byte b) => new Color(r / 255f, g / 255f, b / 255f, 1f);

    /// <summary>Tailwind 600 계열 — 호버 목표색 (500 대비 한 단계 진하게)</summary>
    public static Color ButtonAccent600(AccentColor accent)
    {
        return accent switch
        {
            AccentColor.Red => Rgb(220, 38, 38), // #dc2626
            AccentColor.Orange => Rgb(234, 88, 12), // #ea580c
            AccentColor.Yellow => Rgb(202, 138, 4), // #ca8a04
            AccentColor.Green => Rgb(22, 163, 74), // #16a34a
            AccentColor.Blue => Rgb(37, 99, 235), // #2563eb
            AccentColor.Indigo => Rgb(79, 70, 229), // #4f46e5
            AccentColor.Violet => Rgb(147, 51, 234), // #9333ea
            _ => Rgb(37, 99, 235),
        };
    }

    /// <summary>Tailwind 700 계열 — 눌림 목표색</summary>
    public static Color ButtonAccent700(AccentColor accent)
    {
        return accent switch
        {
            AccentColor.Red => Rgb(185, 28, 28), // #b91c1c
            AccentColor.Orange => Rgb(194, 65, 12), // #c2410c
            AccentColor.Yellow => Rgb(161, 98, 7), // #a16207
            AccentColor.Green => Rgb(21, 128, 61), // #15803d
            AccentColor.Blue => Rgb(29, 78, 216), // #1d4ed8
            AccentColor.Indigo => Rgb(67, 56, 202), // #4338ca
            AccentColor.Violet => Rgb(126, 34, 206), // #7e22ce
            _ => Rgb(29, 78, 216),
        };
    }

    public static Color GetAccent(AccentColor accent) => ButtonStateOnColor(accent);

    // ===== Rounded Sprite =====

    public static Sprite GetRoundedSprite()
    {
        if (_roundedSprite != null)
            return _roundedSprite;

        int size = 64;
        int radius = 16;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float a = RoundedRectAlpha(x, y, size, size, radius);
                tex.SetPixel(x, y, new Color(1, 1, 1, a));
            }
        }
        tex.Apply();

        var border = new Vector4(radius, radius, radius, radius);
        _roundedSprite = Sprite.Create(
            tex,
            new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f),
            100,
            1,
            SpriteMeshType.FullRect,
            border
        );
        return _roundedSprite;
    }

    private static float RoundedRectAlpha(int x, int y, int w, int h, int r)
    {
        int cx,
            cy;
        if (x < r && y < r)
        {
            cx = r;
            cy = r;
        }
        else if (x >= w - r && y < r)
        {
            cx = w - r - 1;
            cy = r;
        }
        else if (x < r && y >= h - r)
        {
            cx = r;
            cy = h - r - 1;
        }
        else if (x >= w - r && y >= h - r)
        {
            cx = w - r - 1;
            cy = h - r - 1;
        }
        else
            return 1f;

        float dist = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
        if (dist <= r - 0.5f)
            return 1f;
        if (dist >= r + 0.5f)
            return 0f;
        return r + 0.5f - dist;
    }

    // ===== Container =====

    public static Color ContainerBg(ThemeMode mode, AccentColor accent)
    {
        return mode == ThemeMode.Dark
            ? new Color(0.10f, 0.10f, 0.13f, 0.94f)
            : new Color(0.97f, 0.97f, 0.98f, 0.94f);
    }

    public static Color TitleColor(ThemeMode mode)
    {
        return mode == ThemeMode.Dark
            ? new Color(0.75f, 0.75f, 0.80f, 1f)
            : new Color(0.35f, 0.35f, 0.40f, 1f);
    }

    public static Color ResolveContainerTitleColor(bool useCustom, Color custom, ThemeMode theme)
    {
        return useCustom ? custom : TitleColor(theme);
    }

    // ===== Button =====

    public static Color ButtonBg(ThemeMode mode, AccentColor accent)
    {
        var c = ButtonStateOnColor(accent);
        return new Color(c.r, c.g, c.b, mode == ThemeMode.Dark ? 0.95f : 0.92f);
    }

    /// <summary>켜짐: 채도 높은 메인색 (Blue = #3b82f6). 꺼짐: 같은 계열 연한 파스텔 (Blue = #dbeafe).</summary>
    public static Color ButtonStateOnColor(AccentColor accent)
    {
        return accent switch
        {
            AccentColor.Red => Rgb(239, 68, 68), // #ef4444
            AccentColor.Orange => Rgb(249, 115, 22), // #f97316
            AccentColor.Yellow => Rgb(234, 179, 8), // #eab308
            AccentColor.Green => Rgb(34, 197, 94), // #22c55e
            AccentColor.Blue => Rgb(59, 130, 246), // #3b82f6
            AccentColor.Indigo => Rgb(99, 102, 241), // #6366f1
            AccentColor.Violet => Rgb(168, 85, 247), // #a855f7
            _ => Rgb(59, 130, 246),
        };
    }

    public static Color ButtonStateOffColor(AccentColor accent)
    {
        return accent switch
        {
            AccentColor.Red => new Color(254 / 255f, 226 / 255f, 226 / 255f), // #fee2e2
            AccentColor.Orange => new Color(255 / 255f, 237 / 255f, 213 / 255f), // #ffedd5
            AccentColor.Yellow => new Color(254 / 255f, 249 / 255f, 195 / 255f), // #fef9c3
            AccentColor.Green => new Color(220 / 255f, 252 / 255f, 231 / 255f), // #dcfce7
            AccentColor.Blue => new Color(219 / 255f, 234 / 255f, 254 / 255f), // #dbeafe
            AccentColor.Indigo => new Color(224 / 255f, 231 / 255f, 255 / 255f), // #e0e7ff
            AccentColor.Violet => new Color(243 / 255f, 232 / 255f, 255 / 255f), // #f3e8ff
            _ => new Color(219 / 255f, 234 / 255f, 254 / 255f),
        };
    }

    public static Color ButtonBgForState(bool stateOn, AccentColor accent)
    {
        const float a = 0.95f;
        if (stateOn)
        {
            var c = ButtonStateOnColor(accent);
            return new Color(c.r, c.g, c.b, a);
        }
        var off = ButtonStateOffColor(accent);
        return new Color(off.r, off.g, off.b, a);
    }

    public static Color ButtonBorderStrokeColor(bool stateOn, AccentColor accent)
    {
        return stateOn ? ButtonAccent700(accent) : ButtonStateOnColor(accent);
    }

    public static Color ButtonStateOffHoverColor(AccentColor accent)
    {
        var off = ButtonStateOffColor(accent);
        return Color.Lerp(off, ButtonAccent600(accent), 0.2f);
    }

    public static Color ButtonHighlightedColorMultiplierForState(bool stateOn, AccentColor accent)
    {
        if (stateOn)
            return ButtonHighlightedColorMultiplier(accent);
        var p = ButtonStateOffColor(accent);
        var h = ButtonStateOffHoverColor(accent);
        const float e = 1e-5f;
        return new Color(
            h.r / Mathf.Max(p.r, e),
            h.g / Mathf.Max(p.g, e),
            h.b / Mathf.Max(p.b, e),
            1f
        );
    }

    public static Color ButtonPressedColorMultiplierForState(bool stateOn, AccentColor accent)
    {
        if (stateOn)
            return ButtonPressedColorMultiplier(accent);
        var p = ButtonStateOffColor(accent);
        var q = Color.Lerp(p, ButtonAccent700(accent), 0.15f);
        const float e = 1e-5f;
        return new Color(
            q.r / Mathf.Max(p.r, e),
            q.g / Mathf.Max(p.g, e),
            q.b / Mathf.Max(p.b, e),
            1f
        );
    }

    public static Color TextForButtonState(bool stateOn, AccentColor accent)
    {
        if (stateOn)
        {
            var c = ButtonStateOnColor(accent);
            float lum = 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
            return lum > 0.72f ? new Color(0.12f, 0.12f, 0.16f, 1f) : Color.white;
        }
        return ButtonAccent600(accent);
    }

    public static Color ButtonHover(AccentColor accent) => ButtonAccent600(accent);

    /// <summary>Unity Button ColorBlock — Image(500) × 반환값 ≈ 600 호버</summary>
    public static Color ButtonHighlightedColorMultiplier(AccentColor accent)
    {
        var p = ButtonStateOnColor(accent);
        var h = ButtonAccent600(accent);
        const float e = 1e-5f;
        return new Color(
            h.r / Mathf.Max(p.r, e),
            h.g / Mathf.Max(p.g, e),
            h.b / Mathf.Max(p.b, e),
            1f
        );
    }

    /// <summary>Image(500) × 반환값 ≈ 700 눌림</summary>
    public static Color ButtonPressedColorMultiplier(AccentColor accent)
    {
        var p = ButtonStateOnColor(accent);
        var q = ButtonAccent700(accent);
        const float e = 1e-5f;
        return new Color(
            q.r / Mathf.Max(p.r, e),
            q.g / Mathf.Max(p.g, e),
            q.b / Mathf.Max(p.b, e),
            1f
        );
    }

    public static Color ButtonPressed(AccentColor accent) => ButtonAccent700(accent);

    public static Color ButtonText(ThemeMode mode, AccentColor accent)
    {
        var bg = ButtonBg(mode, accent);
        float luminance = 0.299f * bg.r + 0.587f * bg.g + 0.114f * bg.b;
        return luminance > 0.55f ? new Color(0.12f, 0.12f, 0.18f, 1f) : new Color(1f, 1f, 1f, 1f);
    }

    // ===== Text =====

    public static Color TextPrimary(ThemeMode mode)
    {
        return mode == ThemeMode.Dark
            ? new Color(0.95f, 0.95f, 0.97f, 1f)
            : new Color(0.12f, 0.12f, 0.18f, 1f);
    }

    /// <summary>버튼 라벨 Outline — 정식/Lite UICanvasRenderer 공통</summary>
    public static Color TextOutlineEffectColor(ThemeMode theme)
    {
        return new Color(0, 0, 0, theme == ThemeMode.Dark ? 0.5f : 0.15f);
    }

    /// <summary>Unity Button ColorBlock.disabledColor</summary>
    public static Color ButtonDisabledOverlayColor()
    {
        return new Color(0.7f, 0.7f, 0.7f, 1f);
    }
}
