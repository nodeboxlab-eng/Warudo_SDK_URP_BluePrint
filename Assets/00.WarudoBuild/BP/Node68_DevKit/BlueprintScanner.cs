using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Graphs;
using Warudo.Core.Plugins;
using Warudo.Core.Scenes;
using Warudo.Core.Serializations;

public class BlueprintScanner
{
    public enum FileCategory
    {
        Animation,
        Particle,
        Sound,
        Props,
        Assets,
        Character,
    }

    public class FileReference
    {
        public string RelativePath;
        public string SourceAssetName;
        public string SourcePortKey;
        public string UriScheme;
        public FileCategory Category;
    }

    public class PluginInfo
    {
        public string Id;
        public string Name;
        public string Description;
        public string Version;
        public string Author;
        public string Icon;
        public string SupportUrl;
    }

    public class ScanResult
    {
        public Graph TargetGraph;
        public List<Asset> CollectedAssets = new List<Asset>();
        public List<FileReference> CollectedFiles = new List<FileReference>();
        public List<string> Warnings = new List<string>();
        public HashSet<string> RequiredPluginIds = new HashSet<string>();
        public List<PluginInfo> RequiredPlugins = new List<PluginInfo>();
    }

    private const string DataUriMarker = "://data/";
    private const string ResourceUriMarker = "://resources/";

    public static ScanResult Scan(Graph graph, bool includeNestedAssets, int maxDepth = 8)
    {
        var result = new ScanResult { TargetGraph = graph };
        var visitedAssetIds = new HashSet<Guid>();
        var visitedFilePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var assetsToProcess = CollectAssetsFromGraph(graph, result);

        if (includeNestedAssets)
        {
            var depth = 0;
            while (assetsToProcess.Count > 0 && depth < maxDepth)
            {
                var nextBatch = new List<Asset>();
                foreach (var asset in assetsToProcess)
                {
                    if (!visitedAssetIds.Add(asset.Id))
                        continue;
                    result.CollectedAssets.Add(asset);
                    TrackPlugin(asset, result);
                    var nested = CollectAssetsFromEntity(asset, result);
                    nextBatch.AddRange(nested);
                }
                assetsToProcess = nextBatch;
                depth++;
            }
        }
        else
        {
            foreach (var asset in assetsToProcess)
            {
                if (!visitedAssetIds.Add(asset.Id))
                    continue;
                result.CollectedAssets.Add(asset);
                TrackPlugin(asset, result);
            }
        }

        foreach (var asset in result.CollectedAssets)
        {
            CollectFilesFromAsset(asset, result, visitedFilePaths);
        }

        CollectFilesFromNodes(graph, result, visitedFilePaths);

        CollectFilesFromGraphJson(graph, result, visitedFilePaths);

        Debug.Log(
            "[Blueprint Exporter] 스캔 완료 - 에셋 "
                + result.CollectedAssets.Count
                + "개, 파일 "
                + result.CollectedFiles.Count
                + "개, 경고 "
                + result.Warnings.Count
                + "개"
        );

        return result;
    }

    public static FileCategory CategorizeByScheme(string scheme, string relativePath)
    {
        if (!string.IsNullOrEmpty(scheme))
        {
            var s = scheme.ToLowerInvariant();
            if (s.Contains("animation"))
                return FileCategory.Animation;
            if (s.Contains("particle"))
                return FileCategory.Particle;
            if (s == "sound" || s == "audio")
                return FileCategory.Sound;
            if (s == "character")
                return FileCategory.Character;
            if (s == "prop" || s == "accessory")
                return FileCategory.Props;
        }

        if (!string.IsNullOrEmpty(relativePath))
        {
            var lower = relativePath.ToLowerInvariant();
            if (lower.Contains("animation") || lower.EndsWith(".anim"))
                return FileCategory.Animation;
            if (lower.Contains("particle") || lower.Contains("vfx") || lower.Contains("effect"))
                return FileCategory.Particle;
            if (
                lower.EndsWith(".wav")
                || lower.EndsWith(".mp3")
                || lower.EndsWith(".ogg")
                || lower.Contains("sound")
                || lower.Contains("audio")
            )
                return FileCategory.Sound;
            if (
                lower.EndsWith(".vrm")
                || lower.EndsWith(".glb")
                || lower.EndsWith(".gltf")
                || lower.Contains("character")
                || lower.Contains("prop")
            )
                return FileCategory.Props;
        }

        return FileCategory.Assets;
    }

    public static string GetCategoryFolderName(FileCategory category)
    {
        switch (category)
        {
            case FileCategory.Animation:
                return "1 Animation";
            case FileCategory.Particle:
                return "2 Particle";
            case FileCategory.Sound:
                return "3 Sound";
            case FileCategory.Character:
                return "4 Character";
            case FileCategory.Props:
                return "5 Props";
            case FileCategory.Assets:
                return "6 Assets";
            default:
                return "6 Assets";
        }
    }

    private static void TrackPlugin(Asset asset, ScanResult result)
    {
        try
        {
            var plugin = asset.Plugin;
            AddPluginToResult(plugin, result);
        }
        catch { }
    }

    private static void TrackNodePlugin(Node node, ScanResult result)
    {
        try
        {
            var plugin = node.Plugin;
            AddPluginToResult(plugin, result);
        }
        catch { }
    }

    private static void AddPluginToResult(Plugin plugin, ScanResult result)
    {
        if (plugin != null && result.RequiredPluginIds.Add(plugin.PluginId))
        {
            var meta = plugin.GetTypeMeta();
            var pt = meta.PluginType;
            result.RequiredPlugins.Add(
                new PluginInfo
                {
                    Id = pt.id ?? plugin.PluginId,
                    Name = pt.name ?? "",
                    Description = pt.description ?? "",
                    Version = pt.version ?? "",
                    Author = pt.author ?? "",
                    Icon = pt.icon ?? "",
                    SupportUrl = pt.supportUrl ?? "",
                }
            );
        }
    }

    private static List<Asset> CollectAssetsFromGraph(Graph graph, ScanResult result)
    {
        var found = new List<Asset>();
        var seen = new HashSet<Guid>();
        var nodes = graph.GetNodes();

        foreach (var (nodeId, node) in nodes)
        {
            try
            {
                foreach (var (portKey, port) in node.DataInputPortCollection.GetPorts())
                {
                    try
                    {
                        var kind = port.Type.GetKind();
                        if (kind == TypeKind.Asset)
                        {
                            var value = port.Getter();
                            if (value is Asset asset && seen.Add(asset.Id))
                                found.Add(asset);
                        }
                        else if (kind == TypeKind.AssetArray)
                        {
                            var value = port.Getter();
                            if (value is Array arr)
                            {
                                foreach (var item in arr)
                                {
                                    if (item is Asset asset && seen.Add(asset.Id))
                                        found.Add(asset);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.Warnings.Add(
                            "Node '" + node.Name + "' port '" + portKey + "': " + e.Message
                        );
                    }
                }
            }
            catch (Exception e)
            {
                result.Warnings.Add("Node '" + node.Name + "': " + e.Message);
            }
        }

        return found;
    }

    private static List<Asset> CollectAssetsFromEntity(Asset parentAsset, ScanResult result)
    {
        var found = new List<Asset>();

        try
        {
            foreach (var (portKey, port) in parentAsset.DataInputPortCollection.GetPorts())
            {
                try
                {
                    var kind = port.Type.GetKind();
                    if (kind == TypeKind.Asset)
                    {
                        var value = port.Getter();
                        if (value is Asset asset)
                            found.Add(asset);
                    }
                    else if (kind == TypeKind.AssetArray)
                    {
                        var value = port.Getter();
                        if (value is Array arr)
                        {
                            foreach (var item in arr)
                            {
                                if (item is Asset asset)
                                    found.Add(asset);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Warnings.Add(
                        "Asset '" + parentAsset.Name + "' port '" + portKey + "': " + e.Message
                    );
                }
            }
        }
        catch (Exception e)
        {
            result.Warnings.Add("Asset '" + parentAsset.Name + "': " + e.Message);
        }

        return found;
    }

    private static void CollectFilesFromNodes(
        Graph graph,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        var nodes = graph.GetNodes();
        Debug.Log("[Blueprint Exporter] 노드 스캔 시작 - 총 " + nodes.Count + "개 노드");

        foreach (var (nodeId, node) in nodes)
        {
            try
            {
                TrackNodePlugin(node, result);

                var serialized = node.Serialize();
                if (serialized.dataInputs == null)
                {
                    Debug.Log("[Blueprint Exporter]   Node '" + node.Name + "': dataInputs null");
                    continue;
                }

                var sourceName = "Node:" + (node.Name ?? "Unknown");
                Debug.Log(
                    "[Blueprint Exporter]   Node '"
                        + node.Name
                        + "': "
                        + serialized.dataInputs.Count
                        + "개 포트 스캔"
                );

                WalkDataInputs(serialized.dataInputs, sourceName, result, visitedFilePaths);
            }
            catch (Exception e)
            {
                result.Warnings.Add("NodeScan '" + node.Name + "': " + e.Message);
                Debug.LogWarning("[Blueprint Exporter]   NodeScan 예외 '" + node.Name + "': " + e);
            }
        }
    }

    private static void CollectFilesFromGraphJson(
        Graph graph,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        try
        {
            var serializedGraph = graph.Serialize();
            if (serializedGraph.nodes == null)
                return;

            var graphJson = JsonConvert.SerializeObject(serializedGraph);
            Debug.Log(
                "[Blueprint Exporter] GraphJSON 브루트포스 스캔 시작 (길이: "
                    + graphJson.Length
                    + ")"
            );

            var searchPos = 0;
            var foundCount = 0;

            while (true)
            {
                var idx = graphJson.IndexOf(DataUriMarker, searchPos, StringComparison.Ordinal);
                if (idx < 0)
                    break;

                var schemeStart = idx - 1;
                while (
                    schemeStart >= 0
                    && graphJson[schemeStart] != '"'
                    && graphJson[schemeStart] != ' '
                    && graphJson[schemeStart] != '{'
                    && graphJson[schemeStart] != ','
                )
                    schemeStart--;
                schemeStart++;

                var scheme = graphJson.Substring(schemeStart, idx - schemeStart);

                var pathStart = idx + DataUriMarker.Length;
                var pathEnd = pathStart;
                while (pathEnd < graphJson.Length)
                {
                    var c = graphJson[pathEnd];
                    if (c == '"' || c == '\\')
                        break;
                    pathEnd++;
                }

                if (pathEnd <= pathStart)
                {
                    searchPos = pathEnd + 1;
                    continue;
                }

                var relativePath = graphJson.Substring(pathStart, pathEnd - pathStart);
                searchPos = pathEnd + 1;

                if (string.IsNullOrEmpty(relativePath))
                    continue;

                Debug.Log(
                    "[Blueprint Exporter]   GraphJSON 발견: scheme='"
                        + scheme
                        + "' path='"
                        + relativePath
                        + "'"
                );

                if (!visitedFilePaths.Add(relativePath))
                {
                    Debug.Log("[Blueprint Exporter]   -> 이미 수집됨 (중복)");
                    continue;
                }

                if (Context.PersistentDataManager.HasFile(relativePath))
                {
                    result.CollectedFiles.Add(
                        new FileReference
                        {
                            RelativePath = relativePath,
                            SourceAssetName = "Graph:" + graph.Name,
                            SourcePortKey = scheme,
                            UriScheme = scheme,
                            Category = CategorizeByScheme(scheme, relativePath),
                        }
                    );
                    foundCount++;
                    Debug.Log("[Blueprint Exporter]   -> 추가됨!");
                }
                else
                {
                    Debug.LogWarning("[Blueprint Exporter]   -> HasFile=false: " + relativePath);
                    result.Warnings.Add("HasFile 실패: " + relativePath);
                }
            }

            Debug.Log(
                "[Blueprint Exporter] GraphJSON 스캔 완료 - " + foundCount + "개 파일 추가 발견"
            );
        }
        catch (Exception e)
        {
            result.Warnings.Add("GraphJSON 스캔: " + e.Message);
            Debug.LogWarning("[Blueprint Exporter] GraphJSON 스캔 예외: " + e);
        }
    }

    private static void CollectFilesFromAsset(
        Asset asset,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        try
        {
            var serialized = asset.Serialize();
            if (serialized.dataInputs == null)
                return;

            WalkDataInputs(serialized.dataInputs, asset.Name, result, visitedFilePaths);
        }
        catch (Exception e)
        {
            result.Warnings.Add("Serialize '" + asset.Name + "': " + e.Message);
        }
    }

    private static void WalkDataInputs(
        Dictionary<string, SerializedDataInputPort> dataInputs,
        string sourceName,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        foreach (var (portKey, port) in dataInputs)
        {
            if (port.value == null)
                continue;

            try
            {
                switch (port.typeKind)
                {
                    case TypeKind.Value:
                        if (IsStringType(port.type))
                            TryAddFileFromString(
                                port.value,
                                sourceName,
                                portKey,
                                result,
                                visitedFilePaths
                            );
                        break;

                    case TypeKind.ValueArray:
                        if (IsStringArrayType(port.type))
                            TryAddFilesFromStringArray(
                                port.value,
                                sourceName,
                                portKey,
                                result,
                                visitedFilePaths
                            );
                        break;

                    case TypeKind.StructuredData:
                        WalkStructuredData(port.value, sourceName, result, visitedFilePaths);
                        break;

                    case TypeKind.StructuredDataArray:
                        WalkStructuredDataArray(port.value, sourceName, result, visitedFilePaths);
                        break;
                }

                if (port.value != null && port.value.Contains(DataUriMarker))
                {
                    Debug.Log(
                        "[Blueprint Exporter]     포트 '"
                            + portKey
                            + "' (typeKind="
                            + port.typeKind
                            + ", type="
                            + port.type
                            + ") 에 URI 포함"
                    );
                }
            }
            catch (Exception e)
            {
                result.Warnings.Add("Walk '" + sourceName + "'.'" + portKey + "': " + e.Message);
            }
        }
    }

    private static void WalkStructuredData(
        string serializedValue,
        string sourceName,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        var sd = JsonConvert.DeserializeObject<SerializedStructuredData>(serializedValue);
        if (sd?.dataInputs != null)
            WalkDataInputs(sd.dataInputs, sourceName, result, visitedFilePaths);
    }

    private static void WalkStructuredDataArray(
        string serializedValue,
        string sourceName,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        var sdArray = JsonConvert.DeserializeObject<SerializedStructuredData[]>(serializedValue);
        if (sdArray == null)
            return;
        foreach (var sd in sdArray)
        {
            if (sd?.dataInputs != null)
                WalkDataInputs(sd.dataInputs, sourceName, result, visitedFilePaths);
        }
    }

    private static void TryAddFileFromString(
        string jsonValue,
        string sourceName,
        string portKey,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        string str;
        try
        {
            str = JsonConvert.DeserializeObject<string>(jsonValue);
        }
        catch
        {
            return;
        }

        if (string.IsNullOrEmpty(str))
            return;

        string scheme;
        var relativePath = ExtractDataRelativePath(str, out scheme);
        if (relativePath == null)
            return;
        if (!visitedFilePaths.Add(relativePath))
            return;

        Debug.Log(
            "[Blueprint Exporter]   파일 후보: '" + relativePath + "' (scheme=" + scheme + ")"
        );

        if (Context.PersistentDataManager.HasFile(relativePath))
        {
            result.CollectedFiles.Add(
                new FileReference
                {
                    RelativePath = relativePath,
                    SourceAssetName = sourceName,
                    SourcePortKey = portKey,
                    UriScheme = scheme,
                    Category = CategorizeByScheme(scheme, relativePath),
                }
            );
            Debug.Log("[Blueprint Exporter]   -> 추가됨!");
        }
        else
        {
            Debug.LogWarning("[Blueprint Exporter]   -> HasFile=false: " + relativePath);
            result.Warnings.Add("HasFile 실패: " + relativePath);
        }
    }

    private static void TryAddFilesFromStringArray(
        string jsonValue,
        string sourceName,
        string portKey,
        ScanResult result,
        HashSet<string> visitedFilePaths
    )
    {
        string[] arr;
        try
        {
            arr = JsonConvert.DeserializeObject<string[]>(jsonValue);
        }
        catch
        {
            return;
        }

        if (arr == null)
            return;

        foreach (var str in arr)
        {
            if (string.IsNullOrEmpty(str))
                continue;

            string scheme;
            var relativePath = ExtractDataRelativePath(str, out scheme);
            if (relativePath == null)
                continue;
            if (!visitedFilePaths.Add(relativePath))
                continue;

            if (Context.PersistentDataManager.HasFile(relativePath))
            {
                result.CollectedFiles.Add(
                    new FileReference
                    {
                        RelativePath = relativePath,
                        SourceAssetName = sourceName,
                        SourcePortKey = portKey,
                        UriScheme = scheme,
                        Category = CategorizeByScheme(scheme, relativePath),
                    }
                );
            }
        }
    }

    private static string ExtractDataRelativePath(string str, out string scheme)
    {
        scheme = null;

        if (str.Contains(ResourceUriMarker))
            return null;

        var dataIdx = str.IndexOf(DataUriMarker, StringComparison.Ordinal);
        if (dataIdx >= 0)
        {
            scheme = str.Substring(0, dataIdx);
            return str.Substring(dataIdx + DataUriMarker.Length);
        }

        if (!str.Contains("://") && (str.Contains("/") || str.Contains("\\")))
        {
            var normalized = str.Replace('\\', '/');
            if (Context.PersistentDataManager.HasFile(normalized))
                return normalized;
        }

        return null;
    }

    private static bool IsStringType(string typeName)
    {
        return typeName == "string" || typeName == "System.String" || typeName == "String";
    }

    private static bool IsStringArrayType(string typeName)
    {
        return typeName == "string[]" || typeName == "System.String[]" || typeName == "String[]";
    }
}
