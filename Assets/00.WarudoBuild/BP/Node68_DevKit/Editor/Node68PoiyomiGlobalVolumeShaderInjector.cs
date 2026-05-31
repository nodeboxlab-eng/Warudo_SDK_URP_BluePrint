#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Node68.ToolkitMods.Node68DevKit.Editor
{
    /// <summary>
    /// Poiyomi 셰이더에 Node68 전역 Volume include·적용 매크로를 주입합니다.
    /// </summary>
    public static class Node68PoiyomiGlobalVolumeShaderInjector
    {
        private const string IncludeLine =
            "#include \"Assets/00.WarudoBuild/BP/Node68_DevKit/Shaders/Node68PoiyomiGlobalVolume.hlsl\"";

        private const string BaseColorDimApplyMarker = "NODE68_APPLY_GLOBAL_BASE_COLOR_DIM";
        private const string LightingCapApplyMarker = "NODE68_APPLY_GLOBAL_LIGHTING_CAP";

        private static readonly string[] DefaultShaderPaths =
        {
            "Assets/_PoiyomiShaders/Shaders/10.0/Pro/Poiyomi Pro URP.shader",
            "Assets/nightfall/Model/OptimizedShaders/[node68] 날개 mat/Poiyomi Pro.shader",
        };

        [MenuItem("Node68 DevKit/Poiyomi Volume/Patch All Poiyomi Pro URP In Project")]
        public static void PatchAllProjectShaders()
        {
            var changed = 0;
            var scanned = 0;
            foreach (
                var path in Directory.GetFiles(
                    "Assets",
                    "Poiyomi Pro URP.shader",
                    SearchOption.AllDirectories
                )
            )
            {
                var assetPath = path.Replace('\\', '/');
                scanned++;
                if (PatchShaderFile(assetPath))
                    changed++;
            }

            AssetDatabase.Refresh();
            Debug.Log(
                $"[Node68 Poiyomi Volume] 프로젝트 전체 패치: {changed}/{scanned}개 변경"
            );
        }

        [MenuItem("Node68 DevKit/Poiyomi Volume/Patch Default Shaders")]
        public static void PatchDefaultShaders()
        {
            var changed = 0;
            foreach (var path in DefaultShaderPaths)
                changed += PatchShaderFile(path) ? 1 : 0;

            AssetDatabase.Refresh();
            Debug.Log(
                $"[Node68 Poiyomi Volume] 기본 셰이더 패치 완료: {changed}/{DefaultShaderPaths.Length}개"
            );
        }

        [MenuItem("Node68 DevKit/Poiyomi Volume/Patch Selected Shader Files")]
        public static void PatchSelectedShaders()
        {
            var changed = 0;
            foreach (var obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) || !path.EndsWith(".shader"))
                    continue;
                if (PatchShaderFile(path))
                    changed++;
            }

            AssetDatabase.Refresh();
            Debug.Log($"[Node68 Poiyomi Volume] 선택 셰이더 패치: {changed}개");
        }

        public static bool PatchShaderFile(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                Debug.LogWarning($"[Node68 Poiyomi Volume] 파일 없음: {assetPath}");
                return false;
            }

            var text = File.ReadAllText(assetPath);
            var original = text;
            var nl = text.Contains("\r\n") ? "\r\n" : "\n";

            TryInjectInclude(ref text, nl);
            TryPatchGlobalBaseColorDim(ref text, nl);
            TryPatchGlobalLightingCap(ref text, nl);

            if (text == original)
                return false;

            File.WriteAllText(assetPath, text);
            Debug.Log($"[Node68 Poiyomi Volume] 패치됨: {assetPath}");
            return true;
        }

        private static void TryInjectInclude(ref string text, string nl)
        {
            if (text.Contains(IncludeLine))
                return;

            const string urpLighting =
                "#include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl\"";
            if (text.Contains(urpLighting))
            {
                text = text.Replace(urpLighting, urpLighting + nl + "\t\t" + IncludeLine);
                return;
            }

            const string urpCore =
                "#include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl\"";
            if (text.Contains(urpCore))
                text = text.Replace(urpCore, urpCore + nl + "\t\t" + IncludeLine);
        }

        private static void TryPatchGlobalBaseColorDim(ref string text, string nl)
        {
            if (text.Contains(BaseColorDimApplyMarker))
                return;

            var mainAdjustAnchor =
                $"\t\t\t\t#endif{nl}\t\t\t\t{nl}\t\t\t\t//ifex _MainColorAdjustToggle==0";
            var mainAdjustPatch =
                $"\t\t\t\t#endif{nl}\t\t\t\tNODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);{nl}\t\t\t\t{nl}\t\t\t\t//ifex _MainColorAdjustToggle==0";
            if (text.Contains(mainAdjustAnchor))
                text = text.Replace(mainAdjustAnchor, mainAdjustPatch);

            var optimizedAnchor = $"\t\t\t\t#endif{nl}\t\t\t\tif (0.0)";
            var optimizedPatch =
                $"\t\t\t\t#endif{nl}\t\t\t\tNODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);{nl}\t\t\t\tif (0.0)";
            if (!text.Contains(BaseColorDimApplyMarker) && text.Contains(optimizedAnchor))
                text = text.Replace(optimizedAnchor, optimizedPatch);
        }

        private static void TryPatchGlobalLightingCap(ref string text, string nl)
        {
            if (text.Contains(LightingCapApplyMarker))
                return;

            var anchor =
                $"\t\t\t\t\tpoiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);{nl}\t\t\t\t}}";
            var patch =
                $"\t\t\t\t\tpoiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);{nl}\t\t\t\t}}{nl}\t\t\t\tNODE68_APPLY_GLOBAL_LIGHTING_CAP";

            if (text.Contains(anchor))
                text = text.Replace(anchor, patch);
        }
    }
}
#endif
