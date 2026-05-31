using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "142a6e15-3242-4d1e-8848-a65ef790c86c",
        Title = "Filter Asset State Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.35f
    )]
    public sealed class FilterAssetStateNode68 : Node
    {
        [DataInput]
        [Label("StateJson")]
        public string StateJson = "";

        [DataInput]
        [Label("IncludePaths")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 전체를 대상으로 합니다. Key 또는 Path 둘 다 매칭됩니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public string[] IncludePaths = Array.Empty<string>();

        [DataInput]
        [Label("ExcludePaths")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Include 결과에서 제외할 Key 또는 Path 목록입니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public string[] ExcludePaths = Array.Empty<string>();

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataOutput]
        [Label("StateJson")]
        public string OutputStateJson() => Node68StateData.ToJson(BuildFilteredState());

        [DataOutput]
        [Label("Summary")]
        public string OutputSummary() => Node68StateValueCodec.BuildSummary(BuildFilteredState());

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Filter Asset State Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Filter Asset State Node68"
            );
        }

        private Node68StateData BuildFilteredState()
        {
            if (!Node68StateData.TryFromJson(StateJson, out var source))
                return new Node68StateData { Source = "Filter" };

            var include = BuildSet(IncludePaths);
            var exclude = BuildSet(ExcludePaths);
            var result = new Node68StateData
            {
                Version = source.Version,
                Source = string.IsNullOrWhiteSpace(source.Source)
                    ? "Filter"
                    : source.Source + "+Filter",
                Entries = new List<Node68StateEntryData>(),
            };

            foreach (var entry in source.Entries ?? new List<Node68StateEntryData>())
            {
                if (entry == null)
                    continue;

                var key = entry.Key?.Trim();
                var path = entry.Path?.Trim();
                var included =
                    include.Count == 0
                    || include.Contains(key ?? string.Empty)
                    || include.Contains(path ?? string.Empty);
                var excluded =
                    exclude.Contains(key ?? string.Empty)
                    || exclude.Contains(path ?? string.Empty);

                if (included && !excluded)
                    result.Entries.Add(CloneEntry(entry));
            }

            return result;
        }

        private static HashSet<string> BuildSet(IEnumerable<string> values)
        {
            return new HashSet<string>(
                (values ?? Array.Empty<string>())
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(v => v.Trim()),
                StringComparer.OrdinalIgnoreCase
            );
        }

        private static Node68StateEntryData CloneEntry(Node68StateEntryData entry)
        {
            return JsonConvert.DeserializeObject<Node68StateEntryData>(
                JsonConvert.SerializeObject(entry)
            );
        }
    }
}
