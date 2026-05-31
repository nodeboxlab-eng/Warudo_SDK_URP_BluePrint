using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    /// <summary>
    /// <see cref="CharacterBoneAttachedTextAsset"/> 의 「내용」데이터 입력 값을 그래프에서 읽습니다.
    /// </summary>
    [NodeType(
        Id = "4f6b8c2e-1a0d-4e7f-9c33-7d2e8f1a9b44",
        Title = "Get Display Text Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.2f
    )]
    public sealed class GetDisplayTextNode68 : Node
    {
        [DataInput]
        [Label("TextDisplay Node68")]
        public CharacterBoneAttachedTextAsset Display;

        [DataOutput]
        [Label("표시 텍스트")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "에셋 「내용」필드와 동일합니다. 연결이 없으면 빈 문자열입니다.")]
        public string DisplayText()
        {
            if (Display == null)
                return "";
            return Display.GetDataInput<string>(nameof(CharacterBoneAttachedTextAsset.BodyText))
                ?? "";
        }

        private const string ShareDisplayNameSuffix = " Shr";

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyShareSuffixToDisplayName();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyShareSuffixToDisplayName();
        }

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Get Display Text Node68";

            if (CustomAssetsBuildRuntime.IsShareBuild())
            {
                var core = string.IsNullOrEmpty(Name)
                    ? baseName
                    : Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
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
                    var cleaned = Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
                    Name = string.IsNullOrEmpty(cleaned) ? baseName : cleaned;
                }
            }
        }
    }
}
