using UnityEditor;

namespace Node68.CustomNodes.Editor
{
    internal static class DisableDefaultBuildMod
    {
        [MenuItem("Warudo/Build Mod %#b", true, 44)]
        private static bool ValidateBuildMod()
        {
            return false;
        }
    }
}
