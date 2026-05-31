using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "30a9d3c2-5a7d-4957-8f98-87f6ca33fd5e",
        Title = "Camera State Preset Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.35f
    )]
    public sealed class CameraStatePresetNode68 : Node
    {
        [DataInput]
        [Label("Enabled")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeEnabled = true;

        [DataInput]
        [Label("Transform")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeTransform = true;

        [DataInput]
        [Label("Controls")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeControls = true;

        [DataInput]
        [Label("Basic Properties")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeBasicProperties = true;

        [DataInput]
        [Label("Effects")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeEffects = false;

        [DataInput]
        [Label("ExtraEntries")]
        [HiddenIf(nameof(HideInShareBuild))]
        public CameraStateFieldEntry[] ExtraEntries = Array.Empty<CameraStateFieldEntry>();

        [DataInput]
        [Label("ExcludePaths")]
        [HiddenIf(nameof(HideInShareBuild))]
        public string[] ExcludePaths = Array.Empty<string>();

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Camera State Preset Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Camera State Preset Node68"
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
            var excluded = new HashSet<string>(
                (ExcludePaths ?? Array.Empty<string>())
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Select(p => p.Trim()),
                StringComparer.OrdinalIgnoreCase
            );

            void Add(Node68StateEntryConfig entry)
            {
                if (entry == null || !entry.Enabled || string.IsNullOrWhiteSpace(entry.DataPath))
                    return;

                var path = entry.DataPath.Trim();
                if (excluded.Contains(path))
                    return;

                data.Entries.Add(
                    new Node68StateEntryConfig
                    {
                        Enabled = true,
                        DataPath = path,
                        ValueType = entry.ValueType,
                        Key = entry.Key,
                    }
                );
            }

            foreach (var entry in BuildGroupEntries())
                Add(entry);

            foreach (var entry in ExtraEntries ?? Array.Empty<CameraStateFieldEntry>())
            {
                if (entry == null)
                    continue;

                Add(
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

        private IEnumerable<Node68StateEntryConfig> BuildGroupEntries()
        {
            if (IncludeEnabled)
                yield return Node68StateFieldCatalog.CreateEntry("Enabled");

            if (IncludeTransform)
            {
                yield return Node68StateFieldCatalog.CreateEntry("Transform.Position");
                yield return Node68StateFieldCatalog.CreateEntry("Transform.Rotation");
            }

            if (IncludeControls)
            {
                yield return Node68StateFieldCatalog.CreateEntry("ControlMode");
                yield return Node68StateFieldCatalog.CreateEntry("OrbitRotation");
                yield return Node68StateFieldCatalog.CreateEntry("OrbitOffset");
            }

            if (IncludeBasicProperties)
            {
                yield return Node68StateFieldCatalog.CreateEntry("FieldOfView");
                yield return Node68StateFieldCatalog.CreateEntry("TransparentBackground");
                yield return Node68StateFieldCatalog.CreateEntry("RenderCharacters");
                yield return Node68StateFieldCatalog.CreateEntry("RenderEnvironment");
                yield return Node68StateFieldCatalog.CreateEntry("RenderScreen");
            }

            if (IncludeEffects)
            {
                foreach (var entry in Node68StateFieldCatalog.CreateEntriesForGroups(
                    Node68StateFieldCatalog.GroupEffects
                ))
                    yield return entry;
            }
        }

        public sealed class CameraStateFieldEntry
            : StructuredData<CameraStatePresetNode68>,
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
