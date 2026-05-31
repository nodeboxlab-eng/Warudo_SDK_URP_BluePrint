using UnityEngine;
using UnityEditor;

namespace MaskCreationTool.Editor
{
    public class MaskCreationToolWindow : EditorWindow
    {
        private const float MIN_WIDTH = 850f;
        private const float MIN_HEIGHT = 600f;
        private const float LEFT_SIDEBAR_WIDTH = 70f;
        private const float BOTTOM_BAR_HEIGHT = 70f;
        private const float BUTTON_SIZE = 60f;
        private const int DEFAULT_TEXTURE_SIZE = 1024;

        private GameObject _targetGameObject;
        private Renderer _targetRenderer;
        private Mesh _targetMesh;

        private MaskCanvasModel _canvasModel;
        private MaskCanvasView _canvasView;
        private MaskDrawController _drawController;
        private MaskUndoRedoManager _undoRedoManager;
        private IslandSelector _islandSelector;

        private int _expansionMargin = 0;
        private MaskDisplayColor _maskDisplayColor = MaskDisplayColor.Gray;

        // ユーザーが選択するマテリアルインデックス（MeshDeleter互換の機能）
        private int _selectedMaterialIndex = 0;

        // Scene 드래그로 삼각형 선택 기능
        private int _sceneHoveredTriangleIndex = -1;
        private Mesh _sceneBakedMeshCache;
        private int _sceneLastFrameCount = -1;
        private bool _sceneIsDrawingByDrag = false;

        // 正方形ボタン用カスタムスタイル（EditorStyles.miniButtonはfixedHeightが固定されているため）
        private GUIStyle _squareButtonStyle;
        private GUIStyle _squareToggleStyle;
        private GUIStyle _dropAreaStyle;

        [MenuItem("Tools/Mask Creation Tool")]
        public static void ShowWindow()
        {
            var window = GetWindow<MaskCreationToolWindow>("マスク作成支援ツール");
            window.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
            window.Show();
        }

        private void OnEnable()
        {
            _canvasModel = new MaskCanvasModel();
            _canvasModel.Initialize(DEFAULT_TEXTURE_SIZE, DEFAULT_TEXTURE_SIZE);

            _drawController = new MaskDrawController();
            _drawController.Initialize();

            _undoRedoManager = new MaskUndoRedoManager();
            _undoRedoManager.Initialize(DEFAULT_TEXTURE_SIZE, DEFAULT_TEXTURE_SIZE);

            _canvasView = new MaskCanvasView();
            _canvasView.SetModel(_canvasModel);
            _canvasView.SetDrawController(_drawController);
            _canvasView.OnDrawingComplete = () => RecordUndoState();
            _canvasView.OnIslandClick = (texCoord) => OnIslandSelected(texCoord);

            // 初期状態を記録
            RecordUndoState();

            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;

            // BakeMesh 캐시 정리
            if (_sceneBakedMeshCache != null)
            {
                Object.DestroyImmediate(_sceneBakedMeshCache);
                _sceneBakedMeshCache = null;
            }

            // Scene上のプレビューをクリア（MeshDeleterWithTexture方式）
            if (_canvasView != null)
            {
                _canvasView.ClearScenePreview();
            }

            if (_canvasModel != null)
            {
                _canvasModel.Dispose();
                _canvasModel = null;
            }
        }

        /// <summary>
        /// 毎フレーム再描画（MeshDeleterWithTexture方式）
        /// ComputeShaderの描画結果を即座にキャンバスとSceneに反映する
        /// </summary>
        private void Update()
        {
            Repaint();
            // Sceneビューも再描画してリアルタイムプレビューを実現
            // EditorWindow.Repaint()はこのウィンドウのみ。Sceneは別ウィンドウなので明示的に指示が必要
            SceneView.RepaintAll();
        }

        private void OnGUI()
        {
            // カスタムスタイルの遅延初期化（OnGUI内でないとEditorStylesが使えない）
            if (_squareButtonStyle == null)
            {
                _squareButtonStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    fixedHeight = 0,
                    alignment = TextAnchor.MiddleCenter
                };
            }
            if (_squareToggleStyle == null)
            {
                _squareToggleStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    fixedHeight = 0,
                    alignment = TextAnchor.MiddleCenter
                };
            }
            if (_dropAreaStyle == null)
            {
                _dropAreaStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 9,
                    wordWrap = true
                };
            }

            DrawToolbar();

            EditorGUILayout.BeginHorizontal();
            {
                DrawLeftSidebar();

                EditorGUILayout.BeginVertical();
                {
                    DrawCanvasArea();
                    DrawBottomBar();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                EditorGUILayout.LabelField("対象:", GUILayout.Width(40));

                var newGameObject = (GameObject)EditorGUILayout.ObjectField(
                    _targetGameObject,
                    typeof(GameObject),
                    true,
                    GUILayout.Width(200)
                );

                if (newGameObject != _targetGameObject)
                {
                    _targetGameObject = newGameObject;
                    OnTargetGameObjectChanged();
                }

                // 選択マテリアルの UI（対象がある場合）
                if (_targetRenderer != null && _targetRenderer.sharedMaterials != null && _targetRenderer.sharedMaterials.Length > 0)
                {
                    string[] names = new string[_targetRenderer.sharedMaterials.Length];
                    for (int i = 0; i < names.Length; i++)
                    {
                        var mat = _targetRenderer.sharedMaterials[i];
                        names[i] = mat != null ? $"[{i}] {mat.name}" : $"[{i}] (null)";
                    }

                    var newIndex = EditorGUILayout.Popup(_selectedMaterialIndex, names, GUILayout.Width(250));
                    if (newIndex != _selectedMaterialIndex)
                    {
                        // 作業状態をリセット（旧マテリアルの復元含む）
                        ResetWorkingState();
                        _selectedMaterialIndex = newIndex;
                        // 新しいマテリアルから背景を読込む
                        LoadBackgroundTexture(_selectedMaterialIndex);
                        RecordUndoState();
                    }
                } else {
                    _selectedMaterialIndex = 0; // リセット
                }

                // UVラインの色
                if (_canvasView != null)
                {
                    EditorGUILayout.LabelField("UVの色", GUILayout.Width(45));
                    _canvasView.UVLineColor = EditorGUILayout.ColorField(
                        GUIContent.none, _canvasView.UVLineColor,
                        false, false, false,
                        GUILayout.Width(40)
                    );
                }

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftSidebar()
        {
            var prevColor = GUI.color;
            GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, 0.7f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(LEFT_SIDEBAR_WIDTH));
            {
                if (_drawController != null)
                {
                    if (GUILayout.Toggle(_drawController.CurrentTool == DrawTool.Pen, "ペン", _squareToggleStyle, GUILayout.Height(BUTTON_SIZE)))
                    {
                        _drawController.CurrentTool = DrawTool.Pen;
                    }
                    if (GUILayout.Toggle(_drawController.CurrentTool == DrawTool.Eraser, "消しゴム", _squareToggleStyle, GUILayout.Height(BUTTON_SIZE)))
                    {
                        _drawController.CurrentTool = DrawTool.Eraser;
                    }
                    if (GUILayout.Toggle(_drawController.CurrentTool == DrawTool.Smudge, "ぼかし", _squareToggleStyle, GUILayout.Height(BUTTON_SIZE)))
                    {
                        _drawController.CurrentTool = DrawTool.Smudge;
                    }
                    if (GUILayout.Toggle(_drawController.CurrentTool == DrawTool.Selection, "選択", _squareToggleStyle, GUILayout.Height(BUTTON_SIZE)))
                    {
                        _drawController.CurrentTool = DrawTool.Selection;
                    }
                }

                GUILayout.Space(10);

                if (GUILayout.Button("クリア", _squareButtonStyle, GUILayout.Height(BUTTON_SIZE)))
                {
                    if (_canvasModel != null)
                    {
                        _canvasModel.Clear();
                        _canvasView?.RefreshComposite();
                        RecordUndoState();
                    }
                }

                if (GUILayout.Button("インポート", _squareButtonStyle, GUILayout.Height(BUTTON_SIZE)))
                {
                    ImportMask();
                }

                // ドロップエリア（残りのスペースを全て使う）
                var dropRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                GUI.Box(dropRect, "マスク画像を\nドロップして\nインポート", _dropAreaStyle);
                HandleMaskDrop(dropRect);
            }
            EditorGUILayout.EndVertical();
            GUI.color = prevColor;
        }

        private void DrawCanvasArea()
        {
            var availableRect = GUILayoutUtility.GetRect(
                0, 0,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );

            // キャンバスを正方形に維持し、利用可能な領域の中央に配置
            float size = Mathf.Min(availableRect.width, availableRect.height);
            var canvasRect = new Rect(
                availableRect.x + (availableRect.width - size) / 2f,
                availableRect.y + (availableRect.height - size) / 2f,
                size,
                size
            );

            if (_canvasView != null)
            {
                _canvasView.Draw(canvasRect);
            }

            // キャンバスへのGameObjectドロップ受付
            HandleGameObjectDrop(canvasRect);
        }

        private void DrawBottomBar()
        {
            var prevColor = GUI.color;
            GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, 0.7f);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(BOTTOM_BAR_HEIGHT));
            {
                if (_drawController != null)
                {
                    // スマッジツール時は強度スライダーを表示
                    if (_drawController.CurrentTool == DrawTool.Smudge)
                    {
                        EditorGUILayout.LabelField("強度", GUILayout.Width(28));
                        _drawController.SmudgeStrength = GUILayout.HorizontalSlider(_drawController.SmudgeStrength, 0f, 1f, GUILayout.Width(100));
                        _drawController.SmudgeStrength = EditorGUILayout.FloatField(_drawController.SmudgeStrength, GUILayout.Width(40));
                    }

                    // 濃さ（ペン・選択ツール共通）
                    if (_drawController.CurrentTool == DrawTool.Pen || _drawController.CurrentTool == DrawTool.Selection)
                    {
                        // サムネイル（バー内に収まる20x20）、^はバーの上に描画
                        var thumbAreaRect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20));
                        var thumbRect = thumbAreaRect;

                        // ^マークをサムネイル中央の真上に描画
                        float caretHeight = 14f;
                        float caretWidth = 20f;
                        float caretX = thumbAreaRect.x + (thumbAreaRect.width - caretWidth) * 0.5f;
                        var caretRect = new Rect(caretX, thumbAreaRect.y - caretHeight + 3f, caretWidth, caretHeight);
                        var caretStyle = new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontSize = 18,
                            padding = new RectOffset(0, 0, 0, 0),
                            margin = new RectOffset(0, 0, 0, 0)
                        };
                        GUI.Label(caretRect, "^", caretStyle);

                        // サムネイル描画
                        EditorGUI.DrawRect(thumbRect, Color.black);
                        var innerRect = new Rect(thumbRect.x + 1, thumbRect.y + 1, thumbRect.width - 2, thumbRect.height - 2);
                        float v = _drawController.BrushValue;

                        Color thumbColor;
                        switch (_maskDisplayColor)
                        {
                            case MaskDisplayColor.Red:
                                thumbColor = new Color(1f, 0f, 0f, 1f - v);
                                break;
                            case MaskDisplayColor.Green:
                                thumbColor = new Color(0f, 1f, 0f, 1f - v);
                                break;
                            case MaskDisplayColor.Blue:
                                thumbColor = new Color(0f, 0f, 1f, 1f - v);
                                break;
                            default:
                                thumbColor = new Color(v, v, v, 1f);
                                break;
                        }

                        if (_maskDisplayColor != MaskDisplayColor.Gray)
                        {
                            EditorGUI.DrawRect(innerRect, Color.white);
                            EditorGUI.DrawRect(innerRect, thumbColor);
                        }
                        else
                        {
                            EditorGUI.DrawRect(innerRect, thumbColor);
                        }

                        // クリックで色パレットポップアップ（^の真上、中心揃え）
                        var clickArea = new Rect(thumbRect.x, caretRect.y, thumbRect.width, thumbRect.yMax - caretRect.y);
                        if (Event.current.type == EventType.MouseDown && clickArea.Contains(Event.current.mousePosition))
                        {
                            var popup = new ColorPalettePopup(_maskDisplayColor, (color) =>
                            {
                                _maskDisplayColor = color;
                                SyncDisplayColorToView();
                            });
                            var popupSize = popup.GetWindowSize();
                            // ポップアップの中心X = ^の中心X
                            float anchorX = caretRect.x + caretRect.width * 0.5f - popupSize.x * 0.5f;
                            var anchorAbove = new Rect(anchorX, caretRect.y - popupSize.y, popupSize.x, 0);
                            PopupWindow.Show(anchorAbove, popup);
                            Event.current.Use();
                        }

                        EditorGUILayout.LabelField("濃さ", GUILayout.Width(28));
                        _drawController.BrushValue = GUILayout.HorizontalSlider(_drawController.BrushValue, 0f, 1f, GUILayout.Width(100));
                        _drawController.BrushValue = EditorGUILayout.FloatField(_drawController.BrushValue, GUILayout.Width(40));
                    }

                    GUILayout.Space(8);

                    // ブラシサイズ（ペン・消しゴム）
                    if (_drawController.CurrentTool == DrawTool.Pen || _drawController.CurrentTool == DrawTool.Eraser)
                    {
                        EditorGUILayout.LabelField("サイズ", GUILayout.Width(35));
                        _drawController.BrushSize = (int)GUILayout.HorizontalSlider(_drawController.BrushSize, 1, 300, GUILayout.Width(100));
                        _drawController.BrushSize = EditorGUILayout.IntField(_drawController.BrushSize, GUILayout.Width(40));
                    }

                    // ブラシサイズ（スマッジ）
                    if (_drawController.CurrentTool == DrawTool.Smudge)
                    {
                        EditorGUILayout.LabelField("サイズ", GUILayout.Width(35));
                        _drawController.SmudgeBrushSize = (int)GUILayout.HorizontalSlider(_drawController.SmudgeBrushSize, 1, 300, GUILayout.Width(100));
                        _drawController.SmudgeBrushSize = EditorGUILayout.IntField(_drawController.SmudgeBrushSize, GUILayout.Width(40));
                    }

                    // 塗り広げ（選択ツール時のみ）
                    if (_drawController.CurrentTool == DrawTool.Selection)
                    {
                        EditorGUILayout.LabelField("塗り広げ", GUILayout.Width(52));
                        _expansionMargin = (int)GUILayout.HorizontalSlider(_expansionMargin, -5, 5, GUILayout.Width(100));
                        _expansionMargin = EditorGUILayout.IntField(_expansionMargin, GUILayout.Width(40));
                        _expansionMargin = Mathf.Clamp(_expansionMargin, -5, 5);
                    }
                }

                GUILayout.FlexibleSpace();

                // はみ出し防止トグル（ペン・ぼかしツール時のみ）
                if (_drawController != null && (_drawController.CurrentTool == DrawTool.Pen || _drawController.CurrentTool == DrawTool.Smudge))
                {
                    bool newClamp = GUILayout.Toggle(
                        _drawController.ClampToIsland,
                        "はみ出し\n防止",
                        _squareButtonStyle,
                        GUILayout.Width(BUTTON_SIZE),
                        GUILayout.Height(BUTTON_SIZE)
                    );
                    _drawController.ClampToIsland = newClamp;
                }

                // 選択ツール: 塗る/消すトグル
                if (_drawController != null && _drawController.CurrentTool == DrawTool.Selection)
                {
                    bool isFill = _drawController.SelectionFillMode;
                    // Toggle: 塗るモード時はON（青色）、消すモード時はOFF
                    if (GUILayout.Toggle(
                        isFill,
                        isFill ? "塗る" : "消す",
                        _squareToggleStyle,
                        GUILayout.Width(BUTTON_SIZE),
                        GUILayout.Height(BUTTON_SIZE)) != isFill)
                    {
                        _drawController.SelectionFillMode = !isFill;
                    }
                }

                // 右側: 正方形ボタン
                GUILayout.Space(10);
                EditorGUI.BeginDisabledGroup(_undoRedoManager == null || !_undoRedoManager.CanUndo);
                if (GUILayout.Button("Undo", _squareButtonStyle, GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
                {
                    PerformUndo();
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(_undoRedoManager == null || !_undoRedoManager.CanRedo);
                if (GUILayout.Button("Redo", _squareButtonStyle, GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
                {
                    PerformRedo();
                }
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);
                if (GUILayout.Button("塗りを\n反転", _squareButtonStyle, GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
                {
                    if (_canvasModel != null)
                    {
                        _canvasModel.Invert();
                        _canvasView?.RefreshComposite();
                        RecordUndoState();
                    }
                }

                if (GUILayout.Button("出力", _squareButtonStyle, GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
                {
                    ExportMask();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = prevColor;
        }

        private void HandleMaskDrop(Rect dropRect)
        {
            if (_canvasModel == null)
                return;

            var e = Event.current;
            if (!dropRect.Contains(e.mousePosition))
                return;

            if (e.type == EventType.DragUpdated)
            {
                // ドラッグ中のオブジェクトにTexture2Dが含まれるかチェック
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is Texture2D)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        e.Use();
                        return;
                    }
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is Texture2D)
                    {
                        var path = AssetDatabase.GetAssetPath(obj);
                        if (!string.IsNullOrEmpty(path) && MaskImportExporter.ImportMask(_canvasModel, path))
                        {
                            RecordUndoState();
                            _canvasView?.RefreshComposite();
                            Repaint();
                        }
                        e.Use();
                        return;
                    }
                }
            }
        }

        private void HandleGameObjectDrop(Rect dropRect)
        {
            var e = Event.current;
            if (!dropRect.Contains(e.mousePosition))
                return;

            if (e.type == EventType.DragUpdated)
            {
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is GameObject go && (go.GetComponent<SkinnedMeshRenderer>() != null || go.GetComponent<MeshRenderer>() != null))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        e.Use();
                        return;
                    }
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is GameObject go && (go.GetComponent<SkinnedMeshRenderer>() != null || go.GetComponent<MeshRenderer>() != null))
                    {
                        _targetGameObject = go;
                        OnTargetGameObjectChanged();
                        Repaint();
                        e.Use();
                        return;
                    }
                }
            }
        }

        private void ImportMask()
        {
            if (_canvasModel == null)
                return;

            var path = MaskImportExporter.ShowImportDialog();

            if (!string.IsNullOrEmpty(path))
            {
                if (MaskImportExporter.ImportMask(_canvasModel, path))
                {
                    RecordUndoState();
                    // Compositeを更新してプレビューを即時反映
                    _canvasView?.RefreshComposite();
                    Repaint();
                }
            }
        }

        private void ExportMask()
        {
            if (_canvasModel == null)
                return;

            var defaultName = "mask_output";
            if (_canvasModel.BackgroundTexture != null && !string.IsNullOrEmpty(_canvasModel.BackgroundTexture.name))
            {
                defaultName = _canvasModel.BackgroundTexture.name + "_mask";
            }

            var path = MaskImportExporter.ShowExportDialog(defaultName);

            if (!string.IsNullOrEmpty(path))
            {
                MaskImportExporter.ExportMask(_canvasModel, path);
            }
        }

        private void RecordUndoState()
        {
            if (_undoRedoManager != null && _canvasModel != null)
            {
                var snapshot = _canvasModel.GetSnapshot();
                _undoRedoManager.RecordState(snapshot);
            }
        }

        private void PerformUndo()
        {
            if (_undoRedoManager == null || _canvasModel == null)
                return;

            var state = _undoRedoManager.Undo();
            if (state != null)
            {
                _canvasModel.RestoreSnapshot(state);
                _canvasView?.RefreshComposite();
                Repaint();
            }
        }

        private void PerformRedo()
        {
            if (_undoRedoManager == null || _canvasModel == null)
                return;

            var state = _undoRedoManager.Redo();
            if (state != null)
            {
                _canvasModel.RestoreSnapshot(state);
                _canvasView?.RefreshComposite();
                Repaint();
            }
        }

        /// <summary>
        /// 作業状態をリセット（Renderer/マテリアル切り替え時）
        /// Sceneプレビューのマテリアルを復元し、マスクとUndo履歴をクリアする
        /// </summary>
        private void ResetWorkingState()
        {
            _canvasView?.ClearScenePreview();
            _canvasModel?.Clear();
            _undoRedoManager?.Clear();
        }

        private void OnTargetGameObjectChanged()
        {
            // 既存の作業状態をリセット（旧Rendererのマテリアル復元含む）
            ResetWorkingState();

            if (_targetGameObject == null)
            {
                _targetRenderer = null;
                _targetMesh = null;
                if (_canvasView != null)
                {
                    _canvasView.SetTargetRenderer(null, null, false);
                }
                _islandSelector = null;
                return;
            }

            // SkinnedMeshRenderer を優先、なければ MeshRenderer + MeshFilter を使用
            var skinned = _targetGameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinned != null)
            {
                _targetRenderer = skinned;
                _targetMesh = skinned.sharedMesh;
            }
            else
            {
                var meshRenderer = _targetGameObject.GetComponent<MeshRenderer>();
                var meshFilter = _targetGameObject.GetComponent<MeshFilter>();
                _targetRenderer = meshRenderer;
                _targetMesh = meshFilter != null ? meshFilter.sharedMesh : null;
            }

            // 背景テクスチャを先に読み込みしておく（Canvas.SetTargetRendererで合成が走るため）
            LoadBackgroundTexture();

            if (_canvasView != null)
            {
                _canvasView.SetTargetRenderer(_targetRenderer, _targetMesh, true);
            }

            // アイランドセレクターを初期化
            if (_targetRenderer != null && _targetMesh != null)
            {
                _islandSelector = new IslandSelector();
                _islandSelector.Initialize(_targetMesh, DEFAULT_TEXTURE_SIZE, DEFAULT_TEXTURE_SIZE);

                // はみ出し防止用クリッピングマスクを生成してDrawControllerにセット
                var clipMask = _islandSelector.GetClippingMask();
                _drawController?.SetClipMask(clipMask);
            }

            // 選択マテリアルを初期化（存在する場合）
            _selectedMaterialIndex = 0;
            if (_targetRenderer != null && _targetRenderer.sharedMaterials != null && _targetRenderer.sharedMaterials.Length > 0)
            {
                // 自動で選択マテリアルから読み込む
                LoadBackgroundTexture(_selectedMaterialIndex);
            } else {
                // 背景がない場合はクリア
                _canvasModel.SetBackgroundTexture(null);
                _canvasView?.RefreshComposite();
            }

            // Compositeを更新（UV再生成 + 合成）
            _canvasView?.RefreshComposite();

            // 初期状態を記録
            RecordUndoState();
        }

        private void LoadBackgroundTexture(int materialIndexHint = -1)
        {
            if (_targetRenderer == null || _canvasModel == null)
            {
                return;
            }

            var materials = _targetRenderer.sharedMaterials;
            if (materials == null || materials.Length == 0)
            {
                _canvasModel.SetBackgroundTexture(null);
                _canvasView?.RefreshComposite();
                return;
            }

            Texture found = null;
            string foundDesc = null;

            // 探索する一般的なテクスチャプロパティ名
            string[] propCandidates = new string[] { "_MainTex", "_BaseMap", "_BaseColorMap" };

            // materialIndexHint が有効であればまずそれを試す
            if (materialIndexHint >= 0 && materialIndexHint < materials.Length)
            {
                var mat = materials[materialIndexHint];
                if (mat != null)
                {
                    if (mat.mainTexture != null)
                    {
                        found = mat.mainTexture;
                        foundDesc = $"materials[{materialIndexHint}].mainTexture";
                    }
                    else
                    {
                        foreach (var prop in propCandidates)
                        {
                            if (mat.HasProperty(prop))
                            {
                                var tex = mat.GetTexture(prop);
                                if (tex != null)
                                {
                                    found = tex;
                                    foundDesc = $"materials[{materialIndexHint}].{prop}";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // 見つからない場合は従来の全探索
            if (found == null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    var mat = materials[i];
                    if (mat == null) continue;

                    if (mat.mainTexture != null)
                    {
                        found = mat.mainTexture;
                        foundDesc = $"materials[{i}].mainTexture";
                        break;
                    }

                    foreach (var prop in propCandidates)
                    {
                        if (mat.HasProperty(prop))
                        {
                            var tex = mat.GetTexture(prop);
                            if (tex != null)
                            {
                                found = tex;
                                foundDesc = $"materials[{i}].{prop}";
                                break;
                            }
                        }
                    }

                    if (found != null) break;
                }
            }

            if (found == null)
            {
                _canvasModel.SetBackgroundTexture(null);
                _canvasView?.RefreshComposite();
                return;
            }

            // 読み取り可能な Texture2D に変換
            var readableTexture = MakeTextureReadable(found);
            if (readableTexture == null)
            {
                _canvasModel.SetBackgroundTexture(null);
                _canvasView?.RefreshComposite();
                return;
            }

            // BackgroundTextureをMaskTexture/PreviewTextureと同じサイズにリサイズ
            if (readableTexture.width != _canvasModel.Width || readableTexture.height != _canvasModel.Height)
            {
                var resized = ResizeTexture(readableTexture, _canvasModel.Width, _canvasModel.Height);
                Object.DestroyImmediate(readableTexture);
                readableTexture = resized;
            }

            _canvasModel.SetBackgroundTexture(readableTexture);

            // PreviewTextureを背景テクスチャで初期化（MeshDeleterWithTexture方式）
            _canvasModel.InitializePreviewFromBackground();

            // Scene上のプレビューを設定（_selectedMaterialIndexを先に更新する）
            // RefreshCompositeのGenerateUVMapが正しいサブメッシュのUVラインを生成するために必要
            if (materialIndexHint >= 0)
            {
                _canvasView?.SetupScenePreview(materialIndexHint);
            }

            // PreviewTextureを再合成 + UVマップ再生成
            _canvasView?.RefreshComposite();

            Repaint();
        }

        private Texture2D MakeTextureReadable(Texture source)
        {
            if (source == null) return null;

            if (source is Texture2D t2d)
            {
                Texture2D readable = new Texture2D(t2d.width, t2d.height, t2d.format, false);
                Graphics.CopyTexture(t2d, 0, 0, readable, 0, 0);
                readable.name = t2d.name;
                readable.Apply();
                return readable;
            }
            else if (source is RenderTexture rt)
            {
                var prev = RenderTexture.active;
                RenderTexture.active = rt;
                Texture2D readable = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
                readable.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                readable.Apply();
                RenderTexture.active = prev;
                return readable;
            }
            else
            {
                int width = source.width > 0 ? source.width : 1024;
                int height = source.height > 0 ? source.height : 1024;

                var renderTex = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
                Graphics.Blit(source, renderTex);

                var prev = RenderTexture.active;
                RenderTexture.active = renderTex;
                var readable = new Texture2D(width, height, TextureFormat.RGBA32, false);
                readable.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readable.Apply();
                RenderTexture.active = prev;

                RenderTexture.ReleaseTemporary(renderTex);
                return readable;
            }
        }

        /// <summary>
        /// テクスチャを指定サイズにリサイズする
        /// Graphics.Blitでバイリニア補間されるため品質を保ったままリサイズできる
        /// </summary>
        private Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            var rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(source, rt);

            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            var resized = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
            resized.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resized.name = source.name;
            resized.Apply();
            RenderTexture.active = prev;

            RenderTexture.ReleaseTemporary(rt);
            return resized;
        }

        private void SyncDisplayColorToView()
        {
            if (_canvasView != null)
            {
                _canvasView.DisplayColor = _maskDisplayColor;
                _canvasView.RefreshComposite();
            }
            Repaint();
        }

        public void OnIslandSelected(Vector2Int texCoord)
        {
            if (_islandSelector == null || _canvasModel == null || _drawController == null)
                return;

            var pixels = _islandSelector.GetIslandPixels(texCoord, _expansionMargin);

            if (pixels.Count > 0)
            {
                _drawController.FillPixels(_canvasModel, pixels);
                _canvasView?.RefreshComposite();
                RecordUndoState();
                Repaint();
            }
        }

        // ─── Scene 드래그 기능 ─────────────────────────────────────

        /// <summary>
        /// Scene 뷰에서 메쉬를 드래그하면 해당 삼각형의 UV 영역을 마스크 캔버스에 채운다
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            if (_targetRenderer == null || _targetMesh == null || _canvasModel == null)
                return;

            Event e = Event.current;

            // 마우스 이동/드래그 시 커서 아래 삼각형 감지
            if (e.type == EventType.MouseMove || e.type == EventType.MouseDrag)
            {
                UpdateSceneHoveredTriangle(e);
            }

            // 좌클릭 시작 → Undo 기록 후 칠하기
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (_sceneHoveredTriangleIndex >= 0)
                {
                    _sceneIsDrawingByDrag = true;
                    RecordUndoState();
                    FillTriangleOnMask(_sceneHoveredTriangleIndex);
                    _canvasView?.RefreshComposite();
                    e.Use();
                }
            }
            // 드래그 중 → 계속 칠하기
            else if (e.type == EventType.MouseDrag && e.button == 0 && _sceneIsDrawingByDrag)
            {
                if (_sceneHoveredTriangleIndex >= 0)
                {
                    FillTriangleOnMask(_sceneHoveredTriangleIndex);
                    _canvasView?.RefreshComposite();
                    e.Use();
                }
            }
            // 좌클릭 종료
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                if (_sceneIsDrawingByDrag)
                {
                    _sceneIsDrawingByDrag = false;
                    _canvasModel.SyncGpuToCpu();
                    RecordUndoState();
                }
            }
        }

        /// <summary>
        /// 마우스 커서 아래의 삼각형 인덱스를 갱신
        /// </summary>
        private void UpdateSceneHoveredTriangle(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (_targetRenderer is SkinnedMeshRenderer skinned)
            {
                // SkinnedMeshRenderer: 매 프레임 BakeMesh 캐싱
                bool needsUpdate = _sceneLastFrameCount != Time.frameCount;
                if (needsUpdate || _sceneBakedMeshCache == null)
                {
                    if (_sceneBakedMeshCache != null)
                        Object.DestroyImmediate(_sceneBakedMeshCache);
                    _sceneBakedMeshCache = new Mesh();
                    skinned.BakeMesh(_sceneBakedMeshCache);
                    _sceneLastFrameCount = Time.frameCount;
                }

                Matrix4x4 ltw = skinned.transform.localToWorldMatrix;
                _sceneHoveredTriangleIndex = GetRaycastHitTriangle(ray, _sceneBakedMeshCache, ltw);
            }
            else
            {
                // 일반 MeshRenderer: 임시 MeshCollider로 Raycast
                MeshCollider tempCol = _targetRenderer.GetComponent<MeshCollider>();
                bool created = false;
                if (tempCol == null)
                {
                    tempCol = _targetRenderer.gameObject.AddComponent<MeshCollider>();
                    tempCol.sharedMesh = _targetMesh;
                    created = true;
                }

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Renderer hr = hit.collider.GetComponent<Renderer>();
                    _sceneHoveredTriangleIndex = (hr != null && hr == _targetRenderer) ? hit.triangleIndex : -1;
                }
                else
                {
                    _sceneHoveredTriangleIndex = -1;
                }

                if (created && tempCol != null)
                    Object.DestroyImmediate(tempCol);
            }
        }

        /// <summary>
        /// Ray와 메쉬의 교차 삼각형 인덱스를 반환 (Möller–Trumbore 알고리즘)
        /// </summary>
        private int GetRaycastHitTriangle(Ray ray, Mesh mesh, Matrix4x4 localToWorldMatrix)
        {
            if (mesh == null) return -1;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            float closestDist = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = localToWorldMatrix.MultiplyPoint3x4(vertices[triangles[i]]);
                Vector3 v1 = localToWorldMatrix.MultiplyPoint3x4(vertices[triangles[i + 1]]);
                Vector3 v2 = localToWorldMatrix.MultiplyPoint3x4(vertices[triangles[i + 2]]);

                if (RayTriangleIntersect(ray, v0, v1, v2, out float dist) && dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i / 3;
                }
            }

            return closestIndex;
        }

        private bool RayTriangleIntersect(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out float distance)
        {
            distance = 0f;
            const float EPSILON = 0.0000001f;
            Vector3 e1 = v1 - v0, e2 = v2 - v0;
            Vector3 h = Vector3.Cross(ray.direction, e2);
            float a = Vector3.Dot(e1, h);
            if (a > -EPSILON && a < EPSILON) return false;
            float f = 1f / a;
            Vector3 s = ray.origin - v0;
            float u = f * Vector3.Dot(s, h);
            if (u < 0f || u > 1f) return false;
            Vector3 q = Vector3.Cross(s, e1);
            float v = f * Vector3.Dot(ray.direction, q);
            if (v < 0f || u + v > 1f) return false;
            float t = f * Vector3.Dot(e2, q);
            if (t > EPSILON) { distance = t; return true; }
            return false;
        }

        /// <summary>
        /// 삼각형 인덱스의 UV 영역을 마스크 캔버스에 채운다
        /// </summary>
        private void FillTriangleOnMask(int triangleIndex)
        {
            var triangles = _targetMesh.triangles;
            var uvs = _targetMesh.uv;

            if (triangleIndex * 3 + 2 >= triangles.Length) return;

            int vi0 = triangles[triangleIndex * 3];
            int vi1 = triangles[triangleIndex * 3 + 1];
            int vi2 = triangles[triangleIndex * 3 + 2];

            if (vi0 >= uvs.Length || vi1 >= uvs.Length || vi2 >= uvs.Length) return;

            // UV를 [0,1] 범위로 정규화 후 텍스처 좌표로 변환
            Vector2Int tex0 = UVToTexCoord(uvs[vi0]);
            Vector2Int tex1 = UVToTexCoord(uvs[vi1]);
            Vector2Int tex2 = UVToTexCoord(uvs[vi2]);

            // 현재 도구에 따라 채울 값 결정 (지우개=1.0/흰색, 그 외=BrushValue)
            float fillValue = (_drawController != null && _drawController.CurrentTool == DrawTool.Eraser)
                ? 1f
                : (_drawController?.BrushValue ?? 0f);

            // 삼각형 바운딩 박스 계산
            int minX = Mathf.Max(0, Mathf.Min(tex0.x, Mathf.Min(tex1.x, tex2.x)));
            int maxX = Mathf.Min(_canvasModel.Width - 1, Mathf.Max(tex0.x, Mathf.Max(tex1.x, tex2.x)));
            int minY = Mathf.Max(0, Mathf.Min(tex0.y, Mathf.Min(tex1.y, tex2.y)));
            int maxY = Mathf.Min(_canvasModel.Height - 1, Mathf.Max(tex0.y, Mathf.Max(tex1.y, tex2.y)));

            // 바운딩 박스 내 픽셀 중 삼각형 내부인 것만 채우기
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (IsPointInSceneTriangle(new Vector2(x, y), tex0, tex1, tex2))
                    {
                        _canvasModel.SetValue(x, y, fillValue);
                    }
                }
            }

            _canvasModel.SyncCpuToGpu();
        }

        private Vector2Int UVToTexCoord(Vector2 uv)
        {
            float u = Mathf.Repeat(uv.x, 1f);
            float v = Mathf.Repeat(uv.y, 1f);
            return new Vector2Int(
                (int)(u * _canvasModel.Width),
                (int)(v * _canvasModel.Height)
            );
        }

        /// <summary>
        /// 점이 삼각형 내부에 있는지 확인 (Barycentric 좌표)
        /// </summary>
        private bool IsPointInSceneTriangle(Vector2 p, Vector2 v0, Vector2 v1, Vector2 v2)
        {
            Vector2 v0v1 = v1 - v0, v0v2 = v2 - v0, v0p = p - v0;
            float d00 = Vector2.Dot(v0v2, v0v2);
            float d01 = Vector2.Dot(v0v2, v0v1);
            float d02 = Vector2.Dot(v0v2, v0p);
            float d11 = Vector2.Dot(v0v1, v0v1);
            float d12 = Vector2.Dot(v0v1, v0p);
            float denom = d00 * d11 - d01 * d01;
            if (Mathf.Abs(denom) < 0.0001f) return false;
            float inv = 1f / denom;
            float u = (d11 * d02 - d01 * d12) * inv;
            float v = (d00 * d12 - d01 * d02) * inv;
            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }
    }

    /// <summary>
    /// 色パレットポップアップ（サムネイル縦並び）
    /// </summary>
    public class ColorPalettePopup : PopupWindowContent
    {
        private const float THUMB_SIZE = 24f;
        private const float PADDING = 4f;
        private readonly MaskDisplayColor _current;
        private readonly System.Action<MaskDisplayColor> _onSelect;

        private static readonly (MaskDisplayColor color, Color displayColor)[] _entries =
        {
            (MaskDisplayColor.Gray, Color.black),
            (MaskDisplayColor.Red, Color.red),
            (MaskDisplayColor.Green, Color.green),
            (MaskDisplayColor.Blue, Color.blue),
        };

        public ColorPalettePopup(MaskDisplayColor current, System.Action<MaskDisplayColor> onSelect)
        {
            _current = current;
            _onSelect = onSelect;
        }

        public override Vector2 GetWindowSize()
        {
            float width = THUMB_SIZE + PADDING * 2;
            float height = _entries.Length * (THUMB_SIZE + PADDING) + PADDING;
            return new Vector2(width, height);
        }

        public override void OnGUI(Rect rect)
        {
            float y = PADDING;
            for (int i = 0; i < _entries.Length; i++)
            {
                var entry = _entries[i];
                var thumbRect = new Rect(PADDING, y, THUMB_SIZE, THUMB_SIZE);

                // 選択中は枠を白に
                EditorGUI.DrawRect(thumbRect, entry.color == _current ? Color.white : Color.gray);
                var innerRect = new Rect(thumbRect.x + 2, thumbRect.y + 2, thumbRect.width - 4, thumbRect.height - 4);
                EditorGUI.DrawRect(innerRect, entry.displayColor);

                if (Event.current.type == EventType.MouseDown && thumbRect.Contains(Event.current.mousePosition))
                {
                    _onSelect?.Invoke(entry.color);
                    editorWindow.Close();
                    Event.current.Use();
                }

                y += THUMB_SIZE + PADDING;
            }
        }
    }
}
