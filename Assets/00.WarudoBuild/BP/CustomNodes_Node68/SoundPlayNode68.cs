using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    /// <summary>
    /// Warudo 내장 PLAY_SOUND 대체. 매 재생마다 새 재생 경로를 사용해
    /// Source 참조가 걸리는 알려진 버그를 우회합니다.
    /// </summary>
    [NodeType(
        Id = "f8a2b3c4-5d6e-4f7a-8b9c-0d1e2f3a4b5c",
        Title = "Sound Play Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.25f
    )]
    public sealed class SoundPlayNode68 : Node
    {
        [DataInput]
        [Label("Source")]
        [Description("sound:// 또는 audioclip:// URI")]
        [AutoCompleteResource("Sound")]
        public string Source;

        [DataInput]
        [Label("Delay")]
        [Description("재생 전 대기 시간(초). 플로우는 즉시 다음 노드로 진행합니다.")]
        [FloatSlider(0f, 120f)]
        public float Delay;

        [DataInput]
        [Label("Volume")]
        [FloatSlider(0f, 1f)]
        public float Volume = 1f;

        [DataInput]
        [Label("Trim")]
        public bool Trim;

        [DataInput]
        [Label("Trim Start")]
        [HiddenIf(nameof(HideTrimFields))]
        [FloatSlider(0f, 600f)]
        public float TrimStart = 1f;

        [DataInput]
        [Label("Trim End")]
        [HiddenIf(nameof(HideTrimFields))]
        [FloatSlider(0f, 600f)]
        public float TrimEnd = 2f;

        [DataInput]
        [Label("Fade In")]
        public bool FadeIn;

        [DataInput]
        [Label("Fade In Duration")]
        [HiddenIf(nameof(HideFadeInFields))]
        [FloatSlider(0f, 30f)]
        public float FadeInDuration = 0.5f;

        [DataInput]
        [Label("Fade Out")]
        public bool FadeOut;

        [DataInput]
        [Label("Fade Out Duration")]
        [HiddenIf(nameof(HideFadeOutFields))]
        [FloatSlider(0f, 30f)]
        public float FadeOutDuration = 0.5f;

        private int _playbackGeneration;
        private CancellationTokenSource _playbackCts;

        private const string ShareDisplayNameSuffix = " Shr";

        private bool HideTrimFields() =>
            !Node68BuildRuntime.IsShareBuild() && !Trim;

        private bool HideFadeInFields() =>
            !Node68BuildRuntime.IsShareBuild() && !FadeIn;

        private bool HideFadeOutFields() =>
            !Node68BuildRuntime.IsShareBuild() && !FadeOut;

        private bool UsesAdvancedPlayback() => Trim || FadeIn || FadeOut;

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
            CancelPlayback();
            base.OnDestroy();
        }

        private void ApplyShareSuffixToDisplayName()
        {
            var baseName = Node68ShareNodeTypeTitles.BaseTitleForGraphDisplay(
                GetTypeMeta().NodeType.title
            );
            if (string.IsNullOrEmpty(baseName))
                baseName = "Sound Play Node68";

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

        [FlowInput]
        public Continuation Enter()
        {
            CancelPlayback();
            var generation = ++_playbackGeneration;
            _playbackCts = new CancellationTokenSource();
            RunPlaybackAsync(generation, _playbackCts.Token).Forget();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        private void CancelPlayback()
        {
            if (_playbackCts == null)
                return;

            _playbackCts.Cancel();
            _playbackCts.Dispose();
            _playbackCts = null;
        }

        private async UniTaskVoid RunPlaybackAsync(int generation, CancellationToken token)
        {
            try
            {
                if (Delay > 1e-6f)
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(Delay),
                        ignoreTimeScale: true,
                        cancellationToken: token
                    );
                }

                if (generation != _playbackGeneration)
                    return;

                var clip = ResolveAudioClip(Source);
                if (clip == null)
                {
                    Debug.LogWarning(
                        "[Sound Play Node68] AudioClip 해석 실패. Source="
                            + (string.IsNullOrWhiteSpace(Source) ? "(비어 있음)" : Source)
                    );
                    return;
                }

                var volume = Mathf.Clamp01(Volume);
                if (!UsesAdvancedPlayback())
                {
                    PlayClipAtPoint(clip, volume);
                    return;
                }

                await PlayClipAdvancedAsync(clip, volume, token);
            }
            catch (OperationCanceledException)
            {
                // 노드 재진입·삭제 시 정상 취소
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Sound Play Node68] 재생 오류: " + ex.Message);
            }
        }

        private static void PlayClipAtPoint(AudioClip clip, float volume)
        {
            AudioSource.PlayClipAtPoint(clip, GetPlayPosition(), volume);
        }

        private async UniTask PlayClipAdvancedAsync(
            AudioClip clip,
            float targetVolume,
            CancellationToken token
        )
        {
            var listener = FindAudioListenerTransform();
            var go = new GameObject("Node68_SoundPlay");
            if (listener != null)
                go.transform.SetParent(listener, false);

            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.bypassListenerEffects = false;
            source.bypassEffects = false;

            var startTime = Trim ? Mathf.Max(0f, TrimStart) : 0f;
            var endTime = Trim ? TrimEnd : GetClipLength(clip);
            if (endTime <= startTime)
                endTime = GetClipLength(clip);
            endTime = Mathf.Min(endTime, GetClipLength(clip));

            var playDuration = Mathf.Max(0f, endTime - startTime);
            if (playDuration <= 1e-6f)
            {
                UnityEngine.Object.Destroy(go);
                Debug.LogWarning(
                    "[Sound Play Node68] 재생 구간 길이가 0입니다. clip="
                        + clip.name
                        + ", length="
                        + clip.length
                );
                return;
            }

            var fadeInDur = FadeIn ? Mathf.Max(0f, FadeInDuration) : 0f;
            var fadeOutDur = FadeOut ? Mathf.Max(0f, FadeOutDuration) : 0f;
            source.volume = fadeInDur > 1e-6f ? 0f : targetVolume;

            source.Play();
            if (startTime > 1e-6f)
                source.time = startTime;

            if (!source.isPlaying)
            {
                Debug.LogWarning(
                    "[Sound Play Node68] AudioSource.Play() 실패. clip=" + clip.name
                );
                UnityEngine.Object.Destroy(go);
                return;
            }

            var sustainEnd = playDuration - fadeOutDur;
            if (sustainEnd < fadeInDur)
                sustainEnd = fadeInDur;

            var elapsed = 0f;
            while (elapsed < playDuration)
            {
                token.ThrowIfCancellationRequested();

                if (!source.isPlaying && elapsed > 0.05f)
                    break;

                elapsed += Time.unscaledDeltaTime;

                if (fadeInDur > 1e-6f && elapsed < fadeInDur)
                    source.volume = targetVolume * (elapsed / fadeInDur);
                else if (FadeOut && fadeOutDur > 1e-6f && elapsed > sustainEnd)
                {
                    var t = (elapsed - sustainEnd) / fadeOutDur;
                    source.volume = targetVolume * (1f - Mathf.Clamp01(t));
                }
                else
                    source.volume = targetVolume;

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            source.Stop();
            UnityEngine.Object.Destroy(go);
        }

        private static float GetClipLength(AudioClip clip)
        {
            if (clip == null)
                return 0f;
            if (clip.length > 1e-6f)
                return clip.length;
            if (clip.samples > 0 && clip.frequency > 0)
                return clip.samples / (float)clip.frequency;
            return 0f;
        }

        private static Vector3 GetPlayPosition()
        {
            var listener = FindAudioListenerTransform();
            return listener != null ? listener.position : Vector3.zero;
        }

        private static Transform FindAudioListenerTransform()
        {
            var listener = UnityEngine.Object.FindObjectOfType<AudioListener>();
            return listener != null ? listener.transform : null;
        }

        private static AudioClip ResolveAudioClip(string source)
        {
            if (string.IsNullOrWhiteSpace(source) || Context.ResourceManager == null)
                return null;

            var trimmed = source.Trim();
            try
            {
                var obj = Context.ResourceManager.ResolveResourceUri(trimmed);
                var clip = ExtractAudioClip(obj);
                if (clip != null)
                    return clip;

                Debug.LogWarning(
                    "[Sound Play Node68] AudioClip 으로 변환 실패. Source="
                        + trimmed
                        + ", resolvedType="
                        + (obj?.GetType().FullName ?? "null")
                );
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[Sound Play Node68] 리소스 해석 실패: " + trimmed + " — " + ex.Message
                );
            }

            return null;
        }

        private static AudioClip ExtractAudioClip(object obj)
        {
            switch (obj)
            {
                case AudioClip clip:
                    return clip;
                case UnityEngine.Object unityObj when unityObj is AudioClip audioClip:
                    return audioClip;
                default:
                    return null;
            }
        }
    }
}
