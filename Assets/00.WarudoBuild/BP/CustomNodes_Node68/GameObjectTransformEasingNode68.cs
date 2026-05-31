using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;
using Warudo.Core.Utils;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.CustomNodes
{
    /// <summary>연속 무빙(사인·핑퐁)과 단발 트윈·펀치·스핀·바운스·팝·넉백을 공통으로 다룹니다.</summary>
    public enum GameObjectTransformEasingMode
    {
        [Label("연속 흔들기 (둥둥·비틀기·스케일 호흡)")]
        OscillateLoop = 0,

        [Label("트윈 (목표 오프셋까지 이징)")]
        TweenOffset = 1,

        [Label("펀치 (DOTween Punch)")]
        Punch = 2,

        [Label("스핀 (회전 델타 이징)")]
        SpinOnce = 4,

        [Label("바운스 (위치 OutBounce)")]
        BounceOnce = 5,

        [Label("팝 스케일")]
        PopScaleOnce = 6,

        [Label("넉백 (한 방향 감속 이동)")]
        KnockbackOnce = 7,
    }

    /// <summary>
    /// <see cref="GameObjectTransformEasingMode.OscillateLoop"/> 전용 곡선.
    /// </summary>
    public enum GameObjectOscillatorShape
    {
        [Label("사인 (부드러운 루프)")]
        Sine = 0,

        [Label("핑퐁 + SmoothStep (끝에서 느려짐)")]
        SmoothPingPong = 1,
    }

    public enum GameObjectEasingSpace
    {
        [Label("로컬")]
        Local = 0,

        [Label("월드")]
        World = 1,
    }

    public enum GameObjectPunchChannel
    {
        [Label("위치")]
        Position = 0,

        [Label("스케일")]
        Scale = 1,

        [Label("회전")]
        Rotation = 2,
    }

    public enum GameObjectEffectTargetKind
    {
        [Label("루트 트랜스폼")]
        Root = 0,

        [Label("휴머노이드 본")]
        HumanoidBone = 1,

        [Label("하위 트랜스폼 경로")]
        TransformPath = 2,
    }

    /// <summary>
    /// <see cref="GameObjectAsset"/> 루트 또는 지정 본·하위 트랜스폼에 이징 기반 모션을 적용합니다.
    /// Enter 시점의 자세를 기준으로 저장하고, 정지 시 복구합니다.
    /// </summary>
    [NodeType(
        Id = "d4e5f6a7-b8c9-4012-b3e4-f5a6b7c8d9e0",
        Title = "GameObject Transform Easing Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.42f
    )]
    public sealed class GameObjectTransformEasingNode68 : Node
    {
        public override long GetVersion() => 21;

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        [DataInput]
        [Label("타겟")]
        [Description("캐릭터·프롭 등 GameObjectAsset. 아래 「적용 대상」으로 루트·본·하위 트랜스폼을 지정합니다.")]
        public GameObjectAsset Target;

        [DataInput]
        [Label("적용 대상")]
        [Description("모든 효과 종류에 공통 적용. 캐릭터는 휴머노이드 본, 프롭은 하위 경로를 고릅니다.")]
        public GameObjectEffectTargetKind EffectTargetKind = GameObjectEffectTargetKind.Root;

        [DataInput]
        [Label("효과 본 (HumanBodyBones)")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Warudo Transform Attachment 와 동일한 Human Body Bone.")]
        [HiddenIf(nameof(HideEffectHumanoidBoneField))]
        public HumanBodyBones PunchHumanoidBone = HumanBodyBones.Head;

        [DataInput]
        [Label("효과 트랜스폼")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Warudo Attach To Transform Path 와 같습니다. Root Transform = 에셋 루트.")]
        [HiddenIf(nameof(HideEffectTransformPathField))]
        [AutoComplete(nameof(AutoCompleteEffectTransformPath), forceSelection: true)]
        public string PunchTransformPath = "";

        /// <summary>구버전 <see cref="PunchBoneName"/> 마이그레이션용.</summary>
        [DataInput]
        [Hidden]
        public string PunchBoneName = "";

        [DataInput]
        [Label("효과 종류")]
        [HiddenIf(nameof(HideInShareBuild))]
        public GameObjectTransformEasingMode Mode = GameObjectTransformEasingMode.OscillateLoop;

        [DataInput]
        [Label("효과 횟수")]
        [IntegerSlider(1, 10)]
        [HiddenIf(nameof(HideOneShotRepeatFields))]
        public int OneShotRepeatCount = 1;

        [DataInput]
        [Label("효과 사이 딜레이 (초)")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "2회 이상일 때, 연속 재생 사이 대기 시간.")]
        [FloatSlider(0f, 2f)]
        [HiddenIf(nameof(HideOneShotRepeatDelayField))]
        public float OneShotRepeatDelay;

        [DataInput]
        [Label("효과 속도 배율")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "1=기본. 2=2배 빠름(시간·딜레이 절반). 각 효과의 재생 시간에 적용됩니다."
        )]
        [FloatSlider(0.25f, 4f)]
        [HiddenIf(nameof(HideOneShotRepeatFields))]
        public float OneShotSpeedMultiplier = 1f;

        [DataInput]
        [Label("좌표 공간")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "연속 흔들기·트윈·펀치·스핀·바운스·팝·넉백 벡터가 해석되는 공간입니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public GameObjectEasingSpace Space = GameObjectEasingSpace.Local;

        [DataInput]
        [Label("Unscaled Time")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "켜면 Time.timeScale 과 무관하게 DOTween·내부 시간이 진행됩니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool UseUnscaledTime;

        [DataInput]
        [Section("시작·정지 램프")]
        [Label("시작 램프 (초)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "「연속 흔들기」만. 중심 오프셋·「진동 시작 램프」·「진동 시작 지연」이 모두 분리되지 않을 때만 사용. "
                + "중심 오프셋·진동 진폭이 함께 0→100%로 이징됩니다."
        )]
        [FloatSlider(0f, 5f)]
        [HiddenIf(nameof(HideUnifiedStartBlendField))]
        public float StartBlendInDuration;

        [DataInput]
        [Label("진동 시작 램프 (초)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "「연속 흔들기」. 「진동 시작 지연」이 끝난 뒤 위치·회전·스케일 진동이 0→100%로 이징되는 시간. "
                + "0이면 그 직후 바로 전 진폭으로 진동합니다."
        )]
        [FloatSlider(0f, 5f)]
        [HiddenIf(nameof(HideStartBlendFields))]
        public float OscillateVibrationBlendDuration;

        [DataInput]
        [Label("진동 시작 지연 (초)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "「연속 흔들기」. 중심 오프셋 적용이 끝난 뒤 진동을 켜기 전에 멈춰 있는 시간. "
                + "0이면 즉시(또는 진동 시작 램프)로 이어집니다."
        )]
        [FloatSlider(0f, 5f)]
        [HiddenIf(nameof(HideStartBlendFields))]
        public float OscillateVibrationStartDelay;

        [DataInput]
        [Label("시작 램프 이징")]
        [HiddenIf(nameof(HideStartBlendDetailFields))]
        public Ease StartBlendEase = Ease.OutCubic;

        [DataInput]
        [Label("정지 램프 (초)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "정지·복구·자동 정지 시 기준 자세까지 걸리는 시간. 0이면 즉시 복구. "
                + "연속 흔들기에서 중심 오프셋이 올라가거나 내려올 때도 같은 시간을 씁니다."
        )]
        [FloatSlider(0f, 5f)]
        [HiddenIf(nameof(HideStopBlendOutDurationField))]
        public float StopBlendOutDuration;

        [DataInput]
        [Label("정지 램프 이징")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "정지·복구 시 진동·회전·스케일(및 단발 모션)이 기준 자세로 돌아올 때의 이징. "
                + "연속 흔들기의 중심 오프셋은 「중심 오프셋 복귀 이징」을 따릅니다."
        )]
        [HiddenIf(nameof(HideStopBlendEaseField))]
        public Ease StopBlendEase = Ease.InOutSine;

        [DataInput]
        [Label("정지·복구 시 Exit")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "켜면 「정지·복구」 입력 직후 Exit 로 이어집니다(기본). "
                + "끄면 정지·복구 시 Exit 뒤 노드는 실행되지 않습니다. "
                + "「정지·복구 완료」·「시작·재시작」의 Exit 는 그대로입니다."
        )]
        public bool StopTriggersExit = true;

        [DataInput]
        [Label("진동 형태")]
        [HiddenIf(nameof(HideOscillateFields))]
        public GameObjectOscillatorShape OscillatorShape = GameObjectOscillatorShape.Sine;

        [DataInput]
        [Label("진동 속도 (Hz)")]
        [FloatSlider(0.05f, 5f)]
        [HiddenIf(nameof(HideOscillateFields))]
        public float OscillatorFrequency = 1f;

        [DataInput]
        [Label("시간 위상 (초)")]
        [HiddenIf(nameof(HideOscillateFields))]
        public float OscillatorTimeOffset;

        [DataInput]
        [Label("둥둥 중심 오프셋")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "Enter 시 잡은 위치에 먼저 더한 뒤, 그 주변으로 「위치 변위」만큼 진동합니다. "
                + "Y만 올리면 대칭 왕복(사인)이어도 발이 바닥 아래로 많이 내려가지 않습니다. "
                + "보통 위치 변위 Y 이상으로 두면 안전합니다. 좌표 공간(로컬/월드)과 같이 적용됩니다. "
                + "아래 「중심 오프셋 이징」·「정지 램프 (초)」로 올라가기·내려오기를 조절합니다."
        )]
        [HiddenIf(nameof(HideOscillateFields))]
        public Vector3 OscillateCenterOffset;

        [DataInput]
        [Label("중심 오프셋 이징")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "Enter 시 중심 오프셋이 기준→설정값으로 이징됩니다. "
                + "「정지 램프 (초)」가 0이면 즉시 적용. 0보다 크면 같은 시간으로 올라가며, "
                + "이 시간 동안 위치·회전·스케일 진동은 꺼져 있습니다."
        )]
        [HiddenIf(nameof(HideOscillateFields))]
        public Ease OscillateCenterBlendEase = Ease.OutCubic;

        [DataInput]
        [Label("중심 오프셋 복귀 이징")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "「정지·복구」·자동 정지 시 중심 오프셋이 기준 자세로 내려올 때의 이징. "
                + "「정지 램프 (초)」와 같이 쓰며, 진동·회전·스케일은 「정지 램프 이징」을 따릅니다."
        )]
        [HiddenIf(nameof(HideOscillateCenterBlendOutEaseField))]
        public Ease OscillateCenterBlendOutEase = Ease.InOutSine;

        [DataInput]
        [Label("위치 변위")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "축마다 이 거리만큼 왕복합니다.")]
        [HiddenIf(nameof(HideOscillateFields))]
        public Vector3 OscillatePositionAmplitude = new Vector3(0f, 0.05f, 0f);

        [DataInput]
        [Label("회전 흔들림 (도)")]
        [HiddenIf(nameof(HideOscillateFields))]
        public Vector3 OscillateRotationAmplitude;

        [DataInput]
        [Label("스케일 호흡")]
        [FloatSlider(0f, 0.35f)]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "0이면 끔. 1 근처에서 커졌다 작아집니다.")]
        [HiddenIf(nameof(HideOscillateFields))]
        public float OscillateScalePulse;

        [DataInput]
        [Label("스케일 호흡 속도 배율")]
        [FloatSlider(0.25f, 3f)]
        [HiddenIf(nameof(HideOscillateFields))]
        public float OscillateScaleSpeedFactor = 1.1f;

        [DataInput]
        [Label("자동 정지 (초)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "「연속 흔들기」만. 0이면 수동 「정지·복구」까지 유지. "
                + "0보다 크면 Enter 시점부터 이 시간이 지난 뒤 「정지 램프」와 같이 복구됩니다."
        )]
        [FloatSlider(0f, 600f)]
        [HiddenIf(nameof(HideOscillateFields))]
        public float OscillateAutoStopAfterSeconds;

        [DataInput]
        [Label("위치 델타")]
        [HiddenIf(nameof(HideTweenFields))]
        public Vector3 TweenPositionDelta;

        [DataInput]
        [Label("회전 델타 (도)")]
        [HiddenIf(nameof(HideTweenFields))]
        public Vector3 TweenRotationDeltaEuler;

        [DataInput]
        [Label("스케일 배율 목표")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "(1,1,1)이면 스케일 트윈 없음. 예: (1.1,1.1,1.1)"
        )]
        [HiddenIf(nameof(HideTweenFields))]
        public Vector3 TweenScaleMultiplier = Vector3.one;

        [DataInput]
        [Label("트윈 시간 (초)")]
        [FloatSlider(0.02f, 10f)]
        [HiddenIf(nameof(HideTweenFields))]
        public float TweenDuration = 0.45f;

        [DataInput]
        [Label("트윈 이징")]
        [HiddenIf(nameof(HideTweenFields))]
        public Ease TweenEase = Ease.OutCubic;

        [DataInput]
        [Label("기준 자세로 복귀")]
        [HiddenIf(nameof(HideTweenFields))]
        public bool TweenReturnToBaseline;

        [DataInput]
        [Label("복귀 시간 (초)")]
        [FloatSlider(0.02f, 10f)]
        [HiddenIf(nameof(HideTweenReturnFields))]
        public float TweenReturnDuration = 0.45f;

        [DataInput]
        [Label("복귀 이징")]
        [HiddenIf(nameof(HideTweenReturnFields))]
        public Ease TweenReturnEase = Ease.InOutCubic;

        [DataInput]
        [Label("펀치 채널")]
        [HiddenIf(nameof(HidePunchFields))]
        public GameObjectPunchChannel PunchChannel = GameObjectPunchChannel.Position;

        [DataInput]
        [Label("펀치 세기")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "펀치 채널(위치·스케일·회전)에 적용되는 주 세기입니다."
        )]
        [HiddenIf(nameof(HidePunchFields))]
        public Vector3 PunchStrength = new Vector3(0f, 0.08f, 0f);

        [DataInput]
        [Label("펀치 회전 세기 (도)")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "위치·스케일 펀치와 동시에 회전 펀치를 더합니다. (0,0,0)이면 회전 추가 없음. "
                + "펀치 채널이 회전이면 주 세기와 합쳐집니다."
        )]
        [HiddenIf(nameof(HidePunchFields))]
        public Vector3 PunchRotationStrength;

        [DataInput]
        [Label("펀치 시간 (초)")]
        [FloatSlider(0.05f, 3f)]
        [HiddenIf(nameof(HidePunchFields))]
        public float PunchDuration = 0.35f;

        [DataInput]
        [Label("펀치 Vibrato")]
        [IntegerSlider(1, 30)]
        [HiddenIf(nameof(HidePunchFields))]
        public int PunchVibrato = 10;

        [DataInput]
        [Label("펀치 탄성")]
        [FloatSlider(0f, 1f)]
        [HiddenIf(nameof(HidePunchFields))]
        public float PunchElasticity = 0.5f;

        [DataInput]
        [Label("스핀 델타 (도)")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "로컬/월드에 더해질 오일러 각 한 번. 예: Y=360 이면 한 바퀴.")]
        [HiddenIf(nameof(HideSpinFields))]
        public Vector3 SpinEulerDelta = new Vector3(0f, 360f, 0f);

        [DataInput]
        [Label("스핀 시간 (초)")]
        [FloatSlider(0.05f, 10f)]
        [HiddenIf(nameof(HideSpinFields))]
        public float SpinDuration = 0.65f;

        [DataInput]
        [Label("스핀 이징")]
        [HiddenIf(nameof(HideSpinFields))]
        public Ease SpinEase = Ease.OutCubic;

        [DataInput]
        [Label("바운스 위치 델타")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "OutBounce 로 목표까지 갔다가 복귀합니다. Y+ 는 위로 통통.")]
        [HiddenIf(nameof(HideBounceFields))]
        public Vector3 BouncePositionDelta = new Vector3(0f, 0.12f, 0f);

        [DataInput]
        [Label("바운스 시간 (초)")]
        [FloatSlider(0.05f, 3f)]
        [HiddenIf(nameof(HideBounceFields))]
        public float BounceDuration = 0.4f;

        [DataInput]
        [Label("바운스 이징")]
        [HiddenIf(nameof(HideBounceFields))]
        public Ease BounceEase = Ease.OutBounce;

        [DataInput]
        [Label("바운스 후 복귀")]
        [HiddenIf(nameof(HideBounceFields))]
        public bool BounceReturnToBaseline = true;

        [DataInput]
        [Label("바운스 복귀 시간 (초)")]
        [FloatSlider(0.02f, 3f)]
        [HiddenIf(nameof(HideBounceReturnFields))]
        public float BounceReturnDuration = 0.25f;

        [DataInput]
        [Label("바운스 복귀 이징")]
        [HiddenIf(nameof(HideBounceReturnFields))]
        public Ease BounceReturnEase = Ease.InOutSine;

        /// <summary>구버전 <see cref="PopScaleRepeatCount"/> 마이그레이션용.</summary>
        [DataInput]
        [Hidden]
        public int PopScaleRepeatCount = 1;

        /// <summary>구버전 <see cref="PopScaleRepeatDelay"/> 마이그레이션용.</summary>
        [DataInput]
        [Hidden]
        public float PopScaleRepeatDelay;

        /// <summary>구버전 <see cref="PopScaleSpeedMultiplier"/> 마이그레이션용.</summary>
        [DataInput]
        [Hidden]
        public float PopScaleSpeedMultiplier = 1f;

        [DataInput]
        [Label("팝 스케일 목표")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "1=변화 없음. 예: (1.15,1.15,1.15) 이면 15% 커졌다 작아짐."
        )]
        [HiddenIf(nameof(HidePopScaleFields))]
        public Vector3 PopScalePeak = new Vector3(1.15f, 1.15f, 1.15f);

        [DataInput]
        [Label("팝 올라갈 시간 (초)")]
        [FloatSlider(0.01f, 2f)]
        [HiddenIf(nameof(HidePopScaleFields))]
        public float PopScaleUpDuration = 0.12f;

        [DataInput]
        [Label("팝 내려올 시간 (초)")]
        [FloatSlider(0.01f, 2f)]
        [HiddenIf(nameof(HidePopScaleFields))]
        public float PopScaleDownDuration = 0.18f;

        [DataInput]
        [Label("팝 올라갈 이징")]
        [HiddenIf(nameof(HidePopScaleFields))]
        public Ease PopScaleUpEase = Ease.OutBack;

        [DataInput]
        [Label("팝 내려올 이징")]
        [HiddenIf(nameof(HidePopScaleFields))]
        public Ease PopScaleDownEase = Ease.InOutCubic;

        [DataInput]
        [Label("넉백 위치 델타")]
        [Description(
            Node68FlavorEmbedded.ShareBuild ? ""
                : "한 방향으로 밀림. 로컬 Z- 는 뒤로 밀림(캐릭터 정면 기준)."
        )]
        [HiddenIf(nameof(HideKnockbackFields))]
        public Vector3 KnockbackPositionDelta = new Vector3(0f, 0f, -0.2f);

        [DataInput]
        [Label("넉백 시간 (초)")]
        [FloatSlider(0.05f, 3f)]
        [HiddenIf(nameof(HideKnockbackFields))]
        public float KnockbackDuration = 0.35f;

        [DataInput]
        [Label("넉백 이징")]
        [HiddenIf(nameof(HideKnockbackFields))]
        public Ease KnockbackEase = Ease.OutCubic;

        [DataInput]
        [Label("넉백 후 복귀")]
        [HiddenIf(nameof(HideKnockbackFields))]
        public bool KnockbackReturnToBaseline = true;

        [DataInput]
        [Label("넉백 복귀 시간 (초)")]
        [FloatSlider(0.02f, 3f)]
        [HiddenIf(nameof(HideKnockbackReturnFields))]
        public float KnockbackReturnDuration = 0.45f;

        [DataInput]
        [Label("넉백 복귀 이징")]
        [HiddenIf(nameof(HideKnockbackReturnFields))]
        public Ease KnockbackReturnEase = Ease.InOutQuad;

        private Sequence _sequence;
        private Tweener _punchTween;

        /// <summary>
        /// DOTween 이 Transform 이 아닌 델타 값만 갱신하고,
        /// <see cref="OnLateUpdate"/> 에서 Warudo 동기화 이후에 실제 자세에 합성합니다.
        /// </summary>
        private bool _manualMotionActive;
        private Vector3 _motionPosDelta;
        private Vector3 _motionRotDeltaEuler;
        private Vector3 _motionScaleMul = Vector3.one;

        private bool _oscillateActive;
        private bool _oscRampingDown;
        private float _oscBlendStartTime;
        private float _stopRampStartTime;
        private float _stopCenterEnvelopeWhenStopBegan;
        private float _stopVibrationEnvelopeWhenStopBegan;

        private readonly struct OscillateMotionEnvelopes
        {
            public readonly float Center;
            public readonly float Vibration;

            public OscillateMotionEnvelopes(float center, float vibration)
            {
                Center = center;
                Vibration = vibration;
            }

            public static OscillateMotionEnvelopes Uniform(float value) => new(value, value);
        }

        private float _oscAutoStopDeadline = -1f;

        private bool _hasBaseline;

        private Transform _activeTransform;

        private Vector3 _baseLocalPos;
        private Quaternion _baseLocalRot;
        private Vector3 _baseLocalScale;
        private Vector3 _baseWorldPos;
        private Quaternion _baseWorldRot;

        private bool HideOscillateFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.OscillateLoop;

        private bool HasOscillateCenterOffset() => OscillateCenterOffset.sqrMagnitude > 1e-10f;

        private float GetOscillateCenterBlendDuration() =>
            HasOscillateCenterOffset() ? Mathf.Max(0f, StopBlendOutDuration) : 0f;

        private bool UsesOscillateCenterBlend() => GetOscillateCenterBlendDuration() > 1e-6f;

        private bool HideOscillateCenterBlendOutEaseField() =>
            HideOscillateFields() || StopBlendOutDuration <= 1e-6f;

        private bool HideTweenFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.TweenOffset;

        private bool HideTweenReturnFields() =>
            HideInShareBuild()
            || Mode != GameObjectTransformEasingMode.TweenOffset
            || !TweenReturnToBaseline;

        private bool HidePunchFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.Punch;

        private bool IsCharacterTarget() => Target is CharacterAsset;

        private bool HideEffectHumanoidBoneField() =>
            HideInShareBuild()
            || EffectTargetKind != GameObjectEffectTargetKind.HumanoidBone
            || !IsCharacterTarget();

        private bool HideEffectTransformPathField() =>
            HideInShareBuild()
            || EffectTargetKind != GameObjectEffectTargetKind.TransformPath;

        private bool HideSpinFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.SpinOnce;

        private bool HideBounceFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.BounceOnce;

        private bool HideBounceReturnFields() =>
            HideInShareBuild()
            || Mode != GameObjectTransformEasingMode.BounceOnce
            || !BounceReturnToBaseline;

        private bool HidePopScaleFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.PopScaleOnce;

        private bool HideOneShotRepeatFields() =>
            HideInShareBuild() || Mode == GameObjectTransformEasingMode.OscillateLoop;

        private bool HideOneShotRepeatDelayField() =>
            HideOneShotRepeatFields() || OneShotRepeatCount <= 1;

        private bool HideKnockbackFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.KnockbackOnce;

        private bool HideKnockbackReturnFields() =>
            HideInShareBuild()
            || Mode != GameObjectTransformEasingMode.KnockbackOnce
            || !KnockbackReturnToBaseline;

        private bool HideStartBlendFields() =>
            HideInShareBuild() || Mode != GameObjectTransformEasingMode.OscillateLoop;

        private bool UsesSplitOscillateStart() =>
            UsesOscillateCenterBlend()
            || OscillateVibrationBlendDuration > 1e-6f
            || OscillateVibrationStartDelay > 1e-6f;

        private bool HideUnifiedStartBlendField() =>
            HideStartBlendFields() || UsesSplitOscillateStart();

        private float GetOscillateVibrationPhaseStartTime() =>
            GetOscillateCenterBlendDuration() + OscillateVibrationStartDelay;

        private bool HideStartBlendDetailFields() =>
            HideInShareBuild()
            || Mode != GameObjectTransformEasingMode.OscillateLoop
            || (
                StartBlendInDuration <= 1e-6f
                && !UsesOscillateCenterBlend()
                && OscillateVibrationBlendDuration <= 1e-6f
                && OscillateVibrationStartDelay <= 1e-6f
            );

        private bool HideStopBlendOutDurationField() => HideInShareBuild();

        private bool HideStopBlendEaseField() =>
            HideInShareBuild() || StopBlendOutDuration <= 1e-6f;

        private static bool IsOneShotManualMotionMode(GameObjectTransformEasingMode mode) =>
            mode == GameObjectTransformEasingMode.Punch
            || mode == GameObjectTransformEasingMode.SpinOnce
            || mode == GameObjectTransformEasingMode.BounceOnce
            || mode == GameObjectTransformEasingMode.PopScaleOnce
            || mode == GameObjectTransformEasingMode.KnockbackOnce;

        private void BindManualMotionSequence(Sequence seq, bool resetDeltasOnComplete = true)
        {
            seq.OnKill(() => _sequence = null);
            seq.OnComplete(() =>
            {
                _sequence = null;
                if (resetDeltasOnComplete)
                {
                    _manualMotionActive = false;
                    ResetMotionDeltas();
                }
                InvokeFlow(nameof(OnMotionComplete));
            });
            _sequence = seq;
        }

        private int GetOneShotRepeatCount() => Mathf.Clamp(OneShotRepeatCount, 1, 10);

        private float GetOneShotRepeatGap() => Mathf.Max(0f, OneShotRepeatDelay);

        private float GetOneShotSpeedMultiplier() => Mathf.Max(0.25f, OneShotSpeedMultiplier);

        private float ScaleOneShotDuration(float duration, float minDuration = 0.01f) =>
            Mathf.Max(minDuration, duration / GetOneShotSpeedMultiplier());

        private void AppendOneShotRepeatGap(Sequence seq, int cycleIndex)
        {
            var gap = GetOneShotRepeatGap();
            if (cycleIndex > 0 && gap > 1e-6f)
                seq.AppendInterval(ScaleOneShotDuration(gap, minDuration: 0f));
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyShareSuffixToDisplayName();
        }

        private const int LegacyShakeMode = 3;
        private const int LegacyPopScaleTwiceMode = 8;

        private const string RootTransformPathLabel = "Root Transform";

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyShareSuffixToDisplayName();
            if ((int)Mode == LegacyShakeMode)
                SetDataInput(nameof(Mode), GameObjectTransformEasingMode.Punch, broadcast: true);
            if ((int)Mode == LegacyPopScaleTwiceMode)
            {
                SetDataInput(nameof(Mode), GameObjectTransformEasingMode.PopScaleOnce, broadcast: true);
                SetDataInput(nameof(OneShotRepeatCount), 2, broadcast: true);
            }
            MigrateLegacyPopScaleRepeatFields();
            MigrateLegacyPunchBoneName();
            MigrateLegacyPunchEffectTarget(serialized);
        }

        private void MigrateLegacyPopScaleRepeatFields()
        {
            if (PopScaleRepeatCount > 1 && OneShotRepeatCount <= 1)
                SetDataInput(nameof(OneShotRepeatCount), PopScaleRepeatCount, broadcast: true);
            if (PopScaleRepeatDelay > 1e-6f && OneShotRepeatDelay <= 1e-6f)
                SetDataInput(nameof(OneShotRepeatDelay), PopScaleRepeatDelay, broadcast: true);
            if (
                Mathf.Abs(PopScaleSpeedMultiplier - 1f) > 1e-4f
                && Mathf.Abs(OneShotSpeedMultiplier - 1f) <= 1e-4f
            )
                SetDataInput(nameof(OneShotSpeedMultiplier), PopScaleSpeedMultiplier, broadcast: true);
        }

        /// <summary>
        /// 구버전(v13)은 펀치 모드에서만 본·경로를 썼습니다. 동일 동작을 유지합니다.
        /// </summary>
        private void MigrateLegacyPunchEffectTarget(SerializedNode serialized)
        {
            if (serialized.version >= 14)
                return;
            if (EffectTargetKind != GameObjectEffectTargetKind.Root)
                return;
            if (Mode != GameObjectTransformEasingMode.Punch)
                return;

            if (Target is CharacterAsset)
            {
                SetDataInput(
                    nameof(EffectTargetKind),
                    GameObjectEffectTargetKind.HumanoidBone,
                    broadcast: true
                );
                return;
            }

            SetDataInput(
                nameof(EffectTargetKind),
                GameObjectEffectTargetKind.TransformPath,
                broadcast: true
            );
        }

        private void MigrateLegacyPunchBoneName()
        {
            if (string.IsNullOrWhiteSpace(PunchBoneName))
                return;

            var trimmed = PunchBoneName.Trim();
            if (
                Enum.TryParse<HumanBodyBones>(trimmed, ignoreCase: true, out var bone)
                && bone != HumanBodyBones.LastBone
            )
                SetDataInput(nameof(PunchHumanoidBone), bone, broadcast: true);
            else
                SetDataInput(nameof(PunchTransformPath), trimmed, broadcast: true);

            SetDataInput(nameof(PunchBoneName), string.Empty, broadcast: true);
        }

        private const string ShareDisplayNameSuffix = " Shr";

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "GameObject Transform Easing Node68";

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

        private Transform ResolveTransform()
        {
            if (Target == null)
                return null;
            var go = Target.GameObject;
            return go != null ? go.transform : null;
        }

        private Transform ResolveEffectTransform()
        {
            var root = ResolveTransform();
            if (root == null)
                return null;

            if (EffectTargetKind == GameObjectEffectTargetKind.Root)
                return root;

            var effectTr = ResolveEffectTargetTransform();
            return effectTr != null ? effectTr : root;
        }

        private Transform GetActiveTransform()
        {
            if (_activeTransform != null)
                return _activeTransform;
            return ResolveTransform();
        }

        private Transform ResolveEffectTargetTransform()
        {
            switch (EffectTargetKind)
            {
                case GameObjectEffectTargetKind.HumanoidBone
                    when Target is CharacterAsset character:
                {
                    var map = character.HumanBodyBoneToBodyTransforms;
                    if (map == null || map.Count == 0)
                        return null;
                    return map.TryGetValue(PunchHumanoidBone, out var boneTr) ? boneTr : null;
                }
                case GameObjectEffectTargetKind.TransformPath:
                    return ResolvePropTransformPath(PunchTransformPath);
                default:
                    return null;
            }
        }

        private Transform ResolvePropTransformPath(string path)
        {
            var root = ResolveTransform();
            if (root == null)
                return null;

            if (IsRootTransformPath(path))
                return root;

            var trimmed = path.Trim();
            if (Target is CharacterAsset character)
            {
                try
                {
                    return character.FindChildTransform(trimmed);
                }
                catch
                {
                    return null;
                }
            }

            return root.Find(trimmed);
        }

        private static bool IsRootTransformPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return true;
            return path.Trim().Equals(RootTransformPathLabel, StringComparison.OrdinalIgnoreCase);
        }

        private UniTask<AutoCompleteList> AutoCompleteEffectTransformPath()
        {
            if (Node68BuildRuntime.IsShareBuild())
                return UniTask.FromResult(new List<AutoCompleteEntry>().ToAutoCompleteList());

            var entries = new List<AutoCompleteEntry>
            {
                new AutoCompleteEntry
                {
                    label = RootTransformPathLabel,
                    value = string.Empty,
                },
            };

            var root = ResolveTransform();
            if (root == null)
                return UniTask.FromResult(entries.ToAutoCompleteList());

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectPropTransformPathEntries(root, root, entries, seen);
            entries.Sort((a, b) => string.Compare(a.label, b.label, StringComparison.OrdinalIgnoreCase));
            return UniTask.FromResult(entries.ToAutoCompleteList());
        }

        private static void CollectPropTransformPathEntries(
            Transform root,
            Transform current,
            List<AutoCompleteEntry> entries,
            HashSet<string> seen
        )
        {
            foreach (Transform child in current)
            {
                var path = GetRelativeTransformPath(root, child);
                if (!string.IsNullOrEmpty(path) && seen.Add(path))
                {
                    entries.Add(
                        new AutoCompleteEntry
                        {
                            label = path,
                            value = path,
                        }
                    );
                }

                CollectPropTransformPathEntries(root, child, entries, seen);
            }
        }

        private static string GetRelativeTransformPath(Transform root, Transform target)
        {
            if (root == null || target == null || target == root)
                return string.Empty;

            var stack = new Stack<string>();
            var current = target;
            while (current != null && current != root)
            {
                stack.Push(current.name);
                current = current.parent;
            }

            if (current != root)
                return string.Empty;

            return string.Join("/", stack);
        }

        private static float EvalOscillator(
            float timeSeconds,
            float cyclesPerSecond,
            GameObjectOscillatorShape shape
        )
        {
            if (cyclesPerSecond <= 0f)
                return 0f;
            switch (shape)
            {
                case GameObjectOscillatorShape.SmoothPingPong:
                {
                    var p = Mathf.PingPong(timeSeconds * cyclesPerSecond, 1f);
                    return Mathf.Lerp(-1f, 1f, Mathf.SmoothStep(0f, 1f, p));
                }
                default:
                    return Mathf.Sin(timeSeconds * cyclesPerSecond * (Mathf.PI * 2f));
            }
        }

        private void ResetMotionDeltas()
        {
            _motionPosDelta = Vector3.zero;
            _motionRotDeltaEuler = Vector3.zero;
            _motionScaleMul = Vector3.one;
        }

        private void KillActiveTweens()
        {
            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill(false);
            _sequence = null;

            if (_punchTween != null && _punchTween.IsActive())
                _punchTween.Kill(false);
            _punchTween = null;
        }

        private void ApplyManualMotionFrame(Transform tr)
        {
            if (Space == GameObjectEasingSpace.Local)
            {
                tr.localPosition = _baseLocalPos + _motionPosDelta;
                tr.localRotation = _baseLocalRot * Quaternion.Euler(_motionRotDeltaEuler);
                tr.localScale = Vector3.Scale(_baseLocalScale, _motionScaleMul);
            }
            else
            {
                tr.position = _baseWorldPos + _motionPosDelta;
                tr.rotation = _baseWorldRot * Quaternion.Euler(_motionRotDeltaEuler);
                tr.localScale = Vector3.Scale(_baseLocalScale, _motionScaleMul);
            }
        }

        private void CaptureBaseline(Transform tr)
        {
            _baseLocalPos = tr.localPosition;
            _baseLocalRot = tr.localRotation;
            _baseLocalScale = tr.localScale;
            _baseWorldPos = tr.position;
            _baseWorldRot = tr.rotation;
            _hasBaseline = true;
        }

        private void ApplyBaselineTransform(Transform tr)
        {
            if (!_hasBaseline)
                return;
            if (Space == GameObjectEasingSpace.Local)
            {
                tr.localPosition = _baseLocalPos;
                tr.localRotation = _baseLocalRot;
                tr.localScale = _baseLocalScale;
            }
            else
            {
                tr.position = _baseWorldPos;
                tr.rotation = _baseWorldRot;
                tr.localScale = _baseLocalScale;
            }
        }

        private float GetNow() => UseUnscaledTime ? Time.unscaledTime : Time.time;

        private Continuation ReturnStopFlow() => StopTriggersExit ? Exit : null;

        private T ConfigureManualMotionTween<T>(T tween)
            where T : Tween
        {
            if (tween == null)
                return null;
            return tween.SetUpdate(UpdateType.Late, UseUnscaledTime);
        }

        private Sequence CreateManualMotionSequence()
        {
            return DOTween.Sequence().SetUpdate(UpdateType.Late, UseUnscaledTime);
        }

        private void StopAndRestoreHard()
        {
            _oscillateActive = false;
            _oscRampingDown = false;
            _oscAutoStopDeadline = -1f;
            _manualMotionActive = false;
            KillActiveTweens();
            ResetMotionDeltas();
            var tr = GetActiveTransform();
            if (tr != null && _hasBaseline)
                ApplyBaselineTransform(tr);
            _hasBaseline = false;
            _activeTransform = null;
        }

        private float SampleOscillateUnifiedEnvelope()
        {
            if (StartBlendInDuration <= 1e-6f)
                return 1f;
            var up = Mathf.Clamp01((GetNow() - _oscBlendStartTime) / StartBlendInDuration);
            return DOVirtual.EasedValue(0f, 1f, up, StartBlendEase);
        }

        private float SampleOscillateCenterEnvelope()
        {
            if (!UsesSplitOscillateStart())
                return SampleOscillateUnifiedEnvelope();

            if (!UsesOscillateCenterBlend())
                return 1f;

            var dur = GetOscillateCenterBlendDuration();
            var up = Mathf.Clamp01((GetNow() - _oscBlendStartTime) / dur);
            return DOVirtual.EasedValue(0f, 1f, up, OscillateCenterBlendEase);
        }

        private float SampleOscillateVibrationEnvelope()
        {
            if (!UsesSplitOscillateStart())
                return SampleOscillateUnifiedEnvelope();

            var elapsed = GetNow() - _oscBlendStartTime;
            var vibrationStart = GetOscillateVibrationPhaseStartTime();
            if (elapsed < vibrationStart)
                return 0f;

            if (OscillateVibrationBlendDuration <= 1e-6f)
                return 1f;

            var up = Mathf.Clamp01(
                (elapsed - vibrationStart) / OscillateVibrationBlendDuration
            );
            return DOVirtual.EasedValue(0f, 1f, up, StartBlendEase);
        }

        private OscillateMotionEnvelopes SampleOscillateEnvelopes() =>
            new(SampleOscillateCenterEnvelope(), SampleOscillateVibrationEnvelope());

        private void CaptureOscillateEnvelopesForStop()
        {
            var env = SampleOscillateEnvelopes();
            _stopCenterEnvelopeWhenStopBegan = env.Center;
            _stopVibrationEnvelopeWhenStopBegan = env.Vibration;
        }

        private OscillateMotionEnvelopes CurrentOscillateEnvelopesDuringStopRamp()
        {
            var dur = Mathf.Max(1e-6f, StopBlendOutDuration);
            var p = Mathf.Clamp01((GetNow() - _stopRampStartTime) / dur);
            return new OscillateMotionEnvelopes(
                DOVirtual.EasedValue(
                    _stopCenterEnvelopeWhenStopBegan,
                    0f,
                    p,
                    OscillateCenterBlendOutEase
                ),
                DOVirtual.EasedValue(
                    _stopVibrationEnvelopeWhenStopBegan,
                    0f,
                    p,
                    StopBlendEase
                )
            );
        }

        private void FinishOscillateStopRamp()
        {
            _oscRampingDown = false;
            _oscillateActive = false;
            _oscAutoStopDeadline = -1f;
            var tr = GetActiveTransform();
            if (tr != null && _hasBaseline)
                ApplyBaselineTransform(tr);
            _hasBaseline = false;
            _activeTransform = null;
            InvokeFlow(nameof(OnStopComplete));
        }

        private void ClearBaselineFlagsOnly()
        {
            _oscillateActive = false;
            _oscRampingDown = false;
            _hasBaseline = false;
            _activeTransform = null;
        }

        private void PlayTweenReturnToBaselineOnly()
        {
            var dur = Mathf.Max(0.02f, StopBlendOutDuration);
            var retEase = StopBlendEase;

            _manualMotionActive = true;
            _sequence = CreateManualMotionSequence();
            _sequence.Join(
                DOTween
                    .To(() => _motionPosDelta, x => _motionPosDelta = x, Vector3.zero, dur)
                    .SetEase(retEase)
            );
            _sequence.Join(
                DOTween
                    .To(() => _motionRotDeltaEuler, x => _motionRotDeltaEuler = x, Vector3.zero, dur)
                    .SetEase(retEase)
            );
            _sequence.Join(
                DOTween
                    .To(() => _motionScaleMul, x => _motionScaleMul = x, Vector3.one, dur)
                    .SetEase(retEase)
            );

            _sequence.OnKill(() => _sequence = null);
            _sequence.OnComplete(() =>
            {
                _sequence = null;
                _manualMotionActive = false;
                ResetMotionDeltas();
                ClearBaselineFlagsOnly();
                InvokeFlow(nameof(OnStopComplete));
            });
        }

        [FlowInput]
        [Label("시작·재시작")]
        public Continuation Enter()
        {
            KillActiveTweens();
            _oscillateActive = false;
            _oscRampingDown = false;
            _oscAutoStopDeadline = -1f;
            _manualMotionActive = false;
            ResetMotionDeltas();
            _activeTransform = null;

            var tr = ResolveEffectTransform();
            if (tr == null)
                return Exit;

            _activeTransform = tr;
            CaptureBaseline(tr);
            _oscBlendStartTime = GetNow();

            switch (Mode)
            {
                case GameObjectTransformEasingMode.OscillateLoop:
                    _oscillateActive = true;
                    if (OscillateAutoStopAfterSeconds > 1e-6f)
                        _oscAutoStopDeadline = GetNow() + OscillateAutoStopAfterSeconds;
                    break;
                case GameObjectTransformEasingMode.TweenOffset:
                    PlayTweenOffset();
                    break;
                case GameObjectTransformEasingMode.Punch:
                    PlayPunch();
                    break;
                case GameObjectTransformEasingMode.SpinOnce:
                    PlaySpinOnce();
                    break;
                case GameObjectTransformEasingMode.BounceOnce:
                    PlayBounceOnce();
                    break;
                case GameObjectTransformEasingMode.PopScaleOnce:
                    PlayPopScaleOnce();
                    break;
                case GameObjectTransformEasingMode.KnockbackOnce:
                    PlayKnockbackOnce();
                    break;
            }

            if (_manualMotionActive)
                ApplyManualMotionFrame(tr);
            else if (_oscillateActive)
                ApplyOscillateFrame(tr, SampleOscillateEnvelopes());

            return Exit;
        }

        /// <summary>
        /// 연속 흔들기 런타임 플래그는 Enter 시점의 <see cref="Mode"/>와 어긋날 수 있습니다.
        /// (예: 플로우 직전 데이터 입력이 바뀌어 Mode만 먼저 바뀐 경우) 그때도 정지가 되도록
        /// 활성/램프 중이면 현재 Mode와 무관하게 이 경로를 탑니다.
        /// </summary>
        private Continuation StopOscillateRampOrComplete()
        {
            _oscAutoStopDeadline = -1f;

            if (!_hasBaseline || (!(_oscillateActive || _oscRampingDown)))
            {
                InvokeFlow(nameof(OnStopComplete));
                return ReturnStopFlow();
            }

            if (_oscRampingDown)
            {
                var current = CurrentOscillateEnvelopesDuringStopRamp();
                _stopCenterEnvelopeWhenStopBegan = current.Center;
                _stopVibrationEnvelopeWhenStopBegan = current.Vibration;
                _stopRampStartTime = GetNow();
                return ReturnStopFlow();
            }

            if (StopBlendOutDuration <= 1e-6f)
            {
                StopAndRestoreHard();
                InvokeFlow(nameof(OnStopComplete));
                return ReturnStopFlow();
            }

            _oscRampingDown = true;
            _stopRampStartTime = GetNow();
            CaptureOscillateEnvelopesForStop();
            return ReturnStopFlow();
        }

        [FlowInput]
        [Label("정지·복구")]
        public Continuation Stop()
        {
            var tr = GetActiveTransform();

            if (_oscillateActive || _oscRampingDown)
                return StopOscillateRampOrComplete();

            if (Mode == GameObjectTransformEasingMode.OscillateLoop)
            {
                _oscAutoStopDeadline = -1f;
                InvokeFlow(nameof(OnStopComplete));
                return ReturnStopFlow();
            }

            if (Mode == GameObjectTransformEasingMode.TweenOffset)
            {
                if (tr == null || !_hasBaseline)
                {
                    StopAndRestoreHard();
                    InvokeFlow(nameof(OnStopComplete));
                    return ReturnStopFlow();
                }

                if (StopBlendOutDuration <= 1e-6f)
                {
                    StopAndRestoreHard();
                    InvokeFlow(nameof(OnStopComplete));
                    return ReturnStopFlow();
                }

                KillActiveTweens();
                PlayTweenReturnToBaselineOnly();
                return ReturnStopFlow();
            }

            if (IsOneShotManualMotionMode(Mode))
            {
                if (tr == null || !_hasBaseline)
                {
                    StopAndRestoreHard();
                    InvokeFlow(nameof(OnStopComplete));
                    return ReturnStopFlow();
                }

                KillActiveTweens();
                if (StopBlendOutDuration <= 1e-6f)
                {
                    ApplyBaselineTransform(tr);
                    _manualMotionActive = false;
                    ResetMotionDeltas();
                    ClearBaselineFlagsOnly();
                    InvokeFlow(nameof(OnStopComplete));
                    return ReturnStopFlow();
                }

                PlayTweenReturnToBaselineOnly();
                return ReturnStopFlow();
            }

            StopAndRestoreHard();
            InvokeFlow(nameof(OnStopComplete));
            return ReturnStopFlow();
        }

        private bool TryGetTweenOffsetChannels(
            out bool hasPos,
            out bool hasRot,
            out bool scaleAlmostOne
        )
        {
            hasPos = TweenPositionDelta.sqrMagnitude > 1e-10f;
            hasRot = TweenRotationDeltaEuler.sqrMagnitude > 1e-10f;
            scaleAlmostOne =
                Mathf.Abs(TweenScaleMultiplier.x - 1f) > 1e-4f
                || Mathf.Abs(TweenScaleMultiplier.y - 1f) > 1e-4f
                || Mathf.Abs(TweenScaleMultiplier.z - 1f) > 1e-4f;
            return hasPos || hasRot || scaleAlmostOne;
        }

        private void AppendTweenOffsetCycle(
            Sequence seq,
            bool hasPos,
            bool hasRot,
            bool scaleAlmostOne
        )
        {
            var dur = ScaleOneShotDuration(TweenDuration, 0.02f);
            var retDur = ScaleOneShotDuration(TweenReturnDuration, 0.02f);
            var ease = TweenEase;
            var retEase = TweenReturnEase;

            var outPhase = CreateManualMotionSequence();
            if (hasPos)
                outPhase.Join(
                    DOTween
                        .To(() => _motionPosDelta, x => _motionPosDelta = x, TweenPositionDelta, dur)
                        .SetEase(ease)
                );
            if (hasRot)
                outPhase.Join(
                    DOTween
                        .To(
                            () => _motionRotDeltaEuler,
                            x => _motionRotDeltaEuler = x,
                            TweenRotationDeltaEuler,
                            dur
                        )
                        .SetEase(ease)
                );
            if (scaleAlmostOne)
                outPhase.Join(
                    DOTween
                        .To(
                            () => _motionScaleMul,
                            x => _motionScaleMul = x,
                            TweenScaleMultiplier,
                            dur
                        )
                        .SetEase(ease)
                );
            seq.Append(outPhase);

            if (TweenReturnToBaseline)
            {
                var back = CreateManualMotionSequence();
                if (hasPos)
                    back.Join(
                        DOTween
                            .To(() => _motionPosDelta, x => _motionPosDelta = x, Vector3.zero, retDur)
                            .SetEase(retEase)
                    );
                if (hasRot)
                    back.Join(
                        DOTween
                            .To(
                                () => _motionRotDeltaEuler,
                                x => _motionRotDeltaEuler = x,
                                Vector3.zero,
                                retDur
                            )
                            .SetEase(retEase)
                    );
                if (scaleAlmostOne)
                    back.Join(
                        DOTween
                            .To(() => _motionScaleMul, x => _motionScaleMul = x, Vector3.one, retDur)
                            .SetEase(retEase)
                    );
                seq.Append(back);
            }
        }

        private void PlayTweenOffset()
        {
            if (!TryGetTweenOffsetChannels(out var hasPos, out var hasRot, out var scaleAlmostOne))
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            _manualMotionActive = true;
            var seq = CreateManualMotionSequence();
            var repeatCount = GetOneShotRepeatCount();
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                AppendTweenOffsetCycle(seq, hasPos, hasRot, scaleAlmostOne);
            }

            seq.OnKill(() => _sequence = null);
            seq.OnComplete(() =>
            {
                _sequence = null;
                if (TweenReturnToBaseline)
                {
                    _manualMotionActive = false;
                    ResetMotionDeltas();
                }
                InvokeFlow(nameof(OnMotionComplete));
            });
            _sequence = seq;
        }

        private bool AppendPunchCycle(Sequence seq)
        {
            var dur = ScaleOneShotDuration(PunchDuration, 0.05f);
            var v = Mathf.Max(1, PunchVibrato);
            var e = Mathf.Clamp01(PunchElasticity);
            var strength = PunchStrength;
            var rotStrength = PunchRotationStrength;

            if (PunchChannel == GameObjectPunchChannel.Rotation)
                rotStrength += strength;

            var hasPrimary =
                PunchChannel != GameObjectPunchChannel.Rotation
                && strength.sqrMagnitude > 1e-10f;
            var hasRotation = rotStrength.sqrMagnitude > 1e-10f;

            if (!hasPrimary && !hasRotation)
                return false;

            var cycle = CreateManualMotionSequence();

            if (hasPrimary)
            {
                switch (PunchChannel)
                {
                    case GameObjectPunchChannel.Scale:
                        cycle.Join(
                            ConfigureManualMotionTween(
                                DOTween.Punch(
                                    () => _motionScaleMul,
                                    x => _motionScaleMul = x,
                                    strength,
                                    dur,
                                    v,
                                    e
                                )
                            )
                        );
                        break;
                    default:
                        cycle.Join(
                            ConfigureManualMotionTween(
                                DOTween.Punch(
                                    () => _motionPosDelta,
                                    x => _motionPosDelta = x,
                                    strength,
                                    dur,
                                    v,
                                    e
                                )
                            )
                        );
                        break;
                }
            }

            if (hasRotation)
            {
                cycle.Join(
                    ConfigureManualMotionTween(
                        DOTween.Punch(
                            () => _motionRotDeltaEuler,
                            x => _motionRotDeltaEuler = x,
                            rotStrength,
                            dur,
                            v,
                            e
                        )
                    )
                );
            }

            seq.Append(cycle);
            return true;
        }

        private void PlayPunch()
        {
            var seq = CreateManualMotionSequence();
            var repeatCount = GetOneShotRepeatCount();
            var any = false;
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                if (AppendPunchCycle(seq))
                    any = true;
            }

            if (!any)
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            _manualMotionActive = true;
            BindManualMotionSequence(seq);
        }

        private void AppendSpinCycle(Sequence seq)
        {
            var dur = ScaleOneShotDuration(SpinDuration, 0.05f);
            seq.Append(
                ConfigureManualMotionTween(
                    DOTween
                        .To(
                            () => _motionRotDeltaEuler,
                            x => _motionRotDeltaEuler = x,
                            SpinEulerDelta,
                            dur
                        )
                        .SetEase(SpinEase)
                )
            );
            seq.AppendCallback(() => _motionRotDeltaEuler = Vector3.zero);
        }

        private void PlaySpinOnce()
        {
            if (SpinEulerDelta.sqrMagnitude < 1e-10f)
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            _manualMotionActive = true;
            var seq = CreateManualMotionSequence();
            var repeatCount = GetOneShotRepeatCount();
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                AppendSpinCycle(seq);
            }

            BindManualMotionSequence(seq);
        }

        private bool AppendBounceCycle(Sequence seq)
        {
            if (BouncePositionDelta.sqrMagnitude < 1e-10f)
                return false;

            var dur = ScaleOneShotDuration(BounceDuration, 0.05f);
            var retDur = ScaleOneShotDuration(BounceReturnDuration, 0.02f);

            seq.Append(
                DOTween
                    .To(() => _motionPosDelta, x => _motionPosDelta = x, BouncePositionDelta, dur)
                    .SetEase(BounceEase)
            );

            if (BounceReturnToBaseline)
            {
                seq.Append(
                    DOTween
                        .To(() => _motionPosDelta, x => _motionPosDelta = x, Vector3.zero, retDur)
                        .SetEase(BounceReturnEase)
                );
            }

            return true;
        }

        private void PlayBounceOnce()
        {
            var seq = CreateManualMotionSequence();
            var repeatCount = GetOneShotRepeatCount();
            var any = false;
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                if (AppendBounceCycle(seq))
                    any = true;
            }

            if (!any)
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            _manualMotionActive = true;
            BindManualMotionSequence(seq, resetDeltasOnComplete: BounceReturnToBaseline);
        }

        private bool TryBeginPopScaleMotion(out Sequence seq)
        {
            seq = null;
            var peak = PopScalePeak;
            var almostOne =
                Mathf.Abs(peak.x - 1f) > 1e-4f
                || Mathf.Abs(peak.y - 1f) > 1e-4f
                || Mathf.Abs(peak.z - 1f) > 1e-4f;
            if (!almostOne)
                return false;

            _manualMotionActive = true;
            seq = CreateManualMotionSequence();
            return true;
        }

        private void AppendPopScaleCycle(Sequence seq)
        {
            var peak = PopScalePeak;
            var upDur = ScaleOneShotDuration(PopScaleUpDuration, 0.01f);
            var downDur = ScaleOneShotDuration(PopScaleDownDuration, 0.01f);
            seq.Append(
                DOTween
                    .To(() => _motionScaleMul, x => _motionScaleMul = x, peak, upDur)
                    .SetEase(PopScaleUpEase)
            );
            seq.Append(
                DOTween
                    .To(() => _motionScaleMul, x => _motionScaleMul = x, Vector3.one, downDur)
                    .SetEase(PopScaleDownEase)
            );
        }

        private void PlayPopScaleOnce()
        {
            if (!TryBeginPopScaleMotion(out var seq))
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            var repeatCount = GetOneShotRepeatCount();
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                AppendPopScaleCycle(seq);
            }
            BindManualMotionSequence(seq);
        }

        private bool AppendKnockbackCycle(Sequence seq)
        {
            if (KnockbackPositionDelta.sqrMagnitude < 1e-10f)
                return false;

            var dur = ScaleOneShotDuration(KnockbackDuration, 0.05f);
            var retDur = ScaleOneShotDuration(KnockbackReturnDuration, 0.02f);

            seq.Append(
                DOTween
                    .To(
                        () => _motionPosDelta,
                        x => _motionPosDelta = x,
                        KnockbackPositionDelta,
                        dur
                    )
                    .SetEase(KnockbackEase)
            );

            if (KnockbackReturnToBaseline)
            {
                seq.Append(
                    DOTween
                        .To(() => _motionPosDelta, x => _motionPosDelta = x, Vector3.zero, retDur)
                        .SetEase(KnockbackReturnEase)
                );
            }

            return true;
        }

        private void PlayKnockbackOnce()
        {
            var seq = CreateManualMotionSequence();
            var repeatCount = GetOneShotRepeatCount();
            var any = false;
            for (var i = 0; i < repeatCount; i++)
            {
                AppendOneShotRepeatGap(seq, i);
                if (AppendKnockbackCycle(seq))
                    any = true;
            }

            if (!any)
            {
                InvokeFlow(nameof(OnMotionComplete));
                return;
            }

            _manualMotionActive = true;
            BindManualMotionSequence(seq, resetDeltasOnComplete: KnockbackReturnToBaseline);
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            var tr = GetActiveTransform();
            if (tr == null)
                return;

            if (!_hasBaseline)
                return;

            var now = GetNow();

            if (
                _oscillateActive
                && !_oscRampingDown
                && _oscAutoStopDeadline > 0f
                && now >= _oscAutoStopDeadline
            )
            {
                _oscAutoStopDeadline = -1f;
                if (StopBlendOutDuration <= 1e-6f)
                    FinishOscillateStopRamp();
                else
                {
                    _oscRampingDown = true;
                    _stopRampStartTime = now;
                    CaptureOscillateEnvelopesForStop();
                }
            }

            if (_oscRampingDown)
            {
                if (StopBlendOutDuration <= 1e-6f)
                {
                    FinishOscillateStopRamp();
                    return;
                }

                var p = Mathf.Clamp01((GetNow() - _stopRampStartTime) / StopBlendOutDuration);
                var env = CurrentOscillateEnvelopesDuringStopRamp();
                if (p >= 1f)
                {
                    FinishOscillateStopRamp();
                    return;
                }

                ApplyOscillateFrame(tr, env);
                return;
            }

            if (_oscillateActive)
                ApplyOscillateFrame(tr, SampleOscillateEnvelopes());
        }

        public override void OnPostLateUpdate()
        {
            base.OnPostLateUpdate();

            if (!_manualMotionActive || !_hasBaseline)
                return;

            var tr = GetActiveTransform();
            if (tr != null)
                ApplyManualMotionFrame(tr);
        }

        private void ApplyOscillateFrame(Transform tr, OscillateMotionEnvelopes envelopes)
        {
            ApplyOscillateFrame(tr, envelopes.Center, envelopes.Vibration);
        }

        private void ApplyOscillateFrame(Transform tr, float centerEnvelope, float vibrationEnvelope)
        {
            var freq = Mathf.Max(0.01f, OscillatorFrequency);
            var t = GetNow() + OscillatorTimeOffset;

            var ox = EvalOscillator(t * 0.93f, freq, OscillatorShape);
            var oy = EvalOscillator(t * 1.0f, freq, OscillatorShape);
            var oz = EvalOscillator(t * 1.07f, freq, OscillatorShape);

            var amp = OscillatePositionAmplitude;
            var posOff = new Vector3(amp.x * ox, amp.y * oy, amp.z * oz) * vibrationEnvelope;
            var centerLift = OscillateCenterOffset * centerEnvelope;

            var rAmp = OscillateRotationAmplitude;
            var rx = rAmp.x * EvalOscillator(t * 1.0f, freq, OscillatorShape) * vibrationEnvelope;
            var ry = rAmp.y * EvalOscillator(t * 0.88f, freq, OscillatorShape) * vibrationEnvelope;
            var rz = rAmp.z * EvalOscillator(t * 1.12f, freq, OscillatorShape) * vibrationEnvelope;
            var rotOff = Quaternion.Euler(rx, ry, rz);

            Vector3 scale;
            if (OscillateScalePulse > 1e-6f)
            {
                var sp = EvalOscillator(
                    t * Mathf.Max(0.05f, OscillateScaleSpeedFactor),
                    freq,
                    OscillatorShape
                );
                var mul = 1f + OscillateScalePulse * sp * vibrationEnvelope;
                scale = Vector3.Scale(_baseLocalScale, new Vector3(mul, mul, mul));
            }
            else
                scale = _baseLocalScale;

            if (Space == GameObjectEasingSpace.Local)
            {
                tr.localPosition = _baseLocalPos + centerLift + posOff;
                tr.localRotation = _baseLocalRot * rotOff;
                tr.localScale = scale;
            }
            else
            {
                tr.position = _baseWorldPos + centerLift + posOff;
                tr.rotation = _baseWorldRot * rotOff;
                tr.localScale = scale;
            }
        }

        protected override void OnDestroy()
        {
            StopAndRestoreHard();
            base.OnDestroy();
        }

        [FlowOutput]
        public Continuation Exit;

        [FlowOutput]
        [Label("모션 한 사이클 끝 (단발 효과)")]
        public Continuation OnMotionComplete;

        [FlowOutput]
        [Label("정지·복구 완료 (램프 끝)")]
        public Continuation OnStopComplete;
    }
}
