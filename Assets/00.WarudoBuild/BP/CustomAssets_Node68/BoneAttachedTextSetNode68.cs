using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    /// <summary>
    /// <see cref="CharacterBoneAttachedTextAsset"/> 의 문자열·색·TMP 폰트 등을 플로우에서 갱신합니다.
    /// </summary>
    [NodeType(
        Id = "c7e91f44-5a3b-4d8e-9c21-8f6e4d2b1a90",
        Title = "본 부착 TMP 텍스트 설정 Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.42f
    )]
    public sealed class BoneAttachedTextSetNode68 : Node
    {
        [DataInput]
        [Label("TextDisplay Node68")]
        public CharacterBoneAttachedTextAsset Display;

        [DataInput]
        [Label("새 텍스트")]
        [MultilineInput]
        public string NewText = "";

        [DataInput]
        [Label("색 바꾸기")]
        public bool SetColor;

        [DataInput]
        [Label("색")]
        [HiddenIf(nameof(HideColorField))]
        public Color TextColor = Color.white;

        private bool HideColorField() => !SetColor;

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

        private const string ShareDisplayNameSuffix = " Shr";

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "본 부착 TMP 텍스트 설정 Node68";

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

            if (!string.IsNullOrEmpty(NewText))
                Display.SetDataInput(
                    nameof(CharacterBoneAttachedTextAsset.BodyText),
                    NewText,
                    true
                );

            if (SetColor)
                Display.SetDataInput(
                    nameof(CharacterBoneAttachedTextAsset.TextColor),
                    TextColor,
                    true
                );

            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;
    }
}
