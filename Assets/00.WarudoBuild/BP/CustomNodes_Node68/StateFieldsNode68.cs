using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "c3d5de43-5385-4a6c-a51b-d2b5adcd8d68",
        Title = "State Fields Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.35f
    )]
    public sealed class StateFieldsNode68 : Node
    {
        [DataInput]
        [Label("Entries")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "저장할 Asset DataPath 목록입니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public StateFieldEntry[] Entries = Array.Empty<StateFieldEntry>();

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "State Fields Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "State Fields Node68"
            );
        }

        [DataOutput]
        [Label("FieldsJson")]
        public string OutputFieldsJson() => Node68StateFieldListData.ToJson(BuildFieldListData());

        [DataOutput]
        [Label("Summary")]
        public string OutputSummary()
        {
            var data = BuildFieldListData();
            var specs = Node68StateFieldListHelper.BuildEntrySpecs(data);
            if (specs.Count == 0)
                return "0 entries";

            var paths = string.Join(", ", specs.Select(s => s.Path));
            return $"{specs.Count} entries\n{paths}";
        }

        private Node68StateFieldListData BuildFieldListData()
        {
            var data = new Node68StateFieldListData();

            foreach (var entry in Entries ?? Array.Empty<StateFieldEntry>())
            {
                if (entry == null)
                    continue;

                data.Entries.Add(
                    new Node68StateEntryConfig
                    {
                        Enabled = entry.Enabled,
                        DataPath = entry.DataPath,
                        ValueType = entry.ValueType,
                        Key = entry.Key,
                    }
                );
            }

            return data;
        }

        public sealed class StateFieldEntry
            : StructuredData<StateFieldsNode68>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("Enabled")]
            public bool Enabled = true;

            [DataInput]
            [Label("DataPath")]
            [AutoComplete(nameof(AutoCompleteDataPath))]
            public string DataPath = "";

            [DataInput]
            [Label("Type")]
            [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Auto 로 두면 DataPath 선택값에 맞춰 자동 결정합니다.")]
            public Node68StateValueType ValueType = Node68StateValueType.Auto;

            [DataInput]
            [Label("Key")]
            [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 DataPath 를 Key 로 사용합니다.")]
            public string Key = "";

            public string GetHeader()
            {
                var path = string.IsNullOrWhiteSpace(DataPath) ? "(empty)" : DataPath.Trim();
                var label = Node68StateFieldCatalog.GetLabel(path);
                return (Enabled ? "" : "OFF ") + label + " · " + ValueType;
            }

            public UniTask<AutoCompleteList> AutoCompleteDataPath()
            {
                return UniTask.FromResult(Node68StateFieldCatalog.BuildAutoCompleteList());
            }
        }
    }
}
