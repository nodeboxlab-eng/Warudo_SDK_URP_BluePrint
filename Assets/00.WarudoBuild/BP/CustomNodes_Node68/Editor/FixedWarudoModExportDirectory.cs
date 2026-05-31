using System;
using System.IO;
using UMod.ModTools.Export;
using UMod.Shared;
using UnityEditor;
using UnityEngine;

namespace Node68.CustomNodes.Editor
{
    /// <summary>
    /// Mod Settings(UMod)의 Export Directory를 프로젝트 전역으로 고정합니다.
    /// 비활성화하려면 <see cref="LockExportDirectory"/> 를 false 로 두세요.
    /// </summary>
    [InitializeOnLoad]
    internal static class FixedWarudoModExportDirectory
    {
        private const bool LockExportDirectory = true;

        /// <summary>Steam 기본 설치 기준 Warudo StreamingAssets Plugins 폴더.</summary>
        internal const string FixedModExportPath =
            @"C:\Program Files (x86)\Steam\steamapps\common\Warudo\Warudo_Data\StreamingAssets\Plugins";

        static FixedWarudoModExportDirectory()
        {
            if (!LockExportDirectory)
                return;

            EditorApplication.delayCall += () => TryApply(logWhenChanged: false);
        }

        private static bool SameExportPath(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(a))
                return string.IsNullOrWhiteSpace(b);

            if (string.IsNullOrWhiteSpace(b))
                return false;

            try
            {
                return string.Equals(
                    Path.GetFullPath(a.Trim()),
                    Path.GetFullPath(b.Trim()),
                    StringComparison.OrdinalIgnoreCase
                );
            }
            catch
            {
                return string.Equals(a.Trim(), b.Trim(), StringComparison.OrdinalIgnoreCase);
            }
        }

        internal static bool TryApply(bool logWhenChanged)
        {
            if (!LockExportDirectory)
                return false;

            ExportSettings settings;
            try
            {
                settings = ModScriptableAsset<ExportSettings>.Active.Load();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[FixedWarudoModExportDirectory] ExportSettings 로드 실패: " + ex.Message);
                return false;
            }

            if (settings == null)
                return false;

            var so = new SerializedObject(settings);
            var profiles = so.FindProperty("exportProfiles");
            if (profiles == null || !profiles.isArray)
                return false;

            var changed = false;
            for (var i = 0; i < profiles.arraySize; i++)
            {
                var pathProp = profiles.GetArrayElementAtIndex(i).FindPropertyRelative("modExportPath");
                if (pathProp == null)
                    continue;

                if (SameExportPath(pathProp.stringValue, FixedModExportPath))
                    continue;

                pathProp.stringValue = FixedModExportPath;
                changed = true;
            }

            if (!changed)
                return false;

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            if (logWhenChanged)
                Debug.Log("[FixedWarudoModExportDirectory] Mod Export Directory → " + FixedModExportPath);

            return true;
        }
    }

    internal sealed class FixedWarudoModExportDirectoryPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            ExportSettings settings;
            try
            {
                settings = ModScriptableAsset<ExportSettings>.Active.Load();
            }
            catch
            {
                return;
            }

            if (settings == null)
                return;

            var activePath = AssetDatabase.GetAssetPath(settings);
            foreach (var path in importedAssets)
            {
                if (path != activePath)
                    continue;

                EditorApplication.delayCall += () =>
                    FixedWarudoModExportDirectory.TryApply(logWhenChanged: false);

                break;
            }
        }
    }
}
