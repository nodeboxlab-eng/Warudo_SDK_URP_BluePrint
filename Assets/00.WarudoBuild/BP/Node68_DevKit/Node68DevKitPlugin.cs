using System;
using Node68.ToolkitMods.LightingControl;
using Node68.ToolkitMods.Node68DevKit;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

[PluginType(
    Id = "com.node68.devkit.warudo",
    Name = "Node68_DevKit",
    Description =
        "블루프린트 내보내기, Poiyomi 조명·Light Control, Poiyomi Volume 등 ⚙️Node68 DevKit 개발 보조 기능을 제공합니다.",
    Version = "1.1.4",
    Author = "Node68 Toolkit",
    SupportUrl = "https://docs.warudo.app",
    AssetTypes = new[]
    {
        typeof(BlueprintInitProfileAsset),
        typeof(BlueprintExporterAsset),
        typeof(Node68PoiyomiGlobalVolumeAsset),
        typeof(MaterialHueShiftAsset),
        typeof(MaterialHueShiftPropAsset),
#if !NODE68_SHARE_BUILD
        typeof(PoiyomiLightingControlAsset),
        typeof(PoiyomiLightingControlPropAsset),
#endif
    },
    NodeTypes = new[]
    {
        typeof(BlendShapeViewerNode68),
        typeof(CharacterMaterialShaderProbeNode68),
        typeof(ToggleAllCharacterMeshesNode68),
        typeof(ExtractMaterialTextureNode68),
        typeof(CreateGraphVariablesNode68),
        typeof(MaterialHueShiftNode68),
        typeof(ConsoleLogNode68),
#if !NODE68_SHARE_BUILD
        typeof(LilToonBacklightNode68),
        typeof(PlayCameraPathNode68),
#endif
    }
)]
public sealed class Node68DevKitPlugin : Plugin
{
    private static readonly Type[] UModCompileAnchors =
    {
        typeof(BlueprintInitProfileAsset),
        typeof(BlueprintInitRuleKind68),
        typeof(BlueprintInitGraphScanner),
        typeof(BlueprintInitChecklistFormatter),
        typeof(BlendShapeViewerNode68),
        typeof(CharacterMaterialShaderProbeNode68),
        typeof(ToggleAllCharacterMeshesNode68),
        typeof(ExtractMaterialTextureNode68),
        typeof(CreateGraphVariablesNode68),
        typeof(CreateGraphVariablesDevKitHelper),
        typeof(MaterialHueShiftAsset),
        typeof(MaterialHueShiftPropAsset),
        typeof(MaterialHueShiftNode68),
        typeof(ConsoleLogNode68),
        typeof(ConsoleLogLevel68),
        typeof(ConsoleLogJsonMode68),
#if !NODE68_SHARE_BUILD
        typeof(LilToonBacklightNode68),
        typeof(PlayCameraPathNode68),
#endif
    };

    protected override void OnCreate()
    {
        base.OnCreate();
        var typeNames = string.Join(", ", Array.ConvertAll(UModCompileAnchors, t => t.Name));
        Debug.Log(
            $"[Node68 DevKit] 플러그인 로드 완료 · 컴파일 앵커 {UModCompileAnchors.Length}개 → {typeNames}"
        );
    }
}
