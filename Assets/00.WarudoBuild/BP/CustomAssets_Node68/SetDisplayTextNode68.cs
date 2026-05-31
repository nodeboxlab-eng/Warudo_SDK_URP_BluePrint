using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    /// <summary>
    /// <see cref="CharacterBoneAttachedTextAsset"/> 의 「내용」만 플로우에서 덮어씁니다 (빈 문자열 허용).
    /// </summary>
    [NodeType(
        Id = "7a3d9e1c-5b2f-4a8e-b6c1-0e9f8d7c6b5a",
        Title = "Set Display Text Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.35f
    )]
    public sealed class SetDisplayTextNode68 : Node
    {
        [DataInput]
        [Label("TextDisplay Node68")]
        public CharacterBoneAttachedTextAsset Display;

        [DataInput]
        [Label("새 텍스트")]
        [MultilineInput]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "실행 시 항상 에셋 「내용」에 반영됩니다. 비우면 빈 문자열로 덮어씁니다.")]
        public string NewText = "";

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
                baseName = "Set Display Text Node68";

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

        [FlowInput]
        public Continuation Enter()
        {
            if (Display == null)
                return Exit;

            Display.SetDataInput(
                nameof(CharacterBoneAttachedTextAsset.BodyText),
                NewText ?? "",
                true
            );

            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;
    }
}
