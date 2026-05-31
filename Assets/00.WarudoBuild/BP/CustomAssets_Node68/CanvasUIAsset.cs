using System.Collections.Generic;
using System.Linq;
using Node68.CustomAssets;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Scenes;
using Warudo.UIControl;
using Object = UnityEngine.Object;

[AssetType(
    Id = "b7e3a1f0-4c9d-4e8b-a2f1-6d0e5c3b8a91",
    Title = "UI 리모컨_Node68",
    Category = CustomAssetsFlavorEmbedded.ShareBuild
        ? CustomAssetsNode68Categories.UiRemoteShare
        : CustomAssetsNode68Categories.UiRemoteDev
)]
public class CanvasUIAsset : Asset
{
    private bool ShareUiHidesLayoutInspectorFields() =>
        CustomAssetsBuildRuntime.IsShareBuild();

    [DataInput]
    [Label("표시")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "끄면 Warudo 화면에서 리모컨 UI를 숨깁니다.")]
    public bool CanvasVisible = true;

    [DataInput]
    [Label("테마")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "다크 / 라이트 중 배경·글자 톤을 고릅니다.")]
    public ThemeMode Theme = ThemeMode.Dark;

    [DataInput]
    [Label("테마색상")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "버튼 강조·켜짐 상태 색입니다. Red 빨강 / Orange 주황 / Yellow 노랑 / Green 초록 / Blue 파랑 / Indigo 남색 / Violet 보라")]
    public AccentColor Accent = AccentColor.Blue;

    [DataInput]
    [Label("정렬 순서")]
    [IntegerSlider(0, 100)]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "값이 클수록 다른 오버레이 UI보다 앞에 그려집니다.")]
    public int SortOrder = 50;

    [DataInput]
    [Label("캔버스 불투명도 조절")]
    [FloatSlider(0f, 1f)]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "모든 컨테이너 배경의 투명도를 한 번에 맞춥니다. 0은 완전 투명, 1은 불투명입니다.")]
    public float CanvasContainerOpacity = 1f;

    [DataInput]
    [Label("편집 모드")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "켜면 화면에서 컨테이너·버튼을 마우스로 직접 드래그해 위치를 조정합니다.")]
    public bool EditMode = false;

    [Trigger]
    [HiddenIf(nameof(ShareUiHidesLayoutInspectorFields))]
    [Label("모든 위치 초기화")]
    public void ResetAllPositions()
    {
        _confirmReset = true;
        BroadcastDataInput(nameof(_confirmReset));
    }

    [Markdown]
    [HiddenIf(nameof(HideUnlessResetConfirmPending))]
    public string _resetWarning =
        "⚠️ **모든 컨테이너/버튼·토글의 위치가 초기화됩니다.** 계속하시겠습니까?";

    [Trigger]
    [HiddenIf(nameof(HideUnlessResetConfirmPending))]
    [Label("✔ 초기화 실행")]
    public void ConfirmReset()
    {
        _confirmReset = false;
        BroadcastDataInput(nameof(_confirmReset));

        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c != null)
                {
                    c.Position = Vector2.zero;
                    if (c.Buttons != null)
                    {
                        foreach (var b in c.Buttons)
                        {
                            if (b != null)
                                b.Position = Vector2.zero;
                        }
                    }
                    if (c.Toggles != null)
                    {
                        foreach (var tg in c.Toggles)
                        {
                            if (tg != null)
                                tg.Position = Vector2.zero;
                        }
                    }
                }
            }
        }
        if (FreeButtons != null)
        {
            foreach (var b in FreeButtons)
            {
                if (b != null)
                    b.Position = Vector2.zero;
            }
        }
        if (FreeToggles != null)
        {
            foreach (var tg in FreeToggles)
            {
                if (tg != null)
                    tg.Position = Vector2.zero;
            }
        }
        SetDataInput(nameof(Containers), Containers, broadcast: true);
        SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: true);
        SetDataInput(nameof(FreeToggles), FreeToggles, broadcast: true);
        RebuildActiveRenderer();
    }

    [Trigger]
    [HiddenIf(nameof(HideUnlessResetConfirmPending))]
    [Label("✖ 취소")]
    public void CancelReset()
    {
        _confirmReset = false;
        BroadcastDataInput(nameof(_confirmReset));
    }

    private bool _confirmReset;

    /// <summary>
    /// 쉐어 빌드이거나 확인 대기가 아니면 숨김(초기화 확인 UI).
    /// </summary>
    public bool HideUnlessResetConfirmPending() =>
        ShareUiHidesLayoutInspectorFields() || !_confirmReset;

    [DataInput]
    [Label("컨테이너 목록")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "컨테이너를 펼쳐서 버튼·토글을 추가/관리하세요")]
    [HiddenIf(nameof(ShareUiHidesLayoutInspectorFields))]
    public ContainerData[] Containers = new ContainerData[0];

    [DataInput]
    [Label("자유 배치 버튼")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "컨테이너에 속하지 않는 독립 버튼")]
    [HiddenIf(nameof(ShareUiHidesLayoutInspectorFields))]
    public UIElementData[] FreeButtons = new UIElementData[0];

    [DataInput]
    [Label("자유 배치 토글")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "컨테이너에 속하지 않는 독립 토글 (버튼과 별개의 on/off 상태)"
        )]
    [HiddenIf(nameof(ShareUiHidesLayoutInspectorFields))]
    public UIToggleElementData[] FreeToggles = new UIToggleElementData[0];

    private GameObject canvasRoot;
    private UICanvasRenderer _proRenderer;
    private readonly Dictionary<string, string> _textOverrides = new Dictionary<string, string>();

    protected override void OnCreate()
    {
        base.OnCreate();
        ApplyCanvasContainerOpacityToContainers();
        SetActive(true);
        CreateCanvas();

        Watch(nameof(CanvasVisible), OnVisibleChanged);
        Watch(nameof(Theme), OnThemeChanged);
        Watch(nameof(Accent), OnThemeChanged);
        Watch(nameof(SortOrder), OnSortOrderChanged);
        Watch(nameof(CanvasContainerOpacity), OnCanvasContainerOpacityChanged);
        Watch(nameof(EditMode), OnEditModeChanged);
        Watch(nameof(Containers), OnDataChanged);
        Watch(nameof(FreeButtons), OnDataChanged);
        Watch(nameof(FreeToggles), OnDataChanged);

        if (CustomAssetsBuildRuntime.IsShareBuild())
            ApplyShareBuildOverlayHidden();
    }

    /// <summary>
    /// 쉐어 빌드: 오버레이는 끄고, 컨테이너 편집용 인스펙터 블록은 숨깁니다(데이터는 시리얼라이즈된 그래프 값 그대로).
    /// </summary>
    private void ApplyShareBuildOverlayHidden()
    {
        CanvasVisible = false;
        if (canvasRoot != null)
            canvasRoot.SetActive(false);
        SetDataInput(nameof(CanvasVisible), false, broadcast: true);
    }

    protected override void OnDestroy()
    {
        DestroyCanvas();
        base.OnDestroy();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        SyncActiveRenderer();
    }

    private void CreateCanvas()
    {
        canvasRoot = new GameObject($"WarudoRemote_{Name}");
        Object.DontDestroyOnLoad(canvasRoot);
        _proRenderer = canvasRoot.AddComponent<UICanvasRenderer>();
        _proRenderer.Initialize(this);
    }

    private void DestroyCanvas()
    {
        if (canvasRoot != null)
        {
            Object.Destroy(canvasRoot);
            canvasRoot = null;
            _proRenderer = null;
        }
    }

    private void OnVisibleChanged()
    {
        if (canvasRoot != null)
            canvasRoot.SetActive(CanvasVisible);
    }

    private void OnThemeChanged()
    {
        SyncActiveRenderer();
    }

    private void OnSortOrderChanged()
    {
        if (_proRenderer != null)
            _proRenderer.SetSortOrder(SortOrder);
    }

    private void ApplyCanvasContainerOpacityToContainers()
    {
        if (Containers == null)
            return;
        foreach (var c in Containers)
        {
            if (c != null)
                c.Opacity = CanvasContainerOpacity;
        }
    }

    private void OnCanvasContainerOpacityChanged()
    {
        ApplyCanvasContainerOpacityToContainers();
        SetDataInput(nameof(Containers), Containers, broadcast: true);
        SyncActiveRenderer();
    }

    private void OnEditModeChanged()
    {
        if (_proRenderer != null)
            _proRenderer.SetEditMode(EditMode);
    }

    private void OnDataChanged()
    {
        SyncActiveRenderer();
    }

    private void SyncActiveRenderer()
    {
        if (_proRenderer != null)
            _proRenderer.Sync();
    }

    private void RebuildActiveRenderer()
    {
        if (_proRenderer != null)
            _proRenderer.Rebuild();
    }

    /// <summary>창 크기 변경 등으로 위치를 캔버스 안으로 보정한 뒤 데이터·인스펙터 동기화</summary>
    public void NotifyContainersLayoutChanged()
    {
        SetDataInput(nameof(Containers), Containers, broadcast: false);
        BroadcastDataInput(nameof(Containers));
        BroadcastDataInputProperties(nameof(Containers));
    }

    public void NotifyFreeButtonsLayoutChanged()
    {
        SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: false);
        BroadcastDataInput(nameof(FreeButtons));
        BroadcastDataInputProperties(nameof(FreeButtons));
    }

    public void NotifyFreeTogglesLayoutChanged()
    {
        SetDataInput(nameof(FreeToggles), FreeToggles, broadcast: false);
        BroadcastDataInput(nameof(FreeToggles));
        BroadcastDataInputProperties(nameof(FreeToggles));
    }

    /// <summary>토글 UI 클릭 시 데이터·이벤트를 반영합니다.</summary>
    public void CommitToggleInteraction(string runtimeId, bool newIsOn)
    {
        var t = FindToggleByRuntimeId(runtimeId);
        if (t == null || t.IsOn == newIsOn)
            return;
        t.IsOn = newIsOn;
        if (FindContainerNameForToggle(runtimeId) != null)
        {
            SetDataInput(nameof(Containers), Containers, broadcast: false);
            BroadcastDataInput(nameof(Containers));
            BroadcastDataInputProperties(nameof(Containers));
        }
        else
        {
            SetDataInput(nameof(FreeToggles), FreeToggles, broadcast: false);
            BroadcastDataInput(nameof(FreeToggles));
            BroadcastDataInputProperties(nameof(FreeToggles));
        }
        Context.EventBus.Broadcast(
            new UIToggleValueChangedEvent
            {
                CanvasAssetId = IdString,
                ElementName = t.ElementName,
                IsOn = newIsOn,
            }
        );
    }

    // ===== 블루프린트 API =====

    public void SetButtonVisible(string containerName, string buttonName, bool visible)
    {
        var btn = FindButton(containerName, buttonName);
        if (btn == null)
            return;
        btn.Visible = visible;
        if (!string.IsNullOrEmpty(containerName))
            SetDataInput(nameof(Containers), Containers, broadcast: true);
        else
            SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: true);
        SyncActiveRenderer();
    }

    public void SetButtonStateOn(string containerName, string buttonName, bool stateOn)
    {
        var btn = FindButton(containerName, buttonName);
        if (btn == null)
            return;
        btn.StateOn = stateOn;
        if (!string.IsNullOrEmpty(containerName))
            SetDataInput(nameof(Containers), Containers, broadcast: true);
        else
            SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: true);
        SyncActiveRenderer();
    }

    /// <summary>블루프린트 노드용 — 이름으로 버튼을 찾아 StateOn을 반전합니다.</summary>
    public void ToggleButtonStateOn(string containerName, string buttonName)
    {
        var btn = FindButton(containerName, buttonName);
        if (btn == null)
            return;
        btn.StateOn = !btn.StateOn;
        if (FindContainerNameForButton(btn._runtimeId) != null)
            SetDataInput(nameof(Containers), Containers, broadcast: true);
        else
            SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: true);
        SyncActiveRenderer();
    }

    /// <summary>클릭 핸들러용 — 현재 StateOn을 반전하고 UI를 갱신합니다.</summary>
    public void ToggleButtonStateOnByRuntimeId(string runtimeId)
    {
        var btn = FindButtonByRuntimeId(runtimeId);
        if (btn == null)
            return;
        btn.StateOn = !btn.StateOn;
        if (FindContainerNameForButton(runtimeId) != null)
            SetDataInput(nameof(Containers), Containers, broadcast: true);
        else
            SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: true);
        SyncActiveRenderer();
    }

    public void SetButtonText(string containerName, string buttonName, string text)
    {
        var btn = FindButton(containerName, buttonName);
        if (btn == null)
            return;
        _textOverrides[btn._runtimeId] = text;
        SyncActiveRenderer();
    }

    public string GetDisplayText(UIElementData btn)
    {
        if (btn == null)
            return "";
        if (_textOverrides.TryGetValue(btn._runtimeId, out var overrideText))
            return overrideText;
        return btn.ElementName;
    }

    public UIElementData FindButton(string containerName, string buttonName)
    {
        if (string.IsNullOrEmpty(buttonName))
            return null;

        if (!string.IsNullOrEmpty(containerName))
        {
            var container = FindContainer(containerName);
            if (container?.Buttons != null)
            {
                foreach (var b in container.Buttons)
                {
                    if (b != null && b.ElementName == buttonName)
                        return b;
                }
            }
            return null;
        }

        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c?.Buttons != null)
                {
                    foreach (var b in c.Buttons)
                    {
                        if (b != null && b.ElementName == buttonName)
                            return b;
                    }
                }
            }
        }
        if (FreeButtons != null)
        {
            foreach (var b in FreeButtons)
            {
                if (b != null && b.ElementName == buttonName)
                    return b;
            }
        }
        return null;
    }

    public ContainerData FindContainer(string containerName)
    {
        if (Containers == null || string.IsNullOrEmpty(containerName))
            return null;
        foreach (var c in Containers)
        {
            if (c != null && c.ContainerName == containerName)
                return c;
        }
        return null;
    }

    public UIElementData FindButtonByRuntimeId(string runtimeId)
    {
        if (string.IsNullOrEmpty(runtimeId))
            return null;
        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c?.Buttons != null)
                {
                    foreach (var b in c.Buttons)
                    {
                        if (b != null && b._runtimeId == runtimeId)
                            return b;
                    }
                }
            }
        }
        if (FreeButtons != null)
        {
            foreach (var b in FreeButtons)
            {
                if (b != null && b._runtimeId == runtimeId)
                    return b;
            }
        }
        return null;
    }

    public ContainerData FindContainerByRuntimeId(string runtimeId)
    {
        if (Containers == null || string.IsNullOrEmpty(runtimeId))
            return null;
        foreach (var c in Containers)
        {
            if (c != null && c._runtimeId == runtimeId)
                return c;
        }
        return null;
    }

    public string FindContainerNameForButton(string runtimeId)
    {
        if (Containers == null || string.IsNullOrEmpty(runtimeId))
            return null;
        foreach (var c in Containers)
        {
            if (c?.Buttons != null)
            {
                foreach (var b in c.Buttons)
                {
                    if (b != null && b._runtimeId == runtimeId)
                        return c.ContainerName;
                }
            }
        }
        return null;
    }

    public UIToggleElementData FindToggle(string containerName, string elementName)
    {
        if (string.IsNullOrEmpty(elementName))
            return null;

        if (!string.IsNullOrEmpty(containerName))
        {
            var container = FindContainer(containerName);
            if (container?.Toggles != null)
            {
                foreach (var tg in container.Toggles)
                {
                    if (tg != null && tg.ElementName == elementName)
                        return tg;
                }
            }
            return null;
        }

        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c?.Toggles != null)
                {
                    foreach (var tg in c.Toggles)
                    {
                        if (tg != null && tg.ElementName == elementName)
                            return tg;
                    }
                }
            }
        }
        if (FreeToggles != null)
        {
            foreach (var tg in FreeToggles)
            {
                if (tg != null && tg.ElementName == elementName)
                    return tg;
            }
        }
        return null;
    }

    public UIToggleElementData FindToggleByRuntimeId(string runtimeId)
    {
        if (string.IsNullOrEmpty(runtimeId))
            return null;
        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c?.Toggles != null)
                {
                    foreach (var tg in c.Toggles)
                    {
                        if (tg != null && tg._runtimeId == runtimeId)
                            return tg;
                    }
                }
            }
        }
        if (FreeToggles != null)
        {
            foreach (var tg in FreeToggles)
            {
                if (tg != null && tg._runtimeId == runtimeId)
                    return tg;
            }
        }
        return null;
    }

    public string FindContainerNameForToggle(string runtimeId)
    {
        if (Containers == null || string.IsNullOrEmpty(runtimeId))
            return null;
        foreach (var c in Containers)
        {
            if (c?.Toggles != null)
            {
                foreach (var tg in c.Toggles)
                {
                    if (tg != null && tg._runtimeId == runtimeId)
                        return c.ContainerName;
                }
            }
        }
        return null;
    }

    public void OnElementDragged(string runtimeId, Vector2 newPosition, bool isContainer)
    {
        if (isContainer)
        {
            var c = FindContainerByRuntimeId(runtimeId);
            if (c != null)
            {
                c.Position = newPosition;
                SetDataInput(nameof(Containers), Containers, broadcast: false);
                BroadcastDataInput(nameof(Containers));
                BroadcastDataInputProperties(nameof(Containers));
            }
        }
        else
        {
            var b = FindButtonByRuntimeId(runtimeId);
            if (b != null)
            {
                b.Position = newPosition;
                var containerName = FindContainerNameForButton(runtimeId);
                if (containerName != null)
                {
                    SetDataInput(nameof(Containers), Containers, broadcast: false);
                    BroadcastDataInput(nameof(Containers));
                    BroadcastDataInputProperties(nameof(Containers));
                }
                else
                {
                    SetDataInput(nameof(FreeButtons), FreeButtons, broadcast: false);
                    BroadcastDataInput(nameof(FreeButtons));
                    BroadcastDataInputProperties(nameof(FreeButtons));
                }
            }
            else
            {
                var t = FindToggleByRuntimeId(runtimeId);
                if (t != null)
                {
                    t.Position = newPosition;
                    var containerName = FindContainerNameForToggle(runtimeId);
                    if (containerName != null)
                    {
                        SetDataInput(nameof(Containers), Containers, broadcast: false);
                        BroadcastDataInput(nameof(Containers));
                        BroadcastDataInputProperties(nameof(Containers));
                    }
                    else
                    {
                        SetDataInput(nameof(FreeToggles), FreeToggles, broadcast: false);
                        BroadcastDataInput(nameof(FreeToggles));
                        BroadcastDataInputProperties(nameof(FreeToggles));
                    }
                }
            }
        }
    }

    public AutoCompleteList GetContainerNameList()
    {
        var entries = new List<AutoCompleteEntry>();
        entries.Add(new AutoCompleteEntry { label = "(전체)", value = "" });
        if (Containers != null)
        {
            foreach (var c in Containers)
            {
                if (c != null && !string.IsNullOrEmpty(c.ContainerName))
                {
                    entries.Add(
                        new AutoCompleteEntry { label = c.ContainerName, value = c.ContainerName }
                    );
                }
            }
        }
        return AutoCompleteList.Single(entries);
    }

    public AutoCompleteList GetButtonNameListByContainer(string containerName)
    {
        var entries = new List<AutoCompleteEntry>();

        if (!string.IsNullOrEmpty(containerName))
        {
            var container = FindContainer(containerName);
            if (container?.Buttons != null)
            {
                foreach (var b in container.Buttons)
                {
                    if (b != null && !string.IsNullOrEmpty(b.ElementName))
                    {
                        entries.Add(
                            new AutoCompleteEntry { label = b.ElementName, value = b.ElementName }
                        );
                    }
                }
            }
        }
        else
        {
            if (Containers != null)
            {
                foreach (var c in Containers)
                {
                    if (c?.Buttons != null)
                    {
                        foreach (var b in c.Buttons)
                        {
                            if (b != null && !string.IsNullOrEmpty(b.ElementName))
                            {
                                var label = $"{b.ElementName}  ({c.ContainerName})";
                                entries.Add(
                                    new AutoCompleteEntry { label = label, value = b.ElementName }
                                );
                            }
                        }
                    }
                }
            }
            if (FreeButtons != null)
            {
                foreach (var b in FreeButtons)
                {
                    if (b != null && !string.IsNullOrEmpty(b.ElementName))
                    {
                        entries.Add(
                            new AutoCompleteEntry
                            {
                                label = $"{b.ElementName}  (자유)",
                                value = b.ElementName,
                            }
                        );
                    }
                }
            }
        }

        if (entries.Count == 0)
        {
            return string.IsNullOrEmpty(containerName)
                ? AutoCompleteList.Message("버튼이 없습니다")
                : AutoCompleteList.Message($"'{containerName}' 컨테이너에 버튼이 없습니다");
        }
        return AutoCompleteList.Single(entries);
    }

    /// <summary>블루프린트 자동완성 — 토글 이름 목록 (컨테이너 비우면 전체 + 자유 배치)</summary>
    public AutoCompleteList GetToggleNameListByContainer(string containerName)
    {
        var entries = new List<AutoCompleteEntry>();

        if (!string.IsNullOrEmpty(containerName))
        {
            var container = FindContainer(containerName);
            if (container?.Toggles != null)
            {
                foreach (var tg in container.Toggles)
                {
                    if (tg != null && !string.IsNullOrEmpty(tg.ElementName))
                    {
                        entries.Add(
                            new AutoCompleteEntry { label = tg.ElementName, value = tg.ElementName }
                        );
                    }
                }
            }
        }
        else
        {
            if (Containers != null)
            {
                foreach (var c in Containers)
                {
                    if (c?.Toggles != null)
                    {
                        foreach (var tg in c.Toggles)
                        {
                            if (tg != null && !string.IsNullOrEmpty(tg.ElementName))
                            {
                                var label = $"{tg.ElementName}  ({c.ContainerName})";
                                entries.Add(
                                    new AutoCompleteEntry { label = label, value = tg.ElementName }
                                );
                            }
                        }
                    }
                }
            }
            if (FreeToggles != null)
            {
                foreach (var tg in FreeToggles)
                {
                    if (tg != null && !string.IsNullOrEmpty(tg.ElementName))
                    {
                        entries.Add(
                            new AutoCompleteEntry
                            {
                                label = $"{tg.ElementName}  (자유)",
                                value = tg.ElementName,
                            }
                        );
                    }
                }
            }
        }

        if (entries.Count == 0)
        {
            return string.IsNullOrEmpty(containerName)
                ? AutoCompleteList.Message("토글이 없습니다")
                : AutoCompleteList.Message($"'{containerName}' 컨테이너에 토글이 없습니다");
        }
        return AutoCompleteList.Single(entries);
    }

    /// <summary>레이아웃에 해당 컨테이너에 버튼이 존재하는지 (노드 필터용).</summary>
    public bool LayoutMatchesButtonInContainer(string containerName, string elementName)
    {
        if (string.IsNullOrEmpty(containerName))
            return true;
        return FindButton(containerName, elementName) != null;
    }

    /// <summary>레이아웃에 해당 컨테이너에 토글이 존재하는지 (노드 필터용).</summary>
    public bool LayoutMatchesToggleInContainer(string containerName, string elementName)
    {
        if (string.IsNullOrEmpty(containerName))
            return true;
        return FindToggle(containerName, elementName) != null;
    }
}
