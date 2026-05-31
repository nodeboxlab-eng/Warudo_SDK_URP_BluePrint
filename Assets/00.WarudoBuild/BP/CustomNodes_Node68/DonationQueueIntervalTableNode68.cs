using System;
using System.Collections.Generic;
using System.Linq;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
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
        [Label("Cases")]
        public string CaseInput = "80,100,75,111";

        [DataInput]
        [Hidden]
        public int[] Cases = { 80, 100, 75, 111 };

        [DataInput]
        [Label("기본 출력")]
        public float DefaultOutput = 1f;

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyDisplayName();
            SetupIntervalPorts();
            RefreshDisplay();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            MigrateLegacyCases(serialized);
            ApplyDisplayName();
            SetupIntervalPorts();
            RestoreIntervalPortValues(serialized);
            RefreshDisplay();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            SetupIntervalPorts();

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
        public float Output() => TryFindInterval(Count, out var seconds) ? seconds : DefaultOutput;

        private int ComputeDisplayRevision()
        {
            unchecked
            {
                var hash = Count;
                hash = (hash * 31) + MathfRoundToInt(DefaultOutput * 100f);
                foreach (var amount in GetNormalizedCases())
                {
                    hash = (hash * 31) + amount;
                    hash = (hash * 31) + MathfRoundToInt(GetIntervalForCase(amount) * 100f);
                }
                return hash;
            }
        }

        private bool TryFindInterval(int count, out float seconds)
        {
            seconds = 0f;

            foreach (var amount in GetNormalizedCases())
            {
                if (count != amount)
                    continue;

                seconds = GetIntervalForCase(amount);
                return true;
            }

            return false;
        }

        private void MigrateLegacyCases(SerializedNode serialized)
        {
            if (serialized?.dataInputs == null)
                return;

            if (!serialized.dataInputs.ContainsKey(nameof(Cases))
                || serialized.dataInputs.ContainsKey(nameof(CaseInput))
                || Cases == null
                || Cases.Length == 0)
                return;

            CaseInput = string.Join(",", Cases.Where(amount => amount > 0).Distinct());
            SetDataInput(nameof(CaseInput), CaseInput, broadcast: true);
        }

        private void SetupIntervalPorts()
        {
            var changed = false;
            var desiredKeys = new HashSet<string>(
                GetNormalizedCases().Select(GetIntervalPortKey),
                StringComparer.Ordinal
            );

            var existingKeys = DataInputPortCollection
                .GetPorts()
                .Keys
                .Where(key => key.StartsWith(IntervalPortPrefix, StringComparison.Ordinal))
                .ToArray();

            foreach (var key in existingKeys)
            {
                if (!desiredKeys.Contains(key))
                {
                    DataInputPortCollection.RemovePort(key);
                    changed = true;
                }
            }

            foreach (var amount in GetNormalizedCases())
            {
                var key = GetIntervalPortKey(amount);
                if (DataInputPortCollection.ContainsPort(key))
                    continue;

                AddDataInputPort(
                    key,
                    typeof(float),
                    GetDefaultInterval(amount),
                    new DataInputProperties
                    {
                        label = $"{amount} 큐 간격",
                        order = 2000f + amount,
                    }
                );
                changed = true;
            }

            if (changed)
                Broadcast();
        }

        private void RestoreIntervalPortValues(SerializedNode serialized)
        {
            if (serialized?.dataInputs == null)
                return;

            foreach (var (key, serializedPort) in serialized.dataInputs)
            {
                if (!key.StartsWith(IntervalPortPrefix, StringComparison.Ordinal))
                    continue;

                var port = DataInputPortCollection.GetPort(key);
                if (port == null)
                    continue;

                port.SetSerializedValue(serializedPort.value, Graph.Scene, this);
            }
        }

        private IEnumerable<int> GetNormalizedCases()
        {
            var source = string.IsNullOrWhiteSpace(CaseInput)
                ? string.Join(",", Cases ?? Array.Empty<int>())
                : CaseInput;

            return source
                .Split(new[] { ',', ' ', '\n', '\r', '\t', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(token => int.TryParse(token.Trim(), out var amount) ? amount : 0)
                .Where(amount => amount > 0)
                .Distinct();
        }

        private float GetIntervalForCase(int amount)
        {
            var port = DataInputPortCollection.GetPort(GetIntervalPortKey(amount));
            if (port?.Getter() is float value)
                return Math.Max(0f, value);

            return GetDefaultInterval(amount);
        }

        private static float GetDefaultInterval(int amount) =>
            amount switch
            {
                75 => 8.5f,
                80 => 3f,
                100 => 6f,
                111 => 1f,
                _ => 1f,
            };

        private void RefreshDisplay()
        {
            _displayRevision = ComputeDisplayRevision();
            Broadcast();
        }

        private static int MathfRoundToInt(float value) => (int)Math.Round(value);

        private const string IntervalPortPrefix = "Interval_";

        private static string GetIntervalPortKey(int amount) => IntervalPortPrefix + amount;
    }
}
