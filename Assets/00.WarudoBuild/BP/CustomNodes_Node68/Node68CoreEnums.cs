// Node68 모드 — 공용 enum 모음 (UI Label·노드/에셋 어디서나 참조).
// TextDisplay enum 은 CustomAssets_Node68/Build/Node68TextDisplayEnums.cs 로 이동.

using Warudo.Core.Attributes;

namespace Node68.CustomNodes
{
    /// <summary>GameObjectAsset 트랜스폼·본에 월드 공간 텍스트를 붙입니다 (TextMesh Pro · SDF).</summary>
    public enum BoneTextAttachMode
    {
        [Label("휴머노이드 본")]
        HumanoidBone = 0,

        [Label("트랜스폼 경로")]
        TransformPath = 1,

        [Label("에셋 트랜스폼 (루트)")]
        AssetTransform = 2,
    }

}
