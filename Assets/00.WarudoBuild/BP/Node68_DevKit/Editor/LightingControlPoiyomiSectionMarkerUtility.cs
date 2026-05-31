using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Node68.ToolkitMods.LightingControl.Editor
{
    /// <summary>
    /// Poiyomi Toon 셰이더 Properties 를 스캔해 ToggleUI / ThryToggle / ThryToggleUI / Toggle(…) 등
    /// 재질 인스펙터 No·Yes 토글이 있는 폴드를 찾고, PoiyomiLightingShaderControlBase / 파생 에셋의 [Section] 제목에 접두를 붙입니다.
    /// [T] = 폴드 reference_property 가 토글 드로어, ● = 그 외로 내부(직접·자식)에 토글이 있는 경우.
    /// </summary>
    public static class LightingControlPoiyomiSectionMarkerUtility
    {
        internal const string DefaultPoiyomiToonUrShaderPath =
            "Assets/_PoiyomiShaders/Shaders/10.0/Toon/Poiyomi Toon URP.shader";

        internal const string TargetAssetCsPath =
            "Assets/00.WarudoBuild/BP/Node68_DevKit/PoiyomiLightingControlAsset.cs";

        static readonly Regex ShaderPropRegex = new Regex(
            @"^\s*((?:\[[^\]]+\]\s*)+)([_a-zA-Z0-9]+)\s*\(",
            RegexOptions.Compiled
        );

        static readonly Regex FoldHideHeaderRegex = new Regex(
            @"\[HideInInspector\]\s*(?<tag>m_start_\w+|s_start_\w+|m_end_\w+|s_end_\w+)\s*\(\s*""(?<raw>[^""]*)""",
            RegexOptions.Compiled
        );

        static readonly Regex MainCategoryRegex = new Regex(
            @"\[HideInInspector\]\s*m_mainCategory\s*\(\s*""(?<raw>[^""]*)""",
            RegexOptions.Compiled
        );

        static readonly Regex RawRefPropRegex = new Regex(
            @"reference_property:\s*(_\w+)",
            RegexOptions.Compiled
        );

        static readonly Regex ToggleDrawerRegex = new Regex(
            @"\[ThryToggle[^\]]*\]|\bThryToggleUI\b|\[ToggleUI[^\]]*\]|ToggleUI|"
                + @"\[KeywordToggle[^\]]*\]|\[MaterialToggle[^\]]*\]|\[Toggle\(\s*[A-Za-z0-9_]+\s*\]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        internal enum MarkerKind : byte
        {
            None = 0,
            PrimaryKeywordOrRefToggle,
            NestedUiToggleOnly,
        }

        sealed class FoldNode
        {
            public string FoldTagName = "";
            public string PrettyTitle = "";
            public string FullPathNormalized = "";
            public string ReferenceProperty;

            public readonly List<string> LoosePropNames = new List<string>();
            public readonly List<FoldNode> Children = new List<FoldNode>();
            public MarkerKind Marker;

            internal bool subtreeHasAnyToggleDrawer;
        }

        [MenuItem("Node68/Lighting Control/Poiyomi 토글 섹션 마킹 갱신 (LST)", priority = 10)]
        public static void RefreshSectionMarkersFromPoiyomiShader()
        {
            var shaderRel = DefaultPoiyomiToonUrShaderPath;
            var absShader = Path.Combine(Directory.GetCurrentDirectory(), shaderRel);

            if (!File.Exists(absShader))
            {
                EditorUtility.DisplayDialog(
                    "Poiyomi 섹션 마킹",
                    "셰이더 파일이 없습니다:\n" + shaderRel,
                    "확인");
                return;
            }

            try
            {
                RefreshSectionMarkers(shaderRel, TargetAssetCsPath, out var log);

                Debug.Log(log);

                EditorUtility.DisplayDialog(
                    "Poiyomi 섹션 마킹",
                    "`PoiyomiLightingControlAsset` · `PoiyomiLightingControlPropAsset` 의 [Section] 접두사를 재작성했습니다.",
                    "확인");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorUtility.DisplayDialog("Poiyomi 섹션 마킹", ex.Message, "확인");
            }
        }

        static void RefreshSectionMarkers(string shaderRelativePath, string csRelativePath, out string summary)
        {
            var shaderFull = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), shaderRelativePath));

            var shaderSrc = File.ReadAllText(shaderFull, Encoding.UTF8);
            var props = ExtractPropertiesBlock(shaderSrc);

            var lines = props.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var propAttrs = BuildPropertyAttributeMap(lines);

            var root = new FoldNode { FoldTagName = "<root>" };
            var stack = new Stack<FoldNode>();

            stack.Push(root);

            for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var rawLine = lines[lineIndex];

                if (FoldHideHeaderRegex.Match(rawLine) is { Success: true } hideMatch)
                {
                    ApplyFoldHeader(stack, hideMatch.Groups["tag"].Value, hideMatch.Groups["raw"].Value);

                    continue;
                }

                if (MainCategoryRegex.Match(rawLine) is { Success: true } mainCat)
                {
                    var pretty = TrimPrettyTitle(mainCat.Groups["raw"].Value);

                    stack.Push(
                        new FoldNode
                        {
                            FoldTagName = "<CN>",
                            PrettyTitle = pretty,
                            FullPathNormalized = CollapseSpaces(pretty),
                        });

                    continue;
                }

                var propMatch = ShaderPropRegex.Match(rawLine);

                if (!propMatch.Success)
                    continue;

                stack.Peek().LoosePropNames.Add(propMatch.Groups[2].Value);
            }

            if (stack.Count != 1)
                Debug.LogWarning(
                    "[Poiyomi LST] Properties 끝에서 스택 깊이 " + stack.Count + " (접힘 불일치 가능).");

            AttachVirtualShadowsGeneric(root, lines, propAttrs);
            AnalyzeSubtreeToggles(root, propAttrs);
            AssignMarkers(root, propAttrs);

            var pathMarks = new Dictionary<string, MarkerKind>(StringComparer.OrdinalIgnoreCase);

            CollectMarks(root, pathMarks);

            var csFull = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), csRelativePath));

            var csText = File.ReadAllText(csFull, Encoding.UTF8);
            var newText = RewriteSectionTitles(csText, pathMarks, out var applied, out var unmatched);

            File.WriteAllText(csFull, newText, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            AssetDatabase.Refresh();

            summary =
                "[Poiyomi LST] [Section] "
                + applied
                + "개 갱신."
                + (unmatched.Count > 0 ? " 미매치: " + string.Join("; ", unmatched) : "");
        }

        static void ApplyFoldHeader(Stack<FoldNode> stack, string tag, string rawHeader)
        {
            if (tag.StartsWith("m_end_", StringComparison.Ordinal) || tag.StartsWith("s_end_", StringComparison.Ordinal))
            {
                if (stack.Count > 1)
                    stack.Pop();

                return;
            }

            if (tag == "m_start_ColorAdjust" && stack.Peek().FoldTagName == "<CN>")
            {
                var cn = stack.Pop();
                stack.Peek().Children.Add(cn);
            }

            var pretty = TrimPrettyTitle(rawHeader);
            var nm = RawRefPropRegex.Match(rawHeader);
            string refProp = nm.Success ? nm.Groups[1].Value : null;

            var parent = stack.Peek();
            string full =
                parent.FoldTagName == "<root>" || string.IsNullOrEmpty(parent.FullPathNormalized)
                    ? CollapseSpaces(pretty)
                    : parent.FullPathNormalized + " / " + CollapseSpaces(pretty);

            var node = new FoldNode
            {
                FoldTagName = tag,
                PrettyTitle = pretty,
                FullPathNormalized = full,
                ReferenceProperty = refProp,
            };

            parent.Children.Add(node);
            stack.Push(node);
        }

        static void AttachVirtualShadowsGeneric(FoldNode root, string[] lines, Dictionary<string, string> attrs)
        {
            var ie = IndexOfLineContains(lines, "s_end_MultilayerMathBorderMap");
            var ich = IndexOfLineContains(lines, "//ifex _LightingMode!=2");

            if (ie < 0 || ich < 0 || ich <= ie)
                return;

            var loosen = CollectPropLinesBetween(lines, ie + 1, ich - 1);

            bool any = loosen.Any(nm => ToggleDrawerRegex.IsMatch(attrs[nm]));

            if (!any)
                return;

            var node = new FoldNode
            {
                FoldTagName = "<VSG>",
                PrettyTitle = "Generic",
                FullPathNormalized = "Shadows / Generic",
                subtreeHasAnyToggleDrawer = true,
                Marker = MarkerKind.NestedUiToggleOnly,
            };

            foreach (var p in loosen)
                node.LoosePropNames.Add(p);

            root.Children.Add(node);
        }

        static List<string> CollectPropLinesBetween(
            string[] lines,
            int incStart,
            int incEndInclusive)
        {
            var ids = new List<string>();

            for (var i = Mathf.Max(0, incStart); i <= incEndInclusive && i < lines.Length; i++)
            {
                var m = ShaderPropRegex.Match(lines[i]);

                if (!m.Success)
                    continue;

                ids.Add(m.Groups[2].Value);
            }

            return ids;
        }

        static int IndexOfLineContains(IReadOnlyList<string> lines, string sub)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].IndexOf(sub, StringComparison.Ordinal) >= 0)
                    return i;
            }

            return -1;
        }

        static void AnalyzeSubtreeToggles(FoldNode n, IReadOnlyDictionary<string, string> propAttrs)
        {
            foreach (var c in n.Children)
                AnalyzeSubtreeToggles(c, propAttrs);

            var mine = false;

            foreach (var p in n.LoosePropNames)
            {
                if (propAttrs.TryGetValue(p, out var blob) && ToggleDrawerRegex.IsMatch(blob))
                    mine = true;
            }

            var childAny = false;

            foreach (var c in n.Children)
            {
                if (c.subtreeHasAnyToggleDrawer)
                    childAny = true;
            }

            n.subtreeHasAnyToggleDrawer = mine || childAny;
        }

        static void AssignMarkers(FoldNode n, Dictionary<string, string> attrs)
        {
            foreach (var c in n.Children)
                AssignMarkers(c, attrs);

            if (n.FoldTagName == "<root>" || string.IsNullOrEmpty(n.FullPathNormalized))
                return;

            if (n.FoldTagName == "<VSG>")
                return;

            var prim =
                !string.IsNullOrEmpty(n.ReferenceProperty)
                && attrs.TryGetValue(n.ReferenceProperty, out var ra)
                && ToggleDrawerRegex.IsMatch(ra);

            bool rest =
                RestHasToggleExcludingRef(n.LoosePropNames, n.ReferenceProperty, attrs)
                || n.Children.Any(c => c.subtreeHasAnyToggleDrawer);

            if (prim)
                n.Marker = MarkerKind.PrimaryKeywordOrRefToggle;
            else if (rest)
                n.Marker = MarkerKind.NestedUiToggleOnly;
        }

        static bool RestHasToggleExcludingRef(
            IReadOnlyList<string> loose,
            string refProp,
            IReadOnlyDictionary<string, string> attrs)
        {
            foreach (var p in loose)
            {
                if (!string.IsNullOrEmpty(refProp) && string.Equals(p, refProp, StringComparison.Ordinal))
                    continue;

                if (attrs.TryGetValue(p, out var ab) && ToggleDrawerRegex.IsMatch(ab))
                    return true;
            }

            return false;
        }

        static void CollectMarks(FoldNode n, Dictionary<string, MarkerKind> dict)
        {
            if (n.FoldTagName != "<root>" && n.Marker != MarkerKind.None && !string.IsNullOrEmpty(n.FullPathNormalized))
                dict[n.FullPathNormalized] = n.Marker;

            foreach (var c in n.Children)
                CollectMarks(c, dict);
        }

        static string RewriteSectionTitles(
            string cs,
            IReadOnlyDictionary<string, MarkerKind> marks,
            out int applied,
            out List<string> unmatched)
        {
            applied = 0;
            unmatched = new List<string>();

            var rx = new Regex(@"\[Section\(\s*""([^""]*)""\s*\)\]");
            var sb = new StringBuilder(cs.Length + 64);
            var last = 0;

            foreach (Match m in rx.Matches(cs))
            {
                sb.Append(cs, last, m.Index - last);

                var rawTitle = m.Groups[1].Value;
                var stripped = StripOldMarkers(rawTitle);
                var nk = LookupMarkerKind(stripped, marks, out var found);

                if (!found && CollapseSpaces(stripped) != "Warudo")
                    unmatched.Add(CollapseSpaces(stripped));

                var decorated = Decorate(stripped, nk);

                sb.Append("[Section(\"");
                sb.Append(escapedCQ(decorated));

                sb.Append("\")]");

                if (nk != MarkerKind.None || stripped != rawTitle.Trim())
                    applied++;

                last = m.Index + m.Length;
            }

            sb.Append(cs, last, cs.Length - last);

            return sb.ToString();
        }

        static string escapedCQ(string s) => s.Replace("\\", "\\\\").Replace("\"", "\\\"");

        static string Decorate(string stripped, MarkerKind nk)
        {
            switch (nk)
            {
                case MarkerKind.PrimaryKeywordOrRefToggle:
                    return "[T] " + stripped;
                case MarkerKind.NestedUiToggleOnly:
                    return "● " + stripped;
                default:

                    return stripped;
            }
        }

        static MarkerKind LookupMarkerKind(
            string strippedTitle,
            IReadOnlyDictionary<string, MarkerKind> marks,
            out bool foundExact)
        {
            foundExact = false;
            var key = CollapseSpaces(strippedTitle);

            if (marks.TryGetValue(key, out var mk))
            {
                foundExact = true;
                return mk;
            }

            foreach (var kv in marks)
            {
                if (string.Equals(CollapseSpaces(kv.Key), key, StringComparison.OrdinalIgnoreCase))
                {
                    foundExact = true;
                    return kv.Value;
                }
            }

            MarkerKind best = MarkerKind.None;
            var longest = -1;

            foreach (var kv in marks)
            {
                var sk = CollapseSpaces(kv.Key);

                if (key.EndsWith(sk, StringComparison.OrdinalIgnoreCase))
                {
                    if (sk.Length > longest)
                    {
                        longest = sk.Length;

                        best = kv.Value;

                        foundExact = true;
                    }
                }
            }

            if (foundExact)
                return best;

            foreach (var kv in marks)
            {
                var sk = CollapseSpaces(kv.Key);

                if (sk.EndsWith(key, StringComparison.OrdinalIgnoreCase) && sk.Length <= key.Length + 4)
                {
                    foundExact = true;

                    return kv.Value;
                }

                var sw = SplitTail(key);

                var skTail = SplitTail(sk);

                if (!string.IsNullOrEmpty(sw) && string.Equals(sw, skTail, StringComparison.OrdinalIgnoreCase))
                {
                    foundExact = true;

                    return kv.Value;
                }
            }

            return MarkerKind.None;
        }

        static string SplitTail(string path)
        {
            var idx = path.LastIndexOf('/');

            return idx < 0 ? path : CollapseSpaces(path.Substring(idx + 1));
        }

        static string StripOldMarkers(string t)
        {
            var s = t.TrimStart();

            s = Regex.Replace(s, @"^\s*\[T\]\s+", "");
            s = Regex.Replace(s, @"^\s*[●‧]\s+", "");
            s = Regex.Replace(s, @"^Toggle Section\s*:\s*", "", RegexOptions.IgnoreCase);

            return CollapseSpaces(s);
        }

        static string CollapseSpaces(string s) => Regex.Replace((s ?? "").Trim(), @"\s+", " ");

        static Dictionary<string, string> BuildPropertyAttributeMap(string[] propLines)
        {
            var r = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var raw in propLines)
            {
                var m = ShaderPropRegex.Match(raw);

                if (!m.Success)
                    continue;

                r[m.Groups[2].Value] = m.Groups[1].Value.Trim();
            }

            return r;
        }

        static string ExtractPropertiesBlock(string shaderSrc)
        {
            var p0 = shaderSrc.IndexOf("Properties", StringComparison.Ordinal);

            if (p0 < 0)
                throw new InvalidOperationException("Properties 블록을 찾을 수 없습니다.");

            var b0 = shaderSrc.IndexOf('{', p0);

            if (b0 < 0)
                throw new InvalidOperationException("Properties `{` 없음.");

            var depth = 0;

            for (var i = b0; i < shaderSrc.Length; i++)
            {
                if (shaderSrc[i] == '{')
                    depth++;
                else if (shaderSrc[i] == '}')
                {
                    depth--;

                    if (depth == 0)
                        return shaderSrc.Substring(b0 + 1, i - b0 - 1);
                }
            }

            throw new InvalidOperationException("Properties `}` 없음.");
        }

        static string TrimPrettyTitle(string rawDisp)
        {
            var t = rawDisp.TrimStart();
            var cut = t.IndexOf("--", StringComparison.Ordinal);

            return CollapseSpaces(cut >= 0 ? t.Substring(0, cut) : t);
        }

    }
}
