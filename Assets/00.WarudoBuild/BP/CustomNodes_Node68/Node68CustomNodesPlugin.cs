// Node68 모드 — PluginType 등록부.
// 노드·에셋·헬퍼는 모드 폴더 「루트(평면)」에 두어야 UMod 빌드가 컴파일에 포함합니다.
// TextDisplay·Light Setting·UI 리모컨 → CustomAssets_Node68.

using System;
using Node68.CustomNodes;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

[PluginType(
    Id = "com.node68.custom-nodes.warudo",
    Name = "CustomNodes_Node68",
    Description = "커스텀 블루프린트 노드 모음.",
    Version = "1.9.90",
    Author = "Node68 Toolkit",
    SupportUrl = "https://docs.warudo.app",
    NodeTypes = new[]
    {
        typeof(SetCharacterMainTransformPivotOffsetNode),
        typeof(SetHumanoidBoneScaleBatchNode),
        typeof(SkinnedMeshBoneScaleNode68),
        typeof(CharacterBlendShapeWeightNode68),
        typeof(GameObjectTransformEasingNode68),
        typeof(StateFieldsNode68),
        typeof(CameraStatePresetNode68),
        typeof(CharacterStatePresetFieldsNode68),
        typeof(SaveAssetStateNode68),
        typeof(RestoreAssetStateNode68),
        typeof(FilterAssetStateNode68),
        typeof(SaveMainCameraOrbitNode68),
        typeof(SetFrontCameraOrbitNode68),
        typeof(DonationQueueNode68),
        typeof(DonationQueueIntervalTableNode68),
        typeof(SoundPlayNode68),
        // typeof(ThrowPropNode68), // Held/ — NODE68_INCLUDE_THROW_PROP 시 복구
    }
)]
public sealed class Node68CustomNodesPlugin : Plugin
{
    private static readonly Type[] UModCompileAnchors =
    {
        typeof(SetCharacterMainTransformPivotOffsetNode),
        typeof(SetHumanoidBoneScaleBatchNode),
        typeof(SkinnedMeshBoneScaleNode68),
        typeof(CharacterBlendShapeWeightNode68),
        typeof(GameObjectTransformEasingNode68),
        typeof(BoneTextAttachMode),
        typeof(CharacterPivotTransformTarget),
        typeof(GameObjectTransformEasingMode),
        typeof(GameObjectOscillatorShape),
        typeof(GameObjectEasingSpace),
        typeof(GameObjectPunchChannel),
        typeof(Node68GraphVariableHelper),
        typeof(Node68CameraOrbitNodeMigration),
        typeof(Node68CameraOrbitHelper),
        typeof(Node68CameraOrbitCharacterBridge),
        typeof(StateFieldsNode68),
        typeof(CameraStatePresetNode68),
        typeof(CharacterStatePresetFieldsNode68),
        typeof(SaveAssetStateNode68),
        typeof(RestoreAssetStateNode68),
        typeof(FilterAssetStateNode68),
        typeof(SaveMainCameraOrbitNode68),
        typeof(SetFrontCameraOrbitNode68),
        typeof(DonationQueueNode68),
        typeof(DonationQueueIntervalTableNode68),
        typeof(SoundPlayNode68),
    };

    protected override void OnCreate()
    {
        base.OnCreate();
        Debug.Log(
            $"[Node68] Custom Nodes 플러그인 로드 완료 · 컴파일 앵커 참조: {UModCompileAnchors.Length}"
        );
    }
}
