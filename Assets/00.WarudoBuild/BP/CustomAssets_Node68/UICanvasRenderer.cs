using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Warudo.Core;

public class UICanvasRenderer : MonoBehaviour
{
    private CanvasUIAsset asset;
    private Canvas canvas;
    private CanvasScaler scaler;

    private readonly Dictionary<string, GameObject> containerObjects =
        new Dictionary<string, GameObject>();
    private readonly Dictionary<string, GameObject> buttonObjects =
        new Dictionary<string, GameObject>();
    private readonly Dictionary<string, GameObject> toggleObjects =
        new Dictionary<string, GameObject>();
    private bool editMode;
    private Font font;

    private ThemeMode theme;
    private AccentColor accent;
    private Sprite roundedSprite;

    public void Initialize(CanvasUIAsset owner)
    {
        asset = owner;
        theme = owner.Theme;
        accent = owner.Accent;
        roundedSprite = UITheme.GetRoundedSprite();
        LoadFont();

        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = owner.SortOrder;

        scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        gameObject.AddComponent<GraphicRaycaster>();
        EnsureEventSystem();
        Rebuild();
        SetEditMode(owner.EditMode);
    }

    private void LoadFont()
    {
        string[] osNames = Font.GetOSInstalledFontNames();
        string[] preferred =
        {
            "Malgun Gothic",
            "맑은 고딕",
            "MalgunGothic",
            "NanumGothic",
            "나눔고딕",
            "Noto Sans CJK KR",
            "Noto Sans KR",
            "Gulim",
            "굴림",
            "Dotum",
            "돋움",
        };

        foreach (var pref in preferred)
        {
            foreach (var osName in osNames)
            {
                if (string.Equals(osName, pref, System.StringComparison.OrdinalIgnoreCase))
                {
                    font = Font.CreateDynamicFontFromOSFont(osName, 16);
                    if (font != null)
                        return;
                }
            }
        }

        foreach (var osName in osNames)
        {
            if (
                osName.IndexOf("Gothic", System.StringComparison.OrdinalIgnoreCase) >= 0
                || osName.Contains("고딕")
                || osName.Contains("굴림")
                || osName.Contains("돋움")
            )
            {
                font = Font.CreateDynamicFontFromOSFont(osName, 16);
                if (font != null)
                    return;
            }
        }

        font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    public Font GetFont() => font;

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;
        var go = new GameObject("WarudoRemote_EventSystem");
        go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();
        DontDestroyOnLoad(go);
    }

    public void SetSortOrder(int order)
    {
        if (canvas != null)
            canvas.sortingOrder = order;
    }

    public void SetEditMode(bool enabled)
    {
        editMode = enabled;

        foreach (var kvp in containerObjects)
        {
            var handler = kvp.Value.GetComponent<UIDragHandler>();
            if (handler != null)
                handler.SetEnabled(enabled);
        }

        foreach (var kvp in buttonObjects)
        {
            var btn = kvp.Value.GetComponent<Button>();
            if (btn != null)
                btn.enabled = !enabled;

            var handler = kvp.Value.GetComponent<UIDragHandler>();
            if (handler != null)
            {
                bool inContainer = asset.FindContainerNameForButton(kvp.Key) != null;
                handler.SetEnabled(enabled && !inContainer);
            }
        }

        foreach (var kvp in toggleObjects)
        {
            var toggle = kvp.Value.GetComponentInChildren<WarudoUIToggle>(true);
            if (toggle != null)
                toggle.SetPointerInputEnabled(!enabled);

            var handler = kvp.Value.GetComponent<UIDragHandler>();
            if (handler != null)
            {
                bool inContainer = asset.FindContainerNameForToggle(kvp.Key) != null;
                handler.SetEnabled(enabled && !inContainer);
            }
        }
    }

    public bool IsAnyDragging()
    {
        foreach (var kvp in containerObjects)
        {
            var h = kvp.Value.GetComponent<UIDragHandler>();
            if (h != null && h.IsDragging())
                return true;
        }
        foreach (var kvp in buttonObjects)
        {
            var h = kvp.Value.GetComponent<UIDragHandler>();
            if (h != null && h.IsDragging())
                return true;
        }
        foreach (var kvp in toggleObjects)
        {
            var h = kvp.Value.GetComponent<UIDragHandler>();
            if (h != null && h.IsDragging())
                return true;
        }
        return false;
    }

    // ===== Rebuild / Sync =====

    public void Rebuild()
    {
        if (asset == null)
            return;
        theme = asset.Theme;
        accent = asset.Accent;

        ClearAll();
        RebuildContainers();
        RebuildAllButtons();
        RebuildAllToggles();
    }

    public void Sync()
    {
        if (asset == null)
            return;
        if (IsAnyDragging())
            return;

        theme = asset.Theme;
        accent = asset.Accent;
        CleanupRemoved();
        SyncContainers();
        SyncAllButtons();
        SyncAllToggles();
    }

    private void ClearAll()
    {
        foreach (var kvp in toggleObjects)
            if (kvp.Value != null)
                Destroy(kvp.Value);
        toggleObjects.Clear();
        foreach (var kvp in buttonObjects)
            if (kvp.Value != null)
                Destroy(kvp.Value);
        buttonObjects.Clear();
        foreach (var kvp in containerObjects)
            if (kvp.Value != null)
                Destroy(kvp.Value);
        containerObjects.Clear();
    }

    private void CleanupRemoved()
    {
        var validContainers = new HashSet<string>();
        if (asset.Containers != null)
        {
            foreach (var c in asset.Containers)
            {
                if (c != null)
                    validContainers.Add(c._runtimeId);
            }
        }
        var staleContainers = new List<string>();
        foreach (var key in containerObjects.Keys)
        {
            if (!validContainers.Contains(key))
                staleContainers.Add(key);
        }
        foreach (var key in staleContainers)
        {
            if (containerObjects[key] != null)
                Destroy(containerObjects[key]);
            containerObjects.Remove(key);
        }

        var validButtons = new HashSet<string>();
        if (asset.Containers != null)
        {
            foreach (var c in asset.Containers)
            {
                if (c?.Buttons != null)
                {
                    foreach (var b in c.Buttons)
                    {
                        if (b != null)
                            validButtons.Add(b._runtimeId);
                    }
                }
            }
        }
        if (asset.FreeButtons != null)
        {
            foreach (var b in asset.FreeButtons)
            {
                if (b != null)
                    validButtons.Add(b._runtimeId);
            }
        }
        var staleButtons = new List<string>();
        foreach (var key in buttonObjects.Keys)
        {
            if (!validButtons.Contains(key))
                staleButtons.Add(key);
        }
        foreach (var key in staleButtons)
        {
            if (buttonObjects[key] != null)
                Destroy(buttonObjects[key]);
            buttonObjects.Remove(key);
        }

        var validToggles = new HashSet<string>();
        if (asset.Containers != null)
        {
            foreach (var c in asset.Containers)
            {
                if (c?.Toggles != null)
                {
                    foreach (var tg in c.Toggles)
                    {
                        if (tg != null)
                            validToggles.Add(tg._runtimeId);
                    }
                }
            }
        }
        if (asset.FreeToggles != null)
        {
            foreach (var tg in asset.FreeToggles)
            {
                if (tg != null)
                    validToggles.Add(tg._runtimeId);
            }
        }
        var staleToggles = new List<string>();
        foreach (var key in toggleObjects.Keys)
        {
            if (!validToggles.Contains(key))
                staleToggles.Add(key);
        }
        foreach (var key in staleToggles)
        {
            if (toggleObjects[key] != null)
                Destroy(toggleObjects[key]);
            toggleObjects.Remove(key);
        }
    }

    // ===== Container =====

    private void RebuildContainers()
    {
        if (asset.Containers == null)
            return;
        foreach (var data in asset.Containers)
        {
            if (data == null)
                continue;
            if (!containerObjects.ContainsKey(data._runtimeId))
            {
                CreateContainer(data);
            }
            ApplyContainerData(data);
        }
    }

    private void SyncContainers()
    {
        if (asset.Containers == null)
            return;
        foreach (var data in asset.Containers)
        {
            if (data == null)
                continue;
            if (!containerObjects.ContainsKey(data._runtimeId))
            {
                CreateContainer(data);
            }
            ApplyContainerData(data);
        }
    }

    private void CreateContainer(ContainerData data)
    {
        var go = new GameObject($"Container_{data.ContainerName}");
        go.transform.SetParent(transform, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        var bg = go.AddComponent<Image>();
        bg.sprite = roundedSprite;
        bg.type = Image.Type.Sliced;
        var initColor = UITheme.ContainerBg(theme, accent);
        bg.color = new Color(initColor.r, initColor.g, initColor.b, data.Opacity);
        bg.raycastTarget = true;

        var titleGo = new GameObject("TitleLabel");
        titleGo.transform.SetParent(go.transform, false);
        var titleRt = titleGo.AddComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0, 1);
        titleRt.anchorMax = new Vector2(1, 1);
        titleRt.pivot = new Vector2(0.5f, 1);
        titleRt.anchoredPosition = new Vector2(0, -4);
        titleRt.sizeDelta = new Vector2(-20, data.TitleFontSize + 10);

        var titleText = titleGo.AddComponent<Text>();
        titleText.font = font;
        titleText.fontSize = data.TitleFontSize;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = UITheme.ResolveContainerTitleColor(
            data.CustomTitleColor,
            data.TitleColor,
            theme
        );
        titleText.horizontalOverflow = HorizontalWrapMode.Overflow;
        titleText.verticalOverflow = VerticalWrapMode.Truncate;
        titleText.raycastTarget = false;
        titleText.fontStyle = FontStyle.Bold;

        var contentGo = new GameObject("Content");
        contentGo.transform.SetParent(go.transform, false);
        var contentRt = contentGo.AddComponent<RectTransform>();
        contentRt.anchorMin = Vector2.zero;
        contentRt.anchorMax = Vector2.one;
        contentRt.offsetMin = new Vector2(0, 0);
        contentRt.offsetMax = new Vector2(0, 0);

        var drag = go.AddComponent<UIDragHandler>();
        drag.Init(asset, data._runtimeId, data.ContainerName, true, font);
        drag.SetEnabled(editMode);

        containerObjects[data._runtimeId] = go;
    }

    private void ApplyContainerData(ContainerData data)
    {
        if (!containerObjects.TryGetValue(data._runtimeId, out var go))
            return;

        go.SetActive(data.Visible);

        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            if (!editMode || !IsAnyDragging())
                rt.anchoredPosition = data.Position;
            rt.sizeDelta = data.Size;
        }

        var bg = go.GetComponent<Image>();
        if (bg != null)
        {
            bg.sprite = roundedSprite;
            bg.type = Image.Type.Sliced;
            var c = UITheme.ContainerBg(theme, accent);
            bg.color = new Color(c.r, c.g, c.b, data.Opacity);
        }

        bool hasTitle = !string.IsNullOrEmpty(data.Title);
        float titleAreaHeight = hasTitle ? data.TitleFontSize + 16f : 0f;

        var titleLabel = go.transform.Find("TitleLabel");
        if (titleLabel != null)
        {
            titleLabel.gameObject.SetActive(hasTitle);
            if (hasTitle)
            {
                var titleRt = titleLabel.GetComponent<RectTransform>();
                if (titleRt != null)
                {
                    titleRt.sizeDelta = new Vector2(-20, data.TitleFontSize + 10);
                }
                var titleText = titleLabel.GetComponent<Text>();
                if (titleText != null)
                {
                    titleText.text = data.Title;
                    titleText.fontSize = data.TitleFontSize;
                    titleText.color = UITheme.ResolveContainerTitleColor(
                        data.CustomTitleColor,
                        data.TitleColor,
                        theme
                    );
                }
            }
        }

        var content = go.transform.Find("Content");
        if (content != null)
        {
            var contentRt = content.GetComponent<RectTransform>();
            if (contentRt != null)
            {
                contentRt.offsetMax = new Vector2(0, -titleAreaHeight);
            }
            ApplyLayout(content.gameObject, data);
            ApplyAutoSize(content.gameObject, data);
        }

        // 표시만 캔버스 안으로 맞춤 — 저장 좌표(data.Position)는 건드리지 않음(창 크기 변화 시 잘린 값이 저장되는 문제 방지)
        if (rt != null)
            ClampRectInsideCanvas(rt);
    }

    /// <summary>창이 줄어들어 UI가 화면 밖으로 나갈 때 앵커 기준으로 캔버스 rect 안에 맞춤</summary>
    private bool ClampRectInsideCanvas(RectTransform child)
    {
        var parent = transform as RectTransform;
        if (parent == null)
            return false;

        Rect pr = parent.rect;
        Rect cr = child.rect;
        Vector2 pivot = child.pivot;
        Vector2 size = cr.size;

        float minX = pr.xMin + size.x * pivot.x;
        float maxX = pr.xMax - size.x * (1f - pivot.x);
        float minY = pr.yMin + size.y * pivot.y;
        float maxY = pr.yMax - size.y * (1f - pivot.y);

        Vector2 pos = child.anchoredPosition;
        Vector2 clamped = pos;

        if (minX <= maxX)
            clamped.x = Mathf.Clamp(pos.x, minX, maxX);
        else
            clamped.x = 0.5f * (pr.xMin + pr.xMax);

        if (minY <= maxY)
            clamped.y = Mathf.Clamp(pos.y, minY, maxY);
        else
            clamped.y = 0.5f * (pr.yMin + pr.yMax);

        if ((clamped - pos).sqrMagnitude < 0.0001f)
            return false;

        child.anchoredPosition = clamped;
        return true;
    }

    private void ApplyLayout(GameObject contentGo, ContainerData data)
    {
        var vlg = contentGo.GetComponent<VerticalLayoutGroup>();
        var hlg = contentGo.GetComponent<HorizontalLayoutGroup>();
        var glg = contentGo.GetComponent<GridLayoutGroup>();

        if (data.Direction == LayoutMode.Grid)
        {
            if (vlg != null)
                DestroyImmediate(vlg);
            if (hlg != null)
                DestroyImmediate(hlg);
            if (glg == null)
                glg = contentGo.AddComponent<GridLayoutGroup>();
            SetGridLayout(glg, data);
        }
        else if (data.Direction == LayoutMode.Vertical)
        {
            if (hlg != null)
                DestroyImmediate(hlg);
            if (glg != null)
                DestroyImmediate(glg);
            if (vlg == null)
                vlg = contentGo.AddComponent<VerticalLayoutGroup>();
            SetLayoutGroup(vlg, data);
        }
        else
        {
            if (vlg != null)
                DestroyImmediate(vlg);
            if (glg != null)
                DestroyImmediate(glg);
            if (hlg == null)
                hlg = contentGo.AddComponent<HorizontalLayoutGroup>();
            SetLayoutGroup(hlg, data);
        }
    }

    private void SetLayoutGroup(HorizontalOrVerticalLayoutGroup lg, ContainerData data)
    {
        lg.spacing = data.Spacing;
        lg.padding = new RectOffset(data.Padding, data.Padding, data.Padding, data.Padding);
        lg.childForceExpandWidth = true;
        lg.childForceExpandHeight = false;
        lg.childControlWidth = true;
        lg.childControlHeight = true;

        lg.childAlignment = data.Alignment switch
        {
            FlexAlign.Start => data.Direction == LayoutMode.Vertical
                ? TextAnchor.UpperCenter
                : TextAnchor.MiddleLeft,
            FlexAlign.End => data.Direction == LayoutMode.Vertical
                ? TextAnchor.LowerCenter
                : TextAnchor.MiddleRight,
            _ => TextAnchor.MiddleCenter,
        };
    }

    private void SetGridLayout(GridLayoutGroup glg, ContainerData data)
    {
        int cols = Mathf.Max(1, data.Columns);
        float availableWidth = data.Size.x - data.Padding * 2f;
        float cellWidth = (availableWidth - data.Spacing * (cols - 1)) / cols;
        cellWidth = Mathf.Max(cellWidth, 20f);

        glg.cellSize = new Vector2(cellWidth, data.ButtonSize.y);
        glg.spacing = new Vector2(data.Spacing, data.Spacing);
        glg.padding = new RectOffset(data.Padding, data.Padding, data.Padding, data.Padding);
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = cols;
        glg.startAxis = GridLayoutGroup.Axis.Horizontal;
        glg.startCorner = GridLayoutGroup.Corner.UpperLeft;

        glg.childAlignment = data.Alignment switch
        {
            FlexAlign.Start => TextAnchor.UpperLeft,
            FlexAlign.End => TextAnchor.LowerRight,
            _ => TextAnchor.UpperCenter,
        };
    }

    private void ApplyAutoSize(GameObject contentGo, ContainerData data)
    {
        var csf = contentGo.GetComponent<ContentSizeFitter>();
        if (data.AutoSize)
        {
            if (csf == null)
                csf = contentGo.AddComponent<ContentSizeFitter>();
            csf.horizontalFit =
                data.Direction == LayoutMode.Horizontal
                    ? ContentSizeFitter.FitMode.PreferredSize
                    : ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit =
                (data.Direction == LayoutMode.Vertical || data.Direction == LayoutMode.Grid)
                    ? ContentSizeFitter.FitMode.PreferredSize
                    : ContentSizeFitter.FitMode.Unconstrained;
        }
        else
        {
            if (csf != null)
                Destroy(csf);
        }
    }

    // ===== Buttons =====

    private void RebuildAllButtons()
    {
        if (asset.Containers != null)
        {
            foreach (var container in asset.Containers)
            {
                if (container?.Buttons == null)
                    continue;
                foreach (var data in container.Buttons)
                {
                    if (data == null)
                        continue;
                    if (!buttonObjects.ContainsKey(data._runtimeId))
                    {
                        CreateButton(data, container);
                    }
                    EnsureButtonParent(data, container);
                    ApplyButtonData(data, container);
                }
            }
        }
        if (asset.FreeButtons != null)
        {
            foreach (var data in asset.FreeButtons)
            {
                if (data == null)
                    continue;
                if (!buttonObjects.ContainsKey(data._runtimeId))
                {
                    CreateButton(data, null);
                }
                EnsureButtonParent(data, null);
                ApplyButtonData(data, null);
            }
        }
    }

    private void SyncAllButtons()
    {
        if (asset.Containers != null)
        {
            foreach (var container in asset.Containers)
            {
                if (container?.Buttons == null)
                    continue;
                foreach (var data in container.Buttons)
                {
                    if (data == null)
                        continue;
                    if (!buttonObjects.ContainsKey(data._runtimeId))
                    {
                        CreateButton(data, container);
                    }
                    EnsureButtonParent(data, container);
                    ApplyButtonData(data, container);
                }
            }
        }
        if (asset.FreeButtons != null)
        {
            foreach (var data in asset.FreeButtons)
            {
                if (data == null)
                    continue;
                if (!buttonObjects.ContainsKey(data._runtimeId))
                {
                    CreateButton(data, null);
                }
                EnsureButtonParent(data, null);
                ApplyButtonData(data, null);
            }
        }
    }

    private Transform GetContainerContent(ContainerData container)
    {
        if (container != null && containerObjects.TryGetValue(container._runtimeId, out var cGo))
        {
            var content = cGo.transform.Find("Content");
            return content != null ? content : cGo.transform;
        }
        return transform;
    }

    private void EnsureButtonParent(UIElementData data, ContainerData container)
    {
        if (!buttonObjects.TryGetValue(data._runtimeId, out var go))
            return;
        var targetParent = GetContainerContent(container);
        if (go.transform.parent != targetParent)
        {
            go.transform.SetParent(targetParent, false);
        }
    }

    private void CreateButton(UIElementData data, ContainerData container)
    {
        var go = new GameObject(data.ElementName);
        go.transform.SetParent(GetContainerContent(container), false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = data.Size;

        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth = data.Size.x;
        le.preferredHeight = data.Size.y;

        UIButtonChrome.CreateBorderAndBg(
            go.transform,
            roundedSprite,
            data.StateOn,
            accent,
            out _,
            out var bgImage
        );

        var btn = go.AddComponent<Button>();
        var nav = new Navigation { mode = Navigation.Mode.None };
        btn.navigation = nav;
        btn.targetGraphic = bgImage;
        btn.enabled = !editMode;
        ApplyButtonColors(btn, accent, data.StateOn);

        btn.onClick.AddListener(() =>
        {
            if (editMode)
                return;
            if (data.ToggleStateOnClick)
                asset.ToggleButtonStateOnByRuntimeId(data._runtimeId);
            Context.EventBus.Broadcast(
                new UIButtonClickEvent
                {
                    CanvasAssetId = asset.IdString,
                    ElementName = data.ElementName,
                }
            );
        });

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(8, 2);
        textRt.offsetMax = new Vector2(-8, -2);

        var text = textGo.AddComponent<Text>();
        text.font = font;
        text.text = asset.GetDisplayText(data);
        text.fontSize = container != null ? container.FontSize : 15;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        text.supportRichText = false;
        text.color = UITheme.TextForButtonState(data.StateOn, accent);

        var outline = textGo.AddComponent<Outline>();
        outline.effectColor = UITheme.TextOutlineEffectColor(theme);
        outline.effectDistance = new Vector2(1, -1);

        bool inContainer = container != null;
        var drag = go.AddComponent<UIDragHandler>();
        drag.Init(asset, data._runtimeId, data.ElementName, false, font);
        drag.SetEnabled(editMode && !inContainer);

        buttonObjects[data._runtimeId] = go;
    }

    private void ApplyButtonColors(Button btn, AccentColor accentColor, bool stateOn)
    {
        var colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = UITheme.ButtonHighlightedColorMultiplierForState(
            stateOn,
            accentColor
        );
        colors.pressedColor = UITheme.ButtonPressedColorMultiplierForState(stateOn, accentColor);
        colors.selectedColor = Color.white;
        colors.disabledColor = UITheme.ButtonDisabledOverlayColor();
        colors.fadeDuration = 0.08f;
        colors.colorMultiplier = 1f;
        btn.colors = colors;
    }

    private void ApplyButtonData(UIElementData data, ContainerData container)
    {
        if (!buttonObjects.TryGetValue(data._runtimeId, out var go))
            return;

        go.SetActive(data.Visible);

        bool inContainer = container != null;
        var size = inContainer ? container.ButtonSize : data.Size;

        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            if (!inContainer && (!editMode || !IsAnyDragging()))
                rt.anchoredPosition = data.Position;
            rt.sizeDelta = size;
        }

        var le = go.GetComponent<LayoutElement>();
        if (le != null)
        {
            le.enabled = inContainer;
            le.preferredWidth = size.x;
            le.preferredHeight = size.y;
        }

        var btn = go.GetComponent<Button>();
        if (btn != null)
        {
            btn.enabled = !editMode;
            ApplyButtonColors(btn, accent, data.StateOn);
        }

        var handler = go.GetComponent<UIDragHandler>();
        if (handler != null)
            handler.SetEnabled(editMode && !inContainer);

        var borderImg = go.transform.Find(UIButtonChrome.BorderName)?.GetComponent<Image>();
        var bgImg = go.transform.Find(UIButtonChrome.BgName)?.GetComponent<Image>();
        UIButtonChrome.ApplyBorderAndBgColors(borderImg, bgImg, data.StateOn, accent);

        var text = go.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = asset.GetDisplayText(data);
            text.fontSize = inContainer ? container.FontSize : 15;
            text.color = UITheme.TextForButtonState(data.StateOn, accent);
        }

        if (!inContainer && rt != null)
            ClampRectInsideCanvas(rt);
    }

    // ===== Toggles =====

    private void RebuildAllToggles()
    {
        if (asset.Containers != null)
        {
            foreach (var container in asset.Containers)
            {
                if (container?.Toggles == null)
                    continue;
                foreach (var data in container.Toggles)
                {
                    if (data == null)
                        continue;
                    if (!toggleObjects.ContainsKey(data._runtimeId))
                        CreateToggle(data, container);
                    EnsureToggleParent(data, container);
                    ApplyToggleData(data, container);
                }
            }
        }
        if (asset.FreeToggles != null)
        {
            foreach (var data in asset.FreeToggles)
            {
                if (data == null)
                    continue;
                if (!toggleObjects.ContainsKey(data._runtimeId))
                    CreateToggle(data, null);
                EnsureToggleParent(data, null);
                ApplyToggleData(data, null);
            }
        }
    }

    private void SyncAllToggles()
    {
        if (asset.Containers != null)
        {
            foreach (var container in asset.Containers)
            {
                if (container?.Toggles == null)
                    continue;
                foreach (var data in container.Toggles)
                {
                    if (data == null)
                        continue;
                    if (!toggleObjects.ContainsKey(data._runtimeId))
                        CreateToggle(data, container);
                    EnsureToggleParent(data, container);
                    ApplyToggleData(data, container);
                }
            }
        }
        if (asset.FreeToggles != null)
        {
            foreach (var data in asset.FreeToggles)
            {
                if (data == null)
                    continue;
                if (!toggleObjects.ContainsKey(data._runtimeId))
                    CreateToggle(data, null);
                EnsureToggleParent(data, null);
                ApplyToggleData(data, null);
            }
        }
    }

    private void EnsureToggleParent(UIToggleElementData data, ContainerData container)
    {
        if (!toggleObjects.TryGetValue(data._runtimeId, out var go))
            return;
        var targetParent = GetContainerContent(container);
        if (go.transform.parent != targetParent)
            go.transform.SetParent(targetParent, false);
    }

    private void CreateToggle(UIToggleElementData data, ContainerData container)
    {
        var go = new GameObject($"Toggle_{data.ElementName}");
        go.transform.SetParent(GetContainerContent(container), false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = data.Size;

        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth = data.Size.x;
        le.preferredHeight = data.Size.y;

        var trackImage = UIToggleSwitchChrome.BuildSwitchRow(
            go.transform,
            data.IsOn,
            accent,
            theme,
            font,
            string.IsNullOrEmpty(data.ElementName) ? "토글" : data.ElementName,
            container != null ? container.FontSize : 15,
            out _
        );

        var toggleGo = trackImage != null ? trackImage.gameObject : go;
        var toggle = toggleGo.AddComponent<WarudoUIToggle>();
        toggle.SetIsOnWithoutNotify(data.IsOn);
        toggle.SetPointerInputEnabled(!editMode);
        toggle.SetInteractionCommittedCallback(on =>
            asset.CommitToggleInteraction(data._runtimeId, on)
        );

        bool inContainer = container != null;
        var drag = go.AddComponent<UIDragHandler>();
        drag.Init(asset, data._runtimeId, data.ElementName, false, font);
        drag.SetEnabled(editMode && !inContainer);

        toggleObjects[data._runtimeId] = go;
    }

    private void ApplyToggleData(UIToggleElementData data, ContainerData container)
    {
        if (!toggleObjects.TryGetValue(data._runtimeId, out var go))
            return;

        go.SetActive(data.Visible);

        bool inContainer = container != null;
        var size = inContainer ? container.ButtonSize : data.Size;

        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            if (!inContainer && (!editMode || !IsAnyDragging()))
                rt.anchoredPosition = data.Position;
            rt.sizeDelta = size;
        }

        var le = go.GetComponent<LayoutElement>();
        if (le != null)
        {
            le.enabled = inContainer;
            le.preferredWidth = size.x;
            le.preferredHeight = size.y;
        }

        var tg = go.GetComponentInChildren<WarudoUIToggle>(true);
        if (tg != null)
        {
            tg.SetPointerInputEnabled(!editMode);
            tg.SetIsOnWithoutNotify(data.IsOn);
        }

        var handler = go.GetComponent<UIDragHandler>();
        if (handler != null)
            handler.SetEnabled(editMode && !inContainer);

        var hitArea = go.transform.Find(UIToggleSwitchChrome.HitAreaName);
        var trackImg =
            hitArea != null
                ? hitArea.Find(UIToggleSwitchChrome.TrackName)?.GetComponent<Image>()
                : null;
        var knobRt =
            hitArea != null
                ? hitArea.Find(UIToggleSwitchChrome.KnobName)?.GetComponent<RectTransform>()
                : null;
        UIToggleSwitchChrome.ApplyTrackAndKnob(trackImg, knobRt, data.IsOn, accent, theme);

        var labelGo = go.transform.Find(UIToggleSwitchChrome.LabelName);
        var labelText = labelGo != null ? labelGo.GetComponent<Text>() : null;
        if (labelText != null)
        {
            labelText.text = string.IsNullOrEmpty(data.ElementName) ? "토글" : data.ElementName;
            labelText.fontSize = inContainer ? container.FontSize : 15;
            labelText.color = UITheme.TextPrimary(theme);
        }

        if (!inContainer && rt != null)
            ClampRectInsideCanvas(rt);
    }
}
