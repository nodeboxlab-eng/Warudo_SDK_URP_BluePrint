using System.Collections.Generic;

namespace Node68.CustomAssets
{
    /// <summary>모드 전역에서 TMP Resources.Load 결과를 로그로 한 번만 남깁니다 (중복 억제).</summary>
    internal static class Node68TextDisplayFontLoadDiagnostics
    {
        internal static readonly HashSet<string> LoggedOkPaths = new HashSet<string>();
        internal static readonly HashSet<string> LoggedFallbackPaths = new HashSet<string>();
    }
}
