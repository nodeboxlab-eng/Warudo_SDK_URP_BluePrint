namespace Node68.CustomNodes
{
    /// <summary>
    /// 쉐어/개발 분기. Warudo 플러그인 런타임에서는 <c>Resources</c>가 동작하지 않는 경우가 많아,
    /// <see cref="Node68FlavorEmbedded.ShareBuild"/>(빌드 메뉴가 생성한 상수)를 사용합니다.
    /// </summary>
    /// <remarks>
    /// 쉐어 빌드 UI: DataInput 필드는 그대로 표시하고
    /// <c>[Description]</c> 툴팁만 <see cref="Node68FlavorEmbedded.ShareBuild"/> 로 비웁니다.
    /// 조건부 <c>[HiddenIf]</c>(모드·토글 등)는 개발·쉐어 공통으로 유지합니다.
    /// </remarks>
    public static class Node68BuildRuntime
    {
        /// <summary>쉐어(배포)로 컴파일된 DLL이면 true.</summary>
        public static bool IsShareBuild() => Node68FlavorEmbedded.ShareBuild;
    }
}
