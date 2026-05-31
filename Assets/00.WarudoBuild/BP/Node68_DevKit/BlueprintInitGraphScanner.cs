using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.ToolkitMods.Node68DevKit
{
    internal static class BlueprintInitGraphScanner
    {
        internal sealed class ScanSuggestion
        {
            public string SourceNodeName;
            public BlueprintInitRuleKind68 SuggestedKind;
            public string Summary;
            public string WiringHint;
        }

        internal static List<ScanSuggestion> Scan(Graph graph)
        {
            var suggestions = new List<ScanSuggestion>();
            if (graph == null)
                return suggestions;

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var (_, node) in graph.GetNodes())
            {
                SerializedNode serialized;
                try
                {
                    serialized = node.Serialize();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[Init Profile] 노드 직렬화 실패 '{node.Name}': {ex.Message}"
                    );
                    continue;
                }

                var nodeName = serialized.name ?? node.Name ?? "(unknown)";
                var dataInputs = serialized.dataInputs;
                if (dataInputs == null)
                    continue;

                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSetPropBlendShape(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchToggleAssetEnabled(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSetAssetPosition(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchTextDisplayShowHide(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSetDisplayText(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSaveMainCameraOrbit(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSetCameraControlMode(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchCameraOrbitCharacter(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchSetAssetPropertyLookAtOrFov(nodeName, dataInputs)
                );
                TryAddUnique(
                    seen,
                    suggestions,
                    MatchGameObjectTransformEasing(nodeName, dataInputs)
                );
            }

            return suggestions;
        }

        private static void TryAddUnique(
            HashSet<string> seen,
            List<ScanSuggestion> list,
            ScanSuggestion suggestion
        )
        {
            if (suggestion == null)
                return;

            var key = suggestion.SuggestedKind + "|" + suggestion.Summary;
            if (!seen.Add(key))
                return;

            list.Add(suggestion);
        }

        private static ScanSuggestion MatchSetPropBlendShape(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "SET_PROP_BLENDSHAPE"))
                return null;

            var propName = ReadAssetName(dataInputs, "Prop");
            var mesh = ReadStringValue(dataInputs, "TargetSkinnedMesh");
            var shape = ReadStringValue(dataInputs, "BlendShape");
            var value = ReadFloatValue(dataInputs, "Value");

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.PropBlendShape,
                Summary =
                    $"Prop `{propName}` · 메시 `{mesh}` · `{shape}` → **0** (현재 노드 Value={value:0.###})",
                WiringHint =
                    "ON_ENABLE_GRAPH → SET_PROP_BLENDSHAPE (Value=0, TransitionTime=0) 또는 Init Profile 규칙에 등록",
            };
        }

        private static ScanSuggestion MatchToggleAssetEnabled(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "TOGGLE_ASSET_ENABLED"))
                return null;

            var assetName = ReadAssetName(dataInputs, "Asset");
            var action = ReadStringValue(dataInputs, "Action");
            var disable =
                action != null
                && (
                    action.IndexOf("비활성", StringComparison.OrdinalIgnoreCase) >= 0
                    || action.IndexOf("Disable", StringComparison.OrdinalIgnoreCase) >= 0
                    || action.Contains("\"value\":2")
                );

            if (!disable)
                return null;

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.GameObjectEnabled,
                Summary = $"GameObject `{assetName}` → **비활성 (Enabled=false)**",
                WiringHint =
                    "ON_ENABLE_GRAPH → TOGGLE_ASSET_ENABLED (비활성) — 종료 시퀀스와 동일하게 유지",
            };
        }

        private static ScanSuggestion MatchSetAssetPosition(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "SET_ASSET_POSITION"))
                return null;

            var assetName = ReadAssetName(dataInputs, "Asset");
            var pos = ReadVector3Value(dataInputs, "Position");

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.GameObjectTransform,
                Summary =
                    $"GameObject `{assetName}` → 위치 **({pos.x:0.###}, {pos.y:0.###}, {pos.z:0.###})**",
                WiringHint =
                    "ON_ENABLE_GRAPH → SET_ASSET_POSITION (TransitionTime=0 권장) — **비활성화 전**에 적용할지 순서 확인",
            };
        }

        private static ScanSuggestion MatchTextDisplayShowHide(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (
                !NodeNameLooksLike(nodeName, "Text Display Show/Hide")
                && !NodeNameLooksLike(nodeName, "TEXT_DISPLAY_SHOW_HIDE")
            )
                return null;

            var displayName = ReadAssetName(dataInputs, "Display");
            var intent = ReadStringValue(dataInputs, "Intent");
            if (
                intent != null
                && intent.IndexOf("보이기", StringComparison.OrdinalIgnoreCase) >= 0
            )
                return null;

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.TextDisplayReset,
                Summary = $"TextDisplay `{displayName}` → **숨기기** (UseFade=false 권장)",
                WiringHint =
                    "ON_ENABLE_GRAPH → Text Display Show/Hide (숨기기) + Set Display Text (\"\")",
            };
        }

        private static ScanSuggestion MatchSetDisplayText(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (
                !NodeNameLooksLike(nodeName, "Set Display Text")
                && !NodeNameLooksLike(nodeName, "SET_DISPLAY_TEXT")
            )
                return null;

            var displayName = ReadAssetName(dataInputs, "Display");
            var text = ReadStringValue(dataInputs, "NewText");
            if (!string.IsNullOrEmpty(text) && text != "\"\"")
                return null;

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.TextDisplayReset,
                Summary = $"TextDisplay `{displayName}` → **텍스트 비우기 (\"\")**",
                WiringHint = "ON_ENABLE_GRAPH → Set Display Text Node68 (NewText=\"\")",
            };
        }

        private static ScanSuggestion MatchSaveMainCameraOrbit(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "Save Main Camera Orbit"))
                return null;

            var preX = ReadStringValue(dataInputs, "PreOrbitX") ?? "PreOrbitX";
            var preY = ReadStringValue(dataInputs, "PreOrbitY") ?? "PreOrbitY";
            var preOff = ReadStringValue(dataInputs, "PreOrbitOffset") ?? "PreOrbitOffset";
            var preFov = ReadStringValue(dataInputs, "PreFOV") ?? "PreFOV";

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.CameraRestoreFromVariables,
                Summary =
                    $"카메라 저장 노드 감지 — Graph Variable `{StripQuotes(preX)}`, `{StripQuotes(preY)}`, `{StripQuotes(preOff)}`, `{StripQuotes(preFov)}` 로 **복구 필요**",
                WiringHint =
                    "ON_ENABLE_GRAPH → GET_*_VARIABLE + CAMERA_ORBIT_CHARACTER + SET_CAMERA_CONTROL_MODE + LookAt/FOV 복구 (TransitionTime=0)",
            };
        }

        private static ScanSuggestion MatchSetCameraControlMode(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "SET_CAMERA_CONTROL_MODE"))
                return null;

            var mode = ReadStringValue(dataInputs, "ControlMode");
            if (mode != null && mode.IndexOf("없음", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new ScanSuggestion
                {
                    SourceNodeName = nodeName,
                    SuggestedKind = BlueprintInitRuleKind68.CameraRestoreFromVariables,
                    Summary = "연출 중 카메라 Control Mode → **없음** 으로 변경됨 — 초기화 시 **PreControlMode** 로 복구",
                    WiringHint =
                        "종료 시퀀스의 SET_CAMERA_CONTROL_MODE (오르빗) 를 ON_ENABLE_GRAPH 에도 연결",
                };
            }

            return null;
        }

        private static ScanSuggestion MatchCameraOrbitCharacter(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "CAMERA_ORBIT_CHARACTER"))
                return null;

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.CameraRestoreFromVariables,
                Summary = "CAMERA_ORBIT_CHARACTER 로 카메라 각도가 변경됨 — **PreOrbit* 변수** 기준 복구 규칙 필요",
                WiringHint =
                    "ON_ENABLE_GRAPH → GET_FLOAT/VECTOR3_VARIABLE (PreOrbit*) → CAMERA_ORBIT_CHARACTER (TransitionTime=0~0.5)",
            };
        }

        private static ScanSuggestion MatchSetAssetPropertyLookAtOrFov(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "SET_ASSET_PROPERTY"))
                return null;

            var path = ReadStringValue(dataInputs, "DataPath");
            if (string.IsNullOrEmpty(path))
                return null;

            var stripped = StripQuotes(path);
            if (
                stripped.Equals("LookAtEnabled", StringComparison.OrdinalIgnoreCase)
                || stripped.Equals("LookAtTarget", StringComparison.OrdinalIgnoreCase)
            )
            {
                return new ScanSuggestion
                {
                    SourceNodeName = nodeName,
                    SuggestedKind = BlueprintInitRuleKind68.CharacterLookAtAndFov,
                    Summary =
                        $"캐릭터 `{ReadAssetName(dataInputs, "Asset")}` · **{stripped}** 변경 — PreLookAt* 변수로 복구",
                    WiringHint =
                        "ON_ENABLE_GRAPH → GET_BOOLEAN/STRING_VARIABLE + SET_ASSET_PROPERTY (LookAt)",
                };
            }

            if (stripped.Equals("FieldOfView", StringComparison.OrdinalIgnoreCase))
            {
                return new ScanSuggestion
                {
                    SourceNodeName = nodeName,
                    SuggestedKind = BlueprintInitRuleKind68.CharacterLookAtAndFov,
                    Summary = "카메라 **FieldOfView** 변경 — PreFOV 변수로 복구",
                    WiringHint = "ON_ENABLE_GRAPH → GET_FLOAT_VARIABLE (PreFOV) → SET_ASSET_PROPERTY (FOV)",
                };
            }

            return null;
        }

        private static ScanSuggestion MatchGameObjectTransformEasing(
            string nodeName,
            Dictionary<string, SerializedDataInputPort> dataInputs
        )
        {
            if (!NodeNameLooksLike(nodeName, "GameObject Transform Easing"))
                return null;

            var target = ReadAssetName(dataInputs, "Target");

            return new ScanSuggestion
            {
                SourceNodeName = nodeName,
                SuggestedKind = BlueprintInitRuleKind68.GameObjectTransform,
                Summary =
                    $"Transform Easing `{target}` — 연출 중 스케일/위치가 변할 수 있음 — **기본 트랜스폼 스냅** 확인",
                WiringHint =
                    "ON_ENABLE_GRAPH → SET_ASSET_POSITION + (필요 시) 스케일 1,1,1 수동 노드 또는 향후 Reset 노드",
            };
        }

        private static bool NodeNameLooksLike(string nodeName, string pattern) =>
            !string.IsNullOrEmpty(nodeName)
            && nodeName.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;

        private static string ReadAssetName(
            Dictionary<string, SerializedDataInputPort> dataInputs,
            string portKey
        )
        {
            var raw = ReadPortRawValue(dataInputs, portKey);
            if (string.IsNullOrEmpty(raw))
                return "(미지정)";

            try
            {
                var token = JToken.Parse(raw);
                return token["name"]?.ToString() ?? StripQuotes(raw);
            }
            catch
            {
                return StripQuotes(raw);
            }
        }

        private static string ReadStringValue(
            Dictionary<string, SerializedDataInputPort> dataInputs,
            string portKey
        )
        {
            var raw = ReadPortRawValue(dataInputs, portKey);
            return string.IsNullOrEmpty(raw) ? null : StripQuotes(raw);
        }

        private static float ReadFloatValue(
            Dictionary<string, SerializedDataInputPort> dataInputs,
            string portKey
        )
        {
            var raw = ReadPortRawValue(dataInputs, portKey);
            if (string.IsNullOrEmpty(raw))
                return 0f;

            raw = StripQuotes(raw);
            return float.TryParse(raw, out var v) ? v : 0f;
        }

        private static Vector3 ReadVector3Value(
            Dictionary<string, SerializedDataInputPort> dataInputs,
            string portKey
        )
        {
            var raw = ReadPortRawValue(dataInputs, portKey);
            if (string.IsNullOrEmpty(raw))
                return Vector3.zero;

            try
            {
                var token = JToken.Parse(raw);
                return new Vector3(
                    token["x"]?.Value<float>() ?? 0f,
                    token["y"]?.Value<float>() ?? 0f,
                    token["z"]?.Value<float>() ?? 0f
                );
            }
            catch
            {
                return Vector3.zero;
            }
        }

        private static string ReadPortRawValue(
            Dictionary<string, SerializedDataInputPort> dataInputs,
            string portKey
        )
        {
            if (dataInputs == null || !dataInputs.TryGetValue(portKey, out var port))
                return null;

            return port.value;
        }

        private static string StripQuotes(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            s = s.Trim();
            if (s.Length >= 2 && s[0] == '"' && s[^1] == '"')
                return s.Substring(1, s.Length - 2);

            return s;
        }
    }
}
