using System;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 후원량별 큐 간격을 한 노드에서 관리합니다.
    /// </summary>
    [NodeType(
        Id = "2ac3ed76-8ef2-46d3-9f33-31b1af3ecb68",
        Title = "Donation Queue Interval Table Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.25f
    )]
    public sealed class DonationQueueIntervalTableNode68 : Node
    {
        private int _displayRevision = -1;

        [DataInput]
        [Label("비교 개수")]
        public int Count;

        [DataInput]
        [Label("1번 후원량")]
        public int Amount1 = 80;

        [DataInput]
        [Label("1번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval1 = 3f;

        [DataInput]
        [Label("2번 후원량")]
        public int Amount2 = 100;

        [DataInput]
        [Label("2번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval2 = 6f;

        [DataInput]
        [Label("3번 후원량")]
        public int Amount3 = 75;

        [DataInput]
        [Label("3번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval3 = 8.5f;

        [DataInput]
        [Label("4번 후원량")]
        public int Amount4 = 111;

        [DataInput]
        [Label("4번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval4 = 1f;

        [DataInput]
        [Label("5번 후원량")]
        public int Amount5;

        [DataInput]
        [Label("5번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval5 = 1f;

        [DataInput]
        [Label("6번 후원량")]
        public int Amount6;

        [DataInput]
        [Label("6번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval6 = 1f;

        [DataInput]
        [Label("7번 후원량")]
        public int Amount7;

        [DataInput]
        [Label("7번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval7 = 1f;

        [DataInput]
        [Label("8번 후원량")]
        public int Amount8;

        [DataInput]
        [Label("8번 큐 간격")]
        [FloatSlider(0f, 60f)]
        public float Interval8 = 1f;

        [DataInput]
        [Label("기본 출력")]
        [Description("슬롯에 없는 후원량일 때 출력할 큐 간격입니다.")]
        [FloatSlider(0f, 60f)]
        public float DefaultOutput = 1f;

        [DataInput]
        [Label("Info")]
        [Markdown(primary: true)]
        public string Info;

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyDisplayName();
            RefreshDisplay();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyDisplayName();
            RefreshDisplay();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var revision = ComputeDisplayRevision();
            if (revision == _displayRevision)
                return;

            _displayRevision = revision;
            RefreshDisplay();
        }

        private void ApplyDisplayName()
        {
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Donation Queue Interval Table Node68"
            );
        }

        [DataOutput]
        [Label("출력")]
        [Description("슬롯에 있는 후원량이면 해당 초, 없으면 기본 출력을 보냅니다.")]
        public float Output() => TryFindInterval(Count, out var seconds) ? seconds : DefaultOutput;

        private int ComputeDisplayRevision()
        {
            unchecked
            {
                var hash = Count;
                hash = (hash * 31) + MathfRoundToInt(DefaultOutput * 100f);
                hash = (hash * 31) + Amount1;
                hash = (hash * 31) + MathfRoundToInt(Interval1 * 100f);
                hash = (hash * 31) + Amount2;
                hash = (hash * 31) + MathfRoundToInt(Interval2 * 100f);
                hash = (hash * 31) + Amount3;
                hash = (hash * 31) + MathfRoundToInt(Interval3 * 100f);
                hash = (hash * 31) + Amount4;
                hash = (hash * 31) + MathfRoundToInt(Interval4 * 100f);
                hash = (hash * 31) + Amount5;
                hash = (hash * 31) + MathfRoundToInt(Interval5 * 100f);
                hash = (hash * 31) + Amount6;
                hash = (hash * 31) + MathfRoundToInt(Interval6 * 100f);
                hash = (hash * 31) + Amount7;
                hash = (hash * 31) + MathfRoundToInt(Interval7 * 100f);
                hash = (hash * 31) + Amount8;
                hash = (hash * 31) + MathfRoundToInt(Interval8 * 100f);
                return hash;
            }
        }

        private bool TryFindInterval(int count, out float seconds)
        {
            seconds = 0f;

            return TryMatch(count, Amount1, Interval1, out seconds)
                || TryMatch(count, Amount2, Interval2, out seconds)
                || TryMatch(count, Amount3, Interval3, out seconds)
                || TryMatch(count, Amount4, Interval4, out seconds)
                || TryMatch(count, Amount5, Interval5, out seconds)
                || TryMatch(count, Amount6, Interval6, out seconds)
                || TryMatch(count, Amount7, Interval7, out seconds)
                || TryMatch(count, Amount8, Interval8, out seconds);
        }

        private static bool TryMatch(int count, int amount, float interval, out float seconds)
        {
            seconds = 0f;
            if (amount <= 0 || count != amount)
                return false;

            seconds = Math.Max(0f, interval);
            return true;
        }

        private void RefreshDisplay()
        {
            var matched = TryFindInterval(Count, out var seconds);
            var output = matched ? seconds : DefaultOutput;
            var status = matched ? "슬롯 일치" : "기본 출력";

            Info =
                $"### 큐 간격 슬롯 <br> 비교 개수: {Count} <br> 상태: {status} <br> 출력: {output:0.##}초";

            SetDataInput(nameof(Info), Info, broadcast: true);
            _displayRevision = ComputeDisplayRevision();
            Broadcast();
        }

        private static int MathfRoundToInt(float value) => (int)Math.Round(value);
    }
}
