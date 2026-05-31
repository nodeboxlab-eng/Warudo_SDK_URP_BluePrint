using System;
using System.Collections.Generic;
using System.IO;
using UMod.BuildEngine;
using UMod.ModTools.Export;
using UMod.Shared;
using UnityEditor;
using UnityEngine;

namespace Node68.CustomNodes.Editor
{
    /// <summary>
    /// 도메인 리로드(스크립트 재컴파일) 뒤에도 UMod 빌드를 한 번 실행하도록 SessionState로 예약합니다.
    /// </summary>
    [InitializeOnLoad]
    internal static class Node68PendingUmodBuild
    {
        private const string SessionKey = "Node68.PendingUmodBuild";

        static Node68PendingUmodBuild()
        {
            EditorApplication.delayCall += TryRunPendingBuild;
        }

        internal static void ScheduleAfterFlavorWrite()
        {
            SessionState.SetBool(SessionKey, true);
            AssetDatabase.Refresh();
            EditorApplication.delayCall += TryRunPendingBuild;
        }

        private static void TryRunPendingBuild()
        {
            if (!SessionState.GetBool(SessionKey, false))
                return;

            if (EditorApplication.isCompiling)
            {
                EditorApplication.delayCall += TryRunPendingBuild;
                return;
            }

            SessionState.EraseBool(SessionKey);

            var settings = ModScriptableAsset<ExportSettings>.Active.Load();
            if (settings == null)
            {
                Debug.LogWarning("[Node68] Pending UMod build skipped: ExportSettings missing.");
                return;
            }

            Node68CsprojSync.SyncSolutionBeforeBuild();

            if (!PoiyomiModBuildGuard.RunPreBuild(out _))
            {
                Debug.LogWarning("[Node68] UMod build cancelled by Poiyomi Build Guard.");
                return;
            }

            Debug.Log("[Node68] Starting UMod build (after flavor / script refresh).");
            ModToolsUtil.StartBuild(settings);
        }
    }

    /// <summary>
    /// UMod 빌드는 <c>Assembly-CSharp.csproj</c> 를 읽어 컴파일 소스 목록을 만듭니다.
    /// 파일을 외부(파일 시스템·Git·IDE 등)에서 추가·이동·삭제하면 <c>.csproj</c> 가 동기화되지 않아
    /// "source file ... not in .csproj ... will not be compiled" 경고와 함께 CS0246 빌드 실패가 납니다.
    /// 빌드 직전에 <c>UnityEditor.SyncVS.SyncSolution()</c> 을 호출해 강제로 동기화합니다.
    /// </summary>
    internal static class Node68CsprojSync
    {
        private const string CustomNodesModRoot =
            "Assets/00.WarudoBuild/BP/CustomNodes_Node68";

        private const string DevKitModRoot =
            "Assets/00.WarudoBuild/BP/Node68_DevKit";

        private const string CustomAssetsModRoot =
            "Assets/00.WarudoBuild/BP/CustomAssets_Node68";

        internal static void SyncSolutionBeforeBuild()
        {
            try
            {
                AssetDatabase.Refresh();

                var syncVs = Type.GetType("UnityEditor.SyncVS,UnityEditor");
                var sync = syncVs?.GetMethod(
                    "SyncSolution",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                );
                if (sync != null)
                {
                    sync.Invoke(null, null);
                    Debug.Log("[Node68] Assembly-CSharp.csproj 동기화 완료 (SyncVS.SyncSolution).");
                }
                else
                {
                    Debug.LogWarning(
                        "[Node68] UnityEditor.SyncVS.SyncSolution 메서드를 찾지 못했습니다. "
                            + "Edit > Preferences > External Tools > Regenerate project files 를 수동 실행하세요."
                    );
                }

                EnsureModSourcesInCsproj(
                    CustomNodesModRoot,
                    "Assets\\00.WarudoBuild\\BP\\CustomNodes_Node68\\Node68CustomNodesPlugin.cs"
                );
                EnsureModSourcesInCsproj(
                    DevKitModRoot,
                    "Assets\\00.WarudoBuild\\BP\\Node68_DevKit\\Node68DevKitPlugin.cs"
                );
                EnsureModSourcesInCsproj(
                    CustomAssetsModRoot,
                    "Assets\\00.WarudoBuild\\BP\\CustomAssets_Node68\\CustomAssetsNode68Plugin.cs"
                );
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Node68] .csproj 동기화 실패(무시 가능): {e.Message}");
            }
        }

        /// <summary>
        /// SyncSolution 직후에도 Cursor/Git 등으로 추가된 .cs 가 csproj 에 빠질 수 있어
        /// 모드 루트·Build/*.cs 를 강제로 Compile Include 합니다.
        /// </summary>
        internal static void EnsureCustomNodesModSourcesInCsproj()
        {
            EnsureModSourcesInCsproj(
                CustomNodesModRoot,
                "Assets\\00.WarudoBuild\\BP\\CustomNodes_Node68\\Node68CustomNodesPlugin.cs"
            );
        }

        internal static void EnsureDevKitModSourcesInCsproj()
        {
            EnsureModSourcesInCsproj(
                DevKitModRoot,
                "Assets\\00.WarudoBuild\\BP\\Node68_DevKit\\Node68DevKitPlugin.cs"
            );
        }

        internal static void EnsureCustomAssetsModSourcesInCsproj()
        {
            EnsureModSourcesInCsproj(
                CustomAssetsModRoot,
                "Assets\\00.WarudoBuild\\BP\\CustomAssets_Node68\\CustomAssetsNode68Plugin.cs"
            );
        }

        private static void EnsureModSourcesInCsproj(string modRoot, string pluginAnchorIncludePath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            if (string.IsNullOrEmpty(projectRoot))
                return;

            var csprojPath = Path.Combine(projectRoot, "Assembly-CSharp.csproj");
            if (!File.Exists(csprojPath))
            {
                Debug.LogWarning("[Node68] Assembly-CSharp.csproj 를 찾지 못했습니다.");
                return;
            }

            var modRootFull = Path.Combine(projectRoot, modRoot.Replace('/', Path.DirectorySeparatorChar));
            if (!Directory.Exists(modRootFull))
                return;

            var sourcePaths = new List<string>();
            CollectModCsFiles(modRootFull, modRootFull, modRoot, sourcePaths);

            var buildDir = Path.Combine(modRootFull, "Build");
            if (Directory.Exists(buildDir))
                CollectModCsFiles(buildDir, modRootFull, modRoot, sourcePaths);

            sourcePaths.Sort(StringComparer.OrdinalIgnoreCase);

            var csprojText = File.ReadAllText(csprojPath);
            var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(
                         csprojText,
                         "<Compile Include=\"([^\"]+)\""
                     ))
            {
                existing.Add(match.Groups[1].Value.Replace('/', '\\'));
            }

            var pluginIncludePath = pluginAnchorIncludePath.Replace('/', '\\');
            var pluginLine = $"    <Compile Include=\"{pluginIncludePath}\" />";
            var pluginMissing = !existing.Contains(pluginIncludePath);

            var insertLines = new List<string>();
            if (pluginMissing)
                insertLines.Add(pluginLine);

            foreach (var relativeUnix in sourcePaths)
            {
                var includePath = relativeUnix.Replace('/', '\\');
                if (existing.Contains(includePath))
                    continue;

                insertLines.Add($"    <Compile Include=\"{includePath}\" />");
                existing.Add(includePath);
            }

            if (insertLines.Count == 0)
                return;

            var anchorIndex = csprojText.IndexOf(pluginLine, StringComparison.Ordinal);
            if (anchorIndex < 0)
            {
                anchorIndex = csprojText.IndexOf(
                    "<Compile Include=\"Assets\\00.WarudoBuild\\BP\\Node68_DevKit\\Node68DevKitPlugin.cs\" />",
                    StringComparison.Ordinal
                );
            }

            if (anchorIndex < 0)
                anchorIndex = csprojText.IndexOf("<Compile Include=", StringComparison.Ordinal);

            if (anchorIndex < 0)
            {
                Debug.LogWarning(
                    $"[Node68] csproj 에 Compile Include 앵커를 찾지 못해 {modRoot} 소스를 수동 확인하세요."
                );
                return;
            }

            var patched =
                csprojText.Insert(anchorIndex, string.Join(Environment.NewLine, insertLines) + Environment.NewLine);
            File.WriteAllText(csprojPath, patched);
            Debug.Log($"[Node68] Assembly-CSharp.csproj 에 {modRoot} 소스 {insertLines.Count}개 추가.");
        }

        private static void CollectModCsFiles(
            string scanDir,
            string modRootFull,
            string modRootAssetPath,
            List<string> dest
        )
        {
            foreach (var file in Directory.GetFiles(scanDir, "*.cs", SearchOption.TopDirectoryOnly))
            {
                var relative = modRootAssetPath
                    + "/"
                    + Path.GetRelativePath(modRootFull, file).Replace('\\', '/');
                dest.Add(relative);
            }
        }
    }

    /// <summary>
    /// Warudo 빌드 전 dev/share 를 컴파일 타임 상수로 기록한 뒤 UMod 빌드를 예약합니다.
    /// </summary>
    internal static class Node68ModBuildFlavorMenu
    {
        private const string ShareBuildScriptingDefine = "NODE68_SHARE_BUILD";

        private const string FlavorRelativePath =
            "Assets/00.WarudoBuild/BP/CustomNodes_Node68/Resources/Node68BuildFlavor.txt";

        private const string EmbeddedRelativePath =
            "Assets/00.WarudoBuild/BP/CustomNodes_Node68/Build/Node68FlavorEmbedded.cs";

        private const string CustomAssetsEmbeddedRelativePath =
            "Assets/00.WarudoBuild/BP/CustomAssets_Node68/Build/CustomAssetsFlavorEmbedded.cs";

        private static readonly (
            string relativePath,
            string csharpNamespace
        )[] CustomAssetsNode68CategoriesTargets =
        {
            (
                "Assets/00.WarudoBuild/BP/CustomAssets_Node68/Build/CustomAssetsNode68Categories.cs",
                "Node68.CustomAssets"
            ),
        };

        private static readonly (
            string relativePath,
            string csharpNamespace
        )[] BpToolkitBuildConstantsTargets =
        {
            (
                "Assets/00.WarudoBuild/BP/PoseThumbnailKit/Build/BpToolkitBuildConstants.cs",
                "Node68.ToolkitMods.PoseThumbnailKit"
            ),
            (
                "Assets/00.WarudoBuild/BP/SelViewTool/Build/BpToolkitBuildConstants.cs",
                "Node68.ToolkitMods.SelViewTool"
            ),
            (
                "Assets/00.WarudoBuild/BP/Node68_DevKit/Build/BpToolkitBuildConstants.cs",
                "Node68.ToolkitMods.Node68DevKit"
            ),
        };

        [MenuItem("Warudo/Build Mod (Node68: 개발·쉐어 선택)…", true, 43)]
        private static bool BuildWithFlavorChoiceValidate()
        {
            return true;
        }

        [MenuItem("Warudo/Build Mod (Node68: 개발·쉐어 선택)…", false, 43)]
        private static void BuildWithFlavorChoice()
        {
            var choice = EditorUtility.DisplayDialogComplex(
                "Node68 빌드 모드",
                "빌드에 포함될 모드를 선택하세요.\n\n"
                    + "· 개발: 본·스케일 등 인스펙터 편집 가능. Node68_DevKit 노드는 개발/쉐어 빌드 모두 동일하게 전체 필드 사용.\n"
                    + "· 쉐어: 배포용 — CustomNodes_Node68 등에서 편집 필드 숨김·이름 접미사·굳힌 값 적용(DevKit 모듈은 예외—숨김 없음).\n"
                    + "· 카테고리: 개발 🚀 / 쉐어 📁Node68 Share (PoseThumbnail·SelView 등). Node68_DevKit(블루프린트 내보내기 포함)은 ⚙️Node68 DevKit 고정.\n\n"
                    + "Flavor 가 바뀌면 스크립트가 재컴파일된 뒤 자동으로 Mod 빌드가 시작됩니다.",
                "개발 (dev)",
                "취소",
                "쉐어 (share)"
            );

            if (choice == 1)
                return;

            if (ModScriptableAsset<ExportSettings>.Active.Load() == null)
            {
                EditorUtility.DisplayDialog(
                    "Build Mod",
                    "Export settings를 찾을 수 없습니다.",
                    "OK"
                );
                return;
            }

            var share = choice != 0;
            SyncShareBuildScriptingDefine(share);
            WriteFlavorTxt(share ? "share" : "dev");
            WriteFlavorEmbeddedCs(share);
            WriteCustomAssetsFlavorEmbeddedCs(share);
            WriteCustomAssetsNode68Categories(share);
            WriteBpToolkitModBuildConstants(share);
            Node68PendingUmodBuild.ScheduleAfterFlavorWrite();
        }

        /// <summary>
        /// 쉐어 빌드: <c>NODE68_SHARE_BUILD</c> 정의 → Poiyomi 조명 제어(캐릭터·프롭) 등 개발 전용 타입 제외.
        /// 개발 빌드: 정의 제거.
        /// </summary>
        private static void SyncShareBuildScriptingDefine(bool share)
        {
            const string define = ShareBuildScriptingDefine;

            foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (group == BuildTargetGroup.Unknown)
                    continue;

                try
                {
                    var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                    var list = new List<string>();
                    var changed = false;
                    var hasDefine = false;
                    foreach (
                        var part in symbols.Split(
                            new[] { ';' },
                            StringSplitOptions.RemoveEmptyEntries
                        )
                    )
                    {
                        var t = part.Trim();
                        if (t.Length == 0)
                            continue;
                        if (string.Equals(t, define, StringComparison.Ordinal))
                        {
                            hasDefine = true;
                            if (!share)
                                changed = true;
                            continue;
                        }

                        list.Add(t);
                    }

                    if (share && !hasDefine)
                    {
                        list.Add(define);
                        changed = true;
                    }

                    if (changed)
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(
                            group,
                            string.Join(";", list)
                        );
                }
                catch
                {
                    // unsupported BuildTargetGroup
                }
            }
        }

        private static void WriteFlavorTxt(string flavor)
        {
            var abs = Path.Combine(
                Application.dataPath,
                "..",
                FlavorRelativePath.Replace('/', Path.DirectorySeparatorChar)
            );
            abs = Path.GetFullPath(abs);
            Directory.CreateDirectory(Path.GetDirectoryName(abs)!);
            File.WriteAllText(abs, flavor + "\n");
            AssetDatabase.ImportAsset(FlavorRelativePath, ImportAssetOptions.ForceUpdate);
        }

        private static void WriteFlavorEmbeddedCs(bool share)
        {
            var cs =
                "// <auto-generated by Node68 build menu — Warudo/Build Mod (Node68: 개발·쉐어 선택)…>\n"
                + "namespace Node68.CustomNodes\n"
                + "{\n"
                + "    internal static class Node68FlavorEmbedded\n"
                + "    {\n"
                + "        internal const bool ShareBuild = "
                + (share ? "true" : "false")
                + ";\n"
                + "    }\n"
                + "}\n";

            var abs = Path.Combine(
                Application.dataPath,
                "..",
                EmbeddedRelativePath.Replace('/', Path.DirectorySeparatorChar)
            );
            abs = Path.GetFullPath(abs);
            File.WriteAllText(abs, cs);
            AssetDatabase.ImportAsset(EmbeddedRelativePath, ImportAssetOptions.ForceUpdate);
        }

        private static void WriteCustomAssetsFlavorEmbeddedCs(bool share)
        {
            var cs =
                "// <auto-generated by Warudo/Build Mod (Node68: 개발·쉐어 선택)…>\n"
                + "namespace Node68.CustomAssets\n"
                + "{\n"
                + "    internal static class CustomAssetsFlavorEmbedded\n"
                + "    {\n"
                + "        internal const bool ShareBuild = "
                + (share ? "true" : "false")
                + ";\n"
                + "    }\n"
                + "}\n";

            var abs = Path.Combine(
                Application.dataPath,
                "..",
                CustomAssetsEmbeddedRelativePath.Replace('/', Path.DirectorySeparatorChar)
            );
            abs = Path.GetFullPath(abs);
            File.WriteAllText(abs, cs);
            AssetDatabase.ImportAsset(
                CustomAssetsEmbeddedRelativePath,
                ImportAssetOptions.ForceUpdate
            );
        }

        private static void WriteCustomAssetsNode68Categories(bool share)
        {
            _ = share;
            foreach (var target in CustomAssetsNode68CategoriesTargets)
            {
                var cs =
                    "// <auto-generated by Node68 build menu — Warudo/Build Mod (Node68: 개발·쉐어 선택)…>\n"
                    + $"namespace {target.csharpNamespace}\n"
                    + "{\n"
                    + "    /// <summary>Warudo 에셋·노드 패널 — CustomAssets_Node68 단일 카테고리.</summary>\n"
                    + "    internal static class CustomAssetsNode68Categories\n"
                    + "    {\n"
                    + "        internal const string DevRoot = \"🚀CustomAssets_Node68\";\n"
                    + "        internal const string ShareRoot = \"📁CustomAssets_Node68\";\n\n"
                    + "        internal const string LightControlDev = DevRoot;\n"
                    + "        internal const string LightControlShare = ShareRoot;\n\n"
                    + "        internal const string TextDisplayDev = DevRoot;\n"
                    + "        internal const string TextDisplayShare = ShareRoot;\n\n"
                    + "        internal const string UiRemoteDev = DevRoot;\n"
                    + "        internal const string UiRemoteShare = ShareRoot;\n"
                    + "    }\n"
                    + "}\n";

                var abs = Path.Combine(
                    Application.dataPath,
                    "..",
                    target.relativePath.Replace('/', Path.DirectorySeparatorChar)
                );
                abs = Path.GetFullPath(abs);
                var dir = Path.GetDirectoryName(abs);
                if (dir != null)
                    Directory.CreateDirectory(dir);

                File.WriteAllText(abs, cs);
                AssetDatabase.ImportAsset(target.relativePath, ImportAssetOptions.ForceUpdate);
            }
        }

        private static void WriteBpToolkitModBuildConstants(bool share)
        {
            foreach (var target in BpToolkitBuildConstantsTargets)
            {
                WriteOneBpToolkitBuildConstantsFile(
                    target.relativePath,
                    target.csharpNamespace,
                    share
                );
            }
        }

        private static void WriteOneBpToolkitBuildConstantsFile(
            string relativeUnityPath,
            string csharpNamespace,
            bool share
        )
        {
            var toolkitCat = "🚀Node68 Toolkit";
            var shareCat = "📁Node68 Share";
            const string warudoSectionLabel = "Warudo";
            if (
                string.Equals(
                    csharpNamespace,
                    "Node68.ToolkitMods.Node68DevKit",
                    StringComparison.Ordinal
                )
            )
            {
                toolkitCat = "⚙️Node68 DevKit";
                shareCat = "⚙️Node68 DevKit";
            }

            var assetCategoryBlock =
                string.Equals(
                    csharpNamespace,
                    "Node68.ToolkitMods.Node68DevKit",
                    StringComparison.Ordinal
                )
                    ? (
                        "\n"
                        + "    internal static class BpToolkitUiLabels\n"
                        + "    {\n"
                        + "        internal const string WarudoSection = \""
                        + warudoSectionLabel
                        + "\";\n"
                        + "    }\n"
                    )
                    : "";

            var cs =
                "// <auto-generated by Node68 build menu — Warudo/Build Mod (Node68: 개발·쉐어 선택)…>\n"
                + $"namespace {csharpNamespace}\n"
                + "{\n"
                + "    internal static class BpToolkitFlavorEmbedded\n"
                + "    {\n"
                + "        internal const bool ShareBuild = "
                + (share ? "true" : "false")
                + ";\n"
                + "    }\n\n"
                + "    internal static class BpToolkitCategories\n"
                + "    {\n"
                + "        internal const string Toolkit = \""
                + toolkitCat
                + "\";\n"
                + "        internal const string Share = \""
                + shareCat
                + "\";\n"
                + "    }\n"
                + assetCategoryBlock
                + "}\n";

            var abs = Path.Combine(
                Application.dataPath,
                "..",
                relativeUnityPath.Replace('/', Path.DirectorySeparatorChar)
            );
            abs = Path.GetFullPath(abs);
            var dir = Path.GetDirectoryName(abs);
            if (dir != null)
                Directory.CreateDirectory(dir);

            File.WriteAllText(abs, cs);
            AssetDatabase.ImportAsset(relativeUnityPath, ImportAssetOptions.ForceUpdate);
        }
    }
}
