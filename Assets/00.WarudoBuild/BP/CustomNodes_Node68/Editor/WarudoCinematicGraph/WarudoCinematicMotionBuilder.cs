using System.Collections.Generic;
using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// 선택 프리셋 → 정규화된 키프레임 시퀀스. AutoShake 등 후처리는 여기에서만 적용합니다.
    /// </summary>
    public static class WarudoCinematicMotionBuilder
    {
        public static IReadOnlyList<WarudoCinematicTransformKeyframe> Build(
            IWarudoCameraMotionPreset preset,
            WarudoCinematicMotionContext ctx
        )
        {
            var list = new List<WarudoCinematicTransformKeyframe>();
            foreach (var k in preset.Build(ctx))
            {
                if (k == null)
                    continue;
                list.Add(k.Clone());
            }

            if (ctx.AutoShake)
                ApplyMicroShake(list, ctx);

            return list;
        }

        private static void ApplyMicroShake(IList<WarudoCinematicTransformKeyframe> list, WarudoCinematicMotionContext ctx)
        {
            if (list.Count <= 1)
                return;

            var rng = new System.Random(ctx.ShakeSeed);
            var a = 0.028f * Mathf.Max(0.05f, ctx.Intensity);

            for (var i = 1; i < list.Count; i++)
            {
                var k = list[i];
                k.Position += RandomUnit(rng) * a;
                k.Rotation += RandomUnit(rng) * (a * 10f);
            }
        }

        private static Vector3 RandomUnit(System.Random rng)
        {
            return new Vector3(
                (float)(rng.NextDouble() * 2 - 1),
                (float)(rng.NextDouble() * 2 - 1),
                (float)(rng.NextDouble() * 2 - 1)
            );
        }
    }
}
