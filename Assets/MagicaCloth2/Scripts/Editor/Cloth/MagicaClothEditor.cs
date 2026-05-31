// Magica Cloth 2.
// Copyright (c) 2023 MagicaSoft.
// https://magicasoft.jp
using UnityEditor;
using UnityEngine;

namespace MagicaCloth2
{
    /// <summary>
    /// MagicaClothコンポーネントのエディタ拡張
    /// </summary>
    [CustomEditor(typeof(MagicaCloth))]
    [CanEditMultipleObjects]
    public class MagicaClothEditor : MagicaEditorBase
    {

        protected void OnEnable()
        {
            //Debug.Log("MagicaClothEditor.OnEnable");
            ClothEditorManager.OnEditMeshBuildComplete += OnEditMeshBuildComplete;
        }

        protected void OnDisable()
        {
            //Debug.Log("MagicaClothEditor.OnDisable");
            ClothEditorManager.OnEditMeshBuildComplete -= OnEditMeshBuildComplete;
            ClothPainter.ExitPaint();
        }

        //=========================================================================================
        int oldAcitve = -1;

        //=========================================================================================
        /// <summary>
        /// 編集用のセレクションデータを取得する
        /// </summary>
        /// <param name="cloth"></param>
        /// <param name="editMesh"></param>
        /// <returns></returns>
        public SelectionData GetSelectionData(MagicaCloth cloth, VirtualMesh editMesh)
        {
            // すでにセレクションデータが存在し、かつユーザー編集データならばコンバートする
            var selectionData = ClothEditorManager.CreateAutoSelectionData(cloth, cloth.SerializeData, editMesh);
            if (cloth.GetSerializeData2().selectionData != null && cloth.GetSerializeData2().selectionData.userEdit)
            {
                //Debug.Log($"セレクションデータコンバート!");
                selectionData.ConvertFrom(cloth.GetSerializeData2().selectionData);
                selectionData.userEdit = true;
            }

            return selectionData;
        }

        /// <summary>
        /// エディットメッシュの構築完了通知（成否問わず）
        /// </summary>
        void OnEditMeshBuildComplete()
        {
            //Debug.Log($"MagicaClothInspector. OnEditMeshBuildComplete.");
            Repaint();
        }

        /// <summary>
        /// インスペクターGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            var cloth = target as MagicaCloth;

            // 状態
            DispVersion();
            DispStatus();
            DispProxyMesh();

            // 設定
            serializedObject.Update();
            Undo.RecordObject(cloth, "MagicaCloth2");
            EditorGUILayout.Space();
            ClothMainInspector();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            ClothParameterInspector();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GizmoInspector();
            EditorGUILayout.Space();
            ClothPreBuildInspector();
            serializedObject.ApplyModifiedProperties();

            //DrawDefaultInspector();

            // アクティブが変更された場合は編集メッシュを再構築する
            int nowActive = cloth.isActiveAndEnabled ? 1 : 0;
            if (nowActive != oldAcitve)
            {
                oldAcitve = nowActive;

                // ただしコンポーネントがProjectビューで選択されている場合は再構築しない
                // Hierarchyおよびプレハブモードはこれに該当しない
#if UNITY_6000_3_OR_NEWER
                bool inProject = AssetDatabase.Contains(cloth.gameObject.GetEntityId());
#else
                bool inProject = AssetDatabase.Contains(cloth.gameObject.GetInstanceID());
#endif
                if (inProject == false)
                {
                    //Develop.Log($"[{cloth.name}] rebuild. active:{nowActive}, inProject:{inProject}");
                    ClothEditorManager.RegisterComponent(cloth, nowActive > 0 ? GizmoType.Active : 0, true);
                }
            }
        }

        /// <summary>
        /// クロスペイントの適用
        /// </summary>
        /// <param name="selectiondata"></param>
        internal void ApplyClothPainter(SelectionData selectionData)
        {
            if (selectionData == null || selectionData.IsValid() == false)
                return;

            var cloth = target as MagicaCloth;

            // セレクションデータ格納
            ClothEditorManager.ApplySelectionData(cloth, selectionData);
        }

        /// <summary>
        /// クロスペイントの変更による編集メッシュの再構築
        /// </summary>
        internal void UpdateEditMesh()
        {
            var cloth = target as MagicaCloth;

            // 編集用メッシュの再構築
            ClothEditorManager.RegisterComponent(cloth, GizmoType.Active, true); // 強制更新
        }

        //=========================================================================================
        void DispVersion()
        {
            EditorGUILayout.LabelField($"Version {AboutMenu.MagicaClothVersion}");
        }

        void DispStatus()
        {
            var cloth = target as MagicaCloth;

            if (EditorApplication.isPlaying)
            {
                StaticStringBuilder.Clear();
                StaticStringBuilder.AppendLine("[State]");
                StaticStringBuilder.Append(cloth.Process.IsState(ClothProcess.State_UsePreBuild) ? "Pre-Build Construction" : "Runtime Construction");
                DispClothStatus(StaticStringBuilder.ToString(), cloth.Process.Result, true);
            }
            else
            {
                var result = ClothEditorManager.GetResultCode(cloth);
                var preBuildData = cloth.GetSerializeData2().preBuildData;
                if (preBuildData.enabled)
                {
                    // pre-build
                    if (result.IsError() == false)
                        result = preBuildData.DataValidate();
                    DispClothStatus("[Pre-Build Construction]", result, false);
                }
                else
                {
                    // runtime
                    DispClothStatus("[Runtime Construction]", result, true);
                }
            }
        }

        void DispClothStatus(string title, ResultCode result, bool dispWarning)
        {
            StaticStringBuilder.Clear();
            StaticStringBuilder.AppendLine(title);

            // normal / error
            MessageType mtype = MessageType.Info;
            if (result.IsError())
                mtype = MessageType.Error;
            var infoMessage = result.GetResultInformation();
            if (infoMessage != null)
            {
                StaticStringBuilder.AppendLine(result.GetResultString());
                StaticStringBuilder.AppendLine(infoMessage);
            }
            else
            {
                StaticStringBuilder.AppendLine(result.GetResultString());
            }
            EditorGUILayout.HelpBox(StaticStringBuilder.ToString(), mtype);

            // warning
            if (dispWarning && result.IsWarning())
            {
                mtype = MessageType.Warning;
                infoMessage = result.GetWarningInformation();
                if (infoMessage != null)
                    EditorGUILayout.HelpBox($"{result.GetWarningString()}\n{infoMessage}", mtype);
                else
                    EditorGUILayout.HelpBox(result.GetWarningString(), mtype);
            }
        }

        void DispProxyMesh()
        {
            var cloth = target as MagicaCloth;

            VirtualMeshContainer cmesh;
            if (EditorApplication.isPlaying)
            {
                cmesh = cloth.Process?.ProxyMeshContainer;
            }
            else
            {
                cmesh = ClothEditorManager.GetEditMeshContainer(cloth);
            }
            if (cmesh == null || cmesh.shareVirtualMesh == null)
                return;

            var vmesh = cmesh.shareVirtualMesh;

            StaticStringBuilder.Clear();

            // 初期化データ
            StaticStringBuilder.AppendLine("[Init Data]");
            int initVersion = cloth.GetSerializeData2().initData?.initVersion ?? 0;
            if (initVersion > 0)
            {
                StaticStringBuilder.Append($"v{initVersion}: ");
            }
            if (EditorApplication.isPlaying)
            {
                StaticStringBuilder.Append($"{cloth.Process.InitDataResult.GetResultString()}");
            }
            else
            {
                StaticStringBuilder.Append($"{cloth.GetSerializeData2().initData.HasData()}");
            }

            // Proxyメッシュ
            StaticStringBuilder.AppendLine();
            if (EditorApplication.isPlaying)
                StaticStringBuilder.AppendLine("[Proxy Mesh]");
            else
                StaticStringBuilder.AppendLine("[Edit Mesh]");
            if (EditorApplication.isPlaying)
            {
                StaticStringBuilder.AppendLine($"Camera Visible: {!cloth.Process.IsCameraCullingInvisible()}");
                StaticStringBuilder.AppendLine($"Distance Visible: {!cloth.Process.IsDistanceCullingInvisible()}");
            }
            StaticStringBuilder.AppendLine($"Vertex: {vmesh.VertexCount}");
            StaticStringBuilder.AppendLine($"Edge: {vmesh.EdgeCount}");
            StaticStringBuilder.AppendLine($"Triangle: {vmesh.TriangleCount}");
            StaticStringBuilder.AppendLine($"SkinBoneCount: {vmesh.SkinBoneCount}");
            StaticStringBuilder.Append($"TransformCount: {cmesh.GetTransformCount()}");


            EditorGUILayout.HelpBox(StaticStringBuilder.ToString(), MessageType.Info);
        }


        void ClothMainInspector()
        {
            var cloth = target as MagicaCloth;
            var clothType = cloth.SerializeData.clothType;
            bool isBoneSpring = clothType == ClothProcess.ClothType.BoneSpring;

            bool runtime = EditorApplication.isPlaying;

            // 同期状態
            bool sync = EditorApplication.isPlaying && cloth.SyncPartnerCloth != null;

            EditorGUILayout.LabelField("메인 (Main)", EditorStyles.boldLabel);

            // Cloth
            {
                var clothTypeProperty = serializedObject.FindProperty("serializeData.clothType");

                using (new EditorGUI.DisabledScope(runtime))
                {
                    EditorGUILayout.PropertyField(clothTypeProperty, new GUIContent("천 타입 (Cloth Type)"));
                }

                var paintMode = serializedObject.FindProperty("serializeData.paintMode");

                using (new EditorGUI.IndentLevelScope())
                {
                    if (clothType == ClothProcess.ClothType.BoneCloth)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rootBones"), new GUIContent("루트 본 (Root Bones)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.connectionMode"), new GUIContent("연결 모드 (Connection Mode)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rootRotation"), new GUIContent("루트 회전 (Root Rotation)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rotationalInterpolation"), new GUIContent("회전 보간 (Rotational Interpolation)"));
                    }
                    else if (clothType == ClothProcess.ClothType.BoneSpring)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rootBones"), new GUIContent("루트 본 (Root Bones)"));
                        // BoneSpringでは接続モードは指定させない。内部ではLineで固定される。
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("연결 모드 (Connection Mode)");
                            EditorGUILayout.LabelField("[라인 (Line)]");
                        }
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rootRotation"), new GUIContent("루트 회전 (Root Rotation)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.rotationalInterpolation"), new GUIContent("회전 보간 (Rotational Interpolation)"));
                    }
                    else if (clothType == ClothProcess.ClothType.MeshCloth)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.sourceRenderers"), new GUIContent("소스 렌더러 (Source Renderers)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.meshWriteMode"), new GUIContent("메시 쓰기 모드 (Mesh Write Mode)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.reductionSetting"), new GUIContent("감소 설정 (Reduction Setting)"));
                    }

                    EditorGUILayout.Space();
                    if (sync == false)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.updateMode"), new GUIContent("업데이트 모드 (Update Mode)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.disableMode"), new GUIContent("비활성화 모드 (Disable Mode)"));
                    }
                    else
                    {
                        // 同期中は操作不可
                        using (new EditorGUI.DisabledScope(true))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField("업데이트 모드 (Update Mode)");
                                EditorGUILayout.LabelField("(동기화 중)");
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField("비활성화 모드 (Disable Mode)");
                                EditorGUILayout.LabelField("(동기화 중)");
                            }
                        }
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.animationPoseRatio"), new GUIContent("애니메이션 포즈 비율 (Animation Pose Ratio)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.blendWeight"), new GUIContent("블렌드 가중치 (Blend Weight)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.normalAxis"), new GUIContent("법선 축 (Normal Axis)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.normalAlignmentSetting.alignmentMode"), new GUIContent("법선 정렬 (Normal Alignment)"));
                    if (cloth.SerializeData.normalAlignmentSetting.alignmentMode == NormalAlignmentSettings.AlignmentMode.Transform)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.normalAlignmentSetting.adjustmentTransform"), new GUIContent("조정 변환 (Adjustment Transform)"));
                    }

                    if (clothType == ClothProcess.ClothType.MeshCloth)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(paintMode, new GUIContent("페인트 모드 (Paint Mode)"));
                        if (paintMode.enumValueIndex != 0)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.paintMaps"), new GUIContent("페인트 맵 (Paint Maps)"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.paintMapUvChannel"), new GUIContent("페인트 맵 UV 채널 (Paint Map UV Channel)"));
                        }
                    }
                }

                // ペイントボタン
                if (paintMode.enumValueIndex == 0)
                {
                    EditorGUILayout.Space();
                    PaintButton(ClothPainter.PaintMode.Attribute);
                }
                else
                    EditorGUILayout.Space();
            }

            // Custom Skinning
            if (isBoneSpring == false)
            {
                Foldout("Custom Skinning", serializedObject.FindProperty("serializeData.customSkinningSetting.enable"), "커스텀 스키닝 (Custom Skinning)", () =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.customSkinningSetting.skinningBones"), new GUIContent("스키닝 본 (Skinning Bones)"));
                });
            }

            // Culling
            Foldout("Culling", "컬링 (Culling)", () =>
            {
                if (sync == false)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.cameraCullingMode"), new GUIContent("카메라 컬링 모드 (Camera Culling Mode)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.cameraCullingMethod"), new GUIContent("카메라 컬링 방법 (Camera Culling Method)"));
                    if (cloth.SerializeData.cullingSettings.cameraCullingMethod == CullingSettings.CameraCullingMethod.ManualRenderer)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.cameraCullingRenderers"), new GUIContent("카메라 컬링 렌더러 (Camera Culling Renderers)"));
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.distanceCullingLength"), new GUIContent("거리 컬링 길이 (Distance Culling Length)"));
                    using (new EditorGUI.DisabledScope(cloth.SerializeData.cullingSettings.distanceCullingLength.use == false))
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.distanceCullingFadeRatio"), new GUIContent("거리 컬링 페이드 비율 (Distance Culling Fade Ratio)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.cullingSettings.distanceCullingReferenceObject"), new GUIContent("거리 컬링 참조 객체 (Distance Culling Reference Object)"));
                        EditorGUILayout.HelpBox("참조 객체가 [없음]인 경우 메인 카메라를 참조합니다.", MessageType.None);
                    }
                }
                else
                {
                    // 同期中は操作不可
                    using (new EditorGUI.DisabledScope(true))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("카메라 컬링 모드 (Camera Culling Mode)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("카메라 컬링 방법 (Camera Culling Method)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("거리 컬링 길이 (Distance Culling Length)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("거리 컬링 페이드 비율 (Distance Culling Fade Ratio)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("거리 컬링 참조 객체 (Distance Culling Reference Object)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                    }
                }
            });
        }

        void ClothPreBuildInspector()
        {
            var cloth = target as MagicaCloth;

            bool generation = false;

            using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
            {
                Foldout("Pre-Build", serializedObject.FindProperty("serializeData2.preBuildData.enabled"), "사전 빌드 (Pre-Build)", () =>
                {
                    // information
                    var preBuildData = cloth.GetSerializeData2().preBuildData;
                    if (preBuildData.UsePreBuild())
                    {
                        DispClothStatus("[사전 빌드 구성 (Pre-Build Construction)]", preBuildData.DataValidate(), false);
                    }

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData2.preBuildData.buildId"), new GUIContent("빌드 ID (Build ID)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData2.preBuildData.preBuildScriptableObject"), new GUIContent("쓰기 객체 (Write Object)"));
                    using (var horizontalScope = new GUILayout.HorizontalScope())
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("사전 빌드 데이터 생성 (Create PreBuild Data)"))
                        {
                            generation = true;
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                    }
                });
            }

            if (generation)
            {
                // PreBuildデータ構築実行
                Develop.Log($"Start PreBuild data creation.");
                var preBuildResult = PreBuildDataCreation.CreatePreBuildData(cloth);
                Develop.Log($"PreBuild data creation completed. [{cloth.GetSerializeData2().preBuildData.buildId}] : {preBuildResult.GetResultString()}");
            }
        }

        void ClothParameterInspector()
        {
            var cloth = target as MagicaCloth;
            var clothType = cloth.SerializeData.clothType;
            bool isBoneSpring = clothType == ClothProcess.ClothType.BoneSpring;

            // 同期状態
            bool sync = EditorApplication.isPlaying && cloth.SyncPartnerCloth != null;

            ClothPresetUtility.DrawPresetButton(cloth, cloth.SerializeData);

            // Force
            Foldout("Force", "힘 (Force)", () =>
            {
                if (isBoneSpring == false)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.gravity"), new GUIContent("중력 (Gravity)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.gravityDirection"), new GUIContent("중력 방향 (Gravity Direction)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.gravityFalloff"), new GUIContent("중력 감쇠 (Gravity Falloff)"));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.damping"), new GUIContent("감쇠 (Damping)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.stablizationTimeAfterReset"), new GUIContent("안정화 시간 (Stablization Time)"));
            });

            // Spring
            if (isBoneSpring)
            {
                Foldout("Spring", serializedObject.FindProperty("serializeData.springConstraint.useSpring"), "스프링 (Spring)", () =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.springConstraint.springPower"), new GUIContent("스프링 강도 (Spring Power)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.springConstraint.limitDistance"), new GUIContent("제한 거리 (Limit Distance)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.springConstraint.normalLimitRatio"), new GUIContent("법선 제한 비율 (Normal Limit Ratio)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.springConstraint.springNoise"), new GUIContent("스프링 노이즈 (Spring Noise)"));
                });
            }

            // Angle Restoration
            Foldout("Angle Restoration", serializedObject.FindProperty("serializeData.angleRestorationConstraint.useAngleRestoration"), "각도 복원 (Angle Restoration)", () =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.angleRestorationConstraint.stiffness"), new GUIContent("강성 (Stiffness)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.angleRestorationConstraint.velocityAttenuation"), new GUIContent("속도 감쇠 (Velocity Attenuation)"));
                if (isBoneSpring == false)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.angleRestorationConstraint.gravityFalloff"), new GUIContent("중력 감쇠 (Gravity Falloff)"));
                }
            }
            );

            // Angle Limit
            Foldout("Angle Limit", serializedObject.FindProperty("serializeData.angleLimitConstraint.useAngleLimit"), "각도 제한 (Angle Limit)", () =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.angleLimitConstraint.limitAngle"), new GUIContent("제한 각도 (Limit Angle)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.angleLimitConstraint.stiffness"), new GUIContent("강성 (Stiffness)"));
            }
            );

            // Shape
            // BoneSpringではすべて定数なので隠蔽する
            if (isBoneSpring == false)
            {
                Foldout("Shape Restoration", "형태 복원 (Shape Restoration)", () =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.distanceConstraint.stiffness"), new GUIContent("거리 강성 (Distance Stiffness)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.tetherConstraint.distanceCompression"), new GUIContent("테더 압축 (Tether Compression)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.triangleBendingConstraint.stiffness"), new GUIContent("삼각형 굽힘 강성 (Triangle Bending Stiffness)"));
                });
            }

            // Inertia
            Foldout("Inertia", "관성 (Inertia)", () =>
            {
                if (sync == false)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.anchor"), new GUIContent("앵커 (Anchor)"));
                    if (cloth.SerializeData.inertiaConstraint.anchor != null)
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.anchorInertia"), new GUIContent("앵커 관성 (Anchor Inertia)"));
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.worldInertia"), new GUIContent("월드 관성 (World Inertia)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.movementInertiaSmoothing"), new GUIContent("월드 관성 스무딩 (World Inertia Smoothing)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.movementSpeedLimit"), new GUIContent("월드 이동 속도 제한 (World Movement Speed Limit)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.rotationSpeedLimit"), new GUIContent("월드 회전 속도 제한 (World Rotation Speed Limit)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.teleportMode"), new GUIContent("텔레포트 모드 (Teleport Mode)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.teleportDistance"), new GUIContent("텔레포트 거리 (Teleport Distance)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.teleportRotation"), new GUIContent("텔레포트 회전 (Teleport Rotation)"));
                }
                else
                {
                    // 同期中は操作不可
                    using (new EditorGUI.DisabledScope(true))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("앵커 (Anchor)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("앵커 관성 (Anchor Inertia)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        EditorGUILayout.Space();
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("월드 관성 (World Inertia)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("월드 관성 스무딩 (World Inertia Smoothing)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("월드 이동 속도 제한 (World Movement Speed Limit)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("월드 회전 속도 제한 (World Rotation Speed Limit)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("텔레포트 모드 (Teleport Mode)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("텔레포트 거리 (Teleport Distance)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("텔레포트 회전 (Teleport Rotation)");
                            EditorGUILayout.LabelField("(동기화 중)");
                        }
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.localInertia"), new GUIContent("로컬 관성 (Local Inertia)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.localMovementSpeedLimit"), new GUIContent("로컬 이동 속도 제한 (Local Movement Speed Limit)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.localRotationSpeedLimit"), new GUIContent("로컬 회전 속도 제한 (Local Rotation Speed Limit)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.depthInertia"), new GUIContent("로컬 깊이 관성 (Local Depth Inertia)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.centrifualAcceleration"), new GUIContent("원심 가속도 (Centrifugal Acceleration)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.inertiaConstraint.particleSpeedLimit"), new GUIContent("파티클 속도 제한 (Particle Speed Limit)"));
            });

            // Motion
            if (isBoneSpring == false)
            {
                Foldout("Movement Limit", "이동 제한 (Movement Limit)", () =>
                {
                    var useMaxDistance = serializedObject.FindProperty("serializeData.motionConstraint.useMaxDistance");
                    var useBackstop = serializedObject.FindProperty("serializeData.motionConstraint.useBackstop");
                    EditorGUILayout.PropertyField(useMaxDistance, new GUIContent("최대 거리 사용 (Use Max Distance)"));
                    using (new EditorGUI.DisabledScope(!useMaxDistance.boolValue))
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.motionConstraint.maxDistance"), new GUIContent("최대 거리 (Max Distance)"));
                    }
                    EditorGUILayout.PropertyField(useBackstop, new GUIContent("백스톱 사용 (Use Backstop)"));
                    using (new EditorGUI.DisabledScope(!useBackstop.boolValue))
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.motionConstraint.backstopRadius"), new GUIContent("백스톱 반경 (Backstop Radius)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.motionConstraint.backstopDistance"), new GUIContent("백스톱 거리 (Backstop Distance)"));
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.motionConstraint.stiffness"), new GUIContent("강성 (Stiffness)"));

                    var paintMode = serializedObject.FindProperty("serializeData.paintMode");
                    if (paintMode.enumValueIndex == 0)
                        PaintButton(ClothPainter.PaintMode.Motion);
                }
                );
            }

            // Collider Collision
            Foldout("Collider Collision", "콜라이더 충돌 (Collider Collision)", () =>
            {
                if (isBoneSpring)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("모드 (Mode)");
                        EditorGUILayout.LabelField("[포인트 (Point)]");
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.colliderCollisionConstraint.mode"), new GUIContent("모드 (Mode)"));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.radius"), new GUIContent("반경 (Radius)"));
                if (clothType == ClothProcess.ClothType.BoneSpring)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.colliderCollisionConstraint.limitDistance"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.colliderCollisionConstraint.collisionBones"));
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.colliderCollisionConstraint.friction"), new GUIContent("마찰 (Friction)"));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.colliderCollisionConstraint.colliderList"));
            }
            );

            // Self Collision
            if (isBoneSpring == false)
            {
                Foldout("Self Collision", "자기 충돌 (Self Collision) (Beta2)", () =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.selfCollisionConstraint.selfMode"), new GUIContent("자기 모드 (Self Mode)"));
                    var syncMode = serializedObject.FindProperty("serializeData.selfCollisionConstraint.syncMode");
                    EditorGUILayout.PropertyField(syncMode, new GUIContent("동기화 모드 (Sync Mode)"));
                    if (syncMode.enumValueIndex != 0)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.selfCollisionConstraint.syncPartner"));
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.selfCollisionConstraint.surfaceThickness"), new GUIContent("표면 두께 (Surface Thickness)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.selfCollisionConstraint.clothMass"), new GUIContent("천 질량 (Cloth Mass)"));
                }
                );
            }

            // Wind
            Foldout("Wind", "바람 (Wind)", () =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.influence"), new GUIContent("영향력 (Influence)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.frequency"), new GUIContent("주파수 (Frequency)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.turbulence"), new GUIContent("난류 (Turbulence)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.blend"), new GUIContent("노이즈 블렌드 (Noise Blend)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.synchronization"), new GUIContent("동기화 (Synchronization)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.depthWeight"), new GUIContent("깊이 가중치 (Depth Weight)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("serializeData.wind.movingWind"), new GUIContent("이동 바람 (Moving Wind)"));
            });
        }

        /// <summary>
        /// 各プロパティの設定範囲.デフォルトは(0.0 ~ 1.0)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Vector2 GetPropertyMinMax(string propertyName)
        {
            var minmax = new Vector2(0.0f, 1.0f);

            switch (propertyName)
            {
                case "radius":
                    minmax.Set(0.001f, 0.5f);
                    break;
                case "limitAngle":
                    minmax.Set(0.0f, 180.0f);
                    break;
                case "maxDistance":
                    minmax.Set(0.0f, 5.0f);
                    break;
                case "surfaceThickness":
                    minmax.Set(Define.System.SelfCollisionThicknessMin, Define.System.SelfCollisionThicknessMax);
                    break;
                case "movementSpeedLimit":
                case "localMovementSpeedLimit":
                    minmax.Set(0.0f, Define.System.MaxMovementSpeedLimit);
                    break;
                case "rotationSpeedLimit":
                case "localRotationSpeedLimit":
                    minmax.Set(0.0f, Define.System.MaxRotationSpeedLimit);
                    break;
                case "particleSpeedLimit":
                    minmax.Set(0.0f, Define.System.MaxParticleSpeedLimit);
                    break;
                case "distanceCullingLength":
                    minmax.Set(0.0f, Define.System.DistanceCullingMaxLength);
                    break;
            }

            return minmax;
        }

        void GizmoInspector()
        {
#if MC2_DEBUG
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData"));
#else
            FoldOut("Gizmos", "기즈모 (Gizmos)", () =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.always"), new GUIContent("항상 (Always)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.enable"), new GUIContent("활성화 (Enable)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.ztest"), new GUIContent("Z 테스트 (Ztest)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.position"), new GUIContent("위치 (Position)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.axis"), new GUIContent("축 (Axis)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.shape"), new GUIContent("형태 (Shape)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.baseLine"), new GUIContent("기준선 (Base Line)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.depth"), new GUIContent("깊이 (Depth)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.collider"), new GUIContent("콜라이더 (Collider)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.animatedPosition"), new GUIContent("애니메이션 위치 (Animated Position)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.animatedAxis"), new GUIContent("애니메이션 축 (Animated Axis)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.animatedShape"), new GUIContent("애니메이션 형태 (Animated Shape)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.inertiaCenter"), new GUIContent("관성 중심 (Inertia Center)"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.basicPosition"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.basicAxis"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoSerializeData.clothDebugSettings.basicShape"));
            });
#endif
        }

        //=========================================================================================
        /// <summary>
        /// 折りたたみ制御
        /// </summary>
        /// <param name="foldKey">折りたたみ保存キー</param>
        /// <param name="title"></param>
        /// <param name="drawAct">内容描画アクション</param>
        /// <param name="enableAct">有効フラグアクション(null=無効)</param>
        /// <param name="enable">現在の有効フラグ</param>
        public void Foldout(
            string foldKey,
            string title = null,
            System.Action drawAct = null,
            System.Action<bool> enableAct = null,
            bool enable = true
            )
        {
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.label).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);

            GUI.backgroundColor = Color.white;
            GUI.Box(rect, title ?? foldKey, style);

            var e = Event.current;
            bool foldOut = EditorPrefs.GetBool(foldKey);

            if (enableAct == null)
            {
                if (e.type == EventType.Repaint)
                {
                    var arrowRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
                    EditorStyles.foldout.Draw(arrowRect, false, false, foldOut, false);
                }
            }
            else
            {
                // 有効チェック
                var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
                bool sw = GUI.Toggle(toggleRect, enable, string.Empty, new GUIStyle("ShurikenCheckMark"));
                if (sw != enable)
                {
                    enableAct(sw);
                }
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                foldOut = !foldOut;
                EditorPrefs.SetBool(foldKey, foldOut);
                e.Use();
            }

            if (foldOut && drawAct != null)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    using (new EditorGUI.DisabledScope(!enable))
                    {
                        drawAct();
                    }
                }
            }
        }

        /// <summary>
        /// 折りたたみ制御（Boolプロパティによるチェックあり）
        /// </summary>
        /// <param name="foldKey"></param>
        /// <param name="boolProperty"></param>
        /// <param name="title"></param>
        /// <param name="drawAct"></param>
        public void Foldout(
            string foldKey,
            SerializedProperty boolProperty,
            string title = null,
            System.Action drawAct = null
            )
        {
            Foldout(
                foldKey, title, drawAct,
                (sw) => boolProperty.boolValue = sw,
                boolProperty.boolValue
                );
        }

        void FoldOut(string key, string title = null, System.Action drawAct = null)
        {
            bool foldOut1 = EditorPrefs.GetBool(key);
            bool foldOut2 = EditorGUILayout.Foldout(foldOut1, title ?? key);
            if (foldOut2)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    drawAct?.Invoke();
                }
            }
            if (foldOut1 != foldOut2)
            {
                EditorPrefs.SetBool(key, foldOut2);
            }
        }

        void PaintButton(ClothPainter.PaintMode paintMode)
        {
            if (EditorApplication.isPlaying)
                return;

            var cloth = target as MagicaCloth;

            using (new EditorGUILayout.HorizontalScope())
            {
                switch (paintMode)
                {
                    case ClothPainter.PaintMode.Attribute:
                        GUI.backgroundColor = new Color(0.5f, 1.0f, 0.5f);
                        break;
                    case ClothPainter.PaintMode.Motion:
                        GUI.backgroundColor = new Color(0.0f, 1.0f, 1.0f);
                        break;
                }

                EditorGUILayout.Space();

                bool edit = ClothPainter.HasEditCloth(cloth);
                //var icon = edit ? EditorGUIUtility.IconContent("winbtn_win_close") : EditorGUIUtility.IconContent("d_editicon.sml");
                //var icon = EditorGUIUtility.IconContent("d_Grid.PaintTool");// 良い
                var icon = EditorGUIUtility.IconContent("d_editicon.sml");
                if (GUILayout.Button(icon, GUILayout.Width(40)))
                {
                    if (edit == false)
                    {
                        // 最新の編集メッシュからセレクションデータを生成する
                        var editMeshContainer = ClothEditorManager.GetEditMeshContainer(cloth);
                        if (editMeshContainer != null && editMeshContainer.shareVirtualMesh != null)
                        {
                            // すでにセレクションデータが存在し、かつユーザー編集データならばコンバートする
                            var selectionData = GetSelectionData(cloth, editMeshContainer.shareVirtualMesh);

                            // セレクションデータにメッシュの最大接続距離を記録する
                            selectionData.maxConnectionDistance = editMeshContainer.shareVirtualMesh.maxVertexDistance.Value;

                            // ペイント開始
                            ClothPainter.EnterPaint(paintMode, this, cloth, editMeshContainer, selectionData);
                            SceneView.RepaintAll();
                        }
                    }
                    else
                    {
                        // 初期化データの保存確認
                        ClothEditorManager.ApplyInitData(cloth, global: true);

                        ClothPainter.ExitPaint();
                        SceneView.RepaintAll();
                    }
                }
                EditorGUILayout.Space();
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.Space(10);
        }
    }
}
