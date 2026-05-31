using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    public sealed class WarudoCinematicGraphGeneratorWindow : EditorWindow
    {
        private string _graphName = "Generated Cinematic Graph";
        private string _cameraAssetName = "Camera 1";
        private string _cameraAssetIdOverride = "";
        private string _targetObjectNameNote = "";
        private float _duration = 12f;
        private float _intensity = 1f;
        private float _orbitRadius = 2.5f;
        private float _heightOffset = 1.1f;
        private bool _autoShake;
        private int _shakeSeed = 1337;
        private Transform _sceneCamera;
        private Transform _sceneTarget;
        private Vector3 _fallbackTarget = Vector3.zero;
        private int _presetIndex;
        private bool _includeKeystroke = true;
        private string _keystrokeLabel = "1";
        private int _keystrokeEnum = 2;
        private bool _keystrokeCtrl = true;
        private bool _exactFindMatch;
        private Vector2 _scroll;

        [MenuItem("Tools/Node68/Warudo Cinematic Graph Generator")]
        public static void Open()
        {
            var w = GetWindow<WarudoCinematicGraphGeneratorWindow>();
            w.titleContent = new GUIContent("Warudo Cinematic Graph");
            w.minSize = new Vector2(460f, 520f);
        }

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            EditorGUILayout.LabelField("입력", EditorStyles.boldLabel);
            _graphName = EditorGUILayout.TextField("그래프 이름", _graphName);
            _cameraAssetName = EditorGUILayout.TextField("Camera Asset Name", _cameraAssetName);
            _cameraAssetIdOverride = EditorGUILayout.TextField("Camera Asset Id (선택)", _cameraAssetIdOverride);

            EditorGUILayout.Space(6f);
            EditorGUILayout.LabelField(
                "씬 기준 (선택) — 카메라/타깃을 넣으면 LookAt·Push 방향이 정확해집니다.",
                EditorStyles.wordWrappedMiniLabel
            );
            _sceneCamera = (Transform)EditorGUILayout.ObjectField("Scene Camera", _sceneCamera, typeof(Transform), true);
            _sceneTarget = (Transform)EditorGUILayout.ObjectField("Scene Target", _sceneTarget, typeof(Transform), true);
            _fallbackTarget = EditorGUILayout.Vector3Field("Fallback Target Origin", _fallbackTarget);
            _targetObjectNameNote = EditorGUILayout.TextField("Target Name (메모)", _targetObjectNameNote);

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("모션 파라미터", EditorStyles.boldLabel);
            _duration = Mathf.Max(0.05f, EditorGUILayout.FloatField("Duration (초)", _duration));
            _intensity = Mathf.Max(0.05f, EditorGUILayout.FloatField("Intensity", _intensity));
            _orbitRadius = Mathf.Max(0.05f, EditorGUILayout.FloatField("Orbit Radius", _orbitRadius));
            _heightOffset = EditorGUILayout.FloatField("Height Offset", _heightOffset);
            _autoShake = EditorGUILayout.ToggleLeft("Auto Shake (후처리)", _autoShake);

            using (new EditorGUI.DisabledScope(!_autoShake))
                _shakeSeed = EditorGUILayout.IntField("Shake Seed", _shakeSeed);

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("프리셋", EditorStyles.boldLabel);
            var presets = WarudoCameraMotionPresetRegistry.AllPresets;
            var names = new string[presets.Count];
            for (var i = 0; i < presets.Count; i++)
                names[i] = presets[i].DisplayName;
            _presetIndex = EditorGUILayout.Popup("Style Preset", _presetIndex, names);

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Warudo 그래프 옵션", EditorStyles.boldLabel);
            _exactFindMatch = EditorGUILayout.ToggleLeft("FIND_ASSET Exact Match", _exactFindMatch);
            _includeKeystroke = EditorGUILayout.ToggleLeft(
                "키스트로크로 시작 (ON_KEYSTROKE_PRESSED)",
                _includeKeystroke
            );
            if (!_includeKeystroke)
                EditorGUILayout.HelpBox(
                    "키스트로크가 없으면 TOGGLE_CAMERA의 Enter는 비어 있습니다. Warudo 에디터에서 원하는 시작 노드와 수동으로 연결하세요.",
                    MessageType.Warning
                );
            using (new EditorGUI.DisabledScope(!_includeKeystroke))
            {
                _keystrokeLabel = EditorGUILayout.TextField("Keystroke Label", _keystrokeLabel);
                _keystrokeEnum = EditorGUILayout.IntField("Keystroke Enum Value", _keystrokeEnum);
                _keystrokeCtrl = EditorGUILayout.ToggleLeft("Require Ctrl", _keystrokeCtrl);
            }

            EditorGUILayout.Space(10f);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Generate JSON…", GUILayout.Height(30f)))
                    Generate();

                if (GUILayout.Button("Assets 폴더 위치 열기", GUILayout.Height(30f), GUILayout.Width(160f)))
                    EditorUtility.RevealInFinder(Path.Combine(Application.dataPath, ".."));
            }

            EditorGUILayout.EndScrollView();
        }

        private void Generate()
        {
            try
            {
                var preset = presetsSafe();
                var ctx = new WarudoCinematicMotionContext
                {
                    CameraAssetName = _cameraAssetName,
                    CameraAssetId = _cameraAssetIdOverride,
                    TargetObjectName = _targetObjectNameNote,
                    SceneCamera = _sceneCamera,
                    SceneTarget = _sceneTarget,
                    FallbackTargetOrigin = _fallbackTarget,
                    TotalDuration = _duration,
                    Intensity = _intensity,
                    OrbitRadius = _orbitRadius,
                    HeightOffset = _heightOffset,
                    AutoShake = _autoShake,
                    ShakeSeed = _shakeSeed
                };

                var keyframes = WarudoCinematicMotionBuilder.Build(preset, ctx);
                var req = new WarudoCinematicGraphExportRequest
                {
                    GraphName = _graphName,
                    CameraAssetName = _cameraAssetName,
                    CameraAssetId = _cameraAssetIdOverride,
                    FindExactMatch = _exactFindMatch,
                    Keyframes = keyframes,
                    IncludeKeystrokeTrigger = _includeKeystroke,
                    KeystrokeLabel = _keystrokeLabel,
                    KeystrokeEnumValue = _keystrokeEnum,
                    KeystrokeRequireCtrl = _keystrokeCtrl
                };

                var json = WarudoCinematicGraphSerializer.Serialize(req);
                var name = SanitizeFileName(string.IsNullOrWhiteSpace(_graphName) ? "WarudoCinematicGraph" : _graphName);
                var path = EditorUtility.SaveFilePanel("Warudo Graph JSON 내보내기", "", name + ".json", "json");
                if (string.IsNullOrEmpty(path))
                    return;

                WarudoCinematicGraphExporter.ExportToFile(path, json);
                EditorUtility.DisplayDialog("완료", "JSON을 저장했습니다:\n" + path, "확인");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog("생성 실패", ex.Message, "확인");
                Debug.LogException(ex);
            }
        }

        private IWarudoCameraMotionPreset presetsSafe()
        {
            var all = WarudoCameraMotionPresetRegistry.AllPresets;
            if (all.Count == 0)
                throw new InvalidOperationException("등록된 프리셋이 없습니다.");
            _presetIndex = Mathf.Clamp(_presetIndex, 0, all.Count - 1);
            return all[_presetIndex];
        }

        private static string SanitizeFileName(string s)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                s = s.Replace(c, '_');
            return string.IsNullOrWhiteSpace(s) ? "WarudoCinematicGraph" : s.Trim();
        }
    }
}
