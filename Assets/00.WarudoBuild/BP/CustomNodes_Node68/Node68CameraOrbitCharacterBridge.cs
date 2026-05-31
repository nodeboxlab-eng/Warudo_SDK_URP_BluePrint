using System;
using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Graphs;
using Warudo.Plugins.Core.Assets.Cinematography;
using Warudo.Plugins.Core.Nodes;

namespace Node68.CustomNodes
{
    /// <summary>
    /// Warudo 코어 <see cref="CameraOrbitCharacterNode"/> 와 동일한 Orbit 트윈 경로를 사용합니다.
    /// UMod 보안 정책상 System.Reflection 은 사용하지 않습니다.
    /// </summary>
    internal sealed class Node68CameraOrbitCharacterBridge
    {
        private CameraOrbitCharacterNode _driver;
        private CameraAsset _camera;
        private float _targetOrbitX;
        private float _targetOrbitY;
        private Vector3 _targetOrbitOffset;
        private float _endTime;
        private bool _running;

        public bool IsRunning => _running;

        public void Start(
            Graph graph,
            CameraAsset camera,
            float orbitX,
            float orbitY,
            Vector3 orbitOffset,
            float transitionTime,
            Ease transitionEasing
        )
        {
            Stop();

            if (graph == null)
                throw new InvalidOperationException("Graph is null.");

            if (camera == null)
                throw new InvalidOperationException("Camera is null.");

            _camera = camera;
            _targetOrbitX = orbitX;
            _targetOrbitY = orbitY;
            _targetOrbitOffset = orbitOffset;

            _driver = (CameraOrbitCharacterNode)
                Context.NodeTypeRegistry.CreateEntity(typeof(CameraOrbitCharacterNode));
            _driver.Graph = graph;
            _driver.Camera = camera;
            _driver.X = orbitX;
            _driver.Y = orbitY;
            _driver.Offset = orbitOffset;
            _driver.TransitionTime = Mathf.Max(0f, transitionTime);
            _driver.TransitionEasing = transitionEasing;

            _driver.Enter();

            var duration = Mathf.Max(0f, transitionTime);
            _endTime = duration <= 0f ? Time.time : Time.time + duration;
            _running = true;
        }

        public void Complete()
        {
            var camera = _camera;
            var targetOrbitX = _targetOrbitX;
            var targetOrbitY = _targetOrbitY;
            var targetOrbitOffset = _targetOrbitOffset;

            Stop();
            Node68CameraOrbitHelper.ApplyOrbitInstant(
                camera,
                targetOrbitX,
                targetOrbitY,
                targetOrbitOffset
            );
        }

        public void UpdateTarget(float orbitX, float orbitY, Vector3 orbitOffset)
        {
            _targetOrbitX = orbitX;
            _targetOrbitY = orbitY;
            _targetOrbitOffset = orbitOffset;
        }

        public void Stop()
        {
            _running = false;
            _camera = null;

            if (_driver == null)
                return;

            try
            {
                _driver.Stop();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Node68/Camera] CameraOrbitCharacter Stop: " + ex.Message);
            }

            try
            {
                _driver.Destroy();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Node68/Camera] CameraOrbitCharacter Destroy: " + ex.Message);
            }

            _driver = null;
        }

        public void OnUpdate()
        {
            if (!_running || _driver == null)
                return;

            try
            {
                _driver.OnUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Node68/Camera] CameraOrbitCharacter OnUpdate: " + ex.Message);
            }
        }

        /// <summary>코어 노드 TransitionTime 경과 시 완료 (공식 노드 OnTransitionEnd 타이밍과 동일).</summary>
        public bool IsTransitionComplete()
        {
            if (!_running)
                return true;

            return Time.time >= _endTime;
        }
    }
}
