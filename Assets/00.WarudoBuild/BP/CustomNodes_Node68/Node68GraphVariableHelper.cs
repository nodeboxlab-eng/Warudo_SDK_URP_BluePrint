using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Data;
using Warudo.Core.Graphs;

namespace Node68.CustomNodes
{
    internal static class Node68GraphVariableHelper
    {
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

        internal static GraphVariable ResolveVariable(
            Node node,
            string targetGraph,
            string variableName,
            GraphVariableType expectedType,
            bool createIfMissing
        )
        {
            if (string.IsNullOrWhiteSpace(variableName))
                return null;

            var graph = ResolveTargetGraph(node, targetGraph);
            var properties = graph?.Properties;
            if (properties == null)
                return null;

            var trimmed = variableName.Trim();
            properties.RebuildVariableMap();
            var existing = properties.GetVariable(trimmed);
            if (existing != null)
            {
                if (existing.VariableType != expectedType)
                {
                    Debug.LogWarning(
                        $"[Node68/Camera] Graph 변수 '{trimmed}' 타입이 {expectedType} 이 아닙니다 ({existing.VariableType})."
                    );
                    return null;
                }

                return existing;
            }

            if (!createIfMissing)
                return null;

            var variablesPort = properties.GetDataInputPort(nameof(GraphProperties.Variables));
            if (variablesPort == null)
                return null;

            var variable = (GraphVariable)variablesPort.AppendArray(properties.Scene, properties);
            variable.SetDataInput(nameof(GraphVariable.Name), trimmed, broadcast: true);
            variable.SetDataInput(nameof(GraphVariable.VariableType), expectedType, broadcast: true);
            properties.RebuildVariableMap();
            properties.BroadcastDataInput(nameof(GraphProperties.Variables));

            Debug.Log(
                $"[Node68/Camera] Graph 변수 '{trimmed}' ({expectedType}) 를 자동 생성했습니다."
            );
            return variable;
        }

        internal static GraphVariable TryGetVariable(
            Node node,
            string targetGraph,
            string variableName,
            GraphVariableType expectedType
        )
        {
            return ResolveVariable(node, targetGraph, variableName, expectedType, false);
        }

        internal static bool TryGetFloat(
            Node node,
            string targetGraph,
            string variableName,
            out float value,
            bool createIfMissing = false
        )
        {
            value = 0f;
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Float,
                createIfMissing
            );
            if (variable == null)
                return false;

            value = variable.FloatValue;
            return true;
        }

        internal static bool TryGetVector3(
            Node node,
            string targetGraph,
            string variableName,
            out Vector3 value,
            bool createIfMissing = false
        )
        {
            value = Vector3.zero;
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Vector3,
                createIfMissing
            );
            if (variable == null)
                return false;

            value = variable.Vector3Value;
            return true;
        }

        internal static bool TryGetBoolean(
            Node node,
            string targetGraph,
            string variableName,
            out bool value,
            bool createIfMissing = false
        )
        {
            value = false;
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Boolean,
                createIfMissing
            );
            if (variable == null)
                return false;

            value = variable.BooleanValue;
            return true;
        }

        internal static bool TrySetFloat(
            Node node,
            string targetGraph,
            string variableName,
            float value,
            bool createIfMissing = false
        )
        {
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Float,
                createIfMissing
            );
            if (variable == null)
                return false;

            variable.SetDataInput(nameof(GraphVariable.FloatValue), value, broadcast: true);
            return true;
        }

        internal static bool TrySetVector3(
            Node node,
            string targetGraph,
            string variableName,
            Vector3 value,
            bool createIfMissing = false
        )
        {
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Vector3,
                createIfMissing
            );
            if (variable == null)
                return false;

            variable.SetDataInput(nameof(GraphVariable.Vector3Value), value, broadcast: true);
            return true;
        }

        internal static bool TryGetString(
            Node node,
            string targetGraph,
            string variableName,
            out string value,
            bool createIfMissing = false
        )
        {
            value = null;
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.String,
                createIfMissing
            );
            if (variable == null)
                return false;

            value = variable.StringValue;
            return true;
        }

        internal static bool TrySetString(
            Node node,
            string targetGraph,
            string variableName,
            string value,
            bool createIfMissing = false
        )
        {
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.String,
                createIfMissing
            );
            if (variable == null)
                return false;

            variable.SetDataInput(nameof(GraphVariable.StringValue), value ?? string.Empty, broadcast: true);
            return true;
        }

        internal static bool TrySetBoolean(
            Node node,
            string targetGraph,
            string variableName,
            bool value,
            bool createIfMissing = false
        )
        {
            var variable = ResolveVariable(
                node,
                targetGraph,
                variableName,
                GraphVariableType.Boolean,
                createIfMissing
            );
            if (variable == null)
                return false;

            variable.SetDataInput(nameof(GraphVariable.BooleanValue), value, broadcast: true);
            return true;
        }

        internal static bool WarnMissingVariable(string nodeTitle, string variableName)
        {
            if (!string.IsNullOrWhiteSpace(variableName))
                return false;

            Debug.LogWarning($"[Node68/Camera] {nodeTitle}: 변수 이름이 비어 있습니다.");
            return true;
        }

        internal static void WarnVariableNotFound(string nodeTitle, string variableName)
        {
            Debug.LogWarning(
                $"[Node68/Camera] {nodeTitle}: Graph 변수 '{variableName}' 를 찾을 수 없습니다."
            );
        }

        internal static async UniTask<AutoCompleteList> AutoCompleteVariableName(
            Node node,
            string targetGraph,
            GraphVariableType type
        )
        {
            await UniTask.Yield();
            var graph = ResolveTargetGraph(node, targetGraph);
            var variables = graph?.Properties?.Variables;
            if (variables == null)
                return new AutoCompleteList();

            var entries = variables
                .Where(v => v != null && v.VariableType == type && !string.IsNullOrWhiteSpace(v.Name))
                .Select(v => new AutoCompleteEntry { label = v.Name, value = v.Name })
                .ToList();

            return AutoCompleteList.Single(entries);
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
