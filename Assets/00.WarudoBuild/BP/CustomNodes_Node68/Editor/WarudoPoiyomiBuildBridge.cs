using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Warudo Mod Tools(<c>ModToolsMenu</c>)에서 리플렉션으로 호출하는 빌드 전 Poiyomi 가드 진입점.
/// </summary>
public static class WarudoPoiyomiBuildBridge
{
    public static bool TryRunPreBuild()
    {
        return PoiyomiModBuildGuard.RunPreBuild(out List<Material> _);
    }
}
