using UnityEngine;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// Warudo <see cref="Node68PoiyomiGlobalVolumeAsset"/> → 셰이더 전역 Base Color Dim · Max Brightness.
    /// Poiyomi 머티리얼 순회 없이 <see cref="Shader.SetGlobalFloat"/> 만 사용합니다.
    /// </summary>
    public static class Node68PoiyomiGlobalVolumeBridge
    {
        public static bool Active { get; private set; }
        public static bool ApplyBaseColorDim { get; private set; }
        public static float BaseColorDim { get; private set; } = 1f;
        public static bool LimitBrightness { get; private set; }
        public static float MaxBrightness { get; private set; } = 10f;

        public static void SetFromAsset(
            bool active,
            bool applyBaseColorDim,
            float baseColorDim,
            bool limitBrightness,
            float maxBrightness
        )
        {
            Active = active;
            ApplyBaseColorDim = applyBaseColorDim;
            BaseColorDim = baseColorDim;
            LimitBrightness = limitBrightness;
            MaxBrightness = maxBrightness;
            PushToShader();
        }

        public static void PushToShader()
        {
            if (!Active || !ApplyBaseColorDim)
            {
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.BaseColorDimEnabled,
                    0f
                );
                Shader.SetGlobalFloat(Node68PoiyomiGlobalVolumeShaderIds.BaseColorDim, 1f);
            }
            else
            {
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.BaseColorDimEnabled,
                    1f
                );
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.BaseColorDim,
                    Mathf.Clamp01(BaseColorDim)
                );
            }

            if (!Active || !LimitBrightness)
            {
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.LightingCapEnabled,
                    0f
                );
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.LightingCap,
                    10f
                );
            }
            else
            {
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.LightingCapEnabled,
                    1f
                );
                Shader.SetGlobalFloat(
                    Node68PoiyomiGlobalVolumeShaderIds.LightingCap,
                    Mathf.Clamp(MaxBrightness, 0f, 10f)
                );
            }
        }
    }

    internal static class Node68PoiyomiGlobalVolumeShaderIds
    {
        public static readonly int BaseColorDimEnabled = Shader.PropertyToID(
            "_Node68GlobalBaseColorDimEnabled"
        );
        public static readonly int BaseColorDim = Shader.PropertyToID(
            "_Node68GlobalBaseColorDim"
        );
        public static readonly int LightingCapEnabled = Shader.PropertyToID(
            "_Node68GlobalLightingCapEnabled"
        );
        public static readonly int LightingCap = Shader.PropertyToID(
            "_Node68GlobalLightingCap"
        );
    }
}
