using System;
using Node68.CustomAssets;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;
using Warudo.UIControl;

[PluginType(
    Id = "com.node68.custom-assets.warudo",
    Name = "CustomAssets_Node68",
    Description = "Light Setting · TextDisplay · UI 리모컨 Node68 커스텀 에셋.",
    Version = "1.0.1",
    Author = "Node68 Toolkit",
    SupportUrl = "https://docs.warudo.app",
    AssetTypes = new[]
    {
        // typeof(PoiyomiMaxBrightnessControlAsset), // Light Setting — 캐릭터 (일단 비활성)
        typeof(PoiyomiMaxBrightnessControlPropAsset),
        // typeof(PoiyomiMaxBrightnessControlParticleAsset), // Light Setting — 파티클 (일단 비활성)
        typeof(CharacterBoneAttachedTextAsset),
        typeof(CanvasUIAsset),
    },
    NodeTypes = new[]
    {
        typeof(GetDisplayTextNode68),
        typeof(SetDisplayTextNode68),
        typeof(TextDisplayShowHideNode68),
        typeof(TextDisplayAnimateNode68),
        typeof(BoneAttachedTextSetNode68),
        typeof(RandomTextByWeightNode68),
        typeof(OnUIButtonClickNode),
        typeof(OnUIToggleClickNode),
    }
)]
public sealed class CustomAssetsNode68Plugin : Plugin
{
    private static readonly Type[] UModCompileAnchors =
    {
        typeof(PoiyomiMaxBrightnessControlPropAsset),
        typeof(CharacterBoneAttachedTextAsset),
        typeof(CanvasUIAsset),
        typeof(Node68TextDisplayFontLoadDiagnostics),
        typeof(Node68TypewriterText68),
        typeof(Node68AnimateOverlaySamples68),
        typeof(TextDisplayTransformSpace68),
        typeof(TextDisplayMotionShape),
        typeof(TextDisplayAnimatePreset68),
        typeof(Node68ShareNodeTypeTitles),
        typeof(BoneTextAttachMode),
    };

    protected override void OnCreate()
    {
        base.OnCreate();
        Debug.Log("[CustomAssets_Node68] 플러그인 로드 완료");
    }
}
