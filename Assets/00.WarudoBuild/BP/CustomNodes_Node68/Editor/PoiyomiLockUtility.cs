using System.Collections.Generic;
using System.Linq;
using Poi.Tools.ShaderTranslator.VersionUpgrade;
using Thry.ThryEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Poiyomi(Thry Shader Optimizer) 미잠금 머테리얼 판별 및 프로젝트 내 수집.
/// </summary>
public static class PoiyomiLockUtility
{
    public static bool IsUnlockedThryPoiyomi(Material m)
    {
        if (m == null || m.shader == null)
            return false;

        try
        {
            if (!PoiyomiVersionDetector.IsPoiyomiShader(m))
                return false;

            Shader eff = PoiyomiVersionDetector.GetEffectiveShader(m);
            if (eff == null || !ShaderOptimizer.IsShaderUsingThryOptimizer(eff))
                return false;

            return !ShaderOptimizer.IsMaterialLocked(m);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>열린 씬 + Assets 프리팹 Renderer가 참조하는 고유 Material.</summary>
    public static List<Material> CollectMaterialsFromProjectRenderers()
    {
        var seen = new HashSet<Material>();
        var list = new List<Material>();

        void Add(Material m)
        {
            if (m != null && seen.Add(m))
                list.Add(m);
        }

        void AddFromRenderer(Renderer r)
        {
            if (r == null)
                return;
            foreach (var m in r.sharedMaterials)
                Add(m);
        }

        foreach (var guid in AssetDatabase.FindAssets("t:Prefab"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var root = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (root == null)
                continue;
            foreach (var r in root.GetComponentsInChildren<Renderer>(true))
                AddFromRenderer(r);
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded)
                continue;
            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var r in root.GetComponentsInChildren<Renderer>(true))
                    AddFromRenderer(r);
            }
        }

        return list;
    }

    public static List<Material> FindUnlockedThryPoiyomiMaterials(IEnumerable<Material> materials)
    {
        return materials
            .Where(m => m != null && IsUnlockedThryPoiyomi(m))
            .Distinct()
            .ToList();
    }
}
