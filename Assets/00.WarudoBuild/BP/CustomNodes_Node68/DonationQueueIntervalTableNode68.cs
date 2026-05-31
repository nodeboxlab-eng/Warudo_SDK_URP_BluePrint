using System;
using System.Globalization;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 후원량별 큐 간격을 한 노드에서 관리합니다.
    /// 형식: 80=3, 100=6, 75=8.5 처럼 줄바꿈/쉼표/세미콜론으로 구분.
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
        [Label("시간표")]
        [Description("형식: 80=3, 100=6, 75=8.5 처럼 줄바꿈/쉼표/세미콜론으로 구분합니다.")]
        public string IntervalTable = "80=3\n100=6\n75=8.5\n111=1";

        [DataInput]
        [Label("기본 출력")]
        [Description("시간표에 없는 후원량일 때 출력할 큐 간격입니다.")]
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
        [Description("시간표에 있는 후원량이면 해당 초, 없으면 기본 출력을 보냅니다.")]
        public float Output() => TryFindInterval(Count, out var seconds) ? seconds : DefaultOutput;

        private int ComputeDisplayRevision()
        {
            unchecked
            {
                var hash = Count;
                hash = (hash * 31) + MathfRoundToInt(DefaultOutput * 100f);
                hash = (hash * 31) + (IntervalTable?.GetHashCode() ?? 0);
                return hash;
            }
        }

        private bool TryFindInterval(int count, out float seconds)
        {
            seconds = 0f;

            if (string.IsNullOrWhiteSpace(IntervalTable))
                return false;

            var entries = IntervalTable.Split(
                new[] { '\r', '\n', ',', ';' },
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (var rawEntry in entries)
            {
                var entry = rawEntry.Trim();
                if (entry.Length == 0 || entry.StartsWith("#", StringComparison.Ordinal))
                    continue;

                var separatorIndex = entry.IndexOf('=');
                if (separatorIndex < 0)
                    separatorIndex = entry.IndexOf(':');

                if (separatorIndex <= 0 || separatorIndex >= entry.Length - 1)
                    continue;

                var keyText = entry.Substring(0, separatorIndex).Trim();
                var valueText = entry.Substring(separatorIndex + 1).Trim();

                if (!int.TryParse(keyText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var key))
                    continue;

                if (key != count)
                    continue;

                if (!float.TryParse(valueText, NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
                    continue;

                seconds = Math.Max(0f, seconds);
                return true;
            }

            return false;
        }

        private void RefreshDisplay()
        {
            var matched = TryFindInterval(Count, out var seconds);
            var output = matched ? seconds : DefaultOutput;
            var status = matched ? "시간표 일치" : "기본 출력";

            Info =
                $"### 큐 간격 시간표 <br> 비교 개수: {Count} <br> 상태: {status} <br> 출력: {output:0.##}초";

            SetDataInput(nameof(Info), Info, broadcast: true);
            _displayRevision = ComputeDisplayRevision();
            Broadcast();
        }

        private static int MathfRoundToInt(float value) => (int)Math.Round(value);
    }
}
