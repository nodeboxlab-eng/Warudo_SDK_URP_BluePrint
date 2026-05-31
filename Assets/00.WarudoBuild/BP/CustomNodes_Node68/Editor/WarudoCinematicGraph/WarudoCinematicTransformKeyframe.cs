using DG.Tweening;
using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// SET_ASSET_TRANSFORM 한 구간에 대응되는 트랜스폼 + 전환 파라미터.
    /// </summary>
    public sealed class WarudoCinematicTransformKeyframe
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = Vector3.one;

        /// <summary>이전 키프레임에서 이 상태로 도달하는 데 걸리는 시간(초). 첫 키는 보통 0.01 스냅.</summary>
        public float TransitionTime = 1f;

        public Ease Easing = Ease.OutSine;

        public WarudoCinematicTransformKeyframe Clone()
        {
            return new WarudoCinematicTransformKeyframe
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
                TransitionTime = TransitionTime,
                Easing = Easing
            };
        }
    }
}
