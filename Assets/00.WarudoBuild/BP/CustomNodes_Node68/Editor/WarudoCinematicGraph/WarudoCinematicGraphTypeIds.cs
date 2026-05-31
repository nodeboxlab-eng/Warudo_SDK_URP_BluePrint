namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// Warudo 코어 그래프에서 사용하는 노드 TypeId (내보낸 그래프 import 시 동일해야 함).
    /// </summary>
    internal static class WarudoCinematicGraphTypeIds
    {
        public const string SetAssetTransform = "e78d186d-0a68-4fe3-939a-a92150d8c2ac";
        public const string ToggleCamera = "b0231d1a-a1c9-449a-82d3-7b343567bcc0";
        public const string FindAssetByName = "1903bb16-682c-41fa-b75f-2661216c7f07";
        public const string OnKeystrokePressed = "d341c82e-d6d2-4910-bdb6-2f9719470619";
    }

    internal static class WarudoCinematicGraphPorts
    {
        public const string FlowEnter = "Enter";
        public const string FlowExit = "Exit";
        public const string OnTransitionEnd = "OnTransitionEnd";
        public const string Asset = "Asset";
        public const string Camera = "Camera";
    }
}
