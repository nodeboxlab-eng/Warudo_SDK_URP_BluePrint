using Warudo.Core.Graphs;

namespace Node68.CustomNodes
{
    /// <summary>
    /// 쉐어 빌드 UI 베이스. 필드는 표시하고 Description 툴팁만 숨깁니다.
    /// <see cref="HideInShareDevFields"/> 는 레거시 호환용이며 새 노드에서 HiddenIf에 쓰지 않습니다.
    /// </summary>
    public abstract class Node68ShareEditableNode : Node
    {
        protected bool HideInShareDevFields() => Node68BuildRuntime.IsShareBuild();

        /// <summary><see cref="HideInShareDevFields"/> 별칭 (기존 노드 호환).</summary>
        protected bool HideInShareBuild() => HideInShareDevFields();
    }
}
