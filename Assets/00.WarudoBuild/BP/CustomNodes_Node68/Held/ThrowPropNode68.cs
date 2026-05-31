// Throw Prop Node68 — 보관 중 (dev/share 빌드·팔레트 미등록).
// 다시 켤 때: Player Settings Scripting Define Symbols 에 NODE68_INCLUDE_THROW_PROP 추가 후
// Node68CustomNodesPlugin 의 ThrowProp 등록을 복구하세요.
#if NODE68_INCLUDE_THROW_PROP

using System;
using Cysharp.Threading.Tasks;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.Rendering;
using Warudo.Core;
using Warudo.Core.Utils;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    public enum ThrowPropTargetType68
    {
        [Label("캐릭터")]
        Character = 0,

        [Label("씬 오브젝트")]
        SceneObject = 1,

        [Label("월드 좌표")]
        WorldPosition = 2,
    }

    public enum ThrowPropCharacterFromType68
    {
        [Label("뷰포트 범위 랜덤 위치")]
        ViewportBoundsRandom = 0,

        [Label("캐릭터 머리 위")]
        AboveCharacterHead = 1,

        [Label("씬 오브젝트")]
        SceneObject = 2,

        [Label("트랜스폼 경로")]
        TransformPath = 3,

        [Label("월드 좌표")]
        WorldPosition = 4,

        [Label("카메라 위치")]
        CameraPosition = 5,
    }

    public enum ThrowPropCharacterToType68
    {
        [Label("몸 랜덤 위치")]
        BodyRandomPosition = 0,

        [Label("휴머노이드 본")]
        HumanBodyBone = 1,

        [Label("트랜스폼 경로")]
        TransformPath = 2,
    }

    public enum ThrowPropSceneObjectFromType68
    {
        [Label("뷰포트 범위 랜덤 위치")]
        ViewportBoundsRandom = 0,

        [Label("씬 오브젝트")]
        SceneObject = 1,

        [Label("트랜스폼 경로")]
        TransformPath = 2,

        [Label("월드 좌표")]
        WorldPosition = 3,
    }

    public enum ThrowPropWorldPositionFromType68
    {
        [Label("뷰포트 범위 랜덤 위치")]
        ViewportBoundsRandom = 0,

        [Label("씬 오브젝트")]
        SceneObject = 1,

        [Label("트랜스폼 경로")]
        TransformPath = 2,

        [Label("월드 좌표")]
        WorldPosition = 3,
    }

    /// <summary>
    /// 공식 Warudo <c>THROW_PROP</c> 노드와 동일한 입력·동작을 재현합니다.
    /// Enter 로 프롭 1개를 물리 발사하고, 충돌 시 <see cref="OnCollide"/> 를 호출합니다.
    /// </summary>
    [NodeType(
        Id = "a4d9e2b8-7f6c-4831-9d12-5e0a1b2c3d4f",
        Title = "Throw Prop Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.55f
    )]
    public sealed class ThrowPropNode68 : Node
    {
        public override long GetVersion() => 8;

        [FlowInput]
        public Continuation Enter()
        {
            LaunchOne();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("충돌 시")]
        public Continuation OnCollide;

        [DataOutput]
        [Label("마지막 충돌 위치")]
        public Vector3 LastCollisionPosition() => _lastCollisionPosition;

        [DataInput]
        [Label("타겟 타입")]
        public ThrowPropTargetType68 TargetType = ThrowPropTargetType68.Character;

        [DataInput]
        [Label("캐릭터")]
        [HiddenIf(nameof(HideCharacterField))]
        public CharacterAsset Character;

        [DataInput]
        [Label("프롭 소스")]
        [AutoCompleteResource("Prop")]
        public string PropSource;

        [DataInput]
        [Label("충돌 파티클 소스")]
        [AutoCompleteResource("Particle")]
        public string ImpactParticleSource;

        [DataInput]
        [Label("트레일 소스")]
        [AutoCompleteResource("Particle")]
        public string TrailSource;

        [DataInput]
        [Label("발사 사운드 소스")]
        [AutoCompleteResource("Sound")]
        public string LaunchSoundSource;

        [DataInput]
        [Label("충돌 사운드 소스")]
        [AutoCompleteResource("Sound")]
        public string ImpactSoundSource;

        [DataInput]
        [Label("사운드 볼륨")]
        [FloatSlider(0f, 1f)]
        public float SoundVolume = 0.1f;

        [DataInput]
        [Label("크기")]
        [FloatSlider(0.01f, 10f)]
        public float Scale = 1f;

        [DataInput]
        [Label("질량")]
        [FloatSlider(0.001f, 500f)]
        public float Mass = 25f;

        [DataInput]
        [Label("속도")]
        [FloatSlider(0.1f, 100f)]
        public float Speed = 5f;

        [DataInput]
        [Label("중력")]
        public bool Gravity = true;

        [DataInput]
        [Label("발사 토크")]
        [FloatSlider(-50f, 50f)]
        public float LaunchTorque;

        [DataInput]
        [Label("발사 회전 랜덤")]
        public bool RandomizeLaunchRotation = true;

        [DataInput]
        [Label("생존 시간")]
        [FloatSlider(0.05f, 120f)]
        public float AliveTime = 5f;

        [DataInput]
        [Label("충돌 시 제거")]
        public bool DespawnOnImpact;

        [DataInput]
        [Label("충돌 시 부착")]
        public bool StickOnImpact;

        [DataInput]
        [Label("충돌 파티클 크기")]
        [FloatSlider(0.001f, 5f)]
        public float ImpactParticleScale = 0.25f;

        [DataInput]
        [Label("트레일 강도")]
        [FloatSlider(0f, 5f)]
        public float TrailIntensity = 1f;

        [DataInput]
        [Label("출발")]
        [HiddenIf(nameof(HideCharacterFromTo))]
        public ThrowPropCharacterFromType68 From = ThrowPropCharacterFromType68.ViewportBoundsRandom;

        [DataInput]
        [Label("출발 카메라")]
        [HiddenIf(nameof(HideCharacterFromCamera))]
        public CameraAsset FromCamera;

        [DataInput]
        [Label("출발 각도")]
        [FloatSlider(-180f, 180f)]
        [HiddenIf(nameof(HideCharacterFromAngle))]
        public float FromAngle;

        [DataInput]
        [Label("출발 씬 오브젝트")]
        [HiddenIf(nameof(HideCharacterFromSceneObject))]
        public GameObjectAsset FromSceneObject;

        [DataInput]
        [Label("출발 트랜스폼")]
        [HiddenIf(nameof(HideCharacterFromTransform))]
        public string FromTransform;

        [DataInput]
        [Label("머리 위 거리")]
        [FloatSlider(0.1f, 20f)]
        [HiddenIf(nameof(HideCharacterFromAboveHead))]
        public float FromAboveHeadDistance = 2f;

        [DataInput]
        [Label("출발 월드 좌표")]
        [HiddenIf(nameof(HideCharacterFromWorldPosition))]
        public Vector3 FromWorldPosition;

        [DataInput]
        [Label("도착")]
        [HiddenIf(nameof(HideCharacterFromTo))]
        public ThrowPropCharacterToType68 To = ThrowPropCharacterToType68.BodyRandomPosition;

        [DataInput]
        [Label("도착 본")]
        [HiddenIf(nameof(HideCharacterToBone))]
        public HumanBodyBones ToBone = HumanBodyBones.Head;

        [DataInput]
        [Label("도착 트랜스폼")]
        [HiddenIf(nameof(HideCharacterToTransform))]
        public string ToTransform;

        [DataInput]
        [Label("뷰포트 깊이 (m)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "뷰포트 가장자리 스폰 시 카메라 렌즈로부터의 거리입니다. "
                + "작을수록 카메라 바로 앞(공식 기본 0.1)에서 날아갑니다."
        )]
        [FloatSlider(0.05f, 10f)]
        [HiddenIf(nameof(HideViewportSpawnDistance))]
        public float ViewportSpawnDistance = 0.1f;

        [DataInput]
        [Label("씬 오브젝트 출발")]
        [HiddenIf(nameof(HideSceneObjectFromTo))]
        public ThrowPropSceneObjectFromType68 SceneObjectFrom =
            ThrowPropSceneObjectFromType68.ViewportBoundsRandom;

        [DataInput]
        [Label("씬 오브젝트 출발 카메라")]
        [HiddenIf(nameof(HideSceneObjectFromCamera))]
        public CameraAsset SceneObjectFromCamera;

        [DataInput]
        [Label("씬 오브젝트 출발 각도")]
        [FloatSlider(-180f, 180f)]
        [HiddenIf(nameof(HideSceneObjectFromAngle))]
        public float SceneObjectFromAngle;

        [DataInput]
        [Label("씬 오브젝트 출발 대상")]
        [HiddenIf(nameof(HideSceneObjectFromSceneObject))]
        public GameObjectAsset SceneObjectFromSceneObject;

        [DataInput]
        [Label("씬 오브젝트 출발 트랜스폼")]
        [HiddenIf(nameof(HideSceneObjectFromTransform))]
        public string SceneObjectFromTransform;

        [DataInput]
        [Label("씬 오브젝트 출발 좌표")]
        [HiddenIf(nameof(HideSceneObjectFromWorldPosition))]
        public Vector3 SceneObjectFromWorldPosition;

        [DataInput]
        [Label("씬 오브젝트 도착 대상")]
        [HiddenIf(nameof(HideSceneObjectFromTo))]
        public GameObjectAsset SceneObjectToSceneObject;

        [DataInput]
        [Label("씬 오브젝트 도착 트랜스폼")]
        [HiddenIf(nameof(HideSceneObjectToTransform))]
        public string SceneObjectToTransform;

        [DataInput]
        [Label("월드 좌표 출발")]
        [HiddenIf(nameof(HideWorldPositionFromTo))]
        public ThrowPropWorldPositionFromType68 WorldPositionFrom =
            ThrowPropWorldPositionFromType68.ViewportBoundsRandom;

        [DataInput]
        [Label("월드 좌표 출발 카메라")]
        [HiddenIf(nameof(HideWorldPositionFromCamera))]
        public CameraAsset WorldPositionFromCamera;

        [DataInput]
        [Label("월드 좌표 출발 각도")]
        [FloatSlider(-180f, 180f)]
        [HiddenIf(nameof(HideWorldPositionFromAngle))]
        public float WorldPositionFromAngle;

        [DataInput]
        [Label("월드 좌표 출발 씬 오브젝트")]
        [HiddenIf(nameof(HideWorldPositionFromSceneObject))]
        public GameObjectAsset WorldPositionFromSceneObject;

        [DataInput]
        [Label("월드 좌표 출발 트랜스폼")]
        [HiddenIf(nameof(HideWorldPositionFromTransform))]
        public string WorldPositionFromTransform;

        [DataInput]
        [Label("월드 좌표 출발 위치")]
        [HiddenIf(nameof(HideWorldPositionFromWorldPosition))]
        public Vector3 WorldPositionFromWorldPosition;

        [DataInput]
        [Label("월드 좌표 도착 위치")]
        [HiddenIf(nameof(HideWorldPositionFromTo))]
        public Vector3 WorldPositionToWorldPosition;

        [DataInput]
        [Label("충돌 시 래그돌 활성화")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "켜면 프롭이 캐릭터에 맞았을 때 래그돌을 활성화합니다. "
                + "별도 Activate Character Ragdoll 노드 없이 이 노드에서 처리합니다."
        )]
        [HiddenIf(nameof(HideRagdollSection))]
        public bool ActivateRagdollOnCollide = true;

        [DataInput]
        [Label("래그돌 발사 힘")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "래그돌 활성화 직후 Hips 등에 가하는 임펄스. 공식 튜토리얼 기본 (0, 300, -600)."
        )]
        [HiddenIf(nameof(HideRagdollFields))]
        public Vector3 LaunchForce = new Vector3(0f, 300f, -600f);

        [DataInput]
        [Label("래그돌 자동 리셋")]
        [HiddenIf(nameof(HideRagdollFields))]
        public bool RagdollAutoReset = true;

        [DataInput]
        [Label("래그돌 리셋 대기 시간")]
        [FloatSlider(0.1f, 120f)]
        [HiddenIf(nameof(HideRagdollResetWait))]
        public float RagdollResetWaitTime = 5f;

        private Vector3 _lastCollisionPosition;
        private int _ragdollResetToken;
        private const string ShareDisplayNameSuffix = " Shr";

        private static Transform _throwPropTemplateRoot;
        private static readonly System.Collections.Generic.Dictionary<string, GameObject> ThrowPropTemplates =
            new();

        private static readonly HumanBodyBones[] BodyRandomBones =
        {
            HumanBodyBones.Head,
            HumanBodyBones.Neck,
            HumanBodyBones.Chest,
            HumanBodyBones.UpperChest,
            HumanBodyBones.Spine,
            HumanBodyBones.Hips,
            HumanBodyBones.LeftUpperArm,
            HumanBodyBones.RightUpperArm,
            HumanBodyBones.LeftLowerArm,
            HumanBodyBones.RightLowerArm,
            HumanBodyBones.LeftUpperLeg,
            HumanBodyBones.RightUpperLeg,
            HumanBodyBones.LeftLowerLeg,
            HumanBodyBones.RightLowerLeg,
        };

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyShareSuffixToDisplayName();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyShareSuffixToDisplayName();
        }

        protected override void OnDestroy()
        {
            _ragdollResetToken++;
            base.OnDestroy();
        }

        internal void HandleCollision(Vector3 position)
        {
            _lastCollisionPosition = position;
            TryActivateRagdollOnCollide();
            InvokeFlow(nameof(OnCollide));
        }

        private void TryActivateRagdollOnCollide()
        {
            if (
                !ActivateRagdollOnCollide
                || TargetType != ThrowPropTargetType68.Character
                || Character == null
            )
                return;

            try
            {
                if (Character.GameObject == null || !Character.Enabled)
                    return;

                Character.SetDataInput(nameof(CharacterAsset.RagdollEnabled), true, broadcast: true);

                if (RagdollAutoReset && RagdollResetWaitTime > 0f)
                    Character.DisableTemporaryRagdollTime =
                        Time.time + Mathf.Max(0.01f, RagdollResetWaitTime);

                try
                {
                    Character.DisableAnimatorNextFrame();
                }
                catch
                {
                    // 런타임 구현에 없을 수 있음 — RagdollEnabled 만으로도 동작하는 경우가 많음.
                }

                ApplyRagdollLaunchForce(Character, LaunchForce);

                if (RagdollAutoReset && RagdollResetWaitTime > 0f)
                    ScheduleRagdollReset(Character, RagdollResetWaitTime);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[ThrowProp Node68] Ragdoll 활성화 실패: " + ex.Message);
            }
        }

        private void ScheduleRagdollReset(CharacterAsset character, float waitSeconds)
        {
            var token = ++_ragdollResetToken;
            ResetRagdollDelayed(character, token, waitSeconds).Forget();
        }

        private async UniTaskVoid ResetRagdollDelayed(
            CharacterAsset character,
            int token,
            float waitSeconds
        )
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.01f, waitSeconds)));

            if (
                token != _ragdollResetToken
                || character == null
                || Graph == null
                || !Graph.Enabled
            )
                return;

            ResetRagdoll(character);
        }

        private static void ResetRagdoll(CharacterAsset character)
        {
            if (character == null)
                return;

            try
            {
                character.SetDataInput(nameof(CharacterAsset.RagdollEnabled), false, broadcast: true);
                character.ResetRagdollState();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[ThrowProp Node68] Ragdoll 리셋 실패: " + ex.Message);
            }
        }

        private static void ApplyRagdollLaunchForce(CharacterAsset character, Vector3 force)
        {
            if (character == null || force.sqrMagnitude < 0.001f)
                return;

            var pm = character.PuppetMaster;
            if (pm != null)
            {
                try
                {
                    pm.SwitchToActiveMode();
                }
                catch
                {
                    pm.mode = PuppetMaster.Mode.Active;
                }

                if (pm.muscles != null)
                {
                    for (var i = 0; i < pm.muscles.Length; i++)
                    {
                        var muscle = pm.muscles[i];
                        if (muscle?.rigidbody == null)
                            continue;
                        if (muscle.props.group != Muscle.Group.Hips)
                            continue;

                        muscle.rigidbody.AddForce(force, ForceMode.Impulse);
                        return;
                    }

                    for (var i = 0; i < pm.muscles.Length; i++)
                    {
                        var muscle = pm.muscles[i];
                        if (muscle?.rigidbody == null)
                            continue;

                        muscle.rigidbody.AddForce(force, ForceMode.Impulse);
                        return;
                    }
                }
            }

            if (character.GameObject == null)
                return;

            var fallback = character.GameObject.GetComponentInChildren<Rigidbody>();
            if (fallback != null)
                fallback.AddForce(force, ForceMode.Impulse);
        }

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Throw Prop Node68";

            if (Node68BuildRuntime.IsShareBuild())
            {
                var core = string.IsNullOrEmpty(Name)
                    ? baseName
                    : Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
                if (string.IsNullOrEmpty(core))
                    core = baseName;
                Name = core + ShareDisplayNameSuffix;
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                    Name = baseName;
                else
                {
                    var cleaned = Node68ShareNodeTypeTitles.StripToBaseDisplayName(
                        Name,
                        ShareDisplayNameSuffix
                    );
                    Name = string.IsNullOrEmpty(cleaned) ? baseName : cleaned;
                }
            }
        }

        private void LaunchOne()
        {
            if (string.IsNullOrWhiteSpace(PropSource))
            {
                Debug.LogWarning("[ThrowProp Node68] Prop Source 가 비어 있습니다.");
                return;
            }

            var propUri = PropSource.Trim();

            if (!TryResolveToPosition(out var toPos))
            {
                Debug.LogWarning("[ThrowProp Node68] 도착 위치를 찾을 수 없어 발사를 건너뜁니다.");
                return;
            }

            if (!TryResolveFromPosition(toPos, out var fromPos))
            {
                Debug.LogWarning("[ThrowProp Node68] 시작 위치를 찾을 수 없어 발사를 건너뜁니다.");
                return;
            }

            if (!TrySpawnPropClone(propUri, out var clone))
            {
                Debug.LogWarning("[ThrowProp Node68] Prop Source 를 생성할 수 없습니다.");
                return;
            }

            var launchRotation = RandomizeLaunchRotation
                ? UnityEngine.Random.rotation
                : clone.transform.rotation;

            clone.name = clone.name.Replace(" (Clone)", "") + " (Thrown)";
            clone.transform.SetParent(null, true);
            clone.transform.position = fromPos;
            clone.transform.rotation = launchRotation;
            clone.transform.localScale *= Mathf.Max(0.001f, Scale);

            if (Character?.GameObject != null)
                ApplyLayerRecursively(clone, Character.GameObject.layer);

            RefreshThrownPropLighting(clone);

            EnsureCollider(clone);
            var rb = EnsureRigidbody(clone);
            rb.mass = Mathf.Max(0.001f, Mass);
            rb.useGravity = Gravity;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            var velocity = ComputeLaunchVelocity(fromPos, toPos);
            SetRigidbodyVelocity(rb, velocity);

            if (Mathf.Abs(LaunchTorque) > 0.001f)
            {
                rb.maxAngularVelocity = Mathf.Max(rb.maxAngularVelocity, Mathf.Abs(LaunchTorque));
                rb.AddTorque(UnityEngine.Random.onUnitSphere * LaunchTorque, ForceMode.VelocityChange);
            }

            AttachTrail(clone);

            PlaySound(LaunchSoundSource, fromPos);

            var tracker = clone.AddComponent<Node68ThrowPropTracker>();
            var useTargetGate =
                TargetType == ThrowPropTargetType68.Character && Character != null;
            tracker.Initialize(
                this,
                Mathf.Max(0.05f, AliveTime),
                DespawnOnImpact,
                StickOnImpact,
                ResolveResourcePrefab(ImpactParticleSource),
                Mathf.Max(0.0001f, ImpactParticleScale),
                Mathf.Max(0.05f, AliveTime),
                ResolveAudioClip(ImpactSoundSource),
                Mathf.Clamp01(SoundVolume),
                useTargetGate ? toPos : (Vector3?)null,
                useTargetGate ? Character.GameObject.transform : null,
                useTargetGate ? ResolveTargetHitRadius() : 0.2f,
                fromPos
            );
        }

        private static Transform GetThrowPropTemplateRoot()
        {
            if (_throwPropTemplateRoot != null)
                return _throwPropTemplateRoot;

            var root = new GameObject("Node68_ThrowPropTemplates")
            {
                hideFlags = HideFlags.HideAndDontSave,
            };
            UnityEngine.Object.DontDestroyOnLoad(root);
            _throwPropTemplateRoot = root.transform;
            return _throwPropTemplateRoot;
        }

        /// <summary>
        /// Warudo 씬에 배치된 Prop 에셋 본체는 건드리지 않고, 숨겨진 복제 템플릿만 만듭니다.
        /// ObjectPools 는 Resolve 결과(씬 Prop)를 비활성/이동시켜 Warudo 가 발밑에 다시 띄우는
        /// 부작용이 있습니다.
        /// </summary>
        private static bool EnsureThrowPropTemplate(string propUri, out GameObject template)
        {
            template = null;
            if (string.IsNullOrWhiteSpace(propUri))
                return false;

            if (ThrowPropTemplates.TryGetValue(propUri, out template) && template != null)
                return true;

            var source = ResolveResourcePrefab(propUri);
            if (source == null)
                return false;

            var root = GetThrowPropTemplateRoot();
            template = UnityEngine.Object.Instantiate(source);
            template.SetActive(false);
            template.transform.SetParent(root, false);
            template.transform.localPosition = Vector3.zero;
            template.transform.localRotation = Quaternion.identity;
            template.transform.localScale = source.transform.localScale;
            template.name = source.name + " (Node68 Template)";
            ThrowPropTemplates[propUri] = template;
            return true;
        }

        private static bool TrySpawnPropClone(string propUri, out GameObject clone)
        {
            clone = null;
            if (!EnsureThrowPropTemplate(propUri, out var template))
                return false;

            clone = UnityEngine.Object.Instantiate(template);
            clone.SetActive(true);
            PrepareThrownPropClone(clone);
            return true;
        }

        private static void PrepareThrownPropClone(GameObject clone)
        {
            var oldTracker = clone.GetComponent<Node68ThrowPropTracker>();
            if (oldTracker != null)
                UnityEngine.Object.Destroy(oldTracker);

            clone.transform.SetParent(null, true);

            var rb = clone.GetComponent<Rigidbody>();
            if (rb == null)
                return;

            rb.isKinematic = false;
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = Vector3.zero;
#else
            rb.velocity = Vector3.zero;
#endif
            rb.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// 풀/템플릿에서 복제된 프롭은 캐릭터·풀 루트 등 외부 본을 probeAnchor 로
        /// 들고 있는 경우가 많아, 그 위치에서 라이트 프로브를 샘플링하면 거의 검게 보입니다.
        /// </summary>
        private static void RefreshThrownPropLighting(GameObject clone)
        {
            if (clone == null)
                return;

            var root = clone.transform;
            foreach (var r in clone.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null)
                    continue;

                if (r.probeAnchor == null || !r.probeAnchor.IsChildOf(root))
                    r.probeAnchor = root;

                r.lightProbeUsage = LightProbeUsage.BlendProbes;
                r.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;

                if (r is SkinnedMeshRenderer smr)
                    smr.updateWhenOffscreen = true;
            }
        }

        private static void ApplyLayerRecursively(GameObject go, int layer)
        {
            if (go == null)
                return;

            go.layer = layer;
            var tr = go.transform;
            for (var i = 0; i < tr.childCount; i++)
                ApplyLayerRecursively(tr.GetChild(i).gameObject, layer);
        }

        private float ResolveTargetHitRadius()
        {
            if (To == ThrowPropCharacterToType68.HumanBodyBone)
            {
                return ToBone switch
                {
                    HumanBodyBones.Head => 0.16f,
                    HumanBodyBones.Neck => 0.14f,
                    HumanBodyBones.Hips => 0.22f,
                    HumanBodyBones.Chest or HumanBodyBones.UpperChest or HumanBodyBones.Spine =>
                        0.2f,
                    _ => 0.18f,
                };
            }

            return 0.2f;
        }

        private void AttachTrail(GameObject clone)
        {
            if (Mathf.Approximately(TrailIntensity, 0f))
                return;

            var trailPrefab = ResolveResourcePrefab(TrailSource);
            if (trailPrefab == null)
                return;

            var trail = UnityEngine.Object.Instantiate(trailPrefab, clone.transform, false);
            trail.transform.localPosition = Vector3.zero;
            trail.transform.localRotation = Quaternion.identity;
            trail.transform.localScale = trailPrefab.transform.localScale * Mathf.Max(0f, TrailIntensity);
        }

        private static void SetRigidbodyVelocity(Rigidbody rb, Vector3 velocity)
        {
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = velocity;
#else
            rb.velocity = velocity;
#endif
        }

        private void PlaySound(string source, Vector3 worldPos)
        {
            var clip = ResolveAudioClip(source);
            if (clip == null)
                return;
            AudioSource.PlayClipAtPoint(clip, worldPos, Mathf.Clamp01(SoundVolume));
        }

        private static AudioClip ResolveAudioClip(string source)
        {
            var obj = ResolveResourceObject(source);
            return obj as AudioClip;
        }

        private static GameObject ResolveResourcePrefab(string source)
        {
            var obj = ResolveResourceObject(source);
            switch (obj)
            {
                case GameObject go:
                    return go;
                case Component component when component != null:
                    return component.gameObject;
                case GameObjectAsset asset when asset?.GameObject != null:
                    return asset.GameObject;
                default:
                    return null;
            }
        }

        private static object ResolveResourceObject(string source)
        {
            if (string.IsNullOrWhiteSpace(source) || Context.ResourceManager == null)
                return null;

            try
            {
                return Context.ResourceManager.ResolveResourceUri(source.Trim());
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[ThrowProp Node68] 리소스 해석 실패: " + source + " — " + ex.Message);
                return null;
            }
        }

        private bool TryResolveFromPosition(Vector3 toPos, out Vector3 result)
        {
            switch (TargetType)
            {
                case ThrowPropTargetType68.Character:
                    return TryResolveCharacterFrom(toPos, out result);
                case ThrowPropTargetType68.SceneObject:
                    return TryResolveSceneObjectFrom(toPos, out result);
                case ThrowPropTargetType68.WorldPosition:
                    return TryResolveWorldPositionFrom(toPos, out result);
                default:
                    result = Vector3.zero;
                    return false;
            }
        }

        private bool TryResolveToPosition(out Vector3 result)
        {
            switch (TargetType)
            {
                case ThrowPropTargetType68.Character:
                    return TryResolveCharacterTo(out result);
                case ThrowPropTargetType68.SceneObject:
                    return TryResolveSceneObjectTo(out result);
                case ThrowPropTargetType68.WorldPosition:
                    result = WorldPositionToWorldPosition;
                    return true;
                default:
                    result = Vector3.zero;
                    return false;
            }
        }

        private bool TryResolveCharacterFrom(Vector3 toPos, out Vector3 result)
        {
            result = Vector3.zero;
            switch (From)
            {
                case ThrowPropCharacterFromType68.ViewportBoundsRandom:
                    return TryResolveViewportRandom(
                        FromCamera,
                        FromAngle,
                        toPos,
                        ViewportSpawnDistance,
                        out result
                    );
                case ThrowPropCharacterFromType68.CameraPosition:
                    return TryResolveCameraPosition(FromCamera, out result);
                case ThrowPropCharacterFromType68.AboveCharacterHead:
                    return TryResolveAboveHead(Character, FromAboveHeadDistance, out result);
                case ThrowPropCharacterFromType68.SceneObject:
                    return TryResolveAssetPosition(FromSceneObject, out result);
                case ThrowPropCharacterFromType68.TransformPath:
                    return TryResolveCharacterTransformPath(FromTransform, out result);
                case ThrowPropCharacterFromType68.WorldPosition:
                    result = FromWorldPosition;
                    return true;
                default:
                    return false;
            }
        }

        private bool TryResolveCharacterTo(out Vector3 result)
        {
            result = Vector3.zero;
            if (Character == null)
                return false;

            switch (To)
            {
                case ThrowPropCharacterToType68.BodyRandomPosition:
                    return TryResolveCharacterBodyRandom(out result);
                case ThrowPropCharacterToType68.HumanBodyBone:
                    return TryResolveCharacterBonePos(ToBone, out result);
                case ThrowPropCharacterToType68.TransformPath:
                    return TryResolveCharacterTransformPath(ToTransform, out result);
                default:
                    return false;
            }
        }

        private bool TryResolveSceneObjectFrom(Vector3 toPos, out Vector3 result)
        {
            result = Vector3.zero;
            switch (SceneObjectFrom)
            {
                case ThrowPropSceneObjectFromType68.ViewportBoundsRandom:
                    return TryResolveViewportRandom(
                        SceneObjectFromCamera,
                        SceneObjectFromAngle,
                        toPos,
                        ViewportSpawnDistance,
                        out result
                    );
                case ThrowPropSceneObjectFromType68.SceneObject:
                    return TryResolveAssetPosition(SceneObjectFromSceneObject, out result);
                case ThrowPropSceneObjectFromType68.TransformPath:
                    return TryResolveAssetTransformPath(
                        SceneObjectFromSceneObject,
                        SceneObjectFromTransform,
                        out result
                    );
                case ThrowPropSceneObjectFromType68.WorldPosition:
                    result = SceneObjectFromWorldPosition;
                    return true;
                default:
                    return false;
            }
        }

        private bool TryResolveSceneObjectTo(out Vector3 result)
        {
            if (!string.IsNullOrWhiteSpace(SceneObjectToTransform))
                return TryResolveAssetTransformPath(
                    SceneObjectToSceneObject,
                    SceneObjectToTransform,
                    out result
                );

            return TryResolveAssetPosition(SceneObjectToSceneObject, out result);
        }

        private bool TryResolveWorldPositionFrom(Vector3 toPos, out Vector3 result)
        {
            result = Vector3.zero;
            switch (WorldPositionFrom)
            {
                case ThrowPropWorldPositionFromType68.ViewportBoundsRandom:
                    return TryResolveViewportRandom(
                        WorldPositionFromCamera,
                        WorldPositionFromAngle,
                        toPos,
                        ViewportSpawnDistance,
                        out result
                    );
                case ThrowPropWorldPositionFromType68.SceneObject:
                    return TryResolveAssetPosition(WorldPositionFromSceneObject, out result);
                case ThrowPropWorldPositionFromType68.TransformPath:
                    return TryResolveAssetTransformPath(
                        WorldPositionFromSceneObject,
                        WorldPositionFromTransform,
                        out result
                    );
                case ThrowPropWorldPositionFromType68.WorldPosition:
                    result = WorldPositionFromWorldPosition;
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryResolveViewportRandom(
            CameraAsset cameraAsset,
            float angle,
            Vector3 anchor,
            float distance,
            out Vector3 result
        )
        {
            result = Vector3.zero;
            var cam = ResolveCamera(cameraAsset);
            if (cam == null)
                return false;

            // 공식 Warudo: 화면 가장자리 랜덤 + 카메라에 매우 가까운 깊이(z)
            var depth = Mathf.Max(0.05f, distance);
            var pos = cam.GenerateRandomPositionAtViewportBounds(null, depth);

            if (Mathf.Abs(angle) > 0.01f)
            {
                var rot = Quaternion.AngleAxis(angle, Vector3.up);
                pos = anchor + rot * (pos - anchor);
            }

            result = pos;
            return true;
        }

        private static bool TryResolveCameraPosition(CameraAsset cameraAsset, out Vector3 result)
        {
            result = Vector3.zero;
            var cam = ResolveCamera(cameraAsset);
            if (cam == null)
                return false;

            // 렌즈 안쪽 클리pping 방지용으로 정면으로 아주 살짝만 밀어냄
            result = cam.transform.position + cam.transform.forward * 0.05f;
            return true;
        }

        private static bool TryResolveAboveHead(
            CharacterAsset character,
            float distance,
            out Vector3 result
        )
        {
            result = Vector3.zero;
            if (character?.HumanBodyBoneToBodyTransforms == null)
                return false;
            if (
                !character.HumanBodyBoneToBodyTransforms.TryGetValue(
                    HumanBodyBones.Head,
                    out var head
                )
                || head == null
            )
                return false;

            result = head.position + Vector3.up * Mathf.Max(0f, distance);
            return true;
        }

        private bool TryResolveCharacterBodyRandom(out Vector3 result)
        {
            result = Vector3.zero;
            if (Character?.HumanBodyBoneToBodyTransforms == null)
                return false;

            for (var attempt = 0; attempt < 8; attempt++)
            {
                var bone = BodyRandomBones[UnityEngine.Random.Range(0, BodyRandomBones.Length)];
                if (TryResolveCharacterBonePos(bone, out result))
                    return true;
            }

            return false;
        }

        private bool TryResolveCharacterBonePos(HumanBodyBones bone, out Vector3 pos)
        {
            pos = Vector3.zero;
            if (Character?.HumanBodyBoneToBodyTransforms == null)
                return false;
            if (
                Character.HumanBodyBoneToBodyTransforms.TryGetValue(bone, out var t)
                && t != null
            )
            {
                pos = t.position;
                return true;
            }

            return false;
        }

        private bool TryResolveCharacterTransformPath(string path, out Vector3 pos)
        {
            pos = Vector3.zero;
            if (Character?.GameObject == null)
                return false;

            var tr = ResolveTransformPath(Character.GameObject.transform, path);
            if (tr == null)
                return false;

            pos = tr.position;
            return true;
        }

        private static bool TryResolveAssetPosition(GameObjectAsset asset, out Vector3 pos)
        {
            pos = Vector3.zero;
            if (asset?.GameObject == null)
                return false;
            pos = asset.GameObject.transform.position;
            return true;
        }

        private static bool TryResolveAssetTransformPath(
            GameObjectAsset asset,
            string path,
            out Vector3 pos
        )
        {
            pos = Vector3.zero;
            if (asset?.GameObject == null)
                return false;

            var tr = ResolveTransformPath(asset.GameObject.transform, path);
            if (tr == null)
                return false;

            pos = tr.position;
            return true;
        }

        private static Transform ResolveTransformPath(Transform root, string path)
        {
            if (root == null)
                return null;

            if (string.IsNullOrWhiteSpace(path))
                return root;

            var trimmed = path.Trim();
            if (
                trimmed.Equals("Root Transform", StringComparison.OrdinalIgnoreCase)
                || trimmed == "/"
            )
                return root;

            return root.Find(trimmed) ?? FindTransformBySuffix(root, trimmed);
        }

        private static Transform FindTransformBySuffix(Transform root, string suffix)
        {
            if (root == null || string.IsNullOrEmpty(suffix))
                return null;

            if (root.name.Equals(suffix, StringComparison.OrdinalIgnoreCase))
                return root;

            for (var i = 0; i < root.childCount; i++)
            {
                var found = FindTransformBySuffix(root.GetChild(i), suffix);
                if (found != null)
                    return found;
            }

            return null;
        }

        private static Camera ResolveCamera(CameraAsset asset)
        {
            if (asset?.Camera != null)
                return asset.Camera;
            return Camera.main;
        }

        private Vector3 ComputeLaunchVelocity(Vector3 fromPos, Vector3 toPos)
        {
            var diff = toPos - fromPos;
            var horizontal = new Vector3(diff.x, 0f, diff.z);
            var hMag = horizontal.magnitude;
            var spd = Mathf.Max(0.01f, Speed);

            if (hMag < 0.01f)
            {
                var dir = diff.sqrMagnitude > 1e-6f ? diff.normalized : Vector3.forward;
                return dir * spd;
            }

            var t = hMag / spd;
            var g = Mathf.Abs(Physics.gravity.y);
            var vh = horizontal / t;
            var vy = diff.y / t + 0.5f * g * t;
            return new Vector3(vh.x, vy, vh.z);
        }

        private static void EnsureCollider(GameObject go)
        {
            if (go.GetComponentInChildren<Collider>() != null)
                return;

            var mesh = go.GetComponentInChildren<MeshFilter>();
            if (mesh != null && mesh.sharedMesh != null)
            {
                var mc = go.AddComponent<MeshCollider>();
                mc.sharedMesh = mesh.sharedMesh;
                mc.convex = true;
                return;
            }

            go.AddComponent<SphereCollider>().radius = 0.05f;
        }

        private static Rigidbody EnsureRigidbody(GameObject go)
        {
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
                return rb;
            return go.AddComponent<Rigidbody>();
        }

        // ───── HiddenIf ─────
        private bool HideRagdollSection() => TargetType != ThrowPropTargetType68.Character;

        private bool HideRagdollFields() =>
            HideRagdollSection() || !ActivateRagdollOnCollide;

        private bool HideRagdollResetWait() => HideRagdollFields() || !RagdollAutoReset;

        private bool HideViewportSpawnDistance()
        {
            if (TargetType == ThrowPropTargetType68.Character)
                return From != ThrowPropCharacterFromType68.ViewportBoundsRandom;
            if (TargetType == ThrowPropTargetType68.SceneObject)
                return SceneObjectFrom != ThrowPropSceneObjectFromType68.ViewportBoundsRandom;
            if (TargetType == ThrowPropTargetType68.WorldPosition)
                return WorldPositionFrom != ThrowPropWorldPositionFromType68.ViewportBoundsRandom;
            return true;
        }

        private bool HideCharacterField() => TargetType != ThrowPropTargetType68.Character;

        private bool HideCharacterFromTo() => TargetType != ThrowPropTargetType68.Character;

        private bool HideCharacterFromCamera() =>
            TargetType != ThrowPropTargetType68.Character
            || (
                From != ThrowPropCharacterFromType68.ViewportBoundsRandom
                && From != ThrowPropCharacterFromType68.AboveCharacterHead
                && From != ThrowPropCharacterFromType68.CameraPosition
            );

        private bool HideCharacterFromAngle() =>
            TargetType != ThrowPropTargetType68.Character
            || From != ThrowPropCharacterFromType68.ViewportBoundsRandom;

        private bool HideCharacterFromSceneObject() =>
            TargetType != ThrowPropTargetType68.Character
            || From != ThrowPropCharacterFromType68.SceneObject;

        private bool HideCharacterFromTransform() =>
            TargetType != ThrowPropTargetType68.Character
            || From != ThrowPropCharacterFromType68.TransformPath;

        private bool HideCharacterFromAboveHead() =>
            TargetType != ThrowPropTargetType68.Character
            || From != ThrowPropCharacterFromType68.AboveCharacterHead;

        private bool HideCharacterFromWorldPosition() =>
            TargetType != ThrowPropTargetType68.Character
            || From != ThrowPropCharacterFromType68.WorldPosition;

        private bool HideCharacterToBone() =>
            TargetType != ThrowPropTargetType68.Character
            || To != ThrowPropCharacterToType68.HumanBodyBone;

        private bool HideCharacterToTransform() =>
            TargetType != ThrowPropTargetType68.Character
            || To != ThrowPropCharacterToType68.TransformPath;

        private bool HideSceneObjectFromTo() => TargetType != ThrowPropTargetType68.SceneObject;

        private bool HideSceneObjectFromCamera() =>
            TargetType != ThrowPropTargetType68.SceneObject
            || SceneObjectFrom != ThrowPropSceneObjectFromType68.ViewportBoundsRandom;

        private bool HideSceneObjectFromAngle() =>
            TargetType != ThrowPropTargetType68.SceneObject
            || SceneObjectFrom != ThrowPropSceneObjectFromType68.ViewportBoundsRandom;

        private bool HideSceneObjectFromSceneObject() =>
            TargetType != ThrowPropTargetType68.SceneObject
            || (
                SceneObjectFrom != ThrowPropSceneObjectFromType68.SceneObject
                && SceneObjectFrom != ThrowPropSceneObjectFromType68.TransformPath
            );

        private bool HideSceneObjectFromTransform() =>
            TargetType != ThrowPropTargetType68.SceneObject
            || SceneObjectFrom != ThrowPropSceneObjectFromType68.TransformPath;

        private bool HideSceneObjectFromWorldPosition() =>
            TargetType != ThrowPropTargetType68.SceneObject
            || SceneObjectFrom != ThrowPropSceneObjectFromType68.WorldPosition;

        private bool HideSceneObjectToTransform() =>
            TargetType != ThrowPropTargetType68.SceneObject;

        private bool HideWorldPositionFromTo() =>
            TargetType != ThrowPropTargetType68.WorldPosition;

        private bool HideWorldPositionFromCamera() =>
            TargetType != ThrowPropTargetType68.WorldPosition
            || WorldPositionFrom != ThrowPropWorldPositionFromType68.ViewportBoundsRandom;

        private bool HideWorldPositionFromAngle() =>
            TargetType != ThrowPropTargetType68.WorldPosition
            || WorldPositionFrom != ThrowPropWorldPositionFromType68.ViewportBoundsRandom;

        private bool HideWorldPositionFromSceneObject() =>
            TargetType != ThrowPropTargetType68.WorldPosition
            || (
                WorldPositionFrom != ThrowPropWorldPositionFromType68.SceneObject
                && WorldPositionFrom != ThrowPropWorldPositionFromType68.TransformPath
            );

        private bool HideWorldPositionFromTransform() =>
            TargetType != ThrowPropTargetType68.WorldPosition
            || WorldPositionFrom != ThrowPropWorldPositionFromType68.TransformPath;

        private bool HideWorldPositionFromWorldPosition() =>
            TargetType != ThrowPropTargetType68.WorldPosition
            || WorldPositionFrom != ThrowPropWorldPositionFromType68.WorldPosition;
    }

    /// <summary><see cref="ThrowPropNode68"/> 가 생성한 프롭 클론의 수명·충돌·스틱을 처리합니다.</summary>
    public sealed class Node68ThrowPropTracker : MonoBehaviour
    {
        private ThrowPropNode68 _ownerNode;
        private float _aliveTime;
        private bool _despawnOnImpact;
        private bool _stickOnImpact;
        private GameObject _impactPrefab;
        private float _impactScale;
        private float _impactAliveTime;
        private AudioClip _impactSoundClip;
        private float _soundVolume;

        private bool _hasCollided;
        private bool _stuck;
        private float _spawnTime;

        private Collider[] _ownColliders;
        private float _sweepRadius;
        private Vector3 _prevPos;
        private const float SweepGraceTime = 0.05f;

        private bool _useTargetGate;
        private Vector3 _targetPoint;
        private Transform _targetRoot;
        private float _hitAcceptRadius;

        private Vector3 _spawnPos;
        private bool _despawned;
        private const float MinTravelBeforeTargetHit = 0.35f;

        public void Initialize(
            ThrowPropNode68 ownerNode,
            float aliveTime,
            bool despawnOnImpact,
            bool stickOnImpact,
            GameObject impactPrefab,
            float impactScale,
            float impactAliveTime,
            AudioClip impactSoundClip,
            float soundVolume,
            Vector3? targetPoint,
            Transform targetRoot,
            float hitAcceptRadius,
            Vector3 spawnPos
        )
        {
            _ownerNode = ownerNode;
            _aliveTime = aliveTime;
            _despawnOnImpact = despawnOnImpact;
            _stickOnImpact = stickOnImpact;
            _impactPrefab = impactPrefab;
            _impactScale = impactScale;
            _impactAliveTime = impactAliveTime;
            _impactSoundClip = impactSoundClip;
            _soundVolume = Mathf.Clamp01(soundVolume);
            _spawnTime = Time.time;
            _spawnPos = spawnPos;
            _despawned = false;

            _useTargetGate = targetPoint.HasValue && targetRoot != null;
            if (_useTargetGate)
            {
                _targetPoint = targetPoint.Value;
                _targetRoot = targetRoot;
                _hitAcceptRadius = Mathf.Max(0.08f, hitAcceptRadius);
            }

            _ownColliders = GetComponentsInChildren<Collider>(true);
            _sweepRadius = EstimateSweepRadius(_ownColliders);
            _prevPos = transform.position;
        }

        private bool IsNearTargetPoint(Vector3 pos)
        {
            if (!_useTargetGate)
                return false;
            return (pos - _targetPoint).sqrMagnitude
                <= _hitAcceptRadius * _hitAcceptRadius;
        }

        /// <summary>
        /// 타겟 캐릭터의 발·다리 콜라이더를 먼저 스치면 충돌로 처리하지 않고,
        /// 목표 본 높이/반경에 도달할 때까지 관통합니다.
        /// </summary>
        private bool ShouldDeferCharacterHit(Collider hit)
        {
            if (!_useTargetGate || hit == null || _targetRoot == null)
                return false;
            if (!hit.transform.IsChildOf(_targetRoot))
                return false;

            var pos = transform.position;
            if (IsNearTargetPoint(pos))
                return false;

            // 목표 본 높이 근처까지는 하체 충돌 무시
            if (pos.y >= _targetPoint.y - 0.1f)
                return false;

            return true;
        }

        private static bool SegmentPassesSphere(
            Vector3 a,
            Vector3 b,
            Vector3 center,
            float radius,
            out Vector3 hitPoint
        )
        {
            hitPoint = center;
            var ab = b - a;
            var ac = center - a;
            var abLenSq = ab.sqrMagnitude;
            if (abLenSq < 1e-8f)
                return false;

            var t = Vector3.Dot(ac, ab) / abLenSq;
            t = Mathf.Clamp01(t);
            var closest = a + ab * t;
            if ((closest - center).sqrMagnitude > radius * radius)
                return false;

            hitPoint = closest;
            return true;
        }

        private static float EstimateSweepRadius(Collider[] cols)
        {
            var r = 0.03f;
            if (cols == null)
                return r;

            foreach (var c in cols)
            {
                if (c == null)
                    continue;
                var ext = c.bounds.extents;
                r = Mathf.Max(r, ext.x, ext.y, ext.z);
            }

            return Mathf.Clamp(r, 0.02f, 0.5f);
        }

        private bool IsOwnCollider(Collider c)
        {
            if (c == null || _ownColliders == null)
                return false;

            for (var i = 0; i < _ownColliders.Length; i++)
            {
                if (_ownColliders[i] == c)
                    return true;
            }

            return false;
        }

        private void Update()
        {
            if (_despawned)
                return;
            if (Time.time - _spawnTime >= _aliveTime)
                DespawnSelf();
        }

        private void DespawnSelf()
        {
            if (_despawned)
                return;
            _despawned = true;

            transform.SetParent(null, true);

            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
#if UNITY_6000_0_OR_NEWER
                rb.linearVelocity = Vector3.zero;
#else
                rb.velocity = Vector3.zero;
#endif
                rb.angularVelocity = Vector3.zero;
            }

            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (_hasCollided || _stuck)
                return;
            if (Time.time - _spawnTime < SweepGraceTime)
            {
                _prevPos = transform.position;
                return;
            }

            var cur = transform.position;

            if (_useTargetGate && !_hasCollided)
            {
                var traveled = (cur - _spawnPos).sqrMagnitude;
                if (traveled >= MinTravelBeforeTargetHit * MinTravelBeforeTargetHit)
                {
                    if (IsNearTargetPoint(cur))
                    {
                        HandleHit(null, _targetPoint);
                        return;
                    }

                    if (
                        SegmentPassesSphere(
                            _prevPos,
                            cur,
                            _targetPoint,
                            _hitAcceptRadius,
                            out var prox
                        )
                    )
                    {
                        HandleHit(null, prox);
                        return;
                    }
                }
            }

            var delta = cur - _prevPos;
            var dist = delta.magnitude;
            if (dist > 1e-4f)
            {
                var hits = Physics.SphereCastAll(
                    _prevPos,
                    _sweepRadius,
                    delta / dist,
                    dist,
                    ~0,
                    QueryTriggerInteraction.Collide
                );
                if (hits != null && hits.Length > 0)
                {
                    for (var i = 0; i < hits.Length; i++)
                    {
                        var h = hits[i];
                        if (h.collider == null || IsOwnCollider(h.collider))
                            continue;
                        if (ShouldDeferCharacterHit(h.collider))
                            continue;

                        var p = h.point.sqrMagnitude < 1e-6f ? cur : h.point;
                        HandleHit(h.collider, p);
                        break;
                    }
                }
            }

            _prevPos = cur;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCollided || IsOwnCollider(other))
                return;
            if (Time.time - _spawnTime < SweepGraceTime)
                return;
            if (ShouldDeferCharacterHit(other))
                return;

            var p = other != null ? other.bounds.ClosestPoint(transform.position) : transform.position;
            HandleHit(other, p);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasCollided)
                return;
            if (Time.time - _spawnTime < SweepGraceTime)
                return;
            if (ShouldDeferCharacterHit(collision.collider))
                return;

            var p =
                collision.contactCount > 0 ? collision.GetContact(0).point : transform.position;
            HandleHit(collision.collider, p);
        }

        private void HandleHit(Collider hit, Vector3 pos)
        {
            if (_hasCollided)
                return;
            _hasCollided = true;

            if (_impactPrefab != null)
            {
                var fx = Instantiate(_impactPrefab);
                fx.transform.SetParent(null, false);
                fx.transform.position = pos;
                fx.transform.localScale = _impactPrefab.transform.localScale * _impactScale;
                Destroy(fx, _impactAliveTime);
            }

            if (_impactSoundClip != null)
                AudioSource.PlayClipAtPoint(_impactSoundClip, pos, _soundVolume);

            _ownerNode?.HandleCollision(pos);

            if (_stickOnImpact && !_stuck)
            {
                var rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
#if UNITY_6000_0_OR_NEWER
                    rb.linearVelocity = Vector3.zero;
#else
                    rb.velocity = Vector3.zero;
#endif
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                if (hit != null)
                    transform.SetParent(hit.transform, true);
                _stuck = true;
            }

            if (_despawnOnImpact)
                DespawnSelf();
        }

        private void OnDestroy()
        {
            _ownerNode = null;
        }
    }
}

#endif
