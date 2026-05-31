using System;
using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Graphs;
using Warudo.Plugins.Core;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    internal static class Node68CameraOrbitHelper
    {
        internal const string ShareDisplayNameSuffix = " Shr";

        internal static CameraAsset TryGetMainCamera()
        {
            try
            {
                var core = Context.PluginManager.GetPlugin<CorePlugin>();
                return core?.MainCameraAsset as CameraAsset;
            }
            catch
            {
                return null;
            }
        }

        internal static CameraAsset ResolveCamera(CameraAsset cameraOverride) =>
            cameraOverride ?? TryGetMainCamera();

        internal static CameraAsset.CameraControlMode GetControlMode(CameraAsset camera)
        {
            if (camera == null)
                return CameraAsset.CameraControlMode.Orbit;

            try
            {
                return camera.ControlMode;
            }
            catch
            {
                return CameraAsset.CameraControlMode.Orbit;
            }
        }

        internal static string FormatControlMode(CameraAsset.CameraControlMode mode) =>
            mode.ToString();

        internal static CharacterAsset ResolveLookAtCharacter(
            CameraAsset camera,
            CharacterAsset characterOverride
        )
        {
            if (characterOverride != null)
                return characterOverride;

            return camera?.FocusCharacter;
        }

        internal static bool GetCharacterLookAtEnabled(CharacterAsset character)
        {
            if (character == null)
                return false;

            try
            {
                return character.LookAtEnabled;
            }
            catch
            {
                return false;
            }
        }

        internal static bool GetCharacterLookAtEnabled(CameraAsset camera)
        {
            return GetCharacterLookAtEnabled(camera?.FocusCharacter);
        }

        internal static string GetCharacterLookAtTargetAssetName(CharacterAsset character)
        {
            if (character == null)
                return string.Empty;

            try
            {
                return character.LookAtTarget?.Name ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        internal static void ApplyCharacterLookAt(
            CharacterAsset character,
            bool lookAtEnabled,
            GameObjectAsset lookAtTarget
        )
        {
            if (character == null)
                return;

            character.SetDataInput(
                nameof(CharacterAsset.LookAtEnabled),
                lookAtEnabled,
                broadcast: true
            );

            if (lookAtEnabled && lookAtTarget != null)
            {
                character.SetDataInput(
                    nameof(CharacterAsset.LookAtTarget),
                    lookAtTarget,
                    broadcast: true
                );
            }
        }

        internal static void ApplyFovInstant(CameraAsset camera, float fov)
        {
            if (camera == null)
                return;

            camera.SetDataInput(nameof(CameraAsset.FieldOfView), fov, broadcast: true);
        }

        internal static void ApplyOrbitInstant(
            CameraAsset camera,
            float orbitX,
            float orbitY,
            Vector3 orbitOffset
        )
        {
            if (camera == null)
                return;

            var orbitRotation = new Vector2(orbitX, orbitY);
            camera.OrbitRotation = orbitRotation;
            camera.OrbitOffset = orbitOffset;
            camera.SetDataInput(nameof(CameraAsset.OrbitRotation), orbitRotation, broadcast: true);
            camera.SetDataInput(nameof(CameraAsset.OrbitOffset), orbitOffset, broadcast: true);
        }

        /// <summary>
        /// 블루프rint MATH_EXPRESSION <c>r - floor(r / 360) * 360 - 180</c> 와 동일.
        /// 캐릭터 Y 회전에 맞춰 Orbit X 를 보정해 정면 구도를 유지합니다.
        /// </summary>
        internal static float ComputeFrontOrbitX(float characterEulerY, float frontOffset = -180f)
        {
            var normalizedY = NormalizeAngle360(characterEulerY);
            return NormalizeOrbitX(normalizedY + frontOffset);
        }

        internal static string FormatSavedFrontOrbitStatus(float frontXOffset)
        {
            return "**저장된 정면 보정** `"
                + frontXOffset.ToString("0.###")
                + "`";
        }

        internal static float ComputeFrontXOffsetFromOrbitX(
            float orbitX,
            float characterEulerY
        )
        {
            var normalizedY = NormalizeAngle360(characterEulerY);
            return NormalizeOrbitX(orbitX) - normalizedY;
        }

        private static float NormalizeAngle360(float degrees)
        {
            return degrees - Mathf.Floor(degrees / 360f) * 360f;
        }

        internal static float NormalizeOrbitX(float degrees)
        {
            return degrees - Mathf.Floor((degrees + 180f) / 360f) * 360f;
        }

        internal static bool TryCaptureFrontOrbitFromCamera(
            CameraAsset camera,
            CharacterAsset character,
            CharacterPivotTransformTarget rotationSource,
            bool useWorldRotationY,
            out float orbitY,
            out Vector3 orbitOffset,
            out float frontXOffset
        )
        {
            orbitY = 0f;
            orbitOffset = Vector3.zero;
            frontXOffset = -180f;

            if (camera == null)
                return false;

            var orbitRotation = camera.OrbitRotation;
            orbitOffset = camera.OrbitOffset;
            orbitY = orbitRotation.y;

            if (
                !TryGetCharacterEulerY(
                    character ?? camera.FocusCharacter,
                    rotationSource,
                    useWorldRotationY,
                    out var characterEulerY
                )
            )
                return false;

            frontXOffset = ComputeFrontXOffsetFromOrbitX(
                orbitRotation.x,
                characterEulerY
            );
            return true;
        }

        internal static bool TryGetCharacterEulerY(
            CharacterAsset character,
            CharacterPivotTransformTarget rotationTarget,
            bool useWorldRotation,
            out float eulerY
        )
        {
            eulerY = 0f;
            if (character == null)
                return false;

            Transform tr = null;
            if (rotationTarget == CharacterPivotTransformTarget.ParentSceneRoot)
            {
                tr = character.ParentTransform;
                if (tr == null)
                    tr = character.MainTransform;
            }
            else
            {
                tr = character.MainTransform;
                if (tr == null)
                    tr = character.ParentTransform;
            }

            if (tr == null && character.GameObject != null)
                tr = character.GameObject.transform;

            if (tr == null)
                return false;

            eulerY = useWorldRotation ? tr.eulerAngles.y : tr.localEulerAngles.y;
            return true;
        }

        internal static Tween CreateFovTransitionSequence(
            CameraAsset camera,
            float targetFov,
            float transitionTime,
            Ease ease
        )
        {
            if (camera == null)
                return null;

            var startFov = camera.FieldOfView;
            var duration = Mathf.Max(0f, transitionTime);

            if (duration <= 0f)
            {
                ApplyFovInstant(camera, targetFov);
                return null;
            }

            var proxy = new FovTweenProxy(startFov);
            return DOTween
                .To(
                    () => proxy.Fov,
                    v =>
                    {
                        proxy.Fov = v;
                        ApplyFovInstant(camera, proxy.Fov);
                    },
                    targetFov,
                    duration
                )
                .SetEase(ease)
                .SetUpdate(UpdateType.Late);
        }

        internal static void ApplyShareSuffixToDisplayName(Node node, string fallbackTitle)
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                node.GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = fallbackTitle;

            if (Node68BuildRuntime.IsShareBuild())
            {
                var core = string.IsNullOrEmpty(node.Name)
                    ? baseName
                    : Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        node.Name,
                        ShareDisplayNameSuffix
                    );
                if (string.IsNullOrEmpty(core))
                    core = baseName;
                node.Name = core + ShareDisplayNameSuffix;
            }
            else if (string.IsNullOrEmpty(node.Name))
            {
                node.Name = baseName;
            }
            else
            {
                var cleaned = Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                    node.Name,
                    ShareDisplayNameSuffix
                );
                node.Name = string.IsNullOrEmpty(cleaned) ? baseName : cleaned;
            }
        }

        private sealed class FovTweenProxy
        {
            public float Fov;

            public FovTweenProxy(float fov) => Fov = fov;
        }
    }
}
