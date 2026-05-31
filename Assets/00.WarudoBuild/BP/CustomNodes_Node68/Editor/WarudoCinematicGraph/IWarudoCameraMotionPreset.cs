using System.Collections.Generic;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    public interface IWarudoCameraMotionPreset
    {
        string Id { get; }
        string DisplayName { get; }

        IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx);
    }
}
