using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Core.Serializations;

namespace Node68.CustomNodes
{
    /// <summary>
    /// Warudo 「스트리머 후원 메시지 큐」와 동일 동작.
    /// 유휴 상태에서 첫 Enter 는 즉시 처리(Exit), 빠른 항목은 큐에 넣지 않고 즉시 Exit.
    /// </summary>
    [NodeType(
        Id = "d7f0b8a2-6d3f-4f4c-9b7e-1a2b3c4d5e01",
        Title = "Donation Queue Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.Share
            : Node68NodeCategories.Toolkit,
        Width = 1.35f
    )]
    public sealed class DonationQueueNode68 : Node
    {
        private sealed class QueueItem
        {
            public int Amount;
            public string NickName;
            public string Message;
            public float FlowInterval;
        }

        [DataInput]
        [Label("후원량")]
        public int Amount;

        [DataInput]
        [Label("닉네임")]
        public string NickName;

        [DataInput]
        [Label("메시지")]
        public string Msg;

        [DataInput]
        [Label("큐 출력 간격 (단위: 초)")]
        [FloatSlider(0f, 60f)]
        public float QueueFlowInterval;

        [DataInput]
        [Label("빠른 큐 기준 (단위: 초)")]
        [Description("이 값 이하의 큐 항목은 긴 대기열에 막히지 않고 먼저 처리합니다.")]
        [FloatSlider(0f, 60f)]
        public float FastQueueIntervalThreshold = 1f;

        [DataInput]
        [Label("Info")]
        [Markdown(primary: true)]
        public string Info =
            "### 큐 상태: <span style='color:green'>작동 중</span> <br> 큐 개수: 0";

        [DataInput]
        [Label("큐 리스트")]
        [Markdown]
        public string QueueList;

        private readonly List<QueueItem> _queue = new();
        private QueueItem _current;
        private CancellationTokenSource _processorCts;
        private bool _paused;
        private int _displayRevision = -1;

        protected override void OnCreate()
        {
            base.OnCreate();
            ApplyDisplayName();
            RefreshDisplay();
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            ApplyDisplayName();
            RefreshDisplay();
        }

        protected override void OnDestroy()
        {
            StopProcessor();
            base.OnDestroy();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var revision = ComputeDisplayRevision();
            if (revision == _displayRevision)
                return;

            _displayRevision = revision;
            RefreshDisplay();
        }

        private int ComputeDisplayRevision()
        {
            unchecked
            {
                var hash = _queue.Count;
                hash = (hash * 31) + (_current != null ? 1 : 0);
                hash = (hash * 31) + (_paused ? 1 : 0);
                hash = (hash * 31) + Mathf.RoundToInt(QueueFlowInterval * 100f);
                hash = (hash * 31) + Mathf.RoundToInt(GetCurrentFlowIntervalSeconds() * 100f);
                return hash;
            }
        }

        private void ApplyDisplayName()
        {
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(this, "Donation Queue Node68");
        }

        [FlowInput]
        [Label("큐 입력")]
        public Continuation Enter()
        {
            EnqueueInternal();
            return null;
        }

        [FlowInput]
        [Label("큐 초기화")]
        public Continuation Clear()
        {
            StopProcessor();
            _queue.Clear();
            _current = null;
            _paused = false;
            RefreshDisplay();
            return null;
        }

        [FlowInput]
        [Label("다음 큐 즉시 시작")]
        public Continuation DequeueNextImmediately()
        {
            if (_paused || _queue.Count == 0)
                return null;

            StopProcessor();
            DequeueOne();
            EnsureProcessorRunning();
            return null;
        }

        [FlowInput]
        [Label("일시정지 / 재개")]
        public Continuation TogglePause()
        {
            _paused = !_paused;

            if (_paused)
                StopProcessor();
            else if (_current == null && _queue.Count > 0)
            {
                DequeueOne();
                EnsureProcessorRunning();
            }
            else if (_current != null || _queue.Count > 0)
            {
                EnsureProcessorRunning();
            }

            RefreshDisplay();
            return null;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("후원량")]
        public int OutputAmount() => _current?.Amount ?? 0;

        [DataOutput]
        [Label("닉네임")]
        public string OutputNickName() => _current?.NickName ?? "";

        [DataOutput]
        [Label("메시지")]
        public string OutputMsg() => _current?.Message ?? "";

        [DataOutput]
        [Label("큐 개수")]
        public int QueueCount() => _queue.Count;

        [DataOutput]
        [Label("큐 리스트")]
        public string QueueListOutput() => BuildQueueListText();

        private void EnqueueInternal()
        {
            var item = new QueueItem
            {
                Amount = Amount,
                NickName = NickName ?? "",
                Message = Msg ?? "",
                FlowInterval = GetFlowIntervalSeconds(),
            };

            if (IsFastQueueItem(item))
            {
                InvokeImmediate(item);
                RefreshDisplay();
                return;
            }

            var wasIdle = !_paused && _current == null && _queue.Count == 0 && _processorCts == null;

            _queue.Add(item);

            if (!_paused)
            {
                // 내장 큐: 유휴 시 첫 건은 간격 없이 바로 처리 상태로 빼고 Exit.
                if (wasIdle)
                    DequeueOne();

                if (_current != null || _queue.Count > 0)
                    EnsureProcessorRunning();
            }

            RefreshDisplay();
        }

        private void InvokeImmediate(QueueItem item)
        {
            var previous = _current;
            _current = item;

            RefreshDisplay();
            InvokeFlow(nameof(Exit));

            _current = previous;
        }

        private void EnsureProcessorRunning()
        {
            if (_processorCts != null)
                return;

            _processorCts = new CancellationTokenSource();
            RunQueueProcessorAsync(_processorCts.Token).Forget();
        }

        private void StopProcessor()
        {
            _processorCts?.Cancel();
            _processorCts?.Dispose();
            _processorCts = null;
        }

        private async UniTaskVoid RunQueueProcessorAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (_paused)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, token);
                        continue;
                    }

                    if (_current == null && _queue.Count == 0)
                        return;

                    // 빠른 항목은 입력 시 즉시 Exit 되므로, 큐 안에는 느린 항목만 남는다.
                    var delaySeconds = GetCurrentFlowIntervalSeconds();
                    if (delaySeconds > 0f)
                        await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken: token);
                    else
                        await UniTask.Yield(PlayerLoopTiming.Update, token);

                    if (token.IsCancellationRequested || _paused)
                        continue;

                    if (_queue.Count == 0)
                    {
                        _current = null;
                        RefreshDisplay();
                        return;
                    }

                    DequeueOne();
                }
            }
            catch (OperationCanceledException)
            {
                // processor stopped
            }
            finally
            {
                if (_processorCts?.Token == token)
                {
                    _processorCts?.Dispose();
                    _processorCts = null;
                }
            }
        }

        private float GetFlowIntervalSeconds() => Mathf.Max(0f, QueueFlowInterval);

        private float GetCurrentFlowIntervalSeconds() => _current?.FlowInterval ?? GetFlowIntervalSeconds();

        private void DequeueOne()
        {
            if (_queue.Count == 0)
                return;

            _current = _queue[0];
            _queue.RemoveAt(0);

            RefreshDisplay();
            InvokeFlow(nameof(Exit));
        }

        private bool IsFastQueueItem(QueueItem item) =>
            Mathf.Max(0f, item.FlowInterval) <= Mathf.Max(0f, FastQueueIntervalThreshold);

        private static string FormatQueueValue(QueueItem item) =>
            $"{item.Amount} / {FormatDisplayText(item.NickName)} / {FormatDisplayText(item.Message)}";

        private static string FormatDisplayText(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return WebUtility
                .HtmlEncode(value)
                .Replace("\r\n", " ")
                .Replace('\r', ' ')
                .Replace('\n', ' ');
        }

        private static string FormatQueueFlowIntervalSummary(float seconds) =>
            seconds <= 0f
                ? "큐 출력 간격: 0초 (2번째 건부터 즉시)"
                : $"큐 출력 간격: {seconds:0.##}초 (2번째 건부터)";

        private string BuildQueueListText()
        {
            var lines = new List<string>(_queue.Count + (_current != null ? 2 : 1))
            {
                "[현재] " + FormatQueueFlowIntervalSummary(GetCurrentFlowIntervalSeconds()),
            };

            if (_current != null)
                lines.Add("[처리 중] " + FormatQueueValue(_current));

            for (var i = 0; i < _queue.Count; i++)
                lines.Add($"{i + 1}. {FormatQueueValue(_queue[i])}");

            if (_current == null && _queue.Count == 0)
                lines.Add("(대기 항목 없음)");

            return string.Join("\n", lines);
        }

        private void RefreshDisplay()
        {
            var statusText = _paused ? "일시정지" : "작동 중";
            var statusColor = _paused ? "orange" : "green";
            var processingLine =
                _current != null
                    ? $"<br> 처리 중: {FormatQueueValue(_current)}"
                    : "";

            Info =
                $"### 큐 상태: <span style='color:{statusColor}'>{statusText}</span> <br> 큐 개수(대기): {_queue.Count} <br> {FormatQueueFlowIntervalSummary(GetCurrentFlowIntervalSeconds())}{processingLine}";

            SetDataInput(nameof(Info), Info, broadcast: true);

            QueueList = BuildQueueListText();
            SetDataInput(nameof(QueueList), QueueList, broadcast: true);

            _displayRevision = ComputeDisplayRevision();
            Broadcast();
        }
    }
}
