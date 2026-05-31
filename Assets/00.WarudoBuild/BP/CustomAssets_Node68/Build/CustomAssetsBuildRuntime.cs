namespace Node68.CustomAssets
{
    /// <remarks>
    /// 쉐어 빌드 UI: DataInput 필드는 그대로 표시하고
    /// <c>[Description]</c> 툴팁만 <see cref="CustomAssetsFlavorEmbedded.ShareBuild"/> 로 비웁니다.
    /// 조건부 <c>[HiddenIf]</c>(모드·토글 등)는 개발·쉐어 공통으로 유지합니다.
    /// </remarks>
    internal static class CustomAssetsBuildRuntime
    {
        internal static bool IsShareBuild() => CustomAssetsFlavorEmbedded.ShareBuild;
    }
}
