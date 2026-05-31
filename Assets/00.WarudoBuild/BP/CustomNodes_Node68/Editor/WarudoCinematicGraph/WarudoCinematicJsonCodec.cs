using System;
using System.Globalization;
using DG.Tweening;
using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// Warudo Serialized Graph JSON의 이중 이스케이프 규칙을 그대로 맞춥니다 (샘플 JSON 기준).
    /// </summary>
    internal static class WarudoCinematicJsonCodec
    {
        public static string EscapeJsonStringContent(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s ?? string.Empty;
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public static string F(float v) => v.ToString("G9", CultureInfo.InvariantCulture);

        public static string BuildVector3Payload(Vector3 v) =>
            $"{{\"x\":{F(v.x)},\"y\":{F(v.y)},\"z\":{F(v.z)}}}";

        private static string BuildVector3DataInput(Vector3 v)
        {
            var inner = BuildVector3Payload(v);
            return $"{{\"type\":\"UnityEngine.Vector3\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildTransformDataInput(string transformId, Vector3 pos, Vector3 rot, Vector3 scale)
        {
            var di =
                "{\"Position\":"
                + BuildVector3DataInput(pos)
                + ",\"Rotation\":"
                + BuildVector3DataInput(rot)
                + ",\"Scale\":"
                + BuildVector3DataInput(scale)
                + "}";

            var inner = $"{{\"id\":\"{transformId}\",\"dataInputs\":{di}}}";
            return $"{{\"type\":\"Warudo.Core.Data.Models.TransformData\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildGameObjectAssetInput(string assetId, string assetName)
        {
            var inner =
                $"{{\"id\":\"{assetId}\",\"name\":\"{EscapeJsonStringContent(assetName)}\"}}";
            return $"{{\"type\":\"Warudo.Plugins.Core.Assets.GameObjectAsset\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildCameraAssetInput(string assetId, string assetName)
        {
            var inner =
                $"{{\"id\":\"{assetId}\",\"name\":\"{EscapeJsonStringContent(assetName)}\"}}";
            return $"{{\"type\":\"Warudo.Plugins.Core.Assets.Cinematography.CameraAsset\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildFloatDataInput(float value) =>
            $"{{\"type\":\"float\",\"value\":\"{F(value)}\"}}";

        public static string BuildBoolDataInput(bool value) =>
            $"{{\"type\":\"bool\",\"value\":\"{(value ? "true" : "false")}\"}}";

        /// <summary>FIND_ASSET_BY_NAME 등에서 사용되는 string dataInput (value가 \"텍스트\" 형태).</summary>
        public static string BuildWarudoQuotedStringDataInput(string text)
        {
            var escaped = EscapeJsonStringContent(text ?? string.Empty);
            var inner = "\\\"" + escaped + "\\\"";
            return $"{{\"type\":\"string\",\"value\":\"{inner}\"}}";
        }

        public static string BuildEaseDataInput(Ease ease)
        {
            var label = ease.ToString();
            var value = (int)ease;
            var inner =
                $"{{\"label\":\"{EscapeJsonStringContent(label)}\",\"value\":{value.ToString(CultureInfo.InvariantCulture)},\"description\":null,\"icon\":null}}";
            return $"{{\"type\":\"DG.Tweening.Ease\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildKeystrokeDataInput(string label, int keystrokeEnumValue)
        {
            var inner =
                $"{{\"label\":\"{EscapeJsonStringContent(label)}\",\"value\":{keystrokeEnumValue.ToString(CultureInfo.InvariantCulture)},\"description\":null,\"icon\":null}}";
            return $"{{\"type\":\"Warudo.Plugins.Core.Events.Keystroke\",\"value\":\"{EscapeJsonStringContent(inner)}\"}}";
        }

        public static string BuildGraphVariablesPropertyInput() =>
            "{\"type\":\"Warudo.Core.Graphs.GraphVariable[]\",\"value\":\"[]\"}";
    }
}
