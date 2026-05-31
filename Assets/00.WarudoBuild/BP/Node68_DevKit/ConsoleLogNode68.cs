using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;

namespace Node68.ToolkitMods.Node68DevKit
{
    [NodeType(
        Id = "9fd3d6d8-1b63-4b36-9f18-97060bc79f3e",
        Title = "Console Log Node68",
        Category = BpToolkitCategories.Toolkit,
        Width = 3.6f
    )]
    public sealed class ConsoleLogNode68 : Node
    {
        [DataInput]
        [Label("태그")]
        public string Tag = "Node68";

        [DataInput]
        [Label("값")]
        [MultilineInput]
        public string Value = "";

        [DataInput]
        [Label("레벨")]
        public ConsoleLogLevel68 Level = ConsoleLogLevel68.Log;

        [DataInput]
        [Label("JSON 모드")]
        [Description("Auto는 값이 JSON일 때만 자동으로 들여쓰기합니다.")]
        public ConsoleLogJsonMode68 JsonMode = ConsoleLogJsonMode68.Auto;

        [DataInput]
        [Label("JSON 들여쓰기")]
        public bool PrettyPrintJson = true;

        [DataInput]
        [Label("localhost 전송")]
        public bool SendToLocalhost = true;

        [DataInput]
        [Label("전송 주소")]
        public string Endpoint = "http://127.0.0.1:8765/api/ingest";

        [DataInput]
        [Label("Unity Console에도 출력")]
        public bool AlsoUnityConsole;

        [Trigger]
        [Label("JSON 정리")]
        public void FormatJsonValue()
        {
            if (!TryPrepareMessage(Value ?? "", out var formatted, out var isJson, out var error))
            {
                _lastStatus = "JSON 정리 실패: " + error;
                _lastIsJson = false;
                Broadcast();
                return;
            }

            if (!isJson)
            {
                _lastStatus = "JSON 아님";
                _lastIsJson = false;
                Broadcast();
                return;
            }

            Value = formatted;
            _lastMessage = formatted;
            _lastIsJson = true;
            _lastStatus = "JSON 정리 완료";
            SetDataInput(nameof(Value), Value, broadcast: true);
            Broadcast();
        }

        [FlowInput]
        public Continuation Enter()
        {
            if (!TryPrepareMessage(Value ?? "", out var message, out var isJson, out var error))
            {
                message = Value ?? "";
                _lastStatus = "JSON 파싱 실패: " + error;
            }

            if (AlsoUnityConsole)
                WriteUnityLog(message);

            if (SendToLocalhost)
                SendAsync(message).Forget();

            _lastMessage = message;
            _lastIsJson = isJson;
            if (string.IsNullOrEmpty(error))
                _lastStatus = SendToLocalhost
                    ? isJson ? "JSON 전송 중" : "전송 중"
                    : "localhost 전송 꺼짐";
            Broadcast();
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("마지막 상태")]
        public string OutputLastStatus() => _lastStatus;

        [DataOutput]
        [Label("마지막 값")]
        public string OutputLastMessage() => _lastMessage;

        [DataOutput]
        [Label("JSON 여부")]
        public bool OutputIsJson() => _lastIsJson;

        private string _lastStatus = "대기 중";
        private string _lastMessage = "";
        private bool _lastIsJson;

        private async UniTaskVoid SendAsync(string message)
        {
            try
            {
                var payload =
                    "{\"level\":\""
                    + EscapeJson(Level.ToString())
                    + "\",\"tag\":\""
                    + EscapeJson(Tag)
                    + "\",\"message\":\""
                    + EscapeJson(message)
                    + "\",\"json\":"
                    + (_lastIsJson ? "true" : "false")
                    + "}";

                var bytes = Encoding.UTF8.GetBytes(payload);
                using var request = new UnityWebRequest(Endpoint, UnityWebRequest.kHttpVerbPOST);
                request.uploadHandler = new UploadHandlerRaw(bytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
                request.timeout = 2;

                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    _lastStatus = "전송 완료";
                }
                else
                {
                    _lastStatus = "전송 실패: " + request.error;
                    Debug.LogWarning("[Console Log Node68] localhost 전송 실패: " + request.error);
                }
            }
            catch (Exception ex)
            {
                _lastStatus = "전송 예외: " + ex.Message;
                Debug.LogWarning("[Console Log Node68] localhost 전송 예외: " + ex.Message);
            }

            Broadcast();
        }

        private bool TryPrepareMessage(
            string input,
            out string message,
            out bool isJson,
            out string error
        )
        {
            message = input ?? "";
            isJson = false;
            error = "";

            if (JsonMode == ConsoleLogJsonMode68.Text)
                return true;

            if (!TryParseJson(message, out var token, out error))
            {
                if (JsonMode == ConsoleLogJsonMode68.Json)
                    return false;

                error = "";
                return true;
            }

            isJson = true;
            message = PrettyPrintJson
                ? token.ToString(Formatting.Indented)
                : token.ToString(Formatting.None);
            return true;
        }

        private static bool TryParseJson(string input, out JToken token, out string error)
        {
            token = null;
            error = "";

            var text = (input ?? "").Trim();
            if (string.IsNullOrEmpty(text))
            {
                error = "값이 비어 있습니다.";
                return false;
            }

            try
            {
                token = JToken.Parse(text);
                if (token.Type == JTokenType.String)
                {
                    var inner = ((string)token).Trim();
                    if (LooksLikeJson(inner))
                        token = JToken.Parse(inner);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static bool LooksLikeJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();
            return (text.StartsWith("{") && text.EndsWith("}"))
                || (text.StartsWith("[") && text.EndsWith("]"));
        }

        private void WriteUnityLog(string message)
        {
            var line = "[" + Tag + "] " + message;
            switch (Level)
            {
                case ConsoleLogLevel68.Warning:
                    Debug.LogWarning(line);
                    break;
                case ConsoleLogLevel68.Error:
                    Debug.LogError(line);
                    break;
                default:
                    Debug.Log(line);
                    break;
            }
        }

        private static string EscapeJson(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var builder = new StringBuilder(value.Length + 16);
            foreach (var c in value)
            {
                switch (c)
                {
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        if (char.IsControl(c))
                            builder.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }
    }

    public enum ConsoleLogLevel68
    {
        [Label("Log")]
        Log,

        [Label("Warning")]
        Warning,

        [Label("Error")]
        Error,
    }

    public enum ConsoleLogJsonMode68
    {
        [Label("Auto")]
        Auto,

        [Label("JSON")]
        Json,

        [Label("Text")]
        Text,
    }
}
