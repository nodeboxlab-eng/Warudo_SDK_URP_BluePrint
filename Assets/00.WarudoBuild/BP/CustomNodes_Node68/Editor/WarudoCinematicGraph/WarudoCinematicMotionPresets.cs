using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    internal static class WarudoCinematicMotionMath
    {
        public static Vector3 EulerLookAt(Vector3 eye, Vector3 target, Vector3 worldUp)
        {
            var dir = target - eye;
            if (dir.sqrMagnitude < 1e-10f)
                return Vector3.zero;
            return Quaternion.LookRotation(dir, worldUp).eulerAngles;
        }

        public static void StampFirstKeyframeSnap(WarudoCinematicTransformKeyframe first)
        {
            first.TransitionTime = 0.01f;
            first.Easing = Ease.Linear;
        }

        public static float EvenSegmentDuration(int keyframeCount, float totalDuration)
        {
            if (keyframeCount <= 1)
                return Mathf.Max(0.01f, totalDuration);
            return Mathf.Max(0.01f, totalDuration / (keyframeCount - 1));
        }
    }

    internal sealed class DroneOrbitPreset : IWarudoCameraMotionPreset
    {
        public string Id => "drone_orbit";
        public string DisplayName => "Drone Orbit";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var target = ctx.GetTargetWorldPosition();
            var r = Mathf.Max(0.1f, ctx.OrbitRadius * Mathf.Max(0.05f, ctx.Intensity));
            var h = ctx.HeightOffset;
            const int steps = 28;
            var seg = WarudoCinematicMotionMath.EvenSegmentDuration(steps, ctx.TotalDuration);

            var list = new List<WarudoCinematicTransformKeyframe>(steps);
            for (var i = 0; i < steps; i++)
            {
                var t = (i / (float)Mathf.Max(1, steps - 1)) * Mathf.PI * 2f;
                var p = target + new Vector3(Mathf.Cos(t) * r, h, Mathf.Sin(t) * r);
                var rot = WarudoCinematicMotionMath.EulerLookAt(p, target, Vector3.up);
                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = rot,
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : seg,
                        Easing = i == 0 ? Ease.Linear : Ease.InOutSine
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class CinematicIntroPreset : IWarudoCameraMotionPreset
    {
        public string Id => "cinematic_intro";
        public string DisplayName => "Cinematic Intro";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var r = Mathf.Max(0.1f, ctx.OrbitRadius * Mathf.Max(0.05f, ctx.Intensity));
            var h = ctx.HeightOffset;

            Vector3[] offsets =
            {
                new Vector3(0f, h * 1.35f, -r * 2.1f),
                new Vector3(-r * 1.15f, h * 0.95f, -r * 0.55f),
                new Vector3(r * 0.35f, h * 0.75f, -r * 1.05f),
                new Vector3(0f, h * 0.32f, -r * 0.28f)
            };

            var weights = new[] { 0.22f, 0.28f, 0.28f, 0.22f };
            var list = new List<WarudoCinematicTransformKeyframe>(offsets.Length);
            for (var i = 0; i < offsets.Length; i++)
            {
                var p = T + offsets[i];
                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = WarudoCinematicMotionMath.EulerLookAt(p, T, Vector3.up),
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : Mathf.Max(0.05f, ctx.TotalDuration * weights[i]),
                        Easing = i <= 1 ? Ease.Linear : Ease.OutCubic
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class HeroShotPreset : IWarudoCameraMotionPreset
    {
        public string Id => "hero_shot";
        public string DisplayName => "Hero Shot";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var r = Mathf.Max(0.1f, ctx.OrbitRadius * Mathf.Max(0.05f, ctx.Intensity));
            var h = Mathf.Max(0.05f, ctx.HeightOffset * 0.35f);

            Vector3[] offsets =
            {
                new Vector3(0f, h * 0.9f, -r * 0.95f),
                new Vector3(-r * 0.25f, h * 0.55f, -r * 0.65f),
                new Vector3(0f, h * 0.45f, -r * 0.42f)
            };

            var list = new List<WarudoCinematicTransformKeyframe>(offsets.Length);
            for (var i = 0; i < offsets.Length; i++)
            {
                var p = T + offsets[i];
                var look = WarudoCinematicMotionMath.EulerLookAt(p, T, Vector3.up);
                look.x -= 8f * Mathf.Clamp(ctx.Intensity, 0.25f, 2.5f);

                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = look,
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : Mathf.Max(0.05f, ctx.TotalDuration / (offsets.Length - 1)),
                        Easing = i == 0 ? Ease.Linear : Ease.OutQuad
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class PushInPreset : IWarudoCameraMotionPreset
    {
        public string Id => "push_in";
        public string DisplayName => "Push In";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var camFwd = ctx.SceneCamera != null ? ctx.SceneCamera.forward : new Vector3(0f, 0f, 1f);
            var dir = (-camFwd).normalized;
            if (dir.sqrMagnitude < 1e-4f)
                dir = new Vector3(0f, 0f, -1f);
            var r0 = Mathf.Max(0.25f, ctx.OrbitRadius * 2.2f * ctx.Intensity);
            var r1 = Mathf.Max(0.1f, ctx.OrbitRadius * 0.35f);
            var h = ctx.HeightOffset;

            const int steps = 10;
            var list = new List<WarudoCinematicTransformKeyframe>(steps);
            var seg = WarudoCinematicMotionMath.EvenSegmentDuration(steps, ctx.TotalDuration);

            for (var i = 0; i < steps; i++)
            {
                var a = i / (float)(steps - 1);
                var d = Mathf.Lerp(r0, r1, Mathf.SmoothStep(0f, 1f, a));
                var p = T + dir * d + Vector3.up * (h * Mathf.Lerp(1f, 0.55f, a));
                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = WarudoCinematicMotionMath.EulerLookAt(p, T, Vector3.up),
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : seg,
                        Easing = i == 0 ? Ease.Linear : Ease.InOutQuad
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class CraneUpPreset : IWarudoCameraMotionPreset
    {
        public string Id => "crane_up";
        public string DisplayName => "Crane Up";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var r = Mathf.Max(0.1f, ctx.OrbitRadius * 0.9f * ctx.Intensity);
            var list = new List<WarudoCinematicTransformKeyframe>();
            var y0 = Mathf.Max(0.05f, ctx.HeightOffset * 0.35f);
            var y1 = Mathf.Max(y0 + 0.1f, ctx.HeightOffset * 1.65f * Mathf.Max(0.35f, ctx.Intensity));

            var p0 = T + new Vector3(0f, y0, -r * 1.05f);
            var p1 = T + new Vector3(r * 0.15f, y1, -r * 0.85f);
            var p2 = T + new Vector3(-r * 0.12f, y1 * 1.05f, -r * 0.55f);

            void Add(Vector3 p, float dur, Ease e)
            {
                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = WarudoCinematicMotionMath.EulerLookAt(p, T, Vector3.up),
                        Scale = Vector3.one,
                        TransitionTime = dur,
                        Easing = e
                    }
                );
            }

            Add(p0, 0.01f, Ease.Linear);
            Add(p1, Mathf.Max(0.05f, ctx.TotalDuration * 0.45f), Ease.InOutSine);
            Add(p2, Mathf.Max(0.05f, ctx.TotalDuration * 0.55f), Ease.OutCubic);

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class OrbitAroundTargetPreset : IWarudoCameraMotionPreset
    {
        public string Id => "orbit_around_target";
        public string DisplayName => "Orbit Around Target";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var inner = new DroneOrbitPreset();
            var kfs = new List<WarudoCinematicTransformKeyframe>(inner.Build(ctx));
            for (var i = 1; i < kfs.Count; i++)
            {
                kfs[i].Easing = Ease.OutQuad;
                kfs[i].TransitionTime *= 1.08f;
            }

            return kfs;
        }
    }

    internal sealed class ZoomOutEndingPreset : IWarudoCameraMotionPreset
    {
        public string Id => "zoom_out_ending";
        public string DisplayName => "Zoom Out Ending";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var r0 = Mathf.Max(0.12f, ctx.OrbitRadius * 0.45f);
            var r1 = Mathf.Max(r0 + 0.1f, ctx.OrbitRadius * 2.4f * Mathf.Max(0.35f, ctx.Intensity));
            var h = ctx.HeightOffset;

            const int steps = 9;
            var list = new List<WarudoCinematicTransformKeyframe>(steps);
            var seg = WarudoCinematicMotionMath.EvenSegmentDuration(steps, ctx.TotalDuration);

            for (var i = 0; i < steps; i++)
            {
                var a = i / (float)(steps - 1);
                var d = Mathf.Lerp(r0, r1, Mathf.Pow(a, 0.85f));
                var p = T + new Vector3(0f, h * Mathf.Lerp(0.55f, 1f, a), -d);
                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = WarudoCinematicMotionMath.EulerLookAt(p, T, Vector3.up),
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : seg,
                        Easing = i == 0 ? Ease.Linear : Ease.InOutSine
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    internal sealed class HandheldShakePreset : IWarudoCameraMotionPreset
    {
        public string Id => "handheld_shake";
        public string DisplayName => "Handheld Shake";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var basePreset = new PushInPreset();
            var list = new List<WarudoCinematicTransformKeyframe>(basePreset.Build(ctx));
            var rng = new System.Random(ctx.ShakeSeed ^ 0x5f356495);
            var a = 0.035f * Mathf.Max(0.25f, ctx.Intensity);

            foreach(var k in list) {
                k.Position += new Vector3((float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 2 - 1)) * a;
                k.Rotation += new Vector3((float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 2 - 1)) * (a * 12f);
            }

            return list;
        }
    }

    internal sealed class FpvDronePreset : IWarudoCameraMotionPreset
    {
        public string Id => "fpv_drone";
        public string DisplayName => "FPV Drone";

        public IReadOnlyList<WarudoCinematicTransformKeyframe> Build(WarudoCinematicMotionContext ctx)
        {
            var T = ctx.GetTargetWorldPosition();
            var r = Mathf.Max(0.2f, ctx.OrbitRadius * Mathf.Max(0.2f, ctx.Intensity));
            var h = ctx.HeightOffset;
            var rng = new System.Random(ctx.ShakeSeed);

            const int steps = 40;
            var list = new List<WarudoCinematicTransformKeyframe>(steps);
            var seg = WarudoCinematicMotionMath.EvenSegmentDuration(steps, ctx.TotalDuration);

            for (var i = 0; i < steps; i++)
            {
                var a = (i / (float)Mathf.Max(1, steps - 1)) * Mathf.PI * 2f;
                var bob = Mathf.Sin(a * 3.1f) * (0.08f * r);
                var weave = Mathf.Cos(a * 2.7f) * (0.14f * r);
                var p = T + new Vector3(Mathf.Cos(a) * r + weave, h + bob, Mathf.Sin(a) * r * 0.92f);
                var look = WarudoCinematicMotionMath.EulerLookAt(p, T + Vector3.up * 0.25f, Vector3.up);
                look.z += (float)(rng.NextDouble() * 2 - 1) * 3.5f * ctx.Intensity;

                list.Add(
                    new WarudoCinematicTransformKeyframe
                    {
                        Position = p,
                        Rotation = look,
                        Scale = Vector3.one,
                        TransitionTime = i == 0 ? 0.01f : seg * 0.92f,
                        Easing = i == 0 ? Ease.Linear : Ease.Linear
                    }
                );
            }

            WarudoCinematicMotionMath.StampFirstKeyframeSnap(list[0]);
            return list;
        }
    }

    public static class WarudoCameraMotionPresetRegistry
    {
        private static readonly IWarudoCameraMotionPreset[] All =
        {
            new DroneOrbitPreset(),
            new CinematicIntroPreset(),
            new HeroShotPreset(),
            new PushInPreset(),
            new CraneUpPreset(),
            new OrbitAroundTargetPreset(),
            new ZoomOutEndingPreset(),
            new HandheldShakePreset(),
            new FpvDronePreset()
        };

        public static IReadOnlyList<IWarudoCameraMotionPreset> AllPresets => All;

        public static IWarudoCameraMotionPreset GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return All[0];
            foreach (var p in All)
                if (string.Equals(p.Id, id, StringComparison.Ordinal))
                    return p;
            return All[0];
        }
    }
}
