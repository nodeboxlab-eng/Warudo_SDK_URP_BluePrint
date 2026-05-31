using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Warudo.Core.Graphs;

namespace Node68.ToolkitMods.Node68DevKit
{
    internal static class BlueprintInitChecklistFormatter
    {
        internal static string Format(
            BlueprintInitProfileAsset profile,
            Graph linkedGraph,
            IReadOnlyList<BlueprintInitGraphScanner.ScanSuggestion> scanSuggestions
        )
        {
            var sb = new StringBuilder();
            var profileName = string.IsNullOrWhiteSpace(profile.ProfileTitle)
                ? profile.Name ?? "Init Profile"
                : profile.ProfileTitle.Trim();

            sb.AppendLine("## 초기화 체크리스트 — " + profileName);
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(profile.Description))
            {
                sb.AppendLine(profile.Description.Trim());
                sb.AppendLine();
            }

            sb.AppendLine("### ON_ENABLE_GRAPH 연결");
            sb.AppendLine(
                "Warudo 재시작·그래프 활성화 시 아래 항목을 **즉시(TransitionTime=0 권장)** 적용하세요."
            );
            sb.AppendLine();
            sb.AppendLine("```");
            sb.AppendLine("ON_ENABLE_GRAPH");
            sb.AppendLine("  → (아래 활성 규칙 순서대로 초기화 노드 연결)");
            sb.AppendLine("  → Exit");
            sb.AppendLine("```");
            sb.AppendLine();

            var rules = profile.Rules ?? Array.Empty<BlueprintInitProfileAsset.InitRuleItem>();
            var enabledRules = rules.Where(r => r != null && r.Enabled).ToList();

            sb.AppendLine(
                "### 등록된 규칙 ("
                    + enabledRules.Count
                    + "/"
                    + rules.Length
                    + " 활성)"
            );
            sb.AppendLine();

            if (enabledRules.Count == 0)
            {
                sb.AppendLine("*활성 규칙이 없습니다. 「규칙」 배열에 항목을 추가하거나 BP 스캔으로 제안을 확인하세요.*");
            }
            else
            {
                var order = 1;
                foreach (var rule in enabledRules.OrderBy(r => r.SortOrder).ThenBy(r => r.Kind))
                {
                    sb.AppendLine(FormatRuleLine(order++, rule));
                    var hint = DescribeWiringHint(rule);
                    if (!string.IsNullOrEmpty(hint))
                        sb.AppendLine("   - BP 노드: " + hint);

                    foreach (var warning in CollectRuleWarnings(rule, linkedGraph))
                        sb.AppendLine("   - ⚠ " + warning);

                    if (!string.IsNullOrWhiteSpace(rule.Note))
                        sb.AppendLine("   - 메모: " + rule.Note.Trim());

                    sb.AppendLine();
                }
            }

            var warnings = CollectProfileWarnings(profile, linkedGraph, enabledRules);
            if (warnings.Count > 0)
            {
                sb.AppendLine("### ⚠ 주의");
                foreach (var w in warnings)
                    sb.AppendLine("- " + w);
                sb.AppendLine();
            }

            if (scanSuggestions != null && scanSuggestions.Count > 0)
            {
                sb.AppendLine("### BP 스캔 제안 (규칙에 아직 없을 수 있음)");
                sb.AppendLine(
                    "연결된 블루프린트에서 **변경을 일으키는 노드**를 분석한 결과입니다."
                );
                sb.AppendLine();

                var idx = 1;
                foreach (var s in scanSuggestions)
                {
                    var covered = IsSuggestionCovered(s, enabledRules);
                    var mark = covered ? "[≈]" : "[+]";
                    sb.AppendLine(
                        mark
                            + " **"
                            + idx++
                            + ". "
                            + KindLabel(s.SuggestedKind)
                            + "** · `"
                            + s.SourceNodeName
                            + "`"
                    );
                    sb.AppendLine("   - " + s.Summary);
                    sb.AppendLine("   - 연결: " + s.WiringHint);
                    sb.AppendLine();
                }

                sb.AppendLine(
                    "범례: `[+]` 아직 규칙에 없음 · `[≈]` 유사 규칙이 이미 있음"
                );
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine("*「체크리스트 갱신」으로 이 내용을 다시 생성합니다.*");

            return sb.ToString().TrimEnd();
        }

        private static string FormatRuleLine(int order, BlueprintInitProfileAsset.InitRuleItem rule)
        {
            var target = ResolveTargetLabel(rule);
            switch (rule.Kind)
            {
                case BlueprintInitRuleKind68.GameObjectEnabled:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · `"
                        + target
                        + "` → "
                        + (rule.BoolValue ? "**활성**" : "**비활성**");

                case BlueprintInitRuleKind68.GameObjectTransform:
                    var p = rule.Vector3Value;
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · `"
                        + target
                        + "` → Pos ("
                        + p.x.ToString("0.###")
                        + ", "
                        + p.y.ToString("0.###")
                        + ", "
                        + p.z.ToString("0.###")
                        + ")";

                case BlueprintInitRuleKind68.PropBlendShape:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · `"
                        + target
                        + "` · `"
                        + NullSafe(rule.SkinnedMeshKey)
                        + "` / `"
                        + NullSafe(rule.BlendShapeName)
                        + "` → **"
                        + rule.FloatValue.ToString("0.###")
                        + "**";

                case BlueprintInitRuleKind68.TextDisplayReset:
                    var td = rule.TextDisplay?.Name ?? target;
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · `"
                        + td
                        + "` → 텍스트 \""
                        + NullSafe(rule.StringValue)
                        + "\", "
                        + (rule.BoolValue ? "숨김" : "표시");

                case BlueprintInitRuleKind68.CameraRestoreFromVariables:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · Orbit `"
                        + NullSafe(rule.StringValue)
                        + "`, `"
                        + NullSafe(rule.StringValue2)
                        + "`, `"
                        + NullSafe(rule.StringValue3)
                        + "` · FOV `"
                        + NullSafe(rule.StringValue4)
                        + "`";

                case BlueprintInitRuleKind68.CharacterLookAtAndFov:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · 캐릭터 `"
                        + (rule.Character?.Name ?? target)
                        + "` · LookAt="
                        + rule.BoolValue
                        + " · 카메라="
                        + NullSafe(rule.StringValue);

                case BlueprintInitRuleKind68.GraphVariable:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · `"
                        + NullSafe(rule.StringValue)
                        + "` → "
                        + rule.FloatValue.ToString("0.###");

                case BlueprintInitRuleKind68.ManualNote:
                default:
                    return order
                        + ". **"
                        + KindLabel(rule.Kind)
                        + "** · "
                        + (string.IsNullOrWhiteSpace(rule.Note) ? "(메모 없음)" : rule.Note.Trim());
            }
        }

        private static string DescribeWiringHint(BlueprintInitProfileAsset.InitRuleItem rule)
        {
            switch (rule.Kind)
            {
                case BlueprintInitRuleKind68.GameObjectEnabled:
                    return rule.BoolValue
                        ? "TOGGLE_ASSET_ENABLED (활성)"
                        : "TOGGLE_ASSET_ENABLED (비활성)";

                case BlueprintInitRuleKind68.GameObjectTransform:
                    return "SET_ASSET_POSITION (TransitionTime=0)";

                case BlueprintInitRuleKind68.PropBlendShape:
                    return "SET_PROP_BLENDSHAPE (TransitionTime=0)";

                case BlueprintInitRuleKind68.TextDisplayReset:
                    return "Set Display Text + Text Display Show/Hide (UseFade=false)";

                case BlueprintInitRuleKind68.CameraRestoreFromVariables:
                    return "GET_*_VARIABLE → CAMERA_ORBIT_CHARACTER + SET_CAMERA_CONTROL_MODE + FOV";

                case BlueprintInitRuleKind68.CharacterLookAtAndFov:
                    return "GET_*_VARIABLE → SET_ASSET_PROPERTY (LookAtEnabled / LookAtTarget / FOV)";

                case BlueprintInitRuleKind68.GraphVariable:
                    return "SET_*_VARIABLE 또는 수동 변수 노드";

                default:
                    return null;
            }
        }

        private static IEnumerable<string> CollectRuleWarnings(
            BlueprintInitProfileAsset.InitRuleItem rule,
            Graph linkedGraph
        )
        {
            switch (rule.Kind)
            {
                case BlueprintInitRuleKind68.GameObjectEnabled:
                case BlueprintInitRuleKind68.GameObjectTransform:
                case BlueprintInitRuleKind68.PropBlendShape:
                    if (rule.TargetAsset == null)
                        yield return "대상 GameObject/Prop 이 지정되지 않았습니다.";
                    if (string.IsNullOrWhiteSpace(rule.BlendShapeName))
                        yield return "블렌드쉐이프 이름이 비어 있습니다.";
                    break;

                case BlueprintInitRuleKind68.TextDisplayReset:
                    if (rule.TextDisplay == null)
                        yield return "TextDisplay 에셋이 지정되지 않았습니다.";
                    break;

                case BlueprintInitRuleKind68.CameraRestoreFromVariables:
                    if (linkedGraph != null)
                    {
                        if (!GraphHasVariable(linkedGraph, rule.StringValue))
                            yield return "Graph Variable `" + rule.StringValue + "` 없음";
                        if (!GraphHasVariable(linkedGraph, rule.StringValue2))
                            yield return "Graph Variable `" + rule.StringValue2 + "` 없음";
                    }
                    break;
            }
        }

        private static List<string> CollectProfileWarnings(
            BlueprintInitProfileAsset profile,
            Graph linkedGraph,
            List<BlueprintInitProfileAsset.InitRuleItem> enabledRules
        )
        {
            var list = new List<string>();

            if (linkedGraph == null && !string.IsNullOrEmpty(profile.TargetGraphId))
                list.Add("연결된 블루프린트를 찾을 수 없습니다 (ID 확인).");

            if (linkedGraph != null && !linkedGraph.Enabled)
                list.Add(
                    "연결된 그래프 `"
                        + linkedGraph.Name
                        + "` 가 **비활성** — ON_ENABLE_GRAPH 가 실행되지 않습니다."
                );

            if (
                enabledRules.Any(r => r.Kind == BlueprintInitRuleKind68.GameObjectTransform)
                && enabledRules.Any(r =>
                    r.Kind == BlueprintInitRuleKind68.GameObjectEnabled && !r.BoolValue
                )
            )
            {
                var transformOrder = enabledRules
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.SortOrder)
                    .FirstOrDefault(r => r.Kind == BlueprintInitRuleKind68.GameObjectTransform);
                var disableOrder = enabledRules
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.SortOrder)
                    .FirstOrDefault(r =>
                        r.Kind == BlueprintInitRuleKind68.GameObjectEnabled && !r.BoolValue
                    );

                if (
                    transformOrder != null
                    && disableOrder != null
                    && transformOrder.SortOrder > disableOrder.SortOrder
                )
                {
                    list.Add(
                        "비활성화(SortOrder="
                            + disableOrder.SortOrder
                            + ")가 위치 이동("
                            + transformOrder.SortOrder
                            + ")보다 먼저 — 이동 중 프롭이 보일 수 있습니다."
                    );
                }
            }

            if (
                !enabledRules.Any(r =>
                    r.Kind == BlueprintInitRuleKind68.CameraRestoreFromVariables
                )
                && enabledRules.Any(r =>
                    r.Kind
                        is BlueprintInitRuleKind68.GameObjectTransform
                            or BlueprintInitRuleKind68.PropBlendShape
                )
            )
            {
                list.Add(
                    "카메라 복구 규칙이 없습니다 — 연출 BP라면 CameraRestore 규칙 추가를 검토하세요."
                );
            }

            return list;
        }

        private static bool IsSuggestionCovered(
            BlueprintInitGraphScanner.ScanSuggestion suggestion,
            List<BlueprintInitProfileAsset.InitRuleItem> enabledRules
        )
        {
            return enabledRules.Any(r => r.Kind == suggestion.SuggestedKind);
        }

        private static bool GraphHasVariable(Graph graph, string variableName)
        {
            if (graph?.Properties == null || string.IsNullOrWhiteSpace(variableName))
                return false;

            try
            {
                return graph.Properties.GetVariable(StripQuotes(variableName)) != null;
            }
            catch
            {
                return false;
            }
        }

        private static string ResolveTargetLabel(BlueprintInitProfileAsset.InitRuleItem rule) =>
            rule.TargetAsset?.Name
            ?? rule.TextDisplay?.Name
            ?? rule.Character?.Name
            ?? "(미지정)";

        private static string KindLabel(BlueprintInitRuleKind68 kind) =>
            kind switch
            {
                BlueprintInitRuleKind68.GameObjectEnabled => "GameObject 활성",
                BlueprintInitRuleKind68.GameObjectTransform => "GameObject 트랜스폼",
                BlueprintInitRuleKind68.PropBlendShape => "Prop 블렌드쉐이프",
                BlueprintInitRuleKind68.TextDisplayReset => "TextDisplay 리셋",
                BlueprintInitRuleKind68.CameraRestoreFromVariables => "카메라 복구",
                BlueprintInitRuleKind68.CharacterLookAtAndFov => "LookAt / FOV",
                BlueprintInitRuleKind68.GraphVariable => "Graph Variable",
                BlueprintInitRuleKind68.ManualNote => "수동 안내",
                _ => kind.ToString(),
            };

        private static string NullSafe(string s) =>
            string.IsNullOrWhiteSpace(s) ? "?" : s.Trim();

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
