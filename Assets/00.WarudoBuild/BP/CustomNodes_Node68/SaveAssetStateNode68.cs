using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Scenes;
using Warudo.Core.Serializations;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Cinematography;

namespace Node68.CustomNodes
{
    [NodeType(
        Id = "9f88fd1f-3c44-4d08-9c0d-2ebd65aa9f68",
        Title = "Save Asset State Node68",
        Category = Node68FlavorEmbedded.ShareBuild
            ? Node68NodeCategories.ShareState
            : Node68NodeCategories.ToolkitState,
        Width = 1.45f
    )]
    public sealed class SaveAssetStateNode68 : Node
    {
        [DataInput]
        [Label("TargetGraph")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 이 노드가 있는 현재 블루프린트 Graph Variable 을 사용합니다.")]
        [AutoComplete(nameof(AutoCompleteTargetGraph))]
        [HiddenIf(nameof(HideInShareBuild))]
        public string TargetGraph;

        [DataInput]
        [Label("Asset")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "상태를 저장할 대상 Asset 입니다.")]
        public Asset Asset;

        [DataInput]
        [Label("StateVariable")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "상태 JSON 을 저장할 String Graph Variable 이름입니다.")]
        [AutoComplete(nameof(AutoCompleteStringVariableName))]
        [HiddenIf(nameof(HideInShareBuild))]
        public string StateVariable = "AssetState";

        [DataInput]
        [Label("Auto Create StateVariable")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "켜면 저장할 때 String Graph Variable 이 없으면 자동 생성합니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool AutoCreateStateVariable = true;

        [DataInput]
        [Label("FieldsJson")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "State Fields Node68 의 FieldsJson 출력을 연결하면 아래 Entries 대신 사용합니다.")]
        public string FieldsJson = "";

        [DataInput]
        [Label("Entries")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "FieldsJson 을 연결하지 않을 때 직접 사용할 저장 DataPath 목록입니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public StateEntry[] Entries = Array.Empty<StateEntry>();

        [DataInput]
        [Label("SkipMissingPaths")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "켜면 읽을 수 없는 DataPath 는 경고만 남기고 건너뜁니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public bool SkipMissingPaths = true;

        [DataOutput]
        [Label("SavedCount")]
        public int OutputSavedCount() => _lastSavedCount;

        [DataOutput]
        [Label("Result")]
        public string OutputResult() => _lastResult;

        private int _lastSavedCount;
        private string _lastResult = "-";

        private bool HideInShareBuild() => Node68BuildRuntime.IsShareBuild();

        protected override void OnCreate()
        {
            base.OnCreate();
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Save Asset State Node68"
            );
        }

        public override void OnAllNodesDeserialized(SerializedNode serialized)
        {
            base.OnAllNodesDeserialized(serialized);
            MigrateLegacyFields(serialized);
            Node68CameraOrbitHelper.ApplyShareSuffixToDisplayName(
                this,
                "Save Asset State Node68"
            );
        }

        [FlowInput]
        public Continuation Enter()
        {
            SaveCurrentState();
            return OnSaved;
        }

        [FlowInput]
        [Label("Create StateVariable")]
        public Continuation CreateStateVariable()
        {
            CreateStateVariableOnly();
            return OnStateVariableCreated;
        }

        [FlowOutput]
        public Continuation OnSaved;

        [FlowOutput]
        [Label("On StateVariable Created")]
        public Continuation OnStateVariableCreated;

        [Trigger]
        [Label("Create StateVariable Now")]
        [Description(Node68FlavorEmbedded.ShareBuild ? "" : "StateVariable 이름으로 String Graph Variable 을 즉시 생성합니다.")]
        [HiddenIf(nameof(HideInShareBuild))]
        public void CreateStateVariableNow()
        {
            CreateStateVariableOnly();
        }

        private void SaveCurrentState()
        {
            const string title = "Save Asset State";
            _lastSavedCount = 0;
            _lastResult = "-";

            if (Node68GraphVariableHelper.WarnMissingVariable(title, StateVariable))
                return;

            if (AutoCreateStateVariable && !EnsureStateVariable(title))
                return;

            if (Asset == null)
            {
                Debug.LogWarning($"[Node68/State] {title}: Asset 이 비어 있습니다.");
                _lastResult = "Asset is empty.";
                Broadcast();
                return;
            }

            var fieldList = ResolveFieldListData();
            var specs = BuildEntrySpecs(fieldList);
            if (specs.Count == 0)
            {
                Debug.LogWarning($"[Node68/State] {title}: 저장할 Entry 가 없습니다.");
                _lastResult = "No entries.";
                Broadcast();
                return;
            }

            var state = new Node68StateData { Source = "Fields" };

            foreach (var spec in specs)
            {
                if (!TryReadStateValue(Asset, spec.Path, spec.ValueType, out var value))
                {
                    var message = $"DataPath '{spec.Path}' 를 읽을 수 없습니다.";
                    if (!SkipMissingPaths)
                    {
                        Debug.LogWarning($"[Node68/State] {title}: {message}");
                        _lastResult = message;
                        Broadcast();
                        return;
                    }

                    Debug.LogWarning($"[Node68/State] {title}: {message} 건너뜁니다.");
                    continue;
                }

                state.Entries.Add(
                    new Node68StateEntryData
                    {
                        Key = spec.Key,
                        Path = spec.Path,
                        Type = spec.ValueType.ToString(),
                        ValueJson = JsonConvert.SerializeObject(
                            NormalizeStateValue(value, spec.ValueType)
                        ),
                    }
                );
            }

            var json = Node68StateData.ToJson(state);
            if (
                !Node68GraphVariableHelper.TrySetString(
                    this,
                    TargetGraph,
                    StateVariable,
                    json,
                    AutoCreateStateVariable
                )
            )
            {
                Node68GraphVariableHelper.WarnVariableNotFound(title, StateVariable);
                _lastResult = "Failed to write graph variable.";
                Broadcast();
                return;
            }

            _lastSavedCount = state.Entries.Count;
            _lastResult = $"Saved {_lastSavedCount} entries.";
            Broadcast();
        }

        private void CreateStateVariableOnly()
        {
            const string title = "Save Asset State";
            _lastSavedCount = 0;
            _lastResult = "-";

            if (Node68GraphVariableHelper.WarnMissingVariable(title, StateVariable))
            {
                _lastResult = "StateVariable is empty.";
                Broadcast();
                return;
            }

            if (EnsureStateVariable(title))
            {
                _lastResult = $"StateVariable '{StateVariable.Trim()}' is ready.";
                Broadcast();
            }
        }

        private bool EnsureStateVariable(string title)
        {
            var variable = Node68GraphVariableHelper.ResolveVariable(
                this,
                TargetGraph,
                StateVariable,
                GraphVariableType.String,
                true
            );

            if (variable != null)
                return true;

            Node68GraphVariableHelper.WarnVariableNotFound(title, StateVariable);
            _lastResult = "Failed to create state variable.";
            Broadcast();
            return false;
        }

        private static List<Node68StateEntrySpec> BuildEntrySpecs(
            Node68StateFieldListData fieldList
        )
        {
            return Node68StateFieldListHelper.BuildEntrySpecs(fieldList);
        }

        private Node68StateFieldListData ResolveFieldListData()
        {
            if (
                !string.IsNullOrWhiteSpace(FieldsJson)
                && Node68StateFieldListData.TryFromJson(FieldsJson, out var linked)
            )
            {
                NormalizeFieldListData(linked);
                return linked;
            }

            var data = new Node68StateFieldListData();

            foreach (var entry in Entries ?? Array.Empty<StateEntry>())
            {
                if (entry == null)
                    continue;

                data.Entries.Add(
                    new Node68StateEntryConfig
                    {
                        Enabled = entry.Enabled,
                        DataPath = entry.DataPath,
                        ValueType = entry.ValueType,
                        Key = entry.Key,
                    }
                );
            }

            NormalizeFieldListData(data);
            return data;
        }

        private static void NormalizeFieldListData(Node68StateFieldListData data)
        {
            if (data.Entries == null)
                data.Entries = new List<Node68StateEntryConfig>();
        }

        internal static bool TryReadStateValue(
            Asset asset,
            string path,
            Node68StateValueType valueType,
            out object value
        )
        {
            value = null;
            try
            {
                if (TryReadTransformValue(asset, path, out value))
                    return true;

                switch (valueType)
                {
                    case Node68StateValueType.Boolean:
                        value = asset.GetDataInput<bool>(path);
                        return true;
                    case Node68StateValueType.Integer:
                        value = asset.GetDataInput<int>(path);
                        return true;
                    case Node68StateValueType.Float:
                        value = asset.GetDataInput<float>(path);
                        return true;
                    case Node68StateValueType.String:
                        value = asset.GetDataInput<string>(path) ?? string.Empty;
                        return true;
                    case Node68StateValueType.Vector2:
                        value = asset.GetDataInput<Vector2>(path);
                        return true;
                    case Node68StateValueType.Vector3:
                        value = asset.GetDataInput<Vector3>(path);
                        return true;
                    case Node68StateValueType.Color:
                        value = asset.GetDataInput<Color>(path);
                        return true;
                    case Node68StateValueType.EnumAsString:
                        return TryReadEnumAsString(asset, path, out value);
                    case Node68StateValueType.AssetReferenceAsString:
                        return TryReadAssetReferenceAsString(asset, path, out value);
                    case Node68StateValueType.SerializedDataInput:
                        return TryReadSerializedDataInput(asset, path, out value);
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool TryReadTransformValue(Asset asset, string path, out object value)
        {
            value = null;
            if (!(asset is GameObjectAsset gameObjectAsset) || gameObjectAsset.Transform == null)
                return false;

            switch (path)
            {
                case "Transform.Position":
                    value = gameObjectAsset.Transform.Position;
                    return true;
                case "Transform.Rotation":
                    value = gameObjectAsset.Transform.Rotation;
                    return true;
                case "Transform.Scale":
                    value = gameObjectAsset.Transform.Scale;
                    return true;
                default:
                    return false;
            }
        }

        internal static object NormalizeStateValue(
            object value,
            Node68StateValueType valueType
        )
        {
            switch (valueType)
            {
                case Node68StateValueType.Vector2:
                {
                    var v = (Vector2)value;
                    return new StateVector2 { x = v.x, y = v.y };
                }
                case Node68StateValueType.Vector3:
                {
                    var v = (Vector3)value;
                    return new StateVector3 { x = v.x, y = v.y, z = v.z };
                }
                case Node68StateValueType.Color:
                {
                    var c = (Color)value;
                    return new StateColor { r = c.r, g = c.g, b = c.b, a = c.a };
                }
                default:
                    return value;
            }
        }

        private static bool TryReadSerializedDataInput(Asset asset, string path, out object value)
        {
            value = null;
            var port = asset.DataInputPortCollection.CreateSerializedPort(path);
            if (port == null)
                return false;

            value = port.value ?? string.Empty;
            return true;
        }

        private static bool TryReadEnumAsString(Asset asset, string path, out object value)
        {
            value = null;
            try
            {
                if (asset is CameraAsset camera)
                {
                    if (path == nameof(CameraAsset.ControlMode))
                    {
                        value = camera.ControlMode.ToString();
                        return true;
                    }

                    if (path == nameof(CameraAsset.ControllerIndex))
                    {
                        value = camera.ControllerIndex.ToString();
                        return true;
                    }
                }

                value = asset.GetDataInput<string>(path) ?? string.Empty;
                return true;
            }
            catch
            {
                try
                {
                    value = asset.GetDataInput<int>(path).ToString();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static bool TryReadAssetReferenceAsString(Asset asset, string path, out object value)
        {
            value = null;
            try
            {
                if (asset is CameraAsset camera && path == nameof(CameraAsset.FocusCharacter))
                {
                    value = FormatAssetReference(camera.FocusCharacter);
                    return true;
                }

                var referenced = asset.GetDataInput<Asset>(path);
                value = FormatAssetReference(referenced);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string FormatAssetReference(Asset asset)
        {
            return asset == null ? string.Empty : asset.Name + "|" + asset.Id;
        }

        private void MigrateLegacyFields(SerializedNode serialized)
        {
            if (serialized?.dataInputs == null)
                return;

            if (serialized.dataInputs.ContainsKey(nameof(StateVariable)))
                return;

            var legacyState = ReadSerializedString(serialized, "StateVariable");
            if (!string.IsNullOrWhiteSpace(legacyState))
            {
                StateVariable = legacyState.Trim();
                return;
            }

            var legacyPrefix = ReadSerializedString(serialized, "VariablePrefix");
            StateVariable = string.IsNullOrWhiteSpace(legacyPrefix)
                ? "AssetState"
                : legacyPrefix.Trim() + "State";
        }

        private static string ReadSerializedString(SerializedNode serialized, string key)
        {
            if (serialized?.dataInputs == null || !serialized.dataInputs.TryGetValue(key, out var port))
                return null;

            var raw = port.value;
            if (string.IsNullOrWhiteSpace(raw) || raw == "null")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<string>(raw);
            }
            catch
            {
                return raw.Trim().Trim('"');
            }
        }

        private async UniTask<AutoCompleteList> AutoCompleteTargetGraph() =>
            await Node68GraphVariableHelper.AutoCompleteTargetGraph(this);

        private async UniTask<AutoCompleteList> AutoCompleteStringVariableName() =>
            await Node68GraphVariableHelper.AutoCompleteVariableName(
                this,
                TargetGraph,
                GraphVariableType.String
            );

        public sealed class StateEntry
            : StructuredData<SaveAssetStateNode68>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("Enabled")]
            public bool Enabled = true;

            [DataInput]
            [Label("DataPath")]
            [AutoComplete(nameof(AutoCompleteDataPath))]
            public string DataPath = "";

            [DataInput]
            [Label("Type")]
            [Description(Node68FlavorEmbedded.ShareBuild ? "" : "Auto 로 두면 DataPath 선택값에 맞춰 자동 결정합니다.")]
            public Node68StateValueType ValueType = Node68StateValueType.Auto;

            [DataInput]
            [Label("Key")]
            [Description(Node68FlavorEmbedded.ShareBuild ? "" : "비우면 DataPath 를 Key 로 사용합니다.")]
            public string Key = "";

            public string GetHeader()
            {
                var path = string.IsNullOrWhiteSpace(DataPath) ? "(empty)" : DataPath.Trim();
                var label = Node68StateFieldCatalog.GetLabel(path);
                return (Enabled ? "" : "OFF ") + label + " · " + ValueType;
            }

            public UniTask<AutoCompleteList> AutoCompleteDataPath()
            {
                return UniTask.FromResult(Node68StateFieldCatalog.BuildAutoCompleteList());
            }
        }

        private struct StateVector2
        {
            public float x;
            public float y;
        }

        private struct StateVector3
        {
            public float x;
            public float y;
            public float z;
        }

        private struct StateColor
        {
            public float r;
            public float g;
            public float b;
            public float a;
        }
    }
}
