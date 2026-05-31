using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;
using Warudo.Core.Resource;

[PluginType(
    Id = "com.pose-thumbnail-kit.warudo",
    Name = "PoseThumbnailKit",
    Description = "포즈 애니메이션용 썸네일을 생성·표시합니다.",
    Version = "1.0.0",
    Author = "Developer",
    SupportUrl = "https://docs.warudo.app",
    AssetTypes = new[] { typeof(PoseThumbnailKitAsset) },
    NodeTypes = new Type[0]
)]
public class PoseThumbnailKitPlugin : Plugin
{
    private PoseThumbnailResolver _thumbnailResolver;

    protected override void OnCreate()
    {
        base.OnCreate();
        _thumbnailResolver = new PoseThumbnailResolver();
        Context.ResourceManager.RegisterUriThumbnailResolver(_thumbnailResolver, this);
        Debug.Log("[Pose Thumbnail Kit] 플러그인 로드 완료");
    }
}

/// <summary>
/// CharacterAnimations 폴더의 커스텀 애니메이션에 대해
/// Thumbnails/Animations/ 아래에서 동명의 PNG를 찾아 반환하는 리졸버.
/// PersistentDataManager를 통해 파일 I/O를 수행하여 UMod 보안 정책을 우회합니다.
/// </summary>
public class PoseThumbnailResolver : IResourceUriThumbnailResolver
{
    private Dictionary<string, string> _pathCache;
    private DateTime _lastCacheTime;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);

    public async Task<byte[]> Resolve(Uri uri)
    {
        if (uri.Scheme != "character-animation")
            return null;

        var authority = uri.Authority;
        var localPath = uri.LocalPath.TrimStart('/');

        if (
            authority != "data"
            || !localPath.StartsWith("CharacterAnimations/", StringComparison.OrdinalIgnoreCase)
        )
            return null;

        var fileName = localPath.Substring(localPath.LastIndexOf('/') + 1);
        var dotIndex = fileName.LastIndexOf('.');
        var animName = dotIndex > 0 ? fileName.Substring(0, dotIndex) : fileName;

        EnsureCache();

        if (_pathCache != null && _pathCache.TryGetValue(animName, out var thumbnailRelPath))
        {
            var pdm = Context.PersistentDataManager;
            if (pdm.HasFile(thumbnailRelPath))
            {
                return await pdm.ReadFileBytesAsync(thumbnailRelPath);
            }
        }

        return null;
    }

    private void EnsureCache()
    {
        if (_pathCache != null && DateTime.UtcNow - _lastCacheTime < CacheDuration)
            return;

        _pathCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var pdm = Context.PersistentDataManager;

        try
        {
            var files = pdm.GetFileEntries(ThumbnailBasePath, "*.png");
            foreach (var file in files)
            {
                var fn = file.fileName;
                var dotIdx = fn.LastIndexOf('.');
                var nameOnly = dotIdx > 0 ? fn.Substring(0, dotIdx) : fn;
                if (!_pathCache.ContainsKey(nameOnly))
                {
                    _pathCache[nameOnly] = file.path;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[Pose Thumbnail Kit] 썸네일 캐시 구축 실패: " + e.Message);
        }

        _lastCacheTime = DateTime.UtcNow;
    }

    public const string ThumbnailBasePath = "Images/PoseThumbnails";

    public void InvalidateCache()
    {
        _pathCache = null;
    }
}
