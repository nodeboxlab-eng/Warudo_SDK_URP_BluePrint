using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Node68.ToolkitMods.Node68DevKit;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Scenes;

[AssetType(
    Id = "a1b2c3d4-5678-9abc-def0-110e5c3b0001",
    Title = "Blueprint Exporter",
    Category = BpToolkitFlavorEmbedded.ShareBuild
        ? BpToolkitCategories.Share
        : BpToolkitCategories.Toolkit
)]
public class BlueprintExporterAsset : Asset
{
    [DataInput]
    [Label("블루프린트 선택")]
    [Description("내보낼 블루프린트(Graph)를 선택하세요.")]
    [AutoComplete(nameof(AutoCompleteTargetGraph))]
    public string TargetGraphId = "";

    [DataInput]
    [Label("중첩 에셋 포함")]
    [Description("에셋이 참조하는 다른 에셋도 재귀적으로 수집합니다.")]
    public bool IncludeNestedAssets = true;

    [DataInput]
    [Label("폴더 이름")]
    [Description("Export 결과가 저장될 폴더 이름입니다. (StreamingAssets/BlueprintExport/ 하위)")]
    public string ExportFolderName = "MyBlueprint";

    [Markdown]
    public string _scanResultDisplay = "*스캔 버튼을 눌러 블루프린트를 분석하세요.*";

    [Trigger]
    [Label("스캔")]
    public void RunScan()
    {
        _lastScanResult = null;

        var graph = FindSelectedGraph();
        if (graph == null)
        {
            UpdateScanDisplay("**오류**: 블루프린트를 먼저 선택하세요.");
            return;
        }

        try
        {
            _lastScanResult = BlueprintScanner.Scan(graph, IncludeNestedAssets);
            UpdateScanDisplay(FormatScanResult(_lastScanResult));
        }
        catch (Exception e)
        {
            UpdateScanDisplay("**스캔 실패**: " + e.Message);
            Debug.LogError("[Blueprint Exporter] Scan error: " + e);
        }
    }

    [Markdown]
    [HiddenIf(nameof(HideExportSection))]
    public string _exportReadyMessage = "";

    [Trigger]
    [HiddenIf(nameof(HideExportSection))]
    [Label("내보내기 실행")]
    public void RunExport()
    {
        if (_lastScanResult == null)
        {
            UpdateScanDisplay("**오류**: 먼저 스캔을 실행하세요.");
            return;
        }

        if (string.IsNullOrWhiteSpace(ExportFolderName))
        {
            UpdateScanDisplay("**오류**: 폴더 이름을 입력하세요.");
            return;
        }

        try
        {
            var exportResult = ExportPackager.Export(_lastScanResult, ExportFolderName.Trim());

            if (exportResult.Success)
            {
                var sizeStr = ExportPackager.FormatFileSize(exportResult.TotalFileSize);
                var msgLines = new List<string>();
                msgLines.Add("**내보내기 완료!**");
                msgLines.Add("");
                msgLines.Add("- 에셋: " + exportResult.AssetCount + "개");
                msgLines.Add("- 파일: " + exportResult.FileCount + "개 (" + sizeStr + ")");

                foreach (var kvp in exportResult.FilesPerCategory)
                {
                    var folderName = BlueprintScanner.GetCategoryFolderName(kvp.Key);
                    msgLines.Add("  - " + folderName + ": " + kvp.Value + "개");
                }

                var extPluginCount = _lastScanResult.RequiredPlugins.Count(p =>
                    !ExportPackager.BuiltInPluginIds.Contains(p.Id)
                );
                msgLines.Add(
                    "  - "
                        + ExportPackager.PluginFolderName
                        + ": required_plugins.json (외부 플러그인 "
                        + extPluginCount
                        + "개)"
                );
                msgLines.Add("  - 7 Blueprint: 그래프 + 에셋 JSON");
                msgLines.Add("- 폴더: `" + exportResult.ExportFolderPath + "`");

                var msg = string.Join("\n", msgLines);
                UpdateExportDisplay(msg);
                UpdateScanDisplay(FormatScanResult(_lastScanResult) + "\n\n---\n" + msg);

                Application.OpenURL("file:///" + exportResult.ExportFolderPath.Replace("\\", "/"));
            }
            else
            {
                UpdateExportDisplay("**내보내기 실패**: " + exportResult.ErrorMessage);
            }
        }
        catch (Exception e)
        {
            UpdateExportDisplay("**내보내기 실패**: " + e.Message);
            Debug.LogError("[Blueprint Exporter] Export error: " + e);
        }
    }

    private BlueprintScanner.ScanResult _lastScanResult;

    public bool HideExportSection() => _lastScanResult == null;

    protected override void OnCreate()
    {
        base.OnCreate();
        SetActive(true);
        Watch(nameof(TargetGraphId), OnGraphChanged);
    }

    private void OnGraphChanged()
    {
        _lastScanResult = null;
        UpdateScanDisplay("*스캔 버튼을 눌러 블루프린트를 분석하세요.*");
        BroadcastDataInput(nameof(_exportReadyMessage));
    }

    public async UniTask<AutoCompleteList> AutoCompleteTargetGraph()
    {
        await UniTask.CompletedTask;
        var scene = Context.OpenedScene;
        if (scene == null)
            return AutoCompleteList.Message("씬이 열려있지 않습니다");

        var graphs = scene.GetGraphs();
        if (graphs == null || graphs.Count == 0)
            return AutoCompleteList.Message("블루프린트가 없습니다");

        var entries = graphs
            .Values.Select(g => new AutoCompleteEntry
            {
                label = g.Name + "  (노드 " + g.GetNodes().Count + "개)",
                value = g.Id.ToString(),
            })
            .ToList();

        return new AutoCompleteList
        {
            categories = new List<AutoCompleteCategory>
            {
                new AutoCompleteCategory { title = "블루프린트", entries = entries },
            },
        };
    }

    private Graph FindSelectedGraph()
    {
        if (string.IsNullOrEmpty(TargetGraphId))
            return null;

        if (!Guid.TryParse(TargetGraphId, out var graphGuid))
            return null;

        return Context.OpenedScene?.GetGraph(graphGuid);
    }

    private string FormatScanResult(BlueprintScanner.ScanResult scan)
    {
        var lines = new List<string>();
        lines.Add("### 스캔 결과: " + scan.TargetGraph.Name);
        lines.Add("");

        lines.Add("**에셋 " + scan.CollectedAssets.Count + "개 발견:**");
        if (scan.CollectedAssets.Count > 0)
        {
            foreach (var asset in scan.CollectedAssets)
            {
                lines.Add("- " + asset.Name + " (`" + asset.Serialize().typeId + "`)");
            }
        }
        else
        {
            lines.Add("- (없음)");
        }

        lines.Add("");

        var categories = new[]
        {
            BlueprintScanner.FileCategory.Animation,
            BlueprintScanner.FileCategory.Particle,
            BlueprintScanner.FileCategory.Sound,
            BlueprintScanner.FileCategory.Character,
            BlueprintScanner.FileCategory.Props,
            BlueprintScanner.FileCategory.Assets,
        };

        lines.Add("**파일 " + scan.CollectedFiles.Count + "개 발견:**");
        lines.Add("");

        foreach (var cat in categories)
        {
            var filesInCat = scan.CollectedFiles.Where(f => f.Category == cat).ToList();
            if (filesInCat.Count == 0)
                continue;

            var folderName = BlueprintScanner.GetCategoryFolderName(cat);
            lines.Add("📁 **" + folderName + "** (" + filesInCat.Count + "개)");
            foreach (var file in filesInCat)
            {
                var fileName = GetDisplayFileName(file.RelativePath);
                lines.Add("  - `" + fileName + "` ← " + file.SourceAssetName);
            }
            lines.Add("");
        }

        if (scan.CollectedFiles.Count == 0)
        {
            lines.Add("- (없음)");
        }

        lines.Add(
            "📁 **"
                + ExportPackager.PluginFolderName
                + "** (내보내기 시 `required_plugins.json` — 외부 플러그인 메타)"
        );
        lines.Add("📁 **7 Blueprint** (그래프 + 에셋 JSON)");
        lines.Add("");

        var externalPlugins = scan
            .RequiredPlugins.Where(p => !ExportPackager.BuiltInPluginIds.Contains(p.Id))
            .ToList();

        if (externalPlugins.Count > 0)
        {
            lines.Add("**필요 플러그인:**");
            lines.Add("");
            lines.Add("| 이름 | 제작자 | 버전 | Plugin ID |");
            lines.Add("|---|---|---|---|");
            foreach (var p in externalPlugins)
            {
                var name = string.IsNullOrEmpty(p.Name) ? "-" : p.Name;
                var author = string.IsNullOrEmpty(p.Author) ? "-" : p.Author;
                var version = string.IsNullOrEmpty(p.Version) ? "-" : p.Version;
                if (!string.IsNullOrEmpty(p.SupportUrl))
                    name = "[" + name + "](" + p.SupportUrl + ")";
                lines.Add("| " + name + " | " + author + " | " + version + " | `" + p.Id + "` |");
            }
            lines.Add("");
        }

        if (scan.Warnings.Count > 0)
        {
            lines.Add("");
            lines.Add("**경고 " + scan.Warnings.Count + "개:**");
            var warningsToShow = scan.Warnings.Count > 10 ? scan.Warnings.Take(10) : scan.Warnings;
            foreach (var w in warningsToShow)
            {
                lines.Add("- " + w);
            }
            if (scan.Warnings.Count > 10)
                lines.Add("- ... 외 " + (scan.Warnings.Count - 10) + "개");
        }

        return string.Join("\n", lines);
    }

    private static string GetDisplayFileName(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return "unknown";
        var lastSlash = relativePath.LastIndexOf('/');
        if (lastSlash >= 0 && lastSlash < relativePath.Length - 1)
            return relativePath.Substring(lastSlash + 1);
        return relativePath;
    }

    private void UpdateScanDisplay(string text)
    {
        _scanResultDisplay = text;
        BroadcastDataInput(nameof(_scanResultDisplay));
    }

    private void UpdateExportDisplay(string text)
    {
        _exportReadyMessage = text;
        BroadcastDataInput(nameof(_exportReadyMessage));
    }
}
