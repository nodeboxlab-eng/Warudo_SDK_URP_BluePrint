using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using UnityEngine;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// Graph Variable 을 리스트 형식으로 한 번에 정의·생성합니다. (DevKit 전용)
    /// </summary>
    [NodeType(
        Id = "f3a8c2d1-9e4b-4f7a-8c6d-2b5e1a9f3c70",
        Title = "Create Graph Variables Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.45f
    )]
    public sealed class CreateGraphVariablesNode68 : Node
    {
        [DataInput]
        [Label("Target Graph")]
        [Description("비우면 이 노드가 있는 현재 블루프린트 Graph Variable 을 사용합니다.")]
        [AutoComplete(nameof(AutoCompleteTargetGraph))]
        public string TargetGraph;

        [DataInput]
        [Label("변수 목록")]
        [Description("이름·타입·초기값을 항목마다 지정합니다. + 버튼으로 항목을 추가하세요.")]
        public GraphVariableSpecEntry68[] Variables = Array.Empty<GraphVariableSpecEntry68>();

        [DataInput]
        [Label("이미 있으면 건너뜀")]
        [Description("켜면 동일 이름 변수가 이미 있을 때 새로 만들지 않습니다.")]
        public bool SkipIfExists = true;

        [DataInput]
        [Label("초기값 적용")]
        [Description("켜면 각 항목의 VALUE 필드를 Graph Variable 초기값으로 설정합니다.")]
        public bool ApplyInitialValues = true;

        [Markdown]
        [Hidden]
        public string _note =
            "Graph Properties → Variables 에 항목을 일괄 추가합니다. "
            + "타입별 VALUE 필드는 TYPE 선택에 따라 표시됩니다.";

        [FlowInput]
        public Continuation Enter()
        {
            ExecuteBatch();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("생성 수")]
        public int OutputCreatedCount() => _lastCreated;

        [DataOutput]
        [Label("건너뜀 수")]
        public int OutputSkippedCount() => _lastSkipped;

        [DataOutput]
        [Label("실패 수")]
        public int OutputFailedCount() => _lastFailed;

        [DataOutput]
        [Label("결과")]
        public string OutputSummary() => _lastSummary;

        private int _lastCreated;
        private int _lastSkipped;
        private int _lastFailed;
        private string _lastSummary = "—";

        private void ExecuteBatch()
        {
            var specs = BuildSpecs(Variables);
            var result = CreateGraphVariablesDevKitHelper.CreateVariablesBatch(
                this,
                TargetGraph,
                specs,
                SkipIfExists,
                ApplyInitialValues
            );

            _lastCreated = result.Created;
            _lastSkipped = result.Skipped;
            _lastFailed = result.Failed;
            _lastSummary = result.Summary;
            Broadcast();
        }

        private static List<CreateGraphVariablesDevKitHelper.GraphVariableCreateSpec> BuildSpecs(
            GraphVariableSpecEntry68[] entries
        )
        {
            if (entries == null || entries.Length == 0)
                return new List<CreateGraphVariablesDevKitHelper.GraphVariableCreateSpec>(0);

            var list = new List<CreateGraphVariablesDevKitHelper.GraphVariableCreateSpec>(
                entries.Length
            );
            for (var i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];
                if (entry == null)
                    continue;

                list.Add(
                    new CreateGraphVariablesDevKitHelper.GraphVariableCreateSpec
                    {
                        Name = entry.Name,
                        VariableType = entry.VariableType,
                        BooleanValue = entry.BooleanValue,
                        IntegerValue = entry.IntegerValue,
                        FloatValue = entry.FloatValue,
                        StringValue = entry.StringValue,
                        Vector3Value = entry.Vector3Value,
                        BooleanListValue = entry.BooleanListValue,
                        IntegerListValue = entry.IntegerListValue,
                        FloatListValue = entry.FloatListValue,
                        StringListValue = entry.StringListValue,
                        Vector3ListValue = entry.Vector3ListValue,
                    }
                );
            }

            return list;
        }

        protected UniTask<AutoCompleteList> AutoCompleteTargetGraph() =>
            CreateGraphVariablesDevKitHelper.AutoCompleteTargetGraph(this);

        public sealed class GraphVariableSpecEntry68
            : StructuredData<CreateGraphVariablesNode68>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("Name")]
            public string Name = "NewVariable";

            [DataInput]
            [Label("Type")]
            public GraphVariableType VariableType;

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideBooleanValue))]
            public bool BooleanValue;

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideIntegerValue))]
            public int IntegerValue;

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideFloatValue))]
            public float FloatValue;

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideStringValue))]
            public string StringValue = "";

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideVector3Value))]
            public Vector3 Vector3Value;

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideBooleanListValue))]
            public bool[] BooleanListValue = Array.Empty<bool>();

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideIntegerListValue))]
            public int[] IntegerListValue = Array.Empty<int>();

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideFloatListValue))]
            public float[] FloatListValue = Array.Empty<float>();

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideStringListValue))]
            public string[] StringListValue = Array.Empty<string>();

            [DataInput]
            [Label("Value")]
            [HiddenIf(nameof(HideVector3ListValue))]
            public Vector3[] Vector3ListValue = Array.Empty<Vector3>();

            protected bool HideBooleanValue() => VariableType != GraphVariableType.Boolean;

            protected bool HideIntegerValue() => VariableType != GraphVariableType.Integer;

            protected bool HideFloatValue() => VariableType != GraphVariableType.Float;

            protected bool HideStringValue() => VariableType != GraphVariableType.String;

            protected bool HideVector3Value() => VariableType != GraphVariableType.Vector3;

            protected bool HideBooleanListValue() =>
                VariableType != GraphVariableType.BooleanList;

            protected bool HideIntegerListValue() => VariableType != GraphVariableType.IntegerList;

            protected bool HideFloatListValue() => VariableType != GraphVariableType.FloatList;

            protected bool HideStringListValue() => VariableType != GraphVariableType.StringList;

            protected bool HideVector3ListValue() =>
                VariableType != GraphVariableType.Vector3List;

            public string GetHeader()
            {
                var typeLabel = VariableType switch
                {
                    GraphVariableType.BooleanList => "Boolean[]",
                    GraphVariableType.IntegerList => "Integer[]",
                    GraphVariableType.FloatList => "Float[]",
                    GraphVariableType.StringList => "String[]",
                    GraphVariableType.Vector3List => "Vector3[]",
                    _ => VariableType.ToString(),
                };

                var displayName = string.IsNullOrWhiteSpace(Name) ? "(이름 없음)" : Name.Trim();
                return $"{displayName} · {typeLabel}";
            }
        }
    }
}
