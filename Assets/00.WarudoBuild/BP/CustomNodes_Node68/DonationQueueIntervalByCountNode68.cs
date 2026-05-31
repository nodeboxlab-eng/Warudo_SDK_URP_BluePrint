using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 비교 개수 == 일치 개수 이면 True(일치 시 출력), 아니면 False(불일치 시 출력) 값을
    /// 「출력」 하나로보냅니다.
    /// </summary>
    [NodeType(
        Id = "e8f1a2b3-4c5d-6e7f-8a9b-0c1d2e3f4a5b",
        Title = "Donation Queue Interval By Count Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.1f
    )]
    public sealed class DonationQueueIntervalByCountNode68 : Node
    {
        [DataInput]
        [Label("비교 개수")]
        public int Count;

        [DataInput]
        [Label("일치 개수")]
        public int MatchCount;

        [DataInput]
        [Label("True 일 때 출력")]
        [Description("비교 개수와 일치 개수가 같을 때")]
        [FloatSlider(0f, 60f)]
        public float OutputWhenMatch;

        [DataInput]
        [Label("False 일 때 출력")]
        [Description("일치하지 않을 때")]
        [FloatSlider(0f, 60f)]
        public float OutputWhenNoMatch = 5f;

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyDisplayName();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyDisplayName();
        }

        private void ApplyDisplayName()
        {
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Donation Queue Interval By Count Node68"
            );
        }

        [DataOutput]
        [Label("출력")]
        [Description("True → True 일 때 출력, False → False 일 때 출력")]
        public float Output() => Count == MatchCount ? OutputWhenMatch : OutputWhenNoMatch;
    }
}
