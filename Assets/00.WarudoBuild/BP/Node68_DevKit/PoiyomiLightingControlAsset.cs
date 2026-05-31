using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Node68.ToolkitMods.Node68DevKit;
using UnityEngine;
using UnityEngine.Rendering;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Prop;

namespace Node68.ToolkitMods.LightingControl
{
    /// <summary>
    /// Poiyomi Global Mask 채널(<c>_MainSaturationGlobalMask</c> 등 공용).
    /// 0=Off, 1~4=GlobalMask Texture #1~#4 의 R/G/B/A 채널.
    /// </summary>
    public enum PoiGlobalMaskChannel
    {
        [Label("Off")]
        Off = 0,

        [Label("1R")]
        Tex1R = 1,

        [Label("1G")]
        Tex1G = 2,

        [Label("1B")]
        Tex1B = 3,

        [Label("1A")]
        Tex1A = 4,

        [Label("2R")]
        Tex2R = 5,

        [Label("2G")]
        Tex2G = 6,

        [Label("2B")]
        Tex2B = 7,

        [Label("2A")]
        Tex2A = 8,

        [Label("3R")]
        Tex3R = 9,

        [Label("3G")]
        Tex3G = 10,

        [Label("3B")]
        Tex3B = 11,

        [Label("3A")]
        Tex3A = 12,

        [Label("4R")]
        Tex4R = 13,

        [Label("4G")]
        Tex4G = 14,

        [Label("4B")]
        Tex4B = 15,

        [Label("4A")]
        Tex4A = 16,
    }

    /// <summary>
    /// Poiyomi Global Mask Blend 모드(<c>_MainSaturationGlobalMaskBlendType</c> 등 공용).
    /// 셰이더 enum 값 그대로 사용(예: Add=7, Replace=0).
    /// </summary>
    public enum PoiGlobalMaskBlend
    {
        [Label("Replace")]
        Replace = 0,

        [Label("Subtract")]
        Subtract = 1,

        [Label("Multiply")]
        Multiply = 2,

        [Label("Divide")]
        Divide = 3,

        [Label("Min")]
        Min = 4,

        [Label("Max")]
        Max = 5,

        [Label("Average")]
        Average = 6,

        [Label("Add")]
        Add = 7,
    }

    /// <summary>
    /// Poiyomi <c>_LightingMode</c> 셰이더 enum.
    /// 인스펙터의 「Shadows / Lighting Type」 드롭다운과 동일.
    /// </summary>
    public enum PoiLightingMode
    {
        [Label("Texture Ramp")]
        TextureRamp = 0,

        [Label("Multilayer Math")]
        MultilayerMath = 1,

        [Label("Wrapped")]
        Wrapped = 2,

        [Label("Skin")]
        Skin = 3,

        [Label("Shade Map")]
        ShadeMap = 4,

        [Label("Flat")]
        Flat = 5,

        [Label("Realistic")]
        Realistic = 6,

        [Label("Cloth")]
        Cloth = 7,

        [Label("SDF")]
        SDF = 8,
    }

    /// <summary>
    /// Poiyomi <c>_ShadowMaskType</c> 셰이더 enum.
    /// Shadow Map 섹션의 「Map Type」 드롭다운.
    /// </summary>
    public enum PoiShadowMapType
    {
        [Label("Strength")]
        Strength = 0,

        [Label("Flat")]
        Flat = 1,
    }

    /// <summary>
    /// Poiyomi <c>_RimStyle</c>·<c>_Rim2Style</c> 셰이더 enum.
    /// Rim Lighting 섹션의 「Style」 드롭다운.
    /// </summary>
    public enum PoiRimStyle
    {
        [Label("Poiyomi")]
        Poiyomi = 0,

        [Label("UTS2")]
        UTS2 = 1,

        [Label("LilToon")]
        LilToon = 2,
    }

    /// <summary>
    /// Poiyomi <c>_RimBlendMode</c>·<c>_Rim2BlendMode</c>(LilToon 스타일 한정) 셰이더 enum.
    /// </summary>
    public enum PoiLilToonRimBlendMode
    {
        [Label("Replace")]
        Replace = 0,

        [Label("Add")]
        Add = 1,

        [Label("Screen")]
        Screen = 2,

        [Label("Multiply")]
        Multiply = 3,
    }

    /// <summary>
    /// Poiyomi <c>_*HueShiftColorSpace</c> 셰이더 enum.
    /// </summary>
    public enum PoiHueColorSpace
    {
        [Label("OKLab")]
        OKLab = 0,

        [Label("HSV")]
        HSV = 1,
    }

    /// <summary>
    /// Poiyomi <c>_*HueSelectOrShift</c> 셰이더 enum.
    /// </summary>
    public enum PoiHueSelectOrShift
    {
        [Label("Hue Select")]
        HueSelect = 0,

        [Label("Hue Shift")]
        HueShift = 1,
    }

    /// <summary>
    /// Poiyomi <c>_DepthRimNormalToUse</c> 셰이더 enum.
    /// </summary>
    public enum PoiDepthRimNormalToUse
    {
        [Label("vertex")]
        Vertex = 0,

        [Label("pixel")]
        Pixel = 1,
    }

    /// <summary>
    /// Poiyomi <c>_DepthRimType</c> 셰이더 enum(Rim Type).
    /// </summary>
    public enum PoiDepthRimSampleCount
    {
        [Label("Two Samples")]
        TwoSamples = 0,

        [Label("Four Samples")]
        FourSamples = 1,

        [Label("Eight Samples")]
        EightSamples = 2,
    }

    /// <summary>
    /// Poiyomi <c>_DepthRimLightDirMethod</c> 셰이더 enum.
    /// </summary>
    public enum PoiDepthRimLightDirMethod
    {
        [Label("NdotL")]
        NdotL = 0,

        [Label("Rim Dot Light")]
        RimDotLight = 1,
    }

    /// <summary>
    /// Poiyomi <c>_DepthRimMaskChannel</c> 인스펙터 표기 「Channel」(비트 스칼라 0=R…3=A).
    /// </summary>
    public enum PoiDepthRimMaskChannel
    {
        [Label("R")]
        R = 0,

        [Label("G")]
        G = 1,

        [Label("B")]
        B = 2,

        [Label("A")]
        A = 3,
    }

    /// <summary>
    /// Poiyomi <c>_OutlineExpansionMode</c> (Outlines → Mode).
    /// </summary>
    public enum PoiOutlineExpansionMode
    {
        [Label("Basic")]
        Basic = 1,

        [Label("Rim Light")]
        RimLight = 2,

        [Label("Directional")]
        Directional = 3,

        [Label("DropShadow")]
        DropShadow = 4,
    }

    /// <summary>
    /// Poiyomi <c>_OutlineSpace</c> (Outlines → Space).
    /// </summary>
    public enum PoiOutlineSpace
    {
        [Label("Local")]
        Local = 0,

        [Label("World")]
        World = 1,
    }

    /// <summary>
    /// Poiyomi <c>_LineColorThemeIndex</c> (Color 옆 테마 드롭다운).
    /// </summary>
    public enum PoiOutlineLineColorTheme
    {
        [Label("Off")]
        Off = 0,

        [Label("Theme Color 0")]
        ThemeColor0 = 1,

        [Label("Theme Color 1")]
        ThemeColor1 = 2,

        [Label("Theme Color 2")]
        ThemeColor2 = 3,

        [Label("Theme Color 3")]
        ThemeColor3 = 4,

        [Label("ColorChord 0")]
        ColorChord0 = 5,

        [Label("ColorChord 1")]
        ColorChord1 = 6,

        [Label("ColorChord 2")]
        ColorChord2 = 7,

        [Label("ColorChord 3")]
        ColorChord3 = 8,

        [Label("AL Theme 0")]
        AlTheme0 = 9,

        [Label("AL Theme 1")]
        AlTheme1 = 10,

        [Label("AL Theme 2")]
        AlTheme2 = 11,

        [Label("AL Theme 3")]
        AlTheme3 = 12,
    }

    /// <summary>
    /// Poiyomi <c>_OutlineVertexColorMask</c> (Vertex Colors).
    /// </summary>
    public enum PoiOutlineVertexColorMask
    {
        [Label("Off")]
        Off = 0,

        [Label("R")]
        R = 1,

        [Label("G")]
        G = 2,

        [Label("B")]
        B = 3,

        [Label("A")]
        A = 4,
    }

    /// <summary>
    /// Poiyomi <c>_OutlineZOffsetVertexColor</c> (Outline Z Offset → Vertex Color Channel).
    /// </summary>
    public enum PoiOutlineZOffsetVertexColorChannel
    {
        [Label("Off")]
        Off = 0,

        [Label("R")]
        R = 1,

        [Label("G")]
        G = 2,

        [Label("B")]
        B = 3,

        [Label("A")]
        A = 4,
    }

    /// <summary>
    /// Poiyomi Depth Rim Lighting 블록에서 Unity 애니메이션(Poiyomi의 녹색 A)과 동일하게 자주 바인딩되는 값들을 씁니다.
    /// 프로퍼티 이름은 <c>Assets/_PoiyomiShaders/.../Poiyomi Toon.shader</c> 와 동일합니다.
    /// <see cref="AssetType"/> 메타는 캐릭터·프롭용 파생 에셋 클래스에 각각 둡니다.
    /// </summary>
    public abstract class PoiyomiLightingShaderControlBase : Asset
    {
        internal const string PoiDepthRimKeyword = "_POI_DEPTH_RIMLIGHT";

        private static readonly int EnableDepthRimLighting = Shader.PropertyToID(
            "_EnableDepthRimLighting"
        );

        private static readonly int DepthRimWidthProp = Shader.PropertyToID("_DepthRimWidth");
        private static readonly int DepthRimCameraClipProp = Shader.PropertyToID(
            "_DepthRimCameraClip"
        );
        private static readonly int DepthRimMinDistanceProp = Shader.PropertyToID(
            "_DepthRimMinDistance"
        );
        private static readonly int DepthRimMaxDistanceProp = Shader.PropertyToID(
            "_DepthRimMaxDistance"
        );
        private static readonly int DepthRimHideInShadowProp = Shader.PropertyToID(
            "_DepthRimHideInShadow"
        );
        private static readonly int DepthRimShadowMaskProp = Shader.PropertyToID(
            "_DepthRimShadowMask"
        );
        private static readonly int DepthRimMixRampedLightMapProp = Shader.PropertyToID(
            "_DepthRimMixRampedLightMap"
        );
        private static readonly int DepthRimColorProp = Shader.PropertyToID("_DepthRimColor");
        private static readonly int DepthRimBrightnessProp = Shader.PropertyToID(
            "_DepthRimBrightness"
        );
        private static readonly int DepthRimEmissionProp = Shader.PropertyToID("_DepthRimEmission");
        private static readonly int DepthRimReplaceProp = Shader.PropertyToID("_DepthRimReplace");
        private static readonly int DepthRimAddProp = Shader.PropertyToID("_DepthRimAdd");
        private static readonly int DepthRimScreenProp = Shader.PropertyToID("_DepthRimScreen");
        private static readonly int DepthRimMultiplyProp = Shader.PropertyToID("_DepthRimMultiply");
        private static readonly int DepthRimAdditiveLightingProp = Shader.PropertyToID(
            "_DepthRimAdditiveLighting"
        );
        private static readonly int DepthRimNormalToUseProp = Shader.PropertyToID(
            "_DepthRimNormalToUse"
        );
        private static readonly int DepthRimTypeProp = Shader.PropertyToID("_DepthRimType");
        private static readonly int DepthRimSharpnessProp = Shader.PropertyToID(
            "_DepthRimSharpness"
        );
        private static readonly int DepthRimDepthThresholdProp = Shader.PropertyToID(
            "_DepthRimDepthThreshold"
        );
        private static readonly int DepthRimBinaryProp = Shader.PropertyToID("_DepthRimBinary");
        private static readonly int DepthRimLightDirMethodProp = Shader.PropertyToID(
            "_DepthRimLightDirMethod"
        );
        private static readonly int DepthRimMaskChannelProp = Shader.PropertyToID(
            "_DepthRimMaskChannel"
        );
        private static readonly int DepthRimMixBaseColorProp = Shader.PropertyToID(
            "_DepthRimMixBaseColor"
        );
        private static readonly int DepthRimMixLightColorProp = Shader.PropertyToID(
            "_DepthRimMixLightColor"
        );

        // ─── Color & Normals ───
        private static readonly int ColorProp = Shader.PropertyToID("_Color");
        private static readonly int BumpScaleProp = Shader.PropertyToID("_BumpScale");
        private static readonly int CutoffProp = Shader.PropertyToID("_Cutoff");

        // ─── Color & Normals / Color Adjust ───
        internal const string ColorGradingHdrKeyword = "COLOR_GRADING_HDR";

        private static readonly int MainColorAdjustToggleProp = Shader.PropertyToID(
            "_MainColorAdjustToggle"
        );
        private static readonly int SaturationProp = Shader.PropertyToID("_Saturation");
        private static readonly int MainBrightnessProp = Shader.PropertyToID("_MainBrightness");
        private static readonly int MainGammaProp = Shader.PropertyToID("_MainGamma");

        private static readonly int MainSaturationGlobalMaskProp = Shader.PropertyToID(
            "_MainSaturationGlobalMask"
        );
        private static readonly int MainSaturationGlobalMaskBlendTypeProp = Shader.PropertyToID(
            "_MainSaturationGlobalMaskBlendType"
        );
        private static readonly int MainBrightnessGlobalMaskProp = Shader.PropertyToID(
            "_MainBrightnessGlobalMask"
        );
        private static readonly int MainBrightnessGlobalMaskBlendTypeProp = Shader.PropertyToID(
            "_MainBrightnessGlobalMaskBlendType"
        );
        private static readonly int MainGammaGlobalMaskProp = Shader.PropertyToID(
            "_MainGammaGlobalMask"
        );
        private static readonly int MainGammaGlobalMaskBlendTypeProp = Shader.PropertyToID(
            "_MainGammaGlobalMaskBlendType"
        );

        // ─── Shadows ───
        private static readonly int LightingModeProp = Shader.PropertyToID("_LightingMode");

        // Shadow Layer 1
        private static readonly int Shadow1ColorProp = Shader.PropertyToID("_ShadowColor");
        private static readonly int Shadow1BorderProp = Shader.PropertyToID("_ShadowBorder");
        private static readonly int Shadow1BlurProp = Shader.PropertyToID("_ShadowBlur");
        private static readonly int Shadow1ReceiveProp = Shader.PropertyToID("_ShadowReceive");
        private static readonly int Shadow1NormalStrengthProp = Shader.PropertyToID(
            "_ShadowNormalStrength"
        );

        // Shadow Layer 2
        private static readonly int Shadow2ColorProp = Shader.PropertyToID("_Shadow2ndColor");
        private static readonly int Shadow2BorderProp = Shader.PropertyToID("_Shadow2ndBorder");
        private static readonly int Shadow2BlurProp = Shader.PropertyToID("_Shadow2ndBlur");
        private static readonly int Shadow2ReceiveProp = Shader.PropertyToID("_Shadow2ndReceive");
        private static readonly int Shadow2NormalStrengthProp = Shader.PropertyToID(
            "_Shadow2ndNormalStrength"
        );

        // Shadow Layer 3
        private static readonly int Shadow3ColorProp = Shader.PropertyToID("_Shadow3rdColor");
        private static readonly int Shadow3BorderProp = Shader.PropertyToID("_Shadow3rdBorder");
        private static readonly int Shadow3BlurProp = Shader.PropertyToID("_Shadow3rdBlur");
        private static readonly int Shadow3ReceiveProp = Shader.PropertyToID("_Shadow3rdReceive");
        private static readonly int Shadow3NormalStrengthProp = Shader.PropertyToID(
            "_Shadow3rdNormalStrength"
        );

        // Border (Multilayer Math Border)
        private static readonly int ShadowBorderColorProp = Shader.PropertyToID(
            "_ShadowBorderColor"
        );
        private static readonly int ShadowBorderRangeProp = Shader.PropertyToID(
            "_ShadowBorderRange"
        );

        // Shadow Map
        private static readonly int ShadowMaskTypeProp = Shader.PropertyToID("_ShadowMaskType");

        // Shadow Border Map
        private static readonly int ShadowBorderMapToggleProp = Shader.PropertyToID(
            "_ShadowBorderMapToggle"
        );
        private static readonly int ShadowBorderMaskLODProp = Shader.PropertyToID(
            "_ShadowBorderMaskLOD"
        );
        private static readonly int ShadowPostAOProp = Shader.PropertyToID("_ShadowPostAO");
        private static readonly int ShadowAOShiftProp = Shader.PropertyToID("_ShadowAOShift");
        private static readonly int ShadowAOShift2Prop = Shader.PropertyToID("_ShadowAOShift2");

        // Generic
        private static readonly int LightingMulitlayerNonLinearProp = Shader.PropertyToID(
            "_LightingMulitlayerNonLinear"
        );
        private static readonly int ShadowMainStrengthProp = Shader.PropertyToID(
            "_ShadowMainStrength"
        );
        private static readonly int ShadowEnvStrengthProp = Shader.PropertyToID(
            "_ShadowEnvStrength"
        );
        private static readonly int ShadowStrengthProp = Shader.PropertyToID("_ShadowStrength");
        private static readonly int LightingIgnoreAmbientColorProp = Shader.PropertyToID(
            "_LightingIgnoreAmbientColor"
        );

        // Global Masks (Shading)
        private static readonly int ShadingRampedLightMapApplyGlobalMaskIndexProp =
            Shader.PropertyToID("_ShadingRampedLightMapApplyGlobalMaskIndex");
        private static readonly int ShadingRampedLightMapApplyGlobalMaskBlendTypeProp =
            Shader.PropertyToID("_ShadingRampedLightMapApplyGlobalMaskBlendType");
        private static readonly int ShadingRampedLightMapInverseApplyGlobalMaskIndexProp =
            Shader.PropertyToID("_ShadingRampedLightMapInverseApplyGlobalMaskIndex");
        private static readonly int ShadingRampedLightMapInverseApplyGlobalMaskBlendTypeProp =
            Shader.PropertyToID("_ShadingRampedLightMapInverseApplyGlobalMaskBlendType");

        // ─── Rim Lighting 0 ───
        // 셰이더 토글: [ThryToggle(_GLOSSYREFLECTIONS_OFF)] _EnableRimLighting → 키워드 _GLOSSYREFLECTIONS_OFF
        internal const string PoiRim0Keyword = "_GLOSSYREFLECTIONS_OFF";
        private static readonly int EnableRimLightingProp = Shader.PropertyToID(
            "_EnableRimLighting"
        );
        private static readonly int RimStyleProp = Shader.PropertyToID("_RimStyle");
        private static readonly int RimColorProp = Shader.PropertyToID("_RimColor");
        private static readonly int RimGlobalMaskProp = Shader.PropertyToID("_RimGlobalMask");
        private static readonly int RimGlobalMaskBlendTypeProp = Shader.PropertyToID(
            "_RimGlobalMaskBlendType"
        );
        private static readonly int RimMainStrengthProp = Shader.PropertyToID("_RimMainStrength");
        private static readonly int RimNormalStrengthProp = Shader.PropertyToID(
            "_RimNormalStrength"
        );
        private static readonly int RimBorderProp = Shader.PropertyToID("_RimBorder");
        private static readonly int RimBlurProp = Shader.PropertyToID("_RimBlur");
        private static readonly int RimFresnelPowerProp = Shader.PropertyToID("_RimFresnelPower");
        private static readonly int RimEnableLightingProp = Shader.PropertyToID(
            "_RimEnableLighting"
        );
        private static readonly int RimShadowMaskProp = Shader.PropertyToID("_RimShadowMask");
        private static readonly int RimBackfaceMaskProp = Shader.PropertyToID("_RimBackfaceMask");
        private static readonly int RimVRParallaxStrengthProp = Shader.PropertyToID(
            "_RimVRParallaxStrength"
        );
        private static readonly int RimBlendModeProp = Shader.PropertyToID("_RimBlendMode");
        private static readonly int RimHueShiftEnabledProp = Shader.PropertyToID(
            "_RimHueShiftEnabled"
        );
        private static readonly int RimHueShiftColorSpaceProp = Shader.PropertyToID(
            "_RimHueShiftColorSpace"
        );
        private static readonly int RimHueSelectOrShiftProp = Shader.PropertyToID(
            "_RimHueSelectOrShift"
        );
        private static readonly int RimHueShiftSpeedProp = Shader.PropertyToID("_RimHueShiftSpeed");
        private static readonly int RimHueShiftProp = Shader.PropertyToID("_RimHueShift");

        // ─── Rim Lighting 1 ───
        internal const string PoiRim1Keyword = "POI_RIM2";
        private static readonly int EnableRim2LightingProp = Shader.PropertyToID(
            "_EnableRim2Lighting"
        );
        private static readonly int Rim2StyleProp = Shader.PropertyToID("_Rim2Style");
        private static readonly int Rim2ColorProp = Shader.PropertyToID("_Rim2Color");
        private static readonly int Rim2GlobalMaskProp = Shader.PropertyToID("_Rim2GlobalMask");
        private static readonly int Rim2GlobalMaskBlendTypeProp = Shader.PropertyToID(
            "_Rim2GlobalMaskBlendType"
        );
        private static readonly int Rim2MainStrengthProp = Shader.PropertyToID("_Rim2MainStrength");
        private static readonly int Rim2NormalStrengthProp = Shader.PropertyToID(
            "_Rim2NormalStrength"
        );
        private static readonly int Rim2BorderProp = Shader.PropertyToID("_Rim2Border");
        private static readonly int Rim2BlurProp = Shader.PropertyToID("_Rim2Blur");
        private static readonly int Rim2FresnelPowerProp = Shader.PropertyToID("_Rim2FresnelPower");
        private static readonly int Rim2EnableLightingProp = Shader.PropertyToID(
            "_Rim2EnableLighting"
        );
        private static readonly int Rim2ShadowMaskProp = Shader.PropertyToID("_Rim2ShadowMask");
        private static readonly int Rim2BackfaceMaskProp = Shader.PropertyToID("_Rim2BackfaceMask");
        private static readonly int Rim2VRParallaxStrengthProp = Shader.PropertyToID(
            "_Rim2VRParallaxStrength"
        );
        private static readonly int Rim2BlendModeProp = Shader.PropertyToID("_Rim2BlendMode");
        private static readonly int Rim2HueShiftEnabledProp = Shader.PropertyToID(
            "_Rim2HueShiftEnabled"
        );
        private static readonly int Rim2HueShiftColorSpaceProp = Shader.PropertyToID(
            "_Rim2HueShiftColorSpace"
        );
        private static readonly int Rim2HueSelectOrShiftProp = Shader.PropertyToID(
            "_Rim2HueSelectOrShift"
        );
        private static readonly int Rim2HueShiftSpeedProp = Shader.PropertyToID(
            "_Rim2HueShiftSpeed"
        );
        private static readonly int Rim2HueShiftProp = Shader.PropertyToID("_Rim2HueShift");

        // ─── Outlines (Poiyomi Toon) ───
        private static readonly int EnableOutlinesProp = Shader.PropertyToID("_EnableOutlines");
        private static readonly int OutlineExpansionModeProp = Shader.PropertyToID(
            "_OutlineExpansionMode"
        );
        private static readonly int OutlineSpaceProp = Shader.PropertyToID("_OutlineSpace");
        private static readonly int LineWidthProp = Shader.PropertyToID("_LineWidth");
        private static readonly int LineColorProp = Shader.PropertyToID("_LineColor");
        private static readonly int LineColorThemeIndexProp = Shader.PropertyToID(
            "_LineColorThemeIndex"
        );
        private static readonly int OutlineRimLightBlendProp = Shader.PropertyToID(
            "_OutlineRimLightBlend"
        );
        private static readonly int OutlinePersonaDirectionProp = Shader.PropertyToID(
            "_OutlinePersonaDirection"
        );
        private static readonly int OutlineDropShadowOffsetProp = Shader.PropertyToID(
            "_OutlineDropShadowOffset"
        );
        private static readonly int OutlineEmissionProp = Shader.PropertyToID("_OutlineEmission");
        private static readonly int OutlineTintMixProp = Shader.PropertyToID("_OutlineTintMix");
        private static readonly int PoiUTSStyleOutlineBlendProp = Shader.PropertyToID(
            "_PoiUTSStyleOutlineBlend"
        );
        private static readonly int OutlineHueShiftProp = Shader.PropertyToID("_OutlineHueShift");
        private static readonly int OutlineHueProp = Shader.PropertyToID("_OutlineHue");
        private static readonly int OutlineSaturationProp = Shader.PropertyToID(
            "_OutlineSaturation"
        );
        private static readonly int OutlineValueProp = Shader.PropertyToID("_OutlineValue");
        private static readonly int OutlineGammaProp = Shader.PropertyToID("_OutlineGamma");
        private static readonly int OutlineHueOffsetSpeedProp = Shader.PropertyToID(
            "_OutlineHueOffsetSpeed"
        );
        private static readonly int OutlineFixedSizeProp = Shader.PropertyToID("_OutlineFixedSize");
        private static readonly int OutlineFixWidthProp = Shader.PropertyToID("_OutlineFixWidth");
        private static readonly int OutlinesMaxDistanceProp = Shader.PropertyToID(
            "_OutlinesMaxDistance"
        );
        private static readonly int OutlineLitProp = Shader.PropertyToID("_OutlineLit");
        private static readonly int OutlineShadowStrengthProp = Shader.PropertyToID(
            "_OutlineShadowStrength"
        );
        private static readonly int OffsetZProp = Shader.PropertyToID("_Offset_Z");
        private static readonly int OutlineZOffsetChannelProp = Shader.PropertyToID(
            "_OutlineZOffsetChannel"
        );
        private static readonly int OutlineZOffsetMaskStrengthProp = Shader.PropertyToID(
            "_OutlineZOffsetMaskStrength"
        );
        private static readonly int OutlineZOffsetInvertMaskChannelProp = Shader.PropertyToID(
            "_OutlineZOffsetInvertMaskChannel"
        );
        private static readonly int OutlineZOffsetVertexColorProp = Shader.PropertyToID(
            "_OutlineZOffsetVertexColor"
        );
        private static readonly int OutlineZOffsetVertexColorStrengthProp = Shader.PropertyToID(
            "_OutlineZOffsetVertexColorStrength"
        );
        private static readonly int OutlineUseVertexColorNormalsProp = Shader.PropertyToID(
            "_OutlineUseVertexColorNormals"
        );
        private static readonly int OutlineVertexColorMaskProp = Shader.PropertyToID(
            "_OutlineVertexColorMask"
        );
        private static readonly int OutlineVertexColorMaskStrengthProp = Shader.PropertyToID(
            "_OutlineVertexColorMaskStrength"
        );
        private static readonly int OutlineClipAtZeroWidthProp = Shader.PropertyToID(
            "_OutlineClipAtZeroWidth"
        );
        private static readonly int OutlineOverrideAlphaProp = Shader.PropertyToID(
            "_OutlineOverrideAlpha"
        );
        private static readonly int OutlineCullProp = Shader.PropertyToID("_OutlineCull");
        private static readonly int OutlineZWriteProp = Shader.PropertyToID("_OutlineZWrite");
        private static readonly int OutlineZTestProp = Shader.PropertyToID("_OutlineZTest");

        [Section("Warudo")]
        [DataInput]
        [Label("에셋 활성")]
        [Description("끄면 이 에셋이 머티리얼에 아무 값도 쓰지 않습니다.")]
        public bool LightingControlEnabled = true;

        [DataInput]
        [Label("SkinnedMeshRenderer")]
        [Description("지정하면 해당 스킨드 메시 렌더러만 대상입니다.")]
        public SkinnedMeshRenderer SkinnedMesh;

        [DataInput]
        [Label("MeshRenderer")]
        [Description("스킨드 메시 렌더러가 비어 있을 때만 사용합니다.")]
        public MeshRenderer StaticMeshRenderer;

        [DataInput]
        [Label("메시 키 (스킨/메시)")]
        [Description(
            "비어 있으면 부모 에셋(캐릭터 또는 프롭)의 모든 SkinnedMesh·MeshRenderer를 순회합니다. "
                + "값이 있으면 해당 키의 스킨드 메시를 먼저 찾고, 없으면 MeshRenderer를 찾습니다."
        )]
        [HiddenIf(nameof(HideMeshKeyAutocomplete))]
        [AutoComplete(nameof(CollectMeshKeysForAutocomplete))]
        public string TargetSkinnedMeshKey = "";

        [DataInput]
        [Label("매 프레임 유지 (LateUpdate)")]
        [Description(
            "켜면 매 LateUpdate마다 다시 적용합니다. 다른 시스템(애니메이션·프리셋 등)이 머티리얼을 덮어써도 이 값이 유지되도록 합니다."
        )]
        public bool MaintainEveryFrame = true;

        [DataInput]
        [Label("인스턴스 머티리얼에 쓰기")]
        [Description(
            "켜면 `Renderer.materials`(실제로 그려지는 인스턴스)를 수정합니다. Warudo에서 권장합니다. "
                + "끄면 `sharedMaterials`만 수정합니다."
        )]
        public bool WriteInstanceMaterials = true;

        // ─── Poiyomi: Color & Normals ───

        [Section("● Color & Normals")]
        [DataInput]
        [Label("Color & Alpha")]
        [Description("Poiyomi `_Color`. 베이스 컬러(RGB)와 알파(A). 셰이더 기본값은 (1,1,1,1).")]
        public Color BaseColor = Color.white;

        [DataInput]
        [Label("Normal Map Intensity")]
        [FloatSlider(0f, 10f)]
        [Description("Poiyomi `_BumpScale`. 노말맵 강도(0~10). 셰이더 기본값 1.")]
        public float NormalMapIntensity = 1f;

        [DataInput]
        [Label("Alpha Cutoff")]
        [FloatSlider(0f, 1f)]
        [Description(
            "Poiyomi `_Cutoff`. 알파 컷오프(0~1). 셰이더 기본값 0.5. Render Type이 Cutout 계열일 때 의미가 있습니다."
        )]
        public float AlphaCutoff = 0.5f;

        // ─── Poiyomi: Color & Normals / Color Adjust ───

        [Section("[T] Color Adjust")]
        [DataInput]
        [Label("Adjust Colors")]
        [Description(
            "Poiyomi `_MainColorAdjustToggle`. 셰이더 키워드 `COLOR_GRADING_HDR`을 함께 토글합니다. "
                + "이 토글을 끄더라도 Saturation/Brightness/Gamma 중 어떤 값이든 기본값을 벗어나면 자동으로 활성화됩니다."
        )]
        public bool ColorAdjustEnabled;

        [DataInput]
        [Label("Saturation")]
        [FloatSlider(-1f, 10f)]
        [Description("Poiyomi `_Saturation`. 채도(-1~10). 셰이더 기본값 0.")]
        public float ColorAdjustSaturation;

        [DataInput]
        [Label("Brightness")]
        [FloatSlider(-1f, 2f)]
        [Description("Poiyomi `_MainBrightness`. 밝기(-1~2). 셰이더 기본값 0.")]
        public float ColorAdjustBrightness;

        [DataInput]
        [Label("Gamma")]
        [FloatSlider(0.01f, 5f)]
        [Description("Poiyomi `_MainGamma`. 감마(0.01~5). 셰이더 기본값 1.")]
        public float ColorAdjustGamma = 1f;

        // ─── Global Mask ───

        [Section("Color Adjust / Global Mask")]
        [DataInput]
        [Label("Saturation Mask")]
        [Description(
            "Poiyomi `_MainSaturationGlobalMask`. 채도에 적용할 글로벌 마스크 채널. Off 가 기본."
        )]
        public PoiGlobalMaskChannel SaturationGlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Saturation Mask Blend")]
        [Description("Poiyomi `_MainSaturationGlobalMaskBlendType`. 셰이더 기본값 Multiply(2).")]
        public PoiGlobalMaskBlend SaturationGlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        [DataInput]
        [Label("Brightness Mask")]
        [Description("Poiyomi `_MainBrightnessGlobalMask`. 밝기에 적용할 글로벌 마스크 채널.")]
        public PoiGlobalMaskChannel BrightnessGlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Brightness Mask Blend")]
        [Description("Poiyomi `_MainBrightnessGlobalMaskBlendType`. 셰이더 기본값 Multiply(2).")]
        public PoiGlobalMaskBlend BrightnessGlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        [DataInput]
        [Label("Gamma Mask")]
        [Description("Poiyomi `_MainGammaGlobalMask`. 감마에 적용할 글로벌 마스크 채널.")]
        public PoiGlobalMaskChannel GammaGlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Gamma Mask Blend")]
        [Description("Poiyomi `_MainGammaGlobalMaskBlendType`. 셰이더 기본값 Multiply(2).")]
        public PoiGlobalMaskBlend GammaGlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        // ─── Poiyomi: Depth Rim Lighting ───

        [Section("Depth Rim Lighting")]
        [DataInput]
        [Label("Depth Rim 적용")]
        [Description(
            "`_DepthRimWidth`가 있는 머티리얼에 대해 깊이 림 블록을 Warudo에서 적용합니다. 끄면 `_EnableDepthRimLighting`/키워드·수치 등 어떤 값도 쓰지 않습니다(다른 시스템이 관리할 때 안전)."
        )]
        public bool DepthRimApply = true;

        [DataInput]
        [Label("Depth Rim Lighting")]
        [Description(
            "Poiyomi `_EnableDepthRimLighting` + 셰이더 키워드 `_POI_DEPTH_RIMLIGHT`. 깊이 림 블록 ON/OFF."
        )]
        public bool DepthRimLightingEnabled = true;

        [DataInput]
        [Label("Normal To Use")]
        [Description("Poiyomi `_DepthRimNormalToUse`. vertex(0)·pixel(1). 셰이더 기본 pixel.")]
        public PoiDepthRimNormalToUse DepthRimNormalToUse = PoiDepthRimNormalToUse.Pixel;

        [DataInput]
        [Label("Rim Type")]
        [Description(
            "Poiyomi `_DepthRimType`. 샘플 수. 셰이더 기본 Eight Samples(2). 인스펙터에서는 Two Samples(0)·Four(1)·Eight(2)."
        )]
        public PoiDepthRimSampleCount DepthRimType = PoiDepthRimSampleCount.EightSamples;

        [Section("Depth Rim Lighting / Shape Control")]
        [DataInput]
        [Label("Width")]
        [FloatSlider(0f, 1f)]
        [Description(
            "Poiyomi `_DepthRimWidth`. 림 두께(0~1). 표시에는 깊이 캡처(예: DepthGet)가 필요할 수 있습니다."
        )]
        public float DepthRimShapeControlWidth = 0.08f;

        [DataInput]
        [Label("Depth Threshold")]
        [FloatSlider(0.001f, 1f)]
        [Description(
            "Poiyomi `_DepthRimDepthThreshold`(0.001~1). 씬 Depth와의 차 기준 두께 느낌. 셰이더 기본 0.25."
        )]
        public float DepthRimDepthThreshold = 0.25f;

        [DataInput]
        [Label("Binary")]
        [Description(
            "Poiyomi `_DepthRimBinary`. ON이면 이진 계단 림(Binary), OFF이면 블러(Sharpness·Blur 느낌) 처리."
        )]
        public bool DepthRimBinary = true;

        [DataInput]
        [Label("Sharpness")]
        [FloatSlider(0f, 1f)]
        [Description(
            "Poiyomi `_DepthRimSharpness`. `_DepthRimBinary`가 OFF일 때만 인스펙터에 보이지만 수치는 항상 존재. 셰이더 기본 0.5."
        )]
        public float DepthRimSharpness = 0.5f;

        [DataInput]
        [Label("Fixed Size Threshold")]
        [Description(
            "Poiyomi `_DepthRimCameraClip`. 인스펙터 표기 Fixed Size Threshold. 깊이 림 크기 거리 고정 스케일."
        )]
        public float DepthRimFixedSizeThreshold = 0.5f;

        [DataInput]
        [Label("Min Distance")]
        [Description("Poiyomi `_DepthRimMinDistance`. 최소 거리(셰이더 기본값 0).")]
        public float DepthRimMinDistance;

        [DataInput]
        [Label("Max Distance")]
        [Description("Poiyomi `_DepthRimMaxDistance`. 최대 거리(셰이더 기본값 0).")]
        public float DepthRimMaxDistance;

        // ─── Light Direction ───

        [Section("Depth Rim Lighting / Light Direction")]
        [DataInput]
        [Label("Method")]
        [Description(
            "Poiyomi `_DepthRimLightDirMethod`. NdotL(0)·Rim Dot Light(1). 셰이더 기본 NdotL(0)."
        )]
        public PoiDepthRimLightDirMethod DepthRimLightDirMethod = PoiDepthRimLightDirMethod.NdotL;

        [DataInput]
        [Label("Map to Light Direction")]
        [FloatSlider(0f, 1f)]
        [Description(
            "Poiyomi `_DepthRimHideInShadow` (머티리얼 라벨: Map to Light Direction). 라이팅 방향과의 연동 정도."
        )]
        public float DepthRimMapToLightDirection;

        [DataInput]
        [Label("Mix Attenuation")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimShadowMask` (머티리얼 라벨: Mix Attenuation).")]
        public float DepthRimMixAttenuation;

        [DataInput]
        [Label("Mix Ramped Light Map")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimMixRampedLightMap`. Ramped 라이트맵 믹스.")]
        public float DepthRimMixRampedLightMap;

        [Section("Depth Rim Lighting / Masking")]
        [DataInput]
        [Label("Mask Channel")]
        [Description(
            "Poiyomi `_DepthRimMaskChannel`(R/G/B/A). `_DepthRimMask` 텍스처 슬롯은 건드리지 않음."
        )]
        public PoiDepthRimMaskChannel DepthRimMaskChannel = PoiDepthRimMaskChannel.R;

        // ─── Color ───

        [Section("Depth Rim Lighting / Color")]
        [DataInput]
        [Label("Use Base Color")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimMixBaseColor`. 베이스 컬러 믹스. 셰이더 기본 0.")]
        public float DepthRimUseBaseColor;

        [DataInput]
        [Label("Light Color Mix")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimMixLightColor`. 직사광 컬러 믹스. 셰이더 기본 0.")]
        public float DepthRimLightColorMix;

        [DataInput]
        [Label("Rim Color")]
        [Description("Poiyomi `_DepthRimColor`. 림 색.")]
        public Color DepthRimRimColor = Color.white;

        [DataInput]
        [Label("Color Brightness")]
        [FloatSlider(0f, 10f)]
        [Description("Poiyomi `_DepthRimBrightness`. 색 밝기(0~10).")]
        public float DepthRimColorBrightness = 1f;

        [DataInput]
        [Label("Emission")]
        [FloatSlider(0f, 20f)]
        [Description("Poiyomi `_DepthRimEmission`. 발광 강도(0~20).")]
        public float DepthRimEmission;

        // ─── Blending ───

        [Section("Depth Rim Lighting / Blending")]
        [DataInput]
        [Label("Replace")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimReplace`. 블렌드 – Replace.")]
        public float DepthRimBlendReplace;

        [DataInput]
        [Label("Add")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimAdd`. 블렌드 – Add.")]
        public float DepthRimBlendAdd;

        [DataInput]
        [Label("Screen")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimScreen`. 블렌드 – Screen(셰이더 기본 1).")]
        public float DepthRimBlendScreen = 1f;

        [DataInput]
        [Label("Multiply")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimMultiply`. 블렌드 – Multiply.")]
        public float DepthRimBlendMultiply;

        [DataInput]
        [Label("Unlit Add")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_DepthRimAdditiveLighting` (머티리얼 라벨: Unlit Add).")]
        public float DepthRimBlendUnlitAdd;

        // ─── Poiyomi: Shadows ───

        [Section("[T] Shadows")]
        [DataInput]
        [Label("Shadows 적용")]
        [Description(
            "Poiyomi `Shadows` 블록 전체(Layer 1~3 / Border / Shadow Map / Shadow Border Map / Generic / Global Masks)를 머티리얼에 씁니다. "
                + "끄면 Lighting Type을 포함해 어떤 셰도우 값도 쓰지 않습니다(예: 다른 시스템·프리셋·애니메이션이 셰도우를 관리할 때 안전)."
        )]
        public bool ShadowsApply;

        [DataInput]
        [Label("Lighting Type")]
        [Description(
            "Poiyomi `_LightingMode`. 셰도우 라이팅 타입. 캐릭터 토온은 보통 Multilayer Math(1)."
        )]
        public PoiLightingMode ShadowsLightingType = PoiLightingMode.MultilayerMath;

        // Layer 1
        [Section("Shadows / Shadow Layer 1")]
        [DataInput]
        [Label("Color")]
        [Description("Poiyomi `_ShadowColor`. 셰이더 기본값 (0.7, 0.75, 0.85, 1).")]
        public Color Shadow1Color = new Color(0.7f, 0.75f, 0.85f, 1f);

        [DataInput]
        [Label("Border")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowBorder`. 셰이더 기본값 0.5.")]
        public float Shadow1Border = 0.5f;

        [DataInput]
        [Label("Blur")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowBlur`. 셰이더 기본값 0.1.")]
        public float Shadow1Blur = 0.1f;

        [DataInput]
        [Label("Receive Shadow")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowReceive`. 셰이더 기본값 0.")]
        public float Shadow1Receive;

        [DataInput]
        [Label("Normal Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowNormalStrength`. 셰이더 기본값 1.")]
        public float Shadow1NormalStrength = 1f;

        // Layer 2
        [Section("Shadows / Shadow Layer 2")]
        [DataInput]
        [Label("Color")]
        [Description("Poiyomi `_Shadow2ndColor`. 셰이더 기본값 (0,0,0,0).")]
        public Color Shadow2Color = new Color(0f, 0f, 0f, 0f);

        [DataInput]
        [Label("Border")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow2ndBorder`. 셰이더 기본값 0.5.")]
        public float Shadow2Border = 0.5f;

        [DataInput]
        [Label("Blur")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow2ndBlur`. 셰이더 기본값 0.3.")]
        public float Shadow2Blur = 0.3f;

        [DataInput]
        [Label("Receive Shadow")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow2ndReceive`. 셰이더 기본값 0.")]
        public float Shadow2Receive;

        [DataInput]
        [Label("Normal Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow2ndNormalStrength`. 셰이더 기본값 1.")]
        public float Shadow2NormalStrength = 1f;

        // Layer 3
        [Section("Shadows / Shadow Layer 3")]
        [DataInput]
        [Label("Color")]
        [Description("Poiyomi `_Shadow3rdColor`. 셰이더 기본값 (0,0,0,0).")]
        public Color Shadow3Color = new Color(0f, 0f, 0f, 0f);

        [DataInput]
        [Label("Border")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow3rdBorder`. 셰이더 기본값 0.25.")]
        public float Shadow3Border = 0.25f;

        [DataInput]
        [Label("Blur")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow3rdBlur`. 셰이더 기본값 0.1.")]
        public float Shadow3Blur = 0.1f;

        [DataInput]
        [Label("Receive Shadow")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow3rdReceive`. 셰이더 기본값 0.")]
        public float Shadow3Receive;

        [DataInput]
        [Label("Normal Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Shadow3rdNormalStrength`. 셰이더 기본값 1.")]
        public float Shadow3NormalStrength = 1f;

        // Border
        [Section("Shadows / Border")]
        [DataInput]
        [Label("Color")]
        [Description("Poiyomi `_ShadowBorderColor`. 셰이더 기본값 (1,0,0,1).")]
        public Color ShadowBorderColor = new Color(1f, 0f, 0f, 1f);

        [DataInput]
        [Label("Border Range")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowBorderRange`. 셰이더 기본값 0.")]
        public float ShadowBorderRange;

        // Shadow Map
        [Section("Shadows / Shadow Map")]
        [DataInput]
        [Label("Map Type")]
        [Description("Poiyomi `_ShadowMaskType`. Strength=0, Flat=1.")]
        public PoiShadowMapType ShadowMapType = PoiShadowMapType.Strength;

        // Shadow Border Map
        [Section("Shadows / Shadow Border Map")]
        [DataInput]
        [Label("Shadow Border Map 사용")]
        [Description(
            "Poiyomi `_ShadowBorderMapToggle`. 켜면 Shadow Border Map(AO Map) 처리를 활성화합니다."
        )]
        public bool ShadowBorderMapToggle;

        [DataInput]
        [Label("Border Map LOD")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowBorderMaskLOD`. 셰이더 기본값 0.")]
        public float ShadowBorderMapLOD;

        [DataInput]
        [Label("Ignore Border Properties")]
        [Description("Poiyomi `_ShadowPostAO`. 켜면 AO Shift만 적용하고 Border 값을 무시합니다.")]
        public bool ShadowIgnoreBorderProperties;

        [DataInput]
        [Label("1st Min")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift.x`. 셰이더 기본값 0.")]
        public float Shadow1stMin;

        [DataInput]
        [Label("1st Max")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift.y`. 셰이더 기본값 1.")]
        public float Shadow1stMax = 1f;

        [DataInput]
        [Label("2nd Min")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift.z`. 셰이더 기본값 0.")]
        public float Shadow2ndMin;

        [DataInput]
        [Label("2nd Max")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift.w`. 셰이더 기본값 1.")]
        public float Shadow2ndMax = 1f;

        [DataInput]
        [Label("3rd Min")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift2.x`. 셰이더 기본값 0.")]
        public float Shadow3rdMin;

        [DataInput]
        [Label("3rd Max")]
        [FloatSlider(-0.01f, 1.01f)]
        [Description("Poiyomi `_ShadowAOShift2.y`. 셰이더 기본값 1.")]
        public float Shadow3rdMax = 1f;

        // Generic
        [Section("Shadows / Generic")]
        [DataInput]
        [Label("Non Linear Lightmap")]
        [Description("Poiyomi `_LightingMulitlayerNonLinear`. 셰이더 기본값 ON.")]
        public bool ShadowNonLinearLightmap = true;

        [DataInput]
        [Label("Base Color Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowMainStrength`. 셰이더 기본값 0.")]
        public float ShadowBaseColorBlend;

        [DataInput]
        [Label("Env Strength on Shadow Color")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowEnvStrength`. 셰이더 기본값 0.")]
        public float ShadowEnvStrength;

        [DataInput]
        [Label("Shadow Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_ShadowStrength`. 셰이더 기본값 1.")]
        public float ShadowStrength = 1f;

        [DataInput]
        [Label("Ignore Indirect Shadow Color")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_LightingIgnoreAmbientColor`. 셰이더 기본값 1.")]
        public float ShadowIgnoreIndirectColor = 1f;

        // Global Masks
        [Section("Shadows / Global Masks")]
        [DataInput]
        [Label("LightMap to Global Mask")]
        [Description("Poiyomi `_ShadingRampedLightMapApplyGlobalMaskIndex`.")]
        public PoiGlobalMaskChannel ShadowLightMapToGlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("LightMap Mask Blend")]
        [Description(
            "Poiyomi `_ShadingRampedLightMapApplyGlobalMaskBlendType`. 셰이더 기본값 Multiply(2)."
        )]
        public PoiGlobalMaskBlend ShadowLightMapToGlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        [DataInput]
        [Label("Inversed LightMap to Global Mask")]
        [Description("Poiyomi `_ShadingRampedLightMapInverseApplyGlobalMaskIndex`.")]
        public PoiGlobalMaskChannel ShadowInversedLightMapToGlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Inversed LightMap Mask Blend")]
        [Description(
            "Poiyomi `_ShadingRampedLightMapInverseApplyGlobalMaskBlendType`. 셰이더 기본값 Multiply(2)."
        )]
        public PoiGlobalMaskBlend ShadowInversedLightMapToGlobalMaskBlend =
            PoiGlobalMaskBlend.Multiply;

        // ─── Poiyomi: Rim Lighting 0 (LilToon 스타일 기준) ───

        [Section("Rim Lighting 0")]
        [DataInput]
        [Label("Rim Lighting 0 적용")]
        [Description(
            "Poiyomi `Rim Lighting 0` 블록을 머티리얼에 씁니다. 끄면 어떤 Rim 0 값도 쓰지 않습니다(`_EnableRimLighting`/키워드 포함). "
                + "텍스처 슬롯(Color/Mask·Mask&Bias·Rim Texture·Set_RimLightMask)과 Audio Link / Antipodean / Light Direction Mask 등 부가 기능은 Warudo에서 다루지 않습니다."
        )]
        public bool Rim0Apply;

        [DataInput]
        [Label("Enable Rim Lighting 0")]
        [Description(
            "Poiyomi `_EnableRimLighting` (셰이더 키워드 `_GLOSSYREFLECTIONS_OFF`). Rim Lighting 0 자체의 ON/OFF."
        )]
        public bool Rim0Enable;

        [DataInput]
        [Label("Style")]
        [Description(
            "Poiyomi `_RimStyle`. 화면의 항목들은 LilToon(2) 스타일에서 의미가 있습니다(Poiyomi/UTS2 스타일에선 일부 무시)."
        )]
        public PoiRimStyle Rim0Style = PoiRimStyle.LilToon;

        [DataInput]
        [Label("Rim Color (HDR)")]
        [Description(
            "Poiyomi `_RimColor` (HDR). LilToon 스타일 한정. 셰이더 기본 (0.66, 0.5, 0.48, 1)."
        )]
        public Color Rim0Color = new Color(0.66f, 0.5f, 0.48f, 1f);

        [DataInput]
        [Label("Color Global Mask")]
        [Description("Poiyomi `_RimGlobalMask`. Rim Color에 적용할 글로벌 마스크 채널.")]
        public PoiGlobalMaskChannel Rim0GlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Color Global Mask Blend")]
        [Description("Poiyomi `_RimGlobalMaskBlendType`. 셰이더 기본값 Multiply(2).")]
        public PoiGlobalMaskBlend Rim0GlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        [DataInput]
        [Label("Main Color Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimMainStrength`. 셰이더 기본값 0.")]
        public float Rim0MainColorBlend;

        [DataInput]
        [Label("Normal Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimNormalStrength`. 셰이더 기본값 1.")]
        public float Rim0NormalStrength = 1f;

        [DataInput]
        [Label("Border")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimBorder`. 셰이더 기본값 0.5.")]
        public float Rim0Border = 0.5f;

        [DataInput]
        [Label("Blur")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimBlur`. LilToon 스타일 셰이더 기본값 0.65.")]
        public float Rim0Blur = 0.65f;

        [DataInput]
        [Label("Fresnel Power")]
        [FloatSlider(0.01f, 50f)]
        [Description("Poiyomi `_RimFresnelPower` (PowerSlider 3.0). 셰이더 기본값 3.5.")]
        public float Rim0FresnelPower = 3.5f;

        [DataInput]
        [Label("Enable Lighting")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimEnableLighting`. 셰이더 기본값 1.")]
        public float Rim0EnableLighting = 1f;

        [DataInput]
        [Label("Shadow Mask")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimShadowMask`. 셰이더 기본값 0.5.")]
        public float Rim0ShadowMask = 0.5f;

        [DataInput]
        [Label("Backface Mask")]
        [Description("Poiyomi `_RimBackfaceMask`. 셰이더 기본값 ON.")]
        public bool Rim0BackfaceMask = true;

        [DataInput]
        [Label("VR Parallax Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimVRParallaxStrength`. 셰이더 기본값 1.")]
        public float Rim0VRParallaxStrength = 1f;

        [DataInput]
        [Label("Blend Mode")]
        [Description("Poiyomi `_RimBlendMode`. LilToon 한정. 셰이더 기본값 Add(1).")]
        public PoiLilToonRimBlendMode Rim0BlendMode = PoiLilToonRimBlendMode.Add;

        // Rim 0 Hue Shift
        [Section("[T] Rim Lighting 0 / Hue Shift")]
        [DataInput]
        [Label("Hue Shift 사용")]
        [Description("Poiyomi `_RimHueShiftEnabled`. 켜면 Rim Color에 Hue Shift를 적용합니다.")]
        public bool Rim0HueShiftEnabled;

        [DataInput]
        [Label("Color Space")]
        [Description("Poiyomi `_RimHueShiftColorSpace`. OKLab(0)/HSV(1).")]
        public PoiHueColorSpace Rim0HueShiftColorSpace = PoiHueColorSpace.OKLab;

        [DataInput]
        [Label("Select or Shift")]
        [Description("Poiyomi `_RimHueSelectOrShift`. 셰이더 기본값 Hue Shift(1).")]
        public PoiHueSelectOrShift Rim0HueSelectOrShift = PoiHueSelectOrShift.HueShift;

        [DataInput]
        [Label("Shift Speed")]
        [Description("Poiyomi `_RimHueShiftSpeed`. 자동 회전 속도. 셰이더 기본값 0.")]
        public float Rim0HueShiftSpeed;

        [DataInput]
        [Label("Hue Shift")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_RimHueShift`. 셰이더 기본값 0.")]
        public float Rim0HueShift;

        // ─── Poiyomi: Rim Lighting 1 (LilToon 스타일 기준) ───

        [Section("Rim Lighting 1")]
        [DataInput]
        [Label("Rim Lighting 1 적용")]
        [Description(
            "Poiyomi `Rim Lighting 1` 블록을 머티리얼에 씁니다. 끄면 어떤 Rim 1 값도 쓰지 않습니다(`_EnableRim2Lighting`/키워드 포함)."
        )]
        public bool Rim1Apply;

        [DataInput]
        [Label("Enable Rim Lighting 1")]
        [Description(
            "Poiyomi `_EnableRim2Lighting` (셰이더 키워드 `POI_RIM2`). Rim Lighting 1 자체의 ON/OFF."
        )]
        public bool Rim1Enable;

        [DataInput]
        [Label("Style")]
        [Description(
            "Poiyomi `_Rim2Style`. LilToon(2) 스타일에서 화면의 항목들이 의미가 있습니다."
        )]
        public PoiRimStyle Rim1Style = PoiRimStyle.LilToon;

        [DataInput]
        [Label("Rim Color (HDR)")]
        [Description(
            "Poiyomi `_Rim2Color` (HDR). LilToon 스타일 한정. 셰이더 기본 (0.66, 0.5, 0.48, 1)."
        )]
        public Color Rim1Color = new Color(0.66f, 0.5f, 0.48f, 1f);

        [DataInput]
        [Label("Color Global Mask")]
        [Description("Poiyomi `_Rim2GlobalMask`. Rim Color에 적용할 글로벌 마스크 채널.")]
        public PoiGlobalMaskChannel Rim1GlobalMask = PoiGlobalMaskChannel.Off;

        [DataInput]
        [Label("Color Global Mask Blend")]
        [Description("Poiyomi `_Rim2GlobalMaskBlendType`. 셰이더 기본값 Multiply(2).")]
        public PoiGlobalMaskBlend Rim1GlobalMaskBlend = PoiGlobalMaskBlend.Multiply;

        [DataInput]
        [Label("Main Color Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2MainStrength`. 셰이더 기본값 0.")]
        public float Rim1MainColorBlend;

        [DataInput]
        [Label("Normal Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2NormalStrength`. 셰이더 기본값 1.")]
        public float Rim1NormalStrength = 1f;

        [DataInput]
        [Label("Border")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2Border`. 셰이더 기본값 0.5.")]
        public float Rim1Border = 0.5f;

        [DataInput]
        [Label("Blur")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2Blur`. LilToon 스타일 셰이더 기본값 0.65.")]
        public float Rim1Blur = 0.65f;

        [DataInput]
        [Label("Fresnel Power")]
        [FloatSlider(0.01f, 50f)]
        [Description("Poiyomi `_Rim2FresnelPower`. 셰이더 기본값 3.5.")]
        public float Rim1FresnelPower = 3.5f;

        [DataInput]
        [Label("Enable Lighting")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2EnableLighting`. 셰이더 기본값 1.")]
        public float Rim1EnableLighting = 1f;

        [DataInput]
        [Label("Shadow Mask")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2ShadowMask`. 셰이더 기본값 0.5.")]
        public float Rim1ShadowMask = 0.5f;

        [DataInput]
        [Label("Backface Mask")]
        [Description("Poiyomi `_Rim2BackfaceMask`. 셰이더 기본값 ON.")]
        public bool Rim1BackfaceMask = true;

        [DataInput]
        [Label("VR Parallax Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2VRParallaxStrength`. 셰이더 기본값 1.")]
        public float Rim1VRParallaxStrength = 1f;

        [DataInput]
        [Label("Blend Mode")]
        [Description("Poiyomi `_Rim2BlendMode`. LilToon 한정. 셰이더 기본값 Add(1).")]
        public PoiLilToonRimBlendMode Rim1BlendMode = PoiLilToonRimBlendMode.Add;

        // Rim 1 Hue Shift
        [Section("[T] Rim Lighting 1 / Hue Shift")]
        [DataInput]
        [Label("Hue Shift 사용")]
        [Description("Poiyomi `_Rim2HueShiftEnabled`. 켜면 Rim Color에 Hue Shift를 적용합니다.")]
        public bool Rim1HueShiftEnabled;

        [DataInput]
        [Label("Color Space")]
        [Description("Poiyomi `_Rim2HueShiftColorSpace`.")]
        public PoiHueColorSpace Rim1HueShiftColorSpace = PoiHueColorSpace.OKLab;

        [DataInput]
        [Label("Select or Shift")]
        [Description("Poiyomi `_Rim2HueSelectOrShift`. 셰이더 기본값 Hue Shift(1).")]
        public PoiHueSelectOrShift Rim1HueSelectOrShift = PoiHueSelectOrShift.HueShift;

        [DataInput]
        [Label("Shift Speed")]
        [Description("Poiyomi `_Rim2HueShiftSpeed`. 자동 회전 속도. 셰이더 기본값 0.")]
        public float Rim1HueShiftSpeed;

        [DataInput]
        [Label("Hue Shift")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_Rim2HueShift`. 셰이더 기본값 0.")]
        public float Rim1HueShift;

        // ─── Poiyomi: Outlines ───
        // Distance Alpha 서브블록·`_OutlineMask`/`_OutlineTexture` 텍스처 슬롯은 Warudo에서 다루지 않습니다.

        [Section("Outlines")]
        [DataInput]
        [Label("Outlines 적용")]
        [Description(
            "Poiyomi `Outlines` 블록을 머티리얼에 씁니다. 끄면 `_EnableOutlines`·모드·수치를 쓰지 않습니다. "
                + "`_OutlineSizeMask`·`_OutlineTexture` 텍스처·`Distance Alpha` 서브섹션은 이 에셋에서 설정하지 않습니다."
        )]
        public bool OutlinesApply;

        [DataInput]
        [Label("Enable Outlines")]
        [Description("Poiyomi `_EnableOutlines`. 아웃라인 카테고리 ON/OFF.")]
        public bool OutlinesEnabled = true;

        [DataInput]
        [Label("Mode")]
        [Description("Poiyomi `_OutlineExpansionMode`. Basic=1 … DropShadow=4.")]
        public PoiOutlineExpansionMode OutlineMode = PoiOutlineExpansionMode.Basic;

        [DataInput]
        [Label("Space")]
        [Description("Poiyomi `_OutlineSpace`. Local(0)·World(1).")]
        public PoiOutlineSpace OutlineSpace = PoiOutlineSpace.Local;

        [DataInput]
        [Label("Outline Size")]
        [Description(
            "Poiyomi `_LineWidth`. 인스펙터 Outline Size(셰이더는 /100 스케일로 버텍스 오프셋에 사용)."
        )]
        public float OutlineSize = 0.07f;

        [DataInput]
        [Label("Color")]
        [Description("Poiyomi `_LineColor`.")]
        public Color OutlineLineColor = Color.white;

        [DataInput]
        [Label("Color Theme")]
        [Description("Poiyomi `_LineColorThemeIndex`. Off·Theme·AL 등 드롭다운.")]
        public PoiOutlineLineColorTheme OutlineLineColorTheme = PoiOutlineLineColorTheme.Off;

        [DataInput]
        [Label("Outline Emission")]
        [FloatSlider(0f, 20f)]
        [Description("Poiyomi `_OutlineEmission`.")]
        public float OutlineEmission;

        [DataInput]
        [Label("MainTex blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineTintMix`.")]
        public float OutlineMainTexBlend;

        [DataInput]
        [Label("UTS2 style Blend")]
        [Description("Poiyomi `_PoiUTSStyleOutlineBlend`.")]
        public bool OutlineUTS2StyleBlend;

        [DataInput]
        [Label("Rim Light Blend")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineRimLightBlend`. Mode가 Rim Light(2)일 때 의미가 있습니다.")]
        public float OutlineRimLightBlend;

        [DataInput]
        [Label("Directional Offset")]
        [Description(
            "Poiyomi `_OutlinePersonaDirection`. Mode가 Directional(3)일 때(XY 사용). 셰이더 기본 (1,0,0,0)."
        )]
        public Vector4 OutlineDirectionalOffset = new Vector4(1f, 0f, 0f, 0f);

        [DataInput]
        [Label("Drop Shadow Direction")]
        [Description(
            "Poiyomi `_OutlineDropShadowOffset`. Mode가 DropShadow(4)일 때. 셰이더 기본 (1,0,0,0)."
        )]
        public Vector4 OutlineDropShadowDirection = new Vector4(1f, 0f, 0f, 0f);

        [Section("[T] Outlines / Color Adjust")]
        [DataInput]
        [Label("Color Adjust")]
        [Description("Poiyomi `_OutlineHueShift`.")]
        public bool OutlineColorAdjustEnabled;

        [DataInput]
        [Label("Hue")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineHue`.")]
        public float OutlineAdjustHue;

        [DataInput]
        [Label("Saturation")]
        [FloatSlider(0f, 2f)]
        [Description("Poiyomi `_OutlineSaturation`. 셰이더 기본 1.")]
        public float OutlineAdjustSaturation = 1f;

        [DataInput]
        [Label("Value")]
        [FloatSlider(0f, 2f)]
        [Description("Poiyomi `_OutlineValue`. 셰이더 기본 1.")]
        public float OutlineAdjustValue = 1f;

        [DataInput]
        [Label("Gamma")]
        [FloatSlider(0.01f, 2f)]
        [Description("Poiyomi `_OutlineGamma`. 셰이더 기본 1.")]
        public float OutlineAdjustGamma = 1f;

        [DataInput]
        [Label("Shift Speed")]
        [Description("Poiyomi `_OutlineHueOffsetSpeed`.")]
        public float OutlineHueOffsetSpeed;

        [Section("Outlines / Fixed Size Over Distance")]
        [DataInput]
        [Label("Fixed Size")]
        [Description("Poiyomi `_OutlineFixedSize`.")]
        public bool OutlineFixedSize = true;

        [DataInput]
        [Label("Fixed Width")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineFixWidth`.")]
        public float OutlineFixedWidth = 0.3f;

        [DataInput]
        [Label("Fixed Size Max Distance")]
        [Description("Poiyomi `_OutlinesMaxDistance`. 셰이더 기본 1.")]
        public float OutlineFixedSizeMaxDistance = 1f;

        [Section("Outlines / Lighting")]
        [DataInput]
        [Label("Enable Lighting")]
        [Description("Poiyomi `_OutlineLit`.")]
        public bool OutlineLitEnabled = true;

        [DataInput]
        [Label("Shadow Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineShadowStrength`.")]
        public float OutlineShadowStrength;

        [Section("Outlines / Outline Z Offset")]
        [DataInput]
        [Label("Overall Strength")]
        [Description("Poiyomi `_Offset_Z`.")]
        public float OutlineZOffsetOverall;

        [DataInput]
        [Label("Outline Mask Channel")]
        [Description(
            "Poiyomi `_OutlineZOffsetChannel`. `_OutlineMask` 텍스처는 건드리지 않으며 채널 선택만 반영합니다."
        )]
        public PoiDepthRimMaskChannel OutlineZOffsetMaskChannel = PoiDepthRimMaskChannel.R;

        [DataInput]
        [Label("Mask Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineZOffsetMaskStrength`. 셰이더 기본 1.")]
        public float OutlineZOffsetMaskStrength = 1f;

        [DataInput]
        [Label("Invert Mask Channel")]
        [Description("Poiyomi `_OutlineZOffsetInvertMaskChannel`.")]
        public bool OutlineZOffsetInvertMaskChannel;

        [DataInput]
        [Label("Vertex Color Channel")]
        [Description("Poiyomi `_OutlineZOffsetVertexColor`.")]
        public PoiOutlineZOffsetVertexColorChannel OutlineZOffsetVertexColorChannel =
            PoiOutlineZOffsetVertexColorChannel.Off;

        [DataInput]
        [Label("Vertex Color Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineZOffsetVertexColorStrength`. 셰이더 기본 1.")]
        public float OutlineZOffsetVertexColorStrength = 1f;

        [Section("Outlines / Vertex Colors")]
        [DataInput]
        [Label("Vertex Color Normals")]
        [Description("Poiyomi `_OutlineUseVertexColorNormals`.")]
        public bool OutlineUseVertexColorNormals;

        [DataInput]
        [Label("Vertex Color Mask")]
        [Description("Poiyomi `_OutlineVertexColorMask`.")]
        public PoiOutlineVertexColorMask OutlineVertexColorMask = PoiOutlineVertexColorMask.Off;

        [DataInput]
        [Label("VC Mask Strength")]
        [FloatSlider(0f, 1f)]
        [Description("Poiyomi `_OutlineVertexColorMaskStrength`. 셰이더 기본 1.")]
        public float OutlineVertexColorMaskStrength = 1f;

        [Section("Outlines / Rendering Options")]
        [DataInput]
        [Label("Clip 0 Width")]
        [Description("Poiyomi `_OutlineClipAtZeroWidth`. 셰이더 기본 ON.")]
        public bool OutlineClipAtZeroWidth = true;

        [DataInput]
        [Label("Override Base Alpha")]
        [Description("Poiyomi `_OutlineOverrideAlpha`.")]
        public bool OutlineOverrideAlpha;

        [DataInput]
        [Label("Cull")]
        [Description("Poiyomi `_OutlineCull`. `UnityEngine.Rendering.CullMode` 값과 동일.")]
        public CullMode OutlineCull = CullMode.Front;

        [DataInput]
        [Label("ZWrite")]
        [Description("Poiyomi `_OutlineZWrite`. 0=Off, 1=On.")]
        public bool OutlineZWrite = true;

        [DataInput]
        [Label("ZTest")]
        [Description("Poiyomi `_OutlineZTest`. `UnityEngine.Rendering.CompareFunction` 값과 동일.")]
        public CompareFunction OutlineZTest = CompareFunction.LessEqual;

        [Markdown]
        [Hidden]
        public string _note =
            "위 항목은 Poiyomi `Poiyomi Toon.shader`의 `Color & Normals` / `Color Adjust` / `Depth Rim Lighting` / `Shadows` / `Rim Lighting 0` / `Rim Lighting 1` / `Outlines` 블록과 동일한 프로퍼티명을 씁니다. "
            + "Color & Normals: `_Color`/`_BumpScale`/`_Cutoff`만 씁니다(텍스처 슬롯은 건드리지 않음). "
            + "Color Adjust: Saturation/Brightness/Gamma + Global Mask. `_MainColorAdjustToggle` 활성 시 셰이더 키워드 `COLOR_GRADING_HDR`을 함께 토글합니다. `_MainColorAdjustToggle`이 없는 슬롯은 건너뜁니다. "
            + "(Hue Shift / AudioLink 항목은 셰이더 변종 stripping 문제로 Warudo에서 제외됨.) "
            + "Depth Rim: 「Depth Rim 적용」 시 `_DepthRimWidth`가 있는 슬롯만 씁니다. 「Depth Rim Lighting」 토글로 `_EnableDepthRimLighting`·`_POI_DEPTH_RIMLIGHT` 전환. "
            + "`_DepthRimMask` 텍스처는 건드리지 않으며 Channel만 씁니다. "
            + "Shadows: 「Shadows 적용」 토글이 켜진 경우에만 씁니다(Lighting Type 포함). "
            + "Color Tex(`_ShadowColorTex`/`_Shadow2ndColorTex`/`_Shadow3rdColorTex`)·Shadow Map(`_ShadowStrengthMask`)·"
            + "AO Map(`_ShadowBorderMask`)·Blur Map(`_MultilayerMathBlurMap`) 등 텍스처 슬롯은 Warudo에서 건드리지 않습니다. "
            + "Rim Lighting 0/1: 「적용」 토글이 켜진 경우에만 씁니다(Enable Rim, Style, LilToon 스타일의 색·블러·프레넬·블렌드·Hue Shift). "
            + "텍스처 슬롯(Color/Mask·Mask&Bias·Rim Texture·Set_RimLightMask)·Light Direction Mask·Antipodean·Audio Link·Alpha Masking은 Warudo에서 다루지 않습니다. "
            + "Enable Rim 토글 시 셰이더 키워드 `_GLOSSYREFLECTIONS_OFF`(Rim 0)·`POI_RIM2`(Rim 1)도 함께 전환합니다. "
            + "Outlines: 「Outlines 적용」 시 `_EnableOutlines`가 있는 슬롯만 씁니다. `_OutlineMask`·`_OutlineTexture`·Distance Alpha 블록은 건드리지 않습니다. "
            + "Audio Link·Outline Stencil·Outline Blending 등은 Warudo에서 다루지 않습니다.";

        /// <summary>스킨/메시 딕셔너리 해석은 파생 클래스(캐릭터 에셋)에서 구현합니다.</summary>
        protected abstract void WatchMeshParentAsset();

        protected abstract Dictionary<string, SkinnedMeshRenderer> GetParentSkinnedMeshRenderers();

        protected abstract Dictionary<string, MeshRenderer> GetParentMeshRenderers();

        /// <summary>자동완성 캐시 무효화에 사용(캐릭터 참조).</summary>
        protected abstract object GetMeshAutocompleteParentRef();

        protected abstract string GetParentRequiredAutocompleteMessage();

        private AutoCompleteList _acCachedList;
        private object _acCacheMeshParentRef;
        private Dictionary<string, SkinnedMeshRenderer> _acCacheSmrsDictRef;
        private Dictionary<string, MeshRenderer> _acCacheMrsDictRef;

        // HiddenIf 용: Warudo는 구체 Asset 타입에서 메서드를 찾으므로 private이면 등록 실패.
        protected bool HideMeshKeyAutocomplete() =>
            SkinnedMesh != null || StaticMeshRenderer != null;

        public async UniTask<AutoCompleteList> CollectMeshKeysForAutocomplete()
        {
            await UniTask.CompletedTask;

            if (GetMeshAutocompleteParentRef() == null)
                return AutoCompleteList.Message(GetParentRequiredAutocompleteMessage());

            var smrs = GetParentSkinnedMeshRenderers();
            var mrs = GetParentMeshRenderers();
            var smEmpty = smrs == null || smrs.Count == 0;
            var mrEmpty = mrs == null || mrs.Count == 0;
            if (smEmpty && mrEmpty)
                return AutoCompleteList.Message("스킨드 메시·MeshRenderer가 없습니다.");

            if (
                _acCachedList != null
                && ReferenceEquals(_acCacheMeshParentRef, GetMeshAutocompleteParentRef())
                && ReferenceEquals(_acCacheSmrsDictRef, smrs)
                && ReferenceEquals(_acCacheMrsDictRef, mrs)
            )
                return _acCachedList;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            var keysBuffer = new List<string>();
            if (!smEmpty)
            {
                foreach (var kv in smrs!)
                {
                    if (kv.Value != null && seen.Add(kv.Key))
                        keysBuffer.Add(kv.Key);
                }
            }

            if (!mrEmpty)
            {
                foreach (var kv in mrs!)
                {
                    if (kv.Value != null && seen.Add(kv.Key))
                        keysBuffer.Add(kv.Key);
                }
            }

            keysBuffer.Sort(StringComparer.OrdinalIgnoreCase);

            var entries = new List<AutoCompleteEntry>(keysBuffer.Count);
            foreach (var key in keysBuffer)
                entries.Add(new AutoCompleteEntry { label = key, value = key });

            _acCachedList = entries.ToAutoCompleteList();
            _acCacheMeshParentRef = GetMeshAutocompleteParentRef();
            _acCacheSmrsDictRef = smrs;
            _acCacheMrsDictRef = mrs;
            return _acCachedList;
        }

        [Trigger]
        [Label("지금 적용")]
        public void ApplyNow()
        {
            ApplyOnce();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            SetActive(true);

            Watch(nameof(LightingControlEnabled), OnInputChanged);
            Watch(nameof(SkinnedMesh), OnInputChanged);
            Watch(nameof(StaticMeshRenderer), OnInputChanged);
            WatchMeshParentAsset();
            Watch(nameof(TargetSkinnedMeshKey), OnInputChanged);
            Watch(nameof(MaintainEveryFrame), OnInputChanged);
            Watch(nameof(WriteInstanceMaterials), OnInputChanged);

            Watch(nameof(BaseColor), OnInputChanged);
            Watch(nameof(NormalMapIntensity), OnInputChanged);
            Watch(nameof(AlphaCutoff), OnInputChanged);

            Watch(nameof(ColorAdjustEnabled), OnInputChanged);
            Watch(nameof(ColorAdjustSaturation), OnInputChanged);
            Watch(nameof(ColorAdjustBrightness), OnInputChanged);
            Watch(nameof(ColorAdjustGamma), OnInputChanged);
            Watch(nameof(SaturationGlobalMask), OnInputChanged);
            Watch(nameof(SaturationGlobalMaskBlend), OnInputChanged);
            Watch(nameof(BrightnessGlobalMask), OnInputChanged);
            Watch(nameof(BrightnessGlobalMaskBlend), OnInputChanged);
            Watch(nameof(GammaGlobalMask), OnInputChanged);
            Watch(nameof(GammaGlobalMaskBlend), OnInputChanged);

            Watch(nameof(DepthRimApply), OnInputChanged);
            Watch(nameof(DepthRimLightingEnabled), OnInputChanged);
            Watch(nameof(DepthRimNormalToUse), OnInputChanged);
            Watch(nameof(DepthRimType), OnInputChanged);

            Watch(nameof(DepthRimShapeControlWidth), OnInputChanged);
            Watch(nameof(DepthRimDepthThreshold), OnInputChanged);
            Watch(nameof(DepthRimBinary), OnInputChanged);
            Watch(nameof(DepthRimSharpness), OnInputChanged);
            Watch(nameof(DepthRimFixedSizeThreshold), OnInputChanged);
            Watch(nameof(DepthRimMinDistance), OnInputChanged);
            Watch(nameof(DepthRimMaxDistance), OnInputChanged);

            Watch(nameof(DepthRimLightDirMethod), OnInputChanged);
            Watch(nameof(DepthRimMapToLightDirection), OnInputChanged);
            Watch(nameof(DepthRimMixAttenuation), OnInputChanged);
            Watch(nameof(DepthRimMixRampedLightMap), OnInputChanged);
            Watch(nameof(DepthRimMaskChannel), OnInputChanged);

            Watch(nameof(DepthRimUseBaseColor), OnInputChanged);
            Watch(nameof(DepthRimLightColorMix), OnInputChanged);
            Watch(nameof(DepthRimRimColor), OnInputChanged);
            Watch(nameof(DepthRimColorBrightness), OnInputChanged);
            Watch(nameof(DepthRimEmission), OnInputChanged);
            Watch(nameof(DepthRimBlendReplace), OnInputChanged);
            Watch(nameof(DepthRimBlendAdd), OnInputChanged);
            Watch(nameof(DepthRimBlendScreen), OnInputChanged);
            Watch(nameof(DepthRimBlendMultiply), OnInputChanged);
            Watch(nameof(DepthRimBlendUnlitAdd), OnInputChanged);

            Watch(nameof(ShadowsApply), OnInputChanged);
            Watch(nameof(ShadowsLightingType), OnInputChanged);

            Watch(nameof(Shadow1Color), OnInputChanged);
            Watch(nameof(Shadow1Border), OnInputChanged);
            Watch(nameof(Shadow1Blur), OnInputChanged);
            Watch(nameof(Shadow1Receive), OnInputChanged);
            Watch(nameof(Shadow1NormalStrength), OnInputChanged);

            Watch(nameof(Shadow2Color), OnInputChanged);
            Watch(nameof(Shadow2Border), OnInputChanged);
            Watch(nameof(Shadow2Blur), OnInputChanged);
            Watch(nameof(Shadow2Receive), OnInputChanged);
            Watch(nameof(Shadow2NormalStrength), OnInputChanged);

            Watch(nameof(Shadow3Color), OnInputChanged);
            Watch(nameof(Shadow3Border), OnInputChanged);
            Watch(nameof(Shadow3Blur), OnInputChanged);
            Watch(nameof(Shadow3Receive), OnInputChanged);
            Watch(nameof(Shadow3NormalStrength), OnInputChanged);

            Watch(nameof(ShadowBorderColor), OnInputChanged);
            Watch(nameof(ShadowBorderRange), OnInputChanged);

            Watch(nameof(ShadowMapType), OnInputChanged);

            Watch(nameof(ShadowBorderMapToggle), OnInputChanged);
            Watch(nameof(ShadowBorderMapLOD), OnInputChanged);
            Watch(nameof(ShadowIgnoreBorderProperties), OnInputChanged);
            Watch(nameof(Shadow1stMin), OnInputChanged);
            Watch(nameof(Shadow1stMax), OnInputChanged);
            Watch(nameof(Shadow2ndMin), OnInputChanged);
            Watch(nameof(Shadow2ndMax), OnInputChanged);
            Watch(nameof(Shadow3rdMin), OnInputChanged);
            Watch(nameof(Shadow3rdMax), OnInputChanged);

            Watch(nameof(ShadowNonLinearLightmap), OnInputChanged);
            Watch(nameof(ShadowBaseColorBlend), OnInputChanged);
            Watch(nameof(ShadowEnvStrength), OnInputChanged);
            Watch(nameof(ShadowStrength), OnInputChanged);
            Watch(nameof(ShadowIgnoreIndirectColor), OnInputChanged);

            Watch(nameof(ShadowLightMapToGlobalMask), OnInputChanged);
            Watch(nameof(ShadowLightMapToGlobalMaskBlend), OnInputChanged);
            Watch(nameof(ShadowInversedLightMapToGlobalMask), OnInputChanged);
            Watch(nameof(ShadowInversedLightMapToGlobalMaskBlend), OnInputChanged);

            // Rim Lighting 0
            Watch(nameof(Rim0Apply), OnInputChanged);
            Watch(nameof(Rim0Enable), OnInputChanged);
            Watch(nameof(Rim0Style), OnInputChanged);
            Watch(nameof(Rim0Color), OnInputChanged);
            Watch(nameof(Rim0GlobalMask), OnInputChanged);
            Watch(nameof(Rim0GlobalMaskBlend), OnInputChanged);
            Watch(nameof(Rim0MainColorBlend), OnInputChanged);
            Watch(nameof(Rim0NormalStrength), OnInputChanged);
            Watch(nameof(Rim0Border), OnInputChanged);
            Watch(nameof(Rim0Blur), OnInputChanged);
            Watch(nameof(Rim0FresnelPower), OnInputChanged);
            Watch(nameof(Rim0EnableLighting), OnInputChanged);
            Watch(nameof(Rim0ShadowMask), OnInputChanged);
            Watch(nameof(Rim0BackfaceMask), OnInputChanged);
            Watch(nameof(Rim0VRParallaxStrength), OnInputChanged);
            Watch(nameof(Rim0BlendMode), OnInputChanged);
            Watch(nameof(Rim0HueShiftEnabled), OnInputChanged);
            Watch(nameof(Rim0HueShiftColorSpace), OnInputChanged);
            Watch(nameof(Rim0HueSelectOrShift), OnInputChanged);
            Watch(nameof(Rim0HueShiftSpeed), OnInputChanged);
            Watch(nameof(Rim0HueShift), OnInputChanged);

            // Rim Lighting 1
            Watch(nameof(Rim1Apply), OnInputChanged);
            Watch(nameof(Rim1Enable), OnInputChanged);
            Watch(nameof(Rim1Style), OnInputChanged);
            Watch(nameof(Rim1Color), OnInputChanged);
            Watch(nameof(Rim1GlobalMask), OnInputChanged);
            Watch(nameof(Rim1GlobalMaskBlend), OnInputChanged);
            Watch(nameof(Rim1MainColorBlend), OnInputChanged);
            Watch(nameof(Rim1NormalStrength), OnInputChanged);
            Watch(nameof(Rim1Border), OnInputChanged);
            Watch(nameof(Rim1Blur), OnInputChanged);
            Watch(nameof(Rim1FresnelPower), OnInputChanged);
            Watch(nameof(Rim1EnableLighting), OnInputChanged);
            Watch(nameof(Rim1ShadowMask), OnInputChanged);
            Watch(nameof(Rim1BackfaceMask), OnInputChanged);
            Watch(nameof(Rim1VRParallaxStrength), OnInputChanged);
            Watch(nameof(Rim1BlendMode), OnInputChanged);
            Watch(nameof(Rim1HueShiftEnabled), OnInputChanged);
            Watch(nameof(Rim1HueShiftColorSpace), OnInputChanged);
            Watch(nameof(Rim1HueSelectOrShift), OnInputChanged);
            Watch(nameof(Rim1HueShiftSpeed), OnInputChanged);
            Watch(nameof(Rim1HueShift), OnInputChanged);

            Watch(nameof(OutlinesApply), OnInputChanged);
            Watch(nameof(OutlinesEnabled), OnInputChanged);
            Watch(nameof(OutlineMode), OnInputChanged);
            Watch(nameof(OutlineSpace), OnInputChanged);
            Watch(nameof(OutlineSize), OnInputChanged);
            Watch(nameof(OutlineLineColor), OnInputChanged);
            Watch(nameof(OutlineLineColorTheme), OnInputChanged);
            Watch(nameof(OutlineEmission), OnInputChanged);
            Watch(nameof(OutlineMainTexBlend), OnInputChanged);
            Watch(nameof(OutlineUTS2StyleBlend), OnInputChanged);
            Watch(nameof(OutlineRimLightBlend), OnInputChanged);
            Watch(nameof(OutlineDirectionalOffset), OnInputChanged);
            Watch(nameof(OutlineDropShadowDirection), OnInputChanged);
            Watch(nameof(OutlineColorAdjustEnabled), OnInputChanged);
            Watch(nameof(OutlineAdjustHue), OnInputChanged);
            Watch(nameof(OutlineAdjustSaturation), OnInputChanged);
            Watch(nameof(OutlineAdjustValue), OnInputChanged);
            Watch(nameof(OutlineAdjustGamma), OnInputChanged);
            Watch(nameof(OutlineHueOffsetSpeed), OnInputChanged);
            Watch(nameof(OutlineFixedSize), OnInputChanged);
            Watch(nameof(OutlineFixedWidth), OnInputChanged);
            Watch(nameof(OutlineFixedSizeMaxDistance), OnInputChanged);
            Watch(nameof(OutlineLitEnabled), OnInputChanged);
            Watch(nameof(OutlineShadowStrength), OnInputChanged);
            Watch(nameof(OutlineZOffsetOverall), OnInputChanged);
            Watch(nameof(OutlineZOffsetMaskChannel), OnInputChanged);
            Watch(nameof(OutlineZOffsetMaskStrength), OnInputChanged);
            Watch(nameof(OutlineZOffsetInvertMaskChannel), OnInputChanged);
            Watch(nameof(OutlineZOffsetVertexColorChannel), OnInputChanged);
            Watch(nameof(OutlineZOffsetVertexColorStrength), OnInputChanged);
            Watch(nameof(OutlineUseVertexColorNormals), OnInputChanged);
            Watch(nameof(OutlineVertexColorMask), OnInputChanged);
            Watch(nameof(OutlineVertexColorMaskStrength), OnInputChanged);
            Watch(nameof(OutlineClipAtZeroWidth), OnInputChanged);
            Watch(nameof(OutlineOverrideAlpha), OnInputChanged);
            Watch(nameof(OutlineCull), OnInputChanged);
            Watch(nameof(OutlineZWrite), OnInputChanged);
            Watch(nameof(OutlineZTest), OnInputChanged);

            ApplyOnce();
        }

        protected void OnInputChanged()
        {
            ApplyOnce();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (MaintainEveryFrame && LightingControlEnabled)
                ApplyOnce();
        }

        private void ApplyOnce()
        {
            if (!LightingControlEnabled)
                return;

            foreach (var r in ResolveRenderers())
            {
                if (r == null)
                    continue;

                if (WriteInstanceMaterials)
                {
                    var inst = r.materials;
                    if (inst == null)
                        continue;
                    for (var i = 0; i < inst.Length; i++)
                    {
                        var m = inst[i];
                        if (m == null)
                            continue;
                        TryApplyPoiyomiColorAndNormalsBlock(m);
                        TryApplyPoiyomiColorAdjustBlock(m);
                        TryApplyPoiyomiDepthRimBlock(m);
                        TryApplyPoiyomiShadowsBlock(m);
                        TryApplyPoiyomiRimLighting0Block(m);
                        TryApplyPoiyomiRimLighting1Block(m);
                        TryApplyPoiyomiOutlinesBlock(m);
                    }

                    r.materials = inst;
                }
                else
                {
                    var mats = r.sharedMaterials;
                    if (mats == null)
                        continue;
                    for (var i = 0; i < mats.Length; i++)
                    {
                        var m = mats[i];
                        if (m == null)
                            continue;
                        TryApplyPoiyomiColorAndNormalsBlock(m);
                        TryApplyPoiyomiColorAdjustBlock(m);
                        TryApplyPoiyomiDepthRimBlock(m);
                        TryApplyPoiyomiShadowsBlock(m);
                        TryApplyPoiyomiRimLighting0Block(m);
                        TryApplyPoiyomiRimLighting1Block(m);
                        TryApplyPoiyomiOutlinesBlock(m);
                    }
                }
            }
        }

        /// <summary>
        /// Poiyomi `Color & Normals` 섹션의 핵심 값(베이스 컬러/노말 강도/알파 컷오프)을 씁니다.
        /// 텍스처 슬롯(`_MainTex`/`_BumpMap`/`_AlphaMask`)은 건드리지 않습니다.
        /// </summary>
        private void TryApplyPoiyomiColorAndNormalsBlock(Material m)
        {
            if (m.HasProperty(ColorProp))
                m.SetColor(ColorProp, BaseColor);
            if (m.HasProperty(BumpScaleProp))
                m.SetFloat(BumpScaleProp, Mathf.Clamp(NormalMapIntensity, 0f, 10f));
            if (m.HasProperty(CutoffProp))
                m.SetFloat(CutoffProp, Mathf.Clamp01(AlphaCutoff));
        }

        /// <summary>
        /// Poiyomi `Color & Normals / Color Adjust` 섹션 전체(메인/Hue Shift/AudioLink/Global Mask)를 씁니다.
        /// `_MainColorAdjustToggle` 토글 시 셰이더 키워드 `COLOR_GRADING_HDR`도 함께 전환합니다.
        /// `_MainColorAdjustToggle`이 없는 머티리얼(Poiyomi 외)은 건너뜁니다.
        /// </summary>
        private void TryApplyPoiyomiColorAdjustBlock(Material m)
        {
            if (!m.HasProperty(MainColorAdjustToggleProp))
                return;

            // 셰이더에서 Color Adjust 블록이 실제로 컴파일·실행되려면
            // `COLOR_GRADING_HDR` 키워드가 켜져 있어야 합니다(`#pragma shader_feature COLOR_GRADING_HDR`).
            // 사용자가 `Adjust Colors` 상위 토글을 따로 켜지 않더라도, Saturation /
            // Brightness / Gamma 중 하나라도 의미 있는 값이면 키워드를 자동 활성화해
            // 「슬라이더를 움직였는데 안 변함」 사고를 방지합니다.
            var needsColorGrading =
                ColorAdjustEnabled
                || ColorAdjustSaturation != 0f
                || ColorAdjustBrightness != 0f
                || !Mathf.Approximately(ColorAdjustGamma, 1f);

            m.SetFloat(MainColorAdjustToggleProp, needsColorGrading ? 1f : 0f);
            if (needsColorGrading)
                m.EnableKeyword(ColorGradingHdrKeyword);
            else
                m.DisableKeyword(ColorGradingHdrKeyword);

            if (m.HasProperty(SaturationProp))
                m.SetFloat(SaturationProp, Mathf.Clamp(ColorAdjustSaturation, -1f, 10f));
            if (m.HasProperty(MainBrightnessProp))
                m.SetFloat(MainBrightnessProp, Mathf.Clamp(ColorAdjustBrightness, -1f, 2f));
            if (m.HasProperty(MainGammaProp))
                m.SetFloat(MainGammaProp, Mathf.Clamp(ColorAdjustGamma, 0.01f, 5f));

            if (m.HasProperty(MainSaturationGlobalMaskProp))
                m.SetFloat(MainSaturationGlobalMaskProp, (int)SaturationGlobalMask);
            if (m.HasProperty(MainSaturationGlobalMaskBlendTypeProp))
                m.SetFloat(MainSaturationGlobalMaskBlendTypeProp, (int)SaturationGlobalMaskBlend);
            if (m.HasProperty(MainBrightnessGlobalMaskProp))
                m.SetFloat(MainBrightnessGlobalMaskProp, (int)BrightnessGlobalMask);
            if (m.HasProperty(MainBrightnessGlobalMaskBlendTypeProp))
                m.SetFloat(MainBrightnessGlobalMaskBlendTypeProp, (int)BrightnessGlobalMaskBlend);
            if (m.HasProperty(MainGammaGlobalMaskProp))
                m.SetFloat(MainGammaGlobalMaskProp, (int)GammaGlobalMask);
            if (m.HasProperty(MainGammaGlobalMaskBlendTypeProp))
                m.SetFloat(MainGammaGlobalMaskBlendTypeProp, (int)GammaGlobalMaskBlend);
        }

        /// <summary>
        /// <c>_DepthRimWidth</c> 존재 시 Poiyomi Depth Rim 블록으로 간주하고 필드를 씁니다.
        /// 「Depth Rim 적용」이 꺼져 있으면 건너뜁니다. 「Depth Rim Lighting」이 꺼지면 기능 OFF + 키워드 비활성.
        /// </summary>
        private void TryApplyPoiyomiDepthRimBlock(Material m)
        {
            if (!DepthRimApply)
                return;
            if (!m.HasProperty(DepthRimWidthProp))
                return;

            if (m.HasProperty(EnableDepthRimLighting))
                m.SetFloat(EnableDepthRimLighting, DepthRimLightingEnabled ? 1f : 0f);

            if (DepthRimLightingEnabled)
                m.EnableKeyword(PoiDepthRimKeyword);
            else
                m.DisableKeyword(PoiDepthRimKeyword);

            if (m.HasProperty(DepthRimNormalToUseProp))
                m.SetFloat(DepthRimNormalToUseProp, (int)DepthRimNormalToUse);
            if (m.HasProperty(DepthRimTypeProp))
                m.SetFloat(DepthRimTypeProp, (int)DepthRimType);

            m.SetFloat(DepthRimWidthProp, Mathf.Clamp01(DepthRimShapeControlWidth));

            if (m.HasProperty(DepthRimSharpnessProp))
                m.SetFloat(DepthRimSharpnessProp, Mathf.Clamp01(DepthRimSharpness));
            if (m.HasProperty(DepthRimDepthThresholdProp))
                m.SetFloat(
                    DepthRimDepthThresholdProp,
                    Mathf.Clamp(DepthRimDepthThreshold, 0.001f, 1f)
                );
            if (m.HasProperty(DepthRimBinaryProp))
                m.SetFloat(DepthRimBinaryProp, DepthRimBinary ? 1f : 0f);
            if (m.HasProperty(DepthRimCameraClipProp))
                m.SetFloat(DepthRimCameraClipProp, DepthRimFixedSizeThreshold);
            if (m.HasProperty(DepthRimMinDistanceProp))
                m.SetFloat(DepthRimMinDistanceProp, DepthRimMinDistance);
            if (m.HasProperty(DepthRimMaxDistanceProp))
                m.SetFloat(DepthRimMaxDistanceProp, DepthRimMaxDistance);

            if (m.HasProperty(DepthRimLightDirMethodProp))
                m.SetFloat(DepthRimLightDirMethodProp, (int)DepthRimLightDirMethod);
            if (m.HasProperty(DepthRimHideInShadowProp))
                m.SetFloat(DepthRimHideInShadowProp, Mathf.Clamp01(DepthRimMapToLightDirection));
            if (m.HasProperty(DepthRimShadowMaskProp))
                m.SetFloat(DepthRimShadowMaskProp, Mathf.Clamp01(DepthRimMixAttenuation));
            if (m.HasProperty(DepthRimMixRampedLightMapProp))
                m.SetFloat(DepthRimMixRampedLightMapProp, Mathf.Clamp01(DepthRimMixRampedLightMap));

            if (m.HasProperty(DepthRimMaskChannelProp))
                m.SetFloat(DepthRimMaskChannelProp, (int)DepthRimMaskChannel);

            if (m.HasProperty(DepthRimMixBaseColorProp))
                m.SetFloat(DepthRimMixBaseColorProp, Mathf.Clamp01(DepthRimUseBaseColor));
            if (m.HasProperty(DepthRimMixLightColorProp))
                m.SetFloat(DepthRimMixLightColorProp, Mathf.Clamp01(DepthRimLightColorMix));

            if (m.HasProperty(DepthRimColorProp))
                m.SetColor(DepthRimColorProp, DepthRimRimColor);
            if (m.HasProperty(DepthRimBrightnessProp))
                m.SetFloat(DepthRimBrightnessProp, Mathf.Clamp(DepthRimColorBrightness, 0f, 10f));
            if (m.HasProperty(DepthRimEmissionProp))
                m.SetFloat(DepthRimEmissionProp, Mathf.Clamp(DepthRimEmission, 0f, 20f));

            if (m.HasProperty(DepthRimReplaceProp))
                m.SetFloat(DepthRimReplaceProp, Mathf.Clamp01(DepthRimBlendReplace));
            if (m.HasProperty(DepthRimAddProp))
                m.SetFloat(DepthRimAddProp, Mathf.Clamp01(DepthRimBlendAdd));
            if (m.HasProperty(DepthRimScreenProp))
                m.SetFloat(DepthRimScreenProp, Mathf.Clamp01(DepthRimBlendScreen));
            if (m.HasProperty(DepthRimMultiplyProp))
                m.SetFloat(DepthRimMultiplyProp, Mathf.Clamp01(DepthRimBlendMultiply));
            if (m.HasProperty(DepthRimAdditiveLightingProp))
                m.SetFloat(DepthRimAdditiveLightingProp, Mathf.Clamp01(DepthRimBlendUnlitAdd));
        }

        /// <summary>
        /// Poiyomi `Shadows` 블록의 비-텍스처 프로퍼티(Layer 1~3 / Border / Shadow Map / Shadow Border Map / Generic / Global Masks)를 씁니다.
        /// 텍스처 슬롯(<c>_ShadowColorTex</c>·<c>_Shadow{2nd,3rd}ColorTex</c>·<c>_ShadowStrengthMask</c>·<c>_ShadowBorderMask</c>·<c>_MultilayerMathBlurMap</c>)은 건드리지 않습니다.
        /// 「Shadows 적용」 토글이 꺼져 있거나 `_LightingMode`가 없는 머티리얼은 건너뜁니다.
        /// </summary>
        private void TryApplyPoiyomiShadowsBlock(Material m)
        {
            if (!ShadowsApply)
                return;
            if (!m.HasProperty(LightingModeProp))
                return;

            m.SetFloat(LightingModeProp, (int)ShadowsLightingType);

            if (m.HasProperty(Shadow1ColorProp))
                m.SetColor(Shadow1ColorProp, Shadow1Color);
            if (m.HasProperty(Shadow1BorderProp))
                m.SetFloat(Shadow1BorderProp, Mathf.Clamp01(Shadow1Border));
            if (m.HasProperty(Shadow1BlurProp))
                m.SetFloat(Shadow1BlurProp, Mathf.Clamp01(Shadow1Blur));
            if (m.HasProperty(Shadow1ReceiveProp))
                m.SetFloat(Shadow1ReceiveProp, Mathf.Clamp01(Shadow1Receive));
            if (m.HasProperty(Shadow1NormalStrengthProp))
                m.SetFloat(Shadow1NormalStrengthProp, Mathf.Clamp01(Shadow1NormalStrength));

            if (m.HasProperty(Shadow2ColorProp))
                m.SetColor(Shadow2ColorProp, Shadow2Color);
            if (m.HasProperty(Shadow2BorderProp))
                m.SetFloat(Shadow2BorderProp, Mathf.Clamp01(Shadow2Border));
            if (m.HasProperty(Shadow2BlurProp))
                m.SetFloat(Shadow2BlurProp, Mathf.Clamp01(Shadow2Blur));
            if (m.HasProperty(Shadow2ReceiveProp))
                m.SetFloat(Shadow2ReceiveProp, Mathf.Clamp01(Shadow2Receive));
            if (m.HasProperty(Shadow2NormalStrengthProp))
                m.SetFloat(Shadow2NormalStrengthProp, Mathf.Clamp01(Shadow2NormalStrength));

            if (m.HasProperty(Shadow3ColorProp))
                m.SetColor(Shadow3ColorProp, Shadow3Color);
            if (m.HasProperty(Shadow3BorderProp))
                m.SetFloat(Shadow3BorderProp, Mathf.Clamp01(Shadow3Border));
            if (m.HasProperty(Shadow3BlurProp))
                m.SetFloat(Shadow3BlurProp, Mathf.Clamp01(Shadow3Blur));
            if (m.HasProperty(Shadow3ReceiveProp))
                m.SetFloat(Shadow3ReceiveProp, Mathf.Clamp01(Shadow3Receive));
            if (m.HasProperty(Shadow3NormalStrengthProp))
                m.SetFloat(Shadow3NormalStrengthProp, Mathf.Clamp01(Shadow3NormalStrength));

            if (m.HasProperty(ShadowBorderColorProp))
                m.SetColor(ShadowBorderColorProp, ShadowBorderColor);
            if (m.HasProperty(ShadowBorderRangeProp))
                m.SetFloat(ShadowBorderRangeProp, Mathf.Clamp01(ShadowBorderRange));

            if (m.HasProperty(ShadowMaskTypeProp))
                m.SetFloat(ShadowMaskTypeProp, (int)ShadowMapType);

            if (m.HasProperty(ShadowBorderMapToggleProp))
                m.SetFloat(ShadowBorderMapToggleProp, ShadowBorderMapToggle ? 1f : 0f);
            if (m.HasProperty(ShadowBorderMaskLODProp))
                m.SetFloat(ShadowBorderMaskLODProp, Mathf.Clamp01(ShadowBorderMapLOD));
            if (m.HasProperty(ShadowPostAOProp))
                m.SetFloat(ShadowPostAOProp, ShadowIgnoreBorderProperties ? 1f : 0f);
            if (m.HasProperty(ShadowAOShiftProp))
            {
                m.SetVector(
                    ShadowAOShiftProp,
                    new Vector4(
                        Mathf.Clamp(Shadow1stMin, -0.01f, 1.01f),
                        Mathf.Clamp(Shadow1stMax, -0.01f, 1.01f),
                        Mathf.Clamp(Shadow2ndMin, -0.01f, 1.01f),
                        Mathf.Clamp(Shadow2ndMax, -0.01f, 1.01f)
                    )
                );
            }
            if (m.HasProperty(ShadowAOShift2Prop))
            {
                m.SetVector(
                    ShadowAOShift2Prop,
                    new Vector4(
                        Mathf.Clamp(Shadow3rdMin, -0.01f, 1.01f),
                        Mathf.Clamp(Shadow3rdMax, -0.01f, 1.01f),
                        0f,
                        0f
                    )
                );
            }

            if (m.HasProperty(LightingMulitlayerNonLinearProp))
                m.SetFloat(LightingMulitlayerNonLinearProp, ShadowNonLinearLightmap ? 1f : 0f);
            if (m.HasProperty(ShadowMainStrengthProp))
                m.SetFloat(ShadowMainStrengthProp, Mathf.Clamp01(ShadowBaseColorBlend));
            if (m.HasProperty(ShadowEnvStrengthProp))
                m.SetFloat(ShadowEnvStrengthProp, Mathf.Clamp01(ShadowEnvStrength));
            if (m.HasProperty(ShadowStrengthProp))
                m.SetFloat(ShadowStrengthProp, Mathf.Clamp01(ShadowStrength));
            if (m.HasProperty(LightingIgnoreAmbientColorProp))
                m.SetFloat(
                    LightingIgnoreAmbientColorProp,
                    Mathf.Clamp01(ShadowIgnoreIndirectColor)
                );

            if (m.HasProperty(ShadingRampedLightMapApplyGlobalMaskIndexProp))
                m.SetFloat(
                    ShadingRampedLightMapApplyGlobalMaskIndexProp,
                    (int)ShadowLightMapToGlobalMask
                );
            if (m.HasProperty(ShadingRampedLightMapApplyGlobalMaskBlendTypeProp))
                m.SetFloat(
                    ShadingRampedLightMapApplyGlobalMaskBlendTypeProp,
                    (int)ShadowLightMapToGlobalMaskBlend
                );
            if (m.HasProperty(ShadingRampedLightMapInverseApplyGlobalMaskIndexProp))
                m.SetFloat(
                    ShadingRampedLightMapInverseApplyGlobalMaskIndexProp,
                    (int)ShadowInversedLightMapToGlobalMask
                );
            if (m.HasProperty(ShadingRampedLightMapInverseApplyGlobalMaskBlendTypeProp))
                m.SetFloat(
                    ShadingRampedLightMapInverseApplyGlobalMaskBlendTypeProp,
                    (int)ShadowInversedLightMapToGlobalMaskBlend
                );
        }

        /// <summary>
        /// Poiyomi `Rim Lighting 0` 블록의 비-텍스처 프로퍼티(Style·Color·Global Mask·LilToon 형태 컨트롤·Hue Shift)를 씁니다.
        /// 텍스처 슬롯(<c>_RimColorTex</c>·<c>_RimMask</c>·<c>_RimTex</c>·<c>_Set_RimLightMask</c>) 및
        /// Antipodean / Light Direction Mask / Audio Link / Alpha &amp; Global Masking은 건드리지 않습니다.
        /// 「Rim Lighting 0 적용」 토글이 꺼져 있거나 `_RimStyle`이 없는 머티리얼은 건너뜁니다.
        /// </summary>
        private void TryApplyPoiyomiRimLighting0Block(Material m)
        {
            if (!Rim0Apply)
                return;
            if (!m.HasProperty(RimStyleProp))
                return;

            if (m.HasProperty(EnableRimLightingProp))
                m.SetFloat(EnableRimLightingProp, Rim0Enable ? 1f : 0f);
            if (Rim0Enable)
                m.EnableKeyword(PoiRim0Keyword);
            else
                m.DisableKeyword(PoiRim0Keyword);

            m.SetFloat(RimStyleProp, (int)Rim0Style);

            if (m.HasProperty(RimColorProp))
                m.SetColor(RimColorProp, Rim0Color);
            if (m.HasProperty(RimGlobalMaskProp))
                m.SetFloat(RimGlobalMaskProp, (int)Rim0GlobalMask);
            if (m.HasProperty(RimGlobalMaskBlendTypeProp))
                m.SetFloat(RimGlobalMaskBlendTypeProp, (int)Rim0GlobalMaskBlend);

            if (m.HasProperty(RimMainStrengthProp))
                m.SetFloat(RimMainStrengthProp, Mathf.Clamp01(Rim0MainColorBlend));
            if (m.HasProperty(RimNormalStrengthProp))
                m.SetFloat(RimNormalStrengthProp, Mathf.Clamp01(Rim0NormalStrength));
            if (m.HasProperty(RimBorderProp))
                m.SetFloat(RimBorderProp, Mathf.Clamp01(Rim0Border));
            if (m.HasProperty(RimBlurProp))
                m.SetFloat(RimBlurProp, Mathf.Clamp01(Rim0Blur));
            if (m.HasProperty(RimFresnelPowerProp))
                m.SetFloat(RimFresnelPowerProp, Mathf.Clamp(Rim0FresnelPower, 0.01f, 50f));
            if (m.HasProperty(RimEnableLightingProp))
                m.SetFloat(RimEnableLightingProp, Mathf.Clamp01(Rim0EnableLighting));
            if (m.HasProperty(RimShadowMaskProp))
                m.SetFloat(RimShadowMaskProp, Mathf.Clamp01(Rim0ShadowMask));
            if (m.HasProperty(RimBackfaceMaskProp))
                m.SetFloat(RimBackfaceMaskProp, Rim0BackfaceMask ? 1f : 0f);
            if (m.HasProperty(RimVRParallaxStrengthProp))
                m.SetFloat(RimVRParallaxStrengthProp, Mathf.Clamp01(Rim0VRParallaxStrength));
            if (m.HasProperty(RimBlendModeProp))
                m.SetFloat(RimBlendModeProp, (int)Rim0BlendMode);

            if (m.HasProperty(RimHueShiftEnabledProp))
                m.SetFloat(RimHueShiftEnabledProp, Rim0HueShiftEnabled ? 1f : 0f);
            if (m.HasProperty(RimHueShiftColorSpaceProp))
                m.SetFloat(RimHueShiftColorSpaceProp, (int)Rim0HueShiftColorSpace);
            if (m.HasProperty(RimHueSelectOrShiftProp))
                m.SetFloat(RimHueSelectOrShiftProp, (int)Rim0HueSelectOrShift);
            if (m.HasProperty(RimHueShiftSpeedProp))
                m.SetFloat(RimHueShiftSpeedProp, Rim0HueShiftSpeed);
            if (m.HasProperty(RimHueShiftProp))
                m.SetFloat(RimHueShiftProp, Mathf.Clamp01(Rim0HueShift));
        }

        /// <summary>
        /// Poiyomi `Rim Lighting 1` 블록의 비-텍스처 프로퍼티를 씁니다(Rim 0와 동일 패턴).
        /// 「Rim Lighting 1 적용」 토글이 꺼져 있거나 `_Rim2Style`이 없는 머티리얼은 건너뜁니다.
        /// </summary>
        private void TryApplyPoiyomiRimLighting1Block(Material m)
        {
            if (!Rim1Apply)
                return;
            if (!m.HasProperty(Rim2StyleProp))
                return;

            if (m.HasProperty(EnableRim2LightingProp))
                m.SetFloat(EnableRim2LightingProp, Rim1Enable ? 1f : 0f);
            if (Rim1Enable)
                m.EnableKeyword(PoiRim1Keyword);
            else
                m.DisableKeyword(PoiRim1Keyword);

            m.SetFloat(Rim2StyleProp, (int)Rim1Style);

            if (m.HasProperty(Rim2ColorProp))
                m.SetColor(Rim2ColorProp, Rim1Color);
            if (m.HasProperty(Rim2GlobalMaskProp))
                m.SetFloat(Rim2GlobalMaskProp, (int)Rim1GlobalMask);
            if (m.HasProperty(Rim2GlobalMaskBlendTypeProp))
                m.SetFloat(Rim2GlobalMaskBlendTypeProp, (int)Rim1GlobalMaskBlend);

            if (m.HasProperty(Rim2MainStrengthProp))
                m.SetFloat(Rim2MainStrengthProp, Mathf.Clamp01(Rim1MainColorBlend));
            if (m.HasProperty(Rim2NormalStrengthProp))
                m.SetFloat(Rim2NormalStrengthProp, Mathf.Clamp01(Rim1NormalStrength));
            if (m.HasProperty(Rim2BorderProp))
                m.SetFloat(Rim2BorderProp, Mathf.Clamp01(Rim1Border));
            if (m.HasProperty(Rim2BlurProp))
                m.SetFloat(Rim2BlurProp, Mathf.Clamp01(Rim1Blur));
            if (m.HasProperty(Rim2FresnelPowerProp))
                m.SetFloat(Rim2FresnelPowerProp, Mathf.Clamp(Rim1FresnelPower, 0.01f, 50f));
            if (m.HasProperty(Rim2EnableLightingProp))
                m.SetFloat(Rim2EnableLightingProp, Mathf.Clamp01(Rim1EnableLighting));
            if (m.HasProperty(Rim2ShadowMaskProp))
                m.SetFloat(Rim2ShadowMaskProp, Mathf.Clamp01(Rim1ShadowMask));
            if (m.HasProperty(Rim2BackfaceMaskProp))
                m.SetFloat(Rim2BackfaceMaskProp, Rim1BackfaceMask ? 1f : 0f);
            if (m.HasProperty(Rim2VRParallaxStrengthProp))
                m.SetFloat(Rim2VRParallaxStrengthProp, Mathf.Clamp01(Rim1VRParallaxStrength));
            if (m.HasProperty(Rim2BlendModeProp))
                m.SetFloat(Rim2BlendModeProp, (int)Rim1BlendMode);

            if (m.HasProperty(Rim2HueShiftEnabledProp))
                m.SetFloat(Rim2HueShiftEnabledProp, Rim1HueShiftEnabled ? 1f : 0f);
            if (m.HasProperty(Rim2HueShiftColorSpaceProp))
                m.SetFloat(Rim2HueShiftColorSpaceProp, (int)Rim1HueShiftColorSpace);
            if (m.HasProperty(Rim2HueSelectOrShiftProp))
                m.SetFloat(Rim2HueSelectOrShiftProp, (int)Rim1HueSelectOrShift);
            if (m.HasProperty(Rim2HueShiftSpeedProp))
                m.SetFloat(Rim2HueShiftSpeedProp, Rim1HueShiftSpeed);
            if (m.HasProperty(Rim2HueShiftProp))
                m.SetFloat(Rim2HueShiftProp, Mathf.Clamp01(Rim1HueShift));
        }

        /// <summary>
        /// Poiyomi「Outlines」블록 전달. `_OutlineMask`·`_OutlineTexture`·Distance Alpha 서브블록은 쓰지 않습니다.
        /// 「Outlines 적용」이 꺼져 있거나 `_EnableOutlines`가 없는 머티리얼은 건너뜁니다.
        /// </summary>
        private void TryApplyPoiyomiOutlinesBlock(Material m)
        {
            if (!OutlinesApply)
                return;
            if (!m.HasProperty(EnableOutlinesProp))
                return;

            m.SetFloat(EnableOutlinesProp, OutlinesEnabled ? 1f : 0f);

            if (m.HasProperty(OutlineExpansionModeProp))
                m.SetFloat(OutlineExpansionModeProp, (int)OutlineMode);
            if (m.HasProperty(OutlineSpaceProp))
                m.SetFloat(OutlineSpaceProp, (int)OutlineSpace);

            if (m.HasProperty(LineWidthProp))
                m.SetFloat(LineWidthProp, Mathf.Max(0f, OutlineSize));

            if (m.HasProperty(LineColorProp))
                m.SetColor(LineColorProp, OutlineLineColor);
            if (m.HasProperty(LineColorThemeIndexProp))
                m.SetFloat(LineColorThemeIndexProp, (int)OutlineLineColorTheme);

            if (m.HasProperty(OutlineRimLightBlendProp))
                m.SetFloat(OutlineRimLightBlendProp, Mathf.Clamp01(OutlineRimLightBlend));
            if (m.HasProperty(OutlinePersonaDirectionProp))
                m.SetVector(OutlinePersonaDirectionProp, OutlineDirectionalOffset);
            if (m.HasProperty(OutlineDropShadowOffsetProp))
                m.SetVector(OutlineDropShadowOffsetProp, OutlineDropShadowDirection);

            if (m.HasProperty(OutlineEmissionProp))
                m.SetFloat(OutlineEmissionProp, Mathf.Clamp(OutlineEmission, 0f, 20f));
            if (m.HasProperty(OutlineTintMixProp))
                m.SetFloat(OutlineTintMixProp, Mathf.Clamp01(OutlineMainTexBlend));
            if (m.HasProperty(PoiUTSStyleOutlineBlendProp))
                m.SetFloat(PoiUTSStyleOutlineBlendProp, OutlineUTS2StyleBlend ? 1f : 0f);

            if (m.HasProperty(OutlineHueShiftProp))
                m.SetFloat(OutlineHueShiftProp, OutlineColorAdjustEnabled ? 1f : 0f);
            if (m.HasProperty(OutlineHueProp))
                m.SetFloat(OutlineHueProp, Mathf.Clamp01(OutlineAdjustHue));
            if (m.HasProperty(OutlineSaturationProp))
                m.SetFloat(OutlineSaturationProp, Mathf.Clamp(OutlineAdjustSaturation, 0f, 2f));
            if (m.HasProperty(OutlineValueProp))
                m.SetFloat(OutlineValueProp, Mathf.Clamp(OutlineAdjustValue, 0f, 2f));
            if (m.HasProperty(OutlineGammaProp))
                m.SetFloat(OutlineGammaProp, Mathf.Clamp(OutlineAdjustGamma, 0.01f, 2f));
            if (m.HasProperty(OutlineHueOffsetSpeedProp))
                m.SetFloat(OutlineHueOffsetSpeedProp, OutlineHueOffsetSpeed);

            if (m.HasProperty(OutlineFixedSizeProp))
                m.SetFloat(OutlineFixedSizeProp, OutlineFixedSize ? 1f : 0f);
            if (m.HasProperty(OutlineFixWidthProp))
                m.SetFloat(OutlineFixWidthProp, Mathf.Clamp01(OutlineFixedWidth));
            if (m.HasProperty(OutlinesMaxDistanceProp))
                m.SetFloat(OutlinesMaxDistanceProp, OutlineFixedSizeMaxDistance);

            if (m.HasProperty(OutlineLitProp))
                m.SetFloat(OutlineLitProp, OutlineLitEnabled ? 1f : 0f);
            if (m.HasProperty(OutlineShadowStrengthProp))
                m.SetFloat(OutlineShadowStrengthProp, Mathf.Clamp01(OutlineShadowStrength));

            if (m.HasProperty(OffsetZProp))
                m.SetFloat(OffsetZProp, OutlineZOffsetOverall);
            if (m.HasProperty(OutlineZOffsetChannelProp))
                m.SetFloat(OutlineZOffsetChannelProp, (int)OutlineZOffsetMaskChannel);
            if (m.HasProperty(OutlineZOffsetMaskStrengthProp))
                m.SetFloat(
                    OutlineZOffsetMaskStrengthProp,
                    Mathf.Clamp01(OutlineZOffsetMaskStrength)
                );
            if (m.HasProperty(OutlineZOffsetInvertMaskChannelProp))
                m.SetFloat(
                    OutlineZOffsetInvertMaskChannelProp,
                    OutlineZOffsetInvertMaskChannel ? 1f : 0f
                );
            if (m.HasProperty(OutlineZOffsetVertexColorProp))
                m.SetFloat(OutlineZOffsetVertexColorProp, (int)OutlineZOffsetVertexColorChannel);
            if (m.HasProperty(OutlineZOffsetVertexColorStrengthProp))
                m.SetFloat(
                    OutlineZOffsetVertexColorStrengthProp,
                    Mathf.Clamp01(OutlineZOffsetVertexColorStrength)
                );

            if (m.HasProperty(OutlineUseVertexColorNormalsProp))
                m.SetFloat(
                    OutlineUseVertexColorNormalsProp,
                    OutlineUseVertexColorNormals ? 1f : 0f
                );
            if (m.HasProperty(OutlineVertexColorMaskProp))
                m.SetFloat(OutlineVertexColorMaskProp, (int)OutlineVertexColorMask);
            if (m.HasProperty(OutlineVertexColorMaskStrengthProp))
                m.SetFloat(
                    OutlineVertexColorMaskStrengthProp,
                    Mathf.Clamp01(OutlineVertexColorMaskStrength)
                );

            if (m.HasProperty(OutlineClipAtZeroWidthProp))
                m.SetFloat(OutlineClipAtZeroWidthProp, OutlineClipAtZeroWidth ? 1f : 0f);
            if (m.HasProperty(OutlineOverrideAlphaProp))
                m.SetFloat(OutlineOverrideAlphaProp, OutlineOverrideAlpha ? 1f : 0f);
            if (m.HasProperty(OutlineCullProp))
                m.SetFloat(OutlineCullProp, (float)(int)OutlineCull);
            if (m.HasProperty(OutlineZWriteProp))
                m.SetFloat(OutlineZWriteProp, OutlineZWrite ? 1f : 0f);
            if (m.HasProperty(OutlineZTestProp))
                m.SetFloat(OutlineZTestProp, (float)(int)OutlineZTest);
        }

        private IEnumerable<Renderer> ResolveRenderers()
        {
            if (SkinnedMesh != null)
            {
                yield return SkinnedMesh;
                yield break;
            }

            if (StaticMeshRenderer != null)
            {
                yield return StaticMeshRenderer;
                yield break;
            }

            if (GetMeshAutocompleteParentRef() == null)
                yield break;

            if (!string.IsNullOrWhiteSpace(TargetSkinnedMeshKey))
            {
                var key = TargetSkinnedMeshKey.Trim();
                var smrs = GetParentSkinnedMeshRenderers();
                if (smrs != null && smrs.TryGetValue(key, out var smr) && smr != null)
                {
                    yield return smr;
                    yield break;
                }

                var mrs = GetParentMeshRenderers();
                if (mrs != null && mrs.TryGetValue(key, out var mr) && mr != null)
                {
                    yield return mr;
                    yield break;
                }

                yield break;
            }

            var dictSm = GetParentSkinnedMeshRenderers();
            if (dictSm != null)
            {
                foreach (var kv in dictSm)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }

            var dictMr = GetParentMeshRenderers();
            if (dictMr != null)
            {
                foreach (var kv in dictMr)
                {
                    if (kv.Value != null)
                        yield return kv.Value;
                }
            }
        }
    }

#if !NODE68_SHARE_BUILD
    /// <summary>
    /// Poiyomi 조명·림·셰도우 등을 <see cref="CharacterAsset"/> 메시 머티리얼에 적용합니다.
    /// 프롭은 <see cref="PoiyomiLightingControlPropAsset"/> 을 사용하세요.
    /// </summary>
    [AssetType(
        Id = "9e3c5a7b-1d2f-4e6a-8b0c-1a2b3c4d5e6f",
        Title = "Poiyomi 조명 제어_Node68 (캐릭터)",
        Category = BpToolkitCategories.Toolkit,
        CategoryOrder = -62
    )]
    public sealed class PoiyomiLightingControlAsset : PoiyomiLightingShaderControlBase
    {
        /// <summary>
        /// Warudo는 <see cref="DataInputAttribute"/> 기본 순서가 줄 번호라, 이 필드가 파일 하단에 있으면 인스펙터 맨 아래로 밀립니다.
        /// 명시 순서로 「에셋 활성」 직후·스킨 메시 키보다 위에 두어 먼저 연결하도록 합니다.
        /// </summary>
        [DataInput(726)]
        [Label("캐릭터")]
        [Description(
            "캐릭터 에셋(Character 1 등)을 연결하면 해당 캐릭터의 SkinnedMeshRenderer·MeshRenderer가 메시 키 자동완성·순회 대상이 됩니다. "
                + "프롭은 「Poiyomi 조명 제어_Node68 (프롭)」 에셋을 사용하세요."
        )]
        public CharacterAsset Character;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(Character), OnInputChanged);
        }

        protected override Dictionary<
            string,
            SkinnedMeshRenderer
        > GetParentSkinnedMeshRenderers() => Character?.SkinnedMeshRenderers;

        protected override Dictionary<string, MeshRenderer> GetParentMeshRenderers() =>
            Character?.MeshRenderers;

        protected override object GetMeshAutocompleteParentRef() => Character;

        protected override string GetParentRequiredAutocompleteMessage() =>
            "캐릭터 에셋을 먼저 선택하세요.";
    }

    /// <summary>
    /// <see cref="PropAsset"/> 전용입니다. 동작과 Poiyomi 옵션은 <see cref="PoiyomiLightingControlAsset"/> (캐릭터) 와 같은 베이스를 씁니다.
    /// </summary>
    [AssetType(
        Id = "f8a93c12-47b6-41e9-a73d-c8e2914bcf01",
        Title = "Poiyomi 조명 제어_Node68 (프롭)",
        Category = BpToolkitCategories.Toolkit,
        CategoryOrder = -61
    )]
    public sealed class PoiyomiLightingControlPropAsset : PoiyomiLightingShaderControlBase
    {
        [DataInput(726)]
        [Label("프롭")]
        [Description(
            "소품(Prop) 에셋을 연결하면 해당 프롭의 SkinnedMesh·MeshRenderer 메시가 대상입니다."
        )]
        public PropAsset Prop;

        protected override void WatchMeshParentAsset()
        {
            Watch(nameof(Prop), OnInputChanged);
        }

        protected override Dictionary<
            string,
            SkinnedMeshRenderer
        > GetParentSkinnedMeshRenderers() => Prop?.SkinnedMeshRenderers;

        protected override Dictionary<string, MeshRenderer> GetParentMeshRenderers() =>
            Prop?.MeshRenderers;

        protected override object GetMeshAutocompleteParentRef() => Prop;

        protected override string GetParentRequiredAutocompleteMessage() =>
            "프롭 에셋을 먼저 선택하세요.";
    }
#endif
}
