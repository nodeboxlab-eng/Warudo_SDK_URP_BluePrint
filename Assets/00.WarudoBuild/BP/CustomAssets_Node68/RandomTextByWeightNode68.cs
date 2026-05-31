using System;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomAssets
{
    /// <summary>
    /// 등록된 문자열 항목을 Weight 가중치에 따라 랜덤으로 하나 선택합니다.
    /// </summary>
    [NodeType(
        Id = "b2e8f4a6-3c1d-4e9f-8a7b-5d6c3e2f1a08",
        Title = "Random Text By Weight Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev,
        Width = 1.25f
    )]
    public sealed class RandomTextByWeightNode68 : Node
    {
        private bool HideInShareBuild() => CustomAssetsBuildRuntime.IsShareBuild();

        [DataInput]
        [Label("Items")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild
                ? ""
                : "Text·Weight 목록. Weight가 클수록 선택 확률이 높습니다."
        )]
        [HiddenIf(nameof(HideInShareBuild))]
        public WeightedTextItem[] Items = Array.Empty<WeightedTextItem>();

        private string _lastPickedText = "";

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
                baseName = "Random Text By Weight Node68";

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
        [Label("Pick")]
        public Continuation Pick()
        {
            _lastPickedText = PickWeightedText(Items);
            return OnPicked;
        }

        [FlowOutput]
        [Label("On Picked")]
        public Continuation OnPicked;

        [DataOutput]
        [Label("ResultText")]
        public string ResultText() => _lastPickedText ?? "";

        private static string PickWeightedText(WeightedTextItem[] items)
        {
            try
            {
                if (items == null || items.Length == 0)
                {
                    LogNoSelectableItems();
                    return "";
                }

                var totalWeight = 0f;
                for (var i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item == null)
                        continue;
                    if (item.Weight <= 0f)
                        continue;
                    totalWeight += item.Weight;
                }

                if (totalWeight <= 0f)
                {
                    LogNoSelectableItems();
                    return "";
                }

                var roll = UnityEngine.Random.Range(0f, totalWeight);
                var cumulative = 0f;

                for (var i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item == null)
                        continue;
                    if (item.Weight <= 0f)
                        continue;

                    cumulative += item.Weight;
                    if (roll < cumulative)
                        return item.Text ?? "";
                }

                for (var i = items.Length - 1; i >= 0; i--)
                {
                    var item = items[i];
                    if (item == null || item.Weight <= 0f)
                        continue;
                    return item.Text ?? "";
                }

                LogNoSelectableItems();
                return "";
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    $"[Random Text By Weight Node68] 추첨 중 오류: {ex.Message}"
                );
                return "";
            }
        }

        private static void LogNoSelectableItems()
        {
            Debug.LogWarning(
                "[Random Text By Weight Node68] 선택 가능한 항목이 없습니다 (목록이 비었거나 모든 Weight가 0입니다)."
            );
        }

        public sealed class WeightedTextItem
            : StructuredData<RandomTextByWeightNode68>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("Text")]
            public string Text = "";

            [DataInput]
            [Label("Weight")]
            [FloatSlider(0f, 9999f)]
            public float Weight = 1f;

            public string GetHeader()
            {
                var preview = Text ?? "";
                if (preview.Length > 28)
                    preview = preview.Substring(0, 28) + "…";
                if (string.IsNullOrEmpty(preview))
                    preview = "(empty)";
                return $"{preview} · w={Mathf.Max(0f, Weight):0.##}";
            }
        }
    }
}
