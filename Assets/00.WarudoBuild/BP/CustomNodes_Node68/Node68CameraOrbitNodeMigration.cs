using System.Globalization;
using Newtonsoft.Json;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    internal static class Node68CameraOrbitNodeMigration
    {
        internal static void MigrateSaveFields(SaveMainCameraOrbitNode68 node, SerializedNode serialized)
        {
            if (node == null)
                return;

            node.PreOrbitX = CoalesceVariableName(
                node.PreOrbitX,
                ReadSerializedString(serialized, "PreOrbitX"),
                ReadSerializedString(serialized, "OrbitXVariable"),
                ReadSerializedString(serialized, "OrbitX"),
                "PreOrbitX"
            );
            node.PreOrbitY = CoalesceVariableName(
                node.PreOrbitY,
                ReadSerializedString(serialized, "PreOrbitY"),
                ReadSerializedString(serialized, "OrbitYVariable"),
                ReadSerializedString(serialized, "OrbitY"),
                "PreOrbitY"
            );
            node.PreOrbitOffset = CoalesceVariableName(
                node.PreOrbitOffset,
                ReadSerializedString(serialized, "PreOrbitOffset"),
                ReadSerializedString(serialized, "OrbitOffsetVariable"),
                ReadSerializedString(serialized, "OrbitOffset"),
                "PreOrbitOffset"
            );
            node.PreFov = CoalesceVariableName(
                node.PreFov,
                ReadSerializedString(serialized, "PreFov"),
                ReadSerializedString(serialized, "FovVariable"),
                ReadSerializedString(serialized, "PreFOV"),
                "PreFOV"
            );
            node.PreControlMode = CoalesceVariableName(
                node.PreControlMode,
                ReadSerializedString(serialized, "PreControlMode"),
                ReadSerializedString(serialized, "ControlModeVariable"),
                null,
                "PreControlMode"
            );
            node.PreLookAtTarget = CoalesceVariableName(
                node.PreLookAtTarget,
                ReadSerializedString(serialized, "PreLookAtTarget"),
                ReadSerializedString(serialized, "LookAtTargetVariable"),
                null,
                "PreLookAtTarget"
            );
            node.PreTargetCamera = CoalesceVariableName(
                node.PreTargetCamera,
                ReadSerializedString(serialized, "PreTargetCamera"),
                ReadSerializedString(serialized, "PreLookAtCamera"),
                ReadSerializedString(serialized, "LookAtCameraVariable"),
                "PreTargetCamera"
            );
        }

        private static string CoalesceVariableName(
            string current,
            params string[] candidates
        )
        {
            if (IsValidVariableName(current))
                return current.Trim();

            if (candidates == null || candidates.Length == 0)
                return current;

            for (var i = 0; i < candidates.Length - 1; i++)
            {
                if (IsValidVariableName(candidates[i]))
                    return candidates[i].Trim();
            }

            var fallback = candidates[^1];
            return string.IsNullOrWhiteSpace(fallback) ? current : fallback;
        }

        private static bool IsValidVariableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var trimmed = name.Trim();
            if (trimmed.StartsWith('"') && trimmed.EndsWith('"'))
                trimmed = trimmed.Trim('"');

            if (string.IsNullOrWhiteSpace(trimmed))
                return false;

            if (float.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                return false;

            if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                return false;

            return true;
        }

        private static string ReadSerializedString(SerializedNode serialized, string key)
        {
            if (serialized?.dataInputs == null || !serialized.dataInputs.TryGetValue(key, out var port))
                return null;

            var raw = port.value;
            if (string.IsNullOrWhiteSpace(raw) || raw == "null")
                return null;

            if (port.type != null && port.type.Contains("Vector3"))
                return null;

            if (port.type == "float" || port.type == "int" || port.type == "bool")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<string>(raw);
            }
            catch
            {
                return raw.Trim().Trim('"');
            }
        }
    }
}
