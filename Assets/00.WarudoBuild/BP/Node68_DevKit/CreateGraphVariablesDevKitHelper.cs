using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Data;
using Warudo.Core.Graphs;

namespace Node68.ToolkitMods.Node68DevKit
{
    internal static class CreateGraphVariablesDevKitHelper
    {
        internal sealed class GraphVariableCreateSpec
        {
            internal string Name;
            internal GraphVariableType VariableType;
            internal bool BooleanValue;
            internal int IntegerValue;
            internal float FloatValue;
            internal string StringValue;
            internal Vector3 Vector3Value;
            internal bool[] BooleanListValue;
            internal int[] IntegerListValue;
            internal float[] FloatListValue;
            internal string[] StringListValue;
            internal Vector3[] Vector3ListValue;
        }

        internal sealed class GraphVariableBatchResult
        {
            internal int Created;
            internal int Skipped;
            internal int Failed;
            internal string Summary = "";
        }

        internal static Graph ResolveTargetGraph(Node node, string targetGraph)
        {
            if (node == null)
                return null;

            if (string.IsNullOrWhiteSpace(targetGraph))
                return node.Graph;

            var scene = Context.OpenedScene;
            if (scene == null)
                return node.Graph;

            if (Guid.TryParse(targetGraph, out var graphId))
            {
                var byId = scene.GetGraph(graphId);
                if (byId != null)
                    return byId;
            }

            foreach (var graph in scene.GetGraphList())
            {
                if (graph != null && graph.Id.ToString() == targetGraph)
                    return graph;
                if (graph != null && graph.Name == targetGraph)
                    return graph;
            }

            return node.Graph;
        }

        internal static GraphVariableBatchResult CreateVariablesBatch(
            Node node,
            string targetGraph,
            IReadOnlyList<GraphVariableCreateSpec> specs,
            bool skipIfExists,
            bool applyInitialValues
        )
        {
            var result = new GraphVariableBatchResult();
            if (specs == null || specs.Count == 0)
            {
                result.Summary = "(항목 없음)";
                return result;
            }

            var graph = ResolveTargetGraph(node, targetGraph);
            var properties = graph?.Properties;
            if (properties == null)
            {
                result.Failed = specs.Count;
                result.Summary = "(Graph Properties 없음)";
                return result;
            }

            var variablesPort = properties.GetDataInputPort(nameof(GraphProperties.Variables));
            if (variablesPort == null)
            {
                result.Failed = specs.Count;
                result.Summary = "(Variables 포트 없음)";
                return result;
            }

            var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var touched = false;

            for (var i = 0; i < specs.Count; i++)
            {
                var spec = specs[i];
                if (spec == null || string.IsNullOrWhiteSpace(spec.Name))
                {
                    result.Failed++;
                    continue;
                }

                var trimmed = spec.Name.Trim();
                if (!seenNames.Add(trimmed))
                {
                    Debug.LogWarning(
                        $"[Node68 DevKit/GraphVar] 배치 목록에 중복 이름 '{trimmed}' — 건너뜀."
                    );
                    result.Skipped++;
                    continue;
                }

                properties.RebuildVariableMap();
                var existing = properties.GetVariable(trimmed);
                if (existing != null)
                {
                    if (existing.VariableType != spec.VariableType)
                    {
                        Debug.LogWarning(
                            $"[Node68 DevKit/GraphVar] '{trimmed}' 이미 존재하지만 타입이 다릅니다 "
                                + $"(기존 {existing.VariableType}, 요청 {spec.VariableType})."
                        );
                        result.Failed++;
                        continue;
                    }

                    if (skipIfExists)
                    {
                        result.Skipped++;
                        continue;
                    }

                    if (applyInitialValues)
                    {
                        ApplyInitialValue(existing, spec);
                        touched = true;
                    }

                    result.Skipped++;
                    continue;
                }

                var variable = (GraphVariable)variablesPort.AppendArray(properties.Scene, properties);
                variable.SetDataInput(nameof(GraphVariable.Name), trimmed, broadcast: true);
                variable.SetDataInput(
                    nameof(GraphVariable.VariableType),
                    spec.VariableType,
                    broadcast: true
                );

                if (applyInitialValues)
                    ApplyInitialValue(variable, spec);

                result.Created++;
                touched = true;
            }

            if (touched)
            {
                properties.RebuildVariableMap();
                properties.BroadcastDataInput(nameof(GraphProperties.Variables));
            }

            result.Summary =
                $"생성 {result.Created}, 건너뜀 {result.Skipped}, 실패 {result.Failed}";
            return result;
        }

        internal static void ApplyInitialValue(GraphVariable variable, GraphVariableCreateSpec spec)
        {
            if (variable == null || spec == null)
                return;

            switch (spec.VariableType)
            {
                case GraphVariableType.Boolean:
                    variable.SetDataInput(
                        nameof(GraphVariable.BooleanValue),
                        spec.BooleanValue,
                        broadcast: true
                    );
                    break;
                case GraphVariableType.Integer:
                    variable.SetDataInput(
                        nameof(GraphVariable.IntegerValue),
                        spec.IntegerValue,
                        broadcast: true
                    );
                    break;
                case GraphVariableType.Float:
                    variable.SetDataInput(
                        nameof(GraphVariable.FloatValue),
                        spec.FloatValue,
                        broadcast: true
                    );
                    break;
                case GraphVariableType.String:
                    variable.SetDataInput(
                        nameof(GraphVariable.StringValue),
                        spec.StringValue ?? string.Empty,
                        broadcast: true
                    );
                    break;
                case GraphVariableType.Vector3:
                    variable.SetDataInput(
                        nameof(GraphVariable.Vector3Value),
                        spec.Vector3Value,
                        broadcast: true
                    );
                    break;
                case GraphVariableType.BooleanList:
                    variable.SetDataInput(
                        nameof(GraphVariable.BooleanListValue),
                        spec.BooleanListValue ?? Array.Empty<bool>(),
                        broadcast: true
                    );
                    break;
                case GraphVariableType.IntegerList:
                    variable.SetDataInput(
                        nameof(GraphVariable.IntegerListValue),
                        spec.IntegerListValue ?? Array.Empty<int>(),
                        broadcast: true
                    );
                    break;
                case GraphVariableType.FloatList:
                    variable.SetDataInput(
                        nameof(GraphVariable.FloatListValue),
                        spec.FloatListValue ?? Array.Empty<float>(),
                        broadcast: true
                    );
                    break;
                case GraphVariableType.StringList:
                    variable.SetDataInput(
                        nameof(GraphVariable.StringListValue),
                        spec.StringListValue ?? Array.Empty<string>(),
                        broadcast: true
                    );
                    break;
                case GraphVariableType.Vector3List:
                    variable.SetDataInput(
                        nameof(GraphVariable.Vector3ListValue),
                        spec.Vector3ListValue ?? Array.Empty<Vector3>(),
                        broadcast: true
                    );
                    break;
            }
        }

        internal static async UniTask<AutoCompleteList> AutoCompleteTargetGraph(Node node)
        {
            await UniTask.Yield();
            var scene = Context.OpenedScene;
            if (scene == null)
                return new AutoCompleteList();

            var entries = scene
                .GetGraphList()
                .Where(g => g != null)
                .Select(g => new AutoCompleteEntry
                {
                    label = g.Name,
                    value = g.Id.ToString(),
                })
                .ToList();

            return AutoCompleteList.Single(entries);
        }
    }
}
