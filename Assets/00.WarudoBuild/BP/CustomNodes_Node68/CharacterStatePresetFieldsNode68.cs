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
        Id = "7b53b4f4-faa5-47f8-9c93-0c19a38376f5",
        Title = "Character State Preset Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.35f
    )]
    public sealed class CharacterStatePresetFieldsNode68 : Node
    {
        [DataInput]
        [Label("Animation")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeAnimation = true;

        [DataInput]
        [Label("Look At IK")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool IncludeLookAtIK = true;

        [DataInput]
        [Label("ExtraEntries")]
        [HiddenIf(nameof(HideInShareBuild))]
        public CharacterStateFieldEntry[] ExtraEntries = Array.Empty<CharacterStateFieldEntry>();

        [DataInput]
        [Label("ExcludePaths")]
        [HiddenIf(nameof(HideInShareBuild))]
        public string[] ExcludePaths = Array.Empty<string>();

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataOutput]
        [Label("FieldsJson")]
        public string OutputFieldsJson() => Node68StateFieldListData.ToJson(BuildFieldListData());

        [DataOutput]
        [Label("Summary")]
        public string OutputSummary()
        {
            var specs = Node68StateFieldListHelper.BuildEntrySpecs(BuildFieldListData());
            if (specs.Count == 0)
                return "0 entries";

            return $"{specs.Count} entries\n{string.Join(", ", specs.Select(s => s.Path))}";
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Character State Preset Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Character State Preset Node68"
            );
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

            foreach (var entry in ExtraEntries ?? Array.Empty<CharacterStateFieldEntry>())
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
            if (IncludeAnimation)
            {
                foreach (var path in AnimationPaths)
                    yield return Node68StateFieldCatalog.CreateEntry(path);
            }

            if (IncludeLookAtIK)
            {
                foreach (var path in LookAtIkPaths)
                    yield return Node68StateFieldCatalog.CreateEntry(path);
            }
        }

        internal static readonly string[] AnimationPaths =
        {
            "DefaultIdleAnimation",
            "OverrideHandPoses",
            "BreathingEnabled",
            "BreathingExertion",
            "BreathingRate",
            "SwayingEnabled",
            "SwayingIntensity",
            "OverlappingAnimationTransitionTime",
            "AdditionalBoneOffsets",
        };

        internal static readonly string[] LookAtIkPaths =
        {
            "ApplyFootIK",
            "DisableUnityRetargeting",
            "LookAtEnabled",
            "LookAtTarget",
            "LookAtWeight",
            "LookAtEyesWeight",
            "LookAtHeadWeight",
            "LookAtBodyWeight",
            "LookAtClampEyesWeight",
            "LookAtClampHeadWeight",
            "LookAtClampBodyWeight",
            "SpineIK",
            "LeftHandIK",
            "RightHandIK",
            "LeftFootIK",
            "RightFootIK",
        };

        public sealed class CharacterStateFieldEntry
            : StructuredData<CharacterStatePresetFieldsNode68>,
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
            public Node68StateValueType ValueType = Node68StateValueType.Auto;

            [DataInput]
            [Label("Key")]
            public string Key = "";

            public string GetHeader()
            {
                var path = string.IsNullOrWhiteSpace(DataPath) ? "(empty)" : DataPath.Trim();
                return (Enabled ? "" : "OFF ") + Node68StateFieldCatalog.GetLabel(path);
            }

            public UniTask<AutoCompleteList> AutoCompleteDataPath()
            {
                return UniTask.FromResult(Node68StateFieldCatalog.BuildAutoCompleteList());
            }
        }
    }
}
