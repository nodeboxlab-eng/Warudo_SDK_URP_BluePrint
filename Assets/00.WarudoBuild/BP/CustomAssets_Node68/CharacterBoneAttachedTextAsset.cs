using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UMod;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Utils;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomAssets
{
    [AssetType(
        Id = "a8f3c2e1-9d4b-4e7f-a6c5-8b2d1e3f9045",
        Title = "TextDisplay Node68",
        Category = CustomAssetsFlavorEmbedded.ShareBuild
            ? CustomAssetsNode68Categories.TextDisplayShare
            : CustomAssetsNode68Categories.TextDisplayDev
    )]
    public sealed class CharacterBoneAttachedTextAsset : GameObjectAsset
    {
        public override long GetVersion() => 25;

        private const string RootTransformPathLabel = "Root Transform";
        private const string FixedTmpFontResourcesPath = "Fonts/BazziSDF";

        [DataInput]
        [Section("대상")]
        [Label("표시")]
        [Description(
            "끄면 메시를 숨기고 본 추적도 중단합니다. 렌더링은 TextMesh Pro(SDF)만 사용합니다. 한글 깨짐은 「TMP 폰트 Resources 경로」에 한글 포함 SDF 에셋을 넣거나, 「Unity 폰트 Resources 경로」에 한글 .ttf 가 포함된 Font 를 넣으세요."
        )]
        public bool Visible = true;

        /// <summary>구버전 씬 호환. <see cref="AttachTarget"/> 으로 이전됩니다.</summary>
        [DataInput]
        [Hidden]
        public CharacterAsset Character;

        [DataInput]
        [Section("대상")]
        [Label("부착 대상")]
        [Description("Character · Prop · Camera · Anchor 등 씬의 GameObjectAsset. Warudo Transform Attachment 의 Attach To 와 같습니다.")]
        public GameObjectAsset AttachTarget;

        [DataInput]
        [Section("부착")]
        [Label("부착 방식")]
        public BoneTextAttachMode AttachMode = BoneTextAttachMode.HumanoidBone;

        [DataInput]
        [Label("본 (HumanBodyBones)")]
        [Description("부착 대상이 Character 일 때만 사용합니다.")]
        [HiddenIf(nameof(HideHumanoidBoneField))]
        public HumanBodyBones TargetBone = HumanBodyBones.Head;

        [DataInput]
        [Label("트랜스폼 경로")]
        [Description(
            "부착 대상 루트 기준 계층 경로 (예: Armature/Hips/Spine/Chest/Neck/Head). Root Transform = 에셋 루트."
        )]
        [HiddenIf(nameof(HideTransformPathField))]
        [AutoComplete(nameof(AutoCompleteAttachTransformPath), forceSelection: true)]
        public string BoneTransformPath = "";

        [DataInput]
        [Section("트랜스폼")]
        [Label("좌표계")]
        [Description(
            "부착 로컬 = 부모(본·에셋) 축 기준. 월드 = Unity 월드 X/Y/Z (Z=앞뒤, Y=위아래). 부모가 움직이면 월드 모드에서도 함께 따라갑니다."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public TextDisplayTransformSpace68 TransformSpace = TextDisplayTransformSpace68.AttachLocal;

        [DataInput]
        [Label("위치")]
        [Description("좌표계에 따라 부착 로컬 또는 월드 오프셋입니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public Vector3 LocalPosition;

        [DataInput]
        [Label("회전 (오일러)")]
        [Description("좌표계에 따라 부착 로컬 또는 월드 회전입니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public Vector3 LocalEulerAngles;

        [DataInput]
        [Label("스케일")]
        [Description(
            "TMP(SDF)는 스케일 변화에 비교적 강하지만, 우선 「글자 크기」로 맞추는 편이 안정적입니다."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public Vector3 LocalScale = Vector3.one;

        [DataInput]
        [Section("텍스트")]
        [Label("내용")]
        [MultilineInput]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public string BodyText = "Text";

        /// <summary>구버전 씬 호환. 폰트는 <see cref="FixedTmpFontResourcesPath"/> 로 고정됩니다.</summary>
        [DataInput]
        [Hidden]
        public string TmpFontResourcesPath = FixedTmpFontResourcesPath;

        [DataInput]
        [Hidden]
        public string UnityFontResourcesPath = "";

        [DataInput]
        [Section("텍스트 박스")]
        [Label("너비")]
        [FloatSlider(0.5f, 100f)]
        [Description("이 너비를 넘기면 줄바꿈합니다. 공백 있으면 단어 단위, 없으면 글자 단위입니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public float TextBoxWidth = 10f;

        [DataInput]
        [Label("최대 줄 수")]
        [IntegerSlider(1, 20)]
        [Description("초과분은 마지막 줄 끝에 ... 을 붙입니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public int MaxLines = 2;

        [DataInput]
        [Label("텍스트 박스 표시")]
        [Description("켜면 TextBox 너비 × 최대 줄 수 영역을 와이어 프레임으로 표시합니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool ShowTextBoxGuide;

        [DataInput]
        [Label("가이드 색")]
        [HiddenIf(nameof(HideTextBoxGuideFields))]
        public Color TextBoxGuideColor = new Color(0.25f, 0.9f, 1f, 0.95f);

        [DataInput]
        [Label("가이드 선 두께")]
        [FloatSlider(0.001f, 0.2f)]
        [HiddenIf(nameof(HideTextBoxGuideFields))]
        public float TextBoxGuideLineWidth = 0.015f;

        [DataInput]
        [Label("글자 크기")]
        [FloatSlider(0.05f, 48f)]
        [Description(
            "TMP fontSize (실수). 다른 TMP UI·툴처럼 1~6 등 작은 값도 사용할 수 있습니다."
        )]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public float FontSize = 4f;

        [DataInput]
        [Label("색")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public Color TextColor = Color.white;

        [DataInput]
        [Section("그라데이션 (4코너)")]
        [Label("Enable Gradient")]
        [Description("TMP VertexGradient 만 사용합니다. 인스펙터의 colorMode·그라데이션 프리셋 API 는 Warudo DataInput 으로 노출되지 않아, 사각형 네 모서리 색 고정 방식만 지원합니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool EnableVertexGradient;

        [DataInput]
        [Label("Gradient · 왼쪽 위")]
        [HiddenIf(nameof(HideGradientCornerFields))]
        public Color GradientTopLeft = Color.white;

        [DataInput]
        [Label("Gradient · 오른쪽 위")]
        [HiddenIf(nameof(HideGradientCornerFields))]
        public Color GradientTopRight = Color.white;

        [DataInput]
        [Label("Gradient · 왼쪽 아래")]
        [HiddenIf(nameof(HideGradientCornerFields))]
        public Color GradientBottomLeft = Color.white;

        [DataInput]
        [Label("Gradient · 오른쪽 아래")]
        [HiddenIf(nameof(HideGradientCornerFields))]
        public Color GradientBottomRight = Color.white;

        [DataInput]
        [Label("폰트 스타일")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public FontStyles TmpFontStyle = FontStyles.Normal;

        [DataInput]
        [Label("정렬")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public TextAlignmentOptions TmpAlignment = TextAlignmentOptions.Center;

        [DataInput]
        [Label("Rich Text")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool RichText = true;

        [DataInput]
        [Hidden]
        public TextOverflowModes OverflowMode = TextOverflowModes.Overflow;

        [DataInput]
        [Section("아웃라인")]
        [Label("아웃라인")]
        [Description("TMP SDF 아웃라인(테두리). 두께는 쉐이더 기준 0~1 입니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool UseOutline;

        [DataInput]
        [Label("아웃라인 두께")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideOutlineFields))]
        public float OutlineThickness = 0.22f;

        [DataInput]
        [Label("아웃라인 색")]
        [HiddenIf(nameof(HideOutlineFields))]
        public Color OutlineColor = new Color(0.08f, 0.02f, 0.15f, 1f);

        [DataInput]
        [Label("아웃라인 부드러움")]
        [FloatSlider(0f, 1f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "숫자가 클수록 테두리가 더 번집니다.")]
        [HiddenIf(nameof(HideOutlineFields))]
        public float OutlineSoftness = 0.12f;

        [DataInput]
        [Section("글로우 (TMP SDF)")]
        [Label("글로우")]
        [Description("TextMeshPro/Distance Field 머티리얼의 GLOW_ON 과 동일합니다. 모바일/경량 SDF 셰이더에는 글로우 속성이 없을 수 있습니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool UseGlow;

        [DataInput]
        [Label("글로우 색")]
        [HiddenIf(nameof(HideGlowFields))]
        public Color GlowColor = Color.white;

        [DataInput]
        [Label("글로우 오프셋")]
        [FloatSlider(-0.25f, 0.25f)]
        [HiddenIf(nameof(HideGlowFields))]
        public float GlowOffset;

        [DataInput]
        [Label("글로우 Inner")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideGlowFields))]
        public float GlowInner = 0.05f;

        [DataInput]
        [Label("글로우 Outer")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideGlowFields))]
        public float GlowOuter = 0.05f;

        [DataInput]
        [Label("글로우 Power")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HideGlowFields))]
        public float GlowPower = 0.75f;

        [DataInput]
        [Section("빌보드")]
        [Label("카메라를 향해 회전")]
        [Description("켜면 매 프레임 텍스트가 지정 카메라를 향합니다 (본 로컬 회전은 덮어씁니다).")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool BillboardTowardCamera;

        [DataInput]
        [Label("기준 카메라")]
        [HiddenIf(nameof(HideBillboardCameraField))]
        public CameraAsset ViewCamera;

        [DataInput]
        [Label("카메라 정렬 추가 회전 (오일러)")]
        [HiddenIf(nameof(HideBillboardCameraField))]
        public Vector3 BillboardEulerOffset;

        [DataInput]
        [Section("떠다니는 모션")]
        [Label("모션 사용")]
        [Description("위·회전 좌표계(트랜스폼)와 같습니다. 시간에 따라 위치·(선택)회전·스케일을 흔듭니다.")]
        [HiddenIf(nameof(HideShareDevOnlyFields))]
        public bool MotionEnabled;

        [DataInput]
        [Label("웨이브 형태")]
        [HiddenIf(nameof(HideMotionFields))]
        public TextDisplayMotionShape MotionShape = TextDisplayMotionShape.Sine;

        [DataInput]
        [Label("속도 (Hz)")]
        [FloatSlider(0.05f, 3f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "초당 왕복 횟수에 가깝게 동작합니다.")]
        [HiddenIf(nameof(HideMotionFields))]
        public float MotionSpeed = 0.85f;

        [DataInput]
        [Label("위치 변위")]
        [Description(
            CustomAssetsFlavorEmbedded.ShareBuild ? ""
                : "축마다 이 거리(미터)만큼 왕복합니다. 좌표계가 월드면 Y=위아래, Z=앞뒤입니다."
        )]
        [HiddenIf(nameof(HideMotionFields))]
        public Vector3 MotionPositionAmplitude = new Vector3(0.02f, 0.07f, 0.015f);

        [DataInput]
        [Label("회전 흔들림 (도)")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "축마다 사인으로 비틀립니다.「카메라를 향해 회전」이 켜져 있으면 회전 모션은 넣지 않습니다.")]
        [HiddenIf(nameof(HideMotionFields))]
        public Vector3 MotionRotationAmplitude;

        [DataInput]
        [Label("스케일 호흡")]
        [FloatSlider(0f, 0.35f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "0이면 끔. 1 근처에서 살짝 커졌다 작아지는 박동입니다.")]
        [HiddenIf(nameof(HideMotionFields))]
        public float MotionScalePulse;

        [DataInput]
        [Label("호흡 속도 배율")]
        [FloatSlider(0.25f, 3f)]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "스케일 박동만 속도에 곱합니다. 1이면 위치와 비슷한 리듬.")]
        [HiddenIf(nameof(HideMotionFields))]
        public float MotionScaleSpeedFactor = 1.15f;

        [DataInput]
        [Label("시간 위상 (초)")]
        [Description(CustomAssetsFlavorEmbedded.ShareBuild ? "" : "여러 텍스트가 같이 움직이지 않게 시작 타이밍만 밉니다.")]
        [HiddenIf(nameof(HideMotionFields))]
        public float MotionTimeOffset;

        private TextMeshPro _tmp;

        /// <summary><see cref="TextDisplayAnimateNode68"/> 등에서 트윈 값을 패널 스펙 위에 합성합니다.</summary>
        private float _overlayAnimAlpha = 1f;

        private Vector3 _overlayAnimLocalOffset;

        private Vector3 _overlayAnimScaleMul = Vector3.one;

        private Vector3 _overlayAnimEuler;

        /// <summary>-1 이면 TMP 기본(전체 표시), 0 이상이면 <see cref="TMP_Text.maxVisibleCharacters"/> 로 타자기.</summary>
        private int _overlayTypewriterCap = -1;

        private Color _overlayColorMul = Color.white;

        private string _appliedBodyText;
        private float _appliedTextBoxWidth = float.NaN;
        private int _appliedMaxLines = -1;
        private Color _appliedColor = Color.clear;
        private float _appliedFontSize = float.NaN;
        private TextAlignmentOptions? _appliedAlignment;
        private FontStyles? _appliedFontStyle;
        private bool? _appliedRichText;
        private TMP_FontAsset _appliedFontAssetRef;
        private Transform _lastParent;
        private LineRenderer _textBoxGuideLine;
        private Material _textBoxGuideMaterial;

        private bool _appliedUseOutline;
        private float _appliedOutlineThickness = float.NaN;
        private Color _appliedOutlineColor = new Color(0, 0, 0, 0);
        private float _appliedOutlineSoftness = float.NaN;

        private bool _appliedUseGlow;
        private Color _appliedGlowColor = new Color(0, 0, 0, 0);
        private float _appliedGlowOffset = float.NaN;
        private float _appliedGlowInner = float.NaN;
        private float _appliedGlowOuter = float.NaN;
        private float _appliedGlowPower = float.NaN;

        private bool? _appliedEnableVertexGradient;
        private Color _appliedGradientTopLeft = Color.clear;
        private Color _appliedGradientTopRight = Color.clear;
        private Color _appliedGradientBottomLeft = Color.clear;
        private Color _appliedGradientBottomRight = Color.clear;

        /// <summary>CreateFontAsset 으로 만든 런타임 전용 TMP 에셋 (Resources 에서 로드한 Font 용).</summary>
        private TMP_FontAsset _runtimeTmpFontAsset;

        private Font _runtimeFontSourceRef;

        private string _cachedTmpFontResourcesPath;
        private TMP_FontAsset _cachedTmpFontFromResources;

        private string _cachedUnityFontResourcesPath;
        private Font _cachedUnityFontFromResources;

        private void RefreshResourcesFontCachesForEmptyPaths()
        {
            if (string.IsNullOrWhiteSpace(UnityFontResourcesPath))
            {
                _cachedUnityFontResourcesPath = null;
                _cachedUnityFontFromResources = null;
            }
        }

        private static void MergeTmpFallbackChain(TMP_FontAsset primary)
        {
            if (primary == null)
                return;

            if (primary.fallbackFontAssetTable == null)
                primary.fallbackFontAssetTable = new List<TMP_FontAsset>();

            void AddUnique(TMP_FontAsset fb)
            {
                if (fb == null || fb == primary)
                    return;
                if (!primary.fallbackFontAssetTable.Contains(fb))
                    primary.fallbackFontAssetTable.Add(fb);
            }

            AddUnique(TMP_Settings.defaultFontAsset);
            var globalFallbacks = TMP_Settings.fallbackFontAssets;
            if (globalFallbacks != null)
            {
                for (var i = 0; i < globalFallbacks.Count; i++)
                    AddUnique(globalFallbacks[i]);
            }
        }

        private static void TryWarmGlyphsForText(TMP_FontAsset fa, string text)
        {
            if (fa == null || string.IsNullOrEmpty(text))
                return;

            if (fa.atlasPopulationMode == AtlasPopulationMode.Static)
                return;

            try
            {
                if (!fa.TryAddCharacters(text, out var missing) && !string.IsNullOrEmpty(missing))
                    Debug.LogWarning("[Node68 TextDisplay] 폰트에 없어 빠진 문자: " + missing);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Node68 TextDisplay] TryAddCharacters 실패: " + ex.Message);
            }
        }

        /// <summary>UMod 모드 스크립트는 <see cref="System.IO"/> 사용이 금지되어 순수 문자열로 처리합니다.</summary>
        private static string ModFileNameOnly(string pathOrName)
        {
            if (string.IsNullOrEmpty(pathOrName))
                return pathOrName;
            var s = pathOrName.Replace('\\', '/');
            var i = s.LastIndexOf('/');
            return i >= 0 ? s[(i + 1)..] : s;
        }

        private static string ModExtensionWithDot(string pathOrName)
        {
            var name = ModFileNameOnly(pathOrName);
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            var dot = name.LastIndexOf('.');
            return dot < 0 ? string.Empty : name.Substring(dot);
        }

        private static string ModFileNameWithoutExtension(string pathOrName)
        {
            var name = ModFileNameOnly(pathOrName);
            if (string.IsNullOrEmpty(name))
                return name;
            var dot = name.LastIndexOf('.');
            return dot < 0 ? name : name[..dot];
        }

        private static bool ModAssetBaseNameMatches(string entryName, string baseName)
        {
            if (string.IsNullOrEmpty(entryName))
                return false;
            if (string.Equals(entryName, baseName, StringComparison.OrdinalIgnoreCase))
                return true;
            var ext = ModExtensionWithDot(entryName);
            if (
                ext.Equals(".asset", StringComparison.OrdinalIgnoreCase)
                || ext.Equals(".ttf", StringComparison.OrdinalIgnoreCase)
                || ext.Equals(".otf", StringComparison.OrdinalIgnoreCase)
            )
            {
                var noExt = ModFileNameWithoutExtension(entryName);
                return string.Equals(noExt, baseName, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Warudo .warudo(UMod) 모드는 Unity <see cref="Resources"/> 에 합쳐지지 않습니다.
        /// 패널에 적는 경로는 기존과 같고, <see cref="ModHost.Assets"/> 에서도 같은 논리 경로로 찾습니다.
        /// </summary>
        private TMP_FontAsset TryLoadTmpFontFromModHost(ModHost host, string panelPath)
        {
            if (host == null || !host.IsModLoaded || host.Assets == null)
                return null;

            var p = panelPath.Replace('\\', '/').Trim();
            if (p.EndsWith(".asset", StringComparison.OrdinalIgnoreCase))
                p = p[..^6];

            var slash = p.LastIndexOf('/');
            var folderPart = slash >= 0 ? p[..slash] : "";
            var baseName = slash >= 0 ? p[(slash + 1)..] : p;

            try
            {
                var folders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (!string.IsNullOrEmpty(folderPart))
                {
                    folders.Add(folderPart);
                    folders.Add("Resources/" + folderPart);
                }

                folders.Add("Resources/Fonts");
                folders.Add("Fonts");

                foreach (var folder in folders)
                {
                    foreach (
                        var entry in host.Assets.FindAllInFolderWithExtension(folder, ".asset")
                    )
                    {
                        if (!ModAssetBaseNameMatches(entry.Name, baseName))
                            continue;
                        var fa = entry.Load<TMP_FontAsset>();
                        if (fa != null)
                            return fa;
                    }
                }

                foreach (var entry in host.Assets.FindAllWithName(baseName))
                {
                    var fa = entry.Load<TMP_FontAsset>();
                    if (fa != null)
                        return fa;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Node68 TextDisplay] ModHost TMP 폰트 검색 실패: " + ex.Message);
            }

            return null;
        }

        private TMP_FontAsset TryLoadTmpFontFromResources(string path)
        {
            path = path.Trim();
            if (_cachedTmpFontResourcesPath == path && _cachedTmpFontFromResources != null)
                return _cachedTmpFontFromResources;

            var asset = Resources.Load<TMP_FontAsset>(path);
            if (asset == null)
                asset = TryLoadTmpFontFromModHost(Plugin?.ModHost, path);

            _cachedTmpFontResourcesPath = path;
            _cachedTmpFontFromResources = asset;
            if (asset == null)
            {
                Debug.LogWarning(
                    "[Node68 TextDisplay] Resources/ModHost 에서 TMP Font Asset 을 불러오지 못했습니다: '"
                        + path
                        + "'. 모드(.warudo)에 폰트 에셋이 포함됐는지, 경로(확장자 없음)가 맞는지 확인하세요. Build Mod 후 Warudo 재시작."
                );
            }
            else if (Node68TextDisplayFontLoadDiagnostics.LoggedOkPaths.Add(path))
            {
                Debug.Log(
                    "[Node68 TextDisplay] TMP Font Asset 로드 성공: '" + path + "' → " + asset.name
                );
            }

            return asset;
        }

        /// <summary>Unity Font 는 UMod 허용 범위에서 Resources.Load 만 사용합니다.</summary>
        private Font TryLoadUnityFontFromModHost(ModHost host, string panelPath)
        {
            if (host == null || !host.IsModLoaded || host.Assets == null)
                return null;

            var p = panelPath.Replace('\\', '/').Trim();
            if (p.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase))
                p = p[..^4];
            if (p.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
                p = p[..^4];

            var slash = p.LastIndexOf('/');
            var folderPart = slash >= 0 ? p[..slash] : "";
            var baseName = slash >= 0 ? p[(slash + 1)..] : p;

            try
            {
                var folders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (!string.IsNullOrEmpty(folderPart))
                {
                    folders.Add(folderPart);
                    folders.Add("Resources/" + folderPart);
                }

                folders.Add("Resources/Fonts");
                folders.Add("Fonts");

                foreach (var ext in new[] { ".ttf", ".otf" })
                {
                    foreach (var folder in folders)
                    {
                        foreach (var entry in host.Assets.FindAllInFolderWithExtension(folder, ext))
                        {
                            if (!ModAssetBaseNameMatches(entry.Name, baseName))
                                continue;
                            var f = entry.Load<Font>();
                            if (f != null)
                                return f;
                        }
                    }
                }

                foreach (var entry in host.Assets.FindAllWithName(baseName))
                {
                    var f = entry.Load<Font>();
                    if (f != null)
                        return f;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[Node68 TextDisplay] ModHost Unity Font 검색 실패: " + ex.Message
                );
            }

            return null;
        }

        private Font TryResolveUnityFont(string pathFromPanel)
        {
            pathFromPanel = pathFromPanel.Trim();

            if (_cachedUnityFontResourcesPath == pathFromPanel)
                return _cachedUnityFontFromResources;

            _cachedUnityFontResourcesPath = pathFromPanel;
            var font = Resources.Load<Font>(pathFromPanel);
            if (font == null)
                font = TryLoadUnityFontFromModHost(Plugin?.ModHost, pathFromPanel);

            _cachedUnityFontFromResources = font;
            if (font == null)
            {
                Debug.LogWarning(
                    "[Node68 TextDisplay] Resources/ModHost 에서 Unity Font 를 불러오지 못했습니다: '"
                        + pathFromPanel
                        + "'."
                );
            }

            return font;
        }

        private TMP_FontAsset ResolveTmpFont()
        {
            RefreshResourcesFontCachesForEmptyPaths();

            var tmpPath = FixedTmpFontResourcesPath;
            DisposeRuntimeTmpFontAsset();
            var loaded = TryLoadTmpFontFromResources(tmpPath);
            if (loaded != null)
                return loaded;

            var fallback = TMP_Settings.defaultFontAsset;
            if (
                fallback != null
                && Node68TextDisplayFontLoadDiagnostics.LoggedFallbackPaths.Add(tmpPath)
            )
            {
                Debug.LogWarning(
                    "[Node68 TextDisplay] TMP 경로 '"
                        + tmpPath
                        + "' 로드 실패 — TMP 기본 폰트로 대체합니다: "
                        + fallback.name
                        + " (한글이 기본 폰트에 없으면 네모/빈 글자로 보일 수 있습니다)."
                );
            }

            return fallback;
        }

        private void DisposeRuntimeTmpFontAsset()
        {
            if (_runtimeTmpFontAsset == null)
                return;
            var mat = _runtimeTmpFontAsset.material;
            if (mat != null)
                UnityEngine.Object.Destroy(mat);
            UnityEngine.Object.Destroy(_runtimeTmpFontAsset);
            _runtimeTmpFontAsset = null;
            _runtimeFontSourceRef = null;
        }

        private bool HideInShareBuild() => CustomAssetsBuildRuntime.IsShareBuild();

        private bool HideShareDevOnlyFields() => HideInShareBuild();

        private bool IsCharacterTarget() => AttachTarget is CharacterAsset;

        private bool HideHumanoidBoneField() =>
            AttachMode != BoneTextAttachMode.HumanoidBone || !IsCharacterTarget();

        private bool HideTransformPathField() =>
            AttachMode != BoneTextAttachMode.TransformPath;

        private bool HideTextBoxGuideFields() => HideInShareBuild() || !ShowTextBoxGuide;

        private bool HideBillboardCameraField() => HideInShareBuild() || !BillboardTowardCamera;

        private bool HideOutlineFields() => HideInShareBuild() || !UseOutline;

        private bool HideGlowFields() => HideInShareBuild() || !UseGlow;

        private bool HideGradientCornerFields() => HideInShareBuild() || !EnableVertexGradient;

        private bool HideMotionFields() => HideInShareBuild() || !MotionEnabled;

        private static float EvalMotionOscillator(
            float timeSeconds,
            float cyclesPerSecond,
            TextDisplayMotionShape shape
        )
        {
            if (cyclesPerSecond <= 0f)
                return 0f;
            switch (shape)
            {
                case TextDisplayMotionShape.SmoothPingPong:
                {
                    var p = Mathf.PingPong(timeSeconds * cyclesPerSecond, 1f);
                    return Mathf.Lerp(-1f, 1f, Mathf.SmoothStep(0f, 1f, p));
                }
                default:
                    return Mathf.Sin(timeSeconds * cyclesPerSecond * (Mathf.PI * 2f));
            }
        }

        private void ApplyTmpOutlineEffects()
        {
            if (_tmp == null)
                return;

            var ow = UseOutline ? Mathf.Clamp01(OutlineThickness) : 0f;
            _tmp.outlineWidth = ow;
            var ocol = OutlineColor;
            ocol.a *= _overlayAnimAlpha;
            _tmp.outlineColor = ocol;

            Material mat = null;
            try
            {
                mat = _tmp.fontMaterial;
            }
            catch
            {
                return;
            }

            if (mat == null)
                return;

            if (mat.HasProperty(ShaderUtilities.ID_OutlineSoftness))
                mat.SetFloat(
                    ShaderUtilities.ID_OutlineSoftness,
                    UseOutline ? Mathf.Clamp01(OutlineSoftness) : 0f
                );

            ApplyTmpGlowToMaterial(mat);
        }

        private void ApplyTmpGlowToMaterial(Material mat)
        {
            if (mat == null || !mat.HasProperty(ShaderUtilities.ID_GlowColor))
                return;

            if (UseGlow)
            {
                mat.EnableKeyword(ShaderUtilities.Keyword_Glow);
                var gc = GlowColor;
                gc.a *= _overlayAnimAlpha;
                mat.SetColor(ShaderUtilities.ID_GlowColor, gc);
                if (mat.HasProperty(ShaderUtilities.ID_GlowOffset))
                    mat.SetFloat(ShaderUtilities.ID_GlowOffset, GlowOffset);
                if (mat.HasProperty(ShaderUtilities.ID_GlowInner))
                    mat.SetFloat(ShaderUtilities.ID_GlowInner, Mathf.Clamp01(GlowInner));
                if (mat.HasProperty(ShaderUtilities.ID_GlowOuter))
                    mat.SetFloat(ShaderUtilities.ID_GlowOuter, Mathf.Clamp01(GlowOuter));
                if (mat.HasProperty(ShaderUtilities.ID_GlowPower))
                    mat.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Clamp01(GlowPower));
            }
            else
                mat.DisableKeyword(ShaderUtilities.Keyword_Glow);
        }

        /// <summary>플로우 노드가 패널 값 위에 페이드·이동·스케일 트윈을 올릴 때 사용합니다.</summary>
        public void SetTextDisplayOverlayAnimation(
            Vector3 localOffset,
            Vector3 scaleMultiplier,
            float alphaMultiplier
        )
        {
            SetTextDisplayOverlayAnimation(
                localOffset,
                scaleMultiplier,
                alphaMultiplier,
                Vector3.zero,
                -1,
                Color.white
            );
        }

        /// <param name="typewriterVisibleCharacters">-1 이면 타자기 비활성(전체 글자), 0 이상이면 보이는 글자 수 상한.</param>
        /// <param name="colorRgbMultiplier">면색·그라데이션에 곱합니다 (1,1,1,1) 이 기본, 1 초과면 반짝(플래시)에 사용 가능.</param>
        public void SetTextDisplayOverlayAnimation(
            Vector3 localOffset,
            Vector3 scaleMultiplier,
            float alphaMultiplier,
            Vector3 localEulerAdd,
            int typewriterVisibleCharacters,
            Color colorRgbMultiplier
        )
        {
            _overlayAnimLocalOffset = localOffset;
            _overlayAnimScaleMul = new Vector3(
                Mathf.Max(1e-4f, scaleMultiplier.x),
                Mathf.Max(1e-4f, scaleMultiplier.y),
                Mathf.Max(1e-4f, scaleMultiplier.z)
            );
            _overlayAnimAlpha = Mathf.Clamp01(alphaMultiplier);
            _overlayAnimEuler = localEulerAdd;
            _overlayTypewriterCap = typewriterVisibleCharacters;
            _overlayColorMul = colorRgbMultiplier;
        }

        /// <summary>오버레이를 제거하고 패널 스펙만 쓰도록 되돌립니다.</summary>
        public void ClearTextDisplayOverlayAnimation()
        {
            _overlayAnimLocalOffset = Vector3.zero;
            _overlayAnimScaleMul = Vector3.one;
            _overlayAnimAlpha = 1f;
            _overlayAnimEuler = Vector3.zero;
            _overlayTypewriterCap = -1;
            _overlayColorMul = Color.white;
        }

        /// <summary>본 로컬 좌표만 사용하고, GameObjectAsset 기본 Transform 패널은 숨깁니다.</summary>
        protected override bool HideTransform() => true;

        protected override GameObject CreateGameObject()
        {
            var go = new GameObject("Node68BoneAttachedTMP");
            _tmp = go.AddComponent<TextMeshPro>();
            _tmp.enableWordWrapping = false;
            _tmp.overflowMode = TextOverflowModes.Overflow;
            _tmp.autoSizeTextContainer = false;
            return go;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            MigrateLegacyCharacterTarget();
            SetActive(true);
        }

        private void MigrateLegacyCharacterTarget()
        {
            if (AttachTarget == null && Character != null)
                AttachTarget = Character;
        }

        public override void OnLateUpdate()
        {
            if (GameObject == null)
                return;

            if (!Enabled || !Visible || AttachTarget == null)
            {
                DestroyVisual();
                return;
            }

            var parent = ResolveParentTransform();
            if (parent == null)
            {
                DestroyVisual();
                return;
            }

            EnsureVisual(parent);
            ApplyRootTransform();
            SyncTmpAppearance();

            if (BillboardTowardCamera)
                ApplyBillboard();
        }

        protected override void OnDestroy()
        {
            DisposeTextBoxGuide();
            DisposeRuntimeTmpFontAsset();
            _tmp = null;
            base.OnDestroy();
        }

        private Transform ResolveParentTransform()
        {
            try
            {
                var rootGo = AttachTarget?.GameObject;
                if (rootGo == null)
                    return null;

                var root = rootGo.transform;

                if (AttachMode == BoneTextAttachMode.AssetTransform)
                    return root;

                if (AttachMode == BoneTextAttachMode.HumanoidBone)
                {
                    if (AttachTarget is not CharacterAsset character)
                        return null;

                    var map = character.HumanBodyBoneToBodyTransforms;
                    if (map == null || map.Count == 0)
                        return null;
                    return map.TryGetValue(TargetBone, out var t) ? t : null;
                }

                var path = BoneTransformPath?.Trim();
                if (string.IsNullOrEmpty(path) || IsRootTransformPath(path))
                    return root;

                if (AttachTarget is CharacterAsset charAsset)
                    return charAsset.FindChildTransform(path);

                return root.Find(path);
            }
            catch
            {
                return null;
            }
        }

        private static bool IsRootTransformPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return true;
            return path.Trim().Equals(RootTransformPathLabel, StringComparison.OrdinalIgnoreCase);
        }

        private Transform ResolveAttachRootTransform()
        {
            var go = AttachTarget?.GameObject;
            return go != null ? go.transform : null;
        }

        private UniTask<AutoCompleteList> AutoCompleteAttachTransformPath()
        {
            var entries = new List<AutoCompleteEntry>
            {
                new AutoCompleteEntry
                {
                    label = RootTransformPathLabel,
                    value = string.Empty,
                },
            };

            var root = ResolveAttachRootTransform();
            if (root == null)
                return UniTask.FromResult(entries.ToAutoCompleteList());

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectAttachTransformPathEntries(root, root, entries, seen);
            entries.Sort((a, b) =>
                string.Compare(a.label, b.label, StringComparison.OrdinalIgnoreCase)
            );
            return UniTask.FromResult(entries.ToAutoCompleteList());
        }

        private static void CollectAttachTransformPathEntries(
            Transform root,
            Transform current,
            List<AutoCompleteEntry> entries,
            HashSet<string> seen
        )
        {
            foreach (Transform child in current)
            {
                var path = GetRelativeTransformPath(root, child);
                if (!string.IsNullOrEmpty(path) && seen.Add(path))
                {
                    entries.Add(
                        new AutoCompleteEntry
                        {
                            label = path,
                            value = path,
                        }
                    );
                }

                CollectAttachTransformPathEntries(root, child, entries, seen);
            }
        }

        private static string GetRelativeTransformPath(Transform root, Transform target)
        {
            if (root == null || target == null || target == root)
                return string.Empty;

            var stack = new Stack<string>();
            var current = target;
            while (current != null && current != root)
            {
                stack.Push(current.name);
                current = current.parent;
            }

            if (current != root)
                return string.Empty;

            return string.Join("/", stack);
        }

        private void EnsureVisual(Transform parent)
        {
            if (GameObject == null || parent == null)
                return;

            if (!GameObject.activeSelf)
                GameObject.SetActive(true);

            if (_tmp == null)
                _tmp = GameObject.GetComponent<TextMeshPro>();

            if (GameObject.transform.parent != parent || _lastParent != parent)
            {
                GameObject.transform.SetParent(parent, false);
                _lastParent = parent;
            }

            if (_tmp == null)
                return;

            _tmp.enableWordWrapping = false;
            _tmp.overflowMode = TextOverflowModes.Overflow;
            _tmp.autoSizeTextContainer = false;
            _tmp.rectTransform.sizeDelta = new Vector2(
                Mathf.Max(0.01f, TextBoxWidth),
                _tmp.rectTransform.sizeDelta.y
            );
            _tmp.richText = RichText;

            var font = ResolveTmpFont();
            if (font != null)
                _tmp.font = font;

            var mr = _tmp.renderer;
            if (mr != null)
            {
                mr.receiveShadows = false;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        private void ResetTmpApplyCache()
        {
            _appliedBodyText = null;
            _appliedTextBoxWidth = float.NaN;
            _appliedMaxLines = -1;
            _appliedColor = Color.clear;
            _appliedFontSize = float.NaN;
            _appliedAlignment = null;
            _appliedFontStyle = null;
            _appliedRichText = null;
            _appliedFontAssetRef = null;
            _appliedUseOutline = false;
            _appliedOutlineThickness = float.NaN;
            _appliedOutlineColor = new Color(0, 0, 0, 0);
            _appliedOutlineSoftness = float.NaN;
            _appliedUseGlow = false;
            _appliedGlowColor = new Color(0, 0, 0, 0);
            _appliedGlowOffset = float.NaN;
            _appliedGlowInner = float.NaN;
            _appliedGlowOuter = float.NaN;
            _appliedGlowPower = float.NaN;
            _appliedEnableVertexGradient = null;
            _appliedGradientTopLeft = Color.clear;
            _appliedGradientTopRight = Color.clear;
            _appliedGradientBottomLeft = Color.clear;
            _appliedGradientBottomRight = Color.clear;
        }

        private void ApplyRootTransform()
        {
            if (GameObject == null)
                return;

            var pos = LocalPosition;
            var rotEuler = LocalEulerAngles;
            var scale = LocalScale;

            if (MotionEnabled && MotionSpeed > 0f)
            {
                var t = Time.time + MotionTimeOffset;
                var spd = MotionSpeed;

                var ox = EvalMotionOscillator(t, spd * 0.93f, MotionShape);
                var oy = EvalMotionOscillator(t, spd, MotionShape);
                var oz = EvalMotionOscillator(t, spd * 1.07f, MotionShape);

                pos.x += MotionPositionAmplitude.x * ox;
                pos.y += MotionPositionAmplitude.y * oy;
                pos.z += MotionPositionAmplitude.z * oz;

                if (!BillboardTowardCamera && MotionRotationAmplitude.sqrMagnitude > 1e-10f)
                {
                    var rt = t * (Mathf.PI * 2f) * spd;
                    rotEuler.x += MotionRotationAmplitude.x * Mathf.Sin(rt * 1f);
                    rotEuler.y += MotionRotationAmplitude.y * Mathf.Sin(rt * 0.85f);
                    rotEuler.z += MotionRotationAmplitude.z * Mathf.Sin(rt * 1.1f);
                }

                if (MotionScalePulse > 0f)
                {
                    var pulse = EvalMotionOscillator(
                        t,
                        spd * Mathf.Max(0.05f, MotionScaleSpeedFactor),
                        MotionShape
                    );
                    var sm = 1f + MotionScalePulse * pulse;
                    scale = Vector3.Scale(scale, new Vector3(sm, sm, sm));
                }
            }

            pos += ResolveOverlayPositionOffset();
            scale = Vector3.Scale(scale, _overlayAnimScaleMul);
            rotEuler += _overlayAnimEuler;

            ApplyTransformPosition(pos);
            ApplyTransformRotation(rotEuler);
            GameObject.transform.localScale = scale;
        }

        private Vector3 ResolveOverlayPositionOffset()
        {
            if (_overlayAnimLocalOffset == Vector3.zero)
                return Vector3.zero;

            if (TransformSpace != TextDisplayTransformSpace68.World)
                return _overlayAnimLocalOffset;

            var parent = GameObject.transform.parent;
            return parent != null
                ? parent.rotation * _overlayAnimLocalOffset
                : _overlayAnimLocalOffset;
        }

        private void ApplyTransformPosition(Vector3 pos)
        {
            var tr = GameObject.transform;
            if (TransformSpace == TextDisplayTransformSpace68.World)
            {
                var parent = tr.parent;
                tr.position = parent != null ? parent.position + pos : pos;
                return;
            }

            tr.localPosition = pos;
        }

        private void ApplyTransformRotation(Vector3 rotEuler)
        {
            if (BillboardTowardCamera)
                return;

            var tr = GameObject.transform;
            if (TransformSpace == TextDisplayTransformSpace68.World)
                tr.rotation = Quaternion.Euler(rotEuler);
            else
                tr.localRotation = Quaternion.Euler(rotEuler);
        }

        private static Color32 ScaleColorAlpha(Color c, float alphaMultiplier)
        {
            c.a *= alphaMultiplier;
            return c;
        }

        private static Color MulColorRgb(Color baseColor, Color mul)
        {
            return new Color(
                baseColor.r * mul.r,
                baseColor.g * mul.g,
                baseColor.b * mul.b,
                baseColor.a * mul.a
            );
        }

        private void SyncTmpAppearance()
        {
            if (_tmp == null)
                return;

            var sourceText = BodyText ?? "";
            var fs = Mathf.Clamp(FontSize, 0.01f, 500f);
            var font = ResolveTmpFont();
            var boxWidth = Mathf.Max(0.01f, TextBoxWidth);
            var maxLines = Mathf.Max(1, MaxLines);

            TryWarmGlyphsForText(font, sourceText);

            var outTh = UseOutline ? Mathf.Clamp01(OutlineThickness) : 0f;
            var styleDirty =
                _appliedUseOutline != UseOutline
                || !Mathf.Approximately(_appliedOutlineThickness, outTh)
                || _appliedOutlineColor != OutlineColor
                || !Mathf.Approximately(_appliedOutlineSoftness, OutlineSoftness)
                || _appliedUseGlow != UseGlow
                || _appliedGlowColor != GlowColor
                || !Mathf.Approximately(_appliedGlowOffset, GlowOffset)
                || !Mathf.Approximately(_appliedGlowInner, GlowInner)
                || !Mathf.Approximately(_appliedGlowOuter, GlowOuter)
                || !Mathf.Approximately(_appliedGlowPower, GlowPower);

            bool dirty =
                _appliedBodyText != sourceText
                || !Mathf.Approximately(_appliedFontSize, fs)
                || !Mathf.Approximately(_appliedTextBoxWidth, boxWidth)
                || _appliedMaxLines != maxLines
                || _appliedColor != TextColor
                || _appliedEnableVertexGradient != EnableVertexGradient
                || _appliedGradientTopLeft != GradientTopLeft
                || _appliedGradientTopRight != GradientTopRight
                || _appliedGradientBottomLeft != GradientBottomLeft
                || _appliedGradientBottomRight != GradientBottomRight
                || _appliedFontAssetRef != font
                || _appliedAlignment != TmpAlignment
                || _appliedFontStyle != TmpFontStyle
                || _appliedRichText != RichText
                || styleDirty;

            _tmp.fontSize = fs;
            if (font != null)
                _tmp.font = font;
            _tmp.fontStyle = TmpFontStyle;
            _tmp.alignment = TmpAlignment;
            _tmp.richText = RichText;
            _tmp.overflowMode = TextOverflowModes.Overflow;
            _tmp.enableWordWrapping = false;
            _tmp.autoSizeTextContainer = false;
            _tmp.rectTransform.sizeDelta = new Vector2(boxWidth, _tmp.rectTransform.sizeDelta.y);

            var displayText =
                font != null
                    ? Node68TextBoxLayout68.Format(
                        sourceText,
                        boxWidth,
                        maxLines,
                        _tmp,
                        fs,
                        font,
                        TmpFontStyle
                    )
                    : sourceText;

            _tmp.text = displayText;
            var aMul = _overlayAnimAlpha;
            var cMul = _overlayColorMul;
            if (EnableVertexGradient)
            {
                _tmp.enableVertexGradient = true;
                _tmp.colorGradient = new VertexGradient(
                    ScaleColorAlpha(MulColorRgb(GradientTopLeft, cMul), aMul),
                    ScaleColorAlpha(MulColorRgb(GradientTopRight, cMul), aMul),
                    ScaleColorAlpha(MulColorRgb(GradientBottomLeft, cMul), aMul),
                    ScaleColorAlpha(MulColorRgb(GradientBottomRight, cMul), aMul)
                );
                var face = MulColorRgb(Color.white, cMul);
                face.a = aMul;
                _tmp.color = face;
            }
            else
            {
                _tmp.enableVertexGradient = false;
                var face = MulColorRgb(TextColor, cMul);
                face.a *= aMul;
                _tmp.color = face;
            }
            if (_overlayTypewriterCap >= 0)
                _tmp.maxVisibleCharacters = Mathf.Max(0, _overlayTypewriterCap);
            else
                _tmp.maxVisibleCharacters = 99999;

            ApplyTmpOutlineEffects();

            var mr = _tmp.renderer;
            if (mr != null)
            {
                mr.receiveShadows = false;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            if (dirty)
            {
                _appliedBodyText = sourceText;
                _appliedFontSize = fs;
                _appliedTextBoxWidth = boxWidth;
                _appliedMaxLines = maxLines;
                _appliedColor = TextColor;
                _appliedEnableVertexGradient = EnableVertexGradient;
                _appliedGradientTopLeft = GradientTopLeft;
                _appliedGradientTopRight = GradientTopRight;
                _appliedGradientBottomLeft = GradientBottomLeft;
                _appliedGradientBottomRight = GradientBottomRight;
                _appliedFontAssetRef = font;
                _appliedAlignment = TmpAlignment;
                _appliedFontStyle = TmpFontStyle;
                _appliedRichText = RichText;
                _appliedUseOutline = UseOutline;
                _appliedOutlineThickness = outTh;
                _appliedOutlineColor = OutlineColor;
                _appliedOutlineSoftness = OutlineSoftness;
                _appliedUseGlow = UseGlow;
                _appliedGlowColor = GlowColor;
                _appliedGlowOffset = GlowOffset;
                _appliedGlowInner = GlowInner;
                _appliedGlowOuter = GlowOuter;
                _appliedGlowPower = GlowPower;
                _tmp.ForceMeshUpdate(true);
                ApplyTmpOutlineEffects();
            }

            SyncTextBoxGuide(boxWidth, maxLines);
        }

        private void EnsureTextBoxGuideLine()
        {
            if (GameObject == null)
                return;

            if (_textBoxGuideLine == null)
            {
                var guideGo = new GameObject("Node68TextBoxGuide");
                guideGo.transform.SetParent(GameObject.transform, false);
                _textBoxGuideLine = guideGo.AddComponent<LineRenderer>();
                _textBoxGuideLine.useWorldSpace = false;
                _textBoxGuideLine.loop = true;
                _textBoxGuideLine.positionCount = 5;
                _textBoxGuideLine.numCornerVertices = 0;
                _textBoxGuideLine.numCapVertices = 0;
                _textBoxGuideLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                _textBoxGuideLine.receiveShadows = false;
                _textBoxGuideLine.alignment = LineAlignment.View;

                if (_textBoxGuideMaterial == null)
                {
                    var shader =
                        Shader.Find("Universal Render Pipeline/Unlit")
                        ?? Shader.Find("Unlit/Color")
                        ?? Shader.Find("Sprites/Default");
                    if (shader != null)
                        _textBoxGuideMaterial = new Material(shader);
                }

                if (_textBoxGuideMaterial != null)
                    _textBoxGuideLine.material = _textBoxGuideMaterial;
            }
        }

        private void SyncTextBoxGuide(float boxWidth, int maxLines)
        {
            if (!ShowTextBoxGuide || _tmp == null || GameObject == null)
            {
                SetTextBoxGuideVisible(false);
                return;
            }

            EnsureTextBoxGuideLine();
            if (_textBoxGuideLine == null)
                return;

            SetTextBoxGuideVisible(true);

            var height = ResolveTextBoxGuideHeight(boxWidth, maxLines);
            var halfW = boxWidth * 0.5f;
            var halfH = height * 0.5f;
            var yOffset = ResolveTextBoxGuideVerticalOffset(height);

            var c = TextBoxGuideColor;
            _textBoxGuideLine.startColor = c;
            _textBoxGuideLine.endColor = c;
            _textBoxGuideLine.startWidth = TextBoxGuideLineWidth;
            _textBoxGuideLine.endWidth = TextBoxGuideLineWidth;

            _textBoxGuideLine.SetPosition(0, new Vector3(-halfW, yOffset - halfH, 0f));
            _textBoxGuideLine.SetPosition(1, new Vector3(halfW, yOffset - halfH, 0f));
            _textBoxGuideLine.SetPosition(2, new Vector3(halfW, yOffset + halfH, 0f));
            _textBoxGuideLine.SetPosition(3, new Vector3(-halfW, yOffset + halfH, 0f));
            _textBoxGuideLine.SetPosition(4, new Vector3(-halfW, yOffset - halfH, 0f));
        }

        private float ResolveTextBoxGuideHeight(float boxWidth, int maxLines)
        {
            if (_tmp == null)
                return 0.01f;

            maxLines = Mathf.Max(1, maxLines);
            var oneLine = ResolveOneLineHeight(boxWidth);
            return Mathf.Max(0.01f, oneLine * maxLines);
        }

        private float ResolveOneLineHeight(float boxWidth)
        {
            var single = _tmp.GetPreferredValues("Ay", boxWidth, 0f).y;
            var dual = _tmp.GetPreferredValues("Ay\nAy", boxWidth, 0f).y;
            if (dual > single + 1e-5f)
                return dual - single;
            return Mathf.Max(0.01f, single);
        }

        private float ResolveTextBoxGuideVerticalOffset(float boxHeight)
        {
            if (_tmp == null)
                return 0f;

            var align = TmpAlignment;
            if (IsTopAligned(align))
                return boxHeight * 0.5f - ResolveOneLineHeight(TextBoxWidth) * 0.5f;
            if (IsBottomAligned(align))
                return -(boxHeight * 0.5f - ResolveOneLineHeight(TextBoxWidth) * 0.5f);
            return 0f;
        }

        private static bool IsTopAligned(TextAlignmentOptions align)
        {
            return align == TextAlignmentOptions.TopLeft
                || align == TextAlignmentOptions.Top
                || align == TextAlignmentOptions.TopRight
                || align == TextAlignmentOptions.TopJustified
                || align == TextAlignmentOptions.TopGeoAligned
                || align == TextAlignmentOptions.TopFlush
                || align == TextAlignmentOptions.CaplineLeft
                || align == TextAlignmentOptions.Capline
                || align == TextAlignmentOptions.CaplineRight
                || align == TextAlignmentOptions.CaplineGeoAligned
                || align == TextAlignmentOptions.CaplineFlush;
        }

        private static bool IsBottomAligned(TextAlignmentOptions align)
        {
            return align == TextAlignmentOptions.BottomLeft
                || align == TextAlignmentOptions.Bottom
                || align == TextAlignmentOptions.BottomRight
                || align == TextAlignmentOptions.BottomJustified
                || align == TextAlignmentOptions.BottomGeoAligned
                || align == TextAlignmentOptions.BottomFlush
                || align == TextAlignmentOptions.BaselineLeft
                || align == TextAlignmentOptions.Baseline
                || align == TextAlignmentOptions.BaselineRight
                || align == TextAlignmentOptions.BaselineGeoAligned
                || align == TextAlignmentOptions.BaselineFlush;
        }

        private void SetTextBoxGuideVisible(bool visible)
        {
            if (_textBoxGuideLine != null)
                _textBoxGuideLine.gameObject.SetActive(visible);
        }

        private void DisposeTextBoxGuide()
        {
            if (_textBoxGuideLine != null)
            {
                UnityEngine.Object.Destroy(_textBoxGuideLine.gameObject);
                _textBoxGuideLine = null;
            }

            if (_textBoxGuideMaterial != null)
            {
                UnityEngine.Object.Destroy(_textBoxGuideMaterial);
                _textBoxGuideMaterial = null;
            }
        }

        private void ApplyBillboard()
        {
            if (GameObject == null)
                return;

            Camera cam = null;
            if (ViewCamera != null)
                cam = ViewCamera.Camera;

            if (cam == null)
                cam = Camera.main;

            if (cam == null)
                return;

            var camTr = cam.transform;
            var toCam = camTr.position - GameObject.transform.position;
            if (toCam.sqrMagnitude < 1e-8f)
                return;

            var look = Quaternion.LookRotation(-toCam.normalized, camTr.up);
            GameObject.transform.rotation = look * Quaternion.Euler(BillboardEulerOffset);
        }

        private void DestroyVisual()
        {
            _lastParent = null;
            SetTextBoxGuideVisible(false);
            if (GameObject != null)
            {
                GameObject.SetActive(false);
                GameObject.transform.SetParent(null, false);
            }

            ResetTmpApplyCache();
            ClearTextDisplayOverlayAnimation();
        }
    }

    /// <summary>
    /// TextDisplay Node68 — TextBox 너비·최대 줄 수 기준 수동 줄바꿈·말줄임.
    /// </summary>
    internal static class Node68TextBoxLayout68
    {
        private const string Ellipsis = "...";

        public static string Format(
            string input,
            float maxWidth,
            int maxLines,
            TextMeshPro measureTmp,
            float fontSize,
            TMP_FontAsset font,
            FontStyles fontStyle
        )
        {
            if (string.IsNullOrEmpty(input))
                return input ?? string.Empty;

            maxLines = Mathf.Max(1, maxLines);
            maxWidth = Mathf.Max(0.01f, maxWidth);

            measureTmp.font = font;
            measureTmp.fontSize = fontSize;
            measureTmp.fontStyle = fontStyle;
            measureTmp.enableWordWrapping = false;

            float Measure(string s) =>
                string.IsNullOrEmpty(s) ? 0f : measureTmp.GetPreferredValues(s, 0, 0).x;

            var normalized = input.Replace("\r\n", "\n").Replace("\r", "\n");
            var paragraphs = normalized.Split('\n');
            var allLines = new List<string>();

            foreach (var paragraph in paragraphs)
            {
                var wrapped = ContainsSpace(paragraph)
                    ? WrapParagraphByWords(paragraph, maxWidth, Measure)
                    : WrapParagraphByChars(paragraph, maxWidth, Measure);
                allLines.AddRange(wrapped);
            }

            if (allLines.Count <= maxLines)
                return string.Join("\n", allLines);

            var head = allLines.GetRange(0, maxLines - 1);
            var tailJoin = ContainsSpace(normalized) ? " " : string.Empty;
            var lastLine = allLines[maxLines - 1];
            var overflow = string.Join(
                tailJoin,
                allLines.GetRange(maxLines, allLines.Count - maxLines)
            );
            var combined = string.IsNullOrEmpty(overflow)
                ? lastLine
                : lastLine + tailJoin + overflow;
            head.Add(TruncateWithEllipsis(combined, maxWidth, Measure));
            return string.Join("\n", head);
        }

        private static bool ContainsSpace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            for (var i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                    return true;
            }

            return false;
        }

        private static List<string> WrapParagraphByWords(
            string text,
            float maxWidth,
            Func<string, float> measure
        )
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(text))
            {
                lines.Add(string.Empty);
                return lines;
            }

            var index = 0;
            while (index < text.Length)
            {
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
                if (index >= text.Length)
                    break;

                var line = string.Empty;
                while (index < text.Length)
                {
                    while (index < text.Length && char.IsWhiteSpace(text[index]))
                        index++;
                    if (index >= text.Length)
                        break;

                    var wordStart = index;
                    while (index < text.Length && !char.IsWhiteSpace(text[index]))
                        index++;
                    var word = text.Substring(wordStart, index - wordStart);

                    var candidate = string.IsNullOrEmpty(line) ? word : line + " " + word;
                    if (measure(candidate) <= maxWidth)
                    {
                        line = candidate;
                        continue;
                    }

                    if (string.IsNullOrEmpty(line))
                    {
                        lines.AddRange(WrapParagraphByChars(word, maxWidth, measure));
                        line = string.Empty;
                    }
                    else
                    {
                        index = wordStart;
                    }

                    break;
                }

                if (!string.IsNullOrEmpty(line))
                    lines.Add(line);
            }

            if (lines.Count == 0)
                lines.Add(string.Empty);

            return lines;
        }

        private static List<string> WrapParagraphByChars(
            string text,
            float maxWidth,
            Func<string, float> measure
        )
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(text))
            {
                lines.Add(string.Empty);
                return lines;
            }

            var index = 0;
            while (index < text.Length)
            {
                var lineLength = 0;
                while (index + lineLength < text.Length)
                {
                    var candidate = text.Substring(index, lineLength + 1);
                    if (measure(candidate) <= maxWidth)
                    {
                        lineLength++;
                        continue;
                    }

                    break;
                }

                if (lineLength == 0)
                    lineLength = 1;

                lines.Add(text.Substring(index, lineLength));
                index += lineLength;
            }

            return lines;
        }

        private static string TruncateWithEllipsis(
            string text,
            float maxWidth,
            Func<string, float> measure
        )
        {
            if (string.IsNullOrEmpty(text))
                return Ellipsis;

            if (measure(text) <= maxWidth)
                return text;

            if (measure(Ellipsis) > maxWidth)
                return Ellipsis;

            var lo = 0;
            var hi = text.Length;
            while (lo < hi)
            {
                var mid = (lo + hi + 1) / 2;
                if (measure(text.Substring(0, mid) + Ellipsis) <= maxWidth)
                    lo = mid;
                else
                    hi = mid - 1;
            }

            return lo <= 0 ? Ellipsis : text.Substring(0, lo) + Ellipsis;
        }
    }
}
