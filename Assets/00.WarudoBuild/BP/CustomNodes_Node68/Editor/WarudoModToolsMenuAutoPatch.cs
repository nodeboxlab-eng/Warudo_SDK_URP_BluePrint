using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// <c>app.warudo.modtool</c> 패키지의 <see cref="ModToolsMenu"/>에 Poiyomi 빌드 가드를 자동 주입합니다.
/// </summary>
[InitializeOnLoad]
static class WarudoModToolsMenuAutoPatch
{
    const string PatchMarker = "NODE68_POIYOMI_BUILD_GUARD";
    const string BridgeTypeName = "WarudoPoiyomiBuildBridge, Assembly-CSharp-Editor";

    static readonly string GuardBlock =
        @"            // NODE68_POIYOMI_BUILD_GUARD
            {
                var _poiyomiGuardType = System.Type.GetType(""" + BridgeTypeName + @""");
                if (_poiyomiGuardType != null)
                {
                    var _poiyomiGuardMethod = _poiyomiGuardType.GetMethod(
                        ""TryRunPreBuild"",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                    );
                    if (
                        _poiyomiGuardMethod != null
                        && _poiyomiGuardMethod.Invoke(null, null) is bool _poiyomiContinueBuild
                        && !_poiyomiContinueBuild
                    )
                        return;
                }
            }
";

    static WarudoModToolsMenuAutoPatch()
    {
        EditorApplication.delayCall += TryPatchAll;
    }

    static void TryPatchAll()
    {
        var packageRoot = Path.Combine(Application.dataPath, "..", "Library", "PackageCache");
        if (!Directory.Exists(packageRoot))
            return;

        foreach (
            var modToolsDir in Directory
                .EnumerateDirectories(packageRoot, "app.warudo.modtool@*", SearchOption.TopDirectoryOnly)
                .Select(d => Path.Combine(d, "Warudo Mod Tools", "Scripts", "Editor"))
                .Where(Directory.Exists)
        )
        {
            var menuPath = Path.Combine(modToolsDir, "ModToolsMenu.cs");
            if (File.Exists(menuPath))
                TryPatchModToolsMenu(menuPath);
        }
    }

    static void TryPatchModToolsMenu(string menuPath)
    {
        try
        {
            var text = File.ReadAllText(menuPath);
            if (text.Contains(PatchMarker, StringComparison.Ordinal))
                return;

            const string startBuildCall = "UMod.BuildEngine.ModToolsUtil.StartBuild(settings);";
            if (!text.Contains(startBuildCall, StringComparison.Ordinal))
            {
                Debug.LogWarning(
                    "[Poiyomi Build Guard] ModToolsMenu.cs 형식이 예상과 달라 패치하지 않았습니다: "
                        + menuPath
                );
                return;
            }

            text = text.Replace(startBuildCall, GuardBlock + startBuildCall);
            File.WriteAllText(menuPath, text);
            Debug.Log("[Poiyomi Build Guard] ModToolsMenu.cs에 빌드 전 Poiyomi 가드 패치 적용: " + menuPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("[Poiyomi Build Guard] ModToolsMenu 패치 실패: " + e.Message);
        }
    }
}
