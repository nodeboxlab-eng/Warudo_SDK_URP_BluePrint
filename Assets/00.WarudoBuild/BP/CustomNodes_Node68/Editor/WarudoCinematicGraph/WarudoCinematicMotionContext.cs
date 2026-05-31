using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// 프리셋·모션 빌더에 들어가는 입력 (씬 오브젝트는 JSON에 직렬화되지 않고 베이킹에만 사용).
    /// </summary>
    public sealed class WarudoCinematicMotionContext
    {
        public string CameraAssetName = "Camera 1";

        /// <summary>비우면 CameraAssetName 기반으로 결정론적 GUID 생성.</summary>
        public string CameraAssetId = "";

        public string TargetObjectName = "";

        public Transform SceneCamera;

        public Transform SceneTarget;

        public Vector3 FallbackTargetOrigin = Vector3.zero;

        public float TotalDuration = 12f;

        public float Intensity = 1f;

        public float OrbitRadius = 2.5f;

        public float HeightOffset = 1.1f;

        public bool AutoShake;

        public int ShakeSeed = 1337;

        public Vector3 GetTargetWorldPosition()
        {
            if (SceneTarget != null)
                return SceneTarget.position;
            return FallbackTargetOrigin;
        }

        public string ResolveCameraAssetId()
        {
            if (!string.IsNullOrWhiteSpace(CameraAssetId))
                return CameraAssetId.Trim();
            return WarudoCinematicDeterministicGuid.From("camera-asset", CameraAssetName).ToString();
        }
    }
}
