Shader "Hidden/Locked/.poiyomi/Poiyomi Pro/e577fce2748fef84d85fbb0d29a98dc8"
{
	Properties
	{
		[HideInInspector] shader_master_label ("<color=#E75898ff>Poiyomi 10.0.10</color>", Float) = 0
		[HideInInspector] shader_is_using_thry_editor ("", Float) = 0
		[HideInInspector] shader_locale ("0db0b86376c3dca4b9a6828ef8615fe0", Float) = 0
		[HideInInspector] footer_website ("{texture:{name:icon-poilogo,height:24},action:{type:URL,data:https://www.poiyomi.com},hover:WEBSITE}", Float) = 0
		[HideInInspector] footer_discord ("{texture:{name:icon-discord,height:24},action:{type:URL,data:https://discord.gg/Ays52PY},hover:DISCORD}", Float) = 0
		[HideInInspector] footer_patreon ("{texture:{name:icon-patreon-new,height:24},action:{type:URL,data:https://www.patreon.com/poiyomi},hover:PATREON}", Float) = 0
		[HideInInspector] footer_youtube ("{texture:{name:icon-youtube,height:24},action:{type:URL,data:https://www.youtube.com/poiyomi},hover:YOUTUBE}", Float) = 0
		[HideInInspector] footer_github ("{texture:{name:icon-github,height:24},action:{type:URL,data:https://github.com/poiyomi/PoiyomiToonShader},hover:GITHUB}", Float) = 0
		[HideInInspector] footer_bluesky ("{texture:{name:icon-bluesky,height:24},action:{type:URL,data:https://bsky.app/profile/poiyomi.com},hover:BLUESKY}", Float) = 0
		[HideInInspector] footer_twitter ("{texture:{name:icon-twitter,height:24},action:{type:URL,data:https://x.com/poiyomi},hover:X}", Float) = 0
		[Header(POIYOMI SHADER UI FAILED TO LOAD)]
		[Header(.    This is caused by scripts failing to compile. It can be fixed.)]
		[Header(.          The inspector will look broken and will not work properly until fixed.)]
		[Header(.    Please check your console for script errors.)]
		[Header(.          You can filter by errors in the console window.)]
		[Header(.          Often the topmost error points to the erroring script.)]
		[Space(30)][Header(Common Error Causes)]
		[Header(.    Forgot to install Poiyomi Shaders)]
		[Header(.          Make sure you follow the instructions on our Documentation.)]
		[Header(.    Installing multiple Poiyomi Shader packages)]
		[Header(.          Make sure to delete the Poiyomi shader folder before you update Poiyomi.)]
		[Header(.          If a package came with Poiyomi this is bad practice and can cause issues.)]
		[Header(.          Delete the package and import it without any Poiyomi components.)]
		[Header(.    Bad VRCSDK installation (e.g. Both VCC and Standalone))]
		[Header(.          Delete the VRCSDK Folder in Assets if you are using the VCC.)]
		[Header(.          Avoid using third party SDKs. They can cause incompatibility.)]
		[Header(.    Script Errors in other scripts)]
		[Header(.          Outdated tools or prefabs can cause this.)]
		[Header(.          Update things that are throwing errors or move them outside the project.)]
		[Space(30)][Header(Visit Our Discord to Ask For Help)]
		[Space(5)]_ShaderUIWarning0 (" → discord.gg/poiyomi ←    We can help you get it fixed!                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         --{condition_showS:(0==1)}", Int) = -0
		[Space(1400)][Header(POIYOMI SHADER UI FAILED TO LOAD)]
		_ShaderUIWarning1 ("Please scroll up for more information!                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     --{condition_showS:(0==1)}", Int) = -0
		[HideInInspector] _ForgotToLockMaterial (";;YOU_FORGOT_TO_LOCK_THIS_MATERIAL;", Int) = 1
		[ThryShaderOptimizerLockButton] _ShaderOptimizerEnabled ("", Int) = 1
		[HideInInspector] GeometryShader_Enabled("GEOMETRY SHADER ENABLED", Float) = 1
		[HideInInspector] Tessellation_Enabled("TESSELLATION ENABLED", Float) = 1
		[ThryWideEnum(Opaque, 0, Cutout, 1, TransClipping, 9, Fade, 2, Transparent, 3, Additive, 4, Soft Additive, 5, Multiplicative, 6, 2x Multiplicative, 7)]_Mode("Rendering Preset--{on_value_actions:[
		{value:0,actions:[{type:SET_PROPERTY,data:render_queue=2000},{type:SET_PROPERTY,data:_AlphaForceOpaque=1}, {type:SET_PROPERTY,data:render_type=Opaque},            {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=0}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:1,actions:[{type:SET_PROPERTY,data:render_queue=2450},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=TransparentCutout}, {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=.5}, {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:9,actions:[{type:SET_PROPERTY,data:render_queue=2460},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=TransparentCutout}, {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0.01},  {type:SET_PROPERTY,data:_SrcBlend=5}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=5}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=5}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:2,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0.002},  {type:SET_PROPERTY,data:_SrcBlend=5}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=5}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=5}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:3,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=1}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:4,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=1},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=1},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:5,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=4}, {type:SET_PROPERTY,data:_DstBlend=1},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=4}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=4}, {type:SET_PROPERTY,data:_OutlineDstBlend=1},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:6,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=2}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=2}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=2}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:7,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=2}, {type:SET_PROPERTY,data:_DstBlend=3},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=2}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=2}, {type:SET_PROPERTY,data:_OutlineDstBlend=3},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]}
		]}", Int) = 0
		[HideInInspector] m_mainCategory ("Color & Normals--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/},hover:Documentation}}", Float) = 0
		_Color ("Color & Alpha--{reference_property:_ColorThemeIndex}", Color) = (1, 1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _ColorThemeIndex ("Color Theme Index", Int) = 0
		[sRGBWarning(true)]_MainTex ("Texture--{reference_properties:[_MainTexPan, _MainTexUV, _MainPixelMode, _MainTexStochastic, _MainIgnoreTexAlpha]}", 2D) = "white" { }
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _MainTexUV ("UV", Int) = 0
		[ThryHideInInspector][Vector2]_MainTexPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ToggleUI]_MainPixelMode ("Pixel Mode", Float) = 0
		[ThryHideInInspector][ToggleUI]_MainTexStochastic ("Stochastic Sampling", Float) = 0
		[ThryHideInInspector][ToggleUI]_MainIgnoreTexAlpha ("Ignore Alpha", Float) = 0
		[sRGBWarning]_AlphaMask ("Alpha Map--{reference_properties:[_AlphaMaskPan, _AlphaMaskUV, _AlphaMaskInvert, _MainAlphaMaskMode, _AlphaMaskBlendStrength, _AlphaMaskValue, _AlphaMaskR, _AlphaMaskG, _AlphaMaskB, _AlphaMaskA, _AlphaMaskChannelBlendMode, _AlphaMaskMinMax, _AlphaMaskGamma], alts:[_AlphaMap]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_AlphaMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _AlphaMaskUV ("UV", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Off, 0, Replace, 1, Multiply, 2, Add, 3, Subtract, 4)]_MainAlphaMaskMode ("Blend Mode", Int) = 2
		[ThryHideInInspector]_AlphaMaskR ("R Channel", Range(0, 1)) = 1
		[ThryHideInInspector]_AlphaMaskG ("G Channel", Range(0, 1)) = 0
		[ThryHideInInspector]_AlphaMaskB ("B Channel", Range(0, 1)) = 0
		[ThryHideInInspector]_AlphaMaskA ("A Channel", Range(0, 1)) = 0
		[ThryHideInInspector][ThryWideEnum(Replace, 0, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Add, 7)]_AlphaMaskChannelBlendMode ("Channel Blend", Int) = 2
		[ThryHideInInspector][MultiSlider]_AlphaMaskMinMax ("Min Max", Vector) = (0, 1, 0, 1)
		[ThryHideInInspector]_AlphaMaskGamma ("Gamma", Range(0.01, 5)) = 1
		[ThryHideInInspector]_AlphaMaskBlendStrength ("Blend Strength", Float) = 1
		[ThryHideInInspector]_AlphaMaskValue ("Blend Offset", Float) = 0
		[ThryHideInInspector][ToggleUI]_AlphaMaskInvert ("Invert", Float) = 0
		[Normal]_BumpMap ("Normal Map--{reference_property:_BumpScale, reference_properties:[_BumpMapPan, _BumpMapUV, _BumpMapStochastic]}", 2D) = "bump" { }
		[ThryHideInInspector]_BumpScale ("Intensity", Range(0, 10)) = 1
		[ThryHideInInspector][Vector2]_BumpMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _BumpMapUV ("UV", Int) = 0
		[ThryHideInInspector][ToggleUI]_BumpMapStochastic ("Stochastic Sampling", Float) = 0
		_Cutoff ("Alpha Cutoff", Range(0, 1.001)) = 0.5
		[HideInInspector] m_start_DetailOptions (" Detail Normals & Texture--{reference_property:_DetailEnabled,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/details},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(FINALPASS)]_DetailEnabled ("Enable", Float) = 0
		[sRGBWarning][ThryRGBAPacker(R Texture Mask, G Normal Mask, B Nothing, A Nothing, linear, false)]_DetailMask ("Detail Mask (Expand)--{reference_properties:[_DetailMaskPan, _DetailMaskUV, _DetailMaskStochastic]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_DetailMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _DetailMaskUV ("UV", Int) = 0
		[ThryHideInInspector][ToggleUI]_DetailMaskStochastic ("Stochastic Sampling", Float) = 0
		[HideInInspector] s_start_DetailTexture ("Detail Texture--{persistent_expand:false,default_expand:true}", Float) = 0
		_DetailTint ("Tint--{reference_property:_DetailTintThemeIndex}", Color) = (1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _DetailTintThemeIndex ("", Int) = 0
		[sRGBWarning(true)]_DetailTex ("Detail Texture--{reference_properties:[_DetailTexPan, _DetailTexUV, _DetailTexStochastic]}", 2D) = "gray" { }
		[ThryHideInInspector][Vector2]_DetailTexPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _DetailTexUV ("UV", Int) = 0
		[ThryHideInInspector][ToggleUI]_DetailTexStochastic ("Stochastic Sampling", Float) = 0
		_DetailTexIntensity ("Intensity", Range(0, 10)) = 1
		_DetailBrightness ("Brightness", Range(0, 2)) = 1
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _DetailTexGlobalMask ("Global Mask--{reference_property:_DetailTexGlobalMaskBlendType}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_DetailTexGlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HideInInspector] s_end_DetailTexture ("Detail Texture", Float) = 0
		[HideInInspector] s_start_DetailNormal ("Detail Normal--{persistent_expand:false,default_expand:true}", Float) = 0
		[Normal]_DetailNormalMap ("Detail Normal--{reference_properties:[_DetailNormalMapPan, _DetailNormalMapUV, _DetailNormalMapScale, _DetailNormalMapStochastic]}", 2D) = "bump" { }
		[ThryHideInInspector]_DetailNormalMapScale ("Intensity", Range(0, 10)) = 1
		[ThryHideInInspector][Vector2]_DetailNormalMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _DetailNormalMapUV ("UV", Int) = 0
		[ThryHideInInspector][ToggleUI]_DetailNormalMapStochastic ("Stochastic Sampling", Float) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _DetailNormalGlobalMask ("Global Mask--{reference_property:_DetailNormalGlobalMaskBlendType}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_DetailNormalGlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HideInInspector] s_end_DetailNormal ("Detail Normal", Float) = 0
		[HideInInspector] m_end_DetailOptions ("Details", Float) = 0
		[HideInInspector] m_start_Alpha ("Alpha Options--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/alpha-options},hover:Documentation}}", Float) = 0
		[ToggleUI]_AlphaForceOpaque ("Force Opaque", Float) = 1
		_AlphaMod ("Alpha Mod", Range(-1, 1)) = 0.0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _AlphaGlobalMask ("Global Mask--{reference_property:_AlphaGlobalMaskBlendType}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _AlphaGlobalMaskBlendType ("Blending", Int) = 2
		[HideInInspector] m_end_Alpha ("Alpha Options", Float) = 0
		[HideInInspector] m_lightingCategory ("Shading", Float) = 0
		[HideInInspector] m_start_PoiLightData ("Light Data--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/shading/light-data},hover:Documentation}}", Float) = 0
		[TextureKeyword][sRGBWarning][ThryRGBAPacker(R, G, B, A, Linear, false)]_LightingAOMaps ("AO Maps (expand)--{reference_properties:[_LightingAOMapsPan, _LightingAOMapsUV,_LightDataAOStrengthR,_LightDataAOStrengthG,_LightDataAOStrengthB,_LightDataAOStrengthA, _LightDataAOGlobalMaskR]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_LightingAOMapsPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _LightingAOMapsUV ("UV", Int) = 0
		[ThryHideInInspector]_LightDataAOStrengthR ("R Strength", Range(0, 1)) = 1
		[ThryHideInInspector]_LightDataAOStrengthG ("G Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightDataAOStrengthB ("B Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightDataAOStrengthA ("A Strength", Range(0, 1)) = 0
		[ThryHideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataAOGlobalMaskR ("Global Mask--{reference_property:_LightDataAOGlobalMaskBlendTypeR}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataAOGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		[TextureKeyword][sRGBWarning][ThryRGBAPacker(R, G, B, A, Linear, false)]_LightingDetailShadowMaps ("Shadow Map (expand)--{reference_properties:[_LightingDetailShadowMapsPan, _LightingDetailShadowMapsUV,_LightingDetailShadowStrengthR,_LightingDetailShadowStrengthG,_LightingDetailShadowStrengthB,_LightingDetailShadowStrengthA,_LightingAddDetailShadowStrengthR,_LightingAddDetailShadowStrengthG,_LightingAddDetailShadowStrengthB,_LightingAddDetailShadowStrengthA, _LightDataDetailShadowGlobalMaskR]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_LightingDetailShadowMapsPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _LightingDetailShadowMapsUV ("UV", Int) = 0
		[ThryHideInInspector]_LightingDetailShadowStrengthR ("R Strength", Range(0, 1)) = 1
		[ThryHideInInspector]_LightingDetailShadowStrengthG ("G Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingDetailShadowStrengthB ("B Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingDetailShadowStrengthA ("A Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingAddDetailShadowStrengthR ("Additive R Strength", Range(0, 1)) = 1
		[ThryHideInInspector]_LightingAddDetailShadowStrengthG ("Additive G Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingAddDetailShadowStrengthB ("Additive B Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingAddDetailShadowStrengthA ("Additive A Strength", Range(0, 1)) = 0
		[ThryHideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataDetailShadowGlobalMaskR ("Global Mask--{reference_property:_LightDataDetailShadowGlobalMaskBlendTypeR}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataDetailShadowGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		[TextureKeyword][sRGBWarning][ThryRGBAPacker(R, G, B, A, Linear, false)]_LightingShadowMasks ("Shadow Masks (expand)--{reference_properties:[_LightingShadowMasksPan, _LightingShadowMasksUV,_LightingShadowMaskStrengthR,_LightingShadowMaskStrengthG,_LightingShadowMaskStrengthB,_LightingShadowMaskStrengthA, _LightDataShadowMaskGlobalMaskR]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_LightingShadowMasksPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _LightingShadowMasksUV ("UV", Int) = 0
		[ThryHideInInspector]_LightingShadowMaskStrengthR ("R Strength", Range(0, 1)) = 1
		[ThryHideInInspector]_LightingShadowMaskStrengthG ("G Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingShadowMaskStrengthB ("B Strength", Range(0, 1)) = 0
		[ThryHideInInspector]_LightingShadowMaskStrengthA ("A Strength", Range(0, 1)) = 0
		[ThryHideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataShadowMaskGlobalMaskR ("Global Mask--{reference_property:_LightDataShadowMaskGlobalMaskBlendTypeR}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataShadowMaskGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		[HideInInspector] s_start_LightDataBasePass ("Base Pass (Directional & Baked Lights)--{persistent_expand:true,default_expand:true}", Float) = 1
		[Enum(Poi Custom, 0, Standard, 1, UTS2, 2, OpenLit(lil toon), 3)] _LightingColorMode ("Light Color Mode", Int) = 0
		[Enum(Poi Custom, 0, Normalized NDotL, 1, Saturated NDotL, 2, Casted Shadows Only, 3, SDF, 4)] _LightingMapMode ("Light Map Mode", Int) = 0
		[HideInInspector] s_start_LightDataSDF ("Signed Distance fields--{persistent_expand:true,default_expand:true, condition_showS:(_LightingMapMode==4)}", Float) = 1
		[TextureKeyword][sRGBWarning][ThryRGBAPacker(R, G, Nothing, Nothing, Linear, false)]_LightDataSDFMap ("SDF Map (expand)--{reference_properties:[_LightDataSDFMapPan, _LightDataSDFMapUV]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_LightDataSDFMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _LightDataSDFMapUV ("UV", Int) = 0
		_LightDataSDFMapLOD ("LOD", Range(0, 1)) = 0
		_LightDataSDFBlendY ("Blend Y Direction", Range(0.001, 2)) = 1
		[HideInInspector] s_end_LightDataSDF ("Signed Distance fields", Float) = 1
		[Enum(Poi Custom, 0, Forced Local Direction, 1, Forced World Direction, 2, UTS2, 3, OpenLit(lil toon), 4, View Direction, 5)] _LightingDirectionMode ("Light Direction Mode", Int) = 0
		[Vector3]_LightngForcedDirection ("Forced Direction--{condition_showS:(_LightingDirectionMode==1 || _LightingDirectionMode==2)}", Vector) = (0, 0, 0)
		_LightingViewDirOffsetPitch ("View Dir Offset Pitch--{condition_showS:_LightingDirectionMode==5}", Range(-90, 90)) = 0
		_LightingViewDirOffsetYaw ("View Dir Offset Yaw--{condition_showS:_LightingDirectionMode==5}", Range(-90, 90)) = 0
		[ToggleUI]_LightingForceColorEnabled ("Force Light Color", Float) = 0
		_LightingForcedColor ("Forced Color--{condition_showS:(_LightingForceColorEnabled==1), reference_property:_LightingForcedColorThemeIndex}", Color) = (1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _LightingForcedColorThemeIndex ("", Int) = 0
		_Unlit_Intensity ("Unlit_Intensity--{condition_showS:(_LightingColorMode==2)}", Range(0.001, 4)) = 1
		[DoNotLock][ToggleUI]_LightingCapEnabled ("Limit Brightness", Float) = 1
		[DoNotLock]_LightingCap ("Max Brightness--{condition_showS:(_LightingCapEnabled==1)}", Range(0, 10)) = 1
		[DoNotLock]_LightingMinLightBrightness ("Min Brightness", Range(0, 1)) = 0
		_LightingIndirectUsesNormals ("Indirect Uses Normals--{condition_showS:(_LightingColorMode==0)}", Range(0, 1)) = 0
		_LightingCastedShadows ("Receive Casted Shadows", Range(0, 1)) = 0
		[DoNotLock]_LightingMonochromatic ("Grayscale Lighting", Range(0, 1)) = 0
		[ToggleUI]_LightingVertexLightingEnabled ("Vertex lights (Non-Important)", Float) = 1
		[ToggleUI]_LightingMirrorVertexLightingEnabled ("Mirror Vertex lights (Non-Important)", Float) = 1
		[ToggleUI]_LightingEnableLightVolumes ("Light Volumes", Float) = 1
		_LightingLightVolumesNormalBias ("Normal Bias Sampling--{condition_showS:(_LightingEnableLightVolumes==1)}", Range(0, 1)) = 0
		[HideInInspector] s_end_LightDataBasePass ("Base Pass", Float) = 1
		[HideInInspector] s_start_LightDataAddPass ("Add Pass (Point & Spot lights)--{persistent_expand:true,default_expand:true}", Float) = 1
		[ToggleUI]_LightingAdditiveEnable ("Pixel lights (Important)", Float) = 1
		[ToggleUI]_DisableDirectionalInAdd ("Ignore Directional Lights--{condition_showS:(_LightingAdditiveEnable==1)}", Float) = 1
		[ToggleUI]_LightingAdditiveLimited ("Limit Brightness", Float) = 1
		_LightingAdditiveLimit ("Max Brightness--{condition_showS:(_LightingAdditiveLimited==1)}", Range(0, 10)) = 1
		_LightingAdditiveCastedShadows ("Receive Casted Shadows", Range(0, 1)) = 1
		_LightingAdditiveMonochromatic ("Grayscale Lighting", Range(0, 1)) = 0
		_LightingAdditivePassthrough ("Point Light Passthrough--{condition_showS:(_LightingAdditiveEnable==1)}", Range(0, 1)) = .5
		[HideInInspector] s_end_LightDataAddPass ("Add Pass", Float) = 1
		[HideInInspector] s_start_LightDataDebug ("Debug / Data Visualizations--{reference_property:_LightDataDebugEnabled,persistent_expand:true}", Float) = 0
		[HideInInspector][DoNotAnimate][ThryToggleUI(false)]_LightDataDebugEnabled ("Debug", Float) = 0
		[ThryWideEnum(Direct Color, 0, Indirect Color, 1, Light Map, 2, Attenuation, 3, N Dot L, 4, Half Dir, 5, Direction, 6, Add Color, 7, Add Attenuation, 8, Add Shadow, 9, Add N Dot L, 10)] _LightingDebugVisualize ("Visualize", Int) = 0
		[HideInInspector] s_end_LightDataDebug ("Debug", Float) = 0
		[HideInInspector] m_end_PoiLightData ("Light Data", Float) = 0
		[HideInInspector] m_start_depthRimLightOptions (" Depth Rim Lighting--{reference_property:_EnableDepthRimLighting,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/shading/depth-rim-lighting},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(_POI_DEPTH_RIMLIGHT)]_EnableDepthRimLighting ("", Float) = 0
		[Helpbox(1, 2)]_DepthRimDepthGetWarning ("Depth Rim Lighting Requires depth. Put the DepthGet.prefab on your avatar so it'll work in every world.", Int) = 0
		[PoiPrefabSpawner(23c61f8f4d0a87243a36c937bd3e1393)] _DepthRimSpawnDepthGet ("Add DepthGet Prefab to Scene", Float) = 0
		[Enum(vertex, 0, pixel, 1)] _DepthRimNormalToUse ("Normal To Use", Int) = 1
		[ThryWideEnum(Two Samples, 0, Four Samples, 1, Eight Samples, 2)] _DepthRimType ("Rim Type", Int) = 2
		[HideInInspector] s_start_FixedRimShapeControl ("Shape Control--{persistent_expand:true,default_expand:true}", Float) = 1
		_DepthRimWidth ("Width", Range(0, 1)) = 0.08
		_DepthRimSharpness ("Sharpness--{condition_showS:(_DepthRimBinary==0)}", Range(0, 1)) = .5
		_DepthRimDepthThreshold ("Depth Threshold", Range(0.001, 1)) = 0.25
		[ToggleUI]_DepthRimBinary ("Binary", Float) = 1
		_DepthRimCameraClip ("Fixed Size Threshold", Float) = .5
		_DepthRimMinDistance ("Min Distance", Float) = 0
		_DepthRimMaxDistance ("Max Distance", Float) = 0
		[HideInInspector] s_end_FixedRimShapeControl ("Shape Control", Float) = 0
		[HideInInspector] s_start_FixedRimLightDir ("Light Direction--{persistent_expand:true,default_expand:false}", Float) = 0
		[Enum(NdotL, 0, Rim Dot Light, 1)] _DepthRimLightDirMethod ("Method", Int) = 0
		_DepthRimHideInShadow ("Map to Light Direction", Range(0, 1)) = 0
		_DepthRimShadowMask ("Mix Attenuation", Range(0, 1)) = 0
		_DepthRimMixRampedLightMap ("Mix Ramped Light Map", Range(0, 1)) = 0
		[HideInInspector] s_end_FixedRimLightDir ("Light Direction", Float) = 0
		[HideInInspector] s_start_FixedRimMasking ("Masking--{persistent_expand:true,default_expand:false}", Float) = 0
		[TextureNoSO]_DepthRimMask ("Mask--{reference_properties:[_DepthRimMaskPan, _DepthRimMaskUV]}", 2D) = "white" {}
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _DepthRimMaskUV ("UV", Int) = 0
		[ThryHideInInspector][Vector2]_DepthRimMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[Enum(R, 0, G, 1, B, 2, A, 3)]_DepthRimMaskChannel ("Channel", Float) = 0
		[HideInInspector] s_end_FixedRimMasking ("Masking", Float) = 0
		[HideInInspector] s_start_FixedRimColor ("Color--{persistent_expand:true,default_expand:true}", Float) = 1
		_DepthRimMixBaseColor ("Use Base Color", Range(0, 1)) = 0
		_DepthRimMixLightColor ("Light Color Mix", Range(0, 1)) = 0
		_DepthRimColor ("Rim Color--{reference_property:_DepthRimColorThemeIndex}", Color) = (1, 1, 1, 1)
		_DepthRimBrightness ("Color Brightness", Range(0, 10)) = 1
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _DepthRimColorThemeIndex ("", Int) = 0
		_DepthRimEmission ("Emission", Range(0, 20)) = 0
		[HideInInspector] s_end_FixedRimColor ("Color", Float) = 0
		[HideInInspector] s_start_FixedRimBlending ("Blending--{persistent_expand:true,default_expand:true}", Float) = 1
		_DepthRimReplace ("Replace", Range(0, 1)) = 0
		_DepthRimAdd ("Add", Range(0, 1)) = 0
		_DepthRimScreen ("Screen", Range(0, 1)) = 1
		_DepthRimMultiply ("Multiply", Range(0, 1)) = 0
		_DepthRimAdditiveLighting ("Unlit Add", Range(0, 1)) = 0
		[HideInInspector] s_end_FixedRimBlending ("Blending", Float) = 0
		[HideInInspector] m_end_depthRimLightOptions ("Rim Lighting", Float) = 0
		[HideInInspector] m_start_stylizedSpec (" Stylized Reflections--{reference_property:_StylizedSpecular,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/shading/stylized-reflections},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_STYLIZED_StylizedSpecular)]_StylizedSpecular ("Enable", Float) = 0
		[DoNotAnimate][KeywordEnum(UnityChan, lilToon)]_StylizedReflectionMode ("Mode", Float) = 0
		[HideInInspector] s_start_StylizedReflectionMode0 ("Unity Chan Specular--{persistent_expand:true,default_expand:true, condition_showS:(_StylizedReflectionMode==0)}", Float) = 1
		[sRGBWarning(true)][ThryTexture]_HighColor_Tex ("Specular Map--{reference_properties:[_HighColor_TexPan, _HighColor_TexUV]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_HighColor_TexPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)]_HighColor_TexUV ("UV", Int) = 0
		_HighColor ("Tint--{reference_property:_HighColorThemeIndex}", Color) = (1, 1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _HighColorThemeIndex ("", Int) = 0
		[sRGBWarning]_Set_HighColorMask ("Mask--{reference_properties:[_Set_HighColorMaskPan, _Set_HighColorMaskUV, _Set_HighColorMaskChannel, _Tweak_HighColorMaskLevel, _StylizedSpecularInvertMask]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_Set_HighColorMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)]_Set_HighColorMaskUV ("UV", Int) = 0
		[ThryHideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_Set_HighColorMaskChannel ("Channel", Float) = 1
		[ThryHideInInspector]_Tweak_HighColorMaskLevel ("Mask Level", Range(-1, 1)) = 0
		[ThryHideInInspector][ToggleUI]_StylizedSpecularInvertMask ("Invert Mask", Float) = 0
		[ThryWideEnum(Toon, 0, Realistic, 1)]_Is_SpecularToHighColor ("Specular Mode", Float) = 0
		[ThryWideEnum(Replace, 0, Add, 1, Screen, 2, Multiply, 3)]_Is_BlendAddToHiColor ("Color Blend Mode", Int) = 0
		_StylizedSpecularStrength ("Strength", Float) = 1
		[ToggleUI] _UseLightColor ("Use Light Color", Float) = 1
		_StylizedSpecularNormalStrength ("Normal Strength", Range(0, 1)) = 1
		[HideInInspector] s_start_StylizedSpecularLayer0 ("Layer 1--{persistent_expand:true,default_expand:true}", Float) = 1
		_HighColor_Power ("Size", Range(0, 1)) = 0.2
		_StylizedSpecularFeather ("Feather--{condition_showS:(_Is_SpecularToHighColor==0)}", Range(0, 1)) = 0
		_Layer1Strength ("Strength", Range(0, 1)) = 1
		[HideInInspector] s_end_StylizedSpecularLayer0 ("Layer 1", Float) = 0
		[HideInInspector] s_start_StylizedSpecularLayer1 ("Layer 2--{persistent_expand:true,default_expand:true}", Float) = 1
		_Layer2Size ("Size", Range(0, 1)) = 0
		_StylizedSpecular2Feather ("Feather--{condition_showS:(_Is_SpecularToHighColor==0)}", Range(0, 1)) = 0
		_Layer2Strength ("Strength", Range(0, 1)) = 0
		[HideInInspector] s_end_StylizedSpecularLayer1 ("Layer 2", Float) = 0
		[HideInInspector] s_start_StylizedSpecularAdvanced ("Advanced--{persistent_expand:true,default_expand:true}", Float) = 0
		[ToggleUI] _StylizedSpecularIgnoreNormal ("Show on back", Float) = 0
		[ToggleUI] _StylizedSpecularIgnoreShadow ("Ignore Shadow", Float) = 0
		[ToggleUI]_SSIgnoreCastedShadows ("Ignore Casted Shadows", Float) = 0
		[HideInInspector] s_end_StylizedSpecularAdvanced ("Advanced", Float) = 0
		[HideInInspector] s_end_StylizedReflectionMode0 ("", Float) = 1
		[HideInInspector] s_start_StylizedReflectionMode1 ("Lil Reflections--{persistent_expand:true,default_expand:true, condition_showS:(_StylizedReflectionMode==1)}", Float) = 1
		[ToggleUI] _UseReflection ("sReflection", Int) = 1
		[NoScaleOffset] _SmoothnessTex ("Smoothness--{reference_property:_Smoothness}", 2D) = "white" { }
		[ThryHideInInspector]_Smoothness ("Smoothness", Range(0, 1)) = 1
		_GSAAStrength ("GSAA", Range(0, 1)) = 0
		[NoScaleOffset] _MetallicGlossMap ("Metallic--{reference_property:_Metallic}", 2D) = "white" { }
		[ThryHideInInspector]_Metallic ("Metallic", Range(0, 1)) = 0
		[NoScaleOffset] _ReflectionColorTex ("Color / Mask--{reference_property:_ReflectionColor}", 2D) = "white" { }
		[ThryHideInInspector][HDR] _ReflectionColor ("Color", Color) = (1, 1, 1, 1)
		_Reflectance ("Reflectance", Range(0, 1)) = 0.04
		[HideInInspector] ss_start_StylizedReflectionLilSpecular ("Specular--{persistent_expand:true,default_expand:true,reference_property:_ApplySpecular}", Float) = 1
		[ThryWideEnum(Realistic, 0, Toon, 1)] _SpecularToon ("Specular Mode", Int) = 1
		[ThryHideInInspector][ToggleUI] _ApplySpecular ("Apply Specular", Int) = 1
		_SpecularNormalStrength ("Normal Map Strength", Range(0, 1)) = 1.0
		_SpecularBorder ("Border--{condition_showS:(_SpecularToon==1)}", Range(0, 1)) = 0.5
		_SpecularBlur ("Blur--{condition_showS:(_SpecularToon==1)}", Range(0, 1)) = 0.0
		[ToggleUI] _ApplySpecularFA ("Multi Light Specular", Int) = 1
		[HideInInspector] ss_end_StylizedReflectionLilSpecular ("", Float) = 1
		[HideInInspector] ss_start_StylizedReflectionLilEnviroReflections ("Environment Reflections--{persistent_expand:true,default_expand:true,reference_property:_ApplyReflection}", Float) = 1
		[ThryHideInInspector][ToggleUI] _ApplyReflection ("Enviro Reflections", Int) = 0
		_ReflectionNormalStrength ("Normal Map Strength", Range(0, 1)) = 1.0
		_ReflectionCubeTex ("Cubemap Fallback--{reference_property:_ReflectionCubeColor}", Cube) = "black" { }
		[ThryHideInInspector][HDR] _ReflectionCubeColor ("Color", Color) = (0, 0, 0, 1)
		[ToggleUI] _ReflectionCubeOverride ("Override", Int) = 0
		_ReflectionCubeEnableLighting ("Enable Lighting (Fallback)", Range(0, 1)) = 1
		[HideInInspector] ss_end_StylizedReflectionLilEnviroReflections ("", Float) = 1
		[ThrySeperator(2, 5, 5)]
		[ToggleUI] _ReflectionApplyTransparency ("Apply Transparency", Int) = 1
		[ThryWideEnum(Replace, 0, Add, 1, Screen, 2, Multiply, 3)] _ReflectionBlendMode ("Blending Mode", Int) = 1
		[HideInInspector] s_end_StylizedReflectionMode1 ("", Float) = 1
		[HideInInspector] m_end_stylizedSpec ("", Float) = 0
		[HideInInspector] m_OutlineCategory (" Outlines--{reference_property:_EnableOutlines,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/outlines/},hover:Documentation}}", Float) = 0
		[HideInInspector] m_specialFXCategory ("Special FX", Float) = 0
		[HideInInspector] m_start_emission1Options (" Emission 1--{reference_property:_EnableEmission1,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/emission},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_EMISSION_1)]
		_EnableEmission1 ("Enable Emission 0", Float) = 0
		[sRGBWarning]_EmissionMask1 ("Emission Mask--{reference_properties:[_EmissionMask1Pan, _EmissionMask1UV, _EmissionMask1Channel, _EmissionMaskInvert1, _EmissionMask1GlobalMask]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_EmissionMask1Pan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _EmissionMask1UV ("UV", Int) = 0
		[ThryHideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_EmissionMask1Channel ("Channel", Float) = 0
		[ThryHideInInspector][ToggleUI]_EmissionMaskInvert1 ("Invert", Float) = 0
		[ThryHideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _EmissionMask1GlobalMask ("Global Mask--{reference_property:_EmissionMask1GlobalMaskBlendType}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_EmissionMask1GlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HDR]_EmissionColor1 ("Emission Color--{reference_property:_EmissionColor1ThemeIndex}", Color) = (1, 1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionColor1ThemeIndex ("Color Theme Index", Int) = 0
		[sRGBWarning(true)][Gradient]_EmissionMap1 ("Emission Map--{reference_properties:[_EmissionMap1Pan, _EmissionMap1UV]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_EmissionMap1Pan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _EmissionMap1UV ("UV", Int) = 0
		_EmissionStrength1 ("Emission Strength", Range(0, 20)) = 0
		[ToggleUI]_EmissionBaseColorAsMap1 ("Use Base Colors", Float) = 0
		[ToggleUI]_EmissionReplace1 ("Override Base Color", Float) = 0
		_EmissionFluorescence1 ("Fluorescence", Range(0, 1)) = 0
		[HideInInspector] s_start_EmissionHueShift1 ("Color Adjust--{reference_property:_EmissionHueShiftEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_EmissionHueShiftEnabled1 ("Hue Shift", Float) = 0
		[ThryWideEnum(OKLab, 0, HSV, 1)] _EmissionHueShiftColorSpace1 ("Color Space", Int) = 0
		[ThryWideEnum(Hue Select, 0, Hue Shift, 1)] _EmissionHueSelectOrShift1 ("Select or Shift", Int) = 1
		_EmissionSaturation1 ("Saturation", Range(-1, 10)) = 0
		_EmissionHueShift1 ("Hue Shift", Range(0, 1)) = 0
		_EmissionHueShiftSpeed1 ("Hue Shift Speed", Float) = 0
		[HideInInspector] s_end_EmissionHueShift1 ("", Float) = 0
		[HideInInspector] s_start_EmissionCenterOut1 ("Center Out--{reference_property:_EmissionCenterOutEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_EmissionCenterOutEnabled1 ("Center Out", Float) = 0
		_EmissionCenterOutSpeed1 ("Flow Speed", Float) = 5
		[ToggleUI]_EmissionCenterOutLinear1 ("Linear", Float) = 0
		[HideInInspector] s_end_EmissionCenterOut1 ("", Float) = 0
		[HideInInspector] s_start_EmissionLightBased1 ("Light Based--{reference_property:_EnableGITDEmission1,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_EnableGITDEmission1 ("Light Based", Float) = 0
		[Enum(World, 0, Mesh, 1)] _GITDEWorldOrMesh1 ("Lighting Type", Int) = 0
		_GITDEMinEmissionMultiplier1 ("Min Emission Multiplier", Range(0, 1)) = 1
		_GITDEMaxEmissionMultiplier1 ("Max Emission Multiplier", Range(0, 1)) = 0
		_GITDEMinLight1 ("Min Lighting", Range(0, 1)) = 0
		_GITDEMaxLight1 ("Max Lighting", Range(0, 1)) = 1
		[HideInInspector] s_end_EmissionLightBased1 ("", Float) = 0
		[HideInInspector] s_start_EmissionBlinking1 ("Blinking--{reference_property:_EmissionBlinkingEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_EmissionBlinkingEnabled1 ("Blinking", Float) = 0
		_EmissiveBlink_Min1 ("Emissive Blink Min", Float) = 0
		_EmissiveBlink_Max1 ("Emissive Blink Max", Float) = 1
		_EmissiveBlink_Velocity1 ("Emissive Blink Velocity", Float) = 4
		_EmissionBlinkingOffset1 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionBlinking1 ("", Float) = 0
		[HideInInspector] s_start_ScrollingEmission1 ("Scrolling--{reference_property:_ScrollingEmission1,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI] _ScrollingEmission1 ("Scrolling", Float) = 0
		[ToggleUI]_EmissionScrollingUseCurve1 ("Use Curve", float) = 0
		[Curve]_EmissionScrollingCurve1 ("Curve--{condition_showS:(_EmissionScrollingUseCurve1==1)}", 2D) = "white" { }
		[ToggleUI]_EmissionScrollingVertexColor1 ("VColor as position", float) = 0
		_EmissiveScroll_Direction1 ("Direction", Vector) = (0, -10, 0, 0)
		_EmissiveScroll_Width1 ("Width", Float) = 10
		_EmissiveScroll_Velocity1 ("Velocity", Float) = 10
		_EmissiveScroll_Interval1 ("Interval", Float) = 20
		_EmissionScrollingOffset1 ("Offset", Float) = 0
		[HideInInspector] s_end_ScrollingEmission1 ("", Float) = 0
		[Space(4)]
		[ThryToggleUI(true)] _EmissionAL1Enabled ("<size=13><b>  Audio Link ♫</b></size>--{ condition_showS:_EnableAudioLink==1}", Float) = 0
		[HideInInspector] s_start_EmissionAL1Multiply ("Strength Multiply--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL1MultipliersBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL1Multipliers ("Multiplier", Vector) = (1, 1, 0, 0)
		[HideInInspector] s_end_EmissionAL1Multiply ("Strength Multiply", Float) = 0
		[HideInInspector] s_start_EmissionAL1Add ("Strength Add--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL1StrengthBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL1StrengthMod ("Strength", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_EmissionAL1Add ("Strength Add", Float) = 0
		[HideInInspector] s_start_EmissionAL1COut ("Center Out--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AudioLinkEmission1CenterOutBand ("Band", Int) = 0
		[VectorLabel(Min, Max)] _AudioLinkEmission1CenterOut ("Strength", Vector) = (0, 0, 0, 0)
		_AudioLinkEmission1CenterOutSize ("Intensity Threshold", Range(0, 1)) = 0
		_AudioLinkEmission1CenterOutDuration ("Duration", Range(-1, 1)) = 1
		[HideInInspector] s_end_EmissionAL1COut ("Center Out", Float) = 0
		[HideInInspector] s_start_EmissionAL1ColorOverride ("Color Override--{reference_property:_EmissionAL1ColorOverrideEnabled,persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[ThryHideInInspector][ToggleUI] _EmissionAL1ColorOverrideEnabled ("Toggle", Float) = 0
		_EmissionAL1ColorOverride ("Color Override", Color) = (1,1,1,1)
		[ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionAL1ColorOverrideThemeIndex ("Theme Color", Int) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL1ColorOverrideBand ("Color Override Band", Int) = 0
		[Ramp4(normalized)]_EmissionAL1ColorOverrideRamp ("Color Override Ramp", Vector) = (0, 1, 0, 1)
		[HideInInspector] s_end_EmissionAL1ColorOverride ("Color Override", Float) = 0
		[HideInInspector] m_end_emission1Options ("", Float) = 0
		[HideInInspector] m_start_glitter (" Glitter / Sparkle--{reference_property:_GlitterEnable,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/glitter},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(_SUNDISK_SIMPLE)]_GlitterEnable ("Enable Glitter", Float) = 0
		[ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _GlitterUV ("UV", Int) = 0
		[Enum(Angle, 0, Linear Emission, 1, Light Reflections, 2)]_GlitterMode ("Mode", Int) = 0
		[Enum(Circle, 0, Square, 1)]_GlitterShape ("Shape", Int) = 0
		[Enum(Add, 0, Replace, 1)] _GlitterBlendType ("Blend Mode", Int) = 0
		_GlitterUseNormals ("Use Normals", Range(0, 1)) = 0
		[IntRange]_GlitterLayers ("Layers", Range(1, 4)) = 1
		[HideInInspector] s_start_GlitterColorAndShape ("Shape & Color--{persistent_expand:true,default_expand:true}", Float) = 1
		_GlitterTexture ("Shape Texture--{reference_properties:[_GlitterTexturePixelMode]}", 2D) = "white" { }
		[ThryHideInInspector][ToggleUI]_GlitterTexturePixelMode ("Pixel Mode", Float) = 0
		[sRGBWarning(true)]_GlitterColorMap ("Color Map--{reference_properties:[_GlitterColorMapPan, _GlitterColorMapUV]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_GlitterColorMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _GlitterColorMapUV ("UV", Int) = 0
		[HDR]_GlitterColor ("Color--{reference_property:_GlitterColorThemeIndex}", Color) = (1, 1, 1, 1)
		[ThryHideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _GlitterColorThemeIndex ("", Int) = 0
		_GlitterUseSurfaceColor ("Use Base Color", Range(0, 1)) = 0
		[ToggleUI]_GlitterRandomColors ("Random Colors", Float) = 0
		[MultiSlider]_GlitterMinMaxSaturation ("Saturation Range--{condition_showS:(_GlitterRandomColors==1)}", Vector) = (0.8, 1, 0, 1)
		[MultiSlider]_GlitterMinMaxBrightness ("Brightness Range--{condition_showS:(_GlitterRandomColors==1)}", Vector) = (0.8, 1, 0, 1)
		[HideInInspector] s_end_GlitterColorAndShape ("Color", Float) = 0
		[HideInInspector] s_start_GlitterPositionSize ("Position & Size--{persistent_expand:true,default_expand:true}", Float) = 0
		_GlitterFrequency ("Glitter Density", Float) = 300.0
		_GlitterSize ("Glitter Size--{condition_show:(_GlitterRandomSize==0)}", Range(0, 1)) = .3
		[Vector2]_GlitterUVPanning ("Panning", Vector) = (0, 0, 0, 0)
		[ToggleUI]_GlitterRandomLocation ("Random Position", Float) = 1.0
		[ToggleUI]_GlitterRandomSize ("Random Size", Float) = 0
		[MultiSlider]_GlitterMinMaxSize ("Size Range--{condition_show:(_GlitterRandomSize==1)}", Vector) = (0.1, 0.5, 0, 1)
		[ToggleUI]_GlitterFullCoverage ("Allow Overlap (Heavy)", Float) = 0
		[HideInInspector] s_end_GlitterPositionSize ("Position & Size", Float) = 0
		[HideInInspector] s_start_GlitterSparkleControl ("Sparkle Control--{persistent_expand:true,default_expand:true,condition_showS:_GlitterMode!=1}", Float) = 1
		_GlitterMinBrightness ("Glitter Min Brightness", Range(0, 1)) = 0
		_GlitterBrightness ("Glitter Max Brightness", Range(0, 40)) = 3
		_GlitterSpeed ("Speed", Float) = 10.0
		_GlitterAngleRange ("Glitter Angle Range--{condition_showS:_GlitterMode==0||_GlitterMode==2}", Range(0, 90)) = 90
		_GlitterBias ("Glitter Bias--{condition_show:_GlitterMode==0}", Range(0, 1)) = .8
		_GlitterContrast ("Post Contrast", Range(1, 1000)) = 300
		_GlitterJaggyFix ("Distant Jaggy Fix--{condition_show:_GlitterShape==1}", Range(0, .1)) = .0
		[ToggleUI]_GlitterDistanceLOD ("Distance LOD", Float) = 1
		[HideInInspector] s_end_GlitterSparkleControl ("Sparkle Control", Float) = 0
		[HideInInspector] s_start_GlitterLinearSparkleControl ("Linear Emission Control--{persistent_expand:true,default_expand:true,condition_showS:_GlitterMode==1}", Float) = 1
		_GlitterLinearMinBrightness ("Min Brightness", Range(0, 1)) = 0
		_GlitterLinearBrightness ("Max Brightness", Range(0, 40)) = 3
		_GlitterLinearSpeed ("Speed", Float) = 0.25
		_GlitterLinearSensitivity ("Sensitivity", Range(0.01, 1)) = .25
		_GlitterLinearContrast ("Contrast", Range(0.01, 100)) = 50
		_GlitterLinearPostContrast ("Post Contrast", Range(0.01, 10)) = 1
		_GlitterLinearAngle ("Angle Limit", Range(0, 1)) = 0
		_GlitterLinearLightDirection ("Light Direction", Range(0, 1)) = 0
		_GlitterLinearVRParallax ("VR Parallax", Range(0, 1)) = 0
		[ToggleUI]_GlitterLinearDistanceLOD ("Distance LOD", Float) = 1
		[HideInInspector] s_end_GlitterLinearSparkleControl ("Linear Emission Control", Float) = 0
		[HideInInspector] s_start_GlitterRotationSection ("Rotations--{persistent_expand:true,default_expand:false}", Float) = 0
		[ToggleUI]_GlitterRandomRotation ("Random Offset", Float) = 0
		_GlitterTextureRotation ("Constant Speed", Float) = 0
		[VectorLabel(Min, Max)]_GlitterRandomRotationSpeed ("Random Speed Range", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_GlitterRotationSection ("Rotations", Float) = 0
		[HideInInspector] s_start_GlitterSpriteSheet ("Sprite Sheet--{reference_property:_GlitterSpriteSheetEnabled,persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_GlitterSpriteSheetEnabled ("Enable Sprite Sheet", Float) = 0
		_GlitterSpriteSheetColumns ("Columns", Float) = 1
		_GlitterSpriteSheetRows ("Rows", Float) = 1
		[ThryWideEnum(Random, 0, Time Based, 1)]_GlitterSpriteSheetFrameSelection ("Frame Selection", Int) = 0
		_GlitterSpriteSheetFPS ("FPS--{condition_showS:(_GlitterSpriteSheetFrameSelection==1)}", Float) = 10
		[ToggleUI]_GlitterSpriteSheetRandomStart ("Random Start Frame--{condition_showS:(_GlitterSpriteSheetFrameSelection==1)}", Float) = 0
		_GlitterSpriteSheetTotalFrames ("Total Frames (0 = auto)--{tooltip:Use when atlas isn't full. 0 = rows*columns}", Float) = 0
		[HideInInspector] s_end_GlitterSpriteSheet ("Sprite Sheet", Float) = 0
		[HideInInspector] s_start_GlitterMask ("Masking & Light Masking--{persistent_expand:true,default_expand:false}", Float) = 0
		[sRGBWarning]_GlitterMask ("Glitter Mask--{reference_properties:[_GlitterMaskPan, _GlitterMaskUV, _GlitterMaskChannel, _GlitterMaskInvert]}", 2D) = "white" { }
		[ThryHideInInspector][Vector2]_GlitterMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[ThryHideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7, Matcap, 9)] _GlitterMaskUV ("UV", Int) = 0
		[ThryHideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_GlitterMaskChannel ("Channel", Float) = 0
		[ThryHideInInspector][ToggleUI]_GlitterMaskInvert ("Invert", Float) = 0
		_GlitterHideInShadow ("Hide in shadow", Range(0, 1)) = 0
		_GlitterScaleWithLighting ("Scale With Lighting", Range(0, 1)) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _GlitterMaskGlobalMask ("Global Mask--{reference_property:_GlitterMaskGlobalMaskBlendType}", Int) = 0
		[ThryHideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_GlitterMaskGlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HideInInspector] s_end_GlitterMask ("Masking", Float) = 0
		[HideInInspector] s_start_GlitterHueShiftSection ("Hue Shift--{reference_property:_GlitterHueShiftEnabled, persistent_expand:true,default_expand:false}", Float) = 0
		[ThryHideInInspector][ToggleUI]_GlitterHueShiftEnabled ("Enable Hue Shift", Float) = 0
		[ThryWideEnum(OKLab, 0, HSV, 1)] _GlitterHueShiftColorSpace ("Color Space", Int) = 0
		[ThryWideEnum(Hue Select, 0, Hue Shift, 1)] _GlitterHueSelectOrShift ("Select or Shift", Int) = 1
		[ToggleUI]_GlitterHueShiftRandomOffset ("Random Offset", Float) = 0
		_GlitterHueShiftSpeed ("Shift Speed", Float) = 0
		_GlitterHueShift ("Hue Shift", Range(0, 1)) = 0
		[HideInInspector] s_end_GlitterHueShiftSection ("Hue Shift", Float) = 0
		[HideInInspector] s_start_GlitterAudioLink ("Audio Link ♫--{reference_property:_GlitterALEnabled,persistent_expand:true,default_expand:false, condition_showS:(_EnableAudioLink==1)}", Float) = 0
		[ThryHideInInspector][ToggleUI] _GlitterALEnabled ("Enable Audio Link", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALAlphaAddBand ("Alpha Band", Int) = 0
		[VectorLabel(Min, Max)]_GlitterALAlphaAdd ("Alpha Mod", Vector) = (0, 0, 0, 0)
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALMaxBrightnessBand ("Max Brightness Band", Int) = 0
		[VectorLabel(Min, Max)]_GlitterALMaxBrightnessAdd ("Max Brightness Mod", Vector) = (0, 0, 0, 0)
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALSizeAddBand ("Size Band", Int) = 0
		[VectorLabel(Min, Max)]_GlitterALSizeAdd ("Size Mod", Vector) = (0, 0, 0, 0)
		[ThryWideEnum(Motion increases as intensity of band increases, 0, Above but Smooth, 1, Motion moves back and forth as a function of intensity, 2, Above but Smoooth, 3, Fixed speed increase when the band is dark Stationary when light, 4, Above but Smooooth, 5, Fixed speed increase when the band is dark Fixed speed decrease when light, 6, Above but Smoooooth, 7)]_GlitterALChronoSparkleSpeedType ("Chrono Sparkle Type", Int) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALChronoSparkleSpeedBand ("Chrono Sparkle Band", Int) = 0
		_GlitterALChronoSparkleSpeed ("Chrono Sparkle Speed", Float) = 0
		[ThryWideEnum(Motion increases as intensity of band increases, 0, Above but Smooth, 1, Motion moves back and forth as a function of intensity, 2, Above but Smoooth, 3, Fixed speed increase when the band is dark Stationary when light, 4, Above but Smooooth, 5, Fixed speed increase when the band is dark Fixed speed decrease when light, 6, Above but Smoooooth, 7)]_GlitterALChronoRotationSpeedType ("Chrono Rotation Type", Int) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALChronoRotationSpeedBand ("Chrono Rotation Band", Int) = 0
		_GlitterALChronoRotationSpeed ("Chrono Rotation Speed", Float) = 0
		[ThrySeperator(2, 2, 2)]
		[ToggleUI] _GlitterALColorOverrideEnabled ("Color Override", Float) = 0
		_GlitterALColorOverride ("Color--{condition_show:(_GlitterALColorOverrideEnabled==1)}", Color) = (1,1,1,1)
		[ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _GlitterALColorOverrideThemeIndex ("Theme Color--{condition_show:(_GlitterALColorOverrideEnabled==1)}", Int) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _GlitterALColorOverrideBand ("Color Override Band--{condition_show:(_GlitterALColorOverrideEnabled==1)}", Int) = 0
		[Ramp4(normalized)]_GlitterALColorOverrideRamp ("Color Override Ramp--{condition_show:(_GlitterALColorOverrideEnabled==1)}", Vector) = (0, 1, 0, 1)
		[HideInInspector] s_end_GlitterAudioLink ("Audio Link", Float) = 0
		[HideInInspector] m_end_glitter ("Glitter / Sparkle", Float) = 0
		[HideInInspector] m_vertexCategory ("Vertex Options", Float) = 0
		[HideInInspector] m_start_Uzumore (" View Clip Prevention (Uzumore)--{reference_property:_UzumoreCategoryToggle,button_author:{text:sigmal00,action:{type:URL,data:https://github.com/sigmal00},hover:GitHub}}, button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/vertex-options/view-clip-prevention},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_UZUMORE)] _UzumoreCategoryToggle (" View Clip Prevention (Uzumore)", Float) = 0
		[ToggleUI] _UzumoreEnabled ("Animation Toggle", Float) = 1
		_UzumoreAmount ("Push Amount (m)", Float) = 0.1
		_UzumoreBias ("Push Bias", Float) = 0.001
		[sRGBWarning]_UzumoreMask ("Push Mask (A)", 2D) = "white" { }
		[ThryWideEnum(R, 0, G, 1, B, 2, A, 3)] _UzumoreMaskUV ("Push Mask Channel", Float) = 3
		[HideInInspector] m_end_Uzumore ("Camera Push Back", Float) = 0
		[HideInInspector] m_modifierCategory ("Global Modifiers & Data", Float) = 0
		[HideInInspector] m_start_PoiGlobalCategory ("Global Data and Masks", Float) = 0
		[HideInInspector] m_start_PoiTimeOptions ("Time Source--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/time-source},hover:Documentation}}", Float) = 0
		[ThryWideEnum(Unity Default, 0, VRChat Network Time, 1)] _PoiTimeSource ("Time Source", Int) = 1
		[HideInInspector] m_end_PoiTimeOptions ("Time Source", Float) = 0
		[HideInInspector] m_start_GlobalThemes ("Global Themes--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/global-themes},hover:Documentation}}", Float) = 0
		[HideInInspector] m_start_GlobalThemeColor0 ("Theme Color 0", Float) = 0
		[HDR]_GlobalThemeColor0 ("Theme Color 0", Color) = (1, 1, 1, 1)
		[ThryWideEnum(OKLab, 0, HSV, 1)] _GlobalThemeHueShiftColorSpace0 ("Color Space", Int) = 0
		_GlobalThemeHueSpeed0 ("Shift Speed", Float) = 0
		_GlobalThemeHue0 ("Hue Shift", Range(0, 1)) = 0
		[ThryWideEnum(Saturation Value (old), 0, Saturation Brightness, 1)] _GlobalThemeSaturationMethod0 ("Saturation Style", Int) = 1
		_GlobalThemeSaturation0 ("Saturation Adjust--{condition_show:(_GlobalThemeSaturationMethod0==0)}", Range(-1, 1)) = 0
		_GlobalThemeValue0 ("Value Adjust--{condition_show:(_GlobalThemeSaturationMethod0==0)}", Range(-1, 1)) = 0
		_GlobalThemeSaturation0 ("Saturation--{condition_show:(_GlobalThemeSaturationMethod0==1)}", Range(-1, 10)) = 0
		_GlobalThemeBrightness0 ("Brightness--{condition_show:(_GlobalThemeSaturationMethod0==1)}", Range(-1, 2)) = 0
		[HideInInspector] m_end_GlobalThemeColor0   ("Theme Color 0", Float) = 0
		[HideInInspector] m_start_GlobalThemeColor1 ("Theme Color 1", Float) = 0
		[HDR]_GlobalThemeColor1 ("Theme Color 1", Color) = (1, 1, 1, 1)
		[ThryWideEnum(OKLab, 0, HSV, 1)] _GlobalThemeHueShiftColorSpace1 ("Color Space", Int) = 0
		_GlobalThemeHueSpeed1 ("Shift Speed", Float) = 0
		_GlobalThemeHue1 ("Hue Shift", Range(0, 1)) = 0
		[ThryWideEnum(Saturation Value (old), 0, Saturation Brightness, 1)] _GlobalThemeSaturationMethod1 ("Saturation Style", Int) = 1
		_GlobalThemeSaturation1 ("Saturation Adjust--{condition_show:(_GlobalThemeSaturationMethod1==0)}", Range(-1, 1)) = 0
		_GlobalThemeValue1 ("Value Adjust--{condition_show:(_GlobalThemeSaturationMethod1==0)}", Range(-1, 1)) = 0
		_GlobalThemeSaturation1 ("Saturation--{condition_show:(_GlobalThemeSaturationMethod1==1)}", Range(-1, 10)) = 0
		_GlobalThemeBrightness1 ("Brightness--{condition_show:(_GlobalThemeSaturationMethod1==1)}", Range(-1, 2)) = 0
		[HideInInspector] m_end_GlobalThemeColor1   ("Theme Color 1", Float) = 0
		[HideInInspector] m_start_GlobalThemeColor2 ("Theme Color 2", Float) = 0
		[HDR]_GlobalThemeColor2 ("Theme Color 2", Color) = (1, 1, 1, 1)
		[ThryWideEnum(OKLab, 0, HSV, 1)] _GlobalThemeHueShiftColorSpace2 ("Color Space", Int) = 0
		_GlobalThemeHueSpeed2 ("Shift Speed", Float) = 0
		_GlobalThemeHue2 ("Hue Shift", Range(0, 1)) = 0
		[ThryWideEnum(Saturation Value (old), 0, Saturation Brightness, 1)] _GlobalThemeSaturationMethod2 ("Saturation Style", Int) = 1
		_GlobalThemeSaturation2 ("Saturation Adjust--{condition_show:(_GlobalThemeSaturationMethod2==0)}", Range(-1, 1)) = 0
		_GlobalThemeValue2 ("Value Adjust--{condition_show:(_GlobalThemeSaturationMethod2==0)}", Range(-1, 1)) = 0
		_GlobalThemeSaturation2 ("Saturation--{condition_show:(_GlobalThemeSaturationMethod2==1)}", Range(-1, 10)) = 0
		_GlobalThemeBrightness2 ("Brightness--{condition_show:(_GlobalThemeSaturationMethod2==1)}", Range(-1, 2)) = 0
		[HideInInspector] m_end_GlobalThemeColor2   ("Theme Color 2", Float) = 0
		[HideInInspector] m_start_GlobalThemeColor3 ("Theme Color 3", Float) = 0
		[HDR]_GlobalThemeColor3 ("Theme Color 3", Color) = (1, 1, 1, 1)
		[ThryWideEnum(OKLab, 0, HSV, 1)] _GlobalThemeHueShiftColorSpace3 ("Color Space", Int) = 0
		_GlobalThemeHueSpeed3 ("Shift Speed", Float) = 0
		_GlobalThemeHue3 ("Hue Shift", Range(0, 1)) = 0
		[ThryWideEnum(Saturation Value (old), 0, Saturation Brightness, 1)] _GlobalThemeSaturationMethod3 ("Saturation Style", Int) = 1
		_GlobalThemeSaturation3 ("Saturation Adjust--{condition_show:(_GlobalThemeSaturationMethod3==0)}", Range(-1, 1)) = 0
		_GlobalThemeValue3 ("Value Adjust--{condition_show:(_GlobalThemeSaturationMethod3==0)}", Range(-1, 1)) = 0
		_GlobalThemeSaturation3 ("Saturation--{condition_show:(_GlobalThemeSaturationMethod3==1)}", Range(-1, 10)) = 0
		_GlobalThemeBrightness3 ("Brightness--{condition_show:(_GlobalThemeSaturationMethod3==1)}", Range(-1, 2)) = 0
		[HideInInspector] m_end_GlobalThemeColor3   ("Theme Color 3", Float) = 0
		[HideInInspector] m_end_GlobalThemes ("Global Themes", Float ) = 0
		[HideInInspector] m_start_GlobalMask ("Global Mask--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/global-masks},hover:Documentation}}", Float) = 0
		[HideInInspector] m_start_GlobalMaskModifiers ("Modifiers", Float) = 0
		[HideInInspector] m_end_GlobalMaskModifiers ("", Float) = 0
		[HideInInspector] m_end_GlobalMask ("Global Mask", Float) = 0
		[HideInInspector] m_end_PoiGlobalCategory ("Global Data and Masks ", Float) = 0
		[HideInInspector] m_start_PoiUVCategory ("UVs", Float) = 0
		[HideInInspector] m_start_Stochastic ("Stochastic Sampling--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/uvs/stochastic-sampling},hover:Documentation}}", Float) = 0
		[KeywordEnum(Deliot Heitz, Hextile, None)] _StochasticMode ("Sampling Mode", Float) = 0
		[HideInInspector] s_start_deliot ("Deliot Heitz--{persistent_expand:true,default_expand:false,condition_show:_StochasticMode==0}", Float) = 0
		_StochasticDeliotHeitzDensity ("Detiling Density", Range(0.1, 10)) = 1
		[HideInInspector] s_end_deliot ("Deliot Heitz", Float) = 0
		[HideInInspector] m_end_Stochastic ("Stochastic Sampling", Float) = 0
		[HideInInspector] m_start_uvSettings ("UV Settings--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/uvs/uv-settings},hover:Documentation}}", Float) = 0
		[HideInInspector] s_start_uvSettingsUV0 ("UV0--{persistent_expand:true,default_expand:false}", Float) = 0
		[VectorLabel(X, Y)]_UVSettingsTiling0 ("Tiling", Vector) = (1, 1, 0, 0)
		[VectorLabel(X, Y)]_UVSettingsOffset0 ("Offset", Vector) = (0, 0, 0, 0)
		_UVSettingsAngle0 ("Angle", Range(-180, 180)) = 0
		[VectorLabel(X, Y)]_UVSettingsPan0 ("Pan", Vector) = (0, 0, 0, 0)
		_UVSettingsRotate0 ("Rotate", Float) = 0
		[HideInInspector] s_end_uvSettingsUV0 ("UV0", Float) = 0
		[HideInInspector] s_start_uvSettingsUV1 ("UV1--{persistent_expand:true,default_expand:false}", Float) = 0
		[VectorLabel(X, Y)]_UVSettingsTiling1 ("Tiling", Vector) = (1, 1, 0, 0)
		[VectorLabel(X, Y)]_UVSettingsOffset1 ("Offset", Vector) = (0, 0, 0, 0)
		_UVSettingsAngle1 ("Angle", Range(-180, 180)) = 0
		[VectorLabel(X, Y)]_UVSettingsPan1 ("Pan", Vector) = (0, 0, 0, 0)
		_UVSettingsRotate1 ("Rotate", Float) = 0
		[HideInInspector] s_end_uvSettingsUV1 ("UV1", Float) = 0
		[HideInInspector] s_start_uvSettingsUV2 ("UV2--{persistent_expand:true,default_expand:false}", Float) = 0
		[VectorLabel(X, Y)]_UVSettingsTiling2 ("Tiling", Vector) = (1, 1, 0, 0)
		[VectorLabel(X, Y)]_UVSettingsOffset2 ("Offset", Vector) = (0, 0, 0, 0)
		_UVSettingsAngle2 ("Angle", Range(-180, 180)) = 0
		[VectorLabel(X, Y)]_UVSettingsPan2 ("Pan", Vector) = (0, 0, 0, 0)
		_UVSettingsRotate2 ("Rotate", Float) = 0
		[HideInInspector] s_end_uvSettingsUV2 ("UV2", Float) = 0
		[HideInInspector] s_start_uvSettingsUV3 ("UV3--{persistent_expand:true,default_expand:false}", Float) = 0
		[VectorLabel(X, Y)]_UVSettingsTiling3 ("Tiling", Vector) = (1, 1, 0, 0)
		[VectorLabel(X, Y)]_UVSettingsOffset3 ("Offset", Vector) = (0, 0, 0, 0)
		_UVSettingsAngle3 ("Angle", Range(-180, 180)) = 0
		[VectorLabel(X, Y)]_UVSettingsPan3 ("Pan", Vector) = (0, 0, 0, 0)
		_UVSettingsRotate3 ("Rotate", Float) = 0
		[HideInInspector] s_end_uvSettingsUV3 ("UV3", Float) = 0
		[ToggleUI] _UVSettingsShiftBackfaceUV ("Shift Backface UV", Float) = 0
		[HideInInspector] m_end_uvSettings ("UV Settings", Float) = 0
		[HideInInspector] m_start_uvLocalWorld ("Local World UV--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/uvs/local-world-uv},hover:Documentation}}", Float) = 0
		[ThryWideEnum(X, 0, Y, 1, Z, 2, Zero, 3, VColor R, 4, VColor G, 5, VColor B, 6, VColor A, 7)] _UVModLocalPos0 ("Local X", Int) = 0
		[ThryWideEnum(X, 0, Y, 1, Z, 2, Zero, 3, VColor R, 4, VColor G, 5, VColor B, 6, VColor A, 7)] _UVModLocalPos1 ("Local Y", Int) = 1
		[Space(10)]
		[ThryWideEnum(X, 0, Y, 1, Z, 2, Zero, 3)] _UVModWorldPos0 ("World X", Int) = 0
		[ThryWideEnum(X, 0, Y, 1, Z, 2, Zero, 3)] _UVModWorldPos1 ("World Y", Int) = 2
		[HideInInspector] m_end_uvLocalWorld ("Local World UV", Float) = 0
		[HideInInspector] m_start_uvPanosphere ("Panosphere UV--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/uvs/panosphere-uv},hover:Documentation}}", Float) = 0
		[ToggleUI] _StereoEnabled ("Stereo Enabled", Float) = 0
		[ToggleUI] _PanoUseBothEyes ("Perspective Correct (VR)", Float) = 1
		[HideInInspector] m_end_uvPanosphere ("Panosphere UV", Float) = 0
		[HideInInspector] m_start_uvPolar ("Polar UV--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/modifiers/uvs/polar-uv},hover:Documentation}}", Float) = 0
		[ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8)] _PolarUV ("UV", Int) = 0
		[ThryWideEnum(Standard, 0, Log Polar, 1, Square Polar, 2)] _PolarMode ("Mode", Int) = 0
		[Vector2]_PolarCenter ("Center Coordinate", Vector) = (.5, .5, 0, 0)
		_PolarRadialScale ("Radial Scale", Float) = 1
		_PolarLengthScale ("Length Scale", Float) = 1
		_PolarSpiralPower ("Spiral Power", Float) = 0
		[HideInInspector] m_end_uvPolar ("Polar UV", Float) = 0
		[HideInInspector] m_end_PoiUVCategory ("UVs ", Float) = 0
		[HideInInspector] m_start_PoiPostProcessingCategory ("Post Processing", Float) = 0
		[HideInInspector] m_start_PPAnimations ("PP Animations--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/post-processing/pp-animations},hover:Documentation}}", Float) = 0
		[Helpbox(1, 2)] _PPHelp ("This section is intended for real time adjustments through animations at runtime. Therefore, these should not be changed in Unity.", Int) = 0
		_PPLightingMultiplier ("Lighting Multiplier", Float) = 1
		_PPLightingAddition ("Lighting Add", Float) = 0
		_PPEmissionMultiplier ("Emission Multiplier", Float) = 1
		_PPFinalColorMultiplier ("Final Color Multiplier", Float) = 1
		[HideInInspector] m_end_PPAnimations ("PP Animations ", Float) = 0
		[HideInInspector] m_end_PoiPostProcessingCategory ("Post Processing ", Float) = 0
		[HideInInspector] m_thirdpartyCategory ("Third Party", Float) = 0
		[HideInInspector] m_renderingCategory ("Rendering--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/rendering/},hover:Documentation}}", Float) = 0
		[DoNotAnimate][Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
		[DoNotAnimate][Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[DoNotAnimate][Enum(Off, 0, On, 1)] _ZWrite ("ZWrite", Int) = 1
		[DoNotAnimate][ThryMask(Thry.ColorMaskFlags)] _ColorMask ("Color Mask", Int) = 15
		[DoNotAnimate] _OffsetFactor ("Offset Factor", Float) = 0.0
		[DoNotAnimate] _OffsetUnits ("Offset Units", Float) = 0.0
		[DoNotAnimate][ToggleUI] _RenderingReduceClipDistance ("Reduce Clip Distance", Float) = 0
		[DoNotAnimate][ToggleUI] _ZClip ("Z Clip", Float) = 1
		[DoNotAnimate][ToggleUI] _IgnoreFog ("Ignore Fog", Float) = 0
		[DoNotAnimate][ToggleUI]_FlipBackfaceNormals ("Flip Backface Normals", Int) = 1
		[DoNotAnimate][HideInInspector] Instancing ("Instancing", Float) = 0 //add this property for instancing variants settings to be shown
		[ToggleUI] _RenderingEarlyZEnabled ("Early Z", Float) = 0
		[HideInInspector] m_start_blending ("Blending--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/rendering/blending},hover:Documentation}}", Float) = 0
		[DoNotAnimate][Enum(Thry.BlendOp)] _BlendOp ("RGB Blend Op", Int) = 0
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("RGB Source Blend", Int) = 1
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("RGB Destination Blend", Int) = 0
		[DoNotAnimate][Space][ThryHeaderLabel(Additive Blending, 13)]
		[DoNotAnimate][Enum(Thry.BlendOp)] _AddBlendOp ("RGB Blend Op", Int) = 4
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _AddSrcBlend ("RGB Source Blend", Int) = 1
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _AddDstBlend ("RGB Destination Blend", Int) = 1
		[DoNotAnimate][HideInInspector] m_start_alphaBlending ("Advanced Alpha Blending", Float) = 0
		[DoNotAnimate][Enum(Thry.BlendOp)] _BlendOpAlpha ("Alpha Blend Op", Int) = 0
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendAlpha ("Alpha Source Blend", Int) = 1
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _DstBlendAlpha ("Alpha Destination Blend", Int) = 10
		[DoNotAnimate][Space][ThryHeaderLabel(Additive Blending, 13)]
		[DoNotAnimate][Enum(Thry.BlendOp)] _AddBlendOpAlpha ("Alpha Blend Op", Int) = 4
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _AddSrcBlendAlpha ("Alpha Source Blend", Int) = 0
		[DoNotAnimate][Enum(UnityEngine.Rendering.BlendMode)] _AddDstBlendAlpha ("Alpha Destination Blend", Int) = 1
		[DoNotAnimate][HideInInspector] m_end_alphaBlending ("Advanced Alpha Blending", Float) = 0
		[HideInInspector] m_end_blending ("Blending", Float) = 0
		[HideInInspector] m_start_StencilPassOptions ("Stencil--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/rendering/stencil},hover:Documentation}}", Float) = 0
		[ThryWideEnum(Simple, 0, Front Face vs Back Face, 1)] _StencilType ("Stencil Type", Float) = 0
		[IntRange] _StencilRef ("Stencil Reference Value", Range(0, 255)) = 0
		[IntRange] _StencilReadMask ("Stencil ReadMask Value", Range(0, 255)) = 255
		[IntRange] _StencilWriteMask ("Stencil WriteMask Value", Range(0, 255)) = 255
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPassOp ("Stencil Pass Op--{condition_showS:(_StencilType==0)}", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFailOp ("Stencil Fail Op--{condition_showS:(_StencilType==0)}", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilZFailOp ("Stencil ZFail Op--{condition_showS:(_StencilType==0)}", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilCompareFunction ("Stencil Compare Function--{condition_showS:(_StencilType==0)}", Float) = 8
		[HideInInspector] m_start_StencilPassBackOptions("Back--{condition_showS:(_StencilType==1)}", Float) = 0
		[Helpbox(1, 2)] _FFBFStencilHelp0 ("Front Face and Back Face Stencils only work when locked in due to Unity's Stencil managment", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilBackPassOp ("Back Pass Op", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilBackFailOp ("Back Fail Op", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilBackZFailOp ("Back ZFail Op", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilBackCompareFunction ("Back Compare Function", Float) = 8
		[HideInInspector] m_end_StencilPassBackOptions("Back", Float) = 0
		[HideInInspector] m_start_StencilPassFrontOptions("Front--{condition_showS:(_StencilType==1)}", Float) = 0
		[Helpbox(1, 2)] _FFBFStencilHelp1 ("Front Face and Back Face Stencils only work when locked in due to Unity's Stencil managment", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFrontPassOp ("Front Pass Op", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFrontFailOp ("Front Fail Op", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFrontZFailOp ("Front ZFail Op", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilFrontCompareFunction ("Front Compare Function", Float) = 8
		[HideInInspector] m_end_StencilPassFrontOptions("Front", Float) = 0
		[HideInInspector] m_end_StencilPassOptions ("Stencil", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "VRCFallback" = "Standard" }
		CGINCLUDE
		#define POI_BIRP 1
		#define POI_URP 2
		#define POI_PIPE POI_BIRP
		#ifndef BUILTIN_TARGET_API
		#define BUILTIN_TARGET_API
		#endif
		#if !defined(STEREO_MULTIVIEW_ON)
		#pragma use_dxc vulkan
		#endif
		#if POI_PIPE == POI_BIRP
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#elif POI_PIPE == POI_URP
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#if defined(SHADER_API_VULKAN)
		#undef TEXTURE3D
		#define TEXTURE3D(textureName) TEXTURE3D_FLOAT(textureName)
		#endif
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Assets/00.WarudoBuild/BP/Node68_DevKit/Shaders/Node68PoiyomiGlobalVolume.hlsl"
		#endif
		SamplerState sampler_linear_clamp;
		SamplerState sampler_linear_repeat;
		SamplerState sampler_trilinear_clamp;
		SamplerState sampler_trilinear_repeat;
		SamplerState sampler_point_clamp;
		SamplerState sampler_point_repeat;
		#define DielectricSpec float4(0.04, 0.04, 0.04, 1.0 - 0.04)
		#define PI_OVER_2 1.5707963f
		#define PI_OVER_4 0.785398f
		#define EPSILON 0.000001f
		#if POI_PIPE == POI_BIRP
		#define HALF_PI float(1.5707964)
		#define PI float(3.14159265359)
		#define TWO_PI float(6.28318530718)
		#define INV_PI 0.31830988618f
		#define INV_TWO_PI 0.15915494309f
		#ifdef LIGHTMAP_ON
		#define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT) OUT.xy = lightmapUV.xy * lightmapScaleOffset.xy + lightmapScaleOffset.zw;
		#else
		#define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT)
		#endif
		#elif POI_PIPE == POI_URP
		#define UNITY_PI 3.14159265359f
		#define UNITY_INV_PI 0.31830988618f
		#define UNITY_TWO_PI 6.28318530718f
		#define UNITY_INV_TWO_PI 0.15915494309f
		#define UNITY_DECLARE_TEX2D(tex) Texture2D tex; SamplerState sampler##tex
		#define UNITY_DECLARE_TEX2D_NOSAMPLER(tex) Texture2D tex
		#define UNITY_SAMPLE_TEX2D_SAMPLER(tex,samplertex,coord) tex.Sample (sampler##samplertex,coord)
		#define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex, samplertex, coord, lod) tex.SampleLevel (sampler##samplertex, coord, lod)
		#define UNITY_SAMPLE_TEXCUBE_LOD(tex,coord,lod) tex.SampleLevel (sampler##tex,coord, lod)
		#define UNITY_SAMPLE_TEXCUBE_SAMPLER_LOD(tex, samplertex, coord, lod) tex.SampleLevel (sampler##samplertex, coord, lod)
		#define UNITY_DECLARE_TEX2DARRAY(tex) TEXTURE2D_ARRAY(tex); SAMPLER(sampler##tex)
		#define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) Texture2DArray tex
		#define UNITY_SAMPLE_TEX2DARRAY(tex,coord) SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
		#define UNITY_SAMPLE_TEX2DARRAY_SAMPLER_LOD(tex,samplertex,coord,lod) tex.SampleLevel (sampler##samplertex,coord,lod)
		#ifdef UNITY_COLORSPACE_GAMMA
		#define unity_ColorSpaceGrey float4(0.5, 0.5, 0.5, 0.5)
		#define unity_ColorSpaceDouble float4(2.0, 2.0, 2.0, 2.0)
		#define unity_ColorSpaceDielectricSpec half4(0.220916301, 0.220916301, 0.220916301, 1.0 - 0.220916301)
		#define unity_ColorSpaceLuminance half4(0.22, 0.707, 0.071, 0.0) // Legacy: alpha is set to 0.0 to specify gamma mode
		#else // Linear values
		#define unity_ColorSpaceGrey float4(0.214041144, 0.214041144, 0.214041144, 0.5)
		#define unity_ColorSpaceDouble float4(4.59479380, 4.59479380, 4.59479380, 2.0)
		#define unity_ColorSpaceDielectricSpec half4(0.04, 0.04, 0.04, 1.0 - 0.04) // standard dielectric reflectivity coef at incident angle (= 4%)
		#define unity_ColorSpaceLuminance half4(0.0396819152, 0.458021790, 0.00609653955, 1.0) // Legacy: alpha is set to 1.0 to specify linear mode
		#endif
		#if UNITY_VERSION < 202100
		#if defined(UNITY_PLATFORM_META_QUEST) && META_QUEST_LIGHTUNROLL
		#define UNROLL_ONELIGHT [unroll(1)]
		#else
		#define UNROLL_ONELIGHT
		#endif
		#define LIGHT_LOOP_BEGIN(lightCount) \
		UNROLL_ONELIGHT \
		for (uint lightIndex = 0u; lightIndex < lightCount; ++lightIndex) {
		#define LIGHT_LOOP_END }
		#endif
		#endif
		#if POI_PIPE == POI_BIRP
		#define POI_SHADOW_COORDS(idx) UNITY_SHADOW_COORDS(idx)
		#define POI_TRANSFER_SHADOW(a, coord) UNITY_TRANSFER_SHADOW(a, coord)
		#define POI_TRANSFER_SHADOW_CASTER_NOPOS(o, opos) TRANSFER_SHADOW_CASTER_NOPOS(o, opos)
		#define POI_SAMPLE_TEXCUBE_LOD(tex,sample,coord,lod) tex.SampleLevel(sample,coord,lod)
		#define POI_SAMPLE_TEX2DARRAY_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
		#elif POI_PIPE == POI_URP
		#define POI_SHADOW_COORDS(idx) float4 shadowCoord : TEXCOORD##idx;
		#define POI_TRANSFER_SHADOW(a, coord) a.shadowCoord = TransformWorldToShadowCoord(a.worldPos.rgb);
		#define POI_TRANSFER_SHADOW_CASTER_NOPOS(o, opos) opos = GetShadowPositionHClip(o.worldPos.rgb, o.normal.rgb);
		#define POI_SAMPLE_TEXCUBE_LOD(tex,sample,coord,lod) SAMPLE_TEXTURECUBE_LOD(tex,sample,coord,lod)
		#define POI_SAMPLE_TEX2DARRAY_SAMPLERGRAD(tex, samplertex, coord, dx, dy) SAMPLE_TEXTURE2D_ARRAY_GRAD(tex, sampler##samplertex, coord.xy, floor(coord.z), dx, dy);
		#endif
		#if POI_PIPE == POI_URP
		static half4 unity_4LightAtten0 = 0;
		#endif
		#define POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
		#define POI2D_SAMPLE_TEX2D_SAMPLERGRADD(tex, samp, uv, pan, dx, dy) tex.SampleGrad(samp, POI_PAN_UV(uv, pan), dx, dy)
		#define POI_PAN_UV(uv, pan) (uv + POI_TIME.x * pan)
		#define POI2D_SAMPLER_PAN(tex, texSampler, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, POI_PAN_UV(uv, pan)))
		#define POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, POI_PAN_UV(uv, pan), dx, dy))
		#define POI2D_SAMPLER(tex, texSampler, uv) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv))
		#define POI_SAMPLE_1D_X(tex, samp, uv) tex.Sample(samp, float2(uv, 0.5))
		#define POI2D_SAMPLER_GRAD(tex, texSampler, uv, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, uv, dx, dy))
		#define POI2D_SAMPLER_GRADD(tex, texSampler, uv, dx, dy) tex.SampleGrad(texSampler, uv, dx, dy)
		#define POI2D_PAN(tex, uv, pan) (tex2D(tex, POI_PAN_UV(uv, pan)))
		#define POI2D(tex, uv) (tex2D(tex, uv))
		#define POI_SAMPLE_TEX2D(tex, uv) (UNITY_SAMPLE_TEX2D(tex, uv))
		#define POI_SAMPLE_TEX2D_PAN(tex, uv, pan) (UNITY_SAMPLE_TEX2D(tex, POI_PAN_UV(uv, pan)))
		#define POI_SAMPLE_CUBE_LOD(tex, sampler, coord, lod) tex.SampleLevel(sampler, coord, lod)
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#ifndef UNITY_DECLARE_SCREENSPACE_TEXTURE
		#define UNITY_DECLARE_SCREENSPACE_TEXTURE UNITY_DECLARE_TEX2DARRAY
		#endif
		#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, float3(uv, unity_StereoEyeIndex))
		#else
		#ifndef UNITY_DECLARE_SCREENSPACE_TEXTURE
		#define UNITY_DECLARE_SCREENSPACE_TEXTURE(tex) sampler2D tex;
		#endif
		#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, uv)
		#endif
		#define POI_SAFE_RGB0 float4(mainTexture.rgb * .0001, 0)
		#define POI_SAFE_RGB1 float4(mainTexture.rgb * .0001, 1)
		#define POI_SAFE_RGBA mainTexture
		#if POI_PIPE == POI_URP || defined(UNITY_COMPILER_HLSL) || defined(SHADER_API_PSSL) || defined(UNITY_COMPILER_HLSLCC)
		#define PoiInitStruct(type,name) name = (type)0;
		#else
		#define PoiInitStruct(type,name)
		#endif
		#define POI_ERROR(poiMesh, gridSize) lerp(float3(1, 0, 1), float3(0, 0, 0), fmod(floor((poiMesh.worldPos.x) * gridSize) + floor((poiMesh.worldPos.y) * gridSize) + floor((poiMesh.worldPos.z) * gridSize), 2) == 0)
		#define POI_NAN (asfloat(-1))
		#define POI_MODE_OPAQUE 0
		#define POI_MODE_CUTOUT 1
		#define POI_MODE_FADE 2
		#define POI_MODE_TRANSPARENT 3
		#define POI_MODE_ADDITIVE 4
		#define POI_MODE_SOFTADDITIVE 5
		#define POI_MODE_MULTIPLICATIVE 6
		#define POI_MODE_2XMULTIPLICATIVE 7
		#define POI_MODE_TRANSCLIPPING 9
		#define POI_MAX_VISIBLE_LIGHTS 64
		#define POI_DIRECTIONAL 1
		#define POI_POINT_SPOT 2
		#if defined(SHADER_API_MOBILE) && defined(STEREO_MULTIVIEW_ON) && (defined(SHADER_API_GLES3) || defined(SHADER_API_VULKAN))
		#define POI_DEVICE_QUEST
		#endif
		#if defined(POI_DEVICE_QUEST)
		#define POI_INITIALIZE_VERTEX_OUTPUT_STEREO(output)
		#else
		#define POI_INITIALIZE_VERTEX_OUTPUT_STEREO(output) UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output)
		#endif
		#if defined(POI_DEVICE_QUEST)
		#define poi_maxvertexcount(x) maxvertexcount(6)
		#define poi_instance(x) instance(1)
		#else
		#define poi_maxvertexcount(x) maxvertexcount(x)
		#define poi_instance(x) instance(x)
		#endif
		#ifndef UNITY_SPECCUBE_LOD_STEPS
		#define UNITY_SPECCUBE_LOD_STEPS (6)
		#endif
		#ifndef UNITY_LIGHTING_COMMON_INCLUDED
		#define UNITY_LIGHTING_COMMON_INCLUDED
		float4 _LightColor0;
		float4 _SpecColor;
		struct UnityLight
		{
			half3 color;
			half3 dir;
			half ndotl;
		};
		struct UnityIndirect
		{
			half3 diffuse;
			half3 specular;
		};
		struct UnityGI
		{
			UnityLight light;
			UnityIndirect indirect;
		};
		struct UnityGIInput
		{
			UnityLight light;
			float3 worldPos;
			half3 worldViewDir;
			half atten;
			half3 ambient;
			#if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION) || defined(UNITY_ENABLE_REFLECTION_BUFFERS)
			float4 boxMin[2];
			#endif
			#ifdef UNITY_SPECCUBE_BOX_PROJECTION
			float4 boxMax[2];
			float4 probePosition[2];
			#endif
			float4 probeHDR[2];
		};
		#endif
		struct Unity_GlossyEnvironmentData
		{
			half roughness;
			half3 reflUVW;
		};
		struct PoiMesh
		{
			float3 normals[2];
			float3 objNormal;
			float3 tangentSpaceNormal;
			float3 bentNormal;
			float hasBentNormal;
			float3 binormal[2];
			float3 tangent[2];
			float3 worldPos;
			float3 localPos;
			float3 objectPosition;
			float isFrontFace;
			float4 vertexColor;
			float4 lightmapUV;
			float2 uv[10];
			float2 parallaxUV;
			float2 dx;
			float2 dy;
			uint isRightHand;
		};
		struct PoiCam
		{
			float3 viewDir;
			float3 forwardDir;
			float3 worldPos;
			float distanceToVert;
			float4 clipPos;
			float4 screenSpacePosition;
			float3 reflectionDir;
			float3 vertexReflectionDir;
			float3 tangentViewDir;
			float4 posScreenSpace;
			float2 posScreenPixels;
			float2 screenUV;
			float vDotN;
			float4 worldDirection;
		};
		struct PoiMods
		{
			float4 Mask;
			float audioLink[5];
			float audioLinkAvailable;
			float audioLinkVersion;
			float4 audioLinkTexture;
			float2 detailMask;
			float2 backFaceDetailIntensity;
			float globalEmission;
			float4 globalColorTheme[12];
			float globalMask[16];
			float ALTime[8];
		};
		#if POI_PIPE == POI_BIRP
		struct Light
		{
			half3   direction;
			half3   color;
			half    distanceAttenuation;
			half    shadowAttenuation;
		};
		struct BRDFData
		{
			half3 albedo;
			half3 diffuse;
			half3 specular;
			half reflectivity;
			half perceptualRoughness;
			half roughness;
			half roughness2;
			half grazingTerm;
			half normalizationTerm;
			half roughness2MinusOne;
		};
		#endif
		struct PoiLight
		{
			float3 direction;
			float nDotVCentered;
			float attenuation;
			float attenuationStrength;
			float3 directColor;
			float3 indirectColor;
			float occlusion;
			#if POI_PIPE == POI_BIRP
			float shadowMask;
			#elif  POI_PIPE == POI_URP
			float4 shadowMask;
			#endif
			float detailShadow;
			float3 halfDir;
			float lightMap;
			float lightMapNoAttenuation;
			float3 rampedLightMap;
			float vertexNDotL;
			float nDotL;
			float nDotV;
			float vertexNDotV;
			float nDotH;
			float vertexNDotH;
			float lDotv;
			float lDotH;
			float nDotLSaturated;
			float nDotLNormalized;
			float additiveShadow;
			float3 finalLighting;
			float3 finalLightAdd;
			float3 LTCGISpecular;
			float3 LTCGIDiffuse;
			float directLuminance;
			float indirectLuminance;
			float finalLuminance;
			float lightingMinLightBrightness;
			float lightingCapEnabled;
			float lightingMonochromatic;
			int lightType;
			Light unityLight;
			#if POI_PIPE == POI_URP
			BRDFData brdfData;
			BRDFData brdfDataClearCoat;
			#endif
			#if defined(VERTEXLIGHT_ON)
			float4 vDotNL;
			float4 vertexVDotNL;
			float3 vColor[4];
			float4 vCorrectedDotNL;
			float4 vAttenuation;
			float4 vSaturatedDotNL;
			float3 vPosition[4];
			float3 vDirection[4];
			float3 vFinalLighting;
			float3 vHalfDir[4];
			half4 vDotNH;
			half4 vertexVDotNH;
			half4 vDotLH;
			#endif
		};
		#if POI_PIPE == POI_URP
		struct PoiLightOut
		{
			float3 totalLighting;
			float3 totalLightAdd;
		};
		#endif
		struct PoiFragData
		{
			float smoothness;
			float smoothness2;
			float metallic;
			float specularMask;
			float reflectionMask;
			float3 baseColor;
			float3 finalColor;
			float alpha;
			float3 emission;
			float toggleVertexLights;
		};
		static float4 PoiSHAr = 0;
		static float4 PoiSHAg = 0;
		static float4 PoiSHAb = 0;
		static float4 PoiSHBr = 0;
		static float4 PoiSHBg = 0;
		static float4 PoiSHBb = 0;
		static float4 PoiSHC  = 0;
		float4 poiTransformClipSpacetoScreenSpaceFrag(float4 clipPos)
		{
			float4 positionSS = float4(clipPos.xyz * clipPos.w, clipPos.w);
			positionSS.xy = positionSS.xy / _ScreenParams.xy;
			return positionSS;
		}
		half3 PoiSHEval_L0L1(half4 normal)
		{
			half3 x;
			x.r = dot(PoiSHAr, normal);
			x.g = dot(PoiSHAg, normal);
			x.b = dot(PoiSHAb, normal);
			return x;
		}
		half3 PoiSHEval_L2(half4 normal)
		{
			half3 x1, x2;
			half4 vB = normal.xyzz * normal.yzzx;
			x1.r = dot(PoiSHBr, vB);
			x1.g = dot(PoiSHBg, vB);
			x1.b = dot(PoiSHBb, vB);
			half  vC = normal.x*normal.x - normal.y*normal.y;
			x2    = PoiSHC.rgb * vC;
			return x1 + x2;
		}
		half3 PoiShadeSH9 (half4 normal)
		{
			half3 res = PoiSHEval_L0L1(normal);
			res += PoiSHEval_L2(normal);
			#ifdef UNITY_COLORSPACE_GAMMA
			res = LinearToGammaSpace(res);
			#endif
			return res;
		}
		struct MochieBRDFData
		{
			float metallic;
			float smoothness;
			float smoothness2;
			float specularMask;
			float reflectionMask;
			float roughness;
			float brdfRoughness;
			float brdfRoughness2;
			float percepRough;
			float percepRough2;
			float omr;
			float3 specCol;
			float2 adjustedAnisotropy;
		};
		inline half Pow5(half x)
		{
			return x * x * x * x * x;
		}
		inline half3 FresnelLerp(half3 F0, half3 F90, half cosA)
		{
			half t = Pow5((1 - cosA));   // ala Schlick interpoliation
			return lerp(F0, F90, t);
		}
		inline half3 FresnelTerm(half3 F0, half cosA)
		{
			half t = Pow5((1 - cosA));   // ala Schlick interpoliation
			return F0 + (1 - F0) * t;
		}
		half perceptualRoughnessToMipmapLevel(half perceptualRoughness)
		{
			return perceptualRoughness * UNITY_SPECCUBE_LOD_STEPS;
		}
		half3 UnpackScaleNormalDXT5nm(half4 packednormal, half bumpScale)
		{
			half3 normal;
			normal.xy = (packednormal.wy * 2 - 1);
			#if (SHADER_TARGET >= 30)
			normal.xy *= bumpScale;
			#endif
			normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
			return normal;
		}
		#ifdef BUILTIN_TARGET_API
		half3 LerpWhiteTo(half3 b, half t)
		{
			half oneMinusT = 1 - t;
			return half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
		}
		#endif
		inline float GGXTerm(float NdotH, float roughness)
		{
			float a2 = roughness * roughness;
			float d = (NdotH * a2 - NdotH) * NdotH + 1.0f; // 2 mad
			return UNITY_INV_PI * a2 / (d * d + 1e-7f); // This function is not intended to be running on Mobile,
		}
		Unity_GlossyEnvironmentData UnityGlossyEnvironmentSetup(half Smoothness, half3 worldViewDir, half3 Normal, half3 fresnel0)
		{
			Unity_GlossyEnvironmentData g;
			g.roughness /* perceptualRoughness */ = 1 - Smoothness;
			g.reflUVW = reflect(-worldViewDir, Normal);
			return g;
		}
		half3 UnpackScaleNormalRGorAG(half4 packednormal, half bumpScale)
		{
			#if defined(UNITY_NO_DXT5nm)
			half3 normal = packednormal.xyz * 2 - 1;
			#if (SHADER_TARGET >= 30)
			normal.xy *= bumpScale;
			#endif
			return normal;
			#elif defined(UNITY_ASTC_NORMALMAP_ENCODING)
			half3 normal;
			normal.xy = (packednormal.wy * 2 - 1);
			normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
			normal.xy *= bumpScale;
			return normal;
			#else
			packednormal.x *= packednormal.w;
			half3 normal;
			normal.xy = (packednormal.xy * 2 - 1);
			#if (SHADER_TARGET >= 30)
			normal.xy *= bumpScale;
			#endif
			normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
			return normal;
			#endif
		}
		half3 UnpackScaleNormal(half4 packednormal, half bumpScale)
		{
			return UnpackScaleNormalRGorAG(packednormal, bumpScale);
		}
		half3 BlendNormals(half3 n1, half3 n2)
		{
			return normalize(half3(n1.xy + n2.xy, n1.z * n2.z));
		}
		inline float2 Pow4(float2 x)
		{
			return x * x * x * x;
		}
		inline float3 Unity_SafeNormalize(float3 inVec)
		{
			float dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}
		inline float3 BoxProjectedCubemapDirection(float3 worldRefl, float3 worldPos, float4 cubemapCenter, float4 boxMin, float4 boxMax)
		{
			
			if (cubemapCenter.w > 0.0)
			{
				float3 nrdir = normalize(worldRefl);
				#if 1
				float3 rbmax = (boxMax.xyz - worldPos) / nrdir;
				float3 rbmin = (boxMin.xyz - worldPos) / nrdir;
				float3 rbminmax = (nrdir > 0.0f) ? rbmax : rbmin;
				#else // Optimized version
				float3 rbmax = (boxMax.xyz - worldPos);
				float3 rbmin = (boxMin.xyz - worldPos);
				float3 select = step(float3(0, 0, 0), nrdir);
				float3 rbminmax = lerp(rbmax, rbmin, select);
				rbminmax /= nrdir;
				#endif
				float fa = min(min(rbminmax.x, rbminmax.y), rbminmax.z);
				worldPos -= cubemapCenter.xyz;
				worldRefl = worldPos + nrdir * fa;
			}
			return worldRefl;
		}
		#if POI_PIPE == POI_BIRP
		half3 Unity_GlossyEnvironment(UNITY_ARGS_TEXCUBE(tex), half4 hdr, Unity_GlossyEnvironmentData glossIn)
		{
			half perceptualRoughness = glossIn.roughness /* perceptualRoughness */ ;
			#if 0
			float m = PerceptualRoughnessToRoughness(perceptualRoughness); // m is the real roughness parameter
			const float fEps = 1.192092896e-07F;        // smallest such that 1.0+FLT_EPSILON != 1.0  (+1e-4h is NOT good here. is visibly very wrong)
			float n = (2.0 / max(fEps, m * m)) - 2.0;        // remap to spec power. See eq. 21 in --> https://dl.dropboxusercontent.com/u/55891920/papers/mm_brdf.pdf
			n /= 4;                                     // remap from n_dot_h formulatino to n_dot_r. See section "Pre-convolved Cube Maps vs Path Tracers" --> https://s3.amazonaws.com/docs.knaldtech.com/knald/1.0.0/lys_power_drops.html
			perceptualRoughness = pow(2 / (n + 2), 0.25);      // remap back to square root of real roughness (0.25 include both the sqrt root of the conversion and sqrt for going from roughness to perceptualRoughness)
			#else
			perceptualRoughness = perceptualRoughness * (1.7 - 0.7 * perceptualRoughness);
			#endif
			half mip = perceptualRoughnessToMipmapLevel(perceptualRoughness);
			half3 R = glossIn.reflUVW;
			half4 rgbm = UNITY_SAMPLE_TEXCUBE_LOD(tex, R, mip);
			return DecodeHDR(rgbm, hdr);
		}
		inline half3 UnityGI_IndirectSpecular(UnityGIInput data, half occlusion, Unity_GlossyEnvironmentData glossIn)
		{
			half3 specular;
			#ifdef UNITY_SPECCUBE_BOX_PROJECTION
			half3 originalReflUVW = glossIn.reflUVW;
			glossIn.reflUVW = BoxProjectedCubemapDirection(originalReflUVW, data.worldPos, data.probePosition[0], data.boxMin[0], data.boxMax[0]);
			#endif
			#ifdef _GLOSSYREFLECTIONS_OFF
			specular = unity_IndirectSpecColor.rgb;
			#else
			half3 env0 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE(unity_SpecCube0), data.probeHDR[0], glossIn);
			#ifdef UNITY_SPECCUBE_BLENDING
			const float kBlendFactor = 0.99999;
			float blendLerp = data.boxMin[0].w;
			
			if (blendLerp < kBlendFactor)
			{
				#ifdef UNITY_SPECCUBE_BOX_PROJECTION
				glossIn.reflUVW = BoxProjectedCubemapDirection(originalReflUVW, data.worldPos, data.probePosition[1], data.boxMin[1], data.boxMax[1]);
				#endif
				half3 env1 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0), data.probeHDR[1], glossIn);
				specular = lerp(env1, env0, blendLerp);
			}
			else
			{
				specular = env0;
			}
			#else
			specular = env0;
			#endif
			#endif
			return specular * occlusion;
		}
		inline half3 UnityGI_IndirectSpecular(UnityGIInput data, half occlusion, half3 normalWorld, Unity_GlossyEnvironmentData glossIn)
		{
			return UnityGI_IndirectSpecular(data, occlusion, glossIn);
		}
		#endif
		#if POI_PIPE == POI_URP
		half LinearRgbToLuminance(half3 linearRgb)
		{
			return dot(linearRgb, half3(0.2126729f,  0.7151522f, 0.0721750f));
		}
		#endif
		#ifndef glsl_mod
		#define glsl_mod(x, y) (((x) - (y) * floor((x) / (y))))
		#endif
		#ifndef mod2pi
		#define mod2pi(x) (frac((x) / UNITY_TWO_PI) * UNITY_TWO_PI)
		#endif
		uniform float random_uniform_float_only_used_to_stop_compiler_warnings = 0.0f;
		#if POI_PIPE == POI_URP
		inline float Linear01Depth( float z )
		{
			return 1.0 / (_ZBufferParams.x * z + _ZBufferParams.y);
		}
		inline half3 LinearToGammaSpace (half3 linRGB)
		{
			linRGB = max(linRGB, half3(0.h, 0.h, 0.h));
			return max(1.055h * pow(linRGB, 0.416666667h) - 0.055h, 0.h);
		}
		inline half3 GammaToLinearSpace (half3 sRGB)
		{
			return sRGB * (sRGB * (sRGB * 0.305306011h + 0.682171111h) + 0.012522878h);
		}
		half4 PoiCalculateShadowMask(float2 shadowCoords)
		{
			#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
			half4 shadowMask = SAMPLE_SHADOWMASK(shadowCoords); // Shadowmask was sampled from lightmap
			#elif !defined(LIGHTMAP_ON) && (defined(PROBE_VOLUMES_L1) || defined(PROBE_VOLUMES_L2))
			half4 shadowMask = SAMPLE_SHADOWMASK(shadowCoords); // Shadowmask (probe occlusion) was sampled from APV
			#elif !defined (LIGHTMAP_ON)
			half4 shadowMask = unity_ProbesOcclusion; // Sample shadowmask (probe occlusion) from legacy probes
			#else
			half4 shadowMask = half4(1, 1, 1, 1); // Fallback shadowmask, fully unoccluded
			#endif
			return shadowMask;
		}
		half3 SHEvalLinearL2 (half4 normal)
		{
			half3 x1, x2;
			half4 vB = normal.xyzz * normal.yzzx;
			x1.r = dot(PoiSHBr,vB);
			x1.g = dot(PoiSHBg,vB);
			x1.b = dot(PoiSHBb,vB);
			half vC = normal.x*normal.x - normal.y*normal.y;
			x2 = PoiSHC.rgb * vC;
			return x1 + x2;
		}
		half3 PoiGlossyEnvironmentReflection(half3 reflectVector, float3 positionWS, half perceptualRoughness, half occlusion, float2 normalizedScreenSpaceUV)
		{
			#if UNITY_VERSION >= 202200
			return GlossyEnvironmentReflection(reflectVector, positionWS, perceptualRoughness, occlusion, normalizedScreenSpaceUV);
			#elif UNITY_VERSION >= 202100
			return GlossyEnvironmentReflection(reflectVector, positionWS, perceptualRoughness, occlusion);
			#else
			return GlossyEnvironmentReflection(reflectVector, perceptualRoughness, occlusion);
			#endif
		}
		half3 PoiGlobalIllumination(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
		half3 bakedGI, half occlusion, float3 positionWS,
		half3 normalWS, half3 viewDirectionWS, float2 normalizedScreenSpaceUV)
		{
			#if UNITY_VERSION >= 202200
			return GlobalIllumination(brdfData, brdfDataClearCoat, clearCoatMask, bakedGI, occlusion, positionWS, normalWS, viewDirectionWS, normalizedScreenSpaceUV);
			#elif UNITY_VERSION >= 202100
			return GlobalIllumination(brdfData, brdfDataClearCoat, clearCoatMask, bakedGI, occlusion, positionWS, normalWS, viewDirectionWS);
			#else
			return GlobalIllumination(brdfData, brdfDataClearCoat, clearCoatMask, bakedGI, occlusion, normalWS, viewDirectionWS);
			#endif
		}
		#endif
		float3 PoiUnpackNormalScale(float4 packedNormal, float bumpScale)
		{
			#if POI_PIPE == POI_BIRP
			return UnpackScaleNormal(packedNormal, bumpScale);
			#elif POI_PIPE == POI_URP
			return UnpackNormalScale(packedNormal, bumpScale);
			#endif
		}
		float4 PoiObjectToClipPos(in float3 pos)
		{
			#if POI_PIPE == POI_BIRP
			return UnityObjectToClipPos(pos);
			#elif POI_PIPE == POI_URP
			return TransformObjectToHClip(pos);
			#endif
		}
		float4 PoiWorldToClipPos(in float3 pos)
		{
			#if POI_PIPE == POI_BIRP
			return UnityWorldToClipPos(pos);
			#elif POI_PIPE == POI_URP
			return TransformWorldToHClip(pos);
			#endif
		}
		float4 PoiComputeScreenPos(in float4 pos)
		{
			#if POI_PIPE == POI_BIRP
			return ComputeGrabScreenPos(pos);
			#elif POI_PIPE == POI_URP
			return ComputeScreenPos(pos);
			#endif
		}
		float3 PoiDecodeHDR(float4 data, float4 decodeInstructions)
		{
			#if POI_PIPE == POI_BIRP
			return DecodeHDR(data, decodeInstructions);
			#elif POI_PIPE == POI_URP
			return DecodeHDREnvironment(data, decodeInstructions);
			#endif
		}
		float3 PoiWorldSpaceViewDir(float3 worldPos)
		{
			#if POI_PIPE == POI_BIRP
			return UnityWorldSpaceViewDir(worldPos);
			#elif POI_PIPE == POI_URP
			return GetWorldSpaceViewDir(worldPos);
			#endif
		}
		float3 PoiObjSpaceViewDir(float4 v)
		{
			#if POI_PIPE == POI_BIRP
			return ObjSpaceViewDir(v);
			#elif POI_PIPE == POI_URP
			return mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0)).xyz - v.xyz;
			#endif
		}
		float2 poiUV(float2 uv, float4 tex_st)
		{
			return uv * tex_st.xy + tex_st.zw;
		}
		float CalculateLuminance(float3 color)
		{
			return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
		}
		float AverageLuminance(float3 color)
		{
			return (color.r + color.g + color.b) / 3.0;
		}
		float dotToDegrees(float dot)
		{
			dot = clamp(dot, -1.0, 1.0);
			return degrees(acos(dot));
		}
		float dotToDegrees(float3 a, float3 b)
		{
			return dotToDegrees(dot(normalize(a), normalize(b)));
		}
		float poiFastAtan(float x)
		{
			return x * (abs(x) * (1.5707963 * abs(x) - 0.00507668) + 0.420691) /
			(abs(x) * (abs(x) * (0.633387806 + abs(x)) + 0.671041944) + 0.215192627);
		}
		float _VRChatCameraMode;
		float _VRChatMirrorMode;
		uint _VRChatTimeNetworkMs;
		uint _VRChatTimeUTCUnixSeconds;
		float _PoiTimeSource;
		static float4 POI_TIME = (1.0 == 1 && _VRChatTimeNetworkMs != 0) ? (((_VRChatTimeNetworkMs << 6) >> 6) * float4(0.00005, 0.001, 0.002, 0.003))
		: _Time;
		float VRCCameraMode()
		{
			return _VRChatCameraMode;
		}
		float VRCMirrorMode()
		{
			return _VRChatMirrorMode;
		}
		bool IsInMirror()
		{
			return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
		}
		bool IsOrthographicCamera()
		{
			return unity_OrthoParams.w == 1 || UNITY_MATRIX_P[3][3] == 1;
		}
		float shEvaluateDiffuseL1Geomerics_local(float L0, float3 L1, float3 n)
		{
			float R0 = max(0, L0);
			float3 R1 = 0.5f * L1;
			float lenR1 = length(R1);
			float q = dot(normalize(R1), n) * 0.5 + 0.5;
			q = saturate(q); // Thanks to ScruffyRuffles for the bug identity.
			float p = 1.0f + 2.0f * lenR1 / R0;
			float a = (1.0f - lenR1 / R0) / (1.0f + lenR1 / R0);
			return R0 * (a + (1.0f - a) * (p + 1.0f) * pow(q, p));
		}
		half3 BetterSH9(half4 normal)
		{
			float3 indirect;
			float3 L0 = float3(PoiSHAr.w, PoiSHAg.w, PoiSHAb.w) + float3(PoiSHBr.z, PoiSHBg.z, PoiSHBb.z) / 3.0;
			indirect.r = shEvaluateDiffuseL1Geomerics_local(L0.r, PoiSHAr.xyz, normal.xyz);
			indirect.g = shEvaluateDiffuseL1Geomerics_local(L0.g, PoiSHAg.xyz, normal.xyz);
			indirect.b = shEvaluateDiffuseL1Geomerics_local(L0.b, PoiSHAb.xyz, normal.xyz);
			indirect = max(0, indirect);
			indirect += SHEvalLinearL2(normal);
			return indirect;
		}
		float3 BetterSH9(float3 normal)
		{
			return BetterSH9(float4(normal, 1));
		}
		float3 getCameraForward()
		{
			#if UNITY_SINGLE_PASS_STEREO
			float3 p1 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 1, 1));
			float3 p2 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 0, 1));
			#else
			float3 p1 = mul(unity_CameraToWorld, float4(0, 0, 1, 1)).xyz;
			float3 p2 = mul(unity_CameraToWorld, float4(0, 0, 0, 1)).xyz;
			#endif
			return normalize(p2 - p1);
		}
		half3 GetSHLength()
		{
			half3 x, x1;
			x.r = length(PoiSHAr);
			x.g = length(PoiSHAg);
			x.b = length(PoiSHAb);
			x1.r = length(PoiSHBr);
			x1.g = length(PoiSHBg);
			x1.b = length(PoiSHBb);
			return x + x1;
		}
		float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
		{
			#if UNITY_SPECCUBE_BOX_PROJECTION
			if (cubemapPosition.w > 0)
			{
				float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
				float scalar = min(min(factors.x, factors.y), factors.z);
				direction = direction * scalar + (position - cubemapPosition.xyz);
			}
			#endif
			return direction;
		}
		float poiMax(float2 i)
		{
			return max(i.x, i.y);
		}
		float poiMax(float3 i)
		{
			return max(max(i.x, i.y), i.z);
		}
		float poiMax(float4 i)
		{
			return max(max(max(i.x, i.y), i.z), i.w);
		}
		float3 calculateNormal(in float3 baseNormal, in PoiMesh poiMesh, in Texture2D normalTexture, in SamplerState mainSampler, in float4 normal_ST, in float2 normalPan, in float normalUV, in float normalIntensity)
		{
			float3 normal = PoiUnpackNormalScale(normalTexture.Sample(mainSampler, POI_PAN_UV(poiUV(poiMesh.uv[normalUV], normal_ST), normalPan)), normalIntensity);
			return normalize(
			normal.x * poiMesh.tangent[0] +
			normal.y * poiMesh.binormal[0] +
			normal.z * baseNormal
			);
		}
		float remap(float x, float minOld, float maxOld, float minNew = 0, float maxNew = 1)
		{
			return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
		}
		float2 remap(float2 x, float2 minOld, float2 maxOld, float2 minNew = 0, float2 maxNew = 1)
		{
			return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
		}
		float3 remap(float3 x, float3 minOld, float3 maxOld, float3 minNew = 0, float3 maxNew = 1)
		{
			return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
		}
		float4 remap(float4 x, float4 minOld, float4 maxOld, float4 minNew = 0, float4 maxNew = 1)
		{
			return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
		}
		float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
		{
			return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
		}
		float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
		{
			return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
		}
		float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
		{
			return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
		}
		float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
		{
			return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
		}
		float2 calcParallax(in float height, in PoiCam poiCam)
		{
			return ((height * - 1) + 1) * (poiCam.tangentViewDir.xy / poiCam.tangentViewDir.z);
		}
		float4 poiBlend(const float sourceFactor, const  float4 sourceColor, const  float destinationFactor, const  float4 destinationColor, const float4 blendFactor)
		{
			float4 sA = 1 - blendFactor;
			const float4 blendData[11] = {
				float4(0.0, 0.0, 0.0, 0.0),
				float4(1.0, 1.0, 1.0, 1.0),
				destinationColor,
				sourceColor,
				float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
				sA,
				float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
				sA,
				float4(1.0, 1.0, 1.0, 1.0) - sA,
				saturate(sourceColor.aaaa),
				1 - sA,
			};
			return lerp(blendData[sourceFactor] * sourceColor + blendData[destinationFactor] * destinationColor, sourceColor, sA);
		}
		float blendColorBurn(float base, float blend)
		{
			return (blend == 0.0) ? blend : max((1.0 - ((1.0 - base) * rcp(random_uniform_float_only_used_to_stop_compiler_warnings + blend))), 0.0);
		}
		float3 blendColorBurn(float3 base, float3 blend)
		{
			return float3(blendColorBurn(base.r, blend.r), blendColorBurn(base.g, blend.g), blendColorBurn(base.b, blend.b));
		}
		float blendColorDodge(float base, float blend)
		{
			return (blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0);
		}
		float3 blendColorDodge(float3 base, float3 blend)
		{
			return float3(blendColorDodge(base.r, blend.r), blendColorDodge(base.g, blend.g), blendColorDodge(base.b, blend.b));
		}
		float blendDarken(float base, float blend)
		{
			return min(blend, base);
		}
		float3 blendDarken(float3 base, float3 blend)
		{
			return float3(blendDarken(base.r, blend.r), blendDarken(base.g, blend.g), blendDarken(base.b, blend.b));
		}
		float blendOverlay(float base, float blend)
		{
			return base < 0.5 ? (2.0 * base * blend) : (1.0 - 2.0 * (1.0 - base) * (1.0 - blend));
		}
		float3 blendOverlay(float3 base, float3 blend)
		{
			return float3(blendOverlay(base.r, blend.r), blendOverlay(base.g, blend.g), blendOverlay(base.b, blend.b));
		}
		float blendLighten(float base, float blend)
		{
			return max(blend, base);
		}
		float3 blendLighten(float3 base, float3 blend)
		{
			return float3(blendLighten(base.r, blend.r), blendLighten(base.g, blend.g), blendLighten(base.b, blend.b));
		}
		float blendLinearDodge(float base, float blend)
		{
			return min(base + blend, 1.0);
		}
		float3 blendLinearDodge(float3 base, float3 blend)
		{
			return base + blend;
		}
		float blendMultiply(float base, float blend)
		{
			return base * blend;
		}
		float3 blendMultiply(float3 base, float3 blend)
		{
			return base * blend;
		}
		float blendNormal(float base, float blend)
		{
			return blend;
		}
		float3 blendNormal(float3 base, float3 blend)
		{
			return blend;
		}
		float blendScreen(float base, float blend)
		{
			return 1.0 - ((1.0 - base) * (1.0 - blend));
		}
		float3 blendScreen(float3 base, float3 blend)
		{
			return float3(blendScreen(base.r, blend.r), blendScreen(base.g, blend.g), blendScreen(base.b, blend.b));
		}
		float blendSubtract(float base, float blend)
		{
			return max(base - blend, 0.0);
		}
		float3 blendSubtract(float3 base, float3 blend)
		{
			return max(base - blend, 0.0);
		}
		float blendMixed(float base, float blend)
		{
			return base + base * blend;
		}
		float3 blendMixed(float3 base, float3 blend)
		{
			return base + base * blend;
		}
		float3 customBlend(float3 base, float3 blend, float blendType, float alpha = 1)
		{
			float3 r0 = lerp(base, blend, alpha);                        // Normal (0)
			float3 r1 = lerp(base, blendDarken(base, blend), alpha);     // Darken (1)
			float3 r2 = base * lerp(1, blend, alpha);                    // Multiply (2)
			float3 r5 = lerp(base, blendLighten(base, blend), alpha);    // Lighten (5)
			float3 r6 = lerp(base, blendScreen(base, blend), alpha);     // Screen (6)
			float3 r7 = blendSubtract(base, blend * alpha);              // Subtract (7)
			float3 r8 = lerp(base, blendLinearDodge(base, blend), alpha);// Linear Dodge (8)
			float3 r9 = lerp(base, blendOverlay(base, blend), alpha);    // Overlay (9)
			float3 r20 = lerp(base, blendMixed(base, blend), alpha);     // Mixed (20)
			float t = blendType;
			float w0 = saturate(1 - abs(t - 0));
			float w1 = saturate(1 - abs(t - 1));
			float w2 = saturate(1 - abs(t - 2));
			float w5 = saturate(1 - abs(t - 5));
			float w6 = saturate(1 - abs(t - 6));
			float w7 = saturate(1 - abs(t - 7));
			float w8 = saturate(1 - abs(t - 8));
			float w9 = saturate(1 - abs(t - 9));
			float w20 = saturate(1 - abs(t - 20));
			return r0*w0 + r1*w1 + r2*w2 + r5*w5 + r6*w6 + r7*w7 + r8*w8 + r9*w9 + r20*w20;
		}
		float customBlend(float base, float blend, float blendType, float alpha = 1)
		{
			float r0 = lerp(base, blend, alpha);                        // Normal (0)
			float r2 = base * lerp(1, blend, alpha);                    // Multiply (2)
			float r5 = lerp(base, blendLighten(base, blend), alpha);    // Lighten (5)
			float r6 = lerp(base, blendScreen(base, blend), alpha);     // Screen (6)
			float r7 = blendSubtract(base, blend * alpha);              // Subtract (7)
			float r8 = lerp(base, blendLinearDodge(base, blend), alpha);// Linear Dodge (8)
			float r9 = lerp(base, blendOverlay(base, blend), alpha);    // Overlay (9)
			float r20 = lerp(base, blendMixed(base, blend), alpha);     // Mixed (20)
			float t = blendType;
			float w0 = saturate(1 - abs(t - 0));
			float w2 = saturate(1 - abs(t - 2));
			float w5 = saturate(1 - abs(t - 5));
			float w6 = saturate(1 - abs(t - 6));
			float w7 = saturate(1 - abs(t - 7));
			float w8 = saturate(1 - abs(t - 8));
			float w9 = saturate(1 - abs(t - 9));
			float w20 = saturate(1 - abs(t - 20));
			return r0*w0 + r2*w2 + r5*w5 + r6*w6 + r7*w7 + r8*w8 + r9*w9 + r20*w20;
		}
		#define REPLACE 0
		#define SUBSTRACT 1
		#define MULTIPLY 2
		#define DIVIDE 3
		#define MIN 4
		#define MAX 5
		#define AVERAGE 6
		#define ADD 7
		float maskBlend(float baseMask, float blendMask, float blendType)
		{
			float replace = blendMask;
			float subtract = baseMask - blendMask;
			float multiply = baseMask * blendMask;
			float divide = baseMask / max(blendMask, 0.0001);
			float minVal = min(baseMask, blendMask);
			float maxVal = max(baseMask, blendMask);
			float average = (baseMask + blendMask) * 0.5;
			float add = baseMask + blendMask;
			float t = blendType + 0.5;
			float w0 = step(t, 1);
			float w1 = step(1, t) * step(t, 2);
			float w2 = step(2, t) * step(t, 3);
			float w3 = step(3, t) * step(t, 4);
			float w4 = step(4, t) * step(t, 5);
			float w5 = step(5, t) * step(t, 6);
			float w6 = step(6, t) * step(t, 7);
			float w7 = step(7, t);
			float output = replace * w0 + subtract * w1 + multiply * w2 + divide * w3 + minVal * w4 + maxVal * w5 + average * w6 + add * w7;
			return saturate(output);
		}
		float globalMaskBlend(float baseMask, float globalMaskIndex, float blendType, PoiMods poiMods)
		{
			if (globalMaskIndex == 0)
			{
				return baseMask;
			}
			else
			{
				return maskBlend(baseMask, poiMods.globalMask[globalMaskIndex - 1], blendType);
			}
		}
		float poiSampleMask(float4 sampledTexture, float4 channelStrengths, float blendMode, float invert, float2 remapMinMax, float gamma, float strength, float offset)
		{
			float mask = sampledTexture.r * channelStrengths.x;
			mask = lerp(mask, maskBlend(mask, sampledTexture.g, blendMode), channelStrengths.y);
			mask = lerp(mask, maskBlend(mask, sampledTexture.b, blendMode), channelStrengths.z);
			mask = lerp(mask, maskBlend(mask, sampledTexture.a, blendMode), channelStrengths.w);
			mask = saturate((mask - remapMinMax.x) / max(remapMinMax.y - remapMinMax.x, 0.0001));
			mask = pow(mask, gamma);
			mask = saturate(mask * strength + offset);
			mask = lerp(mask, 1.0 - mask, invert);
			return mask;
		}
		float poiSampleMask(float4 sampledTexture, float channel, float invert)
		{
			float4 channelStrengths = float4(
			channel < 0.5 ? 1 : 0,
			channel >= 0.5 && channel < 1.5 ? 1 : 0,
			channel >= 1.5 && channel < 2.5 ? 1 : 0,
			channel >= 2.5 ? 1 : 0
			);
			return poiSampleMask(sampledTexture, channelStrengths, 0, invert, float2(0, 1), 1.0, 1.0, 0.0);
		}
		float poiSampleMask(float4 sampledTexture, float channel, float invert, float strength, float offset)
		{
			float4 channelStrengths = float4(
			channel < 0.5 ? 1 : 0,
			channel >= 0.5 && channel < 1.5 ? 1 : 0,
			channel >= 1.5 && channel < 2.5 ? 1 : 0,
			channel >= 2.5 ? 1 : 0
			);
			return poiSampleMask(sampledTexture, channelStrengths, 0, invert, float2(0, 1), 1.0, strength, offset);
		}
		inline float poiRand(float2 co)
		{
			float3 p3 = frac(float3(co.xyx) * 0.1031);
			p3 += dot(p3, p3.yzx + 33.33);
			return frac((p3.x + p3.y) * p3.z);
		}
		inline float4 poiRand4(float2 seed)
		{
			float3 p3 = frac(float3(seed.xyx) * 0.1031);
			p3 += dot(p3, p3.yzx + 33.33);
			float2 a = frac((p3.xx + p3.yz) * p3.zy);
			float2 s2 = seed + 37.0;
			float3 q3 = frac(float3(s2.xyx) * 0.1031);
			q3 += dot(q3, q3.yzx + 33.33);
			float2 b = frac((q3.xx + q3.yz) * q3.zy);
			return float4(a, b);
		}
		inline float2 poiRand2(float seed)
		{
			float2 x = float2(seed, seed * 1.3);
			float3 p3 = frac(float3(x.xyx) * 0.1031);
			p3 += dot(p3, p3.yzx + 33.33);
			return frac((p3.xx + p3.yz) * p3.zy);
		}
		inline float2 poiRand2(float2 seed)
		{
			float3 p3 = frac(float3(seed.xyx) * 0.1031);
			p3 += dot(p3, p3.yzx + 33.33);
			return frac((p3.xx + p3.yz) * p3.zy);
		}
		inline float poiRand3(float seed)
		{
			float p = frac(seed * 0.1031);
			p *= p + 33.33;
			p *= p + p;
			return frac(p);
		}
		inline float3 poiRand3(float2 seed)
		{
			float3 p3 = frac(float3(seed.xyx) * 0.1031);
			p3 += dot(p3, p3.yzx + 33.33);
			return frac((p3.xxy + p3.yzz) * p3.zyx);
		}
		inline float3 poiRand3(float3 seed)
		{
			float3 p3 = frac(seed * 0.1031);
			p3 += dot(p3, p3.zyx + 31.32);
			return frac((p3.xxy + p3.yzz) * p3.zyx);
		}
		inline float3 poiRand3Range(float2 Seed, float Range)
		{
			float3 r = poiRand3(Seed);
			return (r * 2.0 - 1.0) * Range;
		}
		float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed, float timeOffset)
		{
			float3 rando = (float3(
			frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
			frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
			frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
			) * 2 - 1);
			float speed = 1 + wiggleSpeed;
			return float3(sin(((POI_TIME.x + timeOffset) + rando.x * UNITY_PI) * speed), sin(((POI_TIME.x + timeOffset) + rando.y * UNITY_PI) * speed), sin(((POI_TIME.x + timeOffset) + rando.z * UNITY_PI) * speed)) * Range;
		}
		static const float3 HCYwts = float3(0.299, 0.587, 0.114);
		static const float HCLgamma = 3;
		static const float HCLy0 = 100;
		static const float HCLmaxL = 0.530454533953517; // == exp(HCLgamma / HCLy0) - 0.5
		static const float3 wref = float3(1.0, 1.0, 1.0);
		#define TAU 6.28318531
		float3 HUEtoRGB(in float H)
		{
			float R = abs(H * 6 - 3) - 1;
			float G = 2 - abs(H * 6 - 2);
			float B = 2 - abs(H * 6 - 4);
			return saturate(float3(R, G, B));
		}
		float3 RGBtoHCV(in float3 RGB)
		{
			float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
			float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
			float C = Q.x - min(Q.w, Q.y);
			float H = abs((Q.w - Q.y) / (6 * C + EPSILON) + Q.z);
			return float3(H, C, Q.x);
		}
		float3 RGBtoHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}
		float3 HSVtoRGB(float3 c)
		{
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
		}
		void DecomposeHDRColor(in float3 linearColorHDR, out float3 baseLinearColor, out float exposure)
		{
			float maxColorComponent = max(linearColorHDR.r, max(linearColorHDR.g, linearColorHDR.b));
			bool isSDR = maxColorComponent <= 1.0;
			float scaleFactor = isSDR ? 1.0 : (1.0 / maxColorComponent);
			exposure = isSDR ? 0.0 : log(maxColorComponent) * 1.44269504089; // ln(2)
			baseLinearColor = scaleFactor * linearColorHDR;
		}
		float3 ApplyHDRExposure(float3 linearColor, float exposure)
		{
			return linearColor * pow(2, exposure);
		}
		float3 ModifyViaHSV(float3 color, float h, float s, float v)
		{
			float3 colorHSV = RGBtoHSV(color);
			colorHSV.x = frac(colorHSV.x + h);
			colorHSV.y = saturate(colorHSV.y + s);
			colorHSV.z = saturate(colorHSV.z + v);
			return HSVtoRGB(colorHSV);
		}
		float3 ModifyViaHSV(float3 color, float3 HSVMod)
		{
			return ModifyViaHSV(color, HSVMod.x, HSVMod.y, HSVMod.z);
		}
		float4x4 brightnessMatrix(float brightness)
		{
			return float4x4(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			brightness, brightness, brightness, 1
			);
		}
		float4x4 contrastMatrix(float contrast)
		{
			float t = (1.0 - contrast) / 2.0;
			return float4x4(
			contrast, 0, 0, 0,
			0, contrast, 0, 0,
			0, 0, contrast, 0,
			t, t, t, 1
			);
		}
		float4x4 saturationMatrix(float saturation)
		{
			float3 luminance = float3(0.3086, 0.6094, 0.0820);
			float oneMinusSat = 1.0 - saturation;
			float3 red = luminance.x * oneMinusSat;
			red += float3(saturation, 0, 0);
			float3 green = luminance.y * oneMinusSat;
			green += float3(0, saturation, 0);
			float3 blue = luminance.z * oneMinusSat;
			blue += float3(0, 0, saturation);
			return float4x4(
			red, 0,
			green, 0,
			blue, 0,
			0, 0, 0, 1
			);
		}
		float4 PoiColorBCS(float4 color, float brightness, float contrast, float saturation)
		{
			return mul(color, mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation))));
		}
		float3 PoiColorBCS(float3 color, float brightness, float contrast, float saturation)
		{
			return mul(float4(color, 1), mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation)))).rgb;
		}
		float3 linear_srgb_to_oklab(float3 c)
		{
			float l = 0.4122214708 * c.x + 0.5363325363 * c.y + 0.0514459929 * c.z;
			float m = 0.2119034982 * c.x + 0.6806995451 * c.y + 0.1073969566 * c.z;
			float s = 0.0883024619 * c.x + 0.2817188376 * c.y + 0.6299787005 * c.z;
			float l_ = pow(l, 1.0 / 3.0);
			float m_ = pow(m, 1.0 / 3.0);
			float s_ = pow(s, 1.0 / 3.0);
			return float3(
			0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
			1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
			0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
			);
		}
		float3 oklab_to_linear_srgb(float3 c)
		{
			float l_ = c.x + 0.3963377774 * c.y + 0.2158037573 * c.z;
			float m_ = c.x - 0.1055613458 * c.y - 0.0638541728 * c.z;
			float s_ = c.x - 0.0894841775 * c.y - 1.2914855480 * c.z;
			float l = l_ * l_ * l_;
			float m = m_ * m_ * m_;
			float s = s_ * s_ * s_;
			return float3(
			+ 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
			- 1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
			- 0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
			);
		}
		float3 hueShiftOKLab(float3 color, float shift, float selectOrShift)
		{
			float3 oklab = linear_srgb_to_oklab(color);
			float chroma = length(oklab.yz);
			oklab.y = selectOrShift > 0.5 ? oklab.y : chroma;
			oklab.z = selectOrShift > 0.5 ? oklab.z : 0;
			float s, c;
			sincos(shift * UNITY_TWO_PI, s, c);
			oklab.yz = float2(c * oklab.y - s * oklab.z, s * oklab.y + c * oklab.z);
			return oklab_to_linear_srgb(oklab);
		}
		float3 hueShiftHSV(float3 color, float hueOffset, float selectOrShift)
		{
			float3 hsvCol = RGBtoHSV(color);
			hsvCol.x = hsvCol.x * selectOrShift + hueOffset;
			return HSVtoRGB(hsvCol);
		}
		float3 hueShift(float3 color, float shift, float ColorSpace, float selectOrShift)
		{
			float3 oklab = hueShiftOKLab(color, shift, selectOrShift);
			float3 hsv = hueShiftHSV(color, shift, selectOrShift);
			float w = saturate(ColorSpace);
			return lerp(oklab, hsv, w);
		}
		float4 hueShift(float4 color, float shift, float ColorSpace, float selectOrShift)
		{
			return float4(hueShift(color.rgb, shift, ColorSpace, selectOrShift), color.a);
		}
		float4x4 poiRotationMatrixFromAngles(float x, float y, float z)
		{
			float angleX = radians(x);
			float c = cos(angleX);
			float s = sin(angleX);
			float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
			0, c, -s, 0,
			0, s, c, 0,
			0, 0, 0, 1);
			float angleY = radians(y);
			c = cos(angleY);
			s = sin(angleY);
			float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
			0, 1, 0, 0,
			- s, 0, c, 0,
			0, 0, 0, 1);
			float angleZ = radians(z);
			c = cos(angleZ);
			s = sin(angleZ);
			float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
			s, c, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1);
			return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
		}
		float4x4 poiRotationMatrixFromAngles(float3 angles)
		{
			float angleX = radians(angles.x);
			float c = cos(angleX);
			float s = sin(angleX);
			float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
			0, c, -s, 0,
			0, s, c, 0,
			0, 0, 0, 1);
			float angleY = radians(angles.y);
			c = cos(angleY);
			s = sin(angleY);
			float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
			0, 1, 0, 0,
			- s, 0, c, 0,
			0, 0, 0, 1);
			float angleZ = radians(angles.z);
			c = cos(angleZ);
			s = sin(angleZ);
			float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
			s, c, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1);
			return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
		}
		float3 _VRChatMirrorCameraPos;
		float3 getCameraPosition(bool useCenterEye = true)
		{
			float3 camPosWS = _VRChatMirrorMode == 1 ? _VRChatMirrorCameraPos : _WorldSpaceCameraPos;
			#ifdef USING_STEREO_MATRICES
			float3 centerEyePos = unity_StereoWorldSpaceCameraPos[0] * .5 + unity_StereoWorldSpaceCameraPos[1] * .5;
			camPosWS = (useCenterEye) ? centerEyePos : camPosWS;
			#endif
			return camPosWS;
		}
		float2 calcPixelScreenUVs(half4 grabPos)
		{
			half2 uv = grabPos.xy / (grabPos.w + 0.0000000001);
			#if UNITY_SINGLE_PASS_STEREO
			uv.xy *= half2(_ScreenParams.x * 2, _ScreenParams.y);
			#else
			uv.xy *= _ScreenParams.xy;
			#endif
			return uv;
		}
		float CalcMipLevel(float2 texture_coord)
		{
			float2 dx = ddx(texture_coord);
			float2 dy = ddy(texture_coord);
			float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));
			return 0.5 * log2(delta_max_sqr);
		}
		float inverseLerp(float A, float B, float T)
		{
			return (T - A) / (B - A);
		}
		float inverseLerp2(float2 a, float2 b, float2 value)
		{
			float2 AB = b - a;
			float2 AV = value - a;
			return dot(AV, AB) / dot(AB, AB);
		}
		float inverseLerp3(float3 a, float3 b, float3 value)
		{
			float3 AB = b - a;
			float3 AV = value - a;
			return dot(AV, AB) / dot(AB, AB);
		}
		float inverseLerp4(float4 a, float4 b, float4 value)
		{
			float4 AB = b - a;
			float4 AV = value - a;
			return dot(AV, AB) / dot(AB, AB);
		}
		float4 QuaternionFromMatrix(
		float m00, float m01, float m02,
		float m10, float m11, float m12,
		float m20, float m21, float m22)
		{
			float4 q;
			float trace = m00 + m11 + m22;
			if (trace > 0)
			{
				float s = sqrt(trace + 1) * 2;
				q.w = 0.25 * s;
				q.x = (m21 - m12) / s;
				q.y = (m02 - m20) / s;
				q.z = (m10 - m01) / s;
			}
			else if (m00 > m11 && m00 > m22)
			{
				float s = sqrt(1 + m00 - m11 - m22) * 2;
				q.w = (m21 - m12) / s;
				q.x = 0.25 * s;
				q.y = (m01 + m10) / s;
				q.z = (m02 + m20) / s;
			}
			else if (m11 > m22)
			{
				float s = sqrt(1 + m11 - m00 - m22) * 2;
				q.w = (m02 - m20) / s;
				q.x = (m01 + m10) / s;
				q.y = 0.25 * s;
				q.z = (m12 + m21) / s;
			}
			else
			{
				float s = sqrt(1 + m22 - m00 - m11) * 2;
				q.w = (m10 - m01) / s;
				q.x = (m02 + m20) / s;
				q.y = (m12 + m21) / s;
				q.z = 0.25 * s;
			}
			return q;
		}
		float4 MulQuat(float4 a, float4 b)
		{
			return float4(
			a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
			a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
			a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
			a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
			);
		}
		float4 QuaternionFromBasis(float3 sx, float3 sy, float3 sz)
		{
			return QuaternionFromMatrix(
			sx.x, sy.x, sz.x,
			sx.y, sy.y, sz.y,
			sx.z, sy.z, sz.z
			);
		}
		float4 BuildQuatFromForwardUp(float3 forward, float3 up)
		{
			float3 f = normalize(forward);
			float3 u = normalize(up);
			float3 x = normalize(cross(u, f));
			float3 y = cross(f, x);
			return QuaternionFromBasis(x, y, f);
		}
		float3 QuaternionToEuler(float4 q)
		{
			float3 euler;
			float sinr_cosp = 2 * (q.w * q.z + q.x * q.y);
			float cosr_cosp = 1 - 2 * (q.z * q.z + q.x * q.x);
			euler.z = atan2(sinr_cosp, cosr_cosp) * 57.2958;
			float sinp = 2 * (q.w * q.x - q.y * q.z);
			if (abs(sinp) >= 1)
			euler.x = (sinp >= 0 ? 1 : - 1) * 90;
			else
			euler.x = asin(sinp) * 57.2958;
			float siny_cosp = 2 * (q.w * q.y + q.z * q.x);
			float cosy_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
			euler.y = atan2(siny_cosp, cosy_cosp) * 57.2958;
			return euler;
		}
		float4 EulerToQuaternion(float3 euler)
		{
			float3 eulerRad = euler * 0.0174533;
			float cx = cos(eulerRad.x * 0.5);
			float sx = sin(eulerRad.x * 0.5);
			float cy = cos(eulerRad.y * 0.5);
			float sy = sin(eulerRad.y * 0.5);
			float cz = cos(eulerRad.z * 0.5);
			float sz = sin(eulerRad.z * 0.5);
			float4 q;
			q.w = cx * cy * cz + sx * sy * sz;
			q.x = sx * cy * cz - cx * sy * sz;
			q.y = cx * sy * cz + sx * cy * sz;
			q.z = cx * cy * sz - sx * sy * cz;
			return q;
		}
		float4 quaternion_conjugate(float4 v)
		{
			return float4(
			v.x, -v.yzw
			);
		}
		float4 quaternion_mul(float4 v1, float4 v2)
		{
			float4 result1 = (v1.x * v2 + v1 * v2.x);
			float4 result2 = float4(
			- dot(v1.yzw, v2.yzw),
			cross(v1.yzw, v2.yzw)
			);
			return float4(result1 + result2);
		}
		float4 get_quaternion_from_angle(float3 axis, float angle)
		{
			float sn = sin(angle * 0.5);
			float cs = cos(angle * 0.5);
			return float4(axis * sn, cs);
		}
		float4 quaternion_from_vector(float3 inVec)
		{
			return float4(0.0, inVec);
		}
		float degree_to_radius(float degree)
		{
			return (
			degree / 180.0 * UNITY_PI
			);
		}
		float3 rotate_with_quaternion(float3 inVec, float3 rotation)
		{
			float4 qx = get_quaternion_from_angle(float3(1, 0, 0), radians(rotation.x));
			float4 qy = get_quaternion_from_angle(float3(0, 1, 0), radians(rotation.y));
			float4 qz = get_quaternion_from_angle(float3(0, 0, 1), radians(rotation.z));
			#define MUL3(A, B, C) quaternion_mul(quaternion_mul((A), (B)), (C))
			float4 quaternion = normalize(MUL3(qx, qy, qz));
			float4 conjugate = quaternion_conjugate(quaternion);
			float4 inVecQ = quaternion_from_vector(inVec);
			float3 rotated = (
			MUL3(quaternion, inVecQ, conjugate)
			).yzw;
			return rotated;
		}
		float3 RotateByQuaternion(float4 q, float3 v)
		{
			float3 u = q.xyz;
			float s = q.w;
			return 2.0 * dot(u, v) * u
			+ (s * s - dot(u, u)) * v
			+ 2.0 * s * cross(u, v);
		}
		float4 SlerpQuaternion(float4 qa, float4 qb, float t)
		{
			float cosHalfTheta = dot(qa, qb);
			if (cosHalfTheta < 0.0)
			{
				qb = -qb;
				cosHalfTheta = -cosHalfTheta;
			}
			if (cosHalfTheta > 0.9995)
			{
				float4 qr = normalize(qa * (1 - t) + qb * t);
				return qr;
			}
			float halfTheta = acos(cosHalfTheta);
			float sinHalfTheta = sqrt(1.0 - cosHalfTheta * cosHalfTheta);
			float a = sin((1 - t) * halfTheta) / sinHalfTheta;
			float b = sin(t * halfTheta) / sinHalfTheta;
			return qa * a + qb * b;
		}
		float4 transform(float4 input, float4 pos, float4 rotation, float4 scale)
		{
			input.rgb *= (scale.xyz * scale.w);
			input = float4(rotate_with_quaternion(input.xyz, rotation.xyz * rotation.w) + (pos.xyz * pos.w), input.w);
			return input;
		}
		float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
		{
			float RotateUV_ang = _radian;
			float RotateUV_cos = cos(_time * RotateUV_ang);
			float RotateUV_sin = sin(_time * RotateUV_ang);
			return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
		}
		float3 RotateAroundAxis(float3 original, float3 axis, float radian)
		{
			float s = sin(radian);
			float c = cos(radian);
			float one_minus_c = 1.0 - c;
			axis = normalize(axis);
			float3x3 rot_mat = {
				one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
				one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
				one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
			};
			return mul(rot_mat, original);
		}
		float3 lilToneCorrection(float3 c, float4 hsvg)
		{
			c = pow(abs(c), hsvg.w);
			float4 p = (c.b > c.g) ? float4(c.bg, -1.0, 2.0 / 3.0) : float4(c.gb, 0.0, -1.0 / 3.0);
			float4 q = (p.x > c.r) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);
			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			hsv = float3(hsv.x + hsvg.x, saturate(hsv.y * hsvg.y), saturate(hsv.z * hsvg.z));
			return hsv.z - hsv.z * hsv.y + hsv.z * hsv.y * saturate(abs(frac(hsv.x + float3(1.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
		}
		float3 lilBlendColor(float3 dstCol, float3 srcCol, float3 srcA, int blendMode)
		{
			float3 ad = dstCol + srcCol;
			float3 mu = dstCol * srcCol;
			float3 outCol = float3(0, 0, 0);
			if (blendMode == 0) outCol = srcCol; // Normal
			if (blendMode == 1) outCol = ad; // Add
			if (blendMode == 2) outCol = max(ad - mu, dstCol); // Screen
			if (blendMode == 3) outCol = mu; // Multiply
			return lerp(dstCol, outCol, srcA);
		}
		float lilIsIn0to1(float f)
		{
			float value = 0.5 - abs(f - 0.5);
			return saturate(value / clamp(fwidth(value), 0.0001, 1.0));
		}
		float lilIsIn0to1(float f, float nv)
		{
			float value = 0.5 - abs(f - 0.5);
			return saturate(value / clamp(fwidth(value), 0.0001, nv));
		}
		float poiEdgeLinearNoSaturate(float value, float border)
		{
			return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
		}
		float3 poiEdgeLinearNoSaturate(float value, float3 border)
		{
			return float3(
			(value - border.x) / clamp(fwidth(value), 0.0001, 1.0),
			(value - border.y) / clamp(fwidth(value), 0.0001, 1.0),
			(value - border.z) / clamp(fwidth(value), 0.0001, 1.0)
			);
		}
		float poiEdgeLinearNoSaturate(float value, float border, float blur)
		{
			float borderMin = saturate(border - blur * 0.5);
			float borderMax = saturate(border + blur * 0.5);
			return (value - borderMin) / max(saturate(borderMax - borderMin + fwidth(value)), .0001);
		}
		float poiEdgeLinearNoSaturate(float value, float border, float blur, float borderRange)
		{
			float borderMin = saturate(border - blur * 0.5 - borderRange);
			float borderMax = saturate(border + blur * 0.5);
			return (value - borderMin) / max(saturate(borderMax - borderMin + fwidth(value)), .0001);
		}
		float poiEdgeNonLinearNoSaturate(float value, float border)
		{
			float fwidthValue = fwidth(value);
			return smoothstep(border - fwidthValue, border + fwidthValue, value);
		}
		float poiEdgeNonLinearNoSaturate(float value, float border, float blur)
		{
			float fwidthValue = fwidth(value);
			float borderMin = saturate(border - blur * 0.5);
			float borderMax = saturate(border + blur * 0.5);
			return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
		}
		float poiEdgeNonLinearNoSaturate(float value, float border, float blur, float borderRange)
		{
			float fwidthValue = fwidth(value);
			float borderMin = saturate(border - blur * 0.5 - borderRange);
			float borderMax = saturate(border + blur * 0.5);
			return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
		}
		float poiEdgeNonLinear(float value, float border)
		{
			return saturate(poiEdgeNonLinearNoSaturate(value, border));
		}
		float poiEdgeNonLinear(float value, float border, float blur)
		{
			return saturate(poiEdgeNonLinearNoSaturate(value, border, blur));
		}
		float poiEdgeNonLinear(float value, float border, float blur, float borderRange)
		{
			return saturate(poiEdgeNonLinearNoSaturate(value, border, blur, borderRange));
		}
		float poiEdgeLinear(float value, float border)
		{
			return saturate(poiEdgeLinearNoSaturate(value, border));
		}
		float poiEdgeLinear(float value, float border, float blur)
		{
			return saturate(poiEdgeLinearNoSaturate(value, border, blur));
		}
		float poiEdgeLinear(float value, float border, float blur, float borderRange)
		{
			return saturate(poiEdgeLinearNoSaturate(value, border, blur, borderRange));
		}
		float3 OpenLitLinearToSRGB(float3 col)
		{
			return LinearToGammaSpace(col);
		}
		float3 OpenLitSRGBToLinear(float3 col)
		{
			return GammaToLinearSpace(col);
		}
		float OpenLitLuminance(float3 rgb)
		{
			#if defined(UNITY_COLORSPACE_GAMMA)
			return dot(rgb, float3(0.22, 0.707, 0.071));
			#else
			return dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
			#endif
		}
		float3 AdjustLitLuminance(float3 rgb, float targetLuminance)
		{
			float currentLuminance;
			#if defined(UNITY_COLORSPACE_GAMMA)
			currentLuminance = dot(rgb, float3(0.22, 0.707, 0.071));
			#else
			currentLuminance = dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
			#endif
			float luminanceRatio = targetLuminance / currentLuminance;
			return rgb * luminanceRatio;
		}
		float3 ClampLuminance(float3 rgb, float minLuminance, float maxLuminance)
		{
			float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
			float minRatio = (currentLuminance != 0) ? minLuminance / currentLuminance : 1.0;
			float maxRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
			float luminanceRatio = clamp(min(maxRatio, max(minRatio, 1.0)), 0.0, 1.0);
			return lerp(rgb, rgb * luminanceRatio, luminanceRatio < 1.0);
		}
		float3 MaxLuminance(float3 rgb, float maxLuminance)
		{
			float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
			float luminanceRatio = (currentLuminance != 0) ? maxLuminance / max(currentLuminance, 0.00001) : 1.0;
			return lerp(rgb, rgb * luminanceRatio, currentLuminance > maxLuminance);
		}
		float OpenLitGray(float3 rgb)
		{
			return dot(rgb, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));
		}
		void OpenLitShadeSH9ToonDouble(float3 lightDirection, out float3 shMax, out float3 shMin)
		{
			#if !defined(LIGHTMAP_ON)
			float3 N = lightDirection * 0.666666;
			float4 vB = N.xyzz * N.yzzx;
			float3 res = float3(PoiSHAr.w, PoiSHAg.w, PoiSHAb.w);
			res.r += dot(PoiSHBr, vB);
			res.g += dot(PoiSHBg, vB);
			res.b += dot(PoiSHBb, vB);
			res += PoiSHC.rgb * (N.x * N.x - N.y * N.y);
			float3 l1;
			l1.r = dot(PoiSHAr.rgb, N);
			l1.g = dot(PoiSHAg.rgb, N);
			l1.b = dot(PoiSHAb.rgb, N);
			shMax = res + l1;
			shMin = res - l1;
			#if defined(UNITY_COLORSPACE_GAMMA)
			shMax = OpenLitLinearToSRGB(shMax);
			shMin = OpenLitLinearToSRGB(shMin);
			#endif
			#else
			shMax = 0.0;
			shMin = 0.0;
			#endif
		}
		float3 OpenLitComputeCustomLightDirection(float4 lightDirectionOverride)
		{
			float3 customDir = length(lightDirectionOverride.xyz) * normalize(mul((float3x3)unity_ObjectToWorld, lightDirectionOverride.xyz));
			return lightDirectionOverride.w ? customDir : lightDirectionOverride.xyz; // .w isn't doc'd anywhere and is always 0 unless end user changes it
		}
		float3 OpenLitLightingDirectionForSH9(float3 mainDir)
		{
			#if !defined(LIGHTMAP_ON)
			float3 sh9Dir = PoiSHAr.xyz * 0.333333 + PoiSHAg.xyz * 0.333333 + PoiSHAb.xyz * 0.333333;
			float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
			#else
			float3 sh9Dir = 0;
			float3 sh9DirAbs = 0;
			#endif
			float3 lightDirectionForSH9 = sh9Dir + mainDir;
			lightDirectionForSH9 = dot(lightDirectionForSH9, lightDirectionForSH9) < 0.000001 ? 0 : normalize(lightDirectionForSH9);
			return lightDirectionForSH9;
		}
		float3 OpenLitLightingDirectionForSH9(Light light)
		{
			float3 mainDir = light.direction * OpenLitLuminance(light.color);
			return OpenLitLightingDirectionForSH9(mainDir);
		}
		float3 OpenLitLightingDirection(float4 lightDirectionOverride, float3 mainDir)
		{
			#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
			float3 sh9Dir = PoiSHAr.xyz * 0.333333 + PoiSHAg.xyz * 0.333333 + PoiSHAb.xyz * 0.333333;
			float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
			#else
			float3 sh9Dir = 0;
			float3 sh9DirAbs = 0;
			#endif
			float3 customDir = OpenLitComputeCustomLightDirection(lightDirectionOverride);
			return normalize(sh9DirAbs + mainDir + customDir);
		}
		float3 OpenLitLightingDirection(float4 lightDirectionOverride, Light light)
		{
			float3 mainDir = light.direction.xyz * OpenLitLuminance(light.color);
			return OpenLitLightingDirection(lightDirectionOverride, mainDir);
		}
		float3 OpenLitLightingDirection(Light light)
		{
			float4 customDir = float4(0.001, 0.002, 0.001, 0.0);
			return OpenLitLightingDirection(customDir, light);
		}
		inline float4 CalculateFrustumCorrection()
		{
			float x1 = -UNITY_MATRIX_P._31 / (UNITY_MATRIX_P._11 * UNITY_MATRIX_P._34);
			float x2 = -UNITY_MATRIX_P._32 / (UNITY_MATRIX_P._22 * UNITY_MATRIX_P._34);
			return float4(x1, x2, 0, UNITY_MATRIX_P._33 / UNITY_MATRIX_P._34 + x1 * UNITY_MATRIX_P._13 + x2 * UNITY_MATRIX_P._23);
		}
		inline float CorrectedLinearEyeDepth(float z, float correctionFactor)
		{
			return 1.f / (z / UNITY_MATRIX_P._34 + correctionFactor);
		}
		inline float GetFOVScale()
		{
			return abs(UNITY_MATRIX_P._m11) * 0.5;
		}
		inline float GetAspectRatio()
		{
			return _ScreenParams.y / _ScreenParams.x;
		}
		inline float WorldToScreenScale(float worldRadius, float distance)
		{
			return (worldRadius / max(distance, 0.001)) * GetFOVScale();
		}
		inline float2 GetScreenAspectCorrection()
		{
			return float2(_ScreenParams.y / _ScreenParams.x, 1.0);
		}
		float evalRamp4(float time, float4 ramp)
		{
			return lerp(ramp.x, ramp.y, smoothstep(ramp.z, ramp.w, time));
		}
		float2 sharpSample(float4 texelSize, float2 p)
		{
			p = p * texelSize.zw;
			float2 c = max(0.0, fwidth(p));
			p = floor(p) + saturate(frac(p) / c);
			p = (p - 0.5) * texelSize.xy;
			return p;
		}
		void applyToGlobalMask(inout PoiMods poiMods, int index, int blendType, float val)
		{
			float v0 = saturate(maskBlend(poiMods.globalMask[0], val, blendType));
			float v1 = saturate(maskBlend(poiMods.globalMask[1], val, blendType));
			float v2 = saturate(maskBlend(poiMods.globalMask[2], val, blendType));
			float v3 = saturate(maskBlend(poiMods.globalMask[3], val, blendType));
			float v4 = saturate(maskBlend(poiMods.globalMask[4], val, blendType));
			float v5 = saturate(maskBlend(poiMods.globalMask[5], val, blendType));
			float v6 = saturate(maskBlend(poiMods.globalMask[6], val, blendType));
			float v7 = saturate(maskBlend(poiMods.globalMask[7], val, blendType));
			float v8 = saturate(maskBlend(poiMods.globalMask[8], val, blendType));
			float v9 = saturate(maskBlend(poiMods.globalMask[9], val, blendType));
			float v10 = saturate(maskBlend(poiMods.globalMask[10], val, blendType));
			float v11 = saturate(maskBlend(poiMods.globalMask[11], val, blendType));
			float v12 = saturate(maskBlend(poiMods.globalMask[12], val, blendType));
			float v13 = saturate(maskBlend(poiMods.globalMask[13], val, blendType));
			float v14 = saturate(maskBlend(poiMods.globalMask[14], val, blendType));
			float v15 = saturate(maskBlend(poiMods.globalMask[15], val, blendType));
			float i = index;
			float w0 = saturate(1 - abs(i - 0));
			float w1 = saturate(1 - abs(i - 1));
			float w2 = saturate(1 - abs(i - 2));
			float w3 = saturate(1 - abs(i - 3));
			float w4 = saturate(1 - abs(i - 4));
			float w5 = saturate(1 - abs(i - 5));
			float w6 = saturate(1 - abs(i - 6));
			float w7 = saturate(1 - abs(i - 7));
			float w8 = saturate(1 - abs(i - 8));
			float w9 = saturate(1 - abs(i - 9));
			float w10 = saturate(1 - abs(i - 10));
			float w11 = saturate(1 - abs(i - 11));
			float w12 = saturate(1 - abs(i - 12));
			float w13 = saturate(1 - abs(i - 13));
			float w14 = saturate(1 - abs(i - 14));
			float w15 = saturate(1 - abs(i - 15));
			poiMods.globalMask[0] = lerp(poiMods.globalMask[0], v0, w0);
			poiMods.globalMask[1] = lerp(poiMods.globalMask[1], v1, w1);
			poiMods.globalMask[2] = lerp(poiMods.globalMask[2], v2, w2);
			poiMods.globalMask[3] = lerp(poiMods.globalMask[3], v3, w3);
			poiMods.globalMask[4] = lerp(poiMods.globalMask[4], v4, w4);
			poiMods.globalMask[5] = lerp(poiMods.globalMask[5], v5, w5);
			poiMods.globalMask[6] = lerp(poiMods.globalMask[6], v6, w6);
			poiMods.globalMask[7] = lerp(poiMods.globalMask[7], v7, w7);
			poiMods.globalMask[8] = lerp(poiMods.globalMask[8], v8, w8);
			poiMods.globalMask[9] = lerp(poiMods.globalMask[9], v9, w9);
			poiMods.globalMask[10] = lerp(poiMods.globalMask[10], v10, w10);
			poiMods.globalMask[11] = lerp(poiMods.globalMask[11], v11, w11);
			poiMods.globalMask[12] = lerp(poiMods.globalMask[12], v12, w12);
			poiMods.globalMask[13] = lerp(poiMods.globalMask[13], v13, w13);
			poiMods.globalMask[14] = lerp(poiMods.globalMask[14], v14, w14);
			poiMods.globalMask[15] = lerp(poiMods.globalMask[15], v15, w15);
		}
		void assignValueToVectorFromIndex(inout float4 vec, int index, float value)
		{
			float i = clamp(index, 0, 3);
			float w0 = saturate(1 - abs(i - 0));
			float w1 = saturate(1 - abs(i - 1));
			float w2 = saturate(1 - abs(i - 2));
			float w3 = saturate(1 - abs(i - 3));
			vec[0] = lerp(vec[0], value, w0);
			vec[1] = lerp(vec[1], value, w1);
			vec[2] = lerp(vec[2], value, w2);
			vec[3] = lerp(vec[3], value, w3);
		}
		float3 mod289(float3 x)
		{
			return x - floor(x * (1.0 / 289.0)) * 289.0;
		}
		float2 mod289(float2 x)
		{
			return x - floor(x * (1.0 / 289.0)) * 289.0;
		}
		float3 permute(float3 x)
		{
			return mod289(((x * 34.0) + 1.0) * x);
		}
		float snoise(float2 v)
		{
			const float4 C = float4(0.211324865405187, // (3.0 - sqrt(3.0)) / 6.0
			0.366025403784439, // 0.5 * (sqrt(3.0) - 1.0)
			- 0.577350269189626, // - 1.0 + 2.0 * C.x
			0.024390243902439); // 1.0 / 41.0
			float2 i = floor(v + dot(v, C.yy));
			float2 x0 = v - i + dot(i, C.xx);
			float2 i1;
			i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod289(i); // Avoid truncation effects in permutation
			float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
			+ i.x + float3(0.0, i1.x, 1.0));
			float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
			m = m * m ;
			m = m * m ;
			float3 x = 2.0 * frac(p * C.www) - 1.0;
			float3 h = abs(x) - 0.5;
			float3 ox = floor(x + 0.5);
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot(m, g);
		}
		float poiInvertToggle(in float value, in float toggle)
		{
			return (toggle == 0 ? value : 1 - value);
		}
		float3 PoiBlendNormal(float3 dstNormal, float3 srcNormal)
		{
			return float3(dstNormal.xy + srcNormal.xy, dstNormal.z * srcNormal.z);
		}
		float3 lilTransformDirOStoWS(float3 directionOS, bool doNormalize)
		{
			if (doNormalize) return normalize(mul((float3x3)unity_ObjectToWorld, directionOS));
			else            return mul((float3x3)unity_ObjectToWorld, directionOS);
		}
		float2 poiGetWidthAndHeight(Texture2D tex)
		{
			uint width, height;
			tex.GetDimensions(width, height);
			return float2(width, height);
		}
		float2 poiGetWidthAndHeight(Texture2DArray tex)
		{
			uint width, height, element;
			tex.GetDimensions(width, height, element);
			return float2(width, height);
		}
		bool SceneHasReflections()
		{
			bool hasReflection = false;
			float width, height;
			unity_SpecCube0.GetDimensions(width, height);
			hasReflection = !(width * height < 2);
			#if CLUSTER_HAS_REFLECTION_PROBES
			urp_ReflProbes_Atlas.GetDimensions(width, height);
			hasReflection = hasReflection | !(width * height < 2);
			#endif
			return hasReflection;
		}
		void applyUnityFog(inout float3 col, float fogCoord, float ignoreFog)
		{
			if (ignoreFog > 0.5) return;
			float z = UNITY_Z_0_FAR_FROM_CLIPSPACE(fogCoord);
			half fogIntensity = 1.0;
			if (unity_FogParams.z != unity_FogParams.w)
			{
				fogIntensity = saturate(z * unity_FogParams.z + unity_FogParams.w);
			}
			else if (unity_FogParams.x != 0.0)
			{
				half fogFactor = unity_FogParams.x * z;
				fogIntensity = saturate(exp2(-fogFactor * fogFactor));
			}
			float3 appliedFogColor = unity_FogColor.rgb;
			#if defined(POI_PASS_ADD)
			appliedFogColor = float3(0, 0, 0);
			#endif
			col.rgb = lerp(appliedFogColor, col.rgb, fogIntensity);
		}
		ENDCG
		Pass
		{
			Name "Base"
			Tags { "LightMode" = "ForwardBase" }
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilCompareFunction]
				Pass [_StencilPassOp]
				Fail [_StencilFailOp]
				ZFail [_StencilZFailOp]
			}
			ZWrite [_ZWrite]
			Cull Back
			ZTest [_ZTest]
			ColorMask RGBA
			Offset [_OffsetFactor], [_OffsetUnits]
			BlendOp [_BlendOp], [_BlendOpAlpha]
			Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
			CGPROGRAM
 #define FINALPASS 
 #define POI_EMISSION_1 
 #define POI_STYLIZED_StylizedSpecular 
 #define _POI_DEPTH_RIMLIGHT 
 #define _STOCHASTICMODE_DELIOT_HEITZ 
 #define _STYLIZEDREFLECTIONMODE_UNITYCHAN 
 #define _SUNDISK_SIMPLE 
 #define PROP_DETAILTEX 
 #define PROP_DETAILNORMALMAP 
 #define OPTIMIZER_ENABLED 
			#pragma target 5.0
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3 _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH _DECAL_LAYERS
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION _USE_FAST_SRGB_LINEAR_CONVERSION _LIGHT_LAYERS
			#if !defined(POI_WORLD)
			#pragma skip_variants _ADDITIONAL_LIGHTS_VERTEX LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#if POI_PIPE == POI_URP
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _CLUSTER_LIGHT_LOOP _FORWARD_PLUS
			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
			#pragma dynamic_branch _ FOG_LINEAR
			#pragma dynamic_branch _ FOG_EXP
			#pragma dynamic_branch _ FOG_EXP2
			#endif
			#if POI_PIPE == POI_BIRP
			#pragma multi_compile_fwdbase
			#pragma dynamic_branch _ FOG_LINEAR
			#pragma dynamic_branch _ FOG_EXP
			#pragma dynamic_branch _ FOG_EXP2
			#pragma multi_compile_fragment _ VERTEXLIGHT_ON
			#endif
			#ifdef POI_WORLD
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#endif
			#pragma multi_compile_instancing
			#define POI_PASS_BASE
			#ifndef VRC_LIGHT_VOLUMES_INCLUDED
			#define VRC_LIGHT_VOLUMES_INCLUDED
			#define VRCLV_VERSION 2
			#define VRCLV_MAX_VOLUMES_COUNT 32
			#define VRCLV_MAX_LIGHTS_COUNT 128
			#ifndef SHADER_TARGET_SURFACE_ANALYSIS
			cbuffer LightVolumeUniforms {
				#endif
				uniform float _UdonLightVolumeEnabled;
				uniform float _UdonLightVolumeVersion;
				uniform float _UdonLightVolumeCount;
				uniform float _UdonLightVolumeAdditiveMaxOverdraw;
				uniform float _UdonLightVolumeAdditiveCount;
				uniform float _UdonLightVolumeProbesBlend;
				uniform float _UdonLightVolumeSharpBounds;
				uniform float4x4 _UdonLightVolumeInvWorldMatrix[VRCLV_MAX_VOLUMES_COUNT];
				uniform float4 _UdonLightVolumeRotation[VRCLV_MAX_VOLUMES_COUNT * 2]; // Legacy! Used in this version to have back compatibility with older worlds. Array commented above will be used in future releases! Legacy!
				uniform float3 _UdonLightVolumeInvLocalEdgeSmooth[VRCLV_MAX_VOLUMES_COUNT];
				uniform float3 _UdonLightVolumeUvw[VRCLV_MAX_VOLUMES_COUNT * 6]; // Legacy! AABB Bounds of islands on the 3D Texture atlas. Array commented above will be used in future releases! Legacy!
				uniform float4 _UdonLightVolumeOcclusionUvw[VRCLV_MAX_VOLUMES_COUNT];
				uniform float4 _UdonLightVolumeColor[VRCLV_MAX_VOLUMES_COUNT];
				uniform float _UdonPointLightVolumeCount;
				uniform float _UdonPointLightVolumeCubeCount;
				uniform float4 _UdonPointLightVolumePosition[VRCLV_MAX_LIGHTS_COUNT];
				uniform float4 _UdonPointLightVolumeColor[VRCLV_MAX_LIGHTS_COUNT];
				uniform float4 _UdonPointLightVolumeDirection[VRCLV_MAX_LIGHTS_COUNT];
				uniform float3 _UdonPointLightVolumeCustomID[VRCLV_MAX_LIGHTS_COUNT];
				uniform float _UdonLightBrightnessCutoff;
				uniform float _UdonLightVolumeOcclusionCount;
				#ifndef SHADER_TARGET_SURFACE_ANALYSIS
			}
			#endif
			#ifndef SHADER_TARGET_SURFACE_ANALYSIS
			uniform Texture3D _UdonLightVolume;
			uniform SamplerState sampler_UdonLightVolume;
			uniform Texture2DArray _UdonPointLightVolumeTexture;
			#define LV_SAMPLE(tex, uvw) tex.SampleLevel(sampler_UdonLightVolume, uvw, 0)
			#else
			#define LV_SAMPLE(tex, uvw) float4(0,0,0,0)
			#endif
			#define LV_PI 3.141592653589793f
			#define LV_PI2 6.283185307179586f
			float LV_Smoothstep01(float x) {
				return x * x * (3 - 2 * x);
			}
			float3 LV_MultiplyVectorByQuaternion(float3 v, float4 q) {
				float3 t = 2.0 * cross(q.xyz, v);
				return v + q.w * t + cross(q.xyz, t);
			}
			float3 LV_MultiplyVectorByMatrix2x3(float3 v, float3 r0, float3 r1) {
				float3 r2 = cross(r0, r1);
				return float3(dot(v, r0), dot(v, r1), dot(v, r2));
			}
			float LV_FastAcos(float x) {
				float absX = abs(x);
				float res = -0.156583f * absX + LV_PI * 0.5f;
				res *= sqrt(1.0f - absX);
				return (x >= 0) ? res : (LV_PI - res);
			}
			float LV_DistributionGGX(float NoH, float roughness) {
				float f = (roughness - 1) * ((roughness + 1) * (NoH * NoH)) + 1;
				return (roughness * roughness) / ((float) LV_PI * f * f);
			}
			bool LV_PointLocalAABB(float3 localUVW) {
				return all(abs(localUVW) <= 0.5);
			}
			float3 LV_LocalFromVolume(uint volumeID, float3 worldPos) {
				return mul(_UdonLightVolumeInvWorldMatrix[volumeID], float4(worldPos, 1.0)).xyz;
			}
			float LV_EvaluateSH(float L0, float3 L1, float3 n) {
				return L0 + dot(L1, n);
			}
			float4 LV_SampleCubemapArray(uint id, float3 dir) {
				float3 absDir = abs(dir);
				float2 uv;
				uint face;
				if (absDir.x >= absDir.y && absDir.x >= absDir.z) {
					face = dir.x > 0 ? 0 : 1;
					uv = float2((dir.x > 0 ? -dir.z : dir.z), -dir.y) * rcp(absDir.x);
				} else if (absDir.y >= absDir.z) {
					face = dir.y > 0 ? 2 : 3;
					uv = float2(dir.x, (dir.y > 0 ? dir.z : -dir.z)) * rcp(absDir.y);
				} else {
					face = dir.z > 0 ? 4 : 5;
					uv = float2((dir.z > 0 ? dir.x : -dir.x), -dir.y) * rcp(absDir.z);
				}
				float3 uvid = float3(uv * 0.5 + 0.5, id * 6 + face);
				return LV_SAMPLE(_UdonPointLightVolumeTexture, uvid);
			}
			float4 LV_ProjectQuadLightIrradianceSH(float3 shadingPosition, float3 lightVertices[4]) {
				[unroll] for (uint edge0 = 0; edge0 < 4; edge0++) {
					lightVertices[edge0] = normalize(lightVertices[edge0] - shadingPosition);
				}
				const float3 zhDir0 = float3(0.866025, -0.500001, -0.000004);
				const float3 zhDir1 = float3(-0.759553, 0.438522, -0.480394);
				const float3 zhDir2 = float3(-0.000002, 0.638694,  0.769461);
				const float3 zhWeightL1y = float3(2.1995339f, 2.50785367f, 1.56572711f);
				const float3 zhWeightL1z = float3(-1.82572523f, -2.08165037f, 0.00000000f);
				const float3 zhWeightL1x = float3(2.42459869f, 1.44790525f, 0.90397552f);
				float solidAngle = 0.0;
				float3 surfaceIntegral = 0.0;
				[loop] for (uint edge1 = 0; edge1 < 4; edge1++) {
					uint next = (edge1 + 1) % 4;
					uint prev = (edge1 + 4 - 1) % 4;
					float3 prevVert = lightVertices[prev];
					float3 thisVert = lightVertices[edge1];
					float3 nextVert = lightVertices[next];
					float3 a = cross(thisVert, prevVert);
					float3 b = cross(thisVert, nextVert);
					float lenA = length(a);
					float lenB = length(b);
					solidAngle += LV_FastAcos(clamp(dot(a, b) / (lenA * lenB), -1, 1));
					float3 mu = b * rcp(lenB);
					float cosGamma = dot(thisVert, nextVert);
					float gamma = LV_FastAcos(clamp(cosGamma, -1, 1));
					surfaceIntegral.x += gamma * dot(zhDir0, mu);
					surfaceIntegral.y += gamma * dot(zhDir1, mu);
					surfaceIntegral.z += gamma * dot(zhDir2, mu);
				}
				solidAngle = solidAngle - LV_PI2;
				surfaceIntegral *= 0.5;
				const float normalizationL0 = 0.5f * sqrt(1.0f / LV_PI);
				float l0 = normalizationL0 * solidAngle;
				float l1y = dot(zhWeightL1y, surfaceIntegral);
				float l1z = dot(zhWeightL1z, surfaceIntegral);
				float l1x = dot(zhWeightL1x, surfaceIntegral);
				const float cosineKernelL0 = LV_PI; // (1)
				const float cosineKernelL1 = LV_PI2 / 3.0f; // (1)
				const float oneOverPi = 1.0f / LV_PI; // (2)
				const float normalizationL1 = 0.5f * sqrt(3.0f / LV_PI); // (3)
				const float weightL0 = cosineKernelL0 * normalizationL0 * oneOverPi; // (1), (2), (3)
				const float weightL1 = cosineKernelL1 * normalizationL1 * oneOverPi; // (1), (2), (3)
				l0  *= weightL0;
				l1y *= weightL1;
				l1z *= weightL1;
				l1x *= weightL1;
				return float4(l1x, l1y, l1z, l0);
			}
			void LV_QuadLight(float3 worldPos, float3 centroidPos, float4 rotationQuat, float2 size, float3 color, float sqMaxDist, float occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float3 lightToWorldPos = worldPos - centroidPos;
				float3 normal = LV_MultiplyVectorByQuaternion(float3(0, 0, 1), rotationQuat);
				 if (dot(normal, lightToWorldPos) < 0.0) return;
				float sqCutoffDist = sqMaxDist - dot(lightToWorldPos, lightToWorldPos);
				color.rgb *= saturate(sqCutoffDist / sqMaxDist) * LV_PI * occlusion;
				float2 halfSize = size * 0.5f;
				float3 xAxis = LV_MultiplyVectorByQuaternion(float3(1, 0, 0), rotationQuat);
				float3 yAxis = cross(normal, xAxis);
				float3 verts[4];
				verts[0] = centroidPos + (-halfSize.x * xAxis) + ( halfSize.y * yAxis);
				verts[1] = centroidPos + ( halfSize.x * xAxis) + ( halfSize.y * yAxis);
				verts[2] = centroidPos + ( halfSize.x * xAxis) + (-halfSize.y * yAxis);
				verts[3] = centroidPos + (-halfSize.x * xAxis) + (-halfSize.y * yAxis);
				float4 areaLightSH = LV_ProjectQuadLightIrradianceSH(worldPos, verts);
				float lenL1 = length(areaLightSH.xyz);
				if (lenL1 > areaLightSH.w) areaLightSH.xyz *= areaLightSH.w / lenL1;
				L0  += areaLightSH.w * color.rgb;
				L1r += areaLightSH.xyz * color.r;
				L1g += areaLightSH.xyz * color.g;
				L1b += areaLightSH.xyz * color.b;
				count++;
			}
			float3 LV_PointLightAttenuation(float sqdist, float sqlightSize, float3 color, float brightnessCutoff, float sqMaxDist) {
				float mask = saturate(1 - sqdist / sqMaxDist);
				return mask * mask * color * sqlightSize / (sqdist + sqlightSize);
			}
			float LV_PointLightSolidAngle(float sqdist, float sqlightSize) {
				return saturate(sqrt(sqdist / (sqlightSize + sqdist)));
			}
			void LV_SphereLight(float sqdist, float3 dirN, float sqlightSize, float3 color, float occlusion, float sqMaxDist, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float3 att = LV_PointLightAttenuation(sqdist, sqlightSize, color, _UdonLightBrightnessCutoff, sqMaxDist);
				float3 l0 = att * occlusion;
				float3 l1 = dirN * LV_PointLightSolidAngle(sqdist, sqlightSize);
				L0 += l0;
				L1r += l0.r * l1;
				L1g += l0.g * l1;
				L1b += l0.b * l1;
				count++;
			}
			void LV_SphereSpotLight(float sqdist, float3 dirN, float sqlightSize, float3 att, float spotMask, float cosAngle, float coneFalloff, float occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float smoothedCone = LV_Smoothstep01(saturate(spotMask * coneFalloff));
				float3 l0 = att * (occlusion * smoothedCone);
				float3 l1 = dirN * LV_PointLightSolidAngle(sqdist, sqlightSize * saturate(1 - cosAngle));
				L0 += l0;
				L1r += l0.r * l1;
				L1g += l0.g * l1;
				L1b += l0.b * l1;
				count++;
			}
			void LV_SphereSpotLightCookie(float sqdist, float3 dirN, float sqlightSize, float3 att, float4 lightRot, float tanAngle, uint customId, float occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float3 localDir = LV_MultiplyVectorByQuaternion(-dirN, lightRot);
				float2 uv = localDir.xy * rcp(localDir.z * tanAngle);
				 if (
				localDir.z <= 0.0 || // Culling by direction
				abs(uv.x) > 1.0 || abs(uv.y) > 1.0 // Culling by UV
				) return;
				uint id = (uint) _UdonPointLightVolumeCubeCount * 5 - customId - 1;
				float3 uvid = float3(uv * 0.5 + 0.5, id);
				float angleSize = saturate(rsqrt(1 + tanAngle * tanAngle));
				float4 cookie = LV_SAMPLE(_UdonPointLightVolumeTexture, uvid);
				float3 l0 = att * cookie.rgb * (cookie.a * occlusion);
				float3 l1 = dirN * LV_PointLightSolidAngle(sqdist, sqlightSize * (1 - angleSize));
				L0 += l0;
				L1r += l0.r * l1;
				L1g += l0.g * l1;
				L1b += l0.b * l1;
				count++;
			}
			void LV_SphereSpotLightAttenuationLUT(float sqdist, float3 dirN, float sqlightSize, float3 color, float spotMask, float cosAngle, uint customId, float occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float dirRadius = sqdist * abs(sqlightSize);
				float spot = 1 - saturate(spotMask * rcp(1 - cosAngle));
				uint id = (uint) _UdonPointLightVolumeCubeCount * 5 + customId - 1;
				float3 uvid = float3(sqrt(float2(spot, dirRadius)), id);
				float3 att = color.rgb * LV_SAMPLE(_UdonPointLightVolumeTexture, uvid).xyz * occlusion;
				L0 += att;
				L1r += dirN * att.r;
				L1g += dirN * att.g;
				L1b += dirN * att.b;
				count++;
			}
			void LV_PointLight(uint id, float3 worldPos, float4 occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, inout uint count) {
				float3 customID_data = _UdonPointLightVolumeCustomID[id];
				int shadowId = (int) customID_data.y; // Shadowmask id
				int customId = (int) customID_data.x; // Custom Texture ID
				float sqrRange = customID_data.z; // Squared culling distance
				float4 pos = _UdonPointLightVolumePosition[id]; // Light position and inversed squared range
				float3 dir = pos.xyz - worldPos;
				float sqlen = max(dot(dir, dir), 1e-6);
				 if (sqlen > sqrRange) return; // Early distance based culling
				float3 dirN = dir * rsqrt(sqlen);
				float lightOcclusion = 1;
				if (_UdonLightVolumeOcclusionCount != 0 && shadowId >= 0) {
					lightOcclusion = dot(occlusion, float4(shadowId == 0, shadowId == 1, shadowId == 2, shadowId == 3));
				}
				float4 color = _UdonPointLightVolumeColor[id]; // Color, angle
				float4 ldir = _UdonPointLightVolumeDirection[id]; // Dir + falloff or Rotation
				 if (pos.w < 0) { // It is a spot light
					float angle = color.w;
					float spotMask = dot(ldir.xyz, -dirN) - angle;
					 if(customId >= 0 && spotMask < 0) return; // Spot cone based culling
					 if (customId > 0) {  // If it uses Attenuation LUT
						LV_SphereSpotLightAttenuationLUT(sqlen, dirN, -pos.w, color.rgb, spotMask, angle, customId, lightOcclusion, L0, L1r, L1g, L1b, count);
					} else { // If it uses default parametric attenuation
						float3 att = LV_PointLightAttenuation(sqlen, -pos.w, color.rgb, _UdonLightBrightnessCutoff, sqrRange);
						 if (customId < 0) { // If uses cookie
							LV_SphereSpotLightCookie(sqlen, dirN, -pos.w, att, ldir, angle, customId, lightOcclusion, L0, L1r, L1g, L1b, count);
						} else { // If it uses default parametric attenuation
							LV_SphereSpotLight(sqlen, dirN, -pos.w, att, spotMask, angle, ldir.w, lightOcclusion, L0, L1r, L1g, L1b, count);
						}
					}
				} else if (color.w <= 1.5f) { // It is a point light
					 if (customId > 0) { // Using LUT
						float invSqRange = abs(pos.w); // Sign of range defines if it's point light (positive) or a spot light (negative)
						float dirRadius = sqlen * invSqRange;
						uint id = (uint) _UdonPointLightVolumeCubeCount * 5 + customId;
						float3 uvid = float3(sqrt(float2(0, dirRadius)), id);
						float3 att = color.rgb * LV_SAMPLE(_UdonPointLightVolumeTexture, uvid).xyz * lightOcclusion;
						L0 += att;
						L1r += dirN * att.r;
						L1g += dirN * att.g;
						L1b += dirN * att.b;
						count++;
					} else { // If it uses default parametric attenuation
						float3 l0 = 0, l1r = 0, l1g = 0, l1b = 0;
						LV_SphereLight(sqlen, dirN, pos.w, color.rgb, lightOcclusion, sqrRange, l0, l1r, l1g, l1b, count);
						float3 cubeColor = 1;
						 if (customId < 0) { // If it uses a cubemap
							uint id = -customId - 1; // Cubemap ID starts from zero and should not take in count texture array slices count.
							cubeColor = LV_SampleCubemapArray(id, LV_MultiplyVectorByQuaternion(dirN, ldir)).xyz;
						}
						L0 += l0 * cubeColor;
						L1r += l1r * cubeColor.r;
						L1g += l1g * cubeColor.g;
						L1b += l1b * cubeColor.b;
					}
				} else { // It is an area light
					LV_QuadLight(worldPos, pos.xyz, ldir, float2(pos.w, color.w - 2.0f), color.rgb, sqrRange, lightOcclusion, L0, L1r, L1g, L1b, count);
				}
			}
			void LV_SampleLightVolumeTex(float3 uvw0, float3 uvw1, float3 uvw2, out float3 L0, out float3 L1r, out float3 L1g, out float3 L1b) {
				float4 tex0 = LV_SAMPLE(_UdonLightVolume, uvw0);
				float4 tex1 = LV_SAMPLE(_UdonLightVolume, uvw1);
				float4 tex2 = LV_SAMPLE(_UdonLightVolume, uvw2);
				L0 = tex0.rgb;
				L1r = float3(tex1.r, tex2.r, tex0.a);
				L1g = float3(tex1.g, tex2.g, tex1.a);
				L1b = float3(tex1.b, tex2.b, tex2.a);
			}
			float LV_BoundsMask(float3 localUVW, float3 invLocalEdgeSmooth) {
				float3 distToMin = (localUVW + 0.5) * invLocalEdgeSmooth;
				float3 distToMax = (0.5 - localUVW) * invLocalEdgeSmooth;
				float3 fade = saturate(min(distToMin, distToMax));
				return fade.x * fade.y * fade.z;
			}
			void LV_SampleLightProbe(inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b) {
				L0 += float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				L1r += unity_SHAr.xyz;
				L1g += unity_SHAg.xyz;
				L1b += unity_SHAb.xyz;
			}
			void LV_SampleLightProbeDering(inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b) {
				L0 += float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				L1r += unity_SHAr.xyz * 0.565f;
				L1g += unity_SHAg.xyz * 0.565f;
				L1b += unity_SHAb.xyz * 0.565f;
			}
			void LV_SampleVolume(uint id, float3 localUVW, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, out float4 occlusion) {
				uint uvwID = id * 6;
				float3 uvwScaled = saturate(localUVW + 0.5) * (_UdonLightVolumeUvw[uvwID + 1].xyz - _UdonLightVolumeUvw[uvwID].xyz);
				float3 uvw0 = uvwScaled + _UdonLightVolumeUvw[uvwID].xyz;
				float3 uvw1 = uvwScaled + _UdonLightVolumeUvw[uvwID + 2].xyz;
				float3 uvw2 = uvwScaled + _UdonLightVolumeUvw[uvwID + 4].xyz;
				float3 l0, l1r, l1g, l1b;
				LV_SampleLightVolumeTex(uvw0, uvw1, uvw2, l0, l1r, l1g, l1b);
				float4 uvwOcclusion = _UdonLightVolumeOcclusionUvw[id];
				 if (uvwOcclusion.x >= 0) {
					occlusion = 1.0f - LV_SAMPLE(_UdonLightVolume, uvwOcclusion.xyz + uvwScaled * uvwOcclusion.w);
				} else {
					occlusion = 1;
				}
				float4 color = _UdonLightVolumeColor[id];
				L0 += l0 * color.rgb;
				l1r *= color.r;
				l1g *= color.g;
				l1b *= color.b;
				if (color.a != 0) {
					float3 r0 = _UdonLightVolumeRotation[id * 2].xyz;
					float3 r1 = _UdonLightVolumeRotation[id * 2 + 1].xyz;
					L1r += LV_MultiplyVectorByMatrix2x3(l1r, r0, r1);
					L1g += LV_MultiplyVectorByMatrix2x3(l1g, r0, r1);
					L1b += LV_MultiplyVectorByMatrix2x3(l1b, r0, r1);
				} else {
					L1r += l1r;
					L1g += l1g;
					L1b += l1b;
				}
			}
			float4 LV_SampleVolumeOcclusion(uint id, float3 localUVW) {
				float4 uvwOcclusion = _UdonLightVolumeOcclusionUvw[id];
				 if (uvwOcclusion.x >= 0) {
					uint uvwID = id * 6;
					float3 uvwScaled = saturate(localUVW + 0.5) * (_UdonLightVolumeUvw[uvwID + 1].xyz - _UdonLightVolumeUvw[uvwID].xyz);
					return 1.0f - LV_SAMPLE(_UdonLightVolume, uvwOcclusion.xyz + uvwScaled * uvwOcclusion.w);
				} else {
					return 1;
				}
			}
			void LV_PointLightVolumeSH(float3 worldPos, float4 occlusion, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b) {
				uint pointCount = min((uint) _UdonPointLightVolumeCount, VRCLV_MAX_LIGHTS_COUNT);
				 if (pointCount == 0) return;
				uint maxOverdraw = min((uint) _UdonLightVolumeAdditiveMaxOverdraw, VRCLV_MAX_LIGHTS_COUNT);
				uint pcount = 0; // Point lights counter
				[loop] for (uint pid = 0; pid < pointCount && pcount < maxOverdraw; pid++) {
					LV_PointLight(pid, worldPos, occlusion, L0, L1r, L1g, L1b, pcount);
				}
			}
			void LV_LightVolumeSH(float3 worldPos, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, out float4 occlusion) {
				occlusion = 1;
				float4 mOcclusion = 1; // Multiplicative occlusion. Applies on top of regular occlusion
				uint volumesCount = min((uint) _UdonLightVolumeCount, VRCLV_MAX_VOLUMES_COUNT);
				 if (volumesCount == 0) { // Legacy! Fallback to default light probes if Light Volume are not enabled or a version is too old to have a support. Legacy!
					LV_SampleLightProbe(L0, L1r, L1g, L1b);
					return;
				}
				uint maxOverdraw = min((uint) _UdonLightVolumeAdditiveMaxOverdraw, VRCLV_MAX_VOLUMES_COUNT);
				uint additiveCount = min((uint) _UdonLightVolumeAdditiveCount, VRCLV_MAX_VOLUMES_COUNT);
				bool lightProbesBlend = _UdonLightVolumeProbesBlend;
				uint volumeID_A = -1; // Main, dominant volume ID
				uint volumeID_B = -1; // Secondary volume ID to blend main with
				float3 localUVW   = 0; // Last local UVW to use in disabled Light Probes mode
				float3 localUVW_A = 0; // Main local UVW
				float3 localUVW_B = 0; // Secondary local UVW
				bool isNoA = true;
				bool isNoB = true;
				uint addVolumesCount = 0;
				[loop] for (uint id = 0; id < volumesCount; id++) {
					localUVW = LV_LocalFromVolume(id, worldPos);
					 if (LV_PointLocalAABB(localUVW)) { // Intersection test
						 if (id < additiveCount) { // Sampling additive volumes
							 if (addVolumesCount < maxOverdraw) {
								float4 occ; // Multiplicative occlusion
								LV_SampleVolume(id, localUVW, L0, L1r, L1g, L1b, occ);
								mOcclusion *= occ;
								addVolumesCount++;
							}
						} else if (isNoA) { // First, searching for volume A
							volumeID_A = id;
							localUVW_A = localUVW;
							isNoA = false;
						} else { // Next, searching for volume B if A found
							volumeID_B = id;
							localUVW_B = localUVW;
							isNoB = false;
							break;
						}
					}
				}
				 if (isNoA && lightProbesBlend) {
					LV_SampleLightProbe(L0, L1r, L1g, L1b);
					occlusion *= mOcclusion;
					return;
				}
				localUVW_A = isNoA ? localUVW : localUVW_A;
				volumeID_A = isNoA ? volumesCount - 1 : volumeID_A;
				float3 L0_A  = 0;
				float3 L1r_A = 0;
				float3 L1g_A = 0;
				float3 L1b_A = 0;
				float4 occlusion_A = 1;
				LV_SampleVolume(volumeID_A, localUVW_A, L0_A, L1r_A, L1g_A, L1b_A, occlusion_A);
				float mask = LV_BoundsMask(localUVW_A, _UdonLightVolumeInvLocalEdgeSmooth[volumeID_A]);
				 if (mask == 1 || isNoA || (_UdonLightVolumeSharpBounds && isNoB)) { // Returning SH A result if it's the center of mask or out of bounds
					L0  += L0_A;
					L1r += L1r_A;
					L1g += L1g_A;
					L1b += L1b_A;
					occlusion = occlusion_A;
					occlusion *= mOcclusion;
					return;
				}
				float3 L0_B  = 0;
				float3 L1r_B = 0;
				float3 L1g_B = 0;
				float3 L1b_B = 0;
				float4 occlusion_B = 1;
				 if (isNoB && lightProbesBlend) { // No Volume found and light volumes blending enabled
					LV_SampleLightProbe(L0_B, L1r_B, L1g_B, L1b_B);
				} else { // Blending Volume A and Volume B
					localUVW_B = isNoB ? localUVW : localUVW_B;
					volumeID_B = isNoB ? volumesCount - 1 : volumeID_B;
					LV_SampleVolume(volumeID_B, localUVW_B, L0_B, L1r_B, L1g_B, L1b_B, occlusion_B);
				}
				occlusion = lerp(occlusion_B, occlusion_A, mask);
				occlusion *= mOcclusion;
				L0  += lerp(L0_B,  L0_A,  mask);
				L1r += lerp(L1r_B, L1r_A, mask);
				L1g += lerp(L1g_B, L1g_A, mask);
				L1b += lerp(L1b_B, L1b_A, mask);
			}
			void LV_LightVolumeAdditiveSH(float3 worldPos, inout float3 L0, inout float3 L1r, inout float3 L1g, inout float3 L1b, out float4 occlusion) {
				occlusion = 1;
				float4 mOcclusion = 1; // Multiplicative occlusion. Applies on top of regular occlusion
				uint additiveCount = min((uint) _UdonLightVolumeAdditiveCount, VRCLV_MAX_VOLUMES_COUNT);
				 if (additiveCount == 0 && (uint) _UdonPointLightVolumeCount == 0) return; // Legacy!
				uint volumesCount = min((uint) _UdonLightVolumeCount, VRCLV_MAX_VOLUMES_COUNT);
				uint maxOverdraw = min((uint) _UdonLightVolumeAdditiveMaxOverdraw, VRCLV_MAX_VOLUMES_COUNT);
				uint volumeID_A = -1; // Main, dominant volume ID
				uint volumeID_B = -1; // Secondary volume ID to blend main with
				float3 localUVW   = 0; // Last local UVW to use in disabled Light Probes mode
				float3 localUVW_A = 0; // Main local UVW for Y Axis and Free rotations
				float3 localUVW_B = 0; // Secondary local UVW
				bool isNoA = true;
				bool isNoB = true;
				uint addVolumesCount = 0;
				uint count = min(_UdonLightVolumeOcclusionCount == 0 ? additiveCount : volumesCount, VRCLV_MAX_VOLUMES_COUNT); // Only use all volumes if occlusion volumes are enabled
				[loop] for (uint id = 0; id < count; id++) {
					localUVW = LV_LocalFromVolume(id, worldPos);
					 if (LV_PointLocalAABB(localUVW)) { // Intersection test
						 if (id < additiveCount) { // Sampling additive volumes
							 if (addVolumesCount < maxOverdraw) {
								float4 occ; // Multiplicative occlusion
								LV_SampleVolume(id, localUVW, L0, L1r, L1g, L1b, occ);
								mOcclusion *= occ;
								addVolumesCount++;
							}
						} else if (isNoA) { // First, searching for volume A
							volumeID_A = id;
							localUVW_A = localUVW;
							isNoA = false;
						} else { // Next, searching for volume B if A found
							volumeID_B = id;
							localUVW_B = localUVW;
							isNoB = false;
							break;
						}
					}
				}
				 if (isNoA || _UdonLightVolumeOcclusionCount == 0) {
					occlusion *= mOcclusion;
					return;
				}
				localUVW_A = isNoA ? localUVW : localUVW_A;
				volumeID_A = isNoA ? volumesCount - 1 : volumeID_A;
				occlusion = LV_SampleVolumeOcclusion(volumeID_A, localUVW_A);
				float mask = LV_BoundsMask(localUVW_A, _UdonLightVolumeInvLocalEdgeSmooth[volumeID_A]);
				 if (mask == 1 || (_UdonLightVolumeSharpBounds && isNoB)) {
					occlusion *= mOcclusion;
					return; // Returning A result if it's the center of mask or out of bounds
				}
				 if (isNoB) occlusion = lerp(1, occlusion, mask);
				else occlusion = lerp(LV_SampleVolumeOcclusion(volumeID_B, localUVW_B), occlusion, mask);
				occlusion *= mOcclusion;
			}
			float3 LightVolumeSpecular(float3 f0, float smoothness, float3 worldNormal, float3 viewDir, float3 L0, float3 L1r, float3 L1g, float3 L1b) {
				float3 specColor = max(float3(dot(reflect(-L1r, worldNormal), viewDir), dot(reflect(-L1g, worldNormal), viewDir), dot(reflect(-L1b, worldNormal), viewDir)), 0);
				float3 rDir = normalize(normalize(L1r) + viewDir);
				float3 gDir = normalize(normalize(L1g) + viewDir);
				float3 bDir = normalize(normalize(L1b) + viewDir);
				float rNh = saturate(dot(worldNormal, rDir));
				float gNh = saturate(dot(worldNormal, gDir));
				float bNh = saturate(dot(worldNormal, bDir));
				float roughness = 1 - smoothness * 0.9f;
				float roughExp = roughness * roughness;
				float rSpec = LV_DistributionGGX(rNh, roughExp);
				float gSpec = LV_DistributionGGX(gNh, roughExp);
				float bSpec = LV_DistributionGGX(bNh, roughExp);
				float3 specs = (rSpec + gSpec + bSpec) * f0;
				float3 coloredSpecs = specs * specColor;
				float3 a = coloredSpecs + specs * L0;
				float3 b = coloredSpecs * 3;
				return max(lerp(a, b, smoothness) * 0.5f, 0.0);
			}
			float3 LightVolumeSpecular(float3 albedo, float smoothness, float metallic, float3 worldNormal, float3 viewDir, float3 L0, float3 L1r, float3 L1g, float3 L1b) {
				float3 specularf0 = lerp(0.04f, albedo, metallic);
				return LightVolumeSpecular(specularf0, smoothness, worldNormal, viewDir, L0, L1r, L1g, L1b);
			}
			float3 LightVolumeSpecularDominant(float3 f0, float smoothness, float3 worldNormal, float3 viewDir, float3 L0, float3 L1r, float3 L1g, float3 L1b) {
				float3 dominantDir = L1r + L1g + L1b;
				float3 dir = normalize(normalize(dominantDir) + viewDir);
				float nh = saturate(dot(worldNormal, dir));
				float roughness = 1 - smoothness * 0.9f;
				float roughExp = roughness * roughness;
				float spec = LV_DistributionGGX(nh, roughExp);
				return max(spec * L0 * f0, 0.0) * 1.5f;
			}
			float3 LightVolumeSpecularDominant(float3 albedo, float smoothness, float metallic, float3 worldNormal, float3 viewDir, float3 L0, float3 L1r, float3 L1g, float3 L1b) {
				float3 specularf0 = lerp(0.04f, albedo, metallic);
				return LightVolumeSpecularDominant(specularf0, smoothness, worldNormal, viewDir, L0, L1r, L1g, L1b);
			}
			float3 LightVolumeEvaluate(float3 worldNormal, float3 L0, float3 L1r, float3 L1g, float3 L1b) {
				return float3(LV_EvaluateSH(L0.r, L1r, worldNormal), LV_EvaluateSH(L0.g, L1g, worldNormal), LV_EvaluateSH(L0.b, L1b, worldNormal));
			}
			void LightVolumeSH(float3 worldPos, out float3 L0, out float3 L1r, out float3 L1g, out float3 L1b, float3 worldPosOffset = 0) {
				L0 = 0; L1r = 0; L1g = 0; L1b = 0;
				if (_UdonLightVolumeEnabled == 0) {
					LV_SampleLightProbeDering(L0, L1r, L1g, L1b);
				} else {
					float4 occlusion = 1;
					LV_LightVolumeSH(worldPos + worldPosOffset, L0, L1r, L1g, L1b, occlusion);
					LV_PointLightVolumeSH(worldPos, occlusion, L0, L1r, L1g, L1b);
				}
			}
			void LightVolumeAdditiveSH(float3 worldPos, out float3 L0, out float3 L1r, out float3 L1g, out float3 L1b, float3 worldPosOffset = 0) {
				L0 = 0; L1r = 0; L1g = 0; L1b = 0;
				if (_UdonLightVolumeEnabled != 0) {
					float4 occlusion = 1;
					LV_LightVolumeAdditiveSH(worldPos + worldPosOffset, L0, L1r, L1g, L1b, occlusion);
					LV_PointLightVolumeSH(worldPos, occlusion, L0, L1r, L1g, L1b);
				}
			}
			float3 LightVolumeSH_L0(float3 worldPos, float3 worldPosOffset = 0) {
				if (_UdonLightVolumeEnabled == 0) {
					return float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				} else {
					float3 L0 = 0; float4 occlusion = 1;
					float3 unused_L1; // Let's just pray that compiler will strip everything x.x
					LV_LightVolumeSH(worldPos + worldPosOffset, L0, unused_L1, unused_L1, unused_L1, occlusion);
					LV_PointLightVolumeSH(worldPos, occlusion, L0, unused_L1, unused_L1, unused_L1);
					return L0;
				}
			}
			float3 LightVolumeAdditiveSH_L0(float3 worldPos, float3 worldPosOffset = 0) {
				if (_UdonLightVolumeEnabled == 0) {
					return 0;
				} else {
					float3 L0 = 0; float4 occlusion = 1;
					float3 unused_L1; // Let's just pray that compiler will strip everything x.x
					LV_LightVolumeAdditiveSH(worldPos + worldPosOffset, L0, unused_L1, unused_L1, unused_L1, occlusion);
					LV_PointLightVolumeSH(worldPos, occlusion, L0, unused_L1, unused_L1, unused_L1);
					return L0;
				}
			}
			float LightVolumesEnabled() {
				return _UdonLightVolumeEnabled;
			}
			float LightVolumesVersion() {
				return _UdonLightVolumeVersion == 0 ? _UdonLightVolumeEnabled : _UdonLightVolumeVersion;
			}
			#endif
			uint _UdonForceSceneLighting;
			#if defined(PROP_LIGHTINGAOMAPS)
			Texture2D _LightingAOMaps;
			#endif
			#if defined(PROP_LIGHTINGDETAILSHADOWMAPS)
			Texture2D _LightingDetailShadowMaps;
			#endif
			#if defined(PROP_LIGHTINGSHADOWMASKS)
			Texture2D _LightingShadowMasks;
			#endif
			#if defined(PROP_LIGHTDATASDFMAP)
			Texture2D _LightDataSDFMap;
			#endif
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define STEREO_UV(uv) float3(uv, unity_StereoEyeIndex)
			#else
			#define STEREO_UV(uv) uv
			#endif
			#if POI_PIPE == POI_BIRP
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			Texture2DArray<float> _CameraDepthTexture;
			#else
			Texture2D<float> _CameraDepthTexture;
			#endif
			#endif
			#if POI_PIPE == POI_URP
			TEXTURE2D_X_FLOAT(_CameraDepthTexture);
			float4 _CameraDepthTexture_TexelSize;
			#endif
			float SampleScreenDepth(float2 uv)
			{
				uv.y = _ProjectionParams.x * 0.5 + 0.5 - uv.y * _ProjectionParams.x;
				return _CameraDepthTexture.SampleLevel(sampler_point_clamp, STEREO_UV(uv), 0).r;
			}
			bool DepthTextureExists()
			{
				float3 dTexDim = 0;
				#if POI_PIPE == POI_BIRP
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y, dTexDim.z);
				#else
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y);
				#endif
				#endif
				#if POI_PIPE == POI_URP
				dTexDim.xy = _CameraDepthTexture_TexelSize.zw;
				#endif
				return dTexDim.x > 16;
			}
			UNITY_DECLARE_TEX2D(_MainTex);
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			#endif
			#ifdef FINALPASS
			#if defined(PROP_DETAILMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailMask;
			#endif
			#if defined(PROP_DETAILNORMALMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailNormalMap;
			#endif
			#if defined(PROP_DETAILTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailTex;
			#endif
			#endif
			struct MatcapAudioLinkData
			{
				float matcapALEnabled;
				float matcapALAlphaAddBand;
				float4 matcapALAlphaAdd;
				float matcapALEmissionAddBand;
				float4 matcapALEmissionAdd;
				float matcapALIntensityAddBand;
				float4 matcapALIntensityAdd;
				float matcapALChronoPanType;
				float matcapALChronoPanBand;
				float matcapALChronoPanSpeed;
			};
			#ifdef POI_EMISSION_1
			#if defined(PROP_EMISSIONMAP1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMap1;
			#endif
			#if defined(PROP_EMISSIONMASK1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMask1;
			#endif
			#if defined(PROP_EMISSIONSCROLLINGCURVE1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionScrollingCurve1;
			#endif
			#endif
			#ifdef _POI_DEPTH_RIMLIGHT
			static float2 depthRimSampleOffsets4[4] = {
				float2(0, 1), float2(0, -1), float2(-1, 0), float2(1, 0)
			};
			static float2 depthRimSampleOffsets8[8] = {
				float2(-1, 1), float2(0, 1), float2(1, 1),
				float2(-1, 0),               float2(1, 0),
				float2(-1, -1), float2(0, -1), float2(1, -1)
			};
			#endif
			#ifdef _SUNDISK_SIMPLE
			#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterMask;
			#endif
			#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterColorMap;
			#endif
			#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterTexture;
			#endif
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			#if defined(PROP_HIGHCOLOR_TEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _HighColor_Tex;
			#endif
			#if defined(PROP_SET_HIGHCOLORMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _Set_HighColorMask;
			#endif
			#endif
			#endif
			float _GrabMode;
			float _Mode;
			#ifndef _STOCHASTICMODE_NONE
			#ifdef _STOCHASTICMODE_DELIOT_HEITZ
			float _StochasticDeliotHeitzDensity;
			#endif
			#endif
			float4 _LightingAOMaps_ST;
			float2 _LightingAOMapsPan;
			float _LightingAOMapsUV;
			float _LightDataAOStrengthR;
			float _LightDataAOStrengthG;
			float _LightDataAOStrengthB;
			float _LightDataAOStrengthA;
			float _LightDataAOGlobalMaskR;
			float _LightDataAOGlobalMaskBlendTypeR;
			float4 _LightingDetailShadowMaps_ST;
			float2 _LightingDetailShadowMapsPan;
			float _LightingDetailShadowMapsUV;
			float _LightingDetailShadowStrengthR;
			float _LightingDetailShadowStrengthG;
			float _LightingDetailShadowStrengthB;
			float _LightingDetailShadowStrengthA;
			float _LightingAddDetailShadowStrengthR;
			float _LightingAddDetailShadowStrengthG;
			float _LightingAddDetailShadowStrengthB;
			float _LightingAddDetailShadowStrengthA;
			float _LightDataDetailShadowGlobalMaskR;
			float _LightDataDetailShadowGlobalMaskBlendTypeR;
			float4 _LightingShadowMasks_ST;
			float2 _LightingShadowMasksPan;
			float _LightingShadowMasksUV;
			float _LightingShadowMaskStrengthR;
			float _LightingShadowMaskStrengthG;
			float _LightingShadowMaskStrengthB;
			float _LightingShadowMaskStrengthA;
			float _LightDataShadowMaskGlobalMaskR;
			float _LightDataShadowMaskGlobalMaskBlendTypeR;
			float _Unlit_Intensity;
			float _LightingColorMode;
			float _LightingMapMode;
			#if defined(PROP_LIGHTDATASDFMAP)
			float4 _LightDataSDFMap_ST;
			float2 _LightDataSDFMapPan;
			float _LightDataSDFMapUV;
			float _LightDataSDFMapLOD;
			float _LightDataSDFBlendY;
			#endif
			float _LightingDirectionMode;
			float3 _LightngForcedDirection;
			float _LightingViewDirOffsetPitch;
			float _LightingViewDirOffsetYaw;
			float _LightingIndirectUsesNormals;
			float _LightingCapEnabled;
			float _LightingCap;
			float _LightingForceColorEnabled;
			float3 _LightingForcedColor;
			float _LightingForcedColorThemeIndex;
			float _LightingCastedShadows;
			float _LightingMonochromatic;
			float _LightingMinLightBrightness;
			float _LightingAdditiveEnable;
			float _LightingAdditiveLimited;
			float _LightingAdditiveLimit;
			float _LightingAdditiveCastedShadows;
			float _LightingAdditiveMonochromatic;
			float _LightingAdditivePassthrough;
			float _DisableDirectionalInAdd;
			float _LightingVertexLightingEnabled;
			float _LightingMirrorVertexLightingEnabled;
			float _LightingEnableLightVolumes;
			float _LightingLightVolumesNormalBias;
			float _LightDataDebugEnabled;
			float _LightingDebugVisualize;
			float4 _Color;
			float _ColorThemeIndex;
			float _MainPixelMode;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float2 _MainTexPan;
			float _MainTexUV;
			float _MainTexStochastic;
			float _MainIgnoreTexAlpha;
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _MainAlphaMaskMode;
			float _AlphaMaskR;
			float _AlphaMaskG;
			float _AlphaMaskB;
			float _AlphaMaskA;
			float _AlphaMaskChannelBlendMode;
			float2 _AlphaMaskMinMax;
			float _AlphaMaskGamma;
			float _AlphaMaskBlendStrength;
			float _AlphaMaskValue;
			float _Cutoff;
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			float4 _GlobalThemeColor0;
			float4 _GlobalThemeColor1;
			float4 _GlobalThemeColor2;
			float4 _GlobalThemeColor3;
			float _GlobalThemeHueShiftColorSpace0;
			float _GlobalThemeHueShiftColorSpace1;
			float _GlobalThemeHueShiftColorSpace2;
			float _GlobalThemeHueShiftColorSpace3;
			float _GlobalThemeHue0;
			float _GlobalThemeHue1;
			float _GlobalThemeHue2;
			float _GlobalThemeHue3;
			float _GlobalThemeHueSpeed0;
			float _GlobalThemeHueSpeed1;
			float _GlobalThemeHueSpeed2;
			float _GlobalThemeHueSpeed3;
			float _GlobalThemeSaturationMethod0;
			float _GlobalThemeSaturationMethod1;
			float _GlobalThemeSaturationMethod2;
			float _GlobalThemeSaturationMethod3;
			float _GlobalThemeSaturation0;
			float _GlobalThemeSaturation1;
			float _GlobalThemeSaturation2;
			float _GlobalThemeSaturation3;
			float _GlobalThemeValue0;
			float _GlobalThemeValue1;
			float _GlobalThemeValue2;
			float _GlobalThemeValue3;
			float _GlobalThemeBrightness0;
			float _GlobalThemeBrightness1;
			float _GlobalThemeBrightness2;
			float _GlobalThemeBrightness3;
			int _GlobalMaskVertexColorLinearSpace;
			float2 _UVSettingsTiling0;
			float2 _UVSettingsOffset0;
			float _UVSettingsAngle0;
			float2 _UVSettingsPan0;
			float _UVSettingsRotate0;
			float2 _UVSettingsTiling1;
			float2 _UVSettingsOffset1;
			float _UVSettingsAngle1;
			float2 _UVSettingsPan1;
			float _UVSettingsRotate1;
			float2 _UVSettingsTiling2;
			float2 _UVSettingsOffset2;
			float _UVSettingsAngle2;
			float2 _UVSettingsPan2;
			float _UVSettingsRotate2;
			float2 _UVSettingsTiling3;
			float2 _UVSettingsOffset3;
			float _UVSettingsAngle3;
			float2 _UVSettingsPan3;
			float _UVSettingsRotate3;
			float _UVSettingsShiftBackfaceUV;
			float _StereoEnabled;
			float _PolarUV;
			float _PolarMode;
			float2 _PolarCenter;
			float _PolarRadialScale;
			float _PolarLengthScale;
			float _PolarSpiralPower;
			float _PanoUseBothEyes;
			float _UVModWorldPos0;
			float _UVModWorldPos1;
			float _UVModLocalPos0;
			float _UVModLocalPos1;
			#ifdef FINALPASS
			float4 _DetailMask_ST;
			float2 _DetailMaskPan;
			float _DetailMaskUV;
			float _DetailMaskStochastic;
			float4 _DetailNormalMap_ST;
			float2 _DetailNormalMapPan;
			float _DetailNormalMapUV;
			float _DetailNormalMapScale;
			float _DetailNormalMapStochastic;
			float _DetailNormalGlobalMask;
			float _DetailNormalGlobalMaskBlendType;
			float4 _DetailTex_ST;
			float2 _DetailTexPan;
			float _DetailTexUV;
			float _DetailTexStochastic;
			float3 _DetailTint;
			float _DetailTintThemeIndex;
			float _DetailTexIntensity;
			float _DetailBrightness;
			float _DetailTexGlobalMask;
			float _DetailTexGlobalMaskBlendType;
			#endif
			#ifdef POI_EMISSION_1
			float4 _EmissionMap1_ST;
			float2 _EmissionMap1Pan;
			float _EmissionMap1UV;
			float4 _EmissionMask1_ST;
			float2 _EmissionMask1Pan;
			float _EmissionMask1UV;
			float _EmissionMaskInvert1;
			float _EmissionMask1Channel;
			float _EmissionMask1GlobalMask;
			float _EmissionMask1GlobalMaskBlendType;
			float4 _EmissionScrollingCurve1_ST;
			float _EmissionFluorescence1;
			float4 _EmissionColor1;
			float _EmissionBaseColorAsMap1;
			float _EmissionStrength1;
			float _EmissionHueShiftEnabled1;
			float _EmissionHueShiftColorSpace1;
			float _EmissionHueSelectOrShift1;
			float _EmissionSaturation1;
			float _EmissionHueShift1;
			float _EmissionHueShiftSpeed1;
			float _EmissionCenterOutEnabled1;
			float _EmissionCenterOutSpeed1;
			float _EmissionCenterOutLinear1;
			float _EnableGITDEmission1;
			float _GITDEWorldOrMesh1;
			float _GITDEMinEmissionMultiplier1;
			float _GITDEMaxEmissionMultiplier1;
			float _GITDEMinLight1;
			float _GITDEMaxLight1;
			float _EmissionBlinkingEnabled1;
			float _EmissiveBlink_Min1;
			float _EmissiveBlink_Max1;
			float _EmissiveBlink_Velocity1;
			float _EmissionBlinkingOffset1;
			float _ScrollingEmission1;
			float4 _EmissiveScroll_Direction1;
			float _EmissiveScroll_Width1;
			float _EmissiveScroll_Velocity1;
			float _EmissiveScroll_Interval1;
			float _EmissionScrollingOffset1;
			float _EmissionReplace1;
			float _EmissionScrollingVertexColor1;
			float _EmissionScrollingUseCurve1;
			float _EmissionColor1ThemeIndex;
			float _EmissionAL1Enabled;
			float2 _EmissionAL1StrengthMod;
			float _EmissionAL1StrengthBand;
			float2 _AudioLinkEmission1CenterOut;
			float _AudioLinkEmission1CenterOutSize;
			float _AudioLinkEmission1CenterOutBand;
			float _AudioLinkEmission1CenterOutDuration;
			float2 _EmissionAL1Multipliers;
			float _EmissionAL1MultipliersBand;
			float _EmissionAL1ColorOverrideEnabled;
			float4 _EmissionAL1ColorOverride;
			float _EmissionAL1ColorOverrideThemeIndex;
			float _EmissionAL1ColorOverrideBand;
			float4 _EmissionAL1ColorOverrideRamp;
			#endif
			#ifdef _POI_DEPTH_RIMLIGHT
			float _DepthRimNormalToUse;
			float _DepthRimWidth;
			float _DepthRimSharpness;
			float _DepthRimDepthThreshold;
			float _DepthRimBinary;
			float _DepthRimHideInShadow;
			float _DepthRimLightDirMethod;
			float _DepthRimShadowMask;
			float _DepthRimMixRampedLightMap;
			float _DepthRimCameraClip;
			float _DepthRimMinDistance;
			float _DepthRimMaxDistance;
			float4 _DepthRimColor;
			float _DepthRimColorThemeIndex;
			float _DepthRimMixBaseColor;
			float _DepthRimEmission;
			float _DepthRimReplace;
			float _DepthRimAdd;
			float _DepthRimScreen;
			float _DepthRimMultiply;
			float _DepthRimAdditiveLighting;
			float _DepthRimMixLightColor;
			float _DepthRimType;
			float _DepthRimBrightness;
			Texture2D _DepthRimMask;
			float4 _DepthRimMask_ST;
			float2 _DepthRimMaskPan;
			float _DepthRimMaskUV;
			float _DepthRimMaskChannel;
			#endif
			#ifdef _SUNDISK_SIMPLE
			float4 _GlitterRandomRotationSpeed;
			float _GlitterLayers;
			float _GlitterUseNormals;
			float _GlitterUV;
			float4 _GlitterColor;
			float _GlitterColorThemeIndex;
			half _GlitterSpeed;
			half _GlitterBrightness;
			float _GlitterFrequency;
			float _GlitterRandomLocation;
			half _GlitterSize;
			half _GlitterContrast;
			half _GlitterAngleRange;
			half _GlitterMinBrightness;
			half _GlitterBias;
			float _GlitterUseSurfaceColor;
			float _GlitterBlendType;
			float _GlitterMode;
			float _GlitterShape;
			float _GlitterLinearMinBrightness;
			float _GlitterLinearBrightness;
			float _GlitterLinearSpeed;
			float _GlitterLinearSensitivity;
			float _GlitterLinearContrast;
			float _GlitterLinearPostContrast;
			float _GlitterLinearAngle;
			float _GlitterLinearLightDirection;
			float _GlitterLinearVRParallax;
			float _GlitterLinearDistanceLOD;
			float _GlitterJaggyFix;
			float _GlitterDistanceLOD;
			float _GlitterTextureRotation;
			float2 _GlitterUVPanning;
			float _GlitterHueShiftEnabled;
			float _GlitterHueShiftColorSpace;
			float _GlitterHueShiftSpeed;
			float _GlitterHueShift;
			float _GlitterHueSelectOrShift;
			float _GlitterHueShiftRandomOffset;
			float _GlitterHideInShadow;
			float _GlitterScaleWithLighting;
			float _GlitterRandomColors;
			float2 _GlitterMinMaxSaturation;
			float2 _GlitterMinMaxBrightness;
			float _GlitterRandomSize;
			float4 _GlitterMinMaxSize;
			float _GlitterFullCoverage;
			float _GlitterRandomRotation;
			float4 _GlitterMask_ST;
			float2 _GlitterMaskPan;
			float _GlitterMaskUV;
			float _GlitterMaskChannel;
			float _GlitterMaskInvert;
			float _GlitterMaskGlobalMask;
			float _GlitterMaskGlobalMaskBlendType;
			float4 _GlitterColorMap_ST;
			float2 _GlitterColorMapPan;
			float _GlitterColorMapUV;
			float4 _GlitterTexture_ST;
			float4 _GlitterTexture_TexelSize;
			float _GlitterTexturePixelMode;
			float _GlitterALEnabled;
			float _GlitterALAlphaAddBand;
			float4 _GlitterALAlphaAdd;
			float _GlitterALMaxBrightnessBand;
			float4 _GlitterALMaxBrightnessAdd;
			float _GlitterALSizeAddBand;
			float4 _GlitterALSizeAdd;
			float _GlitterALChronoSparkleSpeedType;
			float _GlitterALChronoSparkleSpeedBand;
			float _GlitterALChronoSparkleSpeed;
			float _GlitterALChronoRotationSpeedType;
			float _GlitterALChronoRotationSpeedBand;
			float _GlitterALChronoRotationSpeed;
			float _GlitterALColorOverrideEnabled;
			float4 _GlitterALColorOverride;
			float _GlitterALColorOverrideThemeIndex;
			float _GlitterALColorOverrideBand;
			float4 _GlitterALColorOverrideRamp;
			float _GlitterSpriteSheetEnabled;
			float _GlitterSpriteSheetColumns;
			float _GlitterSpriteSheetRows;
			float _GlitterSpriteSheetFrameSelection;
			float _GlitterSpriteSheetFPS;
			float _GlitterSpriteSheetRandomStart;
			float _GlitterSpriteSheetTotalFrames;
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			float _StylizedReflectionMode;
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			float4 _HighColor_Tex_ST;
			float2 _HighColor_TexPan;
			float _HighColor_TexUV;
			float4 _Set_HighColorMask_ST;
			float2 _Set_HighColorMaskPan;
			float _Set_HighColorMaskUV;
			float _Set_HighColorMaskChannel;
			float _Tweak_HighColorMaskLevel;
			float _StylizedSpecularInvertMask;
			float _StylizedSpecularNormalStrength;
			float4 _HighColor;
			float _UseLightColor;
			float _HighColor_Power;
			float _StylizedSpecularFeather;
			float _Layer1Strength;
			float _Layer2Size;
			float _StylizedSpecular2Feather;
			float _Layer2Strength;
			float _StylizedSpecularStrength;
			float _HighColorThemeIndex;
			float _Is_BlendAddToHiColor;
			float _Is_SpecularToHighColor;
			float _StylizedSpecularIgnoreNormal;
			float _StylizedSpecularIgnoreShadow;
			float _SSIgnoreCastedShadows;
			float _UseSpecularOptMap2;
			#endif
			#endif
			float _PPLightingMultiplier;
			float _PPLightingAddition;
			float _PPEmissionMultiplier;
			float _PPFinalColorMultiplier;
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				#ifdef POI_PASS_MOTION_VECTORS
				float3 positionOld : TEXCOORD4;
				#if _ADD_PRECOMPUTED_VELOCITY
				float3 alembicMotionVector : TEXCOORD5;
				#endif
				#endif
				#ifdef POI_TESSELLATED
				uint vertexId : TEXCOORD6;
				#else
				uint vertexId : SV_VertexID;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float worldDir : TEXCOORD8;
				float fogData: TEXCOORD10;
				POI_SHADOW_COORDS(9)
				#ifdef POI_SQUISH
				float squishAmount : TEXCOORD11;
				#endif
				#ifdef POIBS_BLOOMFOG
				float2 bsFogScreenUV : TEXCOORD12;
				#endif
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD13;
				float4 LightCoord : TEXCOORD14;
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				float4 positionCSNoJitter         : POSITION_CS_NO_JITTER;
				float4 previousPositionCSNoJitter : PREV_POSITION_CS_NO_JITTER;
				#endif
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(POI_DEVICE_QUEST)
				uint gl_Layer : SV_RenderTargetArrayIndex;
				#endif
			};
			float2 vertexUV(in VertexOut o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in appdata v, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return v.uv0.xy * w0 + v.uv1.xy * w1 + v.uv2.xy * w2 + v.uv3.xy * w3;
			}
			#ifdef POI_PASS_LILFUR
			float2 vertexUV(in PoiGeoToFragLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in PoiVertToGeoLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			#endif
			#ifdef POI_AUDIOLINK
			inline int poiALBandPass(int bandIdx)
			{
				bandIdx = clamp(bandIdx, 0, 3);
				return bandIdx == 0 ? ALPASS_AUDIOBASS : bandIdx == 1 ? ALPASS_AUDIOLOWMIDS : bandIdx == 2 ? ALPASS_AUDIOHIGHMIDS : ALPASS_AUDIOTREBLE;
			}
			#endif
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				float3 outputColor = srcColor;
				if (themeIndex != 0)
				{
					themeIndex = max(themeIndex - 1, 0);
					if (themeIndex <= 3)
					{
						outputColor = poiMods.globalColorTheme[themeIndex].rgb;
					}
					else
					{
						#ifdef POI_AUDIOLINK
						if (poiMods.audioLinkAvailable)
						{
							outputColor = poiMods.globalColorTheme[themeIndex].rgb;
						}
						#endif
					}
				}
				return outputColor;
			}
			struct PoiSpriteSheetConfig
			{
				float columns;
				float rows;
				float totalFrames;
				float frameSelection;
				float fps;
				float randomStart;
				float inset;
				float2 textureST;
			};
			float poiSpriteSheetFrameIndex(PoiSpriteSheetConfig config, float progress, float seed)
			{
				float totalFrames = (config.totalFrames > 0) ? config.totalFrames : (config.columns * config.rows);
				float frameIndex = 0;
				if (config.frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (config.frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (config.frameSelection == 2)
				{
					float timeOffset = config.randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * config.fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float poiSpriteSheetFrameIndex(float columns, float rows, float configTotalFrames, float frameSelection, float fps, float randomStart, float progress, float seed)
			{
				float totalFrames = (configTotalFrames > 0) ? configTotalFrames : (columns * rows);
				float frameIndex = 0;
				if (frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (frameSelection == 2)
				{
					float timeOffset = randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float2 poiSpriteSheetUV(float2 uv, float columns, float rows, float frameIndex, float inset, float2 textureST)
			{
				float invCols = 1.0 / columns;
				float invRows = 1.0 / rows;
				float2 frameSize = float2(invCols, invRows);
				float column = fmod(frameIndex, columns);
				float row = floor(frameIndex * invCols);
				row = rows - 1 - row;
				float2 lookupUV = (uv * frameSize) + float2(column, row) * frameSize;
				if (inset > 0)
				{
					float2 texelInset = inset * frameSize / max(textureST, 1e-6);
					lookupUV = lerp(lookupUV, float2(column + 1, row + 1) * frameSize, texelInset);
				}
				return lookupUV;
			}
			float2 poiSpriteSheetUV(float2 uv, PoiSpriteSheetConfig config, float frameIndex)
			{
				return poiSpriteSheetUV(uv, config.columns, config.rows, frameIndex, config.inset, config.textureST);
			}
			void applyReducedRenderClipDistance(inout VertexOut o)
			{
				if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
				{
					#if defined(UNITY_REVERSED_Z) // DirectX
					o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
					#else // OpenGL
					o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
					#endif
				}
			}
			VertexOut vert(appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				POI_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				#ifdef POI_PASS_OUTLINE
				if (0.0 > 0.5)
				{
					return (VertexOut)POI_NAN;
				}
				#endif
				#ifdef POI_AUDIOLINK
				float vertexAudioLink[5];
				vertexAudioLink[0] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 0))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 0))[0];
				vertexAudioLink[1] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 1))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 1))[0];
				vertexAudioLink[2] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 2))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 2))[0];
				vertexAudioLink[3] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 3))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 3))[0];
				vertexAudioLink[4] = AudioLinkData(ALPASS_GENERALVU + float2(8, 0))[0];
				#endif
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				#if POI_PIPE == POI_BIRP
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				#elif  POI_PIPE == POI_URP
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent);
				o.normal = vertexNormalInput.normalWS;
				o.tangent.xyz = vertexNormalInput.tangentWS;
				#endif
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				#if defined(LIGHTMAP_ON)
				OUTPUT_LIGHTMAP_UV(v.uv1.xy, unity_LightmapST, o.lightmapUV.xy);
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				OUTPUT_LIGHTMAP_UV(v.uv2.xy, unity_LightmapST, o.lightmapUV.zw);
				#endif
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				o.localPos.xyz += localOffset;
				o.worldPos.xyz += worldOffset;
				o.pos = PoiObjectToClipPos(o.localPos.xyz);
				o.fogData = o.pos.z;
				#if !defined(POI_PASS_SHADOW)
				POI_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				POI_TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				o.worldDir = dot(o.pos, CalculateFrustumCorrection());
				if (0.0)
				{
					applyReducedRenderClipDistance(o);
				}
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#ifdef EDITOR_VISUALIZATION
				UnityEditorVizData(v.vertex.xyz, o.uv[0].xy, o.uv[0].zw, o.uv[1].xy, o.VizUV, o.LightCoord);
				#endif
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				#if defined(APPLICATION_SPACE_WARP_MOTION)
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				o.pos = o.positionCSNoJitter;
				#else
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				#endif
				float4 prevPos = (unity_MotionVectorsParams.x == 1) ? float4(v.positionOld, 1) : o.localPos;
				#if _ADD_PRECOMPUTED_VELOCITY
				prevPos = prevPos - float4(v.alembicMotionVector, 0);
				#endif
				o.previousPositionCSNoJitter = mul(_PrevViewProjMatrix, mul(UNITY_PREV_MATRIX_M, prevPos));
				#endif
				#endif
				#ifdef POI_PASS_LILFUR
				#endif
				return o;
			}
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D(float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1, 311.7)), dot(s, float2(269.5, 183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * 1.0);
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				float3x3 pos = float3x3(
				float3(vxID, bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				return (bary.z > 0) ? pos : neg;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				return mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			float4 PoiLightDataSetupAdd(
			inout PoiLight poiLight,
			inout PoiFragData poiFragData,
			in PoiMesh poiMesh,
			in PoiCam poiCam,
			in PoiMods poiMods,
			#ifdef POI_PASS_LILFUR
			in PoiGeoToFragLilFur i,
			#else
			in VertexOut i,
			#endif
			in float4 mainTexture)
			{
				if (!1.0)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#if defined(DIRECTIONAL)
				if (1.0)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#endif
				#if POI_PIPE == POI_BIRP
				poiLight.direction = normalize(_WorldSpaceLightPos0.xyz - poiMesh.worldPos.xyz * _WorldSpaceLightPos0.w);
				#elif POI_PIPE == POI_URP
				poiLight.direction = poiLight.unityLight.direction.xyz;
				#endif
				#if defined(POINT) || defined(SPOT)
				#ifdef POINT
				unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1)).xyz;
				poiLight.attenuation = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r;
				#endif
				#ifdef SPOT
				unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1));
				poiLight.attenuation = (lightCoord.z > 0) * UnitySpotCookie(lightCoord) * UnitySpotAttenuate(lightCoord.xyz);
				#endif
				#else
				#if POI_PIPE == POI_BIRP
				#ifndef POI_PASS_LILFUR
				UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
				poiLight.attenuation = attenuation;
				#else
				poiLight.attenuation = 1;
				#endif
				#elif POI_PIPE == POI_URP
				poiLight.attenuation = poiLight.unityLight.distanceAttenuation;
				#endif
				#endif
				#if POI_PIPE == POI_BIRP
				#if !defined(POI_PASS_LILFUR)
				poiLight.additiveShadow = UNITY_SHADOW_ATTENUATION(i, poiMesh.worldPos);
				#else
				poiLight.additiveShadow = 1;
				#endif
				#elif POI_PIPE == POI_URP
				poiLight.additiveShadow = poiLight.unityLight.shadowAttenuation;
				#endif
				poiLight.attenuationStrength = 1.0;
				poiLight.directColor = 1.0 ? MaxLuminance(poiLight.unityLight.color * poiLight.attenuation, 1.0) : poiLight.unityLight.color * poiLight.attenuation;
				#if defined(POINT_COOKIE) || defined(DIRECTIONAL_COOKIE)
				poiLight.indirectColor = 0;
				#else
				poiLight.indirectColor = lerp(0, poiLight.directColor, 0.5);
				poiLight.indirectColor = 1.0 ? MaxLuminance(poiLight.indirectColor, 1.0) : poiLight.indirectColor;
				#endif
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), 1.0);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), 1.0);
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = dot(poiLight.direction, poiLight.halfDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				[flatten]
				switch(2.0)
				{
					case 0:
					case 1:
					case 2:
					{
						poiLight.lightMap = poiLight.nDotLNormalized;
						break;
					}
					default:
					{
						poiLight.lightMap = 1;
						break;
					}
				}
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.lightMapNoAttenuation = poiLight.lightMap;
				poiLight.lightMap *= lerp(1, poiLight.additiveShadow, poiLight.attenuationStrength);
				float4 debugColor = 0;
				return debugColor;
			}
			float4 PoiLightDataSetup(
			inout PoiLight poiLight,
			inout PoiFragData poiFragData,
			in PoiMesh poiMesh,
			in PoiCam poiCam,
			in PoiMods poiMods,
			#ifdef POI_PASS_LILFUR
			in PoiGeoToFragLilFur i,
			#else
			in VertexOut i,
			#endif
			in float4 mainTexture)
			{
				bool lightExists = false;
				if (any(poiLight.unityLight.color.rgb >= 0.002))
				{
					lightExists = true;
				}
				if (1.0)
				{
					poiFragData.toggleVertexLights = 1;
				}
				if (IsInMirror() && 1.0 == 0)
				{
					poiFragData.toggleVertexLights = 0;
				}
				#if POI_PIPE == POI_BIRP
				if (1.0)
				{
					#if defined(VERTEXLIGHT_ON)
					float4 toLightX = unity_4LightPosX0 - poiMesh.worldPos.x;
					float4 toLightY = unity_4LightPosY0 - poiMesh.worldPos.y;
					float4 toLightZ = unity_4LightPosZ0 - poiMesh.worldPos.z;
					float4 lengthSq = 0;
					lengthSq += toLightX * toLightX;
					lengthSq += toLightY * toLightY;
					lengthSq += toLightZ * toLightZ;
					float4 lightAttenSq = unity_4LightAtten0;
					float4 atten = 1.0 / (1.0 + lengthSq * lightAttenSq);
					float4 vLightWeight = saturate(1 - (lengthSq * lightAttenSq / 25));
					poiLight.vAttenuation = min(atten, vLightWeight * vLightWeight);
					poiLight.vDotNL = 0;
					poiLight.vDotNL += toLightX * poiMesh.normals[1].x;
					poiLight.vDotNL += toLightY * poiMesh.normals[1].y;
					poiLight.vDotNL += toLightZ * poiMesh.normals[1].z;
					float4 corr = rsqrt(lengthSq);
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					poiLight.vertexVDotNL = 0;
					poiLight.vertexVDotNL += toLightX * poiMesh.normals[0].x;
					poiLight.vertexVDotNL += toLightY * poiMesh.normals[0].y;
					poiLight.vertexVDotNL += toLightZ * poiMesh.normals[0].z;
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					poiLight.vSaturatedDotNL = saturate(poiLight.vDotNL);
					[unroll]
					for (int index = 0; index < 4; index++)
					{
						poiLight.vPosition[index] = float3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]);
						float3 vertexToLightSource = poiLight.vPosition[index] - poiMesh.worldPos;
						poiLight.vDirection[index] = normalize(vertexToLightSource);
						poiLight.vColor[index] = 1.0 ? MaxLuminance(unity_LightColor[index].rgb * poiLight.vAttenuation[index], 1.0) : unity_LightColor[index].rgb * poiLight.vAttenuation[index];
						poiLight.vColor[index] = lerp(poiLight.vColor[index], dot(poiLight.vColor[index], float3(0.299, 0.587, 0.114)), 1.0);
						poiLight.vHalfDir[index] = Unity_SafeNormalize(poiLight.vDirection[index] + poiCam.viewDir);
						poiLight.vDotNL[index] = dot(poiMesh.normals[1], poiLight.vDirection[index]);
						poiLight.vCorrectedDotNL[index] = .5 * (poiLight.vDotNL[index] + 1);
						poiLight.vDotLH[index] = saturate(dot(poiLight.vDirection[index], poiLight.vHalfDir[index]));
						poiLight.vDotNH[index] = dot(poiMesh.normals[1], poiLight.vHalfDir[index]);
						poiLight.vertexVDotNH[index] = saturate(dot(poiMesh.normals[0], poiLight.vHalfDir[index]));
					}
					#endif
				}
				#endif
				if (2.0 == 0) // Poi Custom Light Color
				{
					float3 magic = max(BetterSH9(normalize(PoiSHAr + PoiSHAg + PoiSHAb)), 0);
					float3 normalLight = poiLight.unityLight.color.rgb + BetterSH9(float4(0, 0, 0, 1));
					float magiLumi = CalculateLuminance(magic);
					float normaLumi = CalculateLuminance(normalLight);
					float maginormalumi = magiLumi + normaLumi;
					float magiratio = magiLumi / maginormalumi;
					float normaRatio = normaLumi / maginormalumi;
					float target = CalculateLuminance(magic * magiratio + normalLight * normaRatio);
					float3 properLightColor = magic + normalLight;
					float properLuminance = CalculateLuminance(magic + normalLight);
					poiLight.directColor = properLightColor * max(0.0001, (target / properLuminance));
					{
						float3 indirectSampleNormal = lerp(0, poiMesh.normals[1], 0.0);
						#ifdef POI_BENTNORMALMAP
						if (poiMesh.hasBentNormal > 0.5 && 1.0 > 0.5)
						{
							indirectSampleNormal = lerp(indirectSampleNormal, poiMesh.bentNormal, 0.0);
						}
						#endif
						poiLight.indirectColor = BetterSH9(float4(indirectSampleNormal, 1));
					}
				}
				if (2.0 == 1) // More standard approach to light color
				{
					float3 indirectNormal = poiMesh.normals[1];
					#ifdef POI_BENTNORMALMAP
					if (poiMesh.hasBentNormal > 0.5 && 1.0 > 0.5)
					{
						indirectNormal = poiMesh.bentNormal;
					}
					#endif
					float3 indirectColor = BetterSH9(float4(indirectNormal, 1));
					if (lightExists)
					{
						poiLight.directColor = poiLight.unityLight.color.rgb;
						poiLight.indirectColor = indirectColor;
					}
					else
					{
						poiLight.directColor = indirectColor * 0.6;
						poiLight.indirectColor = indirectColor * 0.5;
					}
				}
				if (2.0 == 2) // UTS style
				{
					poiLight.indirectColor = saturate(max(half3(0.05, 0.05, 0.05) * 1.0, max(PoiShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), PoiShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb) * 1.0));
					poiLight.directColor = max(poiLight.indirectColor, poiLight.unityLight.color.rgb);
				}
				if (2.0 == 3) // OpenLit
				{
					float3 lightDirectionForSH9 = OpenLitLightingDirectionForSH9(poiLight.unityLight);
					OpenLitShadeSH9ToonDouble(lightDirectionForSH9, poiLight.directColor, poiLight.indirectColor);
					poiLight.directColor += poiLight.unityLight.color.rgb;
				}
				float lightMapMode = 1.0;
				if (3.0 == 0)
				{
					poiLight.direction = CalculateLuminance(poiLight.unityLight.color.rgb) * poiLight.unityLight.direction.xyz + (PoiSHAr.xyz + PoiSHAg.xyz + PoiSHAb.xyz) / 3.0;
				}
				if (3.0 == 1 || 3.0 == 2)
				{
					if (3.0 == 1)
					{
						poiLight.direction = mul((float3x3)unity_ObjectToWorld, float4(0,0,0,1)).xyz;
					}
					if (3.0 == 2)
					{
						poiLight.direction = float4(0,0,0,1);
					}
					if (lightMapMode == 0)
					{
						lightMapMode = 1;
					}
				}
				if (3.0 == 3) // UTS
				{
					float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
					float3 lightDirection = normalize(lerp(defaultLightDirection, poiLight.unityLight.direction.xyz, any(poiLight.unityLight.direction.xyz)));
					poiLight.direction = lightDirection;
				}
				if (3.0 == 4) // OpenLit
				{
					poiLight.direction = OpenLitLightingDirection(poiLight.unityLight); // float4 customDir = 0; // Do we want to give users to alter this (OpenLit always does!)?
				}
				if (3.0 == 5) // View Direction
				{
					float3 upViewDir = normalize(UNITY_MATRIX_V[1].xyz);
					float3 rightViewDir = normalize(UNITY_MATRIX_V[0].xyz);
					float yawOffset_Rads = radians(!IsInMirror() ? - 0.0 : 0.0);
					float3 rotatedViewYaw = normalize(RotateAroundAxis(rightViewDir, upViewDir, yawOffset_Rads));
					float3 rotatedViewCameraMeshOffset = RotateAroundAxis((getCameraPosition() - (poiMesh.worldPos)), upViewDir, yawOffset_Rads);
					float pitchOffset_Rads = radians(!IsInMirror() ? 0.0 : - 0.0);
					float3 rotatedViewPitch = RotateAroundAxis(rotatedViewCameraMeshOffset, rotatedViewYaw, pitchOffset_Rads);
					poiLight.direction = normalize(rotatedViewPitch);
				}
				if (!any(poiLight.direction))
				{
					poiLight.direction = float3(.4, 1, .4);
				}
				poiLight.direction = normalize(poiLight.direction);
				poiLight.attenuationStrength = 1.0;
				poiLight.attenuation = 1;
				if (!all(poiLight.unityLight.color.rgb == 0.0))
				{
					#if POI_PIPE == POI_BIRP
					#ifndef POI_PASS_LILFUR
					UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
					poiLight.attenuation *= attenuation;
					#endif
					#elif POI_PIPE == POI_URP
					poiLight.attenuation *= poiLight.unityLight.shadowAttenuation;
					poiLight.attenuation *= (poiLight.lightType == POI_POINT_SPOT) ? poiLight.unityLight.distanceAttenuation : 1;
					#endif
				}
				#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
				half bakedAtten = UnitySampleBakedOcclusion(poiMesh.lightmapUV.xy, poiMesh.worldPos);
				float zDist = dot(_WorldSpaceCameraPos - poiMesh.worldPos, UNITY_MATRIX_V[2].xyz);
				float fadeDist = UnityComputeShadowFadeDistance(poiMesh.worldPos, zDist);
				poiLight.attenuation = UnityMixRealtimeAndBakedShadows(poiLight.attenuation, bakedAtten, UnityComputeShadowFade(fadeDist));
				#endif
				if (!any(poiLight.directColor) && !any(poiLight.indirectColor) && lightMapMode == 0)
				{
					lightMapMode = 1;
					if (3.0 == 0)
					{
						poiLight.direction = normalize(float3(.4, 1, .4));
					}
				}
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotVCentered = abs(dot(poiMesh.normals[1], normalize(getCameraPosition() - poiMesh.worldPos.xyz)));
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = max(0.00001, dot(poiLight.direction, poiLight.halfDir));
				if (lightMapMode == 0)
				{
					float3 ShadeSH9Plus = GetSHLength();
					float3 ShadeSH9Minus = float3(PoiSHAr.w, PoiSHAg.w, PoiSHAb.w) + float3(PoiSHBr.z, PoiSHBg.z, PoiSHBb.z) / 3.0;
					float3 greyScaleVector = float3(.33333, .33333, .33333);
					float bw_lightColor = dot(poiLight.directColor, greyScaleVector);
					float bw_directLighting = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor * lerp(1, poiLight.attenuation, poiLight.attenuationStrength)) + dot(PoiShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_directLightingNoAtten = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor) + dot(PoiShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_bottomIndirectLighting = dot(ShadeSH9Minus, greyScaleVector);
					float bw_topIndirectLighting = dot(ShadeSH9Plus, greyScaleVector);
					float lightDifference = ((bw_topIndirectLighting + bw_lightColor) - bw_bottomIndirectLighting);
					poiLight.lightMap = smoothstep(0, lightDifference, bw_directLighting - bw_bottomIndirectLighting);
					poiLight.lightMapNoAttenuation = smoothstep(0, lightDifference, bw_directLightingNoAtten - bw_bottomIndirectLighting);
				}
				if (lightMapMode == 1)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 2)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					poiLight.lightMap = poiLight.nDotLSaturated * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 3)
				{
					poiLight.lightMapNoAttenuation = 1;
					poiLight.lightMap = lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 4)
				{
					#if defined(PROP_LIGHTDATASDFMAP)
					float2 lightDataSDFMap = 1;
					if (0.0 > 0)
					{
						float sdfLod = pow(0.0, 4.0);
						lightDataSDFMap = POI2D_SAMPLER_PANGRAD(_LightDataSDFMap, _linear_repeat, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), max(poiMesh.dx, sdfLod), max(poiMesh.dy, sdfLod)).rg;
					}
					else
					{
						lightDataSDFMap = POI2D_SAMPLER_PAN(_LightDataSDFMap, _linear_repeat, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0)).rg;
					}
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					float3 faceR = mul((float3x3)unity_ObjectToWorld, float3(-1.0, 0.0, 0.0));
					float LdotR = dot(poiLight.direction.xz, faceR.xz);
					float sdf = LdotR < 0 ? lightDataSDFMap.g : lightDataSDFMap.r;
					float3 faceF = mul((float3x3)unity_ObjectToWorld, float3(0.0, 0.0, 1.0)).xyz;
					faceF.y *= 1.0;
					faceF = dot(faceF, faceF) == 0 ? 0 : normalize(faceF);
					float3 faceL = poiLight.direction;
					faceL.y *= 1.0;
					faceL = dot(faceL, faceL) == 0 ? 0 : normalize(faceL);
					float lnSDF = dot(faceL, faceF);
					poiLight.lightMapNoAttenuation = saturate(lnSDF * 0.5 + sdf * 0.5 + 0.25);
					poiLight.lightMap = saturate(lnSDF * 0.5 + sdf * 0.5 + 0.25) * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
					#else
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
					#endif
				}
				poiLight.lightMapNoAttenuation *= poiLight.detailShadow;
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.directColor = max(poiLight.directColor, 0.0001);
				poiLight.indirectColor = max(poiLight.indirectColor, 0.0001);
				if (2.0 == 3)
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.lightingMinLightBrightness);
				}
				else
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.lightingMinLightBrightness);
					poiLight.indirectColor = max(poiLight.indirectColor, poiLight.lightingMinLightBrightness);
				}
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), poiLight.lightingMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), poiLight.lightingMonochromatic);
				if (poiLight.lightingCapEnabled)
				{
					poiLight.directColor = min(poiLight.directColor, _LightingCap);
					poiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);
				}
				if (0.0)
				{
					poiLight.directColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				}
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				poiLight.directColor = max(poiLight.directColor * 1.0, 0);
				poiLight.directColor = max(poiLight.directColor + 0.0, 0);
				poiLight.indirectColor = max(poiLight.indirectColor * 1.0, 0);
				poiLight.indirectColor = max(poiLight.indirectColor + 0.0, 0);
				#endif
				float4 debugColor = 0;
				#ifdef POI_PASS_ADD
				debugColor = PoiLightDataSetupAdd(poiLight, poiFragData, poiMesh, poiCam, poiMods, i, mainTexture);
				#endif
				return debugColor;
			}
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + 0.0);
				if (0.0 > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[0.0 - 1], 2.0);
				}
			}
			void calculateGlobalThemes(inout PoiMods poiMods)
			{
				float4 themeColorExposures = 0;
				float4 themeColor0, themeColor1, themeColor2, themeColor3 = 0;
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor0.rgb, themeColorExposures.x);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor1.rgb, themeColorExposures.y);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor2.rgb, themeColorExposures.z);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor3.rgb, themeColorExposures.w);
				themeColor0.rgb = hueShift(themeColor0.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor0.rgb = ModifyViaHSV(themeColor0.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor0.rgb = lerp(themeColor0.rgb, dot(themeColor0.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor0.rgb = saturate(themeColor0.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor0.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[0] = float4(ApplyHDRExposure(themeColor0.rgb, themeColorExposures.x), float4(1,1,1,1).a);
				themeColor1.rgb = hueShift(themeColor1.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor1.rgb = ModifyViaHSV(themeColor1.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor1.rgb = lerp(themeColor1.rgb, dot(themeColor1.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor1.rgb = saturate(themeColor1.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor1.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[1] = float4(ApplyHDRExposure(themeColor1.rgb, themeColorExposures.y), float4(1,1,1,1).a);
				themeColor2.rgb = hueShift(themeColor2.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor2.rgb = ModifyViaHSV(themeColor2.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor2.rgb = lerp(themeColor2.rgb, dot(themeColor2.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor2.rgb = saturate(themeColor2.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor2.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[2] = float4(ApplyHDRExposure(themeColor2.rgb, themeColorExposures.z), float4(1,1,1,1).a);
				themeColor3.rgb = hueShift(themeColor3.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor3.rgb = ModifyViaHSV(themeColor3.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor3.rgb = lerp(themeColor3.rgb, dot(themeColor3.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor3.rgb = saturate(themeColor3.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor3.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[3] = float4(ApplyHDRExposure(themeColor3.rgb, themeColorExposures.w), float4(1,1,1,1).a);
			}
			void ApplyGlobalMaskModifiers(in PoiMesh poiMesh, inout PoiMods poiMods, in PoiCam poiCam)
			{
			}
			float2 applyUVSettings(float2 uv, float2 tiling, float2 offset, float angle, float2 pan, float rotate)
			{
				uv = uv * tiling + offset;
				float angleRad = angle * (UNITY_PI / 180.0);
				float rotateRad = rotate * (UNITY_PI / 180.0);
				float totalAngle = angleRad + rotateRad * POI_TIME.y;
				if (totalAngle != 0)
				{
					float2 pivot = float2(0.5, 0.5);
					float cosA = cos(totalAngle);
					float sinA = sin(totalAngle);
					uv -= pivot;
					uv = float2(
					uv.x * cosA - uv.y * sinA,
					uv.x * sinA + uv.y * cosA
					);
					uv += pivot;
				}
				uv += pan * POI_TIME.y;
				return uv;
			}
			float2 calculatePolarCoordinate(in PoiMesh poiMesh)
			{
				float2 delta = poiMesh.uv[0.0] - float4(0.5,0.5,0,0);
				float dist = length(delta);
				float radius;
				
				if (0.0 == 0) // Standard
				{
					radius = dist * 2 * 1.0;
				}
				else if (0.0 == 1) // Log Polar - preserves texture scale
				{
					radius = log(dist + 0.001) * 1.0 + 1;
				}
				else // Square Polar
				{
					radius = max(abs(delta.x), abs(delta.y)) * 2 * 1.0;
				}
				float angle = atan2(delta.x, delta.y);
				float phi = angle / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				angle = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				angle *= 1.0;
				return float2(radius, angle + dist * 0.0);
			}
			float2 MonoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(1.0, 1.0 / UNITY_PI);
				return float2(1.0, 1.0) - sphereCoords;
			}
			float2 StereoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(0.5, 1.0 / UNITY_PI);
				sphereCoords = float2(0.5, 1.0) - sphereCoords;
				return (sphereCoords + float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).xy) * float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).zw;
			}
			float2 calculateWorldUV(in PoiMesh poiMesh)
			{
				return float2(0.0 != 3 ? poiMesh.worldPos[ 0.0] : 0.0f, 2.0 != 3 ? poiMesh.worldPos[2.0] : 0.0f);
			}
			float2 calculatelocalUV(in PoiMesh poiMesh)
			{
				float localUVs[8];
				localUVs[0] = poiMesh.localPos.x;
				localUVs[1] = poiMesh.localPos.y;
				localUVs[2] = poiMesh.localPos.z;
				localUVs[3] = 0;
				localUVs[4] = poiMesh.vertexColor.r;
				localUVs[5] = poiMesh.vertexColor.g;
				localUVs[6] = poiMesh.vertexColor.b;
				localUVs[7] = poiMesh.vertexColor.a;
				return float2(localUVs[0.0],localUVs[1.0]);
			}
			float2 calculatePanosphereUV(in PoiMesh poiMesh)
			{
				float3 viewDirection = normalize(lerp(getCameraPosition().xyz, _WorldSpaceCameraPos.xyz, 1.0) - poiMesh.worldPos.xyz) * - 1;
				return lerp(MonoPanoProjection(viewDirection), StereoPanoProjection(viewDirection), 0.0);
			}
			#ifdef FINALPASS
			void ApplyDetailColor(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiMods poiMods)
			{
				#if defined(PROP_DETAILTEX) || !defined(OPTIMIZER_ENABLED)
				half3 detailTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_DetailTex, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0).rgb * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				#else
				half3 detailTexture = 0.21763764082 * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				#endif
				poiFragData.baseColor.rgb *= LerpWhiteTo(detailTexture * 0.29 * unity_ColorSpaceDouble.rgb, poiMods.detailMask.r * 0.14);
			}
			void ApplyDetailNormal(inout PoiMods poiMods, inout PoiMesh poiMesh)
			{
				#if defined(PROP_DETAILMASK) || !defined(OPTIMIZER_ENABLED)
				poiMods.detailMask = POI2D_SAMPLER_PAN_STOCHASTIC(_DetailMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0).rg;
				#else
				poiMods.detailMask = 1;
				#endif
				#ifdef POI_BACKFACE
				if (!poiMesh.isFrontFace)
				{
					poiMods.detailMask.rg *= 1.0;
				}
				#endif
				if (0.0 > 0)
				{
					poiMods.detailMask.r = maskBlend(poiMods.detailMask.r, poiMods.globalMask[0.0 - 1], 2.0);
				}
				if (0.0 > 0)
				{
					poiMods.detailMask.g = maskBlend(poiMods.detailMask.g, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#if defined(PROP_DETAILNORMALMAP) || !defined(OPTIMIZER_ENABLED)
				half3 detailNormal = PoiUnpackNormalScale(POI2D_SAMPLER_PAN_STOCHASTIC(_DetailNormalMap, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0), 10.0 * poiMods.detailMask.g);
				poiMesh.tangentSpaceNormal = PoiBlendNormal(detailNormal, poiMesh.tangentSpaceNormal);
				#endif
			}
			#endif
			float calculateGlowInTheDark__1(in float minLight, in float maxLight, in float minEmissionMultiplier, in float maxEmissionMultiplier, in float enabled, in float worldOrMesh, in PoiLight poiLight)
			{
				float glowInTheDarkMultiplier__1 = 1;
				if (enabled)
				{
					float3 lightValue__1 = worldOrMesh ? CalculateLuminance(poiLight.finalLighting.rgb) : CalculateLuminance(poiLight.directColor.rgb);
					float gitdeAlpha__1 = saturate(inverseLerp(minLight, maxLight, dot(lightValue__1, float3(0.299, 0.587, 0.114))));
					glowInTheDarkMultiplier__1 = lerp(minEmissionMultiplier, maxEmissionMultiplier, gitdeAlpha__1);
				}
				return glowInTheDarkMultiplier__1;
			}
			float calculateScrollingEmission__1(in float3 direction, in float velocity, in float interval, in float scrollWidth, float offset, float3 position)
			{
				scrollWidth = max(scrollWidth, 0);
				float phase__1 = 0;
				phase__1 = dot(position, direction);
				phase__1 -= (POI_TIME.y + offset) * velocity;
				phase__1 /= interval;
				phase__1 -= floor(phase__1);
				phase__1 = saturate(phase__1);
				return (pow(phase__1, scrollWidth) + pow(1 - phase__1, scrollWidth * 4)) * 0.5;
			}
			float calculateBlinkingEmission__1(in float blinkMin, in float blinkMax, in float blinkVelocity, float offset)
			{
				float amplitude__1 = (blinkMax - blinkMin) * 0.5f;
				float base__1 = blinkMin + amplitude__1;
				return sin((POI_TIME.y + offset) * blinkVelocity) * amplitude__1 + base__1;
			}
			void applyALEmmissionStrength__1(in PoiMods poiMods, inout float emissionStrength, in float2 emissionStrengthMod, in float emissionStrengthBand, in float2 _EmissionALMultipliers, in float _EmissionALMultipliersBand, in float enabled)
			{
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && enabled)
				{
					emissionStrength += lerp(emissionStrengthMod.x, emissionStrengthMod.y, poiMods.audioLink[emissionStrengthBand]);
					emissionStrength *= lerp(_EmissionALMultipliers.x, _EmissionALMultipliers.y, poiMods.audioLink[_EmissionALMultipliersBand]);
				}
				#endif
			}
			void applyALCenterOutEmission__1(in PoiMods poiMods, in float nDotV, inout float emissionStrength, in float size, in float band, in float2 emissionToAdd, in float enabled, in float duration)
			{
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && enabled)
				{
					float intensity__1;
					[flatten]
					if (duration >= 0)
					{
						intensity__1 = getBandAtTime(band, saturate(remap(nDotV, 1, 0, 0, duration)), size);
					}
					else
					{
						duration *= -1;
						intensity__1 = getBandAtTime(band, saturate(remap(pow(nDotV, 2), 0, 1 + duration, 0, duration)), size);
					}
					emissionStrength += lerp(emissionToAdd[0], emissionToAdd[1], intensity__1);
				}
				#endif
			}
			#ifdef POI_EMISSION_1
			float3 applyEmission__1(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam, in PoiMods poiMods)
			{
				float3 emission0__1 = 0;
				float emissionAlpha__1 = 1;
				float emissionStrength0__1 = 0.1;
				float3 emissionColor0__1 = 0;
				applyALEmmissionStrength__1(poiMods, emissionStrength0__1, float4(0,0,0,0), 0.0, float4(1,1,0,0), 0.0, 0.0);
				applyALCenterOutEmission__1(poiMods, poiLight.nDotV, emissionStrength0__1, 0.0, 0.0, float4(0,0,0,0), 0.0, 1.0);
				float glowInTheDarkMultiplier0__1 = calculateGlowInTheDark__1(0.0, 1.0, 1.0, 0.0, 0.0, 0.0, poiLight);
				#ifdef POI_GRABPASS
				float mixBaseColor__1 = 0;
				#else
				float mixBaseColor__1 = 0.0;
				#endif
				#if defined(PROP_EMISSIONMAP1) || !defined(OPTIMIZER_ENABLED)
				float4 emissionTex__1 = 0;
				if (!0.0)
				{
					emissionTex__1 = POI2D_SAMPLER_PAN(_EmissionMap1, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				}
				else
				{
					float centerOutValue__1 = poiLight.nDotV;
					centerOutValue__1 = 0.0 ? 1 - acos(centerOutValue__1) * 0.63662 : centerOutValue__1;
					emissionTex__1 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMap1, _MainTex, ((.5 + centerOutValue__1 * .5) * float4(1,1,0,0).xy) + POI_TIME.x * 5.0);
				}
				emissionColor0__1 = emissionTex__1.rgb * lerp(1, poiFragData.baseColor, mixBaseColor__1).rgb * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				emissionAlpha__1 = emissionTex__1.a;
				#else
				emissionColor0__1 = lerp(1, poiFragData.baseColor, mixBaseColor__1).rgb * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				#endif
				float3 inverseLighting__1 = saturate((1.0 - poiLight.directColor) * sqrt(poiLight.directColor));
				emissionColor0__1 = lerp(emissionColor0__1.rgb, emissionColor0__1.rgb * inverseLighting__1, 0.0);
				emissionStrength0__1 *= emissionAlpha__1;
				if (0.0)
				{
					float3 pos__1 = poiMesh.localPos;
					if (0.0)
					{
						pos__1 = poiMesh.vertexColor.rgb;
					}
					if (0.0)
					{
						#if defined(PROP_EMISSIONSCROLLINGCURVE1) || !defined(OPTIMIZER_ENABLED)
						emissionStrength0__1 *= UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionScrollingCurve1, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)) + (dot(pos__1, float4(0,-10,0,0).xyz) * 20.0) + POI_TIME.x * 10.0).r;
						#endif
					}
					else
					{
						emissionStrength0__1 *= calculateScrollingEmission__1(float4(0,-10,0,0).xyz, 10.0, 20.0, 10.0, 0.0, pos__1);
					}
				}
				if (0.0)
				{
					emissionStrength0__1 *= calculateBlinkingEmission__1(0.0, 1.0, 4.0, 0.0);
				}
				if(0.0)
				{
					emissionColor0__1 = hueShift(emissionColor0__1, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
					emissionColor0__1 = lerp(emissionColor0__1, dot(emissionColor0__1, float3(0.3, 0.59, 0.11)), - (0.0));
				}
				#ifdef POI_AUDIOLINK
				if(poiMods.audioLinkAvailable && 0.0 && 0.0)
				{
					float alColorOverrideIntensity__1 = poiMods.audioLink[0.0];
					float alColorOverrideBlend__1 = evalRamp4(alColorOverrideIntensity__1, float4(0,1,0,1));
					if(alColorOverrideBlend__1 > 0)
					{
						float3 alOverrideColor__1 = float4(1,1,1,1).rgb;
						if(0.0 > 0)
						{
							alOverrideColor__1 = poiThemeColor(poiMods, alOverrideColor__1, 0.0);
						}
						emissionColor0__1 = lerp(emissionColor0__1, alOverrideColor__1, alColorOverrideBlend__1);
					}
				}
				#endif
				#if defined(PROP_EMISSIONMASK1) || !defined(OPTIMIZER_ENABLED)
				float emissionMask0__1 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMask1, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)) + POI_TIME.x * float4(0,0,0,0))[0.0];
				#else
				float emissionMask0__1 = 1;
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					emissionMask0__1 = 1 - emissionMask0__1;
				}
				if (0.0 > 0)
				{
					emissionMask0__1 = maskBlend(emissionMask0__1, poiMods.globalMask[0.0 - 1], 2.0);
				}
				emissionStrength0__1 *= glowInTheDarkMultiplier0__1 * emissionMask0__1;
				emission0__1 = max(emissionStrength0__1 * emissionColor0__1, 0);
				#ifdef POI_DISSOLVE
				if (_DissolveEmissionSide != 2)
				{
					emission0__1 *= lerp(1 - dissolveAlpha, dissolveAlpha, _DissolveEmissionSide);
				}
				#endif
				poiFragData.emission += emission0__1;
				return emission0__1 * 0.0;
			}
			#endif
			#ifdef _POI_DEPTH_RIMLIGHT
			void ApplyDepthRimLighting(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, inout PoiLight poiLight, in PoiMods poiMods)
			{
				if (!DepthTextureExists()) return;
				float perspectiveDivide = 1.0f / poiCam.clipPos.w;
				float2 screenPos = poiCam.posScreenSpace.xy * perspectiveDivide;
				float z = SampleScreenDepth(screenPos);
				#if UNITY_REVERSED_Z
				if (z == 0) return;
				#else
				if (z == 1) return;
				#endif
				float4 direction = poiCam.worldDirection * perspectiveDivide;
				float linearDepth = CorrectedLinearEyeDepth(z, direction.w);
				float distanceFade = 1;
				if (0.0 > 0)
				{
					distanceFade = smoothstep(0.0, 0.0 + 0.001, poiCam.distanceToVert);
					if (distanceFade <= 0) return;
				}
				float cameraClipScale = 0.5 > 0 ? saturate(poiCam.distanceToVert / 0.5) : 1;
				float scale = WorldToScreenScale(0.1 * 0.1, poiCam.distanceToVert) * cameraClipScale;
				float2 aspectCorrect = GetScreenAspectCorrection();
				float3 viewNorm = normalize(mul((float3x3)UNITY_MATRIX_V, poiMesh.normals[0.0]));
				float normalFacing = 1.0 ? 1 : saturate(1 - abs(viewNorm.z));
				float2 viewNormXY = viewNorm.xy;
				float2 N_View = length(viewNormXY) > 1e-6 ? normalize(viewNormXY) : float2(0, 1);
				float3 viewLight = normalize(mul((float3x3)UNITY_MATRIX_V, poiLight.direction));
				float2 viewLightXY = viewLight.xy;
				float2 L_View = length(viewLightXY) > 1e-6 ? normalize(viewLightXY) : float2(0, 1);
				float rim = 0;
				float depthDiff = 0;
				float depthThreshold = 1.0 / max(cameraClipScale, 0.001);
				float depthThresholdLow = depthThreshold * 0.5;
				float skyboxDepth = linearDepth + depthThreshold * 2;
				switch (0.0)
				{
					case 0:
					{
						float2 ssUV1 = screenPos + N_View * aspectCorrect * scale;
						float2 ssUV2 = screenPos - N_View * aspectCorrect * scale;
						float sampleZ1 = SampleScreenDepth(ssUV1);
						float sampleZ2 = SampleScreenDepth(ssUV2);
						#if UNITY_REVERSED_Z
						float sampleDepth1 = sampleZ1 < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ1, direction.w);
						float sampleDepth2 = sampleZ2 < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ2, direction.w);
						#else
						float sampleDepth1 = sampleZ1 > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ1, direction.w);
						float sampleDepth2 = sampleZ2 > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ2, direction.w);
						#endif
						depthDiff = max(sampleDepth1 - linearDepth, sampleDepth2 - linearDepth);
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
					case 1:
					{
						[unroll]
						for (int i = 0; i < 4; i++)
						{
							float2 ssUV1 = screenPos + depthRimSampleOffsets4[i] * aspectCorrect * scale;
							float sampleZ = SampleScreenDepth(ssUV1);
							#if UNITY_REVERSED_Z
							float sampleDepth = sampleZ < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#else
							float sampleDepth = sampleZ > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#endif
							depthDiff = max(depthDiff, sampleDepth - linearDepth);
						}
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
					case 2:
					{
						[unroll]
						for (int i = 0; i < 8; i++)
						{
							float2 ssUV1 = screenPos + depthRimSampleOffsets8[i] * aspectCorrect * scale;
							float sampleZ = SampleScreenDepth(ssUV1);
							#if UNITY_REVERSED_Z
							float sampleDepth = sampleZ < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#else
							float sampleDepth = sampleZ > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#endif
							depthDiff = max(depthDiff, sampleDepth - linearDepth);
						}
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
				}
				float lightDirFactor = 1.0 == 0
				? saturate(lerp(poiLight.vertexNDotL, poiLight.nDotL, 0.0))
				: saturate(dot(L_View, N_View));
				rim *= lerp(1, lightDirFactor, 0.0);
				rim *= lerp(1, poiLight.attenuation, 0.0);
				rim *= lerp(1, dot(poiLight.rampedLightMap, float3(0.299, 0.587, 0.114)), 0.0);
				float mask = POI2D_SAMPLER_PAN(_DepthRimMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[0.0];
				if (1.0)
				{
					rim *= step(0.5, mask) * step(0.5, distanceFade);
				}
				else
				{
					rim *= mask * distanceFade;
				}
				float3 rimColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0).rgb * lerp(1, poiLight.directColor, 0.0) * lerp(1, poiFragData.baseColor, 1.0) * 1.0;
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				poiLight.finalLightAdd += rim * rimColor * 0.0;
				#endif
				poiFragData.emission += rim * rimColor * 7.7;
				poiFragData.baseColor = lerp(poiFragData.baseColor, rimColor, rim * 0.0);
				poiFragData.baseColor += rim * rimColor * 1.0;
				poiFragData.baseColor = lerp(poiFragData.baseColor, 1 - (1 - poiFragData.baseColor) * (1 - rimColor), rim * 0.0);
				poiFragData.baseColor *= lerp(1, rimColor, rim * 1.0);
			}
			#endif
			#ifdef _SUNDISK_SIMPLE
			float3 RandomColorFromPoint(float2 rando, float hue)
			{
				float saturation = lerp(float4(0.8,1,0,1).x, float4(0.8,1,0,1).y, rando.x);
				float value = lerp(float4(0.8,1,0,1).x, float4(0.8,1,0,1).y, rando.y);
				return HSVtoRGB(float3(hue, saturation, value));
			}
			float2 glitterComputeRotation(float2 pos, float2 center, float2 randomSeed, float timeOffset)
			{
				float randomBoy = poiRand(randomSeed);
				float rotSpeed = lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, randomBoy);
				float randomOffset = 0.0 ? randomBoy : 0;
				float totalSpeed = 0.0 + rotSpeed;
				if (totalSpeed == 0 && timeOffset != 0)
				totalSpeed = 1;
				float theta = radians(randomOffset * 360 + (POI_TIME.x + timeOffset) * totalSpeed * 360);
				float cs = cos(theta);
				float sn = sin(theta);
				return float2((pos.x - center.x) * cs - (pos.y - center.y) * sn + center.x,
				(pos.x - center.x) * sn + (pos.y - center.y) * cs + center.y);
			}
			void processGlitterParticle(
			inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods,
			float2 randPoint, float2 diff,
			float3 norm, float jaggyFix, float glitterFwidth, float glitterSpeedOffset, float glitterRotationTimeOffset,
			bool needsRotation, float glitterMask, float glitterMaxBrightness, float glitterBrightnessOffset)
			{
				float dist = length(diff);
				float randomFromPoint = poiRand(randPoint);
				float size = 0.3;
				if (0.0)
				size = lerp(float4(0.1,0.5,0,1).x, float4(0.1,0.5,0,1).y, randomFromPoint);
				#ifdef POI_AUDIOLINK
				if (0.0)
				size = saturate(size + lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]));
				#endif
				float2 rotatedDiff = diff;
				half glitterAlpha = 1;
				switch (0.0)
				{
					case 0:
					{
						if (0.0 == 1)
						{
							float sqDist = dot(diff, diff);
							glitterAlpha = saturate((size - sqDist) / clamp(fwidth(sqDist), 0.0001, 1.0));
						}
						else
						{
							glitterAlpha = saturate((size - dist) / glitterFwidth);
						}
						break;
					}
					case 1:
					{
						if (needsRotation)
						rotatedDiff = glitterComputeRotation(diff, 0, randPoint * 200, glitterRotationTimeOffset);
						float squareAA = max(glitterFwidth, .1 * jaggyFix);
						glitterAlpha = (1. - smoothstep(size - squareAA, size, abs(rotatedDiff.x))) * (1. - smoothstep(size - squareAA, size, abs(rotatedDiff.y)));
						break;
					}
				}
				if (glitterAlpha <= 0) return;
				float3 finalGlitter = 0;
				half3 glitterColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				float3 randomRotation = 0;
				switch (0.0)
				{
					case 0:
					{
						if (10.0 + glitterSpeedOffset > 0)
						randomRotation = randomFloat3WiggleRange(randPoint, 90.0, 10.0, glitterSpeedOffset);
						else
						randomRotation = poiRand3Range(randPoint, 90.0);
						float3 glitterReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						finalGlitter = lerp(0, 0.0 * glitterAlpha, glitterAlpha) + max(pow(saturate(dot(lerp(glitterReflectionDirection, poiCam.viewDir, 0.8), poiCam.viewDir)), 300.0), 0);
						finalGlitter *= glitterAlpha;
						break;
					}
					case 1:
					{
						float3 cellRand3 = float3(poiRand(randPoint), poiRand(randPoint + 0.5), poiRand(randPoint.yx));
						float3 glitterNorm = abs(frac(cellRand3 * 14.274 + (POI_TIME.x + glitterSpeedOffset) * 0.25) * 2.0 - 1.0);
						glitterNorm = normalize(glitterNorm * 2.0 - 1.0);
						float3 glitterCamDir = lerp(poiCam.forwardDir, poiCam.viewDir, 0.0);
						float glitterDot = dot(glitterNorm, glitterCamDir);
						float glitterVal = abs(frac(glitterDot * 0.25 + 0.25) - 0.5) * 4.0 - 1.0;
						glitterVal = saturate(1.0 - (glitterVal * 50.0 + 50.0));
						glitterVal = pow(glitterVal, 1.0);
						float3 linearHalfDir = normalize(poiCam.viewDir + poiLight.direction * 0.0);
						float linearNDotH = saturate(dot(norm, linearHalfDir));
						glitterVal = saturate(glitterVal * saturate(linearNDotH * 0.0 + 1.0 - 0.0));
						finalGlitter = max(0.0, glitterVal) * glitterAlpha;
						break;
					}
					case 2:
					{
						if (10.0 + glitterSpeedOffset > 0)
						randomRotation = randomFloat3WiggleRange(randPoint, 90.0, 10.0, glitterSpeedOffset);
						else
						randomRotation = poiRand3Range(randPoint, 90.0);
						float3 glitterLightReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						half lightAlpha = glitterAlpha * poiLight.nDotLSaturated;
						float3 halfDir = normalize(poiLight.direction + poiCam.viewDir);
						float specAngle = max(dot(halfDir, glitterLightReflectionDirection), 0.0);
						finalGlitter = lerp(0, 0.0 * lightAlpha, lightAlpha) + max(pow(specAngle, 300.0), 0);
						glitterColor *= poiLight.directColor;
						finalGlitter *= lightAlpha;
						break;
					}
				}
				glitterColor *= lerp(1, poiFragData.baseColor, 0.0);
				#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
				glitterColor *= POI2D_SAMPLER_PANGRAD(_GlitterColorMap, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), poiMesh.dx, poiMesh.dy).rgb;
				#endif
				float2 uv = remapClamped(-size, size, rotatedDiff, 0, 1);
				if (!0.0 && needsRotation)
				uv = glitterComputeRotation(uv, .5, randPoint * 200, glitterRotationTimeOffset);
				#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
				float2 glitterTexUV = uv;
				if (0.0)
				{
					float glitterSeed = poiRand(randPoint);
					float frameMode = 0.0 == 1 ? 2 : 0;
					float glitterFrameIndex = poiSpriteSheetFrameIndex(
					1.0, 1.0, 0.0,
					frameMode, 10.0, 0.0,
					0, glitterSeed);
					glitterTexUV = poiSpriteSheetUV(uv, 1.0, 1.0, glitterFrameIndex, 0, float4(1,1,0,0).xy);
				}
				float2 finalGlitterTexUV = poiUV(glitterTexUV, float4(1,1,0,0));
				if (0.0)
				finalGlitterTexUV = sharpSample(float4(1,1,1,1), finalGlitterTexUV);
				float4 glitterTexture = POI2D_SAMPLER_PANGRAD(_GlitterTexture, _linear_clamp, finalGlitterTexUV, 0, poiMesh.dx, poiMesh.dy);
				#else
				float4 glitterTexture = 1;
				#endif
				glitterColor *= glitterTexture.rgb;
				float2 colorRando = poiRand2(randPoint.x + randPoint.y);
				float randomHue = poiRand(colorRando);
				if (0.0)
				glitterColor *= RandomColorFromPoint(colorRando, randomHue);
				if (0.0)
				{
					float hueOffset = 0.0 + POI_TIME.x * 0.0;
					if (0.0)
					hueOffset += randomHue;
					glitterColor.rgb = hueShift(glitterColor.rgb, hueOffset, 0.0, 1.0);
				}
				#ifdef POI_AUDIOLINK
				if(poiMods.audioLinkAvailable && 0.0 && 0.0)
				{
					float alColorOverrideIntensity = poiMods.audioLink[0.0];
					float alColorOverrideBlend = evalRamp4(alColorOverrideIntensity, float4(0,1,0,1));
					if(alColorOverrideBlend > 0)
					{
						float3 alOverrideColor = float4(1,1,1,1).rgb;
						if(0.0 > 0)
						{
							alOverrideColor = poiThemeColor(poiMods, alOverrideColor, 0.0);
						}
						glitterColor = lerp(glitterColor, alOverrideColor, alColorOverrideBlend);
					}
				}
				#endif
				if (0.0 == 1)
				{
					poiFragData.baseColor = lerp(poiFragData.baseColor, finalGlitter * glitterColor * (glitterMaxBrightness + glitterBrightnessOffset), finalGlitter * glitterTexture.a * glitterMask);
					poiFragData.emission += finalGlitter * glitterColor * max(0, ((glitterMaxBrightness + glitterBrightnessOffset) - 1) * glitterTexture.a) * glitterMask;
				}
				else
				{
					poiFragData.emission += finalGlitter * glitterColor * (glitterMaxBrightness + glitterBrightnessOffset) * glitterTexture.a * glitterMask;
				}
			}
			void applyGlitter(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods)
			{
				float glitterRotationTimeOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				{
					glitterRotationTimeOffset += AudioLinkGetChronoTime(0.0, 0.0) * 0.0;
				}
				#endif
				bool needsRotation = 0.0 || 0.0 != 0 || any(float4(0,0,0,0).xy) || glitterRotationTimeOffset != 0;
				float3 norm = lerp(poiMesh.normals[0], poiMesh.normals[1], 0.0);
				float jaggyFix = pow(poiCam.distanceToVert, 2) * 0.0;
				float glitterSpeedOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterSpeedOffset = AudioLinkGetChronoTime(0.0, 0.0) * 0.0;
				#endif
				#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
				float glitterMask = POI2D_SAMPLER_PAN(_GlitterMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[0.0];
				#else
				float glitterMask = 1;
				#endif
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterMask = clamp(glitterMask + lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]), 0, glitterMask);
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				glitterMask = 1 - glitterMask;
				glitterMask *= lerp(1, poiLight.rampedLightMap * poiLight.attenuation, 0.0);
				glitterMask *= lerp(1, poiLight.directLuminance, 0.0);
				glitterMask *= float4(1,1,1,1).a;
				if (0.0 > 0)
				glitterMask = maskBlend(glitterMask, poiMods.globalMask[0.0 - 1], 2.0);
				if (glitterMask <= 0) return;
				float glitterBrightnessOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterBrightnessOffset = max(lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]), 0);
				#endif
				float glitterMaxBrightness = 0.0 == 1 ? 3.0 : 3.0;
				float maxSize = 0.0 ? float4(0.1,0.5,0,1).y : 0.3;
				#ifdef POI_AUDIOLINK
				if (0.0)
				maxSize = saturate(maxSize + max(float4(0,0,0,0).x, float4(0,0,0,0).y));
				#endif
				float2 st = (poiMesh.uv[0.0] + float4(0,0,0,0).xy * POI_TIME.x) * 300.0;
				float2 dd = fwidth(st);
				float distanceLODEnabled = 0.0 == 1 ? 1.0 : 1.0;
				float glitterFwidth = clamp(length(dd), 0.0001, 1.0);
				if (distanceLODEnabled)
				{
					float lodFactor = frac(sin(dot(floor(st / floor(dd + 3.0)), float2(12.9898, 78.233))) * 46203.4357) + 0.5;
					float2 lodScale = floor(dd + lodFactor * 0.5);
					st = st / max(1.0, lodScale) + 300.0 * lodScale;
				}
				[loop] for (uint glitterLayer = 0; glitterLayer < uint(1.0); glitterLayer++)
				{
					float2 i_st = floor(st);
					float2 f_st = frac(st);
					float m_dist = 10.;
					float2 randoPoint = 0;
					float2 closestDiff = 0;
					[loop] for (int cellJ = -1; cellJ <= 1; cellJ++)
					{
						[loop] for (int cellI = -1; cellI <= 1; cellI++)
						{
							float2 neighbor = float2(cellI, cellJ);
							float2 cellRando = poiRand2(i_st + neighbor + glitterLayer * 0.5141);
							float2 cellDiff = neighbor + cellRando * 1.0 - f_st;
							float cellDist = length(cellDiff);
							
							if (0.0)
							{
								if (cellDist > maxSize + 1.0) continue;
								processGlitterParticle(poiFragData, poiMesh, poiCam, poiLight, poiMods,
								cellRando, cellDiff,
								norm, jaggyFix, glitterFwidth, glitterSpeedOffset, glitterRotationTimeOffset,
								needsRotation, glitterMask, glitterMaxBrightness, glitterBrightnessOffset);
							}
							else
							{
								if (cellDist < m_dist)
								{
									closestDiff = cellDiff;
									m_dist = cellDist;
									randoPoint = cellRando;
								}
							}
						}
					}
					
					if (!0.0)
					{
						processGlitterParticle(poiFragData, poiMesh, poiCam, poiLight, poiMods,
						randoPoint, closestDiff,
						norm, jaggyFix, glitterFwidth, glitterSpeedOffset, glitterRotationTimeOffset,
						needsRotation, glitterMask, glitterMaxBrightness, glitterBrightnessOffset);
					}
				}
			}
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			void CalculateUTSSpecular(inout PoiFragData poiFragData, in PoiCam poiCam, inout PoiLight poiLight, in PoiMesh poiMesh, in PoiMods poiMods)
			{
				float nDotH = dot(lerp(poiMesh.normals[0], poiMesh.normals[1], 1.0), poiLight.halfDir);
				float specArea = 0.5 * nDotH + 0.5;
				#if defined(PROP_HIGHCOLOR_TEX) || !defined(OPTIMIZER_ENABLED)
				float3 specularMap = POI2D_SAMPLER_PAN(_HighColor_Tex, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				#else
				float3 specularMap = 1;
				#endif
				float specMask1 = 0;
				float specMask2 = 0;
				if (0.0)
				{
					specMask1 += pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.253;
					specMask2 += pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.0;
				}
				else
				{
					specMask1 += poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 1.0) * 0.253;
					specMask2 += poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 0.0) * 0.0;
				}
				#if defined(PROP_SET_HIGHCOLORMASK) || !defined(OPTIMIZER_ENABLED)
				float specularMask = POI2D_SAMPLER_PAN(_Set_HighColorMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[1.0];
				#else
				float specularMask = 1;
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					specularMask = 1 - specularMask;
				}
				specularMask = saturate(specularMask + 0.0);
				float specMask = saturate(specMask1 + specMask2) * specularMask * lerp(poiLight.rampedLightMap, 1, 0.0);
				float attenuation = min(lerp(poiLight.nDotLSaturated, 1, 0.0), lerp(lerp(poiLight.attenuation, 1, 0.0), 1, 0.0));
				if (poiLight.lightType == POI_POINT_SPOT)
				{
					attenuation *= lerp(poiLight.additiveShadow, 1, 0.0);
				}
				float finalSpecMask = min(min(specMask, poiLight.occlusion), attenuation) * 1.0;
				switch (0.0)
				{
					case 0:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0), finalSpecMask);
						break;
					}
					case 1:
					{
						poiLight.finalLightAdd += max(0, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0) * finalSpecMask);
						break;
					}
					case 2:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, blendScreen(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0)), finalSpecMask);
						break;
					}
					case 3:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, poiFragData.baseColor * specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0), finalSpecMask);
						break;
					}
				}
				float3 vSpecMask = 0;
				if (poiFragData.toggleVertexLights)
				{
					#if defined(VERTEXLIGHT_ON)
					for (int index = 0; index < 4; index++)
					{
						if (!any(poiLight.vPosition[index])) continue;
						specArea = 0.5 * poiLight.vDotNH[index] + 0.5;
						if (0.0)
						{
							vSpecMask = pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.253 * poiLight.vAttenuation[index];
							vSpecMask = max(vSpecMask, pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.0 * poiLight.vAttenuation[index]);
						}
						else
						{
							vSpecMask = poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 1.0) * 0.253 * poiLight.vAttenuation[index];
							vSpecMask = max(vSpecMask, poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 0.0) * 0.0 * poiLight.vAttenuation[index]);
						}
						vSpecMask *= specularMask;
						float finalSpecMask = min(min(vSpecMask, poiLight.occlusion), attenuation) * 1.0;
						switch (0.0)
						{
							case 0:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0), finalSpecMask);
								break;
							}
							case 1:
							{
								poiLight.finalLightAdd += max(0, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0) * finalSpecMask);
								break;
							}
							case 2:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, blendScreen(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0)), finalSpecMask);
								break;
							}
							case 3:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, poiFragData.baseColor * specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0), finalSpecMask);
								break;
							}
						}
					}
					#endif
				}
			}
			#endif
			#endif
			void PoiLightingNoLoopModules(inout PoiLight poiLight, inout PoiFragData poiFragData,
			in PoiMesh poiMesh, in PoiCam poiCam, inout PoiMods poiMods)
			{
			}
			void PoiLightingDependantModules(inout PoiLight poiLight, inout PoiFragData poiFragData, inout MochieBRDFData mochieBRDFData,
			in PoiMesh poiMesh, in PoiCam poiCam, inout PoiMods poiMods)
			{
				#ifdef POI_STYLIZED_StylizedSpecular
				#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
				CalculateUTSSpecular(poiFragData, poiCam, poiLight, poiMesh, poiMods);
				#endif
				#endif
				#ifdef POI_STYLIZED_StylizedSpecular
				#endif
			}
			float4 frag(VertexOut i, bool facing : SV_IsFrontFace) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				PoiSHAr = unity_SHAr;
				PoiSHAg = unity_SHAg;
				PoiSHAb = unity_SHAb;
				PoiSHBr = unity_SHBr;
				PoiSHBg = unity_SHBg;
				PoiSHBb = unity_SHBb;
				PoiSHC =  unity_SHC;
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				#if POI_PIPE == POI_URP
				PoiLightOut poiLightOut;
				PoiInitStruct(PoiLightOut, poiLightOut);
				#endif
				MochieBRDFData mochieBRDFData;
				PoiInitStruct(MochieBRDFData, mochieBRDFData);
				poiMods.globalEmission = 1;
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				#ifdef POI_UDIMDISCARD
				applyUDIMDiscard(i, facing);
				#endif
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
				poiMesh.objNormal = mul((float3x3)unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				poiMesh.isRightHand = i.tangent.w > 0.0;
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && 1)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				poiCam.viewDir = !IsOrthographicCamera() ? (_WorldSpaceCameraPos - i.worldPos.xyz) : UNITY_MATRIX_I_V._m02_m12_m22;
				poiCam.viewDir = normalize(poiCam.viewDir);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				poiMesh.uv[0] = applyUVSettings(poiMesh.uv[0], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[1] = applyUVSettings(poiMesh.uv[1], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[2] = applyUVSettings(poiMesh.uv[2], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[3] = applyUVSettings(poiMesh.uv[3], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				if (0.0 && !poiMesh.isFrontFace)
				{
					poiMesh.uv[0].x += 1.0;
					poiMesh.uv[1].x += 1.0;
					poiMesh.uv[2].x += 1.0;
					poiMesh.uv[3].x += 1.0;
				}
				poiMesh.uv[4] = calculatePanosphereUV(poiMesh);
				poiMesh.uv[5] = calculateWorldUV(poiMesh);
				poiMesh.uv[6] = calculatePolarCoordinate(poiMesh);
				poiMesh.uv[8] = calculatelocalUV(poiMesh);
				float3 worldViewUp = normalize(float3(0, 1, 0) - poiCam.viewDir * dot(poiCam.viewDir, float3(0, 1, 0)));
				float3 worldViewRight = normalize(cross(poiCam.viewDir, worldViewUp));
				poiMesh.uv[9] = float2(dot(worldViewRight, poiMesh.normals[0]), dot(worldViewUp, poiMesh.normals[0])) * 0.5 + 0.5;
				poiMods.globalMask[0] = 1;
				poiMods.globalMask[1] = 1;
				poiMods.globalMask[2] = 1;
				poiMods.globalMask[3] = 1;
				poiMods.globalMask[4] = 1;
				poiMods.globalMask[5] = 1;
				poiMods.globalMask[6] = 1;
				poiMods.globalMask[7] = 1;
				poiMods.globalMask[8] = 1;
				poiMods.globalMask[9] = 1;
				poiMods.globalMask[10] = 1;
				poiMods.globalMask[11] = 1;
				poiMods.globalMask[12] = 1;
				poiMods.globalMask[13] = 1;
				poiMods.globalMask[14] = 1;
				poiMods.globalMask[15] = 1;
				ApplyGlobalMaskModifiers(poiMesh, poiMods, poiCam);
				#if POI_PIPE == POI_URP
				InputData inputData;
				PoiInitStruct(InputData, inputData);
				inputData.positionWS = poiMesh.worldPos;
				inputData.normalWS = poiMesh.normals[1];
				inputData.viewDirectionWS = poiCam.viewDir;
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(i.pos);
				#endif
				float2 mainUV = poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0));
				if (0.0)
				{
					mainUV = sharpSample(float4(0.0009765625,0.0009765625,1024,1024), mainUV);
				}
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, float4(0,0,0,0), 0.0);
				mainTexture.a = max(mainTexture.a, 0.0);
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = PoiUnpackNormalScale(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0)), float4(0,0,0,0), 0.0), 0.0);
				#else
				poiMesh.tangentSpaceNormal = float3(0, 0, 1);
				#endif
				#ifdef FINALPASS
				#if defined(POI_PASS_BASE) || defined(POI_PASS_ADD) || defined(POI_PASS_META) || defined(POI_PASS_DEPTH_NORMALS) || defined(POI_PASS_DEPTH_ONLY)
				ApplyDetailNormal(poiMods, poiMesh);
				#endif
				#endif
				float3 tangentSpaceNormal = float3(0, 0, 1);
				poiMesh.normals[0] = normalize(
				tangentSpaceNormal.x * poiMesh.tangent[0] +
				tangentSpaceNormal.y * poiMesh.binormal[0] +
				tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = getCameraPosition();
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#ifdef UNITY_SINGLE_PASS_STEREO
				poiCam.posScreenSpace.x = poiCam.posScreenSpace.x * 0.5;
				#endif
				poiCam.screenUV = poiCam.posScreenSpace.xy / poiCam.posScreenSpace.w;
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = i.worldDir;
				calculateGlobalThemes(poiMods);
				if (_UdonForceSceneLighting)
				{
					poiLight.lightingMinLightBrightness = 0;
					poiLight.lightingCapEnabled = 0;
					poiLight.lightingMonochromatic = 0;
				}
				else
				{
					poiLight.lightingMinLightBrightness = _LightingMinLightBrightness;
					poiLight.lightingCapEnabled = _LightingCapEnabled;
					poiLight.lightingMonochromatic = _LightingMonochromatic;
				}
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				float3 L0 = float3(0, 0, 0);
				float3 L1r = float3(0, 0, 0);
				float3 L1g = float3(0, 0, 0);
				float3 L1b = float3(0, 0, 0);
				#if POI_PIPE == POI_BIRP
				if (_UdonLightVolumeEnabled && 1.0)
				{
					LightVolumeSH(poiMesh.worldPos, L0, L1r, L1g, L1b, poiMesh.normals[1] * 0.0);
					PoiSHAr = float4(L1r, L0.r);
					PoiSHAg = float4(L1g, L0.g);
					PoiSHAb = float4(L1b, L0.b);
					PoiSHBr = 0;
					PoiSHBg = 0;
					PoiSHBb = 0;
					PoiSHC = 0;
				}
				#endif
				#if POI_PIPE == POI_URP
				#ifdef PROBE_VOLUMES_L1
				if (1.0)
				{
					APVResources apvRes = FillAPVResources();
					float3 posWS = GetAbsolutePositionWS(poiMesh.worldPos + poiMesh.normals[1] * 0.0);
					posWS = AddNoiseToSamplingPosition(posWS, poiCam.clipPos.xy, poiCam.viewDir);
					APVSample apvSample = SampleAPV(posWS, poiMesh.normals[1], GetMeshRenderingLayer(), poiCam.viewDir);
					if (apvSample.status != APV_SAMPLE_STATUS_INVALID)
					{
						apvSample.Decode();
						PoiSHAr = float4(apvSample.L1_R, apvSample.L0.r);
						PoiSHAg = float4(apvSample.L1_G, apvSample.L0.g);
						PoiSHAb = float4(apvSample.L1_B, apvSample.L0.b);
						#ifdef PROBE_VOLUMES_L2
						PoiSHBr = float4(apvSample.L2_R, apvSample.L0.r);
						PoiSHBg = float4(apvSample.L2_G, apvSample.L0.r);
						PoiSHBb = float4(apvSample.L2_B, apvSample.L0.r);
						PoiSHC = float4(apvSample.L2_C, 0);
						#else
						PoiSHBr = 0;
						PoiSHBg = 0;
						PoiSHBb = 0;
						PoiSHC = 0;
						#endif
					}
				}
				#endif
				#endif
				#endif
				#if defined(PROP_LIGHTINGDETAILSHADOWMAPS)
				float4 DetailShadows = POI2D_SAMPLER_PAN(_LightingDetailShadowMaps, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				float poiDetailShadowAdditional = lerp(1, DetailShadows.r, 1.0) * lerp(1, DetailShadows.g, 0.0) * lerp(1, DetailShadows.b, 0.0) * lerp(1, DetailShadows.a, 0.0);
				float poiDetailShadowMain = lerp(1, DetailShadows.r, 1.0) * lerp(1, DetailShadows.g, 0.0) * lerp(1, DetailShadows.b, 0.0) * lerp(1, DetailShadows.a, 0.0);
				#else
				float poiDetailShadowAdditional = 1;
				float poiDetailShadowMain = 1;
				#endif
				if (0.0 > 0)
				{
					poiDetailShadowAdditional = maskBlend(poiDetailShadowAdditional, poiMods.globalMask[0.0 - 1], 2.0);
					poiDetailShadowMain = maskBlend(poiDetailShadowMain, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#ifndef POI_PASS_ADD
				poiLight.detailShadow = poiDetailShadowMain;
				#else
				poiLight.detailShadow = poiDetailShadowAdditional;
				#endif
				#if defined(PROP_LIGHTINGSHADOWMASKS)
				float4 ShadowMasks = POI2D_SAMPLER_PAN(_LightingShadowMasks, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				poiLight.shadowMask = lerp(1, ShadowMasks.r, 1.0) * lerp(1, ShadowMasks.g, 0.0) * lerp(1, ShadowMasks.b, 0.0) * lerp(1, ShadowMasks.a, 0.0);
				#else
				poiLight.shadowMask = 1;
				#endif
				if (0.0 > 0)
				{
					poiLight.shadowMask = maskBlend(poiLight.shadowMask.r, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#if POI_PIPE == POI_URP && !defined(POI_PASS_LILFUR)
				poiLight.shadowMask = poiLight.shadowMask * PoiCalculateShadowMask(i.lightmapUV.xy);
				#endif
				#if defined(PROP_LIGHTINGAOMAPS)
				float4 AOMaps = POI2D_SAMPLER_PAN(_LightingAOMaps, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				poiLight.occlusion = min(min(min(lerp(1, AOMaps.r, 1.0), lerp(1, AOMaps.g, 0.0)), lerp(1, AOMaps.b, 0.0)), lerp(1, AOMaps.a, 0.0));
				#else
				poiLight.occlusion = 1;
				#endif
				if (0.0 > 0)
				{
					poiLight.occlusion = maskBlend(poiLight.occlusion, poiMods.globalMask[0.0 - 1], 2.0);
				}
				Light mainLight;
				PoiInitStruct(Light, mainLight);
				#if POI_PIPE == POI_BIRP
				mainLight.direction = _WorldSpaceLightPos0.xyz;
				mainLight.color = _LightColor0.rgb;
				#elif POI_PIPE == POI_URP
				mainLight = GetMainLight(TransformWorldToShadowCoord(poiMesh.worldPos), poiMesh.worldPos, poiLight.shadowMask);
				BRDFData brdfData;
				InitializeBRDFData(1, 0.0, 0.0, 0.0, poiFragData.alpha, brdfData);
				BRDFData brdfDataClearCoat;
				InitializeBRDFDataClearCoat(0.0, 0.5, brdfData, brdfDataClearCoat);
				poiLight.brdfData = brdfData;
				poiLight.brdfDataClearCoat = brdfDataClearCoat;
				#endif
				#if POI_PIPE == POI_BIRP && defined(POI_PASS_ADD)
				poiLight.lightType = (_WorldSpaceLightPos0.w == 0.0) ? POI_DIRECTIONAL : POI_POINT_SPOT;
				#else
				poiLight.lightType = POI_DIRECTIONAL;
				#endif
				poiLight.unityLight = mainLight;
				float4 debugLight = PoiLightDataSetup(poiLight, poiFragData, poiMesh, poiCam, poiMods, i, mainTexture);
				#if defined(POI_PASS_BASE) || defined(POI_PASS_ADD)
				if (0.0) return debugLight;
				#endif
				poiFragData.baseColor = mainTexture.rgb;
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				#else
				if(_TwoPassOverrideFirstPassColor == 1)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, _TwoPassColor.rgb, _TwoPassColorThemeIndex);
					poiFragData.alpha = mainTexture.a * _TwoPassColor.a;
				}
				else if(_TwoPassOverrideFirstPassColor == 2)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * _TwoPassAlphaOverride;
				}
				else
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				}
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					float4 alphaMaskTex = 1;
					#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
					alphaMaskTex = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0).xy);
					#endif
					float alphaMask = poiSampleMask(alphaMaskTex, float4(1.0, 0.0, 0.0, 0.0), 2.0, 0.0, float4(0,1,0,1), 1.0, 1.0, 0.0);
					if (0.0 == 1) poiFragData.alpha = alphaMask;
					if (0.0 == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (0.0 == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (0.0 == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				#ifdef FINALPASS
				ApplyDetailColor(poiFragData, poiMesh, poiMods);
				#endif
				poiLight.finalLighting = 1;
				poiLight.rampedLightMap = poiEdgeNonLinear(poiLight.nDotL, 0.1, .1);
				#ifndef VIGNETTE_MASKED
				PoiLightingNoLoopModules(poiLight, poiFragData, poiMesh, poiCam, poiMods);
				PoiLightingDependantModules(poiLight, poiFragData, mochieBRDFData, poiMesh, poiCam, poiMods);
				#endif
				#ifdef _POI_DEPTH_RIMLIGHT
				ApplyDepthRimLighting(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				#ifdef _SUNDISK_SIMPLE
				applyGlitter(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				#if defined(_EMISSION)|| defined(POI_EMISSION_1)|| defined(POI_EMISSION_2)|| defined(POI_EMISSION_3)
				float3 emissionBaseReplace = 0;//<ms_no_postfix>
				#endif
				#ifdef POI_EMISSION_1
				emissionBaseReplace += applyEmission__1(poiFragData, poiMesh, poiLight, poiCam, poiMods);
				#endif
				#if defined(_EMISSION)|| defined(POI_EMISSION_1)|| defined(POI_EMISSION_2)|| defined(POI_EMISSION_3)
				poiFragData.baseColor.rgb = lerp(poiFragData.baseColor.rgb, saturate(emissionBaseReplace), poiMax(emissionBaseReplace));
				#endif
				
				if (0.0)
				{
					poiFragData.baseColor *= saturate(poiFragData.alpha);
				}
				poiFragData.finalColor = poiFragData.baseColor;
				#if POI_PIPE == POI_URP
				poiLight.finalLighting += poiLightOut.totalLighting;
				#endif
				poiFragData.finalColor = poiFragData.baseColor * poiLight.finalLighting;
				#if POI_PIPE == POI_URP
				poiLight.finalLightAdd += poiLightOut.totalLightAdd;
				#endif
				poiFragData.finalColor += poiLight.finalLightAdd;
				#ifndef POI_PASS_LILFUR
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.alpha = 1.0 ? 1 : poiFragData.alpha;
				#else
				poiFragData.alpha = _AlphaForceOpaque2 ? 1 : poiFragData.alpha;
				#endif
				#endif
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				poiFragData.emission = max(poiFragData.emission * 1.0, 0);
				poiFragData.finalColor = max(poiFragData.finalColor * 1.0, 0);
				#endif
				if (0.0 == POI_MODE_OPAQUE)
				{
				}
				clip(poiFragData.alpha - 0.0);
				if (0.0 == POI_MODE_CUTOUT && !0.0)
				{
					poiFragData.alpha = 1;
				}
				poiFragData.finalColor += poiFragData.emission * poiMods.globalEmission;
				applyUnityFog(poiFragData.finalColor, i.fogData, 0.0);
				return float4(poiFragData.finalColor, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			ENDCG
		}
		Pass
		{
			Name "Add"
			Tags { "LightMode" = "ForwardAdd" }
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilCompareFunction]
				Pass [_StencilPassOp]
				Fail [_StencilFailOp]
				ZFail [_StencilZFailOp]
			}
			ZWrite Off
			Cull Back
			ZTest [_ZTest]
			ColorMask RGBA
			Offset [_OffsetFactor], [_OffsetUnits]
			BlendOp [_AddBlendOp], [_AddBlendOpAlpha]
			Blend [_AddSrcBlend] [_AddDstBlend], [_AddSrcBlendAlpha] [_AddDstBlendAlpha]
			CGPROGRAM
 #define FINALPASS 
 #define POI_EMISSION_1 
 #define POI_STYLIZED_StylizedSpecular 
 #define _POI_DEPTH_RIMLIGHT 
 #define _STOCHASTICMODE_DELIOT_HEITZ 
 #define _STYLIZEDREFLECTIONMODE_UNITYCHAN 
 #define _SUNDISK_SIMPLE 
 #define PROP_DETAILTEX 
 #define PROP_DETAILNORMALMAP 
 #define OPTIMIZER_ENABLED 
			#pragma target 5.0
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3 _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH _DECAL_LAYERS
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION _USE_FAST_SRGB_LINEAR_CONVERSION _LIGHT_LAYERS
			#if !defined(POI_WORLD)
			#pragma skip_variants _ADDITIONAL_LIGHTS_VERTEX LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_instancing
			#pragma dynamic_branch _ FOG_LINEAR
			#pragma dynamic_branch _ FOG_EXP
			#pragma dynamic_branch _ FOG_EXP2
			#define POI_PASS_ADD
			uint _UdonForceSceneLighting;
			#if defined(PROP_LIGHTINGAOMAPS)
			Texture2D _LightingAOMaps;
			#endif
			#if defined(PROP_LIGHTINGDETAILSHADOWMAPS)
			Texture2D _LightingDetailShadowMaps;
			#endif
			#if defined(PROP_LIGHTINGSHADOWMASKS)
			Texture2D _LightingShadowMasks;
			#endif
			#if defined(PROP_LIGHTDATASDFMAP)
			Texture2D _LightDataSDFMap;
			#endif
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define STEREO_UV(uv) float3(uv, unity_StereoEyeIndex)
			#else
			#define STEREO_UV(uv) uv
			#endif
			#if POI_PIPE == POI_BIRP
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			Texture2DArray<float> _CameraDepthTexture;
			#else
			Texture2D<float> _CameraDepthTexture;
			#endif
			#endif
			#if POI_PIPE == POI_URP
			TEXTURE2D_X_FLOAT(_CameraDepthTexture);
			float4 _CameraDepthTexture_TexelSize;
			#endif
			float SampleScreenDepth(float2 uv)
			{
				uv.y = _ProjectionParams.x * 0.5 + 0.5 - uv.y * _ProjectionParams.x;
				return _CameraDepthTexture.SampleLevel(sampler_point_clamp, STEREO_UV(uv), 0).r;
			}
			bool DepthTextureExists()
			{
				float3 dTexDim = 0;
				#if POI_PIPE == POI_BIRP
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y, dTexDim.z);
				#else
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y);
				#endif
				#endif
				#if POI_PIPE == POI_URP
				dTexDim.xy = _CameraDepthTexture_TexelSize.zw;
				#endif
				return dTexDim.x > 16;
			}
			UNITY_DECLARE_TEX2D(_MainTex);
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			#endif
			#ifdef FINALPASS
			#if defined(PROP_DETAILMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailMask;
			#endif
			#if defined(PROP_DETAILNORMALMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailNormalMap;
			#endif
			#if defined(PROP_DETAILTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _DetailTex;
			#endif
			#endif
			struct MatcapAudioLinkData
			{
				float matcapALEnabled;
				float matcapALAlphaAddBand;
				float4 matcapALAlphaAdd;
				float matcapALEmissionAddBand;
				float4 matcapALEmissionAdd;
				float matcapALIntensityAddBand;
				float4 matcapALIntensityAdd;
				float matcapALChronoPanType;
				float matcapALChronoPanBand;
				float matcapALChronoPanSpeed;
			};
			#ifdef _POI_DEPTH_RIMLIGHT
			static float2 depthRimSampleOffsets4[4] = {
				float2(0, 1), float2(0, -1), float2(-1, 0), float2(1, 0)
			};
			static float2 depthRimSampleOffsets8[8] = {
				float2(-1, 1), float2(0, 1), float2(1, 1),
				float2(-1, 0),               float2(1, 0),
				float2(-1, -1), float2(0, -1), float2(1, -1)
			};
			#endif
			#ifdef _SUNDISK_SIMPLE
			#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterMask;
			#endif
			#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterColorMap;
			#endif
			#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterTexture;
			#endif
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			#if defined(PROP_HIGHCOLOR_TEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _HighColor_Tex;
			#endif
			#if defined(PROP_SET_HIGHCOLORMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _Set_HighColorMask;
			#endif
			#endif
			#endif
			float _GrabMode;
			float _Mode;
			#ifndef _STOCHASTICMODE_NONE
			#ifdef _STOCHASTICMODE_DELIOT_HEITZ
			float _StochasticDeliotHeitzDensity;
			#endif
			#endif
			float4 _LightingAOMaps_ST;
			float2 _LightingAOMapsPan;
			float _LightingAOMapsUV;
			float _LightDataAOStrengthR;
			float _LightDataAOStrengthG;
			float _LightDataAOStrengthB;
			float _LightDataAOStrengthA;
			float _LightDataAOGlobalMaskR;
			float _LightDataAOGlobalMaskBlendTypeR;
			float4 _LightingDetailShadowMaps_ST;
			float2 _LightingDetailShadowMapsPan;
			float _LightingDetailShadowMapsUV;
			float _LightingDetailShadowStrengthR;
			float _LightingDetailShadowStrengthG;
			float _LightingDetailShadowStrengthB;
			float _LightingDetailShadowStrengthA;
			float _LightingAddDetailShadowStrengthR;
			float _LightingAddDetailShadowStrengthG;
			float _LightingAddDetailShadowStrengthB;
			float _LightingAddDetailShadowStrengthA;
			float _LightDataDetailShadowGlobalMaskR;
			float _LightDataDetailShadowGlobalMaskBlendTypeR;
			float4 _LightingShadowMasks_ST;
			float2 _LightingShadowMasksPan;
			float _LightingShadowMasksUV;
			float _LightingShadowMaskStrengthR;
			float _LightingShadowMaskStrengthG;
			float _LightingShadowMaskStrengthB;
			float _LightingShadowMaskStrengthA;
			float _LightDataShadowMaskGlobalMaskR;
			float _LightDataShadowMaskGlobalMaskBlendTypeR;
			float _Unlit_Intensity;
			float _LightingColorMode;
			float _LightingMapMode;
			#if defined(PROP_LIGHTDATASDFMAP)
			float4 _LightDataSDFMap_ST;
			float2 _LightDataSDFMapPan;
			float _LightDataSDFMapUV;
			float _LightDataSDFMapLOD;
			float _LightDataSDFBlendY;
			#endif
			float _LightingDirectionMode;
			float3 _LightngForcedDirection;
			float _LightingViewDirOffsetPitch;
			float _LightingViewDirOffsetYaw;
			float _LightingIndirectUsesNormals;
			float _LightingCapEnabled;
			float _LightingCap;
			float _LightingForceColorEnabled;
			float3 _LightingForcedColor;
			float _LightingForcedColorThemeIndex;
			float _LightingCastedShadows;
			float _LightingMonochromatic;
			float _LightingMinLightBrightness;
			float _LightingAdditiveEnable;
			float _LightingAdditiveLimited;
			float _LightingAdditiveLimit;
			float _LightingAdditiveCastedShadows;
			float _LightingAdditiveMonochromatic;
			float _LightingAdditivePassthrough;
			float _DisableDirectionalInAdd;
			float _LightingVertexLightingEnabled;
			float _LightingMirrorVertexLightingEnabled;
			float _LightingEnableLightVolumes;
			float _LightingLightVolumesNormalBias;
			float _LightDataDebugEnabled;
			float _LightingDebugVisualize;
			float4 _Color;
			float _ColorThemeIndex;
			float _MainPixelMode;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float2 _MainTexPan;
			float _MainTexUV;
			float _MainTexStochastic;
			float _MainIgnoreTexAlpha;
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _MainAlphaMaskMode;
			float _AlphaMaskR;
			float _AlphaMaskG;
			float _AlphaMaskB;
			float _AlphaMaskA;
			float _AlphaMaskChannelBlendMode;
			float2 _AlphaMaskMinMax;
			float _AlphaMaskGamma;
			float _AlphaMaskBlendStrength;
			float _AlphaMaskValue;
			float _Cutoff;
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			float4 _GlobalThemeColor0;
			float4 _GlobalThemeColor1;
			float4 _GlobalThemeColor2;
			float4 _GlobalThemeColor3;
			float _GlobalThemeHueShiftColorSpace0;
			float _GlobalThemeHueShiftColorSpace1;
			float _GlobalThemeHueShiftColorSpace2;
			float _GlobalThemeHueShiftColorSpace3;
			float _GlobalThemeHue0;
			float _GlobalThemeHue1;
			float _GlobalThemeHue2;
			float _GlobalThemeHue3;
			float _GlobalThemeHueSpeed0;
			float _GlobalThemeHueSpeed1;
			float _GlobalThemeHueSpeed2;
			float _GlobalThemeHueSpeed3;
			float _GlobalThemeSaturationMethod0;
			float _GlobalThemeSaturationMethod1;
			float _GlobalThemeSaturationMethod2;
			float _GlobalThemeSaturationMethod3;
			float _GlobalThemeSaturation0;
			float _GlobalThemeSaturation1;
			float _GlobalThemeSaturation2;
			float _GlobalThemeSaturation3;
			float _GlobalThemeValue0;
			float _GlobalThemeValue1;
			float _GlobalThemeValue2;
			float _GlobalThemeValue3;
			float _GlobalThemeBrightness0;
			float _GlobalThemeBrightness1;
			float _GlobalThemeBrightness2;
			float _GlobalThemeBrightness3;
			int _GlobalMaskVertexColorLinearSpace;
			float2 _UVSettingsTiling0;
			float2 _UVSettingsOffset0;
			float _UVSettingsAngle0;
			float2 _UVSettingsPan0;
			float _UVSettingsRotate0;
			float2 _UVSettingsTiling1;
			float2 _UVSettingsOffset1;
			float _UVSettingsAngle1;
			float2 _UVSettingsPan1;
			float _UVSettingsRotate1;
			float2 _UVSettingsTiling2;
			float2 _UVSettingsOffset2;
			float _UVSettingsAngle2;
			float2 _UVSettingsPan2;
			float _UVSettingsRotate2;
			float2 _UVSettingsTiling3;
			float2 _UVSettingsOffset3;
			float _UVSettingsAngle3;
			float2 _UVSettingsPan3;
			float _UVSettingsRotate3;
			float _UVSettingsShiftBackfaceUV;
			float _StereoEnabled;
			float _PolarUV;
			float _PolarMode;
			float2 _PolarCenter;
			float _PolarRadialScale;
			float _PolarLengthScale;
			float _PolarSpiralPower;
			float _PanoUseBothEyes;
			float _UVModWorldPos0;
			float _UVModWorldPos1;
			float _UVModLocalPos0;
			float _UVModLocalPos1;
			#ifdef FINALPASS
			float4 _DetailMask_ST;
			float2 _DetailMaskPan;
			float _DetailMaskUV;
			float _DetailMaskStochastic;
			float4 _DetailNormalMap_ST;
			float2 _DetailNormalMapPan;
			float _DetailNormalMapUV;
			float _DetailNormalMapScale;
			float _DetailNormalMapStochastic;
			float _DetailNormalGlobalMask;
			float _DetailNormalGlobalMaskBlendType;
			float4 _DetailTex_ST;
			float2 _DetailTexPan;
			float _DetailTexUV;
			float _DetailTexStochastic;
			float3 _DetailTint;
			float _DetailTintThemeIndex;
			float _DetailTexIntensity;
			float _DetailBrightness;
			float _DetailTexGlobalMask;
			float _DetailTexGlobalMaskBlendType;
			#endif
			#ifdef _POI_DEPTH_RIMLIGHT
			float _DepthRimNormalToUse;
			float _DepthRimWidth;
			float _DepthRimSharpness;
			float _DepthRimDepthThreshold;
			float _DepthRimBinary;
			float _DepthRimHideInShadow;
			float _DepthRimLightDirMethod;
			float _DepthRimShadowMask;
			float _DepthRimMixRampedLightMap;
			float _DepthRimCameraClip;
			float _DepthRimMinDistance;
			float _DepthRimMaxDistance;
			float4 _DepthRimColor;
			float _DepthRimColorThemeIndex;
			float _DepthRimMixBaseColor;
			float _DepthRimEmission;
			float _DepthRimReplace;
			float _DepthRimAdd;
			float _DepthRimScreen;
			float _DepthRimMultiply;
			float _DepthRimAdditiveLighting;
			float _DepthRimMixLightColor;
			float _DepthRimType;
			float _DepthRimBrightness;
			Texture2D _DepthRimMask;
			float4 _DepthRimMask_ST;
			float2 _DepthRimMaskPan;
			float _DepthRimMaskUV;
			float _DepthRimMaskChannel;
			#endif
			#ifdef _SUNDISK_SIMPLE
			float4 _GlitterRandomRotationSpeed;
			float _GlitterLayers;
			float _GlitterUseNormals;
			float _GlitterUV;
			float4 _GlitterColor;
			float _GlitterColorThemeIndex;
			half _GlitterSpeed;
			half _GlitterBrightness;
			float _GlitterFrequency;
			float _GlitterRandomLocation;
			half _GlitterSize;
			half _GlitterContrast;
			half _GlitterAngleRange;
			half _GlitterMinBrightness;
			half _GlitterBias;
			float _GlitterUseSurfaceColor;
			float _GlitterBlendType;
			float _GlitterMode;
			float _GlitterShape;
			float _GlitterLinearMinBrightness;
			float _GlitterLinearBrightness;
			float _GlitterLinearSpeed;
			float _GlitterLinearSensitivity;
			float _GlitterLinearContrast;
			float _GlitterLinearPostContrast;
			float _GlitterLinearAngle;
			float _GlitterLinearLightDirection;
			float _GlitterLinearVRParallax;
			float _GlitterLinearDistanceLOD;
			float _GlitterJaggyFix;
			float _GlitterDistanceLOD;
			float _GlitterTextureRotation;
			float2 _GlitterUVPanning;
			float _GlitterHueShiftEnabled;
			float _GlitterHueShiftColorSpace;
			float _GlitterHueShiftSpeed;
			float _GlitterHueShift;
			float _GlitterHueSelectOrShift;
			float _GlitterHueShiftRandomOffset;
			float _GlitterHideInShadow;
			float _GlitterScaleWithLighting;
			float _GlitterRandomColors;
			float2 _GlitterMinMaxSaturation;
			float2 _GlitterMinMaxBrightness;
			float _GlitterRandomSize;
			float4 _GlitterMinMaxSize;
			float _GlitterFullCoverage;
			float _GlitterRandomRotation;
			float4 _GlitterMask_ST;
			float2 _GlitterMaskPan;
			float _GlitterMaskUV;
			float _GlitterMaskChannel;
			float _GlitterMaskInvert;
			float _GlitterMaskGlobalMask;
			float _GlitterMaskGlobalMaskBlendType;
			float4 _GlitterColorMap_ST;
			float2 _GlitterColorMapPan;
			float _GlitterColorMapUV;
			float4 _GlitterTexture_ST;
			float4 _GlitterTexture_TexelSize;
			float _GlitterTexturePixelMode;
			float _GlitterALEnabled;
			float _GlitterALAlphaAddBand;
			float4 _GlitterALAlphaAdd;
			float _GlitterALMaxBrightnessBand;
			float4 _GlitterALMaxBrightnessAdd;
			float _GlitterALSizeAddBand;
			float4 _GlitterALSizeAdd;
			float _GlitterALChronoSparkleSpeedType;
			float _GlitterALChronoSparkleSpeedBand;
			float _GlitterALChronoSparkleSpeed;
			float _GlitterALChronoRotationSpeedType;
			float _GlitterALChronoRotationSpeedBand;
			float _GlitterALChronoRotationSpeed;
			float _GlitterALColorOverrideEnabled;
			float4 _GlitterALColorOverride;
			float _GlitterALColorOverrideThemeIndex;
			float _GlitterALColorOverrideBand;
			float4 _GlitterALColorOverrideRamp;
			float _GlitterSpriteSheetEnabled;
			float _GlitterSpriteSheetColumns;
			float _GlitterSpriteSheetRows;
			float _GlitterSpriteSheetFrameSelection;
			float _GlitterSpriteSheetFPS;
			float _GlitterSpriteSheetRandomStart;
			float _GlitterSpriteSheetTotalFrames;
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			float _StylizedReflectionMode;
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			float4 _HighColor_Tex_ST;
			float2 _HighColor_TexPan;
			float _HighColor_TexUV;
			float4 _Set_HighColorMask_ST;
			float2 _Set_HighColorMaskPan;
			float _Set_HighColorMaskUV;
			float _Set_HighColorMaskChannel;
			float _Tweak_HighColorMaskLevel;
			float _StylizedSpecularInvertMask;
			float _StylizedSpecularNormalStrength;
			float4 _HighColor;
			float _UseLightColor;
			float _HighColor_Power;
			float _StylizedSpecularFeather;
			float _Layer1Strength;
			float _Layer2Size;
			float _StylizedSpecular2Feather;
			float _Layer2Strength;
			float _StylizedSpecularStrength;
			float _HighColorThemeIndex;
			float _Is_BlendAddToHiColor;
			float _Is_SpecularToHighColor;
			float _StylizedSpecularIgnoreNormal;
			float _StylizedSpecularIgnoreShadow;
			float _SSIgnoreCastedShadows;
			float _UseSpecularOptMap2;
			#endif
			#endif
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				#ifdef POI_PASS_MOTION_VECTORS
				float3 positionOld : TEXCOORD4;
				#if _ADD_PRECOMPUTED_VELOCITY
				float3 alembicMotionVector : TEXCOORD5;
				#endif
				#endif
				#ifdef POI_TESSELLATED
				uint vertexId : TEXCOORD6;
				#else
				uint vertexId : SV_VertexID;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float worldDir : TEXCOORD8;
				float fogData: TEXCOORD10;
				POI_SHADOW_COORDS(9)
				#ifdef POI_SQUISH
				float squishAmount : TEXCOORD11;
				#endif
				#ifdef POIBS_BLOOMFOG
				float2 bsFogScreenUV : TEXCOORD12;
				#endif
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD13;
				float4 LightCoord : TEXCOORD14;
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				float4 positionCSNoJitter         : POSITION_CS_NO_JITTER;
				float4 previousPositionCSNoJitter : PREV_POSITION_CS_NO_JITTER;
				#endif
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(POI_DEVICE_QUEST)
				uint gl_Layer : SV_RenderTargetArrayIndex;
				#endif
			};
			float2 vertexUV(in VertexOut o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in appdata v, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return v.uv0.xy * w0 + v.uv1.xy * w1 + v.uv2.xy * w2 + v.uv3.xy * w3;
			}
			#ifdef POI_PASS_LILFUR
			float2 vertexUV(in PoiGeoToFragLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in PoiVertToGeoLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			#endif
			#ifdef POI_AUDIOLINK
			inline int poiALBandPass(int bandIdx)
			{
				bandIdx = clamp(bandIdx, 0, 3);
				return bandIdx == 0 ? ALPASS_AUDIOBASS : bandIdx == 1 ? ALPASS_AUDIOLOWMIDS : bandIdx == 2 ? ALPASS_AUDIOHIGHMIDS : ALPASS_AUDIOTREBLE;
			}
			#endif
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				float3 outputColor = srcColor;
				if (themeIndex != 0)
				{
					themeIndex = max(themeIndex - 1, 0);
					if (themeIndex <= 3)
					{
						outputColor = poiMods.globalColorTheme[themeIndex].rgb;
					}
					else
					{
						#ifdef POI_AUDIOLINK
						if (poiMods.audioLinkAvailable)
						{
							outputColor = poiMods.globalColorTheme[themeIndex].rgb;
						}
						#endif
					}
				}
				return outputColor;
			}
			struct PoiSpriteSheetConfig
			{
				float columns;
				float rows;
				float totalFrames;
				float frameSelection;
				float fps;
				float randomStart;
				float inset;
				float2 textureST;
			};
			float poiSpriteSheetFrameIndex(PoiSpriteSheetConfig config, float progress, float seed)
			{
				float totalFrames = (config.totalFrames > 0) ? config.totalFrames : (config.columns * config.rows);
				float frameIndex = 0;
				if (config.frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (config.frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (config.frameSelection == 2)
				{
					float timeOffset = config.randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * config.fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float poiSpriteSheetFrameIndex(float columns, float rows, float configTotalFrames, float frameSelection, float fps, float randomStart, float progress, float seed)
			{
				float totalFrames = (configTotalFrames > 0) ? configTotalFrames : (columns * rows);
				float frameIndex = 0;
				if (frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (frameSelection == 2)
				{
					float timeOffset = randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float2 poiSpriteSheetUV(float2 uv, float columns, float rows, float frameIndex, float inset, float2 textureST)
			{
				float invCols = 1.0 / columns;
				float invRows = 1.0 / rows;
				float2 frameSize = float2(invCols, invRows);
				float column = fmod(frameIndex, columns);
				float row = floor(frameIndex * invCols);
				row = rows - 1 - row;
				float2 lookupUV = (uv * frameSize) + float2(column, row) * frameSize;
				if (inset > 0)
				{
					float2 texelInset = inset * frameSize / max(textureST, 1e-6);
					lookupUV = lerp(lookupUV, float2(column + 1, row + 1) * frameSize, texelInset);
				}
				return lookupUV;
			}
			float2 poiSpriteSheetUV(float2 uv, PoiSpriteSheetConfig config, float frameIndex)
			{
				return poiSpriteSheetUV(uv, config.columns, config.rows, frameIndex, config.inset, config.textureST);
			}
			void applyReducedRenderClipDistance(inout VertexOut o)
			{
				if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
				{
					#if defined(UNITY_REVERSED_Z) // DirectX
					o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
					#else // OpenGL
					o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
					#endif
				}
			}
			VertexOut vert(appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				POI_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				#ifdef POI_PASS_OUTLINE
				if (0.0 > 0.5)
				{
					return (VertexOut)POI_NAN;
				}
				#endif
				#ifdef POI_AUDIOLINK
				float vertexAudioLink[5];
				vertexAudioLink[0] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 0))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 0))[0];
				vertexAudioLink[1] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 1))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 1))[0];
				vertexAudioLink[2] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 2))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 2))[0];
				vertexAudioLink[3] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 3))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 3))[0];
				vertexAudioLink[4] = AudioLinkData(ALPASS_GENERALVU + float2(8, 0))[0];
				#endif
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				#if POI_PIPE == POI_BIRP
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				#elif  POI_PIPE == POI_URP
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent);
				o.normal = vertexNormalInput.normalWS;
				o.tangent.xyz = vertexNormalInput.tangentWS;
				#endif
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				#if defined(LIGHTMAP_ON)
				OUTPUT_LIGHTMAP_UV(v.uv1.xy, unity_LightmapST, o.lightmapUV.xy);
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				OUTPUT_LIGHTMAP_UV(v.uv2.xy, unity_LightmapST, o.lightmapUV.zw);
				#endif
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				o.localPos.xyz += localOffset;
				o.worldPos.xyz += worldOffset;
				o.pos = PoiObjectToClipPos(o.localPos.xyz);
				o.fogData = o.pos.z;
				#if !defined(POI_PASS_SHADOW)
				POI_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				POI_TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				o.worldDir = dot(o.pos, CalculateFrustumCorrection());
				if (0.0)
				{
					applyReducedRenderClipDistance(o);
				}
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#ifdef EDITOR_VISUALIZATION
				UnityEditorVizData(v.vertex.xyz, o.uv[0].xy, o.uv[0].zw, o.uv[1].xy, o.VizUV, o.LightCoord);
				#endif
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				#if defined(APPLICATION_SPACE_WARP_MOTION)
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				o.pos = o.positionCSNoJitter;
				#else
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				#endif
				float4 prevPos = (unity_MotionVectorsParams.x == 1) ? float4(v.positionOld, 1) : o.localPos;
				#if _ADD_PRECOMPUTED_VELOCITY
				prevPos = prevPos - float4(v.alembicMotionVector, 0);
				#endif
				o.previousPositionCSNoJitter = mul(_PrevViewProjMatrix, mul(UNITY_PREV_MATRIX_M, prevPos));
				#endif
				#endif
				#ifdef POI_PASS_LILFUR
				#endif
				return o;
			}
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D(float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1, 311.7)), dot(s, float2(269.5, 183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * 1.0);
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				float3x3 pos = float3x3(
				float3(vxID, bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				return (bary.z > 0) ? pos : neg;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				return mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			float4 PoiLightDataSetupAdd(
			inout PoiLight poiLight,
			inout PoiFragData poiFragData,
			in PoiMesh poiMesh,
			in PoiCam poiCam,
			in PoiMods poiMods,
			#ifdef POI_PASS_LILFUR
			in PoiGeoToFragLilFur i,
			#else
			in VertexOut i,
			#endif
			in float4 mainTexture)
			{
				if (!1.0)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#if defined(DIRECTIONAL)
				if (1.0)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#endif
				#if POI_PIPE == POI_BIRP
				poiLight.direction = normalize(_WorldSpaceLightPos0.xyz - poiMesh.worldPos.xyz * _WorldSpaceLightPos0.w);
				#elif POI_PIPE == POI_URP
				poiLight.direction = poiLight.unityLight.direction.xyz;
				#endif
				#if defined(POINT) || defined(SPOT)
				#ifdef POINT
				unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1)).xyz;
				poiLight.attenuation = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r;
				#endif
				#ifdef SPOT
				unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1));
				poiLight.attenuation = (lightCoord.z > 0) * UnitySpotCookie(lightCoord) * UnitySpotAttenuate(lightCoord.xyz);
				#endif
				#else
				#if POI_PIPE == POI_BIRP
				#ifndef POI_PASS_LILFUR
				UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
				poiLight.attenuation = attenuation;
				#else
				poiLight.attenuation = 1;
				#endif
				#elif POI_PIPE == POI_URP
				poiLight.attenuation = poiLight.unityLight.distanceAttenuation;
				#endif
				#endif
				#if POI_PIPE == POI_BIRP
				#if !defined(POI_PASS_LILFUR)
				poiLight.additiveShadow = UNITY_SHADOW_ATTENUATION(i, poiMesh.worldPos);
				#else
				poiLight.additiveShadow = 1;
				#endif
				#elif POI_PIPE == POI_URP
				poiLight.additiveShadow = poiLight.unityLight.shadowAttenuation;
				#endif
				poiLight.attenuationStrength = 1.0;
				poiLight.directColor = 1.0 ? MaxLuminance(poiLight.unityLight.color * poiLight.attenuation, 1.0) : poiLight.unityLight.color * poiLight.attenuation;
				#if defined(POINT_COOKIE) || defined(DIRECTIONAL_COOKIE)
				poiLight.indirectColor = 0;
				#else
				poiLight.indirectColor = lerp(0, poiLight.directColor, 0.5);
				poiLight.indirectColor = 1.0 ? MaxLuminance(poiLight.indirectColor, 1.0) : poiLight.indirectColor;
				#endif
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), 1.0);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), 1.0);
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = dot(poiLight.direction, poiLight.halfDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				[flatten]
				switch(2.0)
				{
					case 0:
					case 1:
					case 2:
					{
						poiLight.lightMap = poiLight.nDotLNormalized;
						break;
					}
					default:
					{
						poiLight.lightMap = 1;
						break;
					}
				}
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.lightMapNoAttenuation = poiLight.lightMap;
				poiLight.lightMap *= lerp(1, poiLight.additiveShadow, poiLight.attenuationStrength);
				float4 debugColor = 0;
				return debugColor;
			}
			float4 PoiLightDataSetup(
			inout PoiLight poiLight,
			inout PoiFragData poiFragData,
			in PoiMesh poiMesh,
			in PoiCam poiCam,
			in PoiMods poiMods,
			#ifdef POI_PASS_LILFUR
			in PoiGeoToFragLilFur i,
			#else
			in VertexOut i,
			#endif
			in float4 mainTexture)
			{
				bool lightExists = false;
				if (any(poiLight.unityLight.color.rgb >= 0.002))
				{
					lightExists = true;
				}
				if (1.0)
				{
					poiFragData.toggleVertexLights = 1;
				}
				if (IsInMirror() && 1.0 == 0)
				{
					poiFragData.toggleVertexLights = 0;
				}
				#if POI_PIPE == POI_BIRP
				if (1.0)
				{
					#if defined(VERTEXLIGHT_ON)
					float4 toLightX = unity_4LightPosX0 - poiMesh.worldPos.x;
					float4 toLightY = unity_4LightPosY0 - poiMesh.worldPos.y;
					float4 toLightZ = unity_4LightPosZ0 - poiMesh.worldPos.z;
					float4 lengthSq = 0;
					lengthSq += toLightX * toLightX;
					lengthSq += toLightY * toLightY;
					lengthSq += toLightZ * toLightZ;
					float4 lightAttenSq = unity_4LightAtten0;
					float4 atten = 1.0 / (1.0 + lengthSq * lightAttenSq);
					float4 vLightWeight = saturate(1 - (lengthSq * lightAttenSq / 25));
					poiLight.vAttenuation = min(atten, vLightWeight * vLightWeight);
					poiLight.vDotNL = 0;
					poiLight.vDotNL += toLightX * poiMesh.normals[1].x;
					poiLight.vDotNL += toLightY * poiMesh.normals[1].y;
					poiLight.vDotNL += toLightZ * poiMesh.normals[1].z;
					float4 corr = rsqrt(lengthSq);
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					poiLight.vertexVDotNL = 0;
					poiLight.vertexVDotNL += toLightX * poiMesh.normals[0].x;
					poiLight.vertexVDotNL += toLightY * poiMesh.normals[0].y;
					poiLight.vertexVDotNL += toLightZ * poiMesh.normals[0].z;
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					poiLight.vSaturatedDotNL = saturate(poiLight.vDotNL);
					[unroll]
					for (int index = 0; index < 4; index++)
					{
						poiLight.vPosition[index] = float3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]);
						float3 vertexToLightSource = poiLight.vPosition[index] - poiMesh.worldPos;
						poiLight.vDirection[index] = normalize(vertexToLightSource);
						poiLight.vColor[index] = 1.0 ? MaxLuminance(unity_LightColor[index].rgb * poiLight.vAttenuation[index], 1.0) : unity_LightColor[index].rgb * poiLight.vAttenuation[index];
						poiLight.vColor[index] = lerp(poiLight.vColor[index], dot(poiLight.vColor[index], float3(0.299, 0.587, 0.114)), 1.0);
						poiLight.vHalfDir[index] = Unity_SafeNormalize(poiLight.vDirection[index] + poiCam.viewDir);
						poiLight.vDotNL[index] = dot(poiMesh.normals[1], poiLight.vDirection[index]);
						poiLight.vCorrectedDotNL[index] = .5 * (poiLight.vDotNL[index] + 1);
						poiLight.vDotLH[index] = saturate(dot(poiLight.vDirection[index], poiLight.vHalfDir[index]));
						poiLight.vDotNH[index] = dot(poiMesh.normals[1], poiLight.vHalfDir[index]);
						poiLight.vertexVDotNH[index] = saturate(dot(poiMesh.normals[0], poiLight.vHalfDir[index]));
					}
					#endif
				}
				#endif
				if (2.0 == 0) // Poi Custom Light Color
				{
					float3 magic = max(BetterSH9(normalize(PoiSHAr + PoiSHAg + PoiSHAb)), 0);
					float3 normalLight = poiLight.unityLight.color.rgb + BetterSH9(float4(0, 0, 0, 1));
					float magiLumi = CalculateLuminance(magic);
					float normaLumi = CalculateLuminance(normalLight);
					float maginormalumi = magiLumi + normaLumi;
					float magiratio = magiLumi / maginormalumi;
					float normaRatio = normaLumi / maginormalumi;
					float target = CalculateLuminance(magic * magiratio + normalLight * normaRatio);
					float3 properLightColor = magic + normalLight;
					float properLuminance = CalculateLuminance(magic + normalLight);
					poiLight.directColor = properLightColor * max(0.0001, (target / properLuminance));
					{
						float3 indirectSampleNormal = lerp(0, poiMesh.normals[1], 0.0);
						#ifdef POI_BENTNORMALMAP
						if (poiMesh.hasBentNormal > 0.5 && 1.0 > 0.5)
						{
							indirectSampleNormal = lerp(indirectSampleNormal, poiMesh.bentNormal, 0.0);
						}
						#endif
						poiLight.indirectColor = BetterSH9(float4(indirectSampleNormal, 1));
					}
				}
				if (2.0 == 1) // More standard approach to light color
				{
					float3 indirectNormal = poiMesh.normals[1];
					#ifdef POI_BENTNORMALMAP
					if (poiMesh.hasBentNormal > 0.5 && 1.0 > 0.5)
					{
						indirectNormal = poiMesh.bentNormal;
					}
					#endif
					float3 indirectColor = BetterSH9(float4(indirectNormal, 1));
					if (lightExists)
					{
						poiLight.directColor = poiLight.unityLight.color.rgb;
						poiLight.indirectColor = indirectColor;
					}
					else
					{
						poiLight.directColor = indirectColor * 0.6;
						poiLight.indirectColor = indirectColor * 0.5;
					}
				}
				if (2.0 == 2) // UTS style
				{
					poiLight.indirectColor = saturate(max(half3(0.05, 0.05, 0.05) * 1.0, max(PoiShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), PoiShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb) * 1.0));
					poiLight.directColor = max(poiLight.indirectColor, poiLight.unityLight.color.rgb);
				}
				if (2.0 == 3) // OpenLit
				{
					float3 lightDirectionForSH9 = OpenLitLightingDirectionForSH9(poiLight.unityLight);
					OpenLitShadeSH9ToonDouble(lightDirectionForSH9, poiLight.directColor, poiLight.indirectColor);
					poiLight.directColor += poiLight.unityLight.color.rgb;
				}
				float lightMapMode = 1.0;
				if (3.0 == 0)
				{
					poiLight.direction = CalculateLuminance(poiLight.unityLight.color.rgb) * poiLight.unityLight.direction.xyz + (PoiSHAr.xyz + PoiSHAg.xyz + PoiSHAb.xyz) / 3.0;
				}
				if (3.0 == 1 || 3.0 == 2)
				{
					if (3.0 == 1)
					{
						poiLight.direction = mul((float3x3)unity_ObjectToWorld, float4(0,0,0,1)).xyz;
					}
					if (3.0 == 2)
					{
						poiLight.direction = float4(0,0,0,1);
					}
					if (lightMapMode == 0)
					{
						lightMapMode = 1;
					}
				}
				if (3.0 == 3) // UTS
				{
					float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
					float3 lightDirection = normalize(lerp(defaultLightDirection, poiLight.unityLight.direction.xyz, any(poiLight.unityLight.direction.xyz)));
					poiLight.direction = lightDirection;
				}
				if (3.0 == 4) // OpenLit
				{
					poiLight.direction = OpenLitLightingDirection(poiLight.unityLight); // float4 customDir = 0; // Do we want to give users to alter this (OpenLit always does!)?
				}
				if (3.0 == 5) // View Direction
				{
					float3 upViewDir = normalize(UNITY_MATRIX_V[1].xyz);
					float3 rightViewDir = normalize(UNITY_MATRIX_V[0].xyz);
					float yawOffset_Rads = radians(!IsInMirror() ? - 0.0 : 0.0);
					float3 rotatedViewYaw = normalize(RotateAroundAxis(rightViewDir, upViewDir, yawOffset_Rads));
					float3 rotatedViewCameraMeshOffset = RotateAroundAxis((getCameraPosition() - (poiMesh.worldPos)), upViewDir, yawOffset_Rads);
					float pitchOffset_Rads = radians(!IsInMirror() ? 0.0 : - 0.0);
					float3 rotatedViewPitch = RotateAroundAxis(rotatedViewCameraMeshOffset, rotatedViewYaw, pitchOffset_Rads);
					poiLight.direction = normalize(rotatedViewPitch);
				}
				if (!any(poiLight.direction))
				{
					poiLight.direction = float3(.4, 1, .4);
				}
				poiLight.direction = normalize(poiLight.direction);
				poiLight.attenuationStrength = 1.0;
				poiLight.attenuation = 1;
				if (!all(poiLight.unityLight.color.rgb == 0.0))
				{
					#if POI_PIPE == POI_BIRP
					#ifndef POI_PASS_LILFUR
					UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
					poiLight.attenuation *= attenuation;
					#endif
					#elif POI_PIPE == POI_URP
					poiLight.attenuation *= poiLight.unityLight.shadowAttenuation;
					poiLight.attenuation *= (poiLight.lightType == POI_POINT_SPOT) ? poiLight.unityLight.distanceAttenuation : 1;
					#endif
				}
				#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
				half bakedAtten = UnitySampleBakedOcclusion(poiMesh.lightmapUV.xy, poiMesh.worldPos);
				float zDist = dot(_WorldSpaceCameraPos - poiMesh.worldPos, UNITY_MATRIX_V[2].xyz);
				float fadeDist = UnityComputeShadowFadeDistance(poiMesh.worldPos, zDist);
				poiLight.attenuation = UnityMixRealtimeAndBakedShadows(poiLight.attenuation, bakedAtten, UnityComputeShadowFade(fadeDist));
				#endif
				if (!any(poiLight.directColor) && !any(poiLight.indirectColor) && lightMapMode == 0)
				{
					lightMapMode = 1;
					if (3.0 == 0)
					{
						poiLight.direction = normalize(float3(.4, 1, .4));
					}
				}
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotVCentered = abs(dot(poiMesh.normals[1], normalize(getCameraPosition() - poiMesh.worldPos.xyz)));
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = max(0.00001, dot(poiLight.direction, poiLight.halfDir));
				if (lightMapMode == 0)
				{
					float3 ShadeSH9Plus = GetSHLength();
					float3 ShadeSH9Minus = float3(PoiSHAr.w, PoiSHAg.w, PoiSHAb.w) + float3(PoiSHBr.z, PoiSHBg.z, PoiSHBb.z) / 3.0;
					float3 greyScaleVector = float3(.33333, .33333, .33333);
					float bw_lightColor = dot(poiLight.directColor, greyScaleVector);
					float bw_directLighting = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor * lerp(1, poiLight.attenuation, poiLight.attenuationStrength)) + dot(PoiShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_directLightingNoAtten = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor) + dot(PoiShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_bottomIndirectLighting = dot(ShadeSH9Minus, greyScaleVector);
					float bw_topIndirectLighting = dot(ShadeSH9Plus, greyScaleVector);
					float lightDifference = ((bw_topIndirectLighting + bw_lightColor) - bw_bottomIndirectLighting);
					poiLight.lightMap = smoothstep(0, lightDifference, bw_directLighting - bw_bottomIndirectLighting);
					poiLight.lightMapNoAttenuation = smoothstep(0, lightDifference, bw_directLightingNoAtten - bw_bottomIndirectLighting);
				}
				if (lightMapMode == 1)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 2)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					poiLight.lightMap = poiLight.nDotLSaturated * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 3)
				{
					poiLight.lightMapNoAttenuation = 1;
					poiLight.lightMap = lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 4)
				{
					#if defined(PROP_LIGHTDATASDFMAP)
					float2 lightDataSDFMap = 1;
					if (0.0 > 0)
					{
						float sdfLod = pow(0.0, 4.0);
						lightDataSDFMap = POI2D_SAMPLER_PANGRAD(_LightDataSDFMap, _linear_repeat, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), max(poiMesh.dx, sdfLod), max(poiMesh.dy, sdfLod)).rg;
					}
					else
					{
						lightDataSDFMap = POI2D_SAMPLER_PAN(_LightDataSDFMap, _linear_repeat, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0)).rg;
					}
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					float3 faceR = mul((float3x3)unity_ObjectToWorld, float3(-1.0, 0.0, 0.0));
					float LdotR = dot(poiLight.direction.xz, faceR.xz);
					float sdf = LdotR < 0 ? lightDataSDFMap.g : lightDataSDFMap.r;
					float3 faceF = mul((float3x3)unity_ObjectToWorld, float3(0.0, 0.0, 1.0)).xyz;
					faceF.y *= 1.0;
					faceF = dot(faceF, faceF) == 0 ? 0 : normalize(faceF);
					float3 faceL = poiLight.direction;
					faceL.y *= 1.0;
					faceL = dot(faceL, faceL) == 0 ? 0 : normalize(faceL);
					float lnSDF = dot(faceL, faceF);
					poiLight.lightMapNoAttenuation = saturate(lnSDF * 0.5 + sdf * 0.5 + 0.25);
					poiLight.lightMap = saturate(lnSDF * 0.5 + sdf * 0.5 + 0.25) * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
					#else
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
					#endif
				}
				poiLight.lightMapNoAttenuation *= poiLight.detailShadow;
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.directColor = max(poiLight.directColor, 0.0001);
				poiLight.indirectColor = max(poiLight.indirectColor, 0.0001);
				if (2.0 == 3)
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.lightingMinLightBrightness);
				}
				else
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.lightingMinLightBrightness);
					poiLight.indirectColor = max(poiLight.indirectColor, poiLight.lightingMinLightBrightness);
				}
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), poiLight.lightingMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), poiLight.lightingMonochromatic);
				if (poiLight.lightingCapEnabled)
				{
					poiLight.directColor = min(poiLight.directColor, _LightingCap);
					poiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);
				}
				if (0.0)
				{
					poiLight.directColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				}
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				poiLight.directColor = max(poiLight.directColor * 1.0, 0);
				poiLight.directColor = max(poiLight.directColor + 0.0, 0);
				poiLight.indirectColor = max(poiLight.indirectColor * 1.0, 0);
				poiLight.indirectColor = max(poiLight.indirectColor + 0.0, 0);
				#endif
				float4 debugColor = 0;
				#ifdef POI_PASS_ADD
				debugColor = PoiLightDataSetupAdd(poiLight, poiFragData, poiMesh, poiCam, poiMods, i, mainTexture);
				#endif
				return debugColor;
			}
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + 0.0);
				if (0.0 > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[0.0 - 1], 2.0);
				}
			}
			void calculateGlobalThemes(inout PoiMods poiMods)
			{
				float4 themeColorExposures = 0;
				float4 themeColor0, themeColor1, themeColor2, themeColor3 = 0;
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor0.rgb, themeColorExposures.x);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor1.rgb, themeColorExposures.y);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor2.rgb, themeColorExposures.z);
				DecomposeHDRColor(float4(1,1,1,1).rgb, themeColor3.rgb, themeColorExposures.w);
				themeColor0.rgb = hueShift(themeColor0.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor0.rgb = ModifyViaHSV(themeColor0.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor0.rgb = lerp(themeColor0.rgb, dot(themeColor0.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor0.rgb = saturate(themeColor0.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor0.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[0] = float4(ApplyHDRExposure(themeColor0.rgb, themeColorExposures.x), float4(1,1,1,1).a);
				themeColor1.rgb = hueShift(themeColor1.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor1.rgb = ModifyViaHSV(themeColor1.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor1.rgb = lerp(themeColor1.rgb, dot(themeColor1.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor1.rgb = saturate(themeColor1.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor1.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[1] = float4(ApplyHDRExposure(themeColor1.rgb, themeColorExposures.y), float4(1,1,1,1).a);
				themeColor2.rgb = hueShift(themeColor2.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor2.rgb = ModifyViaHSV(themeColor2.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor2.rgb = lerp(themeColor2.rgb, dot(themeColor2.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor2.rgb = saturate(themeColor2.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor2.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[2] = float4(ApplyHDRExposure(themeColor2.rgb, themeColorExposures.z), float4(1,1,1,1).a);
				themeColor3.rgb = hueShift(themeColor3.rgb, frac(0.0 + 0.0 * POI_TIME.x), 0.0, 1.0);
				[flatten]
				switch((int)1.0)
				{
					case 0:
					{
						themeColor3.rgb = ModifyViaHSV(themeColor3.rgb, 0.0, 0.0, 0.0);
						break;
					}
					case 1:
					{
						themeColor3.rgb = lerp(themeColor3.rgb, dot(themeColor3.rgb, float3(0.3, 0.59, 0.11)), - (0.0));
						themeColor3.rgb = saturate(themeColor3.rgb * (0.0 + 1));
						break;
					}
					default:
					{
						themeColor3.rgb = float3(1.0, 0.0, 0.0);
						break;
					}
				}
				poiMods.globalColorTheme[3] = float4(ApplyHDRExposure(themeColor3.rgb, themeColorExposures.w), float4(1,1,1,1).a);
			}
			void ApplyGlobalMaskModifiers(in PoiMesh poiMesh, inout PoiMods poiMods, in PoiCam poiCam)
			{
			}
			float2 applyUVSettings(float2 uv, float2 tiling, float2 offset, float angle, float2 pan, float rotate)
			{
				uv = uv * tiling + offset;
				float angleRad = angle * (UNITY_PI / 180.0);
				float rotateRad = rotate * (UNITY_PI / 180.0);
				float totalAngle = angleRad + rotateRad * POI_TIME.y;
				if (totalAngle != 0)
				{
					float2 pivot = float2(0.5, 0.5);
					float cosA = cos(totalAngle);
					float sinA = sin(totalAngle);
					uv -= pivot;
					uv = float2(
					uv.x * cosA - uv.y * sinA,
					uv.x * sinA + uv.y * cosA
					);
					uv += pivot;
				}
				uv += pan * POI_TIME.y;
				return uv;
			}
			float2 calculatePolarCoordinate(in PoiMesh poiMesh)
			{
				float2 delta = poiMesh.uv[0.0] - float4(0.5,0.5,0,0);
				float dist = length(delta);
				float radius;
				
				if (0.0 == 0) // Standard
				{
					radius = dist * 2 * 1.0;
				}
				else if (0.0 == 1) // Log Polar - preserves texture scale
				{
					radius = log(dist + 0.001) * 1.0 + 1;
				}
				else // Square Polar
				{
					radius = max(abs(delta.x), abs(delta.y)) * 2 * 1.0;
				}
				float angle = atan2(delta.x, delta.y);
				float phi = angle / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				angle = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				angle *= 1.0;
				return float2(radius, angle + dist * 0.0);
			}
			float2 MonoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(1.0, 1.0 / UNITY_PI);
				return float2(1.0, 1.0) - sphereCoords;
			}
			float2 StereoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(0.5, 1.0 / UNITY_PI);
				sphereCoords = float2(0.5, 1.0) - sphereCoords;
				return (sphereCoords + float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).xy) * float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).zw;
			}
			float2 calculateWorldUV(in PoiMesh poiMesh)
			{
				return float2(0.0 != 3 ? poiMesh.worldPos[ 0.0] : 0.0f, 2.0 != 3 ? poiMesh.worldPos[2.0] : 0.0f);
			}
			float2 calculatelocalUV(in PoiMesh poiMesh)
			{
				float localUVs[8];
				localUVs[0] = poiMesh.localPos.x;
				localUVs[1] = poiMesh.localPos.y;
				localUVs[2] = poiMesh.localPos.z;
				localUVs[3] = 0;
				localUVs[4] = poiMesh.vertexColor.r;
				localUVs[5] = poiMesh.vertexColor.g;
				localUVs[6] = poiMesh.vertexColor.b;
				localUVs[7] = poiMesh.vertexColor.a;
				return float2(localUVs[0.0],localUVs[1.0]);
			}
			float2 calculatePanosphereUV(in PoiMesh poiMesh)
			{
				float3 viewDirection = normalize(lerp(getCameraPosition().xyz, _WorldSpaceCameraPos.xyz, 1.0) - poiMesh.worldPos.xyz) * - 1;
				return lerp(MonoPanoProjection(viewDirection), StereoPanoProjection(viewDirection), 0.0);
			}
			#ifdef FINALPASS
			void ApplyDetailColor(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiMods poiMods)
			{
				#if defined(PROP_DETAILTEX) || !defined(OPTIMIZER_ENABLED)
				half3 detailTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_DetailTex, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0).rgb * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				#else
				half3 detailTexture = 0.21763764082 * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				#endif
				poiFragData.baseColor.rgb *= LerpWhiteTo(detailTexture * 0.29 * unity_ColorSpaceDouble.rgb, poiMods.detailMask.r * 0.14);
			}
			void ApplyDetailNormal(inout PoiMods poiMods, inout PoiMesh poiMesh)
			{
				#if defined(PROP_DETAILMASK) || !defined(OPTIMIZER_ENABLED)
				poiMods.detailMask = POI2D_SAMPLER_PAN_STOCHASTIC(_DetailMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0).rg;
				#else
				poiMods.detailMask = 1;
				#endif
				#ifdef POI_BACKFACE
				if (!poiMesh.isFrontFace)
				{
					poiMods.detailMask.rg *= 1.0;
				}
				#endif
				if (0.0 > 0)
				{
					poiMods.detailMask.r = maskBlend(poiMods.detailMask.r, poiMods.globalMask[0.0 - 1], 2.0);
				}
				if (0.0 > 0)
				{
					poiMods.detailMask.g = maskBlend(poiMods.detailMask.g, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#if defined(PROP_DETAILNORMALMAP) || !defined(OPTIMIZER_ENABLED)
				half3 detailNormal = PoiUnpackNormalScale(POI2D_SAMPLER_PAN_STOCHASTIC(_DetailNormalMap, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), 0.0), 10.0 * poiMods.detailMask.g);
				poiMesh.tangentSpaceNormal = PoiBlendNormal(detailNormal, poiMesh.tangentSpaceNormal);
				#endif
			}
			#endif
			#ifdef _POI_DEPTH_RIMLIGHT
			void ApplyDepthRimLighting(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, inout PoiLight poiLight, in PoiMods poiMods)
			{
				if (!DepthTextureExists()) return;
				float perspectiveDivide = 1.0f / poiCam.clipPos.w;
				float2 screenPos = poiCam.posScreenSpace.xy * perspectiveDivide;
				float z = SampleScreenDepth(screenPos);
				#if UNITY_REVERSED_Z
				if (z == 0) return;
				#else
				if (z == 1) return;
				#endif
				float4 direction = poiCam.worldDirection * perspectiveDivide;
				float linearDepth = CorrectedLinearEyeDepth(z, direction.w);
				float distanceFade = 1;
				if (0.0 > 0)
				{
					distanceFade = smoothstep(0.0, 0.0 + 0.001, poiCam.distanceToVert);
					if (distanceFade <= 0) return;
				}
				float cameraClipScale = 0.5 > 0 ? saturate(poiCam.distanceToVert / 0.5) : 1;
				float scale = WorldToScreenScale(0.1 * 0.1, poiCam.distanceToVert) * cameraClipScale;
				float2 aspectCorrect = GetScreenAspectCorrection();
				float3 viewNorm = normalize(mul((float3x3)UNITY_MATRIX_V, poiMesh.normals[0.0]));
				float normalFacing = 1.0 ? 1 : saturate(1 - abs(viewNorm.z));
				float2 viewNormXY = viewNorm.xy;
				float2 N_View = length(viewNormXY) > 1e-6 ? normalize(viewNormXY) : float2(0, 1);
				float3 viewLight = normalize(mul((float3x3)UNITY_MATRIX_V, poiLight.direction));
				float2 viewLightXY = viewLight.xy;
				float2 L_View = length(viewLightXY) > 1e-6 ? normalize(viewLightXY) : float2(0, 1);
				float rim = 0;
				float depthDiff = 0;
				float depthThreshold = 1.0 / max(cameraClipScale, 0.001);
				float depthThresholdLow = depthThreshold * 0.5;
				float skyboxDepth = linearDepth + depthThreshold * 2;
				switch (0.0)
				{
					case 0:
					{
						float2 ssUV1 = screenPos + N_View * aspectCorrect * scale;
						float2 ssUV2 = screenPos - N_View * aspectCorrect * scale;
						float sampleZ1 = SampleScreenDepth(ssUV1);
						float sampleZ2 = SampleScreenDepth(ssUV2);
						#if UNITY_REVERSED_Z
						float sampleDepth1 = sampleZ1 < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ1, direction.w);
						float sampleDepth2 = sampleZ2 < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ2, direction.w);
						#else
						float sampleDepth1 = sampleZ1 > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ1, direction.w);
						float sampleDepth2 = sampleZ2 > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ2, direction.w);
						#endif
						depthDiff = max(sampleDepth1 - linearDepth, sampleDepth2 - linearDepth);
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
					case 1:
					{
						[unroll]
						for (int i = 0; i < 4; i++)
						{
							float2 ssUV1 = screenPos + depthRimSampleOffsets4[i] * aspectCorrect * scale;
							float sampleZ = SampleScreenDepth(ssUV1);
							#if UNITY_REVERSED_Z
							float sampleDepth = sampleZ < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#else
							float sampleDepth = sampleZ > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#endif
							depthDiff = max(depthDiff, sampleDepth - linearDepth);
						}
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
					case 2:
					{
						[unroll]
						for (int i = 0; i < 8; i++)
						{
							float2 ssUV1 = screenPos + depthRimSampleOffsets8[i] * aspectCorrect * scale;
							float sampleZ = SampleScreenDepth(ssUV1);
							#if UNITY_REVERSED_Z
							float sampleDepth = sampleZ < 0.0001 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#else
							float sampleDepth = sampleZ > 0.9999 ? skyboxDepth : CorrectedLinearEyeDepth(sampleZ, direction.w);
							#endif
							depthDiff = max(depthDiff, sampleDepth - linearDepth);
						}
						rim = 1.0 ? step(depthThreshold, depthDiff) : smoothstep(depthThresholdLow, depthThreshold, depthDiff);
						rim *= normalFacing;
						break;
					}
				}
				float lightDirFactor = 1.0 == 0
				? saturate(lerp(poiLight.vertexNDotL, poiLight.nDotL, 0.0))
				: saturate(dot(L_View, N_View));
				rim *= lerp(1, lightDirFactor, 0.0);
				rim *= lerp(1, poiLight.attenuation, 0.0);
				rim *= lerp(1, dot(poiLight.rampedLightMap, float3(0.299, 0.587, 0.114)), 0.0);
				float mask = POI2D_SAMPLER_PAN(_DepthRimMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[0.0];
				if (1.0)
				{
					rim *= step(0.5, mask) * step(0.5, distanceFade);
				}
				else
				{
					rim *= mask * distanceFade;
				}
				float3 rimColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0).rgb * lerp(1, poiLight.directColor, 0.0) * lerp(1, poiFragData.baseColor, 1.0) * 1.0;
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				poiLight.finalLightAdd += rim * rimColor * 0.0;
				#endif
				poiFragData.emission += rim * rimColor * 7.7;
				poiFragData.baseColor = lerp(poiFragData.baseColor, rimColor, rim * 0.0);
				poiFragData.baseColor += rim * rimColor * 1.0;
				poiFragData.baseColor = lerp(poiFragData.baseColor, 1 - (1 - poiFragData.baseColor) * (1 - rimColor), rim * 0.0);
				poiFragData.baseColor *= lerp(1, rimColor, rim * 1.0);
			}
			#endif
			#ifdef _SUNDISK_SIMPLE
			float3 RandomColorFromPoint(float2 rando, float hue)
			{
				float saturation = lerp(float4(0.8,1,0,1).x, float4(0.8,1,0,1).y, rando.x);
				float value = lerp(float4(0.8,1,0,1).x, float4(0.8,1,0,1).y, rando.y);
				return HSVtoRGB(float3(hue, saturation, value));
			}
			float2 glitterComputeRotation(float2 pos, float2 center, float2 randomSeed, float timeOffset)
			{
				float randomBoy = poiRand(randomSeed);
				float rotSpeed = lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, randomBoy);
				float randomOffset = 0.0 ? randomBoy : 0;
				float totalSpeed = 0.0 + rotSpeed;
				if (totalSpeed == 0 && timeOffset != 0)
				totalSpeed = 1;
				float theta = radians(randomOffset * 360 + (POI_TIME.x + timeOffset) * totalSpeed * 360);
				float cs = cos(theta);
				float sn = sin(theta);
				return float2((pos.x - center.x) * cs - (pos.y - center.y) * sn + center.x,
				(pos.x - center.x) * sn + (pos.y - center.y) * cs + center.y);
			}
			void processGlitterParticle(
			inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods,
			float2 randPoint, float2 diff,
			float3 norm, float jaggyFix, float glitterFwidth, float glitterSpeedOffset, float glitterRotationTimeOffset,
			bool needsRotation, float glitterMask, float glitterMaxBrightness, float glitterBrightnessOffset)
			{
				float dist = length(diff);
				float randomFromPoint = poiRand(randPoint);
				float size = 0.3;
				if (0.0)
				size = lerp(float4(0.1,0.5,0,1).x, float4(0.1,0.5,0,1).y, randomFromPoint);
				#ifdef POI_AUDIOLINK
				if (0.0)
				size = saturate(size + lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]));
				#endif
				float2 rotatedDiff = diff;
				half glitterAlpha = 1;
				switch (0.0)
				{
					case 0:
					{
						if (0.0 == 1)
						{
							float sqDist = dot(diff, diff);
							glitterAlpha = saturate((size - sqDist) / clamp(fwidth(sqDist), 0.0001, 1.0));
						}
						else
						{
							glitterAlpha = saturate((size - dist) / glitterFwidth);
						}
						break;
					}
					case 1:
					{
						if (needsRotation)
						rotatedDiff = glitterComputeRotation(diff, 0, randPoint * 200, glitterRotationTimeOffset);
						float squareAA = max(glitterFwidth, .1 * jaggyFix);
						glitterAlpha = (1. - smoothstep(size - squareAA, size, abs(rotatedDiff.x))) * (1. - smoothstep(size - squareAA, size, abs(rotatedDiff.y)));
						break;
					}
				}
				if (glitterAlpha <= 0) return;
				float3 finalGlitter = 0;
				half3 glitterColor = poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				float3 randomRotation = 0;
				switch (0.0)
				{
					case 0:
					{
						if (10.0 + glitterSpeedOffset > 0)
						randomRotation = randomFloat3WiggleRange(randPoint, 90.0, 10.0, glitterSpeedOffset);
						else
						randomRotation = poiRand3Range(randPoint, 90.0);
						float3 glitterReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						finalGlitter = lerp(0, 0.0 * glitterAlpha, glitterAlpha) + max(pow(saturate(dot(lerp(glitterReflectionDirection, poiCam.viewDir, 0.8), poiCam.viewDir)), 300.0), 0);
						finalGlitter *= glitterAlpha;
						break;
					}
					case 1:
					{
						float3 cellRand3 = float3(poiRand(randPoint), poiRand(randPoint + 0.5), poiRand(randPoint.yx));
						float3 glitterNorm = abs(frac(cellRand3 * 14.274 + (POI_TIME.x + glitterSpeedOffset) * 0.25) * 2.0 - 1.0);
						glitterNorm = normalize(glitterNorm * 2.0 - 1.0);
						float3 glitterCamDir = lerp(poiCam.forwardDir, poiCam.viewDir, 0.0);
						float glitterDot = dot(glitterNorm, glitterCamDir);
						float glitterVal = abs(frac(glitterDot * 0.25 + 0.25) - 0.5) * 4.0 - 1.0;
						glitterVal = saturate(1.0 - (glitterVal * 50.0 + 50.0));
						glitterVal = pow(glitterVal, 1.0);
						float3 linearHalfDir = normalize(poiCam.viewDir + poiLight.direction * 0.0);
						float linearNDotH = saturate(dot(norm, linearHalfDir));
						glitterVal = saturate(glitterVal * saturate(linearNDotH * 0.0 + 1.0 - 0.0));
						finalGlitter = max(0.0, glitterVal) * glitterAlpha;
						break;
					}
					case 2:
					{
						if (10.0 + glitterSpeedOffset > 0)
						randomRotation = randomFloat3WiggleRange(randPoint, 90.0, 10.0, glitterSpeedOffset);
						else
						randomRotation = poiRand3Range(randPoint, 90.0);
						float3 glitterLightReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						half lightAlpha = glitterAlpha * poiLight.nDotLSaturated;
						float3 halfDir = normalize(poiLight.direction + poiCam.viewDir);
						float specAngle = max(dot(halfDir, glitterLightReflectionDirection), 0.0);
						finalGlitter = lerp(0, 0.0 * lightAlpha, lightAlpha) + max(pow(specAngle, 300.0), 0);
						glitterColor *= poiLight.directColor;
						finalGlitter *= lightAlpha;
						break;
					}
				}
				glitterColor *= lerp(1, poiFragData.baseColor, 0.0);
				#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
				glitterColor *= POI2D_SAMPLER_PANGRAD(_GlitterColorMap, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0), poiMesh.dx, poiMesh.dy).rgb;
				#endif
				float2 uv = remapClamped(-size, size, rotatedDiff, 0, 1);
				if (!0.0 && needsRotation)
				uv = glitterComputeRotation(uv, .5, randPoint * 200, glitterRotationTimeOffset);
				#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
				float2 glitterTexUV = uv;
				if (0.0)
				{
					float glitterSeed = poiRand(randPoint);
					float frameMode = 0.0 == 1 ? 2 : 0;
					float glitterFrameIndex = poiSpriteSheetFrameIndex(
					1.0, 1.0, 0.0,
					frameMode, 10.0, 0.0,
					0, glitterSeed);
					glitterTexUV = poiSpriteSheetUV(uv, 1.0, 1.0, glitterFrameIndex, 0, float4(1,1,0,0).xy);
				}
				float2 finalGlitterTexUV = poiUV(glitterTexUV, float4(1,1,0,0));
				if (0.0)
				finalGlitterTexUV = sharpSample(float4(1,1,1,1), finalGlitterTexUV);
				float4 glitterTexture = POI2D_SAMPLER_PANGRAD(_GlitterTexture, _linear_clamp, finalGlitterTexUV, 0, poiMesh.dx, poiMesh.dy);
				#else
				float4 glitterTexture = 1;
				#endif
				glitterColor *= glitterTexture.rgb;
				float2 colorRando = poiRand2(randPoint.x + randPoint.y);
				float randomHue = poiRand(colorRando);
				if (0.0)
				glitterColor *= RandomColorFromPoint(colorRando, randomHue);
				if (0.0)
				{
					float hueOffset = 0.0 + POI_TIME.x * 0.0;
					if (0.0)
					hueOffset += randomHue;
					glitterColor.rgb = hueShift(glitterColor.rgb, hueOffset, 0.0, 1.0);
				}
				#ifdef POI_AUDIOLINK
				if(poiMods.audioLinkAvailable && 0.0 && 0.0)
				{
					float alColorOverrideIntensity = poiMods.audioLink[0.0];
					float alColorOverrideBlend = evalRamp4(alColorOverrideIntensity, float4(0,1,0,1));
					if(alColorOverrideBlend > 0)
					{
						float3 alOverrideColor = float4(1,1,1,1).rgb;
						if(0.0 > 0)
						{
							alOverrideColor = poiThemeColor(poiMods, alOverrideColor, 0.0);
						}
						glitterColor = lerp(glitterColor, alOverrideColor, alColorOverrideBlend);
					}
				}
				#endif
				if (0.0 == 1)
				{
					poiFragData.baseColor = lerp(poiFragData.baseColor, finalGlitter * glitterColor * (glitterMaxBrightness + glitterBrightnessOffset), finalGlitter * glitterTexture.a * glitterMask);
					poiFragData.emission += finalGlitter * glitterColor * max(0, ((glitterMaxBrightness + glitterBrightnessOffset) - 1) * glitterTexture.a) * glitterMask;
				}
				else
				{
					poiFragData.emission += finalGlitter * glitterColor * (glitterMaxBrightness + glitterBrightnessOffset) * glitterTexture.a * glitterMask;
				}
			}
			void applyGlitter(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods)
			{
				float glitterRotationTimeOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				{
					glitterRotationTimeOffset += AudioLinkGetChronoTime(0.0, 0.0) * 0.0;
				}
				#endif
				bool needsRotation = 0.0 || 0.0 != 0 || any(float4(0,0,0,0).xy) || glitterRotationTimeOffset != 0;
				float3 norm = lerp(poiMesh.normals[0], poiMesh.normals[1], 0.0);
				float jaggyFix = pow(poiCam.distanceToVert, 2) * 0.0;
				float glitterSpeedOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterSpeedOffset = AudioLinkGetChronoTime(0.0, 0.0) * 0.0;
				#endif
				#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
				float glitterMask = POI2D_SAMPLER_PAN(_GlitterMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[0.0];
				#else
				float glitterMask = 1;
				#endif
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterMask = clamp(glitterMask + lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]), 0, glitterMask);
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				glitterMask = 1 - glitterMask;
				glitterMask *= lerp(1, poiLight.rampedLightMap * poiLight.attenuation, 0.0);
				glitterMask *= lerp(1, poiLight.directLuminance, 0.0);
				glitterMask *= float4(1,1,1,1).a;
				if (0.0 > 0)
				glitterMask = maskBlend(glitterMask, poiMods.globalMask[0.0 - 1], 2.0);
				if (glitterMask <= 0) return;
				float glitterBrightnessOffset = 0;
				#ifdef POI_AUDIOLINK
				if (0.0)
				glitterBrightnessOffset = max(lerp(float4(0,0,0,0).x, float4(0,0,0,0).y, poiMods.audioLink[0.0]), 0);
				#endif
				float glitterMaxBrightness = 0.0 == 1 ? 3.0 : 3.0;
				float maxSize = 0.0 ? float4(0.1,0.5,0,1).y : 0.3;
				#ifdef POI_AUDIOLINK
				if (0.0)
				maxSize = saturate(maxSize + max(float4(0,0,0,0).x, float4(0,0,0,0).y));
				#endif
				float2 st = (poiMesh.uv[0.0] + float4(0,0,0,0).xy * POI_TIME.x) * 300.0;
				float2 dd = fwidth(st);
				float distanceLODEnabled = 0.0 == 1 ? 1.0 : 1.0;
				float glitterFwidth = clamp(length(dd), 0.0001, 1.0);
				if (distanceLODEnabled)
				{
					float lodFactor = frac(sin(dot(floor(st / floor(dd + 3.0)), float2(12.9898, 78.233))) * 46203.4357) + 0.5;
					float2 lodScale = floor(dd + lodFactor * 0.5);
					st = st / max(1.0, lodScale) + 300.0 * lodScale;
				}
				[loop] for (uint glitterLayer = 0; glitterLayer < uint(1.0); glitterLayer++)
				{
					float2 i_st = floor(st);
					float2 f_st = frac(st);
					float m_dist = 10.;
					float2 randoPoint = 0;
					float2 closestDiff = 0;
					[loop] for (int cellJ = -1; cellJ <= 1; cellJ++)
					{
						[loop] for (int cellI = -1; cellI <= 1; cellI++)
						{
							float2 neighbor = float2(cellI, cellJ);
							float2 cellRando = poiRand2(i_st + neighbor + glitterLayer * 0.5141);
							float2 cellDiff = neighbor + cellRando * 1.0 - f_st;
							float cellDist = length(cellDiff);
							
							if (0.0)
							{
								if (cellDist > maxSize + 1.0) continue;
								processGlitterParticle(poiFragData, poiMesh, poiCam, poiLight, poiMods,
								cellRando, cellDiff,
								norm, jaggyFix, glitterFwidth, glitterSpeedOffset, glitterRotationTimeOffset,
								needsRotation, glitterMask, glitterMaxBrightness, glitterBrightnessOffset);
							}
							else
							{
								if (cellDist < m_dist)
								{
									closestDiff = cellDiff;
									m_dist = cellDist;
									randoPoint = cellRando;
								}
							}
						}
					}
					
					if (!0.0)
					{
						processGlitterParticle(poiFragData, poiMesh, poiCam, poiLight, poiMods,
						randoPoint, closestDiff,
						norm, jaggyFix, glitterFwidth, glitterSpeedOffset, glitterRotationTimeOffset,
						needsRotation, glitterMask, glitterMaxBrightness, glitterBrightnessOffset);
					}
				}
			}
			#endif
			#ifdef POI_STYLIZED_StylizedSpecular
			#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
			void CalculateUTSSpecular(inout PoiFragData poiFragData, in PoiCam poiCam, inout PoiLight poiLight, in PoiMesh poiMesh, in PoiMods poiMods)
			{
				float nDotH = dot(lerp(poiMesh.normals[0], poiMesh.normals[1], 1.0), poiLight.halfDir);
				float specArea = 0.5 * nDotH + 0.5;
				#if defined(PROP_HIGHCOLOR_TEX) || !defined(OPTIMIZER_ENABLED)
				float3 specularMap = POI2D_SAMPLER_PAN(_HighColor_Tex, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				#else
				float3 specularMap = 1;
				#endif
				float specMask1 = 0;
				float specMask2 = 0;
				if (0.0)
				{
					specMask1 += pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.253;
					specMask2 += pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.0;
				}
				else
				{
					specMask1 += poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 1.0) * 0.253;
					specMask2 += poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 0.0) * 0.0;
				}
				#if defined(PROP_SET_HIGHCOLORMASK) || !defined(OPTIMIZER_ENABLED)
				float specularMask = POI2D_SAMPLER_PAN(_Set_HighColorMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0))[1.0];
				#else
				float specularMask = 1;
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					specularMask = 1 - specularMask;
				}
				specularMask = saturate(specularMask + 0.0);
				float specMask = saturate(specMask1 + specMask2) * specularMask * lerp(poiLight.rampedLightMap, 1, 0.0);
				float attenuation = min(lerp(poiLight.nDotLSaturated, 1, 0.0), lerp(lerp(poiLight.attenuation, 1, 0.0), 1, 0.0));
				if (poiLight.lightType == POI_POINT_SPOT)
				{
					attenuation *= lerp(poiLight.additiveShadow, 1, 0.0);
				}
				float finalSpecMask = min(min(specMask, poiLight.occlusion), attenuation) * 1.0;
				switch (0.0)
				{
					case 0:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0), finalSpecMask);
						break;
					}
					case 1:
					{
						poiLight.finalLightAdd += max(0, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0) * finalSpecMask);
						break;
					}
					case 2:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, blendScreen(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0)), finalSpecMask);
						break;
					}
					case 3:
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, poiFragData.baseColor * specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.directColor, 1.0), finalSpecMask);
						break;
					}
				}
				float3 vSpecMask = 0;
				if (poiFragData.toggleVertexLights)
				{
					#if defined(VERTEXLIGHT_ON)
					for (int index = 0; index < 4; index++)
					{
						if (!any(poiLight.vPosition[index])) continue;
						specArea = 0.5 * poiLight.vDotNH[index] + 0.5;
						if (0.0)
						{
							vSpecMask = pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.253 * poiLight.vAttenuation[index];
							vSpecMask = max(vSpecMask, pow(specArea, exp2(lerp(11, 1, 0.0))) * 0.0 * poiLight.vAttenuation[index]);
						}
						else
						{
							vSpecMask = poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 1.0) * 0.253 * poiLight.vAttenuation[index];
							vSpecMask = max(vSpecMask, poiEdgeNonLinear(specArea, (1.0 - pow(0.0, 5)), 0.0) * 0.0 * poiLight.vAttenuation[index]);
						}
						vSpecMask *= specularMask;
						float finalSpecMask = min(min(vSpecMask, poiLight.occlusion), attenuation) * 1.0;
						switch (0.0)
						{
							case 0:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0), finalSpecMask);
								break;
							}
							case 1:
							{
								poiLight.finalLightAdd += max(0, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0) * finalSpecMask);
								break;
							}
							case 2:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, blendScreen(poiFragData.baseColor, specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0)), finalSpecMask);
								break;
							}
							case 3:
							{
								poiFragData.baseColor = lerp(poiFragData.baseColor, poiFragData.baseColor * specularMap * poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0) * lerp(1, poiLight.vColor[index], 1.0), finalSpecMask);
								break;
							}
						}
					}
					#endif
				}
			}
			#endif
			#endif
			void PoiLightingNoLoopModules(inout PoiLight poiLight, inout PoiFragData poiFragData,
			in PoiMesh poiMesh, in PoiCam poiCam, inout PoiMods poiMods)
			{
			}
			void PoiLightingDependantModules(inout PoiLight poiLight, inout PoiFragData poiFragData, inout MochieBRDFData mochieBRDFData,
			in PoiMesh poiMesh, in PoiCam poiCam, inout PoiMods poiMods)
			{
				#ifdef POI_STYLIZED_StylizedSpecular
				#ifdef _STYLIZEDREFLECTIONMODE_UNITYCHAN
				CalculateUTSSpecular(poiFragData, poiCam, poiLight, poiMesh, poiMods);
				#endif
				#endif
				#ifdef POI_STYLIZED_StylizedSpecular
				#endif
			}
			float4 frag(VertexOut i, bool facing : SV_IsFrontFace) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				PoiSHAr = unity_SHAr;
				PoiSHAg = unity_SHAg;
				PoiSHAb = unity_SHAb;
				PoiSHBr = unity_SHBr;
				PoiSHBg = unity_SHBg;
				PoiSHBb = unity_SHBb;
				PoiSHC =  unity_SHC;
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				#if POI_PIPE == POI_URP
				PoiLightOut poiLightOut;
				PoiInitStruct(PoiLightOut, poiLightOut);
				#endif
				MochieBRDFData mochieBRDFData;
				PoiInitStruct(MochieBRDFData, mochieBRDFData);
				poiMods.globalEmission = 1;
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				#ifdef POI_UDIMDISCARD
				applyUDIMDiscard(i, facing);
				#endif
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
				poiMesh.objNormal = mul((float3x3)unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				poiMesh.isRightHand = i.tangent.w > 0.0;
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && 1)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				poiCam.viewDir = !IsOrthographicCamera() ? (_WorldSpaceCameraPos - i.worldPos.xyz) : UNITY_MATRIX_I_V._m02_m12_m22;
				poiCam.viewDir = normalize(poiCam.viewDir);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				poiMesh.uv[0] = applyUVSettings(poiMesh.uv[0], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[1] = applyUVSettings(poiMesh.uv[1], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[2] = applyUVSettings(poiMesh.uv[2], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[3] = applyUVSettings(poiMesh.uv[3], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				if (0.0 && !poiMesh.isFrontFace)
				{
					poiMesh.uv[0].x += 1.0;
					poiMesh.uv[1].x += 1.0;
					poiMesh.uv[2].x += 1.0;
					poiMesh.uv[3].x += 1.0;
				}
				poiMesh.uv[4] = calculatePanosphereUV(poiMesh);
				poiMesh.uv[5] = calculateWorldUV(poiMesh);
				poiMesh.uv[6] = calculatePolarCoordinate(poiMesh);
				poiMesh.uv[8] = calculatelocalUV(poiMesh);
				float3 worldViewUp = normalize(float3(0, 1, 0) - poiCam.viewDir * dot(poiCam.viewDir, float3(0, 1, 0)));
				float3 worldViewRight = normalize(cross(poiCam.viewDir, worldViewUp));
				poiMesh.uv[9] = float2(dot(worldViewRight, poiMesh.normals[0]), dot(worldViewUp, poiMesh.normals[0])) * 0.5 + 0.5;
				poiMods.globalMask[0] = 1;
				poiMods.globalMask[1] = 1;
				poiMods.globalMask[2] = 1;
				poiMods.globalMask[3] = 1;
				poiMods.globalMask[4] = 1;
				poiMods.globalMask[5] = 1;
				poiMods.globalMask[6] = 1;
				poiMods.globalMask[7] = 1;
				poiMods.globalMask[8] = 1;
				poiMods.globalMask[9] = 1;
				poiMods.globalMask[10] = 1;
				poiMods.globalMask[11] = 1;
				poiMods.globalMask[12] = 1;
				poiMods.globalMask[13] = 1;
				poiMods.globalMask[14] = 1;
				poiMods.globalMask[15] = 1;
				ApplyGlobalMaskModifiers(poiMesh, poiMods, poiCam);
				#if POI_PIPE == POI_URP
				InputData inputData;
				PoiInitStruct(InputData, inputData);
				inputData.positionWS = poiMesh.worldPos;
				inputData.normalWS = poiMesh.normals[1];
				inputData.viewDirectionWS = poiCam.viewDir;
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(i.pos);
				#endif
				float2 mainUV = poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0));
				if (0.0)
				{
					mainUV = sharpSample(float4(0.0009765625,0.0009765625,1024,1024), mainUV);
				}
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, float4(0,0,0,0), 0.0);
				mainTexture.a = max(mainTexture.a, 0.0);
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = PoiUnpackNormalScale(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0)), float4(0,0,0,0), 0.0), 0.0);
				#else
				poiMesh.tangentSpaceNormal = float3(0, 0, 1);
				#endif
				#ifdef FINALPASS
				#if defined(POI_PASS_BASE) || defined(POI_PASS_ADD) || defined(POI_PASS_META) || defined(POI_PASS_DEPTH_NORMALS) || defined(POI_PASS_DEPTH_ONLY)
				ApplyDetailNormal(poiMods, poiMesh);
				#endif
				#endif
				float3 tangentSpaceNormal = float3(0, 0, 1);
				poiMesh.normals[0] = normalize(
				tangentSpaceNormal.x * poiMesh.tangent[0] +
				tangentSpaceNormal.y * poiMesh.binormal[0] +
				tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = getCameraPosition();
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#ifdef UNITY_SINGLE_PASS_STEREO
				poiCam.posScreenSpace.x = poiCam.posScreenSpace.x * 0.5;
				#endif
				poiCam.screenUV = poiCam.posScreenSpace.xy / poiCam.posScreenSpace.w;
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = i.worldDir;
				calculateGlobalThemes(poiMods);
				if (_UdonForceSceneLighting)
				{
					poiLight.lightingMinLightBrightness = 0;
					poiLight.lightingCapEnabled = 0;
					poiLight.lightingMonochromatic = 0;
				}
				else
				{
					poiLight.lightingMinLightBrightness = _LightingMinLightBrightness;
					poiLight.lightingCapEnabled = _LightingCapEnabled;
					poiLight.lightingMonochromatic = _LightingMonochromatic;
				}
				#if defined(POI_PASS_BASE) || defined(POI_PASS_OUTLINE)
				float3 L0 = float3(0, 0, 0);
				float3 L1r = float3(0, 0, 0);
				float3 L1g = float3(0, 0, 0);
				float3 L1b = float3(0, 0, 0);
				#if POI_PIPE == POI_BIRP
				if (_UdonLightVolumeEnabled && 1.0)
				{
					LightVolumeSH(poiMesh.worldPos, L0, L1r, L1g, L1b, poiMesh.normals[1] * 0.0);
					PoiSHAr = float4(L1r, L0.r);
					PoiSHAg = float4(L1g, L0.g);
					PoiSHAb = float4(L1b, L0.b);
					PoiSHBr = 0;
					PoiSHBg = 0;
					PoiSHBb = 0;
					PoiSHC = 0;
				}
				#endif
				#if POI_PIPE == POI_URP
				#ifdef PROBE_VOLUMES_L1
				if (1.0)
				{
					APVResources apvRes = FillAPVResources();
					float3 posWS = GetAbsolutePositionWS(poiMesh.worldPos + poiMesh.normals[1] * 0.0);
					posWS = AddNoiseToSamplingPosition(posWS, poiCam.clipPos.xy, poiCam.viewDir);
					APVSample apvSample = SampleAPV(posWS, poiMesh.normals[1], GetMeshRenderingLayer(), poiCam.viewDir);
					if (apvSample.status != APV_SAMPLE_STATUS_INVALID)
					{
						apvSample.Decode();
						PoiSHAr = float4(apvSample.L1_R, apvSample.L0.r);
						PoiSHAg = float4(apvSample.L1_G, apvSample.L0.g);
						PoiSHAb = float4(apvSample.L1_B, apvSample.L0.b);
						#ifdef PROBE_VOLUMES_L2
						PoiSHBr = float4(apvSample.L2_R, apvSample.L0.r);
						PoiSHBg = float4(apvSample.L2_G, apvSample.L0.r);
						PoiSHBb = float4(apvSample.L2_B, apvSample.L0.r);
						PoiSHC = float4(apvSample.L2_C, 0);
						#else
						PoiSHBr = 0;
						PoiSHBg = 0;
						PoiSHBb = 0;
						PoiSHC = 0;
						#endif
					}
				}
				#endif
				#endif
				#endif
				#if defined(PROP_LIGHTINGDETAILSHADOWMAPS)
				float4 DetailShadows = POI2D_SAMPLER_PAN(_LightingDetailShadowMaps, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				float poiDetailShadowAdditional = lerp(1, DetailShadows.r, 1.0) * lerp(1, DetailShadows.g, 0.0) * lerp(1, DetailShadows.b, 0.0) * lerp(1, DetailShadows.a, 0.0);
				float poiDetailShadowMain = lerp(1, DetailShadows.r, 1.0) * lerp(1, DetailShadows.g, 0.0) * lerp(1, DetailShadows.b, 0.0) * lerp(1, DetailShadows.a, 0.0);
				#else
				float poiDetailShadowAdditional = 1;
				float poiDetailShadowMain = 1;
				#endif
				if (0.0 > 0)
				{
					poiDetailShadowAdditional = maskBlend(poiDetailShadowAdditional, poiMods.globalMask[0.0 - 1], 2.0);
					poiDetailShadowMain = maskBlend(poiDetailShadowMain, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#ifndef POI_PASS_ADD
				poiLight.detailShadow = poiDetailShadowMain;
				#else
				poiLight.detailShadow = poiDetailShadowAdditional;
				#endif
				#if defined(PROP_LIGHTINGSHADOWMASKS)
				float4 ShadowMasks = POI2D_SAMPLER_PAN(_LightingShadowMasks, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				poiLight.shadowMask = lerp(1, ShadowMasks.r, 1.0) * lerp(1, ShadowMasks.g, 0.0) * lerp(1, ShadowMasks.b, 0.0) * lerp(1, ShadowMasks.a, 0.0);
				#else
				poiLight.shadowMask = 1;
				#endif
				if (0.0 > 0)
				{
					poiLight.shadowMask = maskBlend(poiLight.shadowMask.r, poiMods.globalMask[0.0 - 1], 2.0);
				}
				#if POI_PIPE == POI_URP && !defined(POI_PASS_LILFUR)
				poiLight.shadowMask = poiLight.shadowMask * PoiCalculateShadowMask(i.lightmapUV.xy);
				#endif
				#if defined(PROP_LIGHTINGAOMAPS)
				float4 AOMaps = POI2D_SAMPLER_PAN(_LightingAOMaps, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0));
				poiLight.occlusion = min(min(min(lerp(1, AOMaps.r, 1.0), lerp(1, AOMaps.g, 0.0)), lerp(1, AOMaps.b, 0.0)), lerp(1, AOMaps.a, 0.0));
				#else
				poiLight.occlusion = 1;
				#endif
				if (0.0 > 0)
				{
					poiLight.occlusion = maskBlend(poiLight.occlusion, poiMods.globalMask[0.0 - 1], 2.0);
				}
				Light mainLight;
				PoiInitStruct(Light, mainLight);
				#if POI_PIPE == POI_BIRP
				mainLight.direction = _WorldSpaceLightPos0.xyz;
				mainLight.color = _LightColor0.rgb;
				#elif POI_PIPE == POI_URP
				mainLight = GetMainLight(TransformWorldToShadowCoord(poiMesh.worldPos), poiMesh.worldPos, poiLight.shadowMask);
				BRDFData brdfData;
				InitializeBRDFData(1, 0.0, 0.0, 0.0, poiFragData.alpha, brdfData);
				BRDFData brdfDataClearCoat;
				InitializeBRDFDataClearCoat(0.0, 0.5, brdfData, brdfDataClearCoat);
				poiLight.brdfData = brdfData;
				poiLight.brdfDataClearCoat = brdfDataClearCoat;
				#endif
				#if POI_PIPE == POI_BIRP && defined(POI_PASS_ADD)
				poiLight.lightType = (_WorldSpaceLightPos0.w == 0.0) ? POI_DIRECTIONAL : POI_POINT_SPOT;
				#else
				poiLight.lightType = POI_DIRECTIONAL;
				#endif
				poiLight.unityLight = mainLight;
				float4 debugLight = PoiLightDataSetup(poiLight, poiFragData, poiMesh, poiCam, poiMods, i, mainTexture);
				#if defined(POI_PASS_BASE) || defined(POI_PASS_ADD)
				if (0.0) return debugLight;
				#endif
				poiFragData.baseColor = mainTexture.rgb;
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				#else
				if(_TwoPassOverrideFirstPassColor == 1)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, _TwoPassColor.rgb, _TwoPassColorThemeIndex);
					poiFragData.alpha = mainTexture.a * _TwoPassColor.a;
				}
				else if(_TwoPassOverrideFirstPassColor == 2)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * _TwoPassAlphaOverride;
				}
				else
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				}
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					float4 alphaMaskTex = 1;
					#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
					alphaMaskTex = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0).xy);
					#endif
					float alphaMask = poiSampleMask(alphaMaskTex, float4(1.0, 0.0, 0.0, 0.0), 2.0, 0.0, float4(0,1,0,1), 1.0, 1.0, 0.0);
					if (0.0 == 1) poiFragData.alpha = alphaMask;
					if (0.0 == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (0.0 == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (0.0 == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				#ifdef FINALPASS
				ApplyDetailColor(poiFragData, poiMesh, poiMods);
				#endif
				poiLight.finalLighting = 1;
				poiLight.rampedLightMap = poiEdgeNonLinear(poiLight.nDotL, 0.1, .1);
				#ifndef VIGNETTE_MASKED
				PoiLightingNoLoopModules(poiLight, poiFragData, poiMesh, poiCam, poiMods);
				PoiLightingDependantModules(poiLight, poiFragData, mochieBRDFData, poiMesh, poiCam, poiMods);
				#endif
				#ifdef _POI_DEPTH_RIMLIGHT
				ApplyDepthRimLighting(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				#ifdef _SUNDISK_SIMPLE
				applyGlitter(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					poiFragData.baseColor *= saturate(poiFragData.alpha);
				}
				poiFragData.finalColor = poiFragData.baseColor;
				#if POI_PIPE == POI_URP
				poiLight.finalLighting += poiLightOut.totalLighting;
				#endif
				poiFragData.finalColor = poiFragData.baseColor * poiLight.finalLighting;
				#if POI_PIPE == POI_URP
				poiLight.finalLightAdd += poiLightOut.totalLightAdd;
				#endif
				poiFragData.finalColor += poiLight.finalLightAdd;
				#ifndef POI_PASS_LILFUR
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.alpha = 1.0 ? 1 : poiFragData.alpha;
				#else
				poiFragData.alpha = _AlphaForceOpaque2 ? 1 : poiFragData.alpha;
				#endif
				#endif
				if (0.0 == POI_MODE_OPAQUE)
				{
					poiFragData.alpha = 1;
				}
				clip(poiFragData.alpha - 0.0);
				if (0.0 == POI_MODE_CUTOUT && !0.0)
				{
					poiFragData.alpha = 1;
				}
				if (4.0 == 4)
				{
					poiFragData.alpha = saturate(poiFragData.alpha * 10.0);
				}
				if (0.0 != POI_MODE_TRANSPARENT)
				{
					poiFragData.finalColor *= poiFragData.alpha;
				}
				applyUnityFog(poiFragData.finalColor, i.fogData, 0.0);
				return float4(poiFragData.finalColor, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			ENDCG
		}
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilCompareFunction]
				Pass [_StencilPassOp]
				Fail [_StencilFailOp]
				ZFail [_StencilZFailOp]
			}
			ZWrite [_ZWrite]
			Cull Back
			AlphaToMask Off
			ZTest [_ZTest]
			ColorMask RGBA
			Offset [_OffsetFactor], [_OffsetUnits]
			BlendOp [_BlendOp], [_BlendOpAlpha]
			Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
			CGPROGRAM
 #define FINALPASS 
 #define POI_EMISSION_1 
 #define POI_STYLIZED_StylizedSpecular 
 #define _POI_DEPTH_RIMLIGHT 
 #define _STOCHASTICMODE_DELIOT_HEITZ 
 #define _STYLIZEDREFLECTIONMODE_UNITYCHAN 
 #define _SUNDISK_SIMPLE 
 #define PROP_DETAILTEX 
 #define PROP_DETAILNORMALMAP 
 #define OPTIMIZER_ENABLED 
			#pragma target 5.0
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3 _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH _DECAL_LAYERS
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION _USE_FAST_SRGB_LINEAR_CONVERSION _LIGHT_LAYERS
			#if !defined(POI_WORLD)
			#pragma skip_variants _ADDITIONAL_LIGHTS_VERTEX LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#if POI_PIPE == POI_URP
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#define FOG_LINEAR 0
			#define FOG_EXP 0
			#define FOG_EXP2 0
			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
			#else
			#pragma dynamic_branch _ FOG_LINEAR
			#pragma dynamic_branch _ FOG_EXP
			#pragma dynamic_branch _ FOG_EXP2
			#endif
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#define POI_PASS_SHADOW
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define STEREO_UV(uv) float3(uv, unity_StereoEyeIndex)
			#else
			#define STEREO_UV(uv) uv
			#endif
			#if POI_PIPE == POI_BIRP
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			Texture2DArray<float> _CameraDepthTexture;
			#else
			Texture2D<float> _CameraDepthTexture;
			#endif
			#endif
			#if POI_PIPE == POI_URP
			TEXTURE2D_X_FLOAT(_CameraDepthTexture);
			float4 _CameraDepthTexture_TexelSize;
			#endif
			float SampleScreenDepth(float2 uv)
			{
				uv.y = _ProjectionParams.x * 0.5 + 0.5 - uv.y * _ProjectionParams.x;
				return _CameraDepthTexture.SampleLevel(sampler_point_clamp, STEREO_UV(uv), 0).r;
			}
			bool DepthTextureExists()
			{
				float3 dTexDim = 0;
				#if POI_PIPE == POI_BIRP
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y, dTexDim.z);
				#else
				_CameraDepthTexture.GetDimensions(dTexDim.x, dTexDim.y);
				#endif
				#endif
				#if POI_PIPE == POI_URP
				dTexDim.xy = _CameraDepthTexture_TexelSize.zw;
				#endif
				return dTexDim.x > 16;
			}
			UNITY_DECLARE_TEX2D(_MainTex);
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			#endif
			float _GrabMode;
			float _Mode;
			#ifndef _STOCHASTICMODE_NONE
			#ifdef _STOCHASTICMODE_DELIOT_HEITZ
			float _StochasticDeliotHeitzDensity;
			#endif
			#endif
			float4 _Color;
			float _ColorThemeIndex;
			float _MainPixelMode;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float2 _MainTexPan;
			float _MainTexUV;
			float _MainTexStochastic;
			float _MainIgnoreTexAlpha;
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _MainAlphaMaskMode;
			float _AlphaMaskR;
			float _AlphaMaskG;
			float _AlphaMaskB;
			float _AlphaMaskA;
			float _AlphaMaskChannelBlendMode;
			float2 _AlphaMaskMinMax;
			float _AlphaMaskGamma;
			float _AlphaMaskBlendStrength;
			float _AlphaMaskValue;
			float _Cutoff;
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			int _GlobalMaskVertexColorLinearSpace;
			float2 _UVSettingsTiling0;
			float2 _UVSettingsOffset0;
			float _UVSettingsAngle0;
			float2 _UVSettingsPan0;
			float _UVSettingsRotate0;
			float2 _UVSettingsTiling1;
			float2 _UVSettingsOffset1;
			float _UVSettingsAngle1;
			float2 _UVSettingsPan1;
			float _UVSettingsRotate1;
			float2 _UVSettingsTiling2;
			float2 _UVSettingsOffset2;
			float _UVSettingsAngle2;
			float2 _UVSettingsPan2;
			float _UVSettingsRotate2;
			float2 _UVSettingsTiling3;
			float2 _UVSettingsOffset3;
			float _UVSettingsAngle3;
			float2 _UVSettingsPan3;
			float _UVSettingsRotate3;
			float _UVSettingsShiftBackfaceUV;
			float _StereoEnabled;
			float _PolarUV;
			float _PolarMode;
			float2 _PolarCenter;
			float _PolarRadialScale;
			float _PolarLengthScale;
			float _PolarSpiralPower;
			float _PanoUseBothEyes;
			float _UVModWorldPos0;
			float _UVModWorldPos1;
			float _UVModLocalPos0;
			float _UVModLocalPos1;
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				#ifdef POI_PASS_MOTION_VECTORS
				float3 positionOld : TEXCOORD4;
				#if _ADD_PRECOMPUTED_VELOCITY
				float3 alembicMotionVector : TEXCOORD5;
				#endif
				#endif
				#ifdef POI_TESSELLATED
				uint vertexId : TEXCOORD6;
				#else
				uint vertexId : SV_VertexID;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float worldDir : TEXCOORD8;
				float fogData: TEXCOORD10;
				POI_SHADOW_COORDS(9)
				#ifdef POI_SQUISH
				float squishAmount : TEXCOORD11;
				#endif
				#ifdef POIBS_BLOOMFOG
				float2 bsFogScreenUV : TEXCOORD12;
				#endif
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD13;
				float4 LightCoord : TEXCOORD14;
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				float4 positionCSNoJitter         : POSITION_CS_NO_JITTER;
				float4 previousPositionCSNoJitter : PREV_POSITION_CS_NO_JITTER;
				#endif
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(POI_DEVICE_QUEST)
				uint gl_Layer : SV_RenderTargetArrayIndex;
				#endif
			};
			float2 vertexUV(in VertexOut o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in appdata v, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return v.uv0.xy * w0 + v.uv1.xy * w1 + v.uv2.xy * w2 + v.uv3.xy * w3;
			}
			#ifdef POI_PASS_LILFUR
			float2 vertexUV(in PoiGeoToFragLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			float2 vertexUV(in PoiVertToGeoLilFur o, int index)
			{
				float i = index;
				float w0 = saturate(1 - abs(i - 0));
				float w1 = saturate(1 - abs(i - 1));
				float w2 = saturate(1 - abs(i - 2));
				float w3 = saturate(1 - abs(i - 3));
				return o.uv[0].xy * w0 + o.uv[0].zw * w1 + o.uv[1].xy * w2 + o.uv[1].zw * w3;
			}
			#endif
			#ifdef POI_AUDIOLINK
			inline int poiALBandPass(int bandIdx)
			{
				bandIdx = clamp(bandIdx, 0, 3);
				return bandIdx == 0 ? ALPASS_AUDIOBASS : bandIdx == 1 ? ALPASS_AUDIOLOWMIDS : bandIdx == 2 ? ALPASS_AUDIOHIGHMIDS : ALPASS_AUDIOTREBLE;
			}
			#endif
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				float3 outputColor = srcColor;
				if (themeIndex != 0)
				{
					themeIndex = max(themeIndex - 1, 0);
					if (themeIndex <= 3)
					{
						outputColor = poiMods.globalColorTheme[themeIndex].rgb;
					}
					else
					{
						#ifdef POI_AUDIOLINK
						if (poiMods.audioLinkAvailable)
						{
							outputColor = poiMods.globalColorTheme[themeIndex].rgb;
						}
						#endif
					}
				}
				return outputColor;
			}
			struct PoiSpriteSheetConfig
			{
				float columns;
				float rows;
				float totalFrames;
				float frameSelection;
				float fps;
				float randomStart;
				float inset;
				float2 textureST;
			};
			float poiSpriteSheetFrameIndex(PoiSpriteSheetConfig config, float progress, float seed)
			{
				float totalFrames = (config.totalFrames > 0) ? config.totalFrames : (config.columns * config.rows);
				float frameIndex = 0;
				if (config.frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (config.frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (config.frameSelection == 2)
				{
					float timeOffset = config.randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * config.fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float poiSpriteSheetFrameIndex(float columns, float rows, float configTotalFrames, float frameSelection, float fps, float randomStart, float progress, float seed)
			{
				float totalFrames = (configTotalFrames > 0) ? configTotalFrames : (columns * rows);
				float frameIndex = 0;
				if (frameSelection == 0)
				frameIndex = floor(seed * totalFrames);
				else if (frameSelection == 1)
				frameIndex = floor(progress * totalFrames);
				else if (frameSelection == 2)
				{
					float timeOffset = randomStart ? seed * totalFrames : 0;
					frameIndex = floor(fmod(POI_TIME.y * fps + timeOffset, totalFrames));
				}
				return clamp(frameIndex, 0, totalFrames - 1);
			}
			float2 poiSpriteSheetUV(float2 uv, float columns, float rows, float frameIndex, float inset, float2 textureST)
			{
				float invCols = 1.0 / columns;
				float invRows = 1.0 / rows;
				float2 frameSize = float2(invCols, invRows);
				float column = fmod(frameIndex, columns);
				float row = floor(frameIndex * invCols);
				row = rows - 1 - row;
				float2 lookupUV = (uv * frameSize) + float2(column, row) * frameSize;
				if (inset > 0)
				{
					float2 texelInset = inset * frameSize / max(textureST, 1e-6);
					lookupUV = lerp(lookupUV, float2(column + 1, row + 1) * frameSize, texelInset);
				}
				return lookupUV;
			}
			float2 poiSpriteSheetUV(float2 uv, PoiSpriteSheetConfig config, float frameIndex)
			{
				return poiSpriteSheetUV(uv, config.columns, config.rows, frameIndex, config.inset, config.textureST);
			}
			void applyReducedRenderClipDistance(inout VertexOut o)
			{
				if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
				{
					#if defined(UNITY_REVERSED_Z) // DirectX
					o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
					#else // OpenGL
					o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
					#endif
				}
			}
			#if POI_PIPE == POI_URP
			#if UNITY_VERSION < 600000
			half IsDirectionalLight()
			{
				return round(_ShadowBias.z) == 1.0 ? 1 : 0;
			}
			float4 ApplyShadowClamping(float4 positionCS)
			{
				#if UNITY_REVERSED_Z
				float clamped = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
				#else
				float clamped = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				positionCS.z = lerp(positionCS.z, clamped, IsDirectionalLight());
				return positionCS;
			}
			#endif
			float4 GetShadowPositionHClip(float3 positionWS, float3 normalWS)
			{
				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
				float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
				float3 lightDirectionWS = _LightDirection;
				#endif
				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
				positionCS = ApplyShadowClamping(positionCS);
				return positionCS;
			}
			#endif
			VertexOut vert(appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				POI_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				#ifdef POI_PASS_OUTLINE
				if (0.0 > 0.5)
				{
					return (VertexOut)POI_NAN;
				}
				#endif
				#ifdef POI_AUDIOLINK
				float vertexAudioLink[5];
				vertexAudioLink[0] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 0))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 0))[0];
				vertexAudioLink[1] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 1))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 1))[0];
				vertexAudioLink[2] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 2))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 2))[0];
				vertexAudioLink[3] = 0.0 == 0 ? AudioLinkData(ALPASS_AUDIOLINK + float2(0, 3))[0] : AudioLinkData(ALPASS_FILTEREDAUDIOLINK + float2((1 - 0.0) * 15.95, 3))[0];
				vertexAudioLink[4] = AudioLinkData(ALPASS_GENERALVU + float2(8, 0))[0];
				#endif
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				#if POI_PIPE == POI_BIRP
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				#elif  POI_PIPE == POI_URP
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent);
				o.normal = vertexNormalInput.normalWS;
				o.tangent.xyz = vertexNormalInput.tangentWS;
				#endif
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				#if defined(LIGHTMAP_ON)
				OUTPUT_LIGHTMAP_UV(v.uv1.xy, unity_LightmapST, o.lightmapUV.xy);
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				OUTPUT_LIGHTMAP_UV(v.uv2.xy, unity_LightmapST, o.lightmapUV.zw);
				#endif
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				o.localPos.xyz += localOffset;
				o.worldPos.xyz += worldOffset;
				o.pos = PoiObjectToClipPos(o.localPos.xyz);
				o.fogData = o.pos.z;
				#if !defined(POI_PASS_SHADOW)
				POI_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				POI_TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				o.worldDir = dot(o.pos, CalculateFrustumCorrection());
				if (0.0)
				{
					applyReducedRenderClipDistance(o);
				}
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#ifdef EDITOR_VISUALIZATION
				UnityEditorVizData(v.vertex.xyz, o.uv[0].xy, o.uv[0].zw, o.uv[1].xy, o.VizUV, o.LightCoord);
				#endif
				#endif
				#ifdef POI_PASS_MOTION_VECTORS
				#if UNITY_VERSION >= 202100
				#if defined(APPLICATION_SPACE_WARP_MOTION)
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				o.pos = o.positionCSNoJitter;
				#else
				o.positionCSNoJitter = mul(_NonJitteredViewProjMatrix, mul(UNITY_MATRIX_M, o.localPos));
				#endif
				float4 prevPos = (unity_MotionVectorsParams.x == 1) ? float4(v.positionOld, 1) : o.localPos;
				#if _ADD_PRECOMPUTED_VELOCITY
				prevPos = prevPos - float4(v.alembicMotionVector, 0);
				#endif
				o.previousPositionCSNoJitter = mul(_PrevViewProjMatrix, mul(UNITY_PREV_MATRIX_M, prevPos));
				#endif
				#endif
				#ifdef POI_PASS_LILFUR
				#endif
				return o;
			}
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D(float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1, 311.7)), dot(s, float2(269.5, 183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * 1.0);
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				float3x3 pos = float3x3(
				float3(vxID, bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				return (bary.z > 0) ? pos : neg;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				return mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + 0.0);
				if (0.0 > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[0.0 - 1], 2.0);
				}
			}
			void ApplyGlobalMaskModifiers(in PoiMesh poiMesh, inout PoiMods poiMods, in PoiCam poiCam)
			{
			}
			float2 applyUVSettings(float2 uv, float2 tiling, float2 offset, float angle, float2 pan, float rotate)
			{
				uv = uv * tiling + offset;
				float angleRad = angle * (UNITY_PI / 180.0);
				float rotateRad = rotate * (UNITY_PI / 180.0);
				float totalAngle = angleRad + rotateRad * POI_TIME.y;
				if (totalAngle != 0)
				{
					float2 pivot = float2(0.5, 0.5);
					float cosA = cos(totalAngle);
					float sinA = sin(totalAngle);
					uv -= pivot;
					uv = float2(
					uv.x * cosA - uv.y * sinA,
					uv.x * sinA + uv.y * cosA
					);
					uv += pivot;
				}
				uv += pan * POI_TIME.y;
				return uv;
			}
			float2 calculatePolarCoordinate(in PoiMesh poiMesh)
			{
				float2 delta = poiMesh.uv[0.0] - float4(0.5,0.5,0,0);
				float dist = length(delta);
				float radius;
				
				if (0.0 == 0) // Standard
				{
					radius = dist * 2 * 1.0;
				}
				else if (0.0 == 1) // Log Polar - preserves texture scale
				{
					radius = log(dist + 0.001) * 1.0 + 1;
				}
				else // Square Polar
				{
					radius = max(abs(delta.x), abs(delta.y)) * 2 * 1.0;
				}
				float angle = atan2(delta.x, delta.y);
				float phi = angle / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				angle = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				angle *= 1.0;
				return float2(radius, angle + dist * 0.0);
			}
			float2 MonoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(1.0, 1.0 / UNITY_PI);
				return float2(1.0, 1.0) - sphereCoords;
			}
			float2 StereoPanoProjection(float3 coords)
			{
				float3 normalizedCoords = normalize(coords);
				float latitude = acos(normalizedCoords.y);
				float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
				float phi = longitude / (UNITY_PI * 2.0);
				float phi_frac = frac(phi);
				longitude = fwidth(phi) - 0.0001 < fwidth(phi_frac) ? phi : phi_frac;
				longitude *= 2;
				float2 sphereCoords = float2(longitude, latitude) * float2(0.5, 1.0 / UNITY_PI);
				sphereCoords = float2(0.5, 1.0) - sphereCoords;
				return (sphereCoords + float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).xy) * float4(0, 1 - unity_StereoEyeIndex, 1, 0.5).zw;
			}
			float2 calculateWorldUV(in PoiMesh poiMesh)
			{
				return float2(0.0 != 3 ? poiMesh.worldPos[ 0.0] : 0.0f, 2.0 != 3 ? poiMesh.worldPos[2.0] : 0.0f);
			}
			float2 calculatelocalUV(in PoiMesh poiMesh)
			{
				float localUVs[8];
				localUVs[0] = poiMesh.localPos.x;
				localUVs[1] = poiMesh.localPos.y;
				localUVs[2] = poiMesh.localPos.z;
				localUVs[3] = 0;
				localUVs[4] = poiMesh.vertexColor.r;
				localUVs[5] = poiMesh.vertexColor.g;
				localUVs[6] = poiMesh.vertexColor.b;
				localUVs[7] = poiMesh.vertexColor.a;
				return float2(localUVs[0.0],localUVs[1.0]);
			}
			float2 calculatePanosphereUV(in PoiMesh poiMesh)
			{
				float3 viewDirection = normalize(lerp(getCameraPosition().xyz, _WorldSpaceCameraPos.xyz, 1.0) - poiMesh.worldPos.xyz) * - 1;
				return lerp(MonoPanoProjection(viewDirection), StereoPanoProjection(viewDirection), 0.0);
			}
			float4 frag(VertexOut i, bool facing : SV_IsFrontFace) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				PoiSHAr = unity_SHAr;
				PoiSHAg = unity_SHAg;
				PoiSHAb = unity_SHAb;
				PoiSHBr = unity_SHBr;
				PoiSHBg = unity_SHBg;
				PoiSHBb = unity_SHBb;
				PoiSHC =  unity_SHC;
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				#if POI_PIPE == POI_URP
				PoiLightOut poiLightOut;
				PoiInitStruct(PoiLightOut, poiLightOut);
				#endif
				MochieBRDFData mochieBRDFData;
				PoiInitStruct(MochieBRDFData, mochieBRDFData);
				poiMods.globalEmission = 1;
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				#ifdef POI_UDIMDISCARD
				applyUDIMDiscard(i, facing);
				#endif
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
				poiMesh.objNormal = mul((float3x3)unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				poiMesh.isRightHand = i.tangent.w > 0.0;
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && 1)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				poiCam.viewDir = !IsOrthographicCamera() ? (_WorldSpaceCameraPos - i.worldPos.xyz) : UNITY_MATRIX_I_V._m02_m12_m22;
				poiCam.viewDir = normalize(poiCam.viewDir);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				poiMesh.uv[0] = applyUVSettings(poiMesh.uv[0], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[1] = applyUVSettings(poiMesh.uv[1], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[2] = applyUVSettings(poiMesh.uv[2], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				poiMesh.uv[3] = applyUVSettings(poiMesh.uv[3], float4(1,1,0,0), float4(0,0,0,0), 0.0, float4(0,0,0,0), 0.0);
				if (0.0 && !poiMesh.isFrontFace)
				{
					poiMesh.uv[0].x += 1.0;
					poiMesh.uv[1].x += 1.0;
					poiMesh.uv[2].x += 1.0;
					poiMesh.uv[3].x += 1.0;
				}
				poiMesh.uv[4] = calculatePanosphereUV(poiMesh);
				poiMesh.uv[5] = calculateWorldUV(poiMesh);
				poiMesh.uv[6] = calculatePolarCoordinate(poiMesh);
				poiMesh.uv[8] = calculatelocalUV(poiMesh);
				float3 worldViewUp = normalize(float3(0, 1, 0) - poiCam.viewDir * dot(poiCam.viewDir, float3(0, 1, 0)));
				float3 worldViewRight = normalize(cross(poiCam.viewDir, worldViewUp));
				poiMesh.uv[9] = float2(dot(worldViewRight, poiMesh.normals[0]), dot(worldViewUp, poiMesh.normals[0])) * 0.5 + 0.5;
				poiMods.globalMask[0] = 1;
				poiMods.globalMask[1] = 1;
				poiMods.globalMask[2] = 1;
				poiMods.globalMask[3] = 1;
				poiMods.globalMask[4] = 1;
				poiMods.globalMask[5] = 1;
				poiMods.globalMask[6] = 1;
				poiMods.globalMask[7] = 1;
				poiMods.globalMask[8] = 1;
				poiMods.globalMask[9] = 1;
				poiMods.globalMask[10] = 1;
				poiMods.globalMask[11] = 1;
				poiMods.globalMask[12] = 1;
				poiMods.globalMask[13] = 1;
				poiMods.globalMask[14] = 1;
				poiMods.globalMask[15] = 1;
				ApplyGlobalMaskModifiers(poiMesh, poiMods, poiCam);
				#if POI_PIPE == POI_URP
				InputData inputData;
				PoiInitStruct(InputData, inputData);
				inputData.positionWS = poiMesh.worldPos;
				inputData.normalWS = poiMesh.normals[1];
				inputData.viewDirectionWS = poiCam.viewDir;
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(i.pos);
				#endif
				float2 mainUV = poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0));
				if (0.0)
				{
					mainUV = sharpSample(float4(0.0009765625,0.0009765625,1024,1024), mainUV);
				}
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, float4(0,0,0,0), 0.0);
				mainTexture.a = max(mainTexture.a, 0.0);
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = PoiUnpackNormalScale(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[0.0].xy, float4(1,1,0,0)), float4(0,0,0,0), 0.0), 0.0);
				#else
				poiMesh.tangentSpaceNormal = float3(0, 0, 1);
				#endif
				#ifdef FINALPASS
				#if defined(POI_PASS_BASE) || defined(POI_PASS_ADD) || defined(POI_PASS_META) || defined(POI_PASS_DEPTH_NORMALS) || defined(POI_PASS_DEPTH_ONLY)
				ApplyDetailNormal(poiMods, poiMesh);
				#endif
				#endif
				float3 tangentSpaceNormal = float3(0, 0, 1);
				poiMesh.normals[0] = normalize(
				tangentSpaceNormal.x * poiMesh.tangent[0] +
				tangentSpaceNormal.y * poiMesh.binormal[0] +
				tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = getCameraPosition();
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#ifdef UNITY_SINGLE_PASS_STEREO
				poiCam.posScreenSpace.x = poiCam.posScreenSpace.x * 0.5;
				#endif
				poiCam.screenUV = poiCam.posScreenSpace.xy / poiCam.posScreenSpace.w;
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = i.worldDir;
				poiFragData.baseColor = mainTexture.rgb;
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
				poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				#else
				if(_TwoPassOverrideFirstPassColor == 1)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, _TwoPassColor.rgb, _TwoPassColorThemeIndex);
					poiFragData.alpha = mainTexture.a * _TwoPassColor.a;
				}
				else if(_TwoPassOverrideFirstPassColor == 2)
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * _TwoPassAlphaOverride;
				}
				else
				{
					poiFragData.baseColor *= poiThemeColor(poiMods, float4(1,1,1,1).rgb, 0.0);
					poiFragData.alpha = mainTexture.a * float4(1,1,1,1).a;
				}
				#endif
				NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(poiFragData.baseColor);
				if (0.0)
				{
					float4 alphaMaskTex = 1;
					#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
					alphaMaskTex = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[0.0], float4(1,1,0,0)), float4(0,0,0,0).xy);
					#endif
					float alphaMask = poiSampleMask(alphaMaskTex, float4(1.0, 0.0, 0.0, 0.0), 2.0, 0.0, float4(0,1,0,1), 1.0, 1.0, 0.0);
					if (0.0 == 1) poiFragData.alpha = alphaMask;
					if (0.0 == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (0.0 == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (0.0 == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				poiFragData.finalColor = poiFragData.baseColor;
				#ifndef POI_PASS_LILFUR
				#if !defined(POI_PASS_BASETWO) && !defined(POI_PASS_ADDTWO)
				poiFragData.alpha = 1.0 ? 1 : poiFragData.alpha;
				#else
				poiFragData.alpha = _AlphaForceOpaque2 ? 1 : poiFragData.alpha;
				#endif
				#endif
				if (0.0 == POI_MODE_OPAQUE)
				{
					poiFragData.alpha = 1;
				}
				clip(poiFragData.alpha - 0.0);
				applyUnityFog(poiFragData.finalColor, i.fogData, 0.0);
				return float4(poiFragData.finalColor, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			ENDCG
		}
	}
	CustomEditor "Thry.ShaderEditor"
}
