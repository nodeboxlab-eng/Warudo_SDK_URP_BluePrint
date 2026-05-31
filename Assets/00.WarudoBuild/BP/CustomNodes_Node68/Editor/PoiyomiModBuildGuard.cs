using System.Collections.Generic;
using Thry.ThryEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Warudo Build Mod 직전에 Poiyomi(Thry) 미잠금 머테리얼을 검사합니다.
/// </summary>
public static class PoiyomiModBuildGuard
{
    const string PrefsKeyMode = "PoiyomiModBuildGuard.GuardMode";

    public enum GuardMode
    {
        WarnOnly = 0,
        AutoLockThenBuild = 1,
        AbortIfUnlocked = 2,
    }

    public static GuardMode Mode
    {
        get => (GuardMode)EditorPrefs.GetInt(PrefsKeyMode, (int)GuardMode.AbortIfUnlocked);
        set => EditorPrefs.SetInt(PrefsKeyMode, (int)value);
    }

    static List<Material> CollectUnlockedProjectMaterials()
    {
        EditorUtility.DisplayProgressBar("Poiyomi Lock", "Renderer 머테리얼 수집 중…", 0.33f);
        try
        {
            var all = PoiyomiLockUtility.CollectMaterialsFromProjectRenderers();
            EditorUtility.DisplayProgressBar("Poiyomi Lock", "미잠금 Poiyomi 검사 중…", 0.66f);
            return PoiyomiLockUtility.FindUnlockedThryPoiyomiMaterials(all);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    public static List<Material> ScanProjectOnly()
    {
        List<Material> unlockedPoiyomi = CollectUnlockedProjectMaterials();

        if (unlockedPoiyomi.Count == 0)
        {
            EditorUtility.DisplayDialog(
                "Poiyomi Lock",
                "검사 대상 Renderer에서 미잠금 Thry Poiyomi 머테리얼이 없습니다.",
                "OK"
            );
            return unlockedPoiyomi;
        }

        foreach (var m in unlockedPoiyomi)
            Debug.LogWarning(
                "[Poiyomi 검사] 잠금되지 않은 Thry Poiyomi — " + AssetDatabase.GetAssetPath(m),
                m
            );

        EditorUtility.DisplayDialog(
            "Poiyomi Lock",
            $"미잠금 Thry Poiyomi 머테리얼 {unlockedPoiyomi.Count}개 — 콘솔을 확인하세요.",
            "OK"
        );
        EditorGUIUtility.PingObject(unlockedPoiyomi[0]);
        return unlockedPoiyomi;
    }

    /// <summary>true면 UMod 빌드 진행 가능.</summary>
    public static bool RunPreBuild(out List<Material> unlockedPoiyomi)
    {
        unlockedPoiyomi = CollectUnlockedProjectMaterials();

        bool canBuild;
        if (unlockedPoiyomi.Count == 0)
        {
            canBuild = true;
        }
        else
        {
            switch (Mode)
            {
                case GuardMode.WarnOnly:
                    foreach (var m in unlockedPoiyomi)
                        Debug.LogWarning(
                            "[Poiyomi Build Guard] 잠금되지 않은 Thry Poiyomi 머테리얼 — "
                                + AssetDatabase.GetAssetPath(m),
                            m
                        );
                    EditorUtility.DisplayDialog(
                        "Poiyomi Lock",
                        $"잠금되지 않은 Thry Poiyomi 머테리얼이 {unlockedPoiyomi.Count}개 있습니다.\n"
                            + "콘솔 경로를 확인한 뒤 Lock 하거나, Warudo/Poiyomi 메뉴에서 전체 Lock을 사용하세요.",
                        "OK"
                    );
                    canBuild = true;
                    break;

                case GuardMode.AutoLockThenBuild:
                    Undo.SetCurrentGroupName("Poiyomi Lock (pre-build)");
                    int g = Undo.GetCurrentGroup();
                    foreach (var m in unlockedPoiyomi)
                        Undo.RecordObject(m, "Poiyomi Lock");
                    ShaderOptimizer.LockMaterials(
                        unlockedPoiyomi,
                        ShaderOptimizer.ProgressBar.Cancellable
                    );
                    Undo.CollapseUndoOperations(g);
                    Debug.Log(
                        $"[Poiyomi Build Guard] 자동 Lock 적용 — {unlockedPoiyomi.Count}개 머테리얼"
                    );
                    canBuild = true;
                    break;

                case GuardMode.AbortIfUnlocked:
                default:
                    foreach (var m in unlockedPoiyomi)
                        Debug.LogError(
                            "[Poiyomi Build Guard] 빌드 중단 — 잠금 필요: "
                                + AssetDatabase.GetAssetPath(m),
                            m
                        );
                    EditorUtility.DisplayDialog(
                        "빌드 중단",
                        $"잠금되지 않은 Thry Poiyomi 머테리얼이 {unlockedPoiyomi.Count}개 있어 빌드를 취소했습니다.\n"
                            + "(Warudo/Poiyomi/빌드 전 동작 에서 경고만·자동 잠금 등으로 변경 가능)",
                        "OK"
                    );
                    canBuild = false;
                    break;
            }
        }

        if (canBuild)
            SaveProjectBeforeBuild();

        return canBuild;
    }

    static void SaveProjectBeforeBuild()
    {
        AssetDatabase.SaveAssets();

        var savedScenes = new List<string>();
        var skippedScenes = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded || !scene.isDirty)
                continue;

            if (string.IsNullOrEmpty(scene.path))
            {
                skippedScenes.Add(scene.name);
                continue;
            }

            if (EditorSceneManager.SaveScene(scene))
                savedScenes.Add(scene.path);
            else
                Debug.LogWarning("[Poiyomi Build Guard] 씬 저장 실패: " + scene.path);
        }

        if (savedScenes.Count > 0)
            Debug.Log("[Poiyomi Build Guard] 빌드 전 씬 자동 저장 — " + string.Join(", ", savedScenes));

        foreach (var name in skippedScenes)
            Debug.LogWarning(
                "[Poiyomi Build Guard] 저장 경로 없는 씬은 자동 저장 생략(한 번 수동 저장 필요): "
                    + name
            );
    }
    [MenuItem("Warudo/Poiyomi/잠금 안 된 머테리얼 검사", false, 45)]
    public static void MenuScanOnly()
    {
        ScanProjectOnly();
    }

    [MenuItem("Warudo/Poiyomi/검사 대상 전체 Lock", false, 46)]
    public static void MenuLockAllScanned()
    {
        var unlocked = CollectUnlockedProjectMaterials();
        if (unlocked.Count == 0)
        {
            EditorUtility.DisplayDialog(
                "Poiyomi Lock",
                "잠금할 Thry Poiyomi 머테리얼이 없습니다.",
                "OK"
            );
            return;
        }

        if (
            !EditorUtility.DisplayDialog(
                "Poiyomi Lock",
                $"{unlocked.Count}개 머테리얼에 Lock을 적용합니다. 계속할까요?",
                "Lock",
                "취소"
            )
        )
            return;

        Undo.SetCurrentGroupName("Poiyomi Lock All (project renderers)");
        int g = Undo.GetCurrentGroup();
        foreach (var m in unlocked)
            Undo.RecordObject(m, "Poiyomi Lock");
        ShaderOptimizer.LockMaterials(unlocked, ShaderOptimizer.ProgressBar.Cancellable);
        Undo.CollapseUndoOperations(g);
        AssetDatabase.SaveAssets();
        Debug.Log($"[Poiyomi] 전체 Lock 완료 — {unlocked.Count}개");
    }

    const string MenuWarn = "Warudo/Poiyomi/빌드 전 동작/경고만 (콘솔·다이얼로그)";
    const string MenuAuto = "Warudo/Poiyomi/빌드 전 동작/자동 잠금 후 빌드";
    const string MenuAbort = "Warudo/Poiyomi/빌드 전 동작/미잠금 시 빌드 중단";

    [MenuItem(MenuWarn, false, 200)]
    static void SetModeWarn()
    {
        Mode = GuardMode.WarnOnly;
    }

    [MenuItem(MenuWarn, true)]
    static bool SetModeWarnValidate()
    {
        Menu.SetChecked(MenuWarn, Mode == GuardMode.WarnOnly);
        return true;
    }

    [MenuItem(MenuAuto, false, 201)]
    static void SetModeAuto()
    {
        Mode = GuardMode.AutoLockThenBuild;
    }

    [MenuItem(MenuAuto, true)]
    static bool SetModeAutoValidate()
    {
        Menu.SetChecked(MenuAuto, Mode == GuardMode.AutoLockThenBuild);
        return true;
    }

    [MenuItem(MenuAbort, false, 202)]
    static void SetModeAbort()
    {
        Mode = GuardMode.AbortIfUnlocked;
    }

    [MenuItem(MenuAbort, true)]
    static bool SetModeAbortValidate()
    {
        Menu.SetChecked(MenuAbort, Mode == GuardMode.AbortIfUnlocked);
        return true;
    }
}
