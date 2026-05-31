using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Scenes;

public class ExportPackager
{
    public static readonly HashSet<string> BuiltInPluginIds = new HashSet<string>
    {
        "Warudo.Core",
        "Warudo.iFacialMocap",
        "Warudo.Interactions",
    };

    [Serializable]
    public class Manifest
    {
        public string pluginVersion = "1.0.0";
        public string exportDate;
        public string graphName;
        public string graphId;
        public List<AssetEntry> assets = new List<AssetEntry>();
        public List<FileEntry> files = new List<FileEntry>();
        public List<string> requiredPlugins = new List<string>();
        public List<string> warnings = new List<string>();
    }

    [Serializable]
    public class AssetEntry
    {
        public string id;
        public string typeId;
        public string name;
    }

    [Serializable]
    public class FileEntry
    {
        public string category;
        public string relativePath;
        public string exportPath;
    }

    public class ExportResult
    {
        public bool Success;
        public string ExportFolderPath;
        public string ErrorMessage;
        public int AssetCount;
        public int FileCount;
        public long TotalFileSize;
        public Dictionary<BlueprintScanner.FileCategory, int> FilesPerCategory =
            new Dictionary<BlueprintScanner.FileCategory, int>();
    }

    private const string ExportBaseDir = "BlueprintExport";

    /// <summary>항상 생성되는 플러그인 메타 폴더 (번호 0).</summary>
    public const string PluginFolderName = "0 Plugin";

    public static ExportResult Export(
        BlueprintScanner.ScanResult scanResult,
        string exportFolderName
    )
    {
        var result = new ExportResult();

        try
        {
            var exportPrefix = ExportBaseDir + "/" + exportFolderName;

            WritePluginFolder(scanResult, exportPrefix);
            var fileEntries = CopyFilesByCategory(scanResult, exportPrefix, result);
            ExportBlueprint(scanResult, exportPrefix);
            WriteManifest(scanResult, exportPrefix, fileEntries);
            WriteReadme(scanResult, exportPrefix);

            result.Success = true;
            result.ExportFolderPath = Context.PersistentDataManager.GetFullPath(exportPrefix);
            result.AssetCount = scanResult.CollectedAssets.Count;
            result.FileCount = scanResult.CollectedFiles.Count;

            Debug.Log("[Blueprint Exporter] Export 완료: " + result.ExportFolderPath);
        }
        catch (Exception e)
        {
            result.Success = false;
            result.ErrorMessage = e.Message;
            Debug.LogError("[Blueprint Exporter] Export 실패: " + e);
        }

        return result;
    }

    /// <summary>
    /// 외부(비내장) 플러그인 목록을 JSON으로 남겨 <see cref="PluginFolderName"/> 폴더를 항상 만든다.
    /// </summary>
    private static void WritePluginFolder(
        BlueprintScanner.ScanResult scanResult,
        string exportPrefix
    )
    {
        var pluginDir = exportPrefix + "/" + PluginFolderName;
        var externalPlugins = scanResult
            .RequiredPlugins.Where(p => !BuiltInPluginIds.Contains(p.Id))
            .ToList();

        var arr = new JArray();
        foreach (var p in externalPlugins)
        {
            try
            {
                var pluginObj = JObject.FromObject(p);
                pluginObj.Remove("Icon");
                arr.Add(pluginObj);
            }
            catch (Exception e)
            {
                arr.Add(
                    new JObject
                    {
                        ["id"] = p.Id,
                        ["name"] = p.Name,
                        ["_serializeError"] = e.Message,
                    }
                );
            }
        }

        var doc = new JObject
        {
            ["exportDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["graphName"] = scanResult.TargetGraph.Name,
            ["graphId"] = scanResult.TargetGraph.Id.ToString(),
            ["requiredPlugins"] = arr,
        };

        var json = doc.ToString(Formatting.Indented);
        Context.PersistentDataManager.WriteFile(pluginDir + "/required_plugins.json", json);
    }

    private static void ExportBlueprint(BlueprintScanner.ScanResult scanResult, string exportPrefix)
    {
        var blueprintDir = exportPrefix + "/7 Blueprint";

        var serializedGraph = scanResult.TargetGraph.Serialize();
        var graphJson = JsonConvert.SerializeObject(serializedGraph, Formatting.Indented);
        var graphFileName = SanitizeFileName(scanResult.TargetGraph.Name);
        Context.PersistentDataManager.WriteFile(
            blueprintDir + "/" + graphFileName + ".json",
            graphJson
        );

        foreach (var asset in scanResult.CollectedAssets)
        {
            try
            {
                var serialized = asset.Serialize();
                var json = JsonConvert.SerializeObject(serialized, Formatting.Indented);
                var safeName = SanitizeFileName(asset.Name);
                var fileName = safeName + ".json";

                var folder = IsCharacterAsset(serialized.typeId) ? "/4 Character/" : "/6 Assets/";
                Context.PersistentDataManager.WriteFile(exportPrefix + folder + fileName, json);
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    "[Blueprint Exporter] Asset '" + asset.Name + "' 직렬화 실패: " + e.Message
                );
            }
        }
    }

    private static List<FileEntry> CopyFilesByCategory(
        BlueprintScanner.ScanResult scanResult,
        string exportPrefix,
        ExportResult result
    )
    {
        var fileEntries = new List<FileEntry>();
        long totalSize = 0;

        foreach (var file in scanResult.CollectedFiles)
        {
            try
            {
                var categoryFolder = BlueprintScanner.GetCategoryFolderName(file.Category);
                var exportPath =
                    exportPrefix + "/" + categoryFolder + "/" + GetFileName(file.RelativePath);

                var bytes = Context.PersistentDataManager.ReadFileBytes(file.RelativePath);
                Context.PersistentDataManager.WriteFileBytes(exportPath, bytes);
                totalSize += bytes.Length;

                if (!result.FilesPerCategory.ContainsKey(file.Category))
                    result.FilesPerCategory[file.Category] = 0;
                result.FilesPerCategory[file.Category]++;

                fileEntries.Add(
                    new FileEntry
                    {
                        category = categoryFolder,
                        relativePath = file.RelativePath,
                        exportPath = categoryFolder + "/" + GetFileName(file.RelativePath),
                    }
                );
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    "[Blueprint Exporter] 파일 복사 실패 '" + file.RelativePath + "': " + e.Message
                );
            }
        }

        result.TotalFileSize = totalSize;
        return fileEntries;
    }

    private static void WriteManifest(
        BlueprintScanner.ScanResult scanResult,
        string exportPrefix,
        List<FileEntry> fileEntries
    )
    {
        var manifest = new Manifest
        {
            exportDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            graphName = scanResult.TargetGraph.Name,
            graphId = scanResult.TargetGraph.Id.ToString(),
            assets = scanResult
                .CollectedAssets.Select(a =>
                {
                    var serialized = a.Serialize();
                    return new AssetEntry
                    {
                        id = a.Id.ToString(),
                        typeId = serialized.typeId,
                        name = a.Name,
                    };
                })
                .ToList(),
            files = fileEntries,
            requiredPlugins = scanResult.RequiredPluginIds.ToList(),
            warnings = scanResult.Warnings,
        };

        var json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
        Context.PersistentDataManager.WriteFile(exportPrefix + "/manifest.json", json);
    }

    private static void WriteReadme(BlueprintScanner.ScanResult scanResult, string exportPrefix)
    {
        var lines = new List<string>();
        lines.Add("=== " + scanResult.TargetGraph.Name + " ===");
        lines.Add("");
        lines.Add("Exported: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        lines.Add("");

        var externalPlugins = scanResult
            .RequiredPlugins.Where(p => !BuiltInPluginIds.Contains(p.Id))
            .ToList();

        if (externalPlugins.Count > 0)
        {
            lines.Add("[ 필요 플러그인 ]");
            lines.Add("이 블루프린트를 사용하려면 아래 플러그인을 먼저 설치해주세요.");
            lines.Add("각 플러그인의 원본 메타데이터(JSON)를 그대로 표시합니다.");
            lines.Add("");

            for (int i = 0; i < externalPlugins.Count; i++)
            {
                var p = externalPlugins[i];
                var name = string.IsNullOrEmpty(p.Name) ? p.Id : p.Name;
                lines.Add("  " + (i + 1) + ". " + name);
                lines.Add("     플러그인 ID: " + p.Id);
                lines.Add("     --- BEGIN PLUGIN DATA (JSON, Icon 제외) ---");
                try
                {
                    var pluginObj = JObject.FromObject(p);
                    pluginObj.Remove("Icon");
                    var pluginJson = pluginObj.ToString(Formatting.Indented);
                    var jsonLines = pluginJson.Split('\n');
                    foreach (var jsonLine in jsonLines)
                    {
                        lines.Add("       " + jsonLine.TrimEnd('\r'));
                    }
                }
                catch (Exception e)
                {
                    lines.Add("     [JSON 직렬화 실패] " + e.Message);
                }
                lines.Add("     --- END PLUGIN DATA ---");
                lines.Add("");
            }
        }
        else
        {
            lines.Add("[ 추가 플러그인 불필요 ]");
            lines.Add("");
        }

        var orderedAssets = GetAssetImportOrder(scanResult);

        lines.Add("[ 에셋 목록 (Import 순서) ]");
        lines.Add("아래 순서대로 Warudo 씬에 에셋을 추가해주세요.");
        lines.Add("(다른 에셋이 참조하는 에셋을 먼저 추가해야 합니다)");
        lines.Add("");
        for (int i = 0; i < orderedAssets.Count; i++)
        {
            var asset = orderedAssets[i];
            var serialized = asset.Serialize();
            lines.Add("  " + (i + 1) + ". " + asset.Name + " (" + serialized.typeId + ")");
        }
        lines.Add("");

        var categories = new[]
        {
            BlueprintScanner.FileCategory.Animation,
            BlueprintScanner.FileCategory.Particle,
            BlueprintScanner.FileCategory.Sound,
            BlueprintScanner.FileCategory.Character,
            BlueprintScanner.FileCategory.Props,
        };

        var hasFiles = false;
        foreach (var cat in categories)
        {
            var filesInCat = scanResult.CollectedFiles.Where(f => f.Category == cat).ToList();
            if (filesInCat.Count == 0)
                continue;

            if (!hasFiles)
            {
                lines.Add("[ 포함된 파일 ]");
                lines.Add("");
                hasFiles = true;
            }

            var folderName = BlueprintScanner.GetCategoryFolderName(cat);
            lines.Add("  " + folderName + " (" + filesInCat.Count + "개)");
            foreach (var file in filesInCat)
            {
                var fileName = GetFileName(file.RelativePath);
                lines.Add("    - " + fileName);
            }
            lines.Add("");
        }

        if (!hasFiles)
        {
            lines.Add("[ 포함된 파일 ]");
            lines.Add("");
            lines.Add("  포함된 파일이 없습니다.");
            lines.Add("");
        }

        var basePath = @"Steam\steamapps\common\Warudo\Warudo_Data\StreamingAssets";

        lines.Add("[ Import 순서 및 경로 안내 ]");
        lines.Add("아래 순서대로 해당 경로에 파일을 넣어주세요.");
        lines.Add("");
        lines.Add(
            "  0 Plugin     -> Warudo에서 먼저 설치 (이 패키지의 "
                + PluginFolderName
                + @"\required_plugins.json 참고)"
        );
        lines.Add("  1 Animation  -> " + basePath + @"\CharacterAnimations");
        lines.Add("  2 Particle   -> " + basePath + @"\Particles");
        lines.Add("  3 Sound      -> " + basePath + @"\Sounds");
        lines.Add("  4 Character  -> " + basePath + @"\Characters");
        lines.Add("  5 Props      -> " + basePath + @"\Props");
        lines.Add("  6 Assets     -> Warudo 씬에서 직접 설정");
        lines.Add("  7 Blueprint  -> Warudo 씬에서 블루프린트 Import");
        lines.Add("");

        var text = string.Join("\n", lines);
        Context.PersistentDataManager.WriteFile(exportPrefix + "/README.txt", text);
    }

    private static List<Asset> GetAssetImportOrder(BlueprintScanner.ScanResult scanResult)
    {
        var assets = scanResult.CollectedAssets;
        var dependsOn = new Dictionary<Guid, HashSet<Guid>>();
        foreach (var a in assets)
            dependsOn[a.Id] = new HashSet<Guid>();

        foreach (var asset in assets)
        {
            try
            {
                var serialized = asset.Serialize();
                if (serialized.dataInputs == null)
                    continue;

                var json = JsonConvert.SerializeObject(serialized.dataInputs);
                foreach (var other in assets)
                {
                    if (other.Id == asset.Id)
                        continue;
                    if (json.Contains(other.Id.ToString()))
                        dependsOn[asset.Id].Add(other.Id);
                }
            }
            catch { }
        }

        Debug.Log("[Blueprint Exporter] === 에셋 의존성 분석 ===");
        foreach (var asset in assets)
        {
            var deps = dependsOn[asset.Id];
            if (deps.Count == 0)
            {
                Debug.Log(
                    "[Blueprint Exporter]   "
                        + asset.Name
                        + " → 의존성 없음 (타입 우선순위: "
                        + GetTypePriority(asset)
                        + ")"
                );
            }
            else
            {
                foreach (var depId in deps)
                {
                    var depAsset = assets.FirstOrDefault(a => a.Id == depId);
                    var depName = depAsset != null ? depAsset.Name : depId.ToString();
                    Debug.Log(
                        "[Blueprint Exporter]   " + asset.Name + " → " + depName + " 를 참조"
                    );
                }
            }
        }

        var inDegree = new Dictionary<Guid, int>();
        foreach (var a in assets)
            inDegree[a.Id] = 0;

        foreach (var a in assets)
        foreach (var depId in dependsOn[a.Id])
            if (inDegree.ContainsKey(depId))
                inDegree[a.Id]++;

        var buckets = new SortedDictionary<int, List<Asset>>();
        foreach (var a in assets)
        {
            if (inDegree[a.Id] == 0)
            {
                var pri = GetTypePriority(a);
                if (!buckets.ContainsKey(pri))
                    buckets[pri] = new List<Asset>();
                buckets[pri].Add(a);
            }
        }

        var queue = new Queue<Asset>();
        foreach (var bucket in buckets.Values)
        foreach (var a in bucket)
            queue.Enqueue(a);

        var sorted = new List<Asset>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sorted.Add(current);

            var released = new List<Asset>();
            foreach (var a in assets)
            {
                if (dependsOn[a.Id].Contains(current.Id))
                {
                    inDegree[a.Id]--;
                    if (inDegree[a.Id] == 0)
                        released.Add(a);
                }
            }
            released.Sort((a, b) => GetTypePriority(a).CompareTo(GetTypePriority(b)));
            foreach (var a in released)
                queue.Enqueue(a);
        }

        foreach (var a in assets)
            if (!sorted.Contains(a))
                sorted.Add(a);

        Debug.Log("[Blueprint Exporter] === Import 순서 ===");
        for (int i = 0; i < sorted.Count; i++)
            Debug.Log("[Blueprint Exporter]   " + (i + 1) + ". " + sorted[i].Name);

        return sorted;
    }

    private static int GetTypePriority(Asset asset)
    {
        try
        {
            var typeId = asset.Serialize().typeId;
            if (string.IsNullOrEmpty(typeId))
                return 99;
            var lower = typeId.ToLowerInvariant();

            if (lower.Contains("character"))
                return 0;
            if (lower.Contains("prop") || lower.Contains("anchor"))
                return 1;
            if (lower.Contains("camera"))
                return 2;
            if (lower.Contains("light") || lower.Contains("environment"))
                return 3;
        }
        catch { }
        return 99;
    }

    private static bool IsCharacterAsset(string typeId)
    {
        if (string.IsNullOrEmpty(typeId))
            return false;
        var lower = typeId.ToLowerInvariant();
        return lower.Contains("character");
    }

    private static string GetFileName(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return "unknown";
        var lastSlash = relativePath.LastIndexOf('/');
        if (lastSlash >= 0 && lastSlash < relativePath.Length - 1)
            return relativePath.Substring(lastSlash + 1);
        return relativePath;
    }

    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "unnamed";
        var chars = name.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            if (
                c == '<'
                || c == '>'
                || c == ':'
                || c == '"'
                || c == '/'
                || c == '\\'
                || c == '|'
                || c == '?'
                || c == '*'
                || c < 32
            )
            {
                chars[i] = '_';
            }
        }
        var result = new string(chars);
        if (result.Length > 60)
            result = result.Substring(0, 60);
        return result;
    }

    public static string FormatFileSize(long bytes)
    {
        if (bytes < 1024)
            return bytes + " B";
        if (bytes < 1024 * 1024)
            return (bytes / 1024.0).ToString("F1") + " KB";
        if (bytes < 1024L * 1024 * 1024)
            return (bytes / (1024.0 * 1024)).ToString("F1") + " MB";
        return (bytes / (1024.0 * 1024 * 1024)).ToString("F2") + " GB";
    }
}
