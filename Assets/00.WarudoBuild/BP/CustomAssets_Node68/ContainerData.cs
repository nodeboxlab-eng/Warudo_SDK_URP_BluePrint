using Node68.CustomAssets;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;

public enum LayoutMode
{
    Vertical = 0,
    Horizontal = 1,
    Grid = 2,
}

public enum FlexAlign
{
    Start,
    Center,
    End,
}

public class ContainerData : StructuredData<CanvasUIAsset>, ICollapsibleStructuredData
{
    public string _runtimeId = System.Guid.NewGuid().ToString("N").Substring(0, 8);

    [DataInput]
    [Label("컨테이너 이름")]
    public string ContainerName = "컨테이너";

    [DataInput]
    [Label("타이틀")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "패널 상단에 표시되는 제목 (비워두면 숨김)"
        )]
    public string Title = "";

    [DataInput]
    [Label("타이틀 글자 크기")]
    [IntegerSlider(8, 36)]
    public int TitleFontSize = 13;

    [DataInput]
    [Label("타이틀 색 사용자 지정")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "켜면 아래 타이틀 색을 씁니다. 끄면 에셋 테마의 기본 타이틀 색입니다.")]
    public bool CustomTitleColor = false;

    [DataInput]
    [Label("타이틀 색")]
    [HiddenIf(nameof(HideTitleColorPicker))]
    public Color TitleColor = new Color(0.75f, 0.75f, 0.80f, 1f);

    [DataInput]
    [Label("표시")]
    public bool Visible = true;

    [DataInput]
    [Label("위치")]
    public Vector2 Position = Vector2.zero;

    [DataInput]
    [Label("크기")]
    public Vector2 Size = new Vector2(220, 350);

    [DataInput]
    [Label("자동 크기")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "자식 요소에 맞춰 크기를 자동 조절합니다")]
    public bool AutoSize = false;

    [DataInput]
    [Label("방향")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Vertical = 위→아래, Horizontal = 왼→오른, Grid = 격자 자동배치")]
    public LayoutMode Direction = LayoutMode.Grid;

    [DataInput]
    [Label("정렬")]
    public FlexAlign Alignment = FlexAlign.Center;

    [DataInput]
    [Label("열 수")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "Grid 모드에서 한 줄에 들어가는 버튼 수")]
    [IntegerSlider(1, 10)]
    [HiddenIf(nameof(IsNotGrid))]
    public int Columns = 2;

    [DataInput]
    [Label("간격")]
    [IntegerSlider(0, 50)]
    public int Spacing = 8;

    [DataInput]
    [Label("안쪽 여백")]
    [IntegerSlider(0, 50)]
    public int Padding = 14;

    [DataInput]
    [Label("불투명도")]
    [FloatSlider(0f, 1f)]
    public float Opacity = 0.94f;

    [DataInput]
    [Label("버튼 크기")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "컨테이너 안 버튼들의 크기를 일괄 설정합니다")]
    public Vector2 ButtonSize = new Vector2(180, 44);

    [DataInput]
    [Label("글자 크기")]
    [IntegerSlider(10, 48)]
    public int FontSize = 15;

    [DataInput]
    [Label("버튼 목록")]
    public UIElementData[] Buttons = new UIElementData[0];

    [DataInput]
    [Label("토글 목록")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "버튼과 별개로 on/off 상태를 유지하는 토글")]
    public UIToggleElementData[] Toggles = new UIToggleElementData[0];

    [Trigger]
    [Label("버튼 목록 초기화")]
    public void RequestClearButtons()
    {
        _confirmClear = true;
        if (Parent != null)
            Parent.BroadcastDataInput("Containers");
    }

    [Markdown]
    [HiddenIf(nameof(HideConfirmClear))]
    public string _clearWarning = "⚠️ **이 컨테이너의 모든 버튼이 삭제됩니다.**";

    [Trigger]
    [HiddenIf(nameof(HideConfirmClear))]
    [Label("✔ 삭제 실행")]
    public void ConfirmClearButtons()
    {
        _confirmClear = false;
        Buttons = new UIElementData[0];
        if (Parent != null)
            Parent.SetDataInput("Containers", Parent.Containers, broadcast: true);
    }

    [Trigger]
    [HiddenIf(nameof(HideConfirmClear))]
    [Label("✖ 취소")]
    public void CancelClearButtons()
    {
        _confirmClear = false;
        if (Parent != null)
            Parent.BroadcastDataInput("Containers");
    }

    private bool _confirmClear;

    public bool HideConfirmClear() => !_confirmClear;

    [Trigger]
    [Label("토글 목록 초기화")]
    public void RequestClearToggles()
    {
        _confirmClearToggles = true;
        if (Parent != null)
            Parent.BroadcastDataInput("Containers");
    }

    [Markdown]
    [HiddenIf(nameof(HideConfirmClearToggles))]
    public string _clearTogglesWarning = "⚠️ **이 컨테이너의 모든 토글이 삭제됩니다.**";

    [Trigger]
    [HiddenIf(nameof(HideConfirmClearToggles))]
    [Label("✔ 삭제 실행")]
    public void ConfirmClearToggles()
    {
        _confirmClearToggles = false;
        Toggles = new UIToggleElementData[0];
        if (Parent != null)
            Parent.SetDataInput("Containers", Parent.Containers, broadcast: true);
    }

    [Trigger]
    [HiddenIf(nameof(HideConfirmClearToggles))]
    [Label("✖ 취소")]
    public void CancelClearToggles()
    {
        _confirmClearToggles = false;
        if (Parent != null)
            Parent.BroadcastDataInput("Containers");
    }

    private bool _confirmClearToggles;

    public bool HideConfirmClearToggles() => !_confirmClearToggles;

    public bool IsNotGrid() => Direction != LayoutMode.Grid;

    public bool HideTitleColorPicker() => !CustomTitleColor;

    public string GetHeader()
    {
        var display = string.IsNullOrEmpty(Title) ? ContainerName : $"{ContainerName} - {Title}";
        return display;
    }
}
