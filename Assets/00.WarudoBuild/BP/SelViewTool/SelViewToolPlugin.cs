using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

[PluginType(
    Id = "com.selviewtool.warudo",
    Name = "SelViewTool",
    Description = "방셀 촬영용 단축키 및 에셋 도구입니다.",
    Version = "1.0.4",
    Author = "Developer",
    SupportUrl = "https://docs.warudo.app",
    AssetTypes = new[] { typeof(SelViewToolAsset) },
    NodeTypes = new System.Type[0]
)]
public class SelViewToolPlugin : Plugin
{
    protected override void OnCreate()
    {
        base.OnCreate();
        Debug.Log("[SelView Tool] 플러그인 로드 완료");
    }
}
