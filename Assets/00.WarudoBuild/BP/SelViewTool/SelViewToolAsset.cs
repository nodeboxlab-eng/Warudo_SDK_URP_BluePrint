using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Node68.ToolkitMods.SelViewTool;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Resource;
using Warudo.Core.Scenes;
using Warudo.Core.Server;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;
using Warudo.Plugins.Core.Events;

[AssetType(
    Id = "f4a1e2d3-6b7c-8d9e-0f1a-2b3c4d5e6f70",
    Title = "SelView Tool",
    Category =
        BpToolkitFlavorEmbedded.ShareBuild ? BpToolkitCategories.Share : BpToolkitCategories.Toolkit
)]
public class SelViewToolAsset : Asset
{
    // ───────────────────────── 기본 설정 ─────────────────────────

    [DataInput]
    [Label("활성화")]
    [Description("끄면 모든 단축키가 비활성화됩니다.")]
    public bool Enabled = true;

    [DataInput]
    [Label("캐릭터")]
    [Description("포즈/표정을 적용할 캐릭터를 선택하세요.")]
    public CharacterAsset Character;

    [DataInput]
    [Label("카메라")]
    [Description("카메라 프리셋/스크린샷에 사용할 카메라를 선택하세요.")]
    public CameraAsset Camera;

    [Section("단축키 안내")]
    [Markdown]
    public string _shortcutInfo =
        "| 키 | 기능 |\n"
        + "|---|---|\n"
        + "| **Z** | 포즈 변경 (랜덤/순서) |\n"
        + "| **X** | 랜덤 표정 변경 |\n"
        + "| **C** | 카메라 위치 변경 |\n"
        + "| **=** | 스크린샷 촬영 |\n"
        + "| **Ctrl+Shift+A** | 페이셜 ON/OFF 토글 |\n"
        + "| **Ctrl+F1~F5** | 볼륨 프리셋 변경 |";

    // ───────────────────────── 포즈 설정 ─────────────────────────

    [Section("포즈 리스트")]
    [DataInput]
    [Label("포즈 순서 모드")]
    [Description("랜덤: 무작위로 포즈 변경 / 순서대로: 목록 순서대로 순환")]
    public PoseOrderMode PoseMode = PoseOrderMode.Random;

    [Markdown]
    public string _poseCountInfo = "등록된 포즈: **0**개";

    [DataInput]
    [Label("제외 카테고리")]
    [Description("일괄 추가 시 제외할 카테고리 키워드 (쉼표 구분). 예: Dance, Locomotion")]
    public string ExcludeCategories = "Dance";

    [Trigger]
    [Label("📦 애니메이션 일괄 추가")]
    [Description(
        "사용 가능한 모든 캐릭터 애니메이션을 포즈 리스트에 일괄 추가합니다. 제외 카테고리에 해당하는 애니메이션과 이미 등록된 애니메이션은 건너뜁니다."
    )]
    public void AddAllAnimations()
    {
        var results = Context.ResourceManager.ProvideResources("CharacterAnimation");
        if (results == null || results.Count == 0)
        {
            UpdateStatus("⚠️ 사용 가능한 애니메이션이 없습니다.");
            return;
        }

        var excludeKeywords = (ExcludeCategories ?? "")
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToArray();

        var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (Poses != null)
        {
            foreach (var p in Poses)
            {
                if (p != null && !string.IsNullOrEmpty(p.Animation))
                    existing.Add(p.Animation);
            }
        }

        var newList = new List<PosePreset>(Poses ?? new PosePreset[0]);
        int addedCount = 0;
        int skippedCount = 0;

        foreach (var result in results)
        {
            if (result.resources == null)
                continue;
            foreach (var resource in result.resources)
            {
                var uri = resource.uri?.ToString();
                if (string.IsNullOrEmpty(uri))
                    continue;
                if (existing.Contains(uri))
                    continue;

                var cat = resource.category ?? "";
                var lbl = resource.label ?? "";
                bool excluded = excludeKeywords.Any(kw =>
                    cat.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0
                    || lbl.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0
                );
                if (excluded)
                {
                    skippedCount++;
                    continue;
                }

                existing.Add(uri);
                newList.Add(
                    StructuredData.Create<PosePreset, SelViewToolAsset>(
                        this,
                        p => p.Animation = uri
                    )
                );
                addedCount++;
            }
        }

        if (addedCount == 0)
        {
            UpdateStatus("ℹ️ 모든 애니메이션이 이미 등록되어 있습니다.");
            return;
        }

        SetDataInput(nameof(Poses), newList.ToArray(), broadcast: true);
        var skipMsg = skippedCount > 0 ? $", 제외: {skippedCount}개" : "";
        UpdateStatus($"✅ {addedCount}개 추가 완료 (총 {newList.Count}개{skipMsg})");
    }

    [Trigger]
    [Label("🗑 포즈 리스트 전체 삭제")]
    [Description("포즈 리스트의 모든 항목을 삭제합니다.")]
    public void ClearAllPoses()
    {
        SetDataInput(nameof(Poses), new PosePreset[0], broadcast: true);
        _currentPoseIndex = -1;
        _highlightedPoseIndex = -1;
        UpdateStatus("🗑 포즈 리스트 전체 삭제됨");
    }

    [DataInput]
    [Label("포즈 리스트")]
    [Description("Z키로 변경할 애니메이션 목록입니다.")]
    public PosePreset[] Poses = new PosePreset[0];

    [DataInput]
    [Label("포즈 전환 시간")]
    [FloatSlider(0f, 3f)]
    public float PoseTransitionTime = 1.2f;

    [Section("포즈 썸네일 패널")]
    [DataInput]
    [Label("썸네일 패널 표시")]
    [Description("화면에 등록된 포즈의 썸네일을 On/Off 합니다.")]
    public bool ShowThumbnailPanel = false;

    [DataInput]
    [Label("썸네일 크기")]
    [IntegerSlider(48, 200)]
    public int ThumbnailDisplaySize = 80;

    [DataInput]
    [Hidden]
    public int ThumbnailColumns = 3;

    [DataInput]
    [Label("패널 위치")]
    public ThumbnailPanelAnchor PanelAnchor = ThumbnailPanelAnchor.BottomLeft;

    [DataInput]
    [Label("투명도")]
    [FloatSlider(0f, 1f)]
    public float ThumbnailPanelOpacity = 0.8f;

    [DataInput]
    [Hidden]
    [Label("그리드 간격")]
    public int ThumbnailGridSpacing = 4;

    [DataInput]
    [Hidden]
    [Label("페이지당 표시 개수")]
    public int ThumbnailItemsPerPage = 9;

    [DataInput]
    [Label("썸네일 텍스처 최대 해상도")]
    [IntegerSlider(64, 512)]
    [Description(
        "로드된 썸네일 이미지를 이 크기 이하로 자동 축소합니다. 포즈가 많을 때 낮추면 메모리 절약·크래시 방지에 효과적입니다. (와루도 내장: 256)"
    )]
    public int ThumbnailMaxTextureSize = 128;

    [Trigger]
    [Label("▶ 현재 포즈 추가")]
    [Description("캐릭터의 현재 Idle 애니메이션을 목록에 추가합니다.")]
    public void CaptureCurrentPose()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 먼저 선택하세요.");
            return;
        }

        var uri = Character.DefaultIdleAnimation;
        if (string.IsNullOrEmpty(uri))
            uri = CharacterAsset.DefaultIdleAnimationUri;

        var newList = new List<PosePreset>(Poses ?? new PosePreset[0]);
        var preset = StructuredData.Create<PosePreset, SelViewToolAsset>(
            this,
            p => p.Animation = uri
        );
        newList.Add(preset);
        SetDataInput(nameof(Poses), newList.ToArray(), broadcast: true);
        UpdateStatus($"✅ 포즈 추가됨: {uri}");
    }

    [DataInput]
    [Label("내보내기 파일명")]
    [Description(
        "저장할 파일 이름 (.json 자동 추가). StreamingAssets/PoseLists/ 폴더에 저장됩니다."
    )]
    public string PoseListFileName = "MyPoseList";

    [Trigger]
    [Label("💾 리스트 내보내기")]
    [Description("현재 포즈 리스트를 JSON 파일로 저장합니다.")]
    public void ExportPoseList()
    {
        ExportPoseListAsync().Forget();
    }

    [DataInput]
    [Label("불러올 파일 선택")]
    [AutoComplete(nameof(GetSavedPoseFiles))]
    [Description("PoseLists 폴더에서 불러올 파일을 선택하세요.")]
    public string ImportFileName = "";

    private async UniTaskVoid ExportPoseListAsync()
    {
        try
        {
            var poses = Poses;
            if (poses == null || poses.Length == 0)
            {
                UpdateStatus("⚠️ 내보낼 포즈가 없습니다.");
                return;
            }

            var uris = poses
                .Where(p => p != null && !string.IsNullOrEmpty(p.Animation))
                .Select(p => p.Animation)
                .ToArray();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PoseListData
                {
                    version = 1,
                    count = uris.Length,
                    animations = uris,
                },
                Newtonsoft.Json.Formatting.Indented
            );

            var fileName = string.IsNullOrWhiteSpace(PoseListFileName)
                ? "MyPoseList"
                : PoseListFileName.Trim();
            var path = $"PoseLists/{fileName}.json";
            await Context.PersistentDataManager.WriteFileAsync(path, json);
            UpdateStatus($"💾 내보내기 완료: {path} ({uris.Length}개)");
        }
        catch (Exception e)
        {
            UpdateStatus($"❌ 내보내기 실패: {e.Message}");
        }
    }

    public UniTask<AutoCompleteList> GetSavedPoseFiles()
    {
        return UniTask.FromResult(BuildSavedPoseFileList());
    }

    private AutoCompleteList BuildSavedPoseFileList()
    {
        try
        {
            var entries = Context.PersistentDataManager.GetFileEntries("PoseLists", "*.json");
            var jsonFiles = entries
                .OrderBy(e => e.fileName)
                .Select(e => new AutoCompleteEntry { label = e.fileName, value = e.path })
                .ToList();

            if (jsonFiles.Count == 0)
                return AutoCompleteList.Message("저장된 파일이 없습니다.");

            return AutoCompleteList.Single(jsonFiles);
        }
        catch
        {
            return AutoCompleteList.Message("파일 목록을 불러올 수 없습니다.");
        }
    }

    [Trigger]
    [Label("📂 리스트 불러오기")]
    [Description("위에서 선택한 파일로 포즈 리스트를 불러옵니다. 기존 리스트를 대체합니다.")]
    public void ImportPoseList()
    {
        ImportPoseListAsync().Forget();
    }

    private async UniTaskVoid ImportPoseListAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ImportFileName))
            {
                UpdateStatus("⚠️ 불러올 파일을 먼저 선택하세요.");
                return;
            }

            var path = ImportFileName;
            if (!path.StartsWith("PoseLists/") && !path.StartsWith("PoseLists\\"))
                path = $"PoseLists/{path}";

            if (!Context.PersistentDataManager.HasFile(path))
            {
                UpdateStatus($"⚠️ 파일을 찾을 수 없습니다: {path}");
                return;
            }

            var json = await Context.PersistentDataManager.ReadFileAsync(path);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<PoseListData>(json);

            if (data?.animations == null || data.animations.Length == 0)
            {
                UpdateStatus("⚠️ 파일에 포즈 데이터가 없습니다.");
                return;
            }

            var newList = new List<PosePreset>();
            foreach (var uri in data.animations)
            {
                if (string.IsNullOrEmpty(uri))
                    continue;
                newList.Add(
                    StructuredData.Create<PosePreset, SelViewToolAsset>(
                        this,
                        p => p.Animation = uri
                    )
                );
            }

            SetDataInput(nameof(Poses), newList.ToArray(), broadcast: true);
            _currentPoseIndex = -1;
            _highlightedPoseIndex = -1;
            UpdatePoseCountInfo();
            RebuildThumbnailPanel();
            var displayName = path;
            if (displayName.StartsWith("PoseLists/"))
                displayName = displayName.Substring("PoseLists/".Length);
            UpdateStatus($"📂 불러오기 완료: {displayName} ({newList.Count}개)");
        }
        catch (Exception e)
        {
            UpdateStatus($"❌ 불러오기 실패: {e.Message}");
        }
    }

    [Trigger]
    [Label("📋 저장된 리스트 목록")]
    [Description("PoseLists 폴더에 저장된 파일 목록을 표시합니다.")]
    public void ListSavedPoseLists()
    {
        try
        {
            var pdm = Context.PersistentDataManager;
            if (!pdm.HasFile("PoseLists/."))
            {
                var files = pdm.GetFileEntries("PoseLists", "*.json").ToArray();
                if (files.Length == 0)
                {
                    UpdateStatus("ℹ️ 저장된 리스트가 없습니다.");
                    return;
                }
                var names = files.Select(f =>
                {
                    var n = f.fileName;
                    var d = n.LastIndexOf('.');
                    return d > 0 ? n.Substring(0, d) : n;
                });
                UpdateStatus($"📋 저장된 리스트: {string.Join(", ", names)}");
            }
        }
        catch
        {
            try
            {
                var files = Context
                    .PersistentDataManager.GetFileEntries("PoseLists", "*.json")
                    .ToArray();
                if (files.Length == 0)
                {
                    UpdateStatus("ℹ️ 저장된 리스트가 없습니다.");
                    return;
                }
                var names = files.Select(f =>
                {
                    var n = f.fileName;
                    var d = n.LastIndexOf('.');
                    return d > 0 ? n.Substring(0, d) : n;
                });
                UpdateStatus($"📋 저장된 리스트: {string.Join(", ", names)}");
            }
            catch
            {
                UpdateStatus("ℹ️ PoseLists 폴더가 아직 없습니다. 먼저 내보내기를 해주세요.");
            }
        }
    }

    [Trigger]
    [Label("📁 저장 폴더 열기")]
    [Description("PoseLists 폴더를 파일 탐색기로 엽니다.")]
    public void OpenPoseListFolder()
    {
        try
        {
            var basePath = Context.PersistentDataManager.GetBasePath();
            var folderPath = basePath + "PoseLists";
            folderPath = folderPath.Replace('/', '\\');
            Application.OpenURL(folderPath);
            UpdateStatus($"📁 폴더 열기: {folderPath}");
        }
        catch (Exception e)
        {
            UpdateStatus($"❌ 폴더 열기 실패: {e.Message}");
        }
    }

    [Serializable]
    private class PoseListData
    {
        public int version;
        public int count;
        public string[] animations;
    }

    // ───────────────────────── 표정 설정 ─────────────────────────

    [DataInput]
    [Label("표정 프리셋")]
    [Description("랜덤 표정에 사용할 표정 목록입니다. 캐릭터에 등록된 표정 이름을 선택하세요.")]
    public ExpressionPreset[] Expressions = new ExpressionPreset[0];

    [DataInput]
    [Label("이전 표정 해제")]
    [Description("새 표정 적용 시 이전 표정을 자동으로 해제합니다.")]
    public bool AutoExitPreviousExpression = true;

    // ───────────────────────── 카메라 프리셋 ─────────────────────────

    [DataInput]
    [Label("카메라 프리셋")]
    [Description("C키로 순환할 카메라 위치 목록입니다.")]
    public CameraPreset[] CameraPresets = new CameraPreset[0];

    [DataInput]
    [Label("카메라 전환 시간")]
    [FloatSlider(0f, 3f)]
    public float CameraTransitionTime = 0.5f;

    [Trigger]
    [Label("▶ 현재 카메라 위치 추가")]
    [Description("카메라의 현재 위치/회전/FOV를 목록에 추가합니다.")]
    public void CaptureCurrentCameraPosition()
    {
        if (Camera == null)
        {
            UpdateStatus("⚠️ 카메라를 먼저 선택하세요.");
            return;
        }

        var cam = Camera.Camera;
        if (cam == null)
        {
            UpdateStatus("⚠️ 카메라 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        var newList = new List<CameraPreset>(CameraPresets ?? new CameraPreset[0]);
        var preset = new CameraPreset
        {
            Name = $"카메라 {newList.Count + 1}",
            Position = cam.transform.position,
            Rotation = cam.transform.eulerAngles,
            FieldOfView = Camera.FieldOfView,
        };
        newList.Add(preset);
        SetDataInput(nameof(CameraPresets), newList.ToArray(), broadcast: true);
        UpdateStatus($"✅ 카메라 위치 추가됨: {preset.Position}");
    }

    // ───────────────────────── 볼륨 프리셋 ─────────────────────────

    [DataInput]
    [Label("볼륨 프리셋 (Ctrl+F1~F5)")]
    [Description("5가지 분위기 프리셋을 설정하세요. Ctrl+F1~F5로 전환합니다.")]
    public VolumePreset[] VolumePresets = new VolumePreset[]
    {
        new VolumePreset
        {
            Name = "기본",
            Brightness = 1.05f,
            Contrast = 1.02f,
            Vibrance = 1f,
        },
        new VolumePreset
        {
            Name = "따뜻한",
            Brightness = 1.1f,
            Contrast = 1.05f,
            Vibrance = 1.1f,
            Tint = new Color(1f, 0.95f, 0.9f, 0.1f),
            EnableBloom = true,
            BloomIntensity = 0.15f,
        },
        new VolumePreset
        {
            Name = "차가운",
            Brightness = 1.0f,
            Contrast = 1.08f,
            Vibrance = 0.9f,
            Tint = new Color(0.9f, 0.95f, 1f, 0.1f),
        },
        new VolumePreset
        {
            Name = "드라마틱",
            Brightness = 0.95f,
            Contrast = 1.15f,
            Vibrance = 1.2f,
            EnableBloom = true,
            BloomIntensity = 0.2f,
            EnableVignetting = true,
            VignettingFadeOut = 0.3f,
        },
        new VolumePreset
        {
            Name = "소프트",
            Brightness = 1.1f,
            Contrast = 0.98f,
            Vibrance = 0.95f,
            EnableBloom = true,
            BloomIntensity = 0.25f,
            BloomThreshold = 0.5f,
        },
    };

    // ───────────────────────── 수동 트리거 ─────────────────────────

    [Trigger]
    [Label("🎲 다음 포즈 (Z)")]
    public void TriggerNextPose() => ApplyNextPose();

    [Trigger]
    [Label("🎭 랜덤 표정 (X)")]
    public void TriggerRandomExpression() => ApplyRandomExpression();

    [Trigger]
    [Label("📷 다음 카메라 (C)")]
    public void TriggerNextCamera() => CycleCamera();

    [Trigger]
    [Label("📸 스크린샷 (=)")]
    public void TriggerScreenshot() => DoTakeScreenshot();

    [Trigger]
    [Label("🔄 페이셜 토글 (Ctrl+Shift+A)")]
    public void TriggerToggleFacial() => DoToggleFacialTracking();

    // ───────────────────────── 상태 표시 ─────────────────────────

    [Markdown]
    public string _statusDisplay = "*준비 완료*";

    // ───────────────────────── 내부 상태 ─────────────────────────

    private Guid _keystrokeSubscription;
    private int _currentPoseIndex = -1;
    private int _currentCameraIndex = -1;
    private string _lastExpressionName;
    private int _currentVolumeIndex = -1;

    private GameObject _thumbnailPanelRoot;
    private readonly List<Texture2D> _loadedThumbnailTextures = new List<Texture2D>();
    private readonly List<Image> _thumbnailBorders = new List<Image>();
    private int _highlightedPoseIndex = -1;
    private int _thumbnailPage = 0;
    private Transform _thumbnailGridParent;
    private Text _pageLabel;
    private Text _selectionInfoText;
    private Text _totalCountText;
    private Text _nextBtnText;
    private Text _facialBtnText;
    private InputField _searchInputField;
    private string _searchQuery = "";
    private Dictionary<string, string> _thumbnailMapCache;
    private GameObject _settingsSidebarGo;
    private bool _settingsVisible = false;
    private GridLayoutGroup _gridLayoutGroup;
    private Text _modePillText;
    private Text _modeSeqText;
    private Image _modeSeqImg;
    private Text _modeRndText;
    private Image _modeRndImg;
    private GameObject _emptyStateGo;
    private CanvasGroup _panelCanvasGroup;

    // ═══════════════════════════════════════════════════════════════
    //  라이프사이클
    // ═══════════════════════════════════════════════════════════════

    protected override void OnCreate()
    {
        base.OnCreate();
        SetActive(true);

        _keystrokeSubscription = Context.EventBus.Subscribe<KeystrokePressedEvent>(
            OnKeystrokePressed
        );
        UpdatePoseCountInfo();

        Watch(nameof(Enabled), () => UpdateStatus(Enabled ? "✅ 활성화됨" : "⏸️ 비활성화됨"));
        Watch(
            nameof(Character),
            () => UpdateStatus(Character != null ? $"캐릭터: {Character.Name}" : "캐릭터 미선택")
        );
        Watch(
            nameof(Camera),
            () => UpdateStatus(Camera != null ? $"카메라: {Camera.Name}" : "카메라 미선택")
        );

        Watch(nameof(ShowThumbnailPanel), RebuildThumbnailPanel);
        Watch(nameof(PoseMode), UpdateNextBtnLabel);
        Watch(
            nameof(Poses),
            () =>
            {
                UpdatePoseCountInfo();
                RebuildThumbnailPanel();
            }
        );
        Watch(nameof(ThumbnailDisplaySize), RebuildThumbnailPanel);
        Watch(nameof(PanelAnchor), RebuildThumbnailPanel);
        Watch(nameof(ThumbnailPanelOpacity), () =>
        {
            if (_panelCanvasGroup != null)
                _panelCanvasGroup.alpha = ThumbnailPanelOpacity;
        });
        Watch(nameof(ThumbnailMaxTextureSize), RebuildThumbnailPanel);

        Debug.Log("[SelView Tool] 에셋 생성 완료");
    }

    protected override void OnDestroy()
    {
        DestroyThumbnailPanel();
        Context.EventBus.Unsubscribe<KeystrokePressedEvent>(_keystrokeSubscription);
        base.OnDestroy();
    }

    // ═══════════════════════════════════════════════════════════════
    //  키보드 이벤트
    // ═══════════════════════════════════════════════════════════════

    private void OnKeystrokePressed(KeystrokePressedEvent e)
    {
        if (!Enabled)
            return;

        if (e.Keystroke == Keystroke.Z && !e.Ctrl && !e.Shift && !e.Alt)
        {
            ApplyNextPose();
        }
        else if (e.Keystroke == Keystroke.X && !e.Ctrl && !e.Shift && !e.Alt)
        {
            ApplyRandomExpression();
        }
        else if (e.Keystroke == Keystroke.C && !e.Ctrl && !e.Shift && !e.Alt)
        {
            CycleCamera();
        }
        else if (e.Keystroke == Keystroke.EqualsKey && !e.Ctrl && !e.Shift && !e.Alt)
        {
            DoTakeScreenshot();
        }
        else if (e.Keystroke == Keystroke.A && e.Ctrl && e.Shift && !e.Alt)
        {
            DoToggleFacialTracking();
        }
        else if (e.Ctrl && !e.Shift && !e.Alt)
        {
            int presetIndex = e.Keystroke switch
            {
                Keystroke.F1 => 0,
                Keystroke.F2 => 1,
                Keystroke.F3 => 2,
                Keystroke.F4 => 3,
                Keystroke.F5 => 4,
                _ => -1,
            };
            if (presetIndex >= 0)
                ApplyVolumePreset(presetIndex);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  포즈
    // ═══════════════════════════════════════════════════════════════

    private void ApplyNextPose()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 선택하세요.");
            return;
        }

        var available = Poses
            ?.Where(p => p != null && !string.IsNullOrEmpty(p.Animation))
            .ToArray();
        if (available == null || available.Length == 0)
        {
            UpdateStatus("⚠️ 설정된 포즈가 없습니다.");
            return;
        }

        PosePreset selected;
        if (PoseMode == PoseOrderMode.Sequential)
        {
            _currentPoseIndex = (_currentPoseIndex + 1) % available.Length;
            selected = available[_currentPoseIndex];
            ApplyPose(selected);
            UpdateStatus(
                $"▶ 포즈: **{selected.GetHeader()}** ({_currentPoseIndex + 1}/{available.Length})"
            );
        }
        else
        {
            selected = available[UnityEngine.Random.Range(0, available.Length)];
            ApplyPose(selected);
            UpdateStatus($"🎲 포즈: **{selected.GetHeader()}**");
        }

        if (_thumbnailPanelRoot != null)
        {
            int idx = Array.IndexOf(Poses, selected);
            EnsureThumbnailPageForIndex(idx);
            UpdateThumbnailHighlight(idx);
        }
    }

    private void ApplyPreviousPose()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 선택하세요.");
            return;
        }

        var available = Poses
            ?.Where(p => p != null && !string.IsNullOrEmpty(p.Animation))
            .ToArray();
        if (available == null || available.Length == 0)
        {
            UpdateStatus("⚠️ 설정된 포즈가 없습니다.");
            return;
        }

        _currentPoseIndex--;
        if (_currentPoseIndex < 0)
            _currentPoseIndex = available.Length - 1;
        var selected = available[_currentPoseIndex];
        ApplyPose(selected);
        UpdateStatus(
            $"◀ 포즈: **{selected.GetHeader()}** ({_currentPoseIndex + 1}/{available.Length})"
        );

        if (_thumbnailPanelRoot != null)
        {
            int idx = Array.IndexOf(Poses, selected);
            EnsureThumbnailPageForIndex(idx);
            UpdateThumbnailHighlight(idx);
        }
    }

    private void EnsureThumbnailPageForIndex(int globalIndex)
    {
        if (globalIndex < 0)
            return;
        int perPage = Mathf.Max(ThumbnailItemsPerPage, 4);
        int targetPage = globalIndex / perPage;
        if (targetPage != _thumbnailPage)
        {
            _thumbnailPage = targetPage;
            RefreshThumbnailPage();
        }
    }

    private void ApplyPose(PosePreset preset)
    {
        if (Character == null || preset == null)
            return;
        Character.SetDataInput("DefaultIdleAnimation", preset.Animation, broadcast: true);
    }

    // ═══════════════════════════════════════════════════════════════
    //  표정
    // ═══════════════════════════════════════════════════════════════

    private void ApplyRandomExpression()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 선택하세요.");
            return;
        }

        var enabled = Expressions
            ?.Where(e => e != null && e.Enabled && !string.IsNullOrEmpty(e.ExpressionName))
            .ToArray();
        if (enabled == null || enabled.Length == 0)
        {
            UpdateStatus("⚠️ 활성화된 표정이 없습니다.");
            return;
        }

        if (
            AutoExitPreviousExpression
            && !string.IsNullOrEmpty(_lastExpressionName)
            && Character.Expressions != null
        )
        {
            var lastExpr = Character.Expressions.FirstOrDefault(ex =>
                ex.Name == _lastExpressionName
            );
            if (lastExpr != null)
            {
                try
                {
                    lastExpr.ExitExpression();
                }
                catch { }
            }
        }

        var selected = enabled[UnityEngine.Random.Range(0, enabled.Length)];

        if (Character.Expressions != null)
        {
            var expr = Character.Expressions.FirstOrDefault(ex =>
                ex.Name == selected.ExpressionName
            );
            if (expr != null)
            {
                try
                {
                    expr.EnterExpression();
                    _lastExpressionName = selected.ExpressionName;
                    UpdateStatus($"🎭 표정: **{selected.ExpressionName}**");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[SelView Tool] 표정 적용 실패: {e.Message}");
                    UpdateStatus($"⚠️ 표정 적용 실패: {selected.ExpressionName}");
                }
            }
            else
            {
                UpdateStatus($"⚠️ 표정을 찾을 수 없음: {selected.ExpressionName}");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  카메라
    // ═══════════════════════════════════════════════════════════════

    private void CycleCamera()
    {
        if (Camera == null)
        {
            UpdateStatus("⚠️ 카메라를 선택하세요.");
            return;
        }

        if (CameraPresets == null || CameraPresets.Length == 0)
        {
            UpdateStatus("⚠️ 카메라 프리셋이 없습니다.");
            return;
        }

        _currentCameraIndex = (_currentCameraIndex + 1) % CameraPresets.Length;
        var preset = CameraPresets[_currentCameraIndex];

        Camera.TeleportTo(preset.Position, Quaternion.Euler(preset.Rotation));

        if (preset.FieldOfView > 0)
            Camera.SetDataInput("FieldOfView", preset.FieldOfView, broadcast: true);

        UpdateStatus(
            $"📷 카메라: **{preset.Name}** ({_currentCameraIndex + 1}/{CameraPresets.Length})"
        );
    }

    // ═══════════════════════════════════════════════════════════════
    //  스크린샷
    // ═══════════════════════════════════════════════════════════════

    private void DoTakeScreenshot()
    {
        if (Camera == null)
        {
            UpdateStatus("⚠️ 카메라를 선택하세요.");
            return;
        }

        try
        {
            Camera.TakeScreenshot();
            UpdateStatus("📸 스크린샷 촬영 완료!");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[SelView Tool] 스크린샷 실패: {e.Message}");
            UpdateStatus("⚠️ 스크린샷 촬영 실패");
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  페이셜 트래킹 토글
    // ═══════════════════════════════════════════════════════════════

    private void DoToggleFacialTracking()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 선택하세요.");
            return;
        }

        var newState = !Character.TrackingEnabled;
        Character.SetDataInput("TrackingEnabled", newState, broadcast: true);
        UpdateStatus(newState ? "🟢 페이셜 트래킹 **ON**" : "🔴 페이셜 트래킹 **OFF**");
        UpdateFacialBtnLabel();
    }

    // ═══════════════════════════════════════════════════════════════
    //  볼륨 프리셋
    // ═══════════════════════════════════════════════════════════════

    private void ApplyVolumePreset(int index)
    {
        if (Camera == null)
        {
            UpdateStatus("⚠️ 카메라를 선택하세요.");
            return;
        }

        if (VolumePresets == null || index >= VolumePresets.Length || VolumePresets[index] == null)
        {
            UpdateStatus($"⚠️ 볼륨 프리셋 {index + 1}이 없습니다.");
            return;
        }

        var preset = VolumePresets[index];
        _currentVolumeIndex = index;

        Camera.SetDataInput("Brightness", preset.Brightness, broadcast: true);
        Camera.SetDataInput("Contrast", preset.Contrast, broadcast: true);
        Camera.SetDataInput("Vibrance", preset.Vibrance, broadcast: true);
        Camera.SetDataInput("Tint", preset.Tint, broadcast: true);
        Camera.SetDataInput("EnableBloom", preset.EnableBloom, broadcast: true);

        if (preset.EnableBloom)
        {
            Camera.SetDataInput("BloomIntensity", preset.BloomIntensity, broadcast: true);
            Camera.SetDataInput("BloomThreshold", preset.BloomThreshold, broadcast: true);
        }

        Camera.SetDataInput("EnableVignetting", preset.EnableVignetting, broadcast: true);
        if (preset.EnableVignetting)
        {
            Camera.SetDataInput("VignettingColor", preset.VignettingColor, broadcast: true);
            Camera.SetDataInput("VignettingFadeOut", preset.VignettingFadeOut, broadcast: true);
        }

        Camera.SetDataInput("EnableDepthOfField", preset.EnableDepthOfField, broadcast: true);
        if (preset.EnableDepthOfField)
        {
            Camera.SetDataInput(
                "DepthOfFieldAperture",
                preset.DepthOfFieldAperture,
                broadcast: true
            );
            Camera.SetDataInput(
                "DepthOfFieldFocalLength",
                preset.DepthOfFieldFocalLength,
                broadcast: true
            );
        }

        UpdateStatus($"🎨 볼륨 프리셋: **{preset.Name}** (F{index + 1})");
    }

    // ═══════════════════════════════════════════════════════════════
    //  상태 업데이트
    // ═══════════════════════════════════════════════════════════════

    private void UpdateStatus(string message)
    {
        _statusDisplay = message;
        BroadcastDataInput(nameof(_statusDisplay));
    }

    private void UpdatePoseCountInfo()
    {
        int count = Poses?.Length ?? 0;
        _poseCountInfo = $"등록된 포즈: **{count}**개";
        BroadcastDataInput(nameof(_poseCountInfo));
    }

    private void UpdateNextBtnLabel()
    {
        UpdateModePillText();
        UpdateModeToggleBtn();
    }

    private void UpdateModePillText()
    {
        if (_modePillText == null)
            return;
        _modePillText.text =
            PoseMode == PoseOrderMode.Sequential ? "≡  순서 모드: 순서대로" : "≡  순서 모드: 랜덤";
    }

    private void UpdateModeToggleBtn()
    {
        bool seq = PoseMode == PoseOrderMode.Sequential;
        if (_modeSeqImg != null)
            _modeSeqImg.color = seq
                ? new Color(0.22f, 0.38f, 0.65f, 0.95f)
                : new Color(0.15f, 0.15f, 0.20f, 0.6f);
        if (_modeSeqText != null)
            _modeSeqText.color = seq
                ? Color.white
                : new Color(0.50f, 0.50f, 0.58f, 1f);
        if (_modeRndImg != null)
            _modeRndImg.color = seq
                ? new Color(0.15f, 0.15f, 0.20f, 0.6f)
                : new Color(0.28f, 0.17f, 0.50f, 0.95f);
        if (_modeRndText != null)
            _modeRndText.color = seq
                ? new Color(0.50f, 0.50f, 0.58f, 1f)
                : Color.white;
    }

    private bool GetFacialState()
    {
        return Character != null && Character.TrackingEnabled;
    }

    private void UpdateFacialBtnLabel()
    {
        if (_facialBtnText == null)
            return;
        _facialBtnText.text = GetFacialState() ? "🟢 Face" : "🔴 Face";
    }

    // ═══════════════════════════════════════════════════════════════
    //  포즈 썸네일 패널
    // ═══════════════════════════════════════════════════════════════

    private static readonly string[] _thumbnailSearchPaths = new[]
    {
        "Images/PoseThumbnails",
        "Clients/Thumbnails/Animations",
    };
    private Sprite _roundedSprite;
    private Sprite _roundedSpriteSmall;
    private Sprite _roundedSpriteTiny;
    private Sprite _circleSprite;

    private Sprite CreateRoundedRectSprite(int w, int h, int radius)
    {
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        var pixels = new Color32[w * h];
        var white = new Color32(255, 255, 255, 255);
        var clear = new Color32(0, 0, 0, 0);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                bool inside = true;
                int cx = 0,
                    cy = 0;
                if (x < radius && y < radius)
                {
                    cx = radius;
                    cy = radius;
                }
                else if (x >= w - radius && y < radius)
                {
                    cx = w - radius;
                    cy = radius;
                }
                else if (x < radius && y >= h - radius)
                {
                    cx = radius;
                    cy = h - radius;
                }
                else if (x >= w - radius && y >= h - radius)
                {
                    cx = w - radius;
                    cy = h - radius;
                }
                else
                {
                    pixels[y * w + x] = white;
                    continue;
                }

                float dist = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                if (dist > radius + 0.5f)
                    inside = false;
                else if (dist > radius - 0.5f)
                {
                    byte a = (byte)(255 * (1f - (dist - (radius - 0.5f))));
                    pixels[y * w + x] = new Color32(255, 255, 255, a);
                    continue;
                }
                pixels[y * w + x] = inside ? white : clear;
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        var border = new Vector4(radius, radius, radius, radius);
        return Sprite.Create(
            tex,
            new Rect(0, 0, w, h),
            new Vector2(0.5f, 0.5f),
            100,
            0,
            SpriteMeshType.FullRect,
            border
        );
    }

    private void EnsureRoundedSprites()
    {
        if (_roundedSprite == null)
            _roundedSprite = CreateRoundedRectSprite(128, 128, 16);
        if (_roundedSpriteSmall == null)
            _roundedSpriteSmall = CreateRoundedRectSprite(64, 64, 12);
        if (_roundedSpriteTiny == null)
            _roundedSpriteTiny = CreateRoundedRectSprite(32, 32, 2);
        if (_circleSprite == null)
            _circleSprite = CreateRoundedRectSprite(32, 32, 16);
    }

    private void ApplyRoundedSprite(Image img, Sprite spr)
    {
        img.sprite = spr;
        img.type = Image.Type.Sliced;
        img.pixelsPerUnitMultiplier = 1f;
    }

    private void RebuildThumbnailPanel()
    {
        DestroyThumbnailPanel();
        if (!ShowThumbnailPanel)
            return;

        var poses = Poses;
        if (poses == null || poses.Length == 0)
            return;

        _thumbnailPanelRoot = new GameObject("SelViewTool_ThumbnailPanel");
        UnityEngine.Object.DontDestroyOnLoad(_thumbnailPanelRoot);

        var canvas = _thumbnailPanelRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        var scaler = _thumbnailPanelRoot.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        _thumbnailPanelRoot.AddComponent<GraphicRaycaster>();
        EnsureThumbnailEventSystem();
        EnsureRoundedSprites();

        var font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        int size = ThumbnailDisplaySize;
        int cols = Mathf.Clamp(ThumbnailColumns, 1, 15);
        int gap = Mathf.Clamp(ThumbnailGridSpacing, 0, 24);
        float contentWidth = cols * size + (cols - 1) * gap + 8;

        var outerGo = new GameObject("Outer");
        outerGo.transform.SetParent(_thumbnailPanelRoot.transform, false);
        var outerRt = outerGo.AddComponent<RectTransform>();
        SetThumbnailPanelAnchor(outerRt);

        var outerBg = outerGo.AddComponent<Image>();
        outerBg.color = new Color(0.07f, 0.07f, 0.09f, 1f);
        ApplyRoundedSprite(outerBg, _roundedSprite);

        _panelCanvasGroup = outerGo.AddComponent<CanvasGroup>();
        _panelCanvasGroup.alpha = ThumbnailPanelOpacity;

        var outerHlg = outerGo.AddComponent<HorizontalLayoutGroup>();
        outerHlg.spacing = 0;
        outerHlg.padding = new RectOffset(0, 0, 0, 0);
        outerHlg.childForceExpandWidth = false;
        outerHlg.childForceExpandHeight = true;
        outerHlg.childControlWidth = true;
        outerHlg.childControlHeight = true;

        var outerCsf = outerGo.AddComponent<ContentSizeFitter>();
        outerCsf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        outerCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var leftGo = new GameObject("LeftContent");
        leftGo.transform.SetParent(outerGo.transform, false);

        var leftVlg = leftGo.AddComponent<VerticalLayoutGroup>();
        leftVlg.spacing = 0;
        leftVlg.padding = new RectOffset(0, 0, 0, 0);
        leftVlg.childForceExpandWidth = true;
        leftVlg.childForceExpandHeight = false;
        leftVlg.childControlWidth = true;
        leftVlg.childControlHeight = true;
        leftVlg.childAlignment = TextAnchor.UpperCenter;

        var leftLe = leftGo.AddComponent<LayoutElement>();
        leftLe.preferredWidth = contentWidth;

        CreateTitleBar(leftGo.transform, font, contentWidth);
        CreateSearchBar(leftGo.transform, font, contentWidth);

        var gridGo = new GameObject("Grid");
        gridGo.transform.SetParent(leftGo.transform, false);

        var gridCsf = gridGo.AddComponent<ContentSizeFitter>();
        gridCsf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        gridCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var gridLe = gridGo.AddComponent<LayoutElement>();
        gridLe.preferredWidth = contentWidth;

        var glg = gridGo.AddComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(size, size);
        glg.spacing = new Vector2(gap, gap);
        glg.padding = new RectOffset(4, 4, 4, 4);
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = cols;
        glg.startAxis = GridLayoutGroup.Axis.Horizontal;
        glg.startCorner = GridLayoutGroup.Corner.UpperLeft;
        glg.childAlignment = TextAnchor.UpperLeft;

        _thumbnailGridParent = gridGo.transform;
        _gridLayoutGroup = glg;

        BuildThumbnailCache();

        int totalPages = GetTotalPages();
        if (_thumbnailPage >= totalPages)
            _thumbnailPage = 0;

        CreateBottomBar(leftGo.transform, font, contentWidth, totalPages > 1);

        CreateSettingsSidebar(outerGo.transform, font);

        PopulateCurrentPage(size);
    }

    private void CreateTitleBar(Transform parent, Font font, float width)
    {
        var titleGo = new GameObject("TitleBar");
        titleGo.transform.SetParent(parent, false);

        var titleBg = titleGo.AddComponent<Image>();
        titleBg.color = new Color(0.09f, 0.09f, 0.115f, 1f);

        var titleLe = titleGo.AddComponent<LayoutElement>();
        titleLe.preferredWidth = width;
        titleLe.preferredHeight = 28;

        var titleHlg = titleGo.AddComponent<HorizontalLayoutGroup>();
        titleHlg.padding = new RectOffset(10, 8, 6, 4);
        titleHlg.spacing = 6;
        titleHlg.childForceExpandWidth = false;
        titleHlg.childForceExpandHeight = false;
        titleHlg.childControlWidth = true;
        titleHlg.childControlHeight = true;
        titleHlg.childAlignment = TextAnchor.MiddleCenter;

        // ── 타이틀 텍스트
        var labelGo = new GameObject("Title");
        labelGo.transform.SetParent(titleGo.transform, false);
        labelGo.AddComponent<LayoutElement>();
        var labelTxt = labelGo.AddComponent<Text>();
        labelTxt.font = font;
        labelTxt.text = "포즈 선택";
        labelTxt.fontSize = 13;
        labelTxt.fontStyle = FontStyle.Bold;
        labelTxt.alignment = TextAnchor.MiddleLeft;
        labelTxt.color = new Color(0.95f, 0.95f, 0.97f, 1f);
        labelTxt.raycastTarget = false;

        // ── 총 개수 뱃지
        var totalWrap = new GameObject("TotalWrap");
        totalWrap.transform.SetParent(titleGo.transform, false);
        var twBg = totalWrap.AddComponent<Image>();
        twBg.color = new Color(1f, 1f, 1f, 0.07f);
        ApplyRoundedSprite(twBg, _roundedSpriteTiny);
        var twLe = totalWrap.AddComponent<LayoutElement>();
        twLe.preferredWidth = 44;
        twLe.preferredHeight = 20;
        var totalGo = new GameObject("TotalCount");
        totalGo.transform.SetParent(totalWrap.transform, false);
        var tRt = totalGo.AddComponent<RectTransform>();
        tRt.anchorMin = Vector2.zero;
        tRt.anchorMax = Vector2.one;
        tRt.offsetMin = Vector2.zero;
        tRt.offsetMax = Vector2.zero;
        _totalCountText = totalGo.AddComponent<Text>();
        _totalCountText.font = font;
        _totalCountText.fontSize = 11;
        _totalCountText.alignment = TextAnchor.MiddleCenter;
        _totalCountText.color = new Color(0.52f, 0.54f, 0.68f, 1f);
        _totalCountText.raycastTarget = false;

        // ── 모드 세그먼트 (순서 | 랜덤)
        {
            var segGo = new GameObject("ModeSeg");
            segGo.transform.SetParent(titleGo.transform, false);
            var segBg = segGo.AddComponent<Image>();
            segBg.color = new Color(0.10f, 0.10f, 0.14f, 0.9f);
            ApplyRoundedSprite(segBg, _roundedSpriteTiny);
            var segLe = segGo.AddComponent<LayoutElement>();
            segLe.preferredWidth = 56;
            segLe.preferredHeight = 20;
            var segHlg = segGo.AddComponent<HorizontalLayoutGroup>();
            segHlg.spacing = 2;
            segHlg.padding = new RectOffset(2, 2, 2, 2);
            segHlg.childForceExpandWidth = true;
            segHlg.childForceExpandHeight = true;
            segHlg.childControlWidth = true;
            segHlg.childControlHeight = true;

            var sGo = new GameObject("SeqBtn");
            sGo.transform.SetParent(segGo.transform, false);
            _modeSeqImg = sGo.AddComponent<Image>();
            ApplyRoundedSprite(_modeSeqImg, _roundedSpriteTiny);
            var sBtn = sGo.AddComponent<Button>();
            sBtn.targetGraphic = _modeSeqImg;
            var sBtnC = sBtn.colors;
            sBtnC.highlightedColor = new Color(1.2f, 1.2f, 1.3f, 1f);
            sBtnC.pressedColor = new Color(0.75f, 0.75f, 0.85f, 1f);
            sBtnC.fadeDuration = 0.07f;
            sBtn.colors = sBtnC;
            sBtn.onClick.AddListener(() => SetDataInput(
                nameof(PoseMode), PoseOrderMode.Sequential, broadcast: true));
            _modeSeqText = MakeTextChild(sGo.transform, "T", font, "순서", 9,
                Color.white, TextAnchor.MiddleCenter);

            var rGo = new GameObject("RndBtn");
            rGo.transform.SetParent(segGo.transform, false);
            _modeRndImg = rGo.AddComponent<Image>();
            ApplyRoundedSprite(_modeRndImg, _roundedSpriteTiny);
            var rBtn = rGo.AddComponent<Button>();
            rBtn.targetGraphic = _modeRndImg;
            var rBtnC = rBtn.colors;
            rBtnC.highlightedColor = new Color(1.2f, 1.2f, 1.3f, 1f);
            rBtnC.pressedColor = new Color(0.75f, 0.75f, 0.85f, 1f);
            rBtnC.fadeDuration = 0.07f;
            rBtn.colors = rBtnC;
            rBtn.onClick.AddListener(() => SetDataInput(
                nameof(PoseMode), PoseOrderMode.Random, broadcast: true));
            _modeRndText = MakeTextChild(rGo.transform, "T", font, "랜덤", 9,
                Color.white, TextAnchor.MiddleCenter);

            UpdateModeToggleBtn();
        }

        // ── 설정 버튼
        var settBtnGo = new GameObject("SettingsBtn");
        settBtnGo.transform.SetParent(titleGo.transform, false);
        var settBtnLe = settBtnGo.AddComponent<LayoutElement>();
        settBtnLe.preferredWidth = 18;
        settBtnLe.preferredHeight = 18;
        var settBtnBg = settBtnGo.AddComponent<Image>();
        settBtnBg.color = new Color(1f, 1f, 1f, 0.06f);
        ApplyRoundedSprite(settBtnBg, _roundedSpriteTiny);
        var settBtn = settBtnGo.AddComponent<Button>();
        settBtn.targetGraphic = settBtnBg;
        var settBtnColors = settBtn.colors;
        settBtnColors.highlightedColor = new Color(1.5f, 1.5f, 1.7f, 1f);
        settBtnColors.pressedColor = new Color(0.7f, 0.7f, 0.8f, 1f);
        settBtnColors.fadeDuration = 0.07f;
        settBtn.colors = settBtnColors;
        settBtn.onClick.AddListener(ToggleSettingsSidebar);

        MakeTextChild(settBtnGo.transform, "Icon", font, "☰", 14,
            new Color(0.62f, 0.62f, 0.75f, 1f), TextAnchor.MiddleCenter);
    }

    // 공용 텍스트 자식 생성 헬퍼
    private Text MakeTextChild(Transform parent, string name, Font font, string text,
        int size, Color color, TextAnchor anchor)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        var t = go.AddComponent<Text>();
        t.font = font;
        t.text = text;
        t.fontSize = size;
        t.alignment = anchor;
        t.color = color;
        t.raycastTarget = false;
        return t;
    }

    private void ToggleSettingsSidebar()
    {
        _settingsVisible = !_settingsVisible;
        if (_settingsSidebarGo != null)
            _settingsSidebarGo.SetActive(_settingsVisible);
    }

    private void CreateSearchBar(Transform parent, Font font, float width)
    {
        var barGo = new GameObject("SearchBar");
        barGo.transform.SetParent(parent, false);

        var barLe = barGo.AddComponent<LayoutElement>();
        barLe.preferredWidth = width;
        barLe.preferredHeight = 30;

        var barHlg = barGo.AddComponent<HorizontalLayoutGroup>();
        barHlg.padding = new RectOffset(8, 8, 4, 4);
        barHlg.spacing = 6;
        barHlg.childForceExpandWidth = true;
        barHlg.childForceExpandHeight = true;
        barHlg.childControlWidth = true;
        barHlg.childControlHeight = true;

        var fieldGo = new GameObject("InputField");
        fieldGo.transform.SetParent(barGo.transform, false);

        var fieldImg = fieldGo.AddComponent<Image>();
        fieldImg.color = new Color(0.13f, 0.13f, 0.17f, 0.95f);
        ApplyRoundedSprite(fieldImg, _roundedSpriteSmall);

        var inputField = fieldGo.AddComponent<InputField>();

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(fieldGo.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(8, 0);
        textRt.offsetMax = new Vector2(-8, 0);
        var textComp = textGo.AddComponent<Text>();
        textComp.font = font;
        textComp.fontSize = 11;
        textComp.alignment = TextAnchor.MiddleLeft;
        textComp.color = Color.white;
        textComp.supportRichText = false;

        var placeholderGo = new GameObject("Placeholder");
        placeholderGo.transform.SetParent(fieldGo.transform, false);
        var phRt = placeholderGo.AddComponent<RectTransform>();
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = new Vector2(8, 0);
        phRt.offsetMax = new Vector2(-8, 0);
        var phText = placeholderGo.AddComponent<Text>();
        phText.font = font;
        phText.text = "🔍 검색...";
        phText.fontSize = 11;
        phText.fontStyle = FontStyle.Italic;
        phText.alignment = TextAnchor.MiddleLeft;
        phText.color = new Color(0.5f, 0.5f, 0.6f, 0.8f);

        inputField.textComponent = textComp;
        inputField.placeholder = phText;
        inputField.caretColor = Color.white;
        inputField.selectionColor = new Color(0.3f, 0.5f, 0.8f, 0.5f);

        inputField.onValueChanged.AddListener(query =>
        {
            _searchQuery = query ?? "";
            _thumbnailPage = 0;
            RefreshThumbnailPage();
        });

        _searchInputField = inputField;
    }

    private void CreateSettingsSidebar(Transform parent, Font font)
    {
        float sidebarWidth = 280f;

        _settingsSidebarGo = new GameObject("SettingsSidebar");
        _settingsSidebarGo.transform.SetParent(parent, false);

        var sidebarBg = _settingsSidebarGo.AddComponent<Image>();
        sidebarBg.color = new Color(0.05f, 0.05f, 0.07f, 0.99f);

        var sidebarLe = _settingsSidebarGo.AddComponent<LayoutElement>();
        sidebarLe.preferredWidth = sidebarWidth;

        var sidebarVlg = _settingsSidebarGo.AddComponent<VerticalLayoutGroup>();
        sidebarVlg.spacing = 0;
        sidebarVlg.padding = new RectOffset(0, 0, 0, 16);
        sidebarVlg.childForceExpandWidth = true;
        sidebarVlg.childForceExpandHeight = false;
        sidebarVlg.childControlWidth = true;
        sidebarVlg.childControlHeight = true;
        sidebarVlg.childAlignment = TextAnchor.UpperCenter;

        CreateSidebarHeader(_settingsSidebarGo.transform, font);
        CreateSidebarSectionLabel(_settingsSidebarGo.transform, font, "디스플레이",
            new Color(0.42f, 0.72f, 0.98f, 1f));
        CreateSidebarPresetRow(_settingsSidebarGo.transform, font, "썸네일 크기",
            ThumbnailDisplaySize,
            new[] { "80", "100", "110", "130", "150" },
            new[] { 80, 100, 110, 130, 150 },
            v => SetDataInput(nameof(ThumbnailDisplaySize), v, broadcast: true));
        CreateSidebarDivider(_settingsSidebarGo.transform);
        CreateSidebarSectionLabel(_settingsSidebarGo.transform, font, "렌더링",
            new Color(0.72f, 0.52f, 0.98f, 1f));
        CreateSidebarPresetRow(_settingsSidebarGo.transform, font, "투명도",
            Mathf.RoundToInt(ThumbnailPanelOpacity * 100f),
            new[] { "60%", "70%", "80%", "90%", "100%" },
            new[] { 60, 70, 80, 90, 100 },
            v => SetDataInput(nameof(ThumbnailPanelOpacity), v / 100f, broadcast: true));
        CreateSidebarDivider(_settingsSidebarGo.transform);
        CreateSidebarSectionLabel(_settingsSidebarGo.transform, font, "옵션",
            new Color(0.52f, 0.92f, 0.72f, 1f));
        CreateSidebarToggleRow(_settingsSidebarGo.transform, font, "페이셜 트래킹",
            GetFacialState,
            () => { DoToggleFacialTracking(); UpdateFacialBtnLabel(); });

        _settingsSidebarGo.SetActive(_settingsVisible);
    }

    private void CreateSidebarHeader(Transform parent, Font font)
    {
        var go = new GameObject("SidebarHeader");
        go.transform.SetParent(parent, false);
        var bg = go.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.25f);
        var hLe = go.AddComponent<LayoutElement>();
        hLe.preferredHeight = 26;
        hLe.minHeight = 26;
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(12, 8, 2, 2);
        hlg.spacing = 4;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        var tGo = new GameObject("Title");
        tGo.transform.SetParent(go.transform, false);
        tGo.AddComponent<LayoutElement>().flexibleWidth = 1;
        var t = tGo.AddComponent<Text>();
        t.font = font;
        t.text = "뷰포트 설정";
        t.fontSize = 12;
        t.fontStyle = FontStyle.Bold;
        t.alignment = TextAnchor.MiddleLeft;
        t.color = new Color(0.90f, 0.90f, 0.95f, 1f);
        t.raycastTarget = false;

        // 닫기 버튼
        MakeSidebarIconBtn(go.transform, font, "✕", new Color(0.42f, 0.42f, 0.48f, 1f),
            ToggleSettingsSidebar);
    }

    private void MakeSidebarIconBtn(Transform parent, Font font, string icon,
        Color iconColor, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject($"Btn_{icon}");
        go.transform.SetParent(parent, false);
        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth = 24;
        le.preferredHeight = 22;
        var bg = go.AddComponent<Image>();
        bg.color = new Color(1f, 1f, 1f, 0.04f);
        ApplyRoundedSprite(bg, _roundedSpriteSmall);
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = bg;
        var bc = btn.colors;
        bc.highlightedColor = new Color(1.5f, 1.5f, 1.7f, 1f);
        bc.pressedColor = new Color(0.75f, 0.75f, 0.8f, 1f);
        bc.fadeDuration = 0.07f;
        btn.colors = bc;
        btn.onClick.AddListener(onClick);
        MakeTextChild(go.transform, "T", font, icon, 13, iconColor, TextAnchor.MiddleCenter);
    }

    private void CreateSidebarSectionLabel(Transform parent, Font font, string text,
        Color dotColor = default)
    {
        if (dotColor == default)
            dotColor = new Color(0.52f, 0.52f, 0.65f, 1f);

        var go = new GameObject($"Sec_{text}");
        go.transform.SetParent(parent, false);
        go.AddComponent<LayoutElement>().preferredHeight = 34;
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(18, 18, 0, 0);
        hlg.spacing = 7;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        // 컬러 dot (고정 6×6)
        var dotGo = new GameObject("Dot");
        dotGo.transform.SetParent(go.transform, false);
        var dotLe = dotGo.AddComponent<LayoutElement>();
        dotLe.minWidth = 6;
        dotLe.preferredWidth = 6;
        dotLe.minHeight = 6;
        dotLe.preferredHeight = 6;
        dotLe.flexibleWidth = 0;
        dotLe.flexibleHeight = 0;
        var dotImg = dotGo.AddComponent<Image>();
        dotImg.color = dotColor;
        ApplyRoundedSprite(dotImg, _circleSprite);

        // 레이블 텍스트
        var tGo = new GameObject("L");
        tGo.transform.SetParent(go.transform, false);
        var tLe = tGo.AddComponent<LayoutElement>();
        tLe.flexibleWidth = 1;
        tLe.preferredHeight = 34;
        var t = tGo.AddComponent<Text>();
        t.font = font;
        t.text = text.ToUpper();
        t.fontSize = 9;
        t.fontStyle = FontStyle.Bold;
        t.alignment = TextAnchor.MiddleLeft;
        t.color = new Color(0.44f, 0.46f, 0.60f, 0.95f);
        t.raycastTarget = false;
    }

    private void CreateSidebarDivider(Transform parent)
    {
        var go = new GameObject("Divider");
        go.transform.SetParent(parent, false);
        go.AddComponent<LayoutElement>().preferredHeight = 12;
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(16, 16, 5, 5);
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        var line = new GameObject("Line");
        line.transform.SetParent(go.transform, false);
        var lineImg = line.AddComponent<Image>();
        lineImg.color = new Color(1f, 1f, 1f, 0.04f);
    }

    private void CreateSidebarPresetRow(
        Transform parent, Font font, string label,
        int currentValue, string[] btnLabels, int[] values,
        System.Action<int> onChange)
    {
        var wrap = new GameObject($"PR_{label}");
        wrap.transform.SetParent(parent, false);
        var wrapLe = wrap.AddComponent<LayoutElement>();
        wrapLe.preferredHeight = 40;
        wrapLe.flexibleHeight = 0;
        var vlg = wrap.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(18, 18, 2, 2);
        vlg.spacing = 3;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;

        // 레이블
        var lblGo = new GameObject("L");
        lblGo.transform.SetParent(wrap.transform, false);
        lblGo.AddComponent<LayoutElement>().preferredHeight = 13;
        var lt = lblGo.AddComponent<Text>();
        lt.font = font;
        lt.text = label;
        lt.fontSize = 11;
        lt.alignment = TextAnchor.MiddleLeft;
        lt.color = new Color(0.72f, 0.72f, 0.80f, 1f);
        lt.raycastTarget = false;

        // 버튼 행
        var rowGo = new GameObject("Row");
        rowGo.transform.SetParent(wrap.transform, false);
        var rowLe = rowGo.AddComponent<LayoutElement>();
        rowLe.preferredHeight = 22;
        rowLe.flexibleHeight = 0;
        var rHlg = rowGo.AddComponent<HorizontalLayoutGroup>();
        rHlg.spacing = 4;
        rHlg.childForceExpandWidth = true;
        rHlg.childForceExpandHeight = true;
        rHlg.childControlWidth = true;
        rHlg.childControlHeight = true;

        for (int i = 0; i < btnLabels.Length; i++)
        {
            int val = values[i];
            bool sel = (currentValue == val);

            var bGo = new GameObject($"B_{val}");
            bGo.transform.SetParent(rowGo.transform, false);
            var bBg = bGo.AddComponent<Image>();
            bBg.color = sel
                ? new Color(0.50f, 0.32f, 0.88f, 0.95f)
                : new Color(0.13f, 0.13f, 0.17f, 0.95f);
            ApplyRoundedSprite(bBg, _roundedSpriteSmall);

            var bBtn = bGo.AddComponent<Button>();
            bBtn.targetGraphic = bBg;
            var bc = bBtn.colors;
            bc.highlightedColor = new Color(1.3f, 1.3f, 1.4f, 1f);
            bc.pressedColor = new Color(0.78f, 0.78f, 0.85f, 1f);
            bc.fadeDuration = 0.07f;
            bBtn.colors = bc;
            bBtn.onClick.AddListener(() => onChange(val));

            var bT = MakeTextChild(bGo.transform, "T", font, btnLabels[i],
                sel ? 11 : 10,
                sel ? Color.white : new Color(0.48f, 0.50f, 0.62f, 1f),
                TextAnchor.MiddleCenter);
            if (sel) bT.fontStyle = FontStyle.Bold;
        }
    }

    private void CreateSidebarSliderRow(
        Transform parent,
        Font font,
        string label,
        int currentValue,
        int min,
        int max,
        System.Action<int> onChange
    )
    {
        var wrap = new GameObject($"S_{label}");
        wrap.transform.SetParent(parent, false);
        wrap.AddComponent<LayoutElement>().preferredHeight = 54;
        var vlg = wrap.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(18, 18, 4, 4);
        vlg.spacing = 5;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;

        var lbl = new GameObject("L");
        lbl.transform.SetParent(wrap.transform, false);
        lbl.AddComponent<LayoutElement>().preferredHeight = 15;
        var lt = lbl.AddComponent<Text>();
        lt.font = font;
        lt.text = label;
        lt.fontSize = 11;
        lt.alignment = TextAnchor.MiddleLeft;
        lt.color = new Color(0.72f, 0.72f, 0.8f, 1f);
        lt.raycastTarget = false;

        var row = new GameObject("R");
        row.transform.SetParent(wrap.transform, false);
        row.AddComponent<LayoutElement>().preferredHeight = 24;
        var rHlg = row.AddComponent<HorizontalLayoutGroup>();
        rHlg.spacing = 8;
        rHlg.childForceExpandWidth = false;
        rHlg.childForceExpandHeight = true;
        rHlg.childControlWidth = false;
        rHlg.childControlHeight = true;
        rHlg.childAlignment = TextAnchor.MiddleLeft;

        var sGo = new GameObject("Slider");
        sGo.transform.SetParent(row.transform, false);
        sGo.AddComponent<LayoutElement>().flexibleWidth = 1;

        var bg = new GameObject("Bg");
        bg.transform.SetParent(sGo.transform, false);
        var bgRt = bg.AddComponent<RectTransform>();
        bgRt.anchorMin = new Vector2(0, 0.42f);
        bgRt.anchorMax = new Vector2(1, 0.58f);
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;
        var bgI = bg.AddComponent<Image>();
        bgI.color = new Color(0.22f, 0.22f, 0.3f, 1f);
        ApplyRoundedSprite(bgI, _roundedSpriteSmall);

        var fa = new GameObject("FA");
        fa.transform.SetParent(sGo.transform, false);
        var faRt = fa.AddComponent<RectTransform>();
        faRt.anchorMin = new Vector2(0, 0.42f);
        faRt.anchorMax = new Vector2(1, 0.58f);
        faRt.offsetMin = Vector2.zero;
        faRt.offsetMax = Vector2.zero;

        var fill = new GameObject("Fill");
        fill.transform.SetParent(fa.transform, false);
        var fRt = fill.AddComponent<RectTransform>();
        fRt.anchorMin = Vector2.zero;
        fRt.anchorMax = Vector2.one;
        fRt.offsetMin = Vector2.zero;
        fRt.offsetMax = Vector2.zero;
        var fI = fill.AddComponent<Image>();
        fI.color = new Color(0.55f, 0.36f, 0.96f, 0.8f);
        ApplyRoundedSprite(fI, _roundedSpriteSmall);

        var ha = new GameObject("HA");
        ha.transform.SetParent(sGo.transform, false);
        var haRt = ha.AddComponent<RectTransform>();
        haRt.anchorMin = Vector2.zero;
        haRt.anchorMax = Vector2.one;
        haRt.offsetMin = new Vector2(5, 0);
        haRt.offsetMax = new Vector2(-5, 0);

        var h = new GameObject("H");
        h.transform.SetParent(ha.transform, false);
        var hRt = h.AddComponent<RectTransform>();
        hRt.anchorMin = new Vector2(0, 0.5f);
        hRt.anchorMax = new Vector2(0, 0.5f);
        hRt.pivot = new Vector2(0.5f, 0.5f);
        hRt.sizeDelta = new Vector2(16, 16);
        var hI = h.AddComponent<Image>();
        hI.color = new Color(0.55f, 0.36f, 0.96f, 1f);
        ApplyRoundedSprite(hI, _circleSprite);

        var hInner = new GameObject("HInner");
        hInner.transform.SetParent(h.transform, false);
        var hInnerRt = hInner.AddComponent<RectTransform>();
        hInnerRt.anchorMin = Vector2.zero;
        hInnerRt.anchorMax = Vector2.one;
        hInnerRt.offsetMin = new Vector2(2.5f, 2.5f);
        hInnerRt.offsetMax = new Vector2(-2.5f, -2.5f);
        var hInnerI = hInner.AddComponent<Image>();
        hInnerI.color = new Color(0.95f, 0.95f, 0.97f, 1f);
        hInnerI.raycastTarget = false;
        ApplyRoundedSprite(hInnerI, _circleSprite);

        var slider = sGo.AddComponent<Slider>();
        slider.targetGraphic = hI;
        slider.fillRect = fRt;
        slider.handleRect = hRt;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = min;
        slider.maxValue = max;
        slider.wholeNumbers = true;
        slider.value = currentValue;

        var vBg = new GameObject("VBg");
        vBg.transform.SetParent(row.transform, false);
        var vLe = vBg.AddComponent<LayoutElement>();
        vLe.preferredWidth = 52;
        vLe.preferredHeight = 26;
        var vBgI = vBg.AddComponent<Image>();
        vBgI.color = new Color(0.08f, 0.08f, 0.11f, 1f);
        ApplyRoundedSprite(vBgI, _roundedSpriteSmall);

        var sh = vBg.AddComponent<Shadow>();
        sh.effectColor = new Color(0f, 0f, 0f, 0.2f);
        sh.effectDistance = new Vector2(0, -1);

        var vT = new GameObject("V");
        vT.transform.SetParent(vBg.transform, false);
        var vRt = vT.AddComponent<RectTransform>();
        vRt.anchorMin = Vector2.zero;
        vRt.anchorMax = Vector2.one;
        vRt.offsetMin = Vector2.zero;
        vRt.offsetMax = Vector2.zero;
        var vTxt = vT.AddComponent<Text>();
        vTxt.font = font;
        vTxt.text = currentValue.ToString();
        vTxt.fontSize = 12;
        vTxt.fontStyle = FontStyle.Bold;
        vTxt.alignment = TextAnchor.MiddleCenter;
        vTxt.color = new Color(0.88f, 0.88f, 0.94f, 1f);
        vTxt.raycastTarget = false;

        slider.onValueChanged.AddListener(v =>
        {
            vTxt.text = Mathf.RoundToInt(v).ToString();
            onChange(Mathf.RoundToInt(v));
        });
    }

    private void CreateSidebarToggleRow(
        Transform parent,
        Font font,
        string label,
        Func<bool> getState,
        UnityEngine.Events.UnityAction onToggle
    )
    {
        var go = new GameObject($"T_{label}");
        go.transform.SetParent(parent, false);
        var tLe = go.AddComponent<LayoutElement>();
        tLe.preferredHeight = 28;
        tLe.minHeight = 28;
        tLe.flexibleHeight = 0;
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(18, 18, 2, 2);
        hlg.spacing = 10;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        var l = new GameObject("L");
        l.transform.SetParent(go.transform, false);
        l.AddComponent<LayoutElement>().flexibleWidth = 1;
        var lt = l.AddComponent<Text>();
        lt.font = font;
        lt.text = label;
        lt.fontSize = 11;
        lt.alignment = TextAnchor.MiddleLeft;
        lt.color = new Color(0.72f, 0.72f, 0.8f, 1f);
        lt.raycastTarget = false;

        bool state = getState();
        var tGo = new GameObject("Btn");
        tGo.transform.SetParent(go.transform, false);
        tGo.AddComponent<LayoutElement>().preferredWidth = 32;
        var tBg = tGo.AddComponent<Image>();
        tBg.color = state ? new Color(0.55f, 0.36f, 0.96f, 1f) : new Color(0.28f, 0.28f, 0.35f, 1f);
        ApplyRoundedSprite(tBg, _roundedSpriteSmall);
        var tBtn = tGo.AddComponent<Button>();
        tBtn.targetGraphic = tBg;

        var knob = new GameObject("K");
        knob.transform.SetParent(tGo.transform, false);
        var kRt = knob.AddComponent<RectTransform>();
        kRt.sizeDelta = new Vector2(14, 14);
        kRt.anchorMin = new Vector2(state ? 1f : 0f, 0.5f);
        kRt.anchorMax = new Vector2(state ? 1f : 0f, 0.5f);
        kRt.anchoredPosition = new Vector2(state ? -9f : 9f, 0f);
        var kI = knob.AddComponent<Image>();
        kI.color = Color.white;
        ApplyRoundedSprite(kI, _roundedSpriteSmall);

        tBtn.onClick.AddListener(() =>
        {
            onToggle();
            bool ns = getState();
            tBg.color = ns
                ? new Color(0.55f, 0.36f, 0.96f, 1f)
                : new Color(0.28f, 0.28f, 0.35f, 1f);
            kRt.anchorMin = new Vector2(ns ? 1f : 0f, 0.5f);
            kRt.anchorMax = new Vector2(ns ? 1f : 0f, 0.5f);
            kRt.anchoredPosition = new Vector2(ns ? -9f : 9f, 0f);
        });
    }

    private List<int> GetFilteredPoseIndices()
    {
        var poses = Poses;
        if (poses == null || poses.Length == 0)
            return new List<int>();

        var result = new List<int>();
        var q = (_searchQuery ?? "").Trim();

        for (int i = 0; i < poses.Length; i++)
        {
            if (string.IsNullOrEmpty(q))
            {
                result.Add(i);
                continue;
            }
            var header = poses[i]?.GetHeader() ?? "";
            var anim = poses[i]?.Animation ?? "";
            if (
                header.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
                || anim.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
            )
                result.Add(i);
        }
        return result;
    }

    private void BuildThumbnailCache()
    {
        _thumbnailMapCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var pdm = Context.PersistentDataManager;
        Debug.Log($"[SelView Tool] 썸네일 캐시 구축 시작 - basePath: {pdm.GetBasePath()}");

        foreach (var searchPath in _thumbnailSearchPaths)
        {
            try
            {
                var files = pdm.GetFileEntries(searchPath, "*.png");
                int count = 0;
                foreach (var file in files)
                {
                    var fn = file.fileName;
                    var dotIdx = fn.LastIndexOf('.');
                    var nameOnly = dotIdx > 0 ? fn.Substring(0, dotIdx) : fn;
                    if (!_thumbnailMapCache.ContainsKey(nameOnly))
                        _thumbnailMapCache[nameOnly] = file.path;
                    count++;
                }
                Debug.Log($"[SelView Tool]   → {searchPath}: {count}개 PNG 발견");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[SelView Tool]   → {searchPath}: 실패 - {e.Message}");
            }
        }

        Debug.Log($"[SelView Tool] 총 썸네일 캐시: {_thumbnailMapCache.Count}개");
    }

    private int GetTotalPages()
    {
        int count = GetFilteredPoseIndices().Count;
        if (count == 0)
            return 1;
        int perPage = Mathf.Max(ThumbnailItemsPerPage, 4);
        return Mathf.CeilToInt((float)count / perPage);
    }

    private void CreateBottomBar(Transform parent, Font font, float panelWidth, bool showPageNav)
    {
        var barGo = new GameObject("BottomBar");
        barGo.transform.SetParent(parent, false);

        var barBg = barGo.AddComponent<Image>();
        barBg.color = new Color(0.055f, 0.055f, 0.075f, 1f);

        var barLe = barGo.AddComponent<LayoutElement>();
        barLe.preferredWidth = panelWidth;
        barLe.preferredHeight = 54;

        var barHlg = barGo.AddComponent<HorizontalLayoutGroup>();
        barHlg.spacing = 6;
        barHlg.padding = new RectOffset(14, 14, 9, 9);
        barHlg.childForceExpandWidth = false;
        barHlg.childForceExpandHeight = true;
        barHlg.childControlWidth = false;
        barHlg.childControlHeight = true;
        barHlg.childAlignment = TextAnchor.MiddleLeft;

        // ── 페이지 네비게이션 (항상 왼쪽)
        {
            var pgGo = new GameObject("PageNav");
            pgGo.transform.SetParent(barGo.transform, false);
            var pgBg = pgGo.AddComponent<Image>();
            pgBg.color = new Color(1f, 1f, 1f, 0.05f);
            ApplyRoundedSprite(pgBg, _roundedSpriteSmall);
            var pgLe = pgGo.AddComponent<LayoutElement>();
            pgLe.preferredWidth = 108;
            var pgHlg = pgGo.AddComponent<HorizontalLayoutGroup>();
            pgHlg.spacing = 0;
            pgHlg.padding = new RectOffset(2, 2, 0, 0);
            pgHlg.childForceExpandWidth = true;
            pgHlg.childForceExpandHeight = true;
            pgHlg.childControlWidth = true;
            pgHlg.childControlHeight = true;

            CreateNavBtn(pgGo.transform, "<", font, () => ChangeThumbnailPage(-1));

            var lblGo = new GameObject("PageLabel");
            lblGo.transform.SetParent(pgGo.transform, false);
            _pageLabel = lblGo.AddComponent<Text>();
            _pageLabel.font = font;
            _pageLabel.fontSize = 12;
            _pageLabel.alignment = TextAnchor.MiddleCenter;
            _pageLabel.color = new Color(0.82f, 0.82f, 0.88f, 1f);
            _pageLabel.raycastTarget = false;
            UpdatePageLabel();

            CreateNavBtn(pgGo.transform, ">", font, () => ChangeThumbnailPage(1));
        }

        var spacer = new GameObject("Sp");
        spacer.transform.SetParent(barGo.transform, false);
        spacer.AddComponent<LayoutElement>().flexibleWidth = 1;

        // ── 이전 / 재생 / 다음 버튼 그룹
        {
            var grpGo = new GameObject("PlayGrp");
            grpGo.transform.SetParent(barGo.transform, false);
            var grpBg = grpGo.AddComponent<Image>();
            grpBg.color = new Color(0.50f, 0.32f, 0.88f, 0.95f);
            ApplyRoundedSprite(grpBg, _roundedSpriteSmall);
            var grpLe = grpGo.AddComponent<LayoutElement>();
            grpLe.preferredWidth = 70;
            var grpHlg = grpGo.AddComponent<HorizontalLayoutGroup>();
            grpHlg.spacing = 0;
            grpHlg.padding = new RectOffset(0, 0, 0, 0);
            grpHlg.childForceExpandWidth = true;
            grpHlg.childForceExpandHeight = true;
            grpHlg.childControlWidth = true;
            grpHlg.childControlHeight = true;

            // 이전 (<)
            var prevGo = new GameObject("PrevBtn");
            prevGo.transform.SetParent(grpGo.transform, false);
            var prevBg = prevGo.AddComponent<Image>();
            prevBg.color = new Color(1f, 1f, 1f, 0f);
            var prevBtn = prevGo.AddComponent<Button>();
            prevBtn.targetGraphic = prevBg;
            var prevC = prevBtn.colors;
            prevC.highlightedColor = new Color(1.4f, 1.4f, 1.5f, 1f);
            prevC.pressedColor = new Color(0.7f, 0.7f, 0.8f, 1f);
            prevC.fadeDuration = 0.06f;
            prevBtn.colors = prevC;
            prevBtn.onClick.AddListener(ApplyPreviousPose);
            MakeTextChild(prevGo.transform, "T", font, "<", 14,
                Color.white, TextAnchor.MiddleCenter);

            // 재생 (▶)
            var playGo = new GameObject("PlayBtn");
            playGo.transform.SetParent(grpGo.transform, false);
            var playBg = playGo.AddComponent<Image>();
            playBg.color = new Color(1f, 1f, 1f, 0f);
            var playBtn = playGo.AddComponent<Button>();
            playBtn.targetGraphic = playBg;
            var playC = playBtn.colors;
            playC.highlightedColor = new Color(1.4f, 1.4f, 1.5f, 1f);
            playC.pressedColor = new Color(0.7f, 0.7f, 0.8f, 1f);
            playC.fadeDuration = 0.06f;
            playBtn.colors = playC;
            playBtn.onClick.AddListener(ApplyNextPose);
            MakeTextChild(playGo.transform, "T", font, "▶", 14,
                Color.white, TextAnchor.MiddleCenter);

            // 다음 (>)
            var nextGo = new GameObject("NextBtn");
            nextGo.transform.SetParent(grpGo.transform, false);
            var nextBg = nextGo.AddComponent<Image>();
            nextBg.color = new Color(1f, 1f, 1f, 0f);
            var nextBtn = nextGo.AddComponent<Button>();
            nextBtn.targetGraphic = nextBg;
            var nextC = nextBtn.colors;
            nextC.highlightedColor = new Color(1.4f, 1.4f, 1.5f, 1f);
            nextC.pressedColor = new Color(0.7f, 0.7f, 0.8f, 1f);
            nextC.fadeDuration = 0.06f;
            nextBtn.colors = nextC;
            nextBtn.onClick.AddListener(ApplyNextPose);
            MakeTextChild(nextGo.transform, "T", font, ">", 14,
                Color.white, TextAnchor.MiddleCenter);
        }

        // ── 삭제 버튼
        {
            var dGo = new GameObject("DeleteBtn");
            dGo.transform.SetParent(barGo.transform, false);
            var dBg = dGo.AddComponent<Image>();
            dBg.color = new Color(0.55f, 0.16f, 0.16f, 0.9f);
            ApplyRoundedSprite(dBg, _roundedSpriteSmall);
            var dLe = dGo.AddComponent<LayoutElement>();
            dLe.preferredWidth = 70;
            var dBtn = dGo.AddComponent<Button>();
            dBtn.targetGraphic = dBg;
            var dBtnC = dBtn.colors;
            dBtnC.highlightedColor = new Color(1.3f, 1.3f, 1.3f, 1f);
            dBtnC.pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
            dBtnC.fadeDuration = 0.07f;
            dBtn.colors = dBtnC;
            dBtn.onClick.AddListener(DeleteSelectedPose);
            MakeTextChild(dGo.transform, "T", font, "삭제", 12,
                new Color(1f, 0.72f, 0.72f, 1f), TextAnchor.MiddleCenter);
        }

    }

    private void CreateNavBtn(Transform parent, string label, Font font,
        UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject($"Nav_{label}");
        go.transform.SetParent(parent, false);
        var bg = go.AddComponent<Image>();
        bg.color = new Color(1f, 1f, 1f, 0f);
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = bg;
        var bc = btn.colors;
        bc.normalColor = Color.white;
        bc.highlightedColor = new Color(1.6f, 1.6f, 1.8f, 1f);
        bc.pressedColor = new Color(0.7f, 0.7f, 0.8f, 1f);
        bc.fadeDuration = 0.06f;
        btn.colors = bc;
        btn.onClick.AddListener(onClick);
        MakeTextChild(go.transform, "T", font, label, 13,
            new Color(0.65f, 0.65f, 0.75f, 1f), TextAnchor.MiddleCenter);
    }

    private void ApplySelectedPose()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ 캐릭터를 선택하세요.");
            return;
        }
        if (_highlightedPoseIndex < 0 || Poses == null || _highlightedPoseIndex >= Poses.Length)
        {
            UpdateStatus("⚠️ 포즈를 먼저 선택하세요.");
            return;
        }
        var preset = Poses[_highlightedPoseIndex];
        if (preset == null || string.IsNullOrEmpty(preset.Animation))
            return;
        ApplyPose(preset);
        UpdateStatus($"✅ 적용: {preset.GetHeader()}");
    }

    private void DeleteSelectedPose()
    {
        if (_highlightedPoseIndex < 0 || Poses == null || _highlightedPoseIndex >= Poses.Length)
            return;

        var list = new List<PosePreset>(Poses);
        list.RemoveAt(_highlightedPoseIndex);
        SetDataInput(nameof(Poses), list.ToArray(), broadcast: true);

        if (_highlightedPoseIndex >= list.Count)
            _highlightedPoseIndex = list.Count - 1;

        UpdatePoseCountInfo();
        RebuildThumbnailPanel();
        UpdateStatus($"🗑 포즈 #{_highlightedPoseIndex + 2} 삭제됨 (남은 {list.Count}개)");
    }

    private Text CreateBarButton(
        Transform parent,
        string label,
        Font font,
        UnityEngine.Events.UnityAction onClick,
        float width,
        bool accent,
        bool danger = false,
        bool purple = false,
        bool white = false
    )
    {
        var btnGo = new GameObject($"Btn_{label}");
        btnGo.transform.SetParent(parent, false);

        var btnLe = btnGo.AddComponent<LayoutElement>();
        btnLe.preferredWidth = width;

        var btnImg = btnGo.AddComponent<Image>();
        if (white)
            btnImg.color = new Color(0.88f, 0.88f, 0.92f, 0.95f);
        else if (purple)
            btnImg.color = new Color(0.5f, 0.32f, 0.82f, 0.92f);
        else if (danger)
            btnImg.color = new Color(0.6f, 0.18f, 0.18f, 0.9f);
        else if (accent)
            btnImg.color = new Color(0.2f, 0.35f, 0.7f, 0.9f);
        else
            btnImg.color = new Color(1f, 1f, 1f, 0.06f);
        ApplyRoundedSprite(btnImg, _roundedSpriteSmall);

        var btn = btnGo.AddComponent<Button>();
        var colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(1.25f, 1.25f, 1.3f, 1f);
        colors.pressedColor = new Color(0.78f, 0.78f, 0.82f, 1f);
        colors.fadeDuration = 0.07f;
        btn.colors = colors;
        btn.targetGraphic = btnImg;
        btn.onClick.AddListener(onClick);

        var txtGo = new GameObject("Text");
        txtGo.transform.SetParent(btnGo.transform, false);
        var txtRt = txtGo.AddComponent<RectTransform>();
        txtRt.anchorMin = Vector2.zero;
        txtRt.anchorMax = Vector2.one;
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;

        var txt = txtGo.AddComponent<Text>();
        txt.font = font;
        txt.text = label;
        txt.fontSize = (purple || white) ? 13 : 11;
        txt.fontStyle = (purple || white) ? FontStyle.Bold : FontStyle.Normal;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = white ? new Color(0.1f, 0.1f, 0.14f, 1f) : Color.white;
        txt.raycastTarget = false;

        return txt;
    }

    private void ChangeThumbnailPage(int delta)
    {
        int total = GetTotalPages();
        _thumbnailPage = (_thumbnailPage + delta + total) % total;
        RefreshThumbnailPage();
    }

    private void RefreshThumbnailPage()
    {
        ClearThumbnailGrid();
        PopulateCurrentPage(ThumbnailDisplaySize);
        UpdatePageLabel();
    }

    private void ClearThumbnailGrid()
    {
        if (_emptyStateGo != null)
        {
            UnityEngine.Object.Destroy(_emptyStateGo);
            _emptyStateGo = null;
        }
        if (_thumbnailGridParent == null)
            return;
        for (int i = _thumbnailGridParent.childCount - 1; i >= 0; i--)
            UnityEngine.Object.Destroy(_thumbnailGridParent.GetChild(i).gameObject);
        foreach (var tex in _loadedThumbnailTextures)
            if (tex != null)
                UnityEngine.Object.Destroy(tex);
        _loadedThumbnailTextures.Clear();
        _thumbnailBorders.Clear();
    }

    private void UpdatePageLabel()
    {
        if (_pageLabel == null)
            return;
        int total = GetTotalPages();
        _pageLabel.text = $"{_thumbnailPage + 1} / {total}";
    }

    private void UpdateSelectionInfoText()
    {
        int total = Poses?.Length ?? 0;

        if (_totalCountText != null)
            _totalCountText.text = $"({total})";

        if (_selectionInfoText == null)
            return;
        if (_highlightedPoseIndex >= 0 && Poses != null && _highlightedPoseIndex < Poses.Length)
        {
            var header = Poses[_highlightedPoseIndex]?.GetHeader() ?? "";
            _selectionInfoText.text = $"선택:  {header}";
            _selectionInfoText.color = new Color(0.82f, 0.74f, 0.98f, 1f);
        }
        else
        {
            _selectionInfoText.text = "포즈를 선택하세요";
            _selectionInfoText.color = new Color(0.44f, 0.40f, 0.58f, 0.8f);
        }
    }

    private void PopulateCurrentPage(int size)
    {
        var poses = Poses;
        if (poses == null || _thumbnailGridParent == null)
            return;

        var filtered = GetFilteredPoseIndices();
        int perPage = Mathf.Max(ThumbnailItemsPerPage, 4);
        int startIdx = _thumbnailPage * perPage;
        int endIdx = Mathf.Min(startIdx + perPage, filtered.Count);

        var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // 빈 검색 결과 메시지 (그리드 부모의 부모에 배치)
        if (_emptyStateGo != null)
            UnityEngine.Object.Destroy(_emptyStateGo);

        if (filtered.Count == 0 && !string.IsNullOrEmpty(_searchQuery))
        {
            _emptyStateGo = new GameObject("EmptyState");
            _emptyStateGo.transform.SetParent(_thumbnailGridParent.parent, false);
            var emLe = _emptyStateGo.AddComponent<LayoutElement>();
            emLe.preferredHeight = 80;
            var emT = _emptyStateGo.AddComponent<Text>();
            emT.font = font;
            emT.text = $"'{_searchQuery}' 검색 결과 없음";
            emT.fontSize = 12;
            emT.alignment = TextAnchor.MiddleCenter;
            emT.color = new Color(0.38f, 0.40f, 0.52f, 0.85f);
            emT.raycastTarget = false;
            return;
        }

        for (int fi = startIdx; fi < endIdx; fi++)
        {
            int i = filtered[fi];
            if (poses[i] == null)
                continue;

            var itemGo = new GameObject($"Thumb_{i}");
            itemGo.transform.SetParent(_thumbnailGridParent, false);

            var borderImg = itemGo.AddComponent<Image>();
            borderImg.color = new Color(0.18f, 0.18f, 0.22f, 0.7f);
            _thumbnailBorders.Add(borderImg);

            int poseIndex = i;
            var btn = itemGo.AddComponent<Button>();
            var btnColors = btn.colors;
            btnColors.normalColor = Color.white;
            btnColors.highlightedColor = new Color(1.5f, 1.5f, 1.5f, 1f);
            btnColors.pressedColor = new Color(0.8f, 0.8f, 1f, 1f);
            btnColors.fadeDuration = 0.08f;
            btn.colors = btnColors;
            btn.targetGraphic = borderImg;
            btn.onClick.AddListener(() => OnThumbnailClicked(poseIndex));

            var thumbGo = new GameObject("Img");
            thumbGo.transform.SetParent(itemGo.transform, false);
            var thumbRt = thumbGo.AddComponent<RectTransform>();
            thumbRt.anchorMin = Vector2.zero;
            thumbRt.anchorMax = Vector2.one;
            thumbRt.offsetMin = new Vector2(4, 4);
            thumbRt.offsetMax = new Vector2(-4, -4);

            var thumbImg = thumbGo.AddComponent<Image>();
            thumbImg.color = new Color(0.12f, 0.12f, 0.16f, 1f);
            thumbImg.raycastTarget = false;

            var animUri = poses[i].Animation;
            if (!string.IsNullOrEmpty(animUri))
            {
                LoadThumbnailSmart(animUri, thumbImg).Forget();
            }

            // 번호 pill 뱃지 (우하단)
            var numPill = new GameObject("NumBadge");
            numPill.transform.SetParent(itemGo.transform, false);
            var numPillRt = numPill.AddComponent<RectTransform>();
            numPillRt.anchorMin = new Vector2(1, 0);
            numPillRt.anchorMax = new Vector2(1, 0);
            numPillRt.pivot = new Vector2(1f, 0f);
            numPillRt.anchoredPosition = new Vector2(-5f, 5f);
            numPillRt.sizeDelta = new Vector2(28, 16);
            var numPillBg = numPill.AddComponent<Image>();
            numPillBg.color = new Color(0f, 0f, 0f, 0.55f);
            ApplyRoundedSprite(numPillBg, _roundedSpriteSmall);
            numPillBg.raycastTarget = false;

            var numTxtGo = new GameObject("T");
            numTxtGo.transform.SetParent(numPill.transform, false);
            var numTxtRt = numTxtGo.AddComponent<RectTransform>();
            numTxtRt.anchorMin = Vector2.zero;
            numTxtRt.anchorMax = Vector2.one;
            numTxtRt.offsetMin = Vector2.zero;
            numTxtRt.offsetMax = Vector2.zero;
            var numTxt = numTxtGo.AddComponent<Text>();
            numTxt.font = font;
            numTxt.text = $"#{i + 1}";
            numTxt.fontSize = 9;
            numTxt.alignment = TextAnchor.MiddleCenter;
            numTxt.color = new Color(0.78f, 0.78f, 0.85f, 0.9f);
            numTxt.raycastTarget = false;
        }

        UpdateThumbnailHighlight(_highlightedPoseIndex);
    }

    private void OnThumbnailClicked(int poseIndex)
    {
        var poses = Poses;
        if (poses == null || poseIndex < 0 || poseIndex >= poses.Length)
            return;
        if (Character == null)
            return;

        var preset = poses[poseIndex];
        if (preset == null || string.IsNullOrEmpty(preset.Animation))
            return;

        ApplyPose(preset);
        _highlightedPoseIndex = poseIndex;
        UpdateThumbnailHighlight(poseIndex);
        UpdateStatus($"👆 포즈: **{preset.GetHeader()}**");
    }

    private void EnsureThumbnailEventSystem()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null)
            return;
        var go = new GameObject("SelViewTool_EventSystem");
        go.AddComponent<UnityEngine.EventSystems.EventSystem>();
        go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        UnityEngine.Object.DontDestroyOnLoad(go);
    }

    private string AnimUriToThumbnailPath(string animUri)
    {
        // character-animation://resources/Animations/AGIA/03_Others/AGIA_Other_cat_01_emote_01
        // → Clients/Thumbnails/Animations/AGIA/03_Others/AGIA_Other_cat_01_emote_01.png
        const string scheme = "character-animation://";
        var path = animUri;
        if (path.StartsWith(scheme))
            path = path.Substring(scheme.Length);
        if (path.StartsWith("resources/"))
            path = path.Substring("resources/".Length);
        return "Clients/Thumbnails/" + path + ".png";
    }

    private async UniTaskVoid LoadThumbnailSmart(string animUri, Image thumbImg)
    {
        byte[] bytes = null;
        await UniTask.SwitchToMainThread();
        var basePath = Context.PersistentDataManager.GetBasePath();

        // 1차: file:// 로 StreamingAssets에서 직접 읽기 (내장 애니메이션)
        try
        {
            var thumbRelPath = AnimUriToThumbnailPath(animUri);
            var fileUrl = "file:///" + basePath + thumbRelPath;
            using var www = UnityWebRequest.Get(fileUrl);
            www.timeout = 3;
            await www.SendWebRequest();
            if (
                www.result == UnityWebRequest.Result.Success
                && www.downloadHandler?.data?.Length > 0
            )
                bytes = www.downloadHandler.data;
        }
        catch { }

        // 2차: HTTP API 로 썸네일 가져오기 (Steam Workshop 모드 등)
        if (bytes == null || bytes.Length == 0)
        {
            try
            {
                await UniTask.SwitchToMainThread();
                var apiUrl =
                    $"http://localhost:{Service.Port}/api/thumbnail?uri={Uri.EscapeDataString(animUri)}";
                using var www = UnityWebRequest.Get(apiUrl);
                www.timeout = 5;
                await www.SendWebRequest();
                if (
                    www.result == UnityWebRequest.Result.Success
                    && www.downloadHandler?.data?.Length > 0
                )
                    bytes = www.downloadHandler.data;
            }
            catch { }
        }

        // 3차: 파일 캐시 폴백 (커스텀 PoseThumbnailKit)
        if ((bytes == null || bytes.Length == 0) && _thumbnailMapCache != null)
        {
            var animName = ExtractAnimNameFromUri(animUri);
            if (
                !string.IsNullOrEmpty(animName)
                && _thumbnailMapCache.TryGetValue(animName, out var thumbPath)
            )
            {
                try
                {
                    await UniTask.SwitchToMainThread();
                    var fileUrl = "file:///" + basePath + thumbPath;
                    using var www = UnityWebRequest.Get(fileUrl);
                    www.timeout = 3;
                    await www.SendWebRequest();
                    if (
                        www.result == UnityWebRequest.Result.Success
                        && www.downloadHandler?.data?.Length > 0
                    )
                        bytes = www.downloadHandler.data;
                }
                catch { }
            }
        }

        await UniTask.SwitchToMainThread();
        if (bytes == null || bytes.Length == 0 || thumbImg == null)
            return;

        var tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        tex = DownscaleTexture(tex, Mathf.Clamp(ThumbnailMaxTextureSize, 64, 512));
        _loadedThumbnailTextures.Add(tex);
        thumbImg.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );
        thumbImg.color = Color.white;
        thumbImg.preserveAspect = true;
    }

    private Texture2D DownscaleTexture(Texture2D original, int maxSize)
    {
        if (original.width <= maxSize && original.height <= maxSize)
            return original;

        float scale = Mathf.Min((float)maxSize / original.width, (float)maxSize / original.height);
        int newW = Mathf.Max(1, Mathf.RoundToInt(original.width * scale));
        int newH = Mathf.Max(1, Mathf.RoundToInt(original.height * scale));

        var rt = RenderTexture.GetTemporary(newW, newH, 0, RenderTextureFormat.ARGB32);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        Graphics.Blit(original, rt);

        var downscaled = new Texture2D(newW, newH, TextureFormat.RGBA32, false);
        downscaled.ReadPixels(new Rect(0, 0, newW, newH), 0, 0);
        downscaled.Apply(false, true);

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        UnityEngine.Object.Destroy(original);

        return downscaled;
    }

    private void SetThumbnailPanelAnchor(RectTransform rt)
    {
        float m = 20f;
        switch (PanelAnchor)
        {
            case ThumbnailPanelAnchor.TopLeft:
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2(m, -m);
                break;
            case ThumbnailPanelAnchor.TopRight:
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1, 1);
                rt.anchoredPosition = new Vector2(-m, -m);
                break;
            case ThumbnailPanelAnchor.BottomRight:
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1, 0);
                rt.anchoredPosition = new Vector2(-m, m);
                break;
            default:
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0);
                rt.anchoredPosition = new Vector2(m, m);
                break;
        }
    }

    private void UpdateThumbnailHighlight(int activeIndex)
    {
        _highlightedPoseIndex = activeIndex;
        var filtered = GetFilteredPoseIndices();
        int perPage = Mathf.Max(ThumbnailItemsPerPage, 4);
        int startIdx = _thumbnailPage * perPage;

        for (int i = 0; i < _thumbnailBorders.Count; i++)
        {
            if (_thumbnailBorders[i] == null)
                continue;
            int fi = startIdx + i;
            int globalIdx = fi < filtered.Count ? filtered[fi] : -1;
            bool selected = globalIdx == activeIndex;
            _thumbnailBorders[i].color = selected
                ? new Color(0.65f, 0.45f, 1f, 1f)
                : new Color(0.18f, 0.18f, 0.22f, 0.7f);
        }

        UpdateSelectionInfoText();
    }

    private void DestroyThumbnailPanel()
    {
        if (_thumbnailPanelRoot != null)
        {
            UnityEngine.Object.Destroy(_thumbnailPanelRoot);
            _thumbnailPanelRoot = null;
        }
        foreach (var tex in _loadedThumbnailTextures)
            if (tex != null)
                UnityEngine.Object.Destroy(tex);
        _loadedThumbnailTextures.Clear();
        _thumbnailBorders.Clear();
        _thumbnailGridParent = null;
        _pageLabel = null;
        _selectionInfoText = null;
        _totalCountText = null;
        _nextBtnText = null;
        _facialBtnText = null;
        _modeSeqText = null;
        _modeSeqImg = null;
        _modeRndText = null;
        _modeRndImg = null;
        _emptyStateGo = null;
        _searchInputField = null;
        _searchQuery = "";
        _thumbnailMapCache = null;
        if (_roundedSprite != null)
        {
            if (_roundedSprite.texture != null)
                UnityEngine.Object.Destroy(_roundedSprite.texture);
            UnityEngine.Object.Destroy(_roundedSprite);
            _roundedSprite = null;
        }
        if (_roundedSpriteSmall != null)
        {
            if (_roundedSpriteSmall.texture != null)
                UnityEngine.Object.Destroy(_roundedSpriteSmall.texture);
            UnityEngine.Object.Destroy(_roundedSpriteSmall);
            _roundedSpriteSmall = null;
        }
        if (_circleSprite != null)
        {
            if (_circleSprite.texture != null)
                UnityEngine.Object.Destroy(_circleSprite.texture);
            UnityEngine.Object.Destroy(_circleSprite);
            _circleSprite = null;
        }
    }

    private static string ExtractAnimNameFromUri(string uri)
    {
        if (string.IsNullOrEmpty(uri))
            return null;
        var lastSlash = uri.LastIndexOf('/');
        var fileName = lastSlash >= 0 ? uri.Substring(lastSlash + 1) : uri;
        var dotIdx = fileName.LastIndexOf('.');
        return dotIdx > 0 ? fileName.Substring(0, dotIdx) : fileName;
    }

    // ═══════════════════════════════════════════════════════════════
    //  StructuredData 정의
    // ═══════════════════════════════════════════════════════════════

    public enum PoseOrderMode
    {
        [Label("랜덤")]
        Random,

        [Label("순서대로")]
        Sequential,
    }

    public enum ThumbnailPanelAnchor
    {
        [Label("좌하단")]
        BottomLeft,

        [Label("우하단")]
        BottomRight,

        [Label("좌상단")]
        TopLeft,

        [Label("우상단")]
        TopRight,
    }

    public class PosePreset : StructuredData<SelViewToolAsset>, ICollapsibleStructuredData
    {
        [DataInput]
        [Label("애니메이션")]
        [AutoCompleteResource("CharacterAnimation")]
        [PreviewGallery]
        [Description(
            "캐릭터 Idle 애니메이션과 동일하게 드롭다운·갤러리(썸네일)로 선택할 수 있습니다."
        )]
        public string Animation = "";

        public string GetHeader()
        {
            if (string.IsNullOrEmpty(Animation))
                return "(미선택)";
            var name = Animation;
            var lastSlash = name.LastIndexOf('/');
            if (lastSlash >= 0)
                name = name.Substring(lastSlash + 1);
            var dotIndex = name.LastIndexOf('.');
            if (dotIndex >= 0)
                name = name.Substring(0, dotIndex);
            return name;
        }
    }

    public class ExpressionPreset : StructuredData<SelViewToolAsset>, ICollapsibleStructuredData
    {
        [DataInput]
        [Label("활성화")]
        public bool Enabled = true;

        [DataInput]
        [Label("표정 이름")]
        [Description("캐릭터에 등록된 표정 이름을 입력하세요.")]
        [AutoComplete(nameof(AutoCompleteExpressionName))]
        public string ExpressionName = "";

        public async UniTask<AutoCompleteList> AutoCompleteExpressionName()
        {
            await UniTask.CompletedTask;

            var asset = Parent;
            if (asset?.Character == null)
                return AutoCompleteList.Message("캐릭터를 먼저 선택하세요");

            var expressions = asset.Character.Expressions;
            if (expressions == null || expressions.Length == 0)
                return AutoCompleteList.Message("캐릭터에 등록된 표정이 없습니다");

            var entries = expressions
                .Where(e => e != null && !string.IsNullOrEmpty(e.Name))
                .Select(e => new AutoCompleteEntry { label = e.Name, value = e.Name })
                .ToList();

            if (entries.Count == 0)
                return AutoCompleteList.Message("캐릭터에 등록된 표정이 없습니다");

            return AutoCompleteList.Single(entries);
        }

        public string GetHeader() => Enabled ? $"✓ {ExpressionName}" : $"✗ {ExpressionName}";
    }

    public class CameraPreset : StructuredData<SelViewToolAsset>, ICollapsibleStructuredData
    {
        [DataInput]
        [Label("이름")]
        public string Name = "카메라";

        [DataInput]
        [Label("위치")]
        public Vector3 Position = Vector3.zero;

        [DataInput]
        [Label("회전")]
        [Description("오일러 각도 (Euler Angles)")]
        public Vector3 Rotation = Vector3.zero;

        [DataInput]
        [Label("FOV")]
        [FloatSlider(1f, 120f)]
        public float FieldOfView = 35f;

        [Trigger]
        [Label("현재 위치 캡처")]
        public void CaptureFromCamera()
        {
            var asset = Parent;
            if (asset?.Camera?.Camera == null)
                return;

            var cam = asset.Camera.Camera;
            Position = cam.transform.position;
            Rotation = cam.transform.eulerAngles;
            FieldOfView = asset.Camera.FieldOfView;

            asset.SetDataInput(nameof(CameraPresets), asset.CameraPresets, broadcast: true);
            asset.UpdateStatus($"✅ 카메라 위치 갱신: {Name}");
        }

        public string GetHeader() => $"📷 {Name}";
    }

    public class VolumePreset : StructuredData<SelViewToolAsset>, ICollapsibleStructuredData
    {
        [DataInput]
        [Label("이름")]
        public string Name = "프리셋";

        [DataInput]
        [Label("밝기")]
        [FloatSlider(0.5f, 2f)]
        public float Brightness = 1.05f;

        [DataInput]
        [Label("대비")]
        [FloatSlider(0.5f, 2f)]
        public float Contrast = 1.02f;

        [DataInput]
        [Label("채도")]
        [FloatSlider(0f, 2f)]
        public float Vibrance = 1f;

        [DataInput]
        [Label("색조")]
        public Color Tint = Color.clear;

        [DataInput]
        [Label("블룸")]
        public bool EnableBloom = false;

        [DataInput]
        [Label("블룸 강도")]
        [FloatSlider(0f, 1f)]
        public float BloomIntensity = 0.1f;

        [DataInput]
        [Label("블룸 임계값")]
        [FloatSlider(0f, 2f)]
        public float BloomThreshold = 0.75f;

        [DataInput]
        [Label("비네팅")]
        public bool EnableVignetting = false;

        [DataInput]
        [Label("비네팅 색상")]
        public Color VignettingColor = new Color(76f / 255f, 76f / 255f, 76f / 255f, 13f / 255f);

        [DataInput]
        [Label("비네팅 페이드")]
        [FloatSlider(0f, 1f)]
        public float VignettingFadeOut = 0f;

        [DataInput]
        [Label("DOF")]
        public bool EnableDepthOfField = false;

        [DataInput]
        [Label("DOF 조리개")]
        [FloatSlider(0.5f, 16f)]
        public float DepthOfFieldAperture = 2.8f;

        [DataInput]
        [Label("DOF 초점 거리")]
        [FloatSlider(0f, 1f)]
        public float DepthOfFieldFocalLength = 0.25f;

        [Trigger]
        [Label("현재 카메라 값 가져오기")]
        public void CaptureFromCamera()
        {
            var asset = Parent;
            if (asset?.Camera == null)
                return;

            var cam = asset.Camera;
            Brightness = cam.Brightness;
            Contrast = cam.Contrast;
            Vibrance = cam.Vibrance;
            Tint = cam.Tint;
            EnableBloom = cam.EnableBloom;
            BloomIntensity = cam.BloomIntensity;
            BloomThreshold = cam.BloomThreshold;
            EnableVignetting = cam.EnableVignetting;
            VignettingColor = cam.VignettingColor;
            VignettingFadeOut = cam.VignettingFadeOut;
            EnableDepthOfField = cam.EnableDepthOfField;
            DepthOfFieldAperture = cam.DepthOfFieldAperture;
            DepthOfFieldFocalLength = cam.DepthOfFieldFocalLength;

            asset.SetDataInput(nameof(VolumePresets), asset.VolumePresets, broadcast: true);
            asset.UpdateStatus($"✅ 볼륨 값 캡처: {Name}");
        }

        public string GetHeader() => $"🎨 {Name}";
    }
}
