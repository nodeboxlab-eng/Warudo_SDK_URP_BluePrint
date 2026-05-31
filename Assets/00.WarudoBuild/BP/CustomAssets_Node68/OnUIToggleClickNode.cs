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
    Id = "d1a2b3c4-e5f6-7890-a1b2-c3d4e5f60020",
    Title = "UI 리모컨_Node68 토글 클릭 시",
    Category =
        CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.UiRemoteShare
            : CustomAssetsNode68Categories.UiRemoteDev
)]
public class OnUIToggleClickNode : Node
{
    [DataInput]
    [Label("리모컨")]
    public CanvasUIAsset Canvas;

    [DataInput]
    [Label("컨테이너")]
    [Description("특정 컨테이너의 토글만 보려면 선택하세요")]
    [AutoComplete(nameof(AutoCompleteContainerName))]
    public string ContainerName = "";

    [DataInput]
    [Label("토글 이름")]
    [Description("비워두면 모든 토글 변경을 감지합니다")]
    [AutoComplete(nameof(AutoCompleteToggleName))]
    public string ToggleName = "";

    [FlowOutput]
    public Continuation Exit;

    [DataOutput]
    [Label("토글 이름")]
    public string ChangedToggleName() => _lastToggleName;

    [DataOutput]
    [Label("토글 상태")]
    public bool ToggleIsOn() => _lastIsOn;

    private string _lastToggleName;
    private bool _lastIsOn;
    private Guid _subscriptionHandle;

    private const string ShareDisplayNameSuffix = " Shr";

    /// <summary>
    /// 쉐어 빌드: 팔레트 제목은 <c> 쉐어</c>, 그래프 타일 이름은 <c> Shr</c>.
    /// </summary>
    private void ApplyShareSuffixToDisplayName()
    {
        var baseName = UIControlShareNodeTypeTitles.BaseTitleForGraphDisplay(
            GetTypeMeta().NodeType.title
        );
        if (string.IsNullOrEmpty(baseName))
            baseName = "UI 리모컨_Node68 토글 클릭 시";

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

    protected override void OnCreate()
    {
        base.OnCreate();
        ApplyShareSuffixToDisplayName();
        _subscriptionHandle = Context.EventBus.Subscribe<UIToggleValueChangedEvent>(OnToggle);
    }

    public override void OnAllNodesDeserialized(SerializedNode serialized)
    {
        base.OnAllNodesDeserialized(serialized);
        ApplyShareSuffixToDisplayName();
    }

    public async UniTask<AutoCompleteList> AutoCompleteContainerName()
    {
        await UniTask.CompletedTask;
        if (Canvas == null)
            return AutoCompleteList.Message("리모컨을 먼저 선택하세요");
        return Canvas.GetContainerNameList();
    }

    public async UniTask<AutoCompleteList> AutoCompleteToggleName()
    {
        await UniTask.CompletedTask;
        if (Canvas == null)
            return AutoCompleteList.Message("리모컨을 먼저 선택하세요");
        return Canvas.GetToggleNameListByContainer(ContainerName);
    }

    protected override void OnDestroy()
    {
        Context.EventBus.Unsubscribe<UIToggleValueChangedEvent>(_subscriptionHandle);
        base.OnDestroy();
    }

    private void OnToggle(UIToggleValueChangedEvent e)
    {
        if (Canvas == null)
            return;
        if (e.CanvasAssetId != Canvas.IdString)
            return;
        if (!string.IsNullOrEmpty(ToggleName) && e.ElementName != ToggleName)
            return;

        if (!string.IsNullOrEmpty(ContainerName))
        {
            if (!Canvas.LayoutMatchesToggleInContainer(ContainerName, e.ElementName))
                return;
        }

        _lastToggleName = e.ElementName;
        _lastIsOn = e.IsOn;
        InvokeFlow(nameof(Exit));
    }
}
