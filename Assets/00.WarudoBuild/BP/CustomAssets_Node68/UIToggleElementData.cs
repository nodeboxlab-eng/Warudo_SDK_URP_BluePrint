using Node68.CustomAssets;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;

/// <summary>
/// 버튼(<see cref="UIElementData"/>)과 별개인 상태 유지형 토글. 인스펙터 「토글 목록」에서 추가합니다.
/// </summary>
public class UIToggleElementData : StructuredData<CanvasUIAsset>, ICollapsibleStructuredData
{
    public string _runtimeId = System.Guid.NewGuid().ToString("N").Substring(0, 8);

    [DataInput]
    [Label("토글 이름")]
    public string ElementName = "토글";

    [DataInput]
    [Label("표시")]
    public bool Visible = true;

    [DataInput]
    [Label("토글 상태")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "현재 ON/OFF 여부입니다. 에디터(No/Yes)와 화면의 스위치가 같은 값을 씁니다. 플레이 중 스위치를 누르면 이 값도 함께 갱신됩니다."
        )]
    public bool IsOn = true;

    [DataInput]
    [Label("위치")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "자유 배치 토글 전용 (컨테이너 안에서는 무시)"
        )]
    public Vector2 Position = Vector2.zero;

    [DataInput]
    [Label("크기")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "자유 배치 토글 전용 (컨테이너 안에서는 컨테이너의 버튼 크기 사용)"
        )]
    public Vector2 Size = new Vector2(180, 44);

    public string GetHeader()
    {
        return string.IsNullOrEmpty(ElementName) ? "토글" : ElementName;
    }
}
