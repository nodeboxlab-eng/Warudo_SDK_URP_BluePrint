using UnityEditor;

namespace Node68.CustomNodes.Editor
{
    /// <summary>
    /// Bazzi 한글 TMP는 Font Asset Creator로 만듭니다.
    /// 구버전 자동 생성(TMP 내부 API 직접 조작)은 TMP 3.x 에서 컴파일되지 않아 제거했습니다.
    /// </summary>
    internal static class Node68BazziTmpFontAssetGenerator
    {
        private const string FontAssetCreatorMenu = "Window/TextMeshPro/Font Asset Creator";

        [MenuItem("Node68/Bazzi · TMP 폰트 (Font Asset Creator)")]
        private static void OpenFontAssetCreator()
        {
            EditorApplication.ExecuteMenuItem(FontAssetCreatorMenu);
        }
    }
}
