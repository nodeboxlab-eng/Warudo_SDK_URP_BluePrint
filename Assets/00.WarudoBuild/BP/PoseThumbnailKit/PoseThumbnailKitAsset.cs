using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Node68.ToolkitMods.PoseThumbnailKit;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Persistence;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

[AssetType(
    Id = "b2e4f6a8-1c3d-5e7f-9a0b-c2d4e6f80123",
    Title = "Pose Thumbnail Kit",
    Category =
        BpToolkitFlavorEmbedded.ShareBuild ? BpToolkitCategories.Share : BpToolkitCategories.Toolkit
)]
public class PoseThumbnailKitAsset : Asset
{
    // ───────────────────────── 기본 설정 ─────────────────────────

    [DataInput]
    [Label("캐릭터")]
    [Description("포즈를 적용할 캐릭터를 선택하세요.")]
    public CharacterAsset Character;

    [DataInput]
    [Label("카메라")]
    [Description(
        "썸네일 촬영에 사용할 카메라를 선택하세요. 카메라 위치와 화각이 그대로 썸네일에 반영됩니다."
    )]
    public CameraAsset Camera;

    // ───────────────────────── 출력 설정 ─────────────────────────

    [DataInput]
    [Hidden]
    public int ThumbnailSize = 256;

    [DataInput]
    [Label("출력 폴더명")]
    [Description("Images/PoseThumbnails/ 하위에 생성될 폴더명입니다.")]
    public string OutputSubfolder = "MyPoses";

    [DataInput]
    [Label("기존 썸네일 덮어쓰기")]
    [Description("이미 썸네일이 있는 애니메이션도 다시 촬영합니다.")]
    public bool OverwriteExisting = false;

    // ───────────────────────── 촬영 설정 ─────────────────────────

    [DataInput]
    [Label("촬영 대기 시간 (초)")]
    [FloatSlider(0.3f, 5f)]
    [Description(
        "포즈 적용 후 촬영까지 대기 시간입니다. 블렌딩이 완료될 만큼 충분히 설정하세요. (1 이하는 권장하지 않습니다)"
    )]
    public float CaptureDelay = 1.0f;

    [DataInput]
    [Label("배경 투명")]
    [Description("썸네일 배경을 투명(알파)으로 처리합니다.")]
    public bool TransparentBackground = true;

    [DataInput]
    [Label("촬영 중 트래킹 끄기")]
    [Description("촬영 동안 페이셜 트래킹을 일시 비활성화하여 자연스러운 포즈를 캡처합니다.")]
    public bool DisableTrackingDuringCapture = true;

    // ───────────────────────── 상태 표시 ─────────────────────────

    [Markdown]
    public string _statusDisplay = "*캐릭터와 카메라를 선택한 후 스캔 또는 실행 버튼을 누르세요.*";

    // ───────────────────────── 액션 ─────────────────────────

    [Trigger]
    [Label("🔍 애니메이션 스캔")]
    [Description("CharacterAnimations 폴더에서 .anim 파일을 검색하고 썸네일 유무를 확인합니다.")]
    public void ScanAnimations()
    {
        var pdm = Context.PersistentDataManager;

        FileEntry[] animFiles;
        try
        {
            animFiles = GetAnimationFiles(pdm);
        }
        catch
        {
            UpdateStatus("⚠️ `CharacterAnimations` 폴더를 찾을 수 없습니다.");
            return;
        }

        if (animFiles.Length == 0)
        {
            UpdateStatus("⚠️ `.anim` 파일이 없습니다.");
            return;
        }

        var folder = GetOutputSubfolder();
        int withThumb = 0;
        int withoutThumb = 0;
        var missingNames = new List<string>();

        foreach (var file in animFiles)
        {
            var animName = StripExtension(file.fileName);
            var thumbRelPath =
                PoseThumbnailResolver.ThumbnailBasePath + "/" + folder + "/" + animName + ".png";
            if (pdm.HasFile(thumbRelPath))
                withThumb++;
            else
            {
                withoutThumb++;
                if (missingNames.Count < 20)
                    missingNames.Add(animName);
            }
        }

        var outputFullPath = pdm.GetFullPath(
            PoseThumbnailResolver.ThumbnailBasePath + "/" + folder
        );
        var lines = new List<string>
        {
            "### 📋 스캔 결과",
            "",
            "| 항목 | 수량 |",
            "|---|---|",
            "| 애니메이션 파일 | **" + animFiles.Length + "**개 |",
            "| 썸네일 있음 | **" + withThumb + "**개 |",
            "| 썸네일 없음 | **" + withoutThumb + "**개 |",
            "",
            "출력 경로: `" + outputFullPath + "`",
        };

        if (missingNames.Count > 0)
        {
            lines.Add("");
            lines.Add("**썸네일 없는 애니메이션:**");
            foreach (var name in missingNames)
                lines.Add("- `" + name + "`");
            if (withoutThumb > 20)
                lines.Add("- ... 외 **" + (withoutThumb - 20) + "**개");
        }

        UpdateStatus(string.Join("\n", lines));
    }

    [Trigger]
    [Label("▶ 썸네일 생성 실행")]
    [Description("커스텀 애니메이션 썸네일을 일괄 생성합니다.")]
    public void GenerateThumbnails()
    {
        if (Character == null)
        {
            UpdateStatus("⚠️ **캐릭터**를 먼저 선택하세요.");
            return;
        }
        if (Camera == null)
        {
            UpdateStatus("⚠️ **카메라**를 먼저 선택하세요.");
            return;
        }
        if (_isGenerating)
        {
            UpdateStatus("⚠️ 이미 생성 중입니다. 중지하려면 ⏹ 버튼을 누르세요.");
            return;
        }

        DoGenerateThumbnails().Forget();
    }

    [Trigger]
    [Label("📂 저장 폴더 열기")]
    [Description("썸네일이 저장된 폴더를 파일 탐색기에서 엽니다.")]
    public void OpenOutputFolder()
    {
        var pdm = Context.PersistentDataManager;
        var folder = GetOutputSubfolder();
        var fullPath = pdm.GetFullPath(PoseThumbnailResolver.ThumbnailBasePath + "/" + folder);
        Application.OpenURL("file:///" + fullPath.Replace('\\', '/'));
    }

    [Trigger]
    [Label("⏹ 중지")]
    [Description("진행 중인 썸네일 생성을 중지합니다.")]
    public void StopGeneration()
    {
        if (_isGenerating)
        {
            _isGenerating = false;
            UpdateStatus("⏹ 생성이 중지되었습니다.");
        }
    }

    // ───────────────────────── 내부 상태 ─────────────────────────

    private bool _isGenerating;

    // ───────────────────────── 라이프사이클 ─────────────────────────

    protected override void OnCreate()
    {
        base.OnCreate();
        SetActive(true);
    }

    // ───────────────────────── 유틸리티 ─────────────────────────

    private FileEntry[] GetAnimationFiles(PersistentDataManager pdm)
    {
        return pdm.GetFileEntries("CharacterAnimations", "*.anim")
            .Where(f =>
                !f.fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)
                && !f.fileName.EndsWith(".bundle", StringComparison.OrdinalIgnoreCase)
            )
            .OrderBy(f => f.fileName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private string GetOutputSubfolder()
    {
        var folder = OutputSubfolder;
        if (string.IsNullOrEmpty(folder) || folder.Trim().Length == 0)
            return "MyPoses";
        return folder.Trim();
    }

    private static string StripExtension(string fileName)
    {
        var dotIndex = fileName.LastIndexOf('.');
        return dotIndex > 0 ? fileName.Substring(0, dotIndex) : fileName;
    }

    private void UpdateStatus(string message)
    {
        _statusDisplay = message;
        BroadcastDataInput(nameof(_statusDisplay));
    }

    // ═══════════════════════════════════════════════════════════════
    //  썸네일 생성 코어
    // ═══════════════════════════════════════════════════════════════

    private async UniTaskVoid DoGenerateThumbnails()
    {
        _isGenerating = true;
        try
        {
            var pdm = Context.PersistentDataManager;

            FileEntry[] animFiles;
            try
            {
                animFiles = GetAnimationFiles(pdm);
            }
            catch
            {
                UpdateStatus("⚠️ `CharacterAnimations` 폴더를 찾을 수 없습니다.");
                return;
            }

            if (animFiles.Length == 0)
            {
                UpdateStatus("⚠️ 애니메이션 파일(.anim)이 없습니다.");
                return;
            }

            var cam = Camera.Camera;
            if (cam == null)
            {
                UpdateStatus("⚠️ 카메라 컴포넌트를 찾을 수 없습니다.");
                return;
            }

            var folder = GetOutputSubfolder();
            var thumbBasePath = PoseThumbnailResolver.ThumbnailBasePath + "/" + folder + "/";

            var originalIdle = Character.DefaultIdleAnimation;
            var originalTracking = Character.TrackingEnabled;

            if (DisableTrackingDuringCapture && originalTracking)
                Character.SetDataInput("TrackingEnabled", false, broadcast: true);

            var size = 256;
            var renderTex = new RenderTexture(size, size, 24, RenderTextureFormat.ARGB32);
            renderTex.antiAliasing = 2;

            int generated = 0;
            int skipped = 0;
            int failed = 0;
            int total = animFiles.Length;

            UpdateStatus("🚀 생성 시작... (0/" + total + ")");

            foreach (var animFile in animFiles)
            {
                if (!_isGenerating)
                    break;

                var animName = StripExtension(animFile.fileName);
                var thumbRelPath = thumbBasePath + animName + ".png";

                if (!OverwriteExisting && pdm.HasFile(thumbRelPath))
                {
                    skipped++;
                    continue;
                }

                try
                {
                    var uri = "character-animation://data/CharacterAnimations/" + animFile.fileName;
                    Character.SetDataInput("DefaultIdleAnimation", uri, broadcast: true);

                    await UniTask.Delay(TimeSpan.FromSeconds(CaptureDelay));
                    if (!_isGenerating)
                        break;

                    var pngBytes = CaptureThumbnail(cam, renderTex, size);
                    if (pngBytes != null && pngBytes.Length > 0)
                    {
                        pdm.WriteFileBytes(thumbRelPath, pngBytes);
                        generated++;
                    }
                    else
                    {
                        failed++;
                    }

                    int progress = generated + skipped + failed;
                    UpdateStatus(
                        "📸 생성 중... ("
                            + progress
                            + "/"
                            + total
                            + ")\n\n"
                            + "| 상태 | 수량 |\n|---|---|\n"
                            + "| ✅ 생성 | "
                            + generated
                            + " |\n"
                            + "| ⏭ 건너뜀 | "
                            + skipped
                            + " |\n"
                            + "| ❌ 실패 | "
                            + failed
                            + " |\n\n"
                            + "현재: **"
                            + animName
                            + "**"
                    );
                }
                catch (Exception e)
                {
                    failed++;
                    Debug.LogWarning(
                        "[Pose Thumbnail Kit] " + animName + " 캡처 실패: " + e.Message
                    );
                }
            }

            Character.SetDataInput(
                "DefaultIdleAnimation",
                string.IsNullOrEmpty(originalIdle)
                    ? CharacterAsset.DefaultIdleAnimationUri
                    : originalIdle,
                broadcast: true
            );

            if (DisableTrackingDuringCapture && originalTracking)
                Character.SetDataInput("TrackingEnabled", true, broadcast: true);

            RenderTexture.active = null;
            UnityEngine.Object.Destroy(renderTex);

            var resultLines = new List<string>
            {
                _isGenerating ? "### ✅ 생성 완료!" : "### ⏹ 생성 중지됨",
                "",
                "| 항목 | 수량 |",
                "|---|---|",
                "| 생성 완료 | **" + generated + "**개 |",
                "| 건너뜀 (이미 존재) | **" + skipped + "**개 |",
            };
            if (failed > 0)
                resultLines.Add("| 실패 | **" + failed + "**개 |");
            resultLines.Add("");
            resultLines.Add("출력 폴더: `" + pdm.GetFullPath(thumbBasePath) + "`");
            resultLines.Add("");
            resultLines.Add("*Warudo를 재시작하면 Preview Gallery에 썸네일이 표시됩니다.*");

            UpdateStatus(string.Join("\n", resultLines));
        }
        catch (Exception e)
        {
            Debug.LogError("[Pose Thumbnail Kit] Error: " + e);
            UpdateStatus("⚠️ 오류 발생: " + e.Message);
        }
        finally
        {
            _isGenerating = false;
        }
    }

    private byte[] CaptureThumbnail(UnityEngine.Camera cam, RenderTexture renderTex, int size)
    {
        var prevTarget = cam.targetTexture;
        var prevClearFlags = cam.clearFlags;
        var prevBgColor = cam.backgroundColor;

        try
        {
            if (TransparentBackground)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.clear;
            }

            cam.targetTexture = renderTex;
            cam.Render();

            RenderTexture.active = renderTex;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            var pngBytes = tex.EncodeToPNG();
            UnityEngine.Object.Destroy(tex);

            return pngBytes;
        }
        finally
        {
            cam.targetTexture = prevTarget;
            cam.clearFlags = prevClearFlags;
            cam.backgroundColor = prevBgColor;
        }
    }
}
