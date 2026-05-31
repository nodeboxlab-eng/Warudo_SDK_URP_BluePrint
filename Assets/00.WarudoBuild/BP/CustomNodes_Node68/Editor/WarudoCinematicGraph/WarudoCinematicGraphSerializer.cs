using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DG.Tweening;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// FIND_ASSET · TOGGLE_CAMERA · SET_ASSET_TRANSFORM 체인을 Warudo Graph JSON으로 직렬화합니다.
    /// </summary>
    public sealed class WarudoCinematicGraphExportRequest
    {
        public string GraphName = "Generated Cinematic Graph";

        public string CameraAssetName = "Camera 1";

        public string CameraAssetId = "";

        public bool FindExactMatch;

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Keyframes = Array.Empty<WarudoCinematicTransformKeyframe>();

        public bool IncludeKeystrokeTrigger = true;

        public string KeystrokeLabel = "1";

        public int KeystrokeEnumValue = 2;

        public bool KeystrokeRequireCtrl = true;

        public bool KeystrokeRequireShift;

        public bool KeystrokeRequireAlt;

        public bool KeystrokeRequireMeta;

        public float PanningX;

        public float PanningY;

        public float Scaling = 1f;
    }

    public static class WarudoCinematicGraphSerializer
    {
        public static string Serialize(WarudoCinematicGraphExportRequest req)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));
            if (string.IsNullOrWhiteSpace(req.GraphName))
                throw new InvalidOperationException("GraphName이 비어 있습니다.");

            var keyframes = req.Keyframes ?? Array.Empty<WarudoCinematicTransformKeyframe>();
            if (keyframes.Count == 0)
                throw new InvalidOperationException("Keyframes가 비어 있습니다.");

            var graphId = WarudoCinematicDeterministicGuid.From("graph", req.GraphName, req.CameraAssetName);
            var propsId = WarudoCinematicDeterministicGuid.From("props", req.GraphName);

            var findId = WarudoCinematicDeterministicGuid.From(req.GraphName, "FIND_ASSET_BY_NAME");
            var toggleId = WarudoCinematicDeterministicGuid.From(req.GraphName, "TOGGLE_CAMERA");
            var keystrokeId = WarudoCinematicDeterministicGuid.From(req.GraphName, "ON_KEYSTROKE_PRESSED");

            var cameraUid = string.IsNullOrWhiteSpace(req.CameraAssetId)
                ? WarudoCinematicDeterministicGuid.From("camera-asset", req.CameraAssetName).ToString()
                : req.CameraAssetId.Trim();

            var goAsset = WarudoCinematicJsonCodec.BuildGameObjectAssetInput(cameraUid, req.CameraAssetName);
            var camAsset = WarudoCinematicJsonCodec.BuildCameraAssetInput(cameraUid, req.CameraAssetName);

            var nodes = new SortedDictionary<string, string>(StringComparer.Ordinal);

            nodes[findId.ToString()] = BuildFindAssetByNameNode(
                findId.ToString(),
                req.CameraAssetName,
                req.FindExactMatch
            );

            nodes[toggleId.ToString()] = BuildToggleCameraNode(toggleId.ToString(), camAsset);

            if (req.IncludeKeystrokeTrigger)
                nodes[keystrokeId.ToString()] = BuildKeystrokeNode(
                    keystrokeId.ToString(),
                    req.KeystrokeLabel,
                    req.KeystrokeEnumValue,
                    req.KeystrokeRequireCtrl,
                    req.KeystrokeRequireShift,
                    req.KeystrokeRequireAlt,
                    req.KeystrokeRequireMeta
                );

            var setIds = new List<string>(keyframes.Count);
            for (var i = 0; i < keyframes.Count; i++)
            {
                var sid = WarudoCinematicDeterministicGuid.From(req.GraphName, "SET_ASSET_TRANSFORM", i.ToString(CultureInfo.InvariantCulture));
                setIds.Add(sid.ToString());
            }

            for (var i = 0; i < keyframes.Count; i++)
            {
                var k = keyframes[i];
                var transformId = WarudoCinematicDeterministicGuid
                    .From(req.GraphName, "transform", i.ToString(CultureInfo.InvariantCulture))
                    .ToString();

                var transformInput = WarudoCinematicJsonCodec.BuildTransformDataInput(
                    transformId,
                    k.Position,
                    k.Rotation,
                    k.Scale
                );

                nodes[setIds[i]] = BuildSetAssetTransformNode(
                    setIds[i],
                    goAsset,
                    transformInput,
                    k.TransitionTime,
                    k.Easing,
                    1120f + i * 380f,
                    40f
                );
            }

            var flow = new List<FlowEdge>();
            if (req.IncludeKeystrokeTrigger)
                flow.Add(new FlowEdge(keystrokeId.ToString(), toggleId.ToString(), WarudoCinematicGraphPorts.FlowExit, WarudoCinematicGraphPorts.FlowEnter));

            flow.Add(new FlowEdge(toggleId.ToString(), setIds[0], WarudoCinematicGraphPorts.FlowExit, WarudoCinematicGraphPorts.FlowEnter));

            for (var i = 0; i < setIds.Count - 1; i++)
                flow.Add(
                    new FlowEdge(
                        setIds[i],
                        setIds[i + 1],
                        WarudoCinematicGraphPorts.OnTransitionEnd,
                        WarudoCinematicGraphPorts.FlowEnter
                    )
                );

            var data = new List<DataEdge>();
            foreach (var sid in setIds)
                data.Add(new DataEdge(findId.ToString(), sid, WarudoCinematicGraphPorts.Asset, WarudoCinematicGraphPorts.Asset));

            data.Add(new DataEdge(findId.ToString(), toggleId.ToString(), WarudoCinematicGraphPorts.Asset, WarudoCinematicGraphPorts.Camera));

            flow.Sort((a, b) => string.CompareOrdinal($"{a.Out}:{a.In}:{a.OutP}:{a.InP}", $"{b.Out}:{b.In}:{b.OutP}:{b.InP}"));

            data.Sort((a, b) => string.CompareOrdinal($"{a.Out}:{a.In}:{a.OutP}:{a.InP}", $"{b.Out}:{b.In}:{b.OutP}:{b.InP}"));

            var sb = new StringBuilder(8192);
            sb.Append("{\"id\":\"").Append(graphId).Append('\"');
            sb.Append(",\"enabled\":true,\"name\":\"").Append(WarudoCinematicJsonCodec.EscapeJsonStringContent(req.GraphName)).Append('\"');
            sb.Append(",\"order\":0,\"group\":null");
            sb.Append(",\"panningX\":").Append(WarudoCinematicJsonCodec.F(req.PanningX));
            sb.Append(",\"panningY\":").Append(WarudoCinematicJsonCodec.F(req.PanningY));
            sb.Append(",\"scaling\":").Append(WarudoCinematicJsonCodec.F(req.Scaling));
            sb.Append(",\"nodes\":{");

            var first = true;
            foreach (var kv in nodes)
            {
                if (!first)
                    sb.Append(',');
                first = false;
                sb.Append('"').Append(kv.Key).Append("\":").Append(kv.Value);
            }

            sb.Append("},\"dataConnections\":[");

            for (var i = 0; i < data.Count; i++)
            {
                if (i > 0)
                    sb.Append(',');
                var e = data[i];
                sb.Append("{\"outputNode\":\"").Append(e.Out).Append("\",\"inputNode\":\"").Append(e.In);
                sb.Append("\",\"outputPort\":\"").Append(e.OutP).Append("\",\"inputPort\":\"").Append(e.InP).Append("\"}");
            }

            sb.Append("],\"flowConnections\":[");

            for (var i = 0; i < flow.Count; i++)
            {
                if (i > 0)
                    sb.Append(',');
                var e = flow[i];
                sb.Append("{\"outputNode\":\"").Append(e.Out).Append("\",\"inputNode\":\"").Append(e.In);
                sb.Append("\",\"outputPort\":\"").Append(e.OutP).Append("\",\"inputPort\":\"").Append(e.InP).Append("\"}");
            }

            sb.Append("],\"properties\":{\"id\":\"").Append(propsId).Append("\",\"dataInputs\":{");
            sb.Append("\"Variables\":").Append(WarudoCinematicJsonCodec.BuildGraphVariablesPropertyInput());
            sb.Append("}}}");

            return sb.ToString();
        }

        private readonly struct FlowEdge
        {
            public readonly string Out;
            public readonly string In;
            public readonly string OutP;
            public readonly string InP;

            public FlowEdge(string o, string i, string op, string ip)
            {
                Out = o;
                In = i;
                OutP = op;
                InP = ip;
            }
        }

        private readonly struct DataEdge
        {
            public readonly string Out;
            public readonly string In;
            public readonly string OutP;
            public readonly string InP;

            public DataEdge(string o, string i, string op, string ip)
            {
                Out = o;
                In = i;
                OutP = op;
                InP = ip;
            }
        }

        private static string BuildFindAssetByNameNode(string id, string assetName, bool exact)
        {
            var sb = new StringBuilder(512);
            sb.Append("{\"id\":\"").Append(id).Append("\",\"dataInputs\":{");
            sb.Append("\"AssetName\":").Append(WarudoCinematicJsonCodec.BuildWarudoQuotedStringDataInput(assetName)).Append(',');
            sb.Append("\"ExactMatch\":").Append(WarudoCinematicJsonCodec.BuildBoolDataInput(exact));
            sb.Append("},\"typeId\":\"").Append(WarudoCinematicGraphTypeIds.FindAssetByName);
            sb.Append("\",\"name\":\"FIND_ASSET_BY_NAME\",\"x\":512.0,\"y\":420.0}");
            return sb.ToString();
        }

        private static string BuildToggleCameraNode(string id, string cameraInputJson)
        {
            var sb = new StringBuilder(768);
            sb.Append("{\"id\":\"").Append(id).Append("\",\"dataInputs\":{");
            sb.Append("\"Camera\":").Append(cameraInputJson).Append(',');
            sb.Append("\"TransitionTime\":").Append(WarudoCinematicJsonCodec.BuildFloatDataInput(0f)).Append(',');
            sb.Append("\"TransitionEasing\":").Append(WarudoCinematicJsonCodec.BuildEaseDataInput(Ease.Linear));
            sb.Append("},\"typeId\":\"").Append(WarudoCinematicGraphTypeIds.ToggleCamera);
            sb.Append("\",\"name\":\"TOGGLE_CAMERA\",\"x\":912.0,\"y\":40.0}");
            return sb.ToString();
        }

        private static string BuildKeystrokeNode(
            string id,
            string label,
            int keystrokeEnumValue,
            bool ctrl,
            bool shift,
            bool alt,
            bool meta
        )
        {
            var sb = new StringBuilder(768);
            sb.Append("{\"id\":\"").Append(id).Append("\",\"dataInputs\":{");
            sb.Append("\"Keystroke\":").Append(WarudoCinematicJsonCodec.BuildKeystrokeDataInput(label, keystrokeEnumValue)).Append(',');
            sb.Append("\"RequireCtrl\":").Append(WarudoCinematicJsonCodec.BuildBoolDataInput(ctrl)).Append(',');
            sb.Append("\"RequireShift\":").Append(WarudoCinematicJsonCodec.BuildBoolDataInput(shift)).Append(',');
            sb.Append("\"RequireAlt\":").Append(WarudoCinematicJsonCodec.BuildBoolDataInput(alt)).Append(',');
            sb.Append("\"RequireMeta\":").Append(WarudoCinematicJsonCodec.BuildBoolDataInput(meta));
            sb.Append("},\"typeId\":\"").Append(WarudoCinematicGraphTypeIds.OnKeystrokePressed);
            sb.Append("\",\"name\":\"ON_KEYSTROKE_PRESSED\",\"x\":520.0,\"y\":-140.0}");
            return sb.ToString();
        }

        private static string BuildSetAssetTransformNode(
            string id,
            string gameObjectAssetInput,
            string transformInput,
            float transitionTime,
            Ease easing,
            float x,
            float y
        )
        {
            var sb = new StringBuilder(2048);
            sb.Append("{\"id\":\"").Append(id).Append("\",\"dataInputs\":{");
            sb.Append("\"Asset\":").Append(gameObjectAssetInput).Append(',');
            sb.Append("\"Transform\":").Append(transformInput).Append(',');
            sb.Append("\"TransitionTime\":").Append(WarudoCinematicJsonCodec.BuildFloatDataInput(transitionTime)).Append(',');
            sb.Append("\"TransitionEasing\":").Append(WarudoCinematicJsonCodec.BuildEaseDataInput(easing));
            sb.Append("},\"typeId\":\"").Append(WarudoCinematicGraphTypeIds.SetAssetTransform);
            sb.Append("\",\"name\":\"SET_ASSET_TRANSFORM\",\"x\":").Append(WarudoCinematicJsonCodec.F(x));
            sb.Append(",\"y\":").Append(WarudoCinematicJsonCodec.F(y)).Append('}');
            return sb.ToString();
        }
    }
}
