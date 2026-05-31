using Warudo.Core.Attributes;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>Blueprint Init Profile — 초기화 규칙 종류.</summary>
    public enum BlueprintInitRuleKind68
    {
        [Label("GameObject · 활성/비활성")]
        GameObjectEnabled = 0,

        [Label("GameObject · 트랜스폼 (위치·회전·스케일)")]
        GameObjectTransform = 1,

        [Label("Prop · 블렌드쉐이프")]
        PropBlendShape = 2,

        [Label("TextDisplay · 텍스트·표시 리셋")]
        TextDisplayReset = 3,

        [Label("카메라 · Pre* 변수 복구")]
        CameraRestoreFromVariables = 4,

        [Label("캐릭터 · LookAt / FOV")]
        CharacterLookAtAndFov = 5,

        [Label("Graph Variable · 기본값")]
        GraphVariable = 6,

        [Label("기타 (수동 안내)")]
        ManualNote = 7,
    }
}
