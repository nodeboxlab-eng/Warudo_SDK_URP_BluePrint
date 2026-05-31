using System;
using Cysharp.Threading.Tasks;
using Node68.CustomAssets;
using Warudo.UIControl;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

[NodeType(
    Id = "d1a2b3c4-e5f6-7890-a1b2-c3d4e5f60001",
    Title = "UI 리모컨_Node68 버튼 클릭 시",
    Category =
        CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.UiRemoteShare
            : CustomAssetsNode68Categories.UiRemoteDev
)]
public class OnUIButtonClickNode : Node
{
    [DataInput]
    [Label("리모컨")]
    public CanvasUIAsset Canvas;

    [DataInput]
    [Label("컨테이너")]
    [Description("특정 컨테이너의 버튼만 보려면 선택하세요")]
    [AutoComplete(nameof(AutoCompleteContainerName))]
    public string ContainerName = "";

    [DataInput]
    [Label("버튼 이름")]
    [Description("비워두면 모든 버튼 클릭을 감지합니다")]
    [AutoComplete(nameof(AutoCompleteButtonName))]
    public string ButtonName = "";

    [FlowOutput]
    public Continuation Exit;

    [DataOutput]
    [Label("클릭된 버튼")]
    public string ClickedButtonName() => _lastButtonName;

    private string _lastButtonName;
    private Guid _subscriptionHandle;

    private const string ShareDisplayNameSuffix = " Shr";

    private void ApplyShareSuffixToDisplayName()
    {
        var baseName = UIControlShareNodeTypeTitles.BaseTitleForGraphDisplay(
            GetTypeMeta().NodeType.title
        );
        if (string.IsNullOrEmpty(baseName))
            baseName = "UI 리모컨_Node68 버튼 클릭 시";

        if (CustomAssetsBuildRuntime.IsShareBuild())
        {
            var core = string.IsNullOrEmpty(Name)
                ? baseName
                : UIControlShareNodeTypeTitles.StripToBaseDisplayName(Name, ShareDisplayNameSuffix);
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
                var cleaned = UIControlShareNodeTypeTitles.StripToBaseDisplayName(
                    Name,
                    ShareDisplayNameSuffix
                );
                Name = string.IsNullOrEmpty(cleaned) ? baseName : cleaned;
            }
        }
    }

    public async UniTask<AutoCompleteList> AutoCompleteContainerName()
    {
        await UniTask.CompletedTask;
        if (Canvas == null)
            return AutoCompleteList.Message("리모컨을 먼저 선택하세요");
        return Canvas.GetContainerNameList();
    }

    public async UniTask<AutoCompleteList> AutoCompleteButtonName()
    {
        await UniTask.CompletedTask;
        if (Canvas == null)
            return AutoCompleteList.Message("리모컨을 먼저 선택하세요");
        return Canvas.GetButtonNameListByContainer(ContainerName);
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        ApplyShareSuffixToDisplayName();
        _subscriptionHandle = Context.EventBus.Subscribe<UIButtonClickEvent>(OnClick);
    }

    public override void OnAllNodesDeserialized(SerializedNode serialized)
    {
        base.OnAllNodesDeserialized(serialized);
        ApplyShareSuffixToDisplayName();
    }

    protected override void OnDestroy()
    {
        Context.EventBus.Unsubscribe<UIButtonClickEvent>(_subscriptionHandle);
        base.OnDestroy();
    }

    private void OnClick(UIButtonClickEvent e)
    {
        if (Canvas == null)
            return;
        if (e.CanvasAssetId != Canvas.IdString)
            return;
        if (!string.IsNullOrEmpty(ButtonName) && e.ElementName != ButtonName)
            return;

        if (!string.IsNullOrEmpty(ContainerName))
        {
            if (!Canvas.LayoutMatchesButtonInContainer(ContainerName, e.ElementName))
                return;
        }

        _lastButtonName = e.ElementName;
        InvokeFlow(nameof(Exit));
    }
}
