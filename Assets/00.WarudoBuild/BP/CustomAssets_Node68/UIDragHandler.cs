using UnityEngine;
using UnityEngine.UI;

public class UIDragHandler : MonoBehaviour
{
    private CanvasUIAsset asset;
    private string runtimeId;
    private string displayName;
    private bool isContainer;
    private bool isEnabled;
    private Font font;

    private RectTransform rectTransform;
    private bool isDragging;
    private Vector2 dragOffset;
    private bool isHovered;

    private GameObject highlight;
    private GameObject nameLabel;

    public void Init(CanvasUIAsset owner, string id, string name, bool container, Font sharedFont)
    {
        asset = owner;
        runtimeId = id;
        displayName = name;
        isContainer = container;
        font = sharedFont;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        if (enabled)
        {
            ShowNameLabel(true);
        }
        else
        {
            isDragging = false;
            isHovered = false;
            ShowNameLabel(false);
            ShowHighlight(false);
        }
    }

    private void Update()
    {
        if (!isEnabled || rectTransform == null)
            return;

        var mousePos = Input.mousePosition;
        bool overMe = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            mousePos,
            null
        );

        if (overMe != isHovered)
        {
            isHovered = overMe;
            ShowHighlight(isHovered);
        }

        if (Input.GetMouseButtonDown(0) && overMe && !isDragging)
        {
            isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                mousePos,
                null,
                out var localPoint
            );
            dragOffset = rectTransform.anchoredPosition - localPoint;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                mousePos,
                null,
                out var localPoint
            );
            rectTransform.anchoredPosition = localPoint + dragOffset;
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (asset != null)
            {
                asset.OnElementDragged(runtimeId, rectTransform.anchoredPosition, isContainer);
            }
        }
    }

    public bool IsDragging() => isDragging;

    private void ShowHighlight(bool show)
    {
        if (show && highlight == null)
        {
            highlight = new GameObject("Highlight");
            highlight.transform.SetParent(transform, false);
            var rt = highlight.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(-3, -3);
            rt.offsetMax = new Vector2(3, 3);
            var img = highlight.AddComponent<Image>();
            img.color = isContainer
                ? new Color(0.2f, 0.8f, 0.4f, 0.4f)
                : new Color(0.2f, 0.6f, 1f, 0.4f);
            img.raycastTarget = false;
            highlight.transform.SetAsLastSibling();
        }
        else if (!show && highlight != null)
        {
            Destroy(highlight);
            highlight = null;
        }
    }

    private void ShowNameLabel(bool show)
    {
        if (show && nameLabel == null)
        {
            nameLabel = new GameObject("NameTag");
            nameLabel.transform.SetParent(transform, false);

            var rt = nameLabel.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 0);
            rt.anchoredPosition = new Vector2(0, 2);
            rt.sizeDelta = new Vector2(0, 18);

            var bg = nameLabel.AddComponent<Image>();
            bg.color = isContainer
                ? new Color(0.1f, 0.6f, 0.3f, 0.9f)
                : new Color(0.1f, 0.4f, 0.8f, 0.9f);
            bg.raycastTarget = false;

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(nameLabel.transform, false);
            var trt = textGo.AddComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = new Vector2(4, 0);
            trt.offsetMax = new Vector2(-4, 0);

            var txt = textGo.AddComponent<Text>();
            txt.text = isContainer ? $"{displayName}: 편집중" : displayName;
            txt.font = font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 11;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.raycastTarget = false;
        }
        else if (!show && nameLabel != null)
        {
            Destroy(nameLabel);
            nameLabel = null;
        }
    }

    private void OnDestroy()
    {
        if (highlight != null)
            Destroy(highlight);
        if (nameLabel != null)
            Destroy(nameLabel);
    }
}
