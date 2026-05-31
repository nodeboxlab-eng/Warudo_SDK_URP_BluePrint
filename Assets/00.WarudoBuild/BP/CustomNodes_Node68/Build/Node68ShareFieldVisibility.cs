using System;
using Warudo.Core.Scenes;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 쉐어 빌드 UI: 제작자가 값을 넣어 둔 필드는 숨기고, 비어 있으면 수신자가 채울 수 있게 표시합니다.
    /// </summary>
    internal static class Node68ShareFieldVisibility
    {
        internal static bool HideDevOnlyInShare(bool isShareBuild) => isShareBuild;

        internal static bool HideInShareWhenAssetSet(bool isShareBuild, Asset asset) =>
            isShareBuild && asset != null;

        internal static bool HideInShareWhenStringSet(bool isShareBuild, string value) =>
            isShareBuild && !string.IsNullOrWhiteSpace(value);

        internal static bool HideInShareWhenArrayHasItems(bool isShareBuild, Array array) =>
            isShareBuild && array != null && array.Length > 0;
    }
}
