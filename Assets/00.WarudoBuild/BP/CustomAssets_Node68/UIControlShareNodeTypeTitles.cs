using System;

namespace Warudo.UIControl
{
    /// <summary>
    /// UI 리모컨 노드 그래프 <see cref="Warudo.Core.Graphs.Node.Name"/> 의 <c> Shr</c> 접미사와,
    /// 예전에 팔레트 제목에 붙던 <c> 쉐어</c> 잔여 접미사(역호환)를 정규화합니다.
    /// 카테고리 구분은 <c>[NodeType]</c>/<c>[AssetType]</c> 의 Category 분기(<see cref="UIControlNodeCategories"/>) 로 합니다.
    /// </summary>
    internal static class UIControlShareNodeTypeTitles
    {
        private const string LegacyPaletteTitleShareSuffix = " 쉐어";

        internal static string BaseTitleForGraphDisplay(string titleFromTypeMeta)
        {
            if (string.IsNullOrEmpty(titleFromTypeMeta))
                return titleFromTypeMeta;

            var t = titleFromTypeMeta;
            if (t.EndsWith(LegacyPaletteTitleShareSuffix, StringComparison.Ordinal))
                t = t[..^LegacyPaletteTitleShareSuffix.Length].TrimEnd();
            return t;
        }

        internal static string StripToBaseDisplayName(string name, string graphNodeSuffix)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var s = name.TrimEnd();
            if (!string.IsNullOrEmpty(graphNodeSuffix) && s.EndsWith(graphNodeSuffix, StringComparison.Ordinal))
                s = s[..^graphNodeSuffix.Length].TrimEnd();
            if (s.EndsWith(LegacyPaletteTitleShareSuffix, StringComparison.Ordinal))
                s = s[..^LegacyPaletteTitleShareSuffix.Length].TrimEnd();
            return s;
        }
    }
}
