using Node68.CustomAssets;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;

public class UIElementData : StructuredData<CanvasUIAsset>, ICollapsibleStructuredData
{
    public string _runtimeId = System.Guid.NewGuid().ToString("N").Substring(0, 8);

    [DataInput]
    [Label("버튼 이름")]
    public string ElementName = "버튼";

    [DataInput]
    [Label("표시")]
    public bool Visible = true;

    [DataInput]
    [Label("켜짐 (색상)")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "꺼짐일 때 버튼 배경·글자를 중립 회색으로 표시합니다")]
    public bool StateOn = true;

    [DataInput]
    [Label("클릭 시 색상 토글")]
    [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "클릭할 때마다 켜짐/꺼짐을 뒤바꿉니다. 블루프린트로 같은 버튼 상태를 바꾸는 노드와 동시에 쓰면 겹칠 수 있습니다")]
    public bool ToggleStateOnClick = false;

    [DataInput]
    [Label("위치")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "자유 배치 버튼 전용 (컨테이너 안에서는 무시)"
        )]
    public Vector2 Position = Vector2.zero;

    [DataInput]
    [Label("크기")]
    [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "자유 배치 버튼 전용 (컨테이너 안에서는 컨테이너 설정 사용)"
        )]
    public Vector2 Size = new Vector2(180, 44);

    public string GetHeader()
    {
        return string.IsNullOrEmpty(ElementName) ? "버튼" : ElementName;
    }
}
