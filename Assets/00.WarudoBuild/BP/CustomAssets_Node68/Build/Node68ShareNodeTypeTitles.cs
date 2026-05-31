using System;

namespace Node68.CustomAssets
{
    /// <summary>
    /// 쉐어 빌드에서 그래프 노드 <see cref="Warudo.Core.Graphs.Node.Name"/> 에 붙는 <c> Shr</c> 접미사와,
    /// 예전 빌드에서 팔레트 제목에 쓰이던 <c> [shr]</c> 잔여 접미사를 정규화할 때 사용합니다.
    /// </summary>
    internal static class Node68ShareNodeTypeTitles
    {
        private const string LegacyPaletteTitleBracketShr = " [shr]";

        internal static string BaseTitleForGraphDisplay(string titleFromTypeMeta)
        {
            if (string.IsNullOrEmpty(titleFromTypeMeta))
                return titleFromTypeMeta;

            var t = titleFromTypeMeta;
            if (t.EndsWith(LegacyPaletteTitleBracketShr, StringComparison.Ordinal))
                t = t[..^LegacyPaletteTitleBracketShr.Length].TrimEnd();
            return t;
        }

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
