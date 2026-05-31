using System;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 쉐어 빌드에서 그래프 노드 <see cref="Warudo.Core.Graphs.Node.Name"/> 에 붙는 <c> Shr</c> 접미사와,
    /// 예전 빌드에서 팔레트 제목에 쓰이던 <c> [shr]</c> 잔여 접미사를 정규화할 때 사용합니다.
    /// </summary>
    internal static class Node68ShareNodeTypeTitles
    {
        /// <summary>구버전: Title 속성에 붙던 팔레트 접미사(더 이상 쓰이지 않음, 역호환).</summary>
        private const string LegacyPaletteTitleBracketShr = " [shr]";

        /// <summary>
        /// 팔레트 제목 문자열에서 그래프용 기준 제목으로 쓸 때 — 예전 <see cref="LegacyPaletteTitleBracketShr"/> 만 제거합니다.
        /// </summary>
        internal static string BaseTitleForGraphDisplay(string titleFromTypeMeta)
        {
            if (string.IsNullOrEmpty(titleFromTypeMeta))
                return titleFromTypeMeta;

            var t = titleFromTypeMeta;
            if (t.EndsWith(LegacyPaletteTitleBracketShr, StringComparison.Ordinal))
                t = t[..^LegacyPaletteTitleBracketShr.Length].TrimEnd();
            return t;
        }

        /// <summary>
        /// <see cref="Warudo.Core.Graphs.Node.Name"/> 에서 그래프 접미사(예: <c> Shr</c>)와 예전 팔레트 접미사를
        /// 순서대로 제거한 뒤의 코어 이름.
        /// </summary>
        internal static string StripToBaseDisplayName(string name, string graphNodeSuffix)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var s = name.TrimEnd();
            if (!string.IsNullOrEmpty(graphNodeSuffix) && s.EndsWith(graphNodeSuffix, StringComparison.Ordinal))
                s = s[..^graphNodeSuffix.Length].TrimEnd();
            if (s.EndsWith(LegacyPaletteTitleBracketShr, StringComparison.Ordinal))
                s = s[..^LegacyPaletteTitleBracketShr.Length].TrimEnd();
            return s;
        }
    }
}
