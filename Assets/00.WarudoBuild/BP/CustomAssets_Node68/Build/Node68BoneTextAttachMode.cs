using Warudo.Core.Attributes;

namespace Node68.CustomAssets
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
