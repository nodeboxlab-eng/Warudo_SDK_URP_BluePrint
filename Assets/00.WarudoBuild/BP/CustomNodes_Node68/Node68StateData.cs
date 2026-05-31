using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Data;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Cinematography;
using Warudo.Plugins.Core.Events;

namespace Node68.CustomNodes
{
    internal sealed class Node68StateData
    {
        public int Version = 1;
        public string Source;
        public List<Node68StateEntryData> Entries = new List<Node68StateEntryData>();

        internal static string ToJson(Node68StateData data) =>
            JsonConvert.SerializeObject(data);

        internal static bool TryFromJson(string json, out Node68StateData data)
        {
            data = null;
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                data = JsonConvert.DeserializeObject<Node68StateData>(json);
                return data != null;
            }
            catch
            {
                return false;
            }
        }
    }

    internal sealed class Node68StateEntryData
    {
        public string Key;
        public string Path;
        public string Type;
        public string ValueJson;
    }

    internal sealed class Node68StateFieldListData
    {
        public int Version = 1;
        public List<Node68StateEntryConfig> Entries = new List<Node68StateEntryConfig>();

        internal static string ToJson(Node68StateFieldListData data) =>
            JsonConvert.SerializeObject(data);

        internal static bool TryFromJson(string json, out Node68StateFieldListData data)
        {
            data = null;
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                data = JsonConvert.DeserializeObject<Node68StateFieldListData>(json);
                return data != null;
            }
            catch
            {
                return false;
            }
        }
    }

    internal sealed class Node68StateEntryConfig
    {
        public bool Enabled = true;
        public string DataPath;
        public Node68StateValueType ValueType = Node68StateValueType.Auto;
        public string Key;
    }

    public enum Node68StateValueType
    {
        Auto = -1,
        Boolean = 0,
        Integer = 1,
        Float = 2,
        String = 3,
        Vector2 = 4,
        Vector3 = 5,
        Color = 6,
        EnumAsString = 7,
        AssetReferenceAsString = 8,
        SerializedDataInput = 9,
    }

    internal readonly struct Node68StateEntrySpec
    {
        public readonly string Path;
        public readonly string Key;
        public readonly Node68StateValueType ValueType;

        public Node68StateEntrySpec(
            string path,
            Node68StateValueType valueType,
            string key = null
        )
        {
            Path = path;
            ValueType = valueType;
            Key = string.IsNullOrWhiteSpace(key) ? path : key;
        }
    }

    internal static class Node68StateFieldListHelper
    {
        internal static List<Node68StateEntrySpec> BuildEntrySpecs(
            Node68StateFieldListData fieldList
        )
        {
            var result = new List<Node68StateEntrySpec>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void Add(Node68StateEntrySpec spec)
            {
                if (string.IsNullOrWhiteSpace(spec.Path))
                    return;

                var path = spec.Path.Trim();
                if (!seen.Add(path))
                    return;

                result.Add(new Node68StateEntrySpec(
                    path,
                    spec.ValueType,
                    string.IsNullOrWhiteSpace(spec.Key) ? path : spec.Key.Trim()
                ));
            }

            foreach (var entry in fieldList.Entries ?? new List<Node68StateEntryConfig>())
            {
                if (entry == null || !entry.Enabled)
                    continue;

                Add(
                    new Node68StateEntrySpec(
                        entry.DataPath,
                        ResolveValueType(entry.DataPath, entry.ValueType),
                        entry.Key
                    )
                );
            }

            return result;
        }

        private static Node68StateValueType ResolveValueType(
            string path,
            Node68StateValueType requestedType
        )
        {
            if (requestedType != Node68StateValueType.Auto)
                return requestedType;

            return Node68StateFieldCatalog.TryGetValueType(path, out var inferred)
                ? inferred
                : Node68StateValueType.String;
        }
    }

    internal sealed class Node68StateApplyOptions
    {
        public bool RestoreEnabled;
        public bool RestoreAssetReferences;
        public bool SkipMissingPaths = true;
        public bool Broadcast = true;
    }

    internal static class Node68StateValueCodec
    {
        internal static bool TryParseValueType(string type, out Node68StateValueType valueType)
        {
            if (Enum.TryParse(type, true, out valueType))
                return true;

            valueType = Node68StateValueType.String;
            return false;
        }

        internal static bool TryDeserializeValue(
            Node68StateEntryData entry,
            out object value,
            out Node68StateValueType valueType
        )
        {
            value = null;
            valueType = Node68StateValueType.String;
            if (entry == null || !TryParseValueType(entry.Type, out valueType))
                return false;

            try
            {
                switch (valueType)
                {
                    case Node68StateValueType.Boolean:
                        value = JsonConvert.DeserializeObject<bool>(entry.ValueJson);
                        return true;
                    case Node68StateValueType.Integer:
                        value = JsonConvert.DeserializeObject<int>(entry.ValueJson);
                        return true;
                    case Node68StateValueType.Float:
                        value = JsonConvert.DeserializeObject<float>(entry.ValueJson);
                        return true;
                    case Node68StateValueType.String:
                    case Node68StateValueType.EnumAsString:
                    case Node68StateValueType.AssetReferenceAsString:
                    case Node68StateValueType.SerializedDataInput:
                        value = JsonConvert.DeserializeObject<string>(entry.ValueJson) ?? string.Empty;
                        return true;
                    case Node68StateValueType.Vector2:
                    {
                        var v = JsonConvert.DeserializeObject<StateVector2>(entry.ValueJson);
                        value = new Vector2(v.x, v.y);
                        return true;
                    }
                    case Node68StateValueType.Vector3:
                    {
                        var v = JsonConvert.DeserializeObject<StateVector3>(entry.ValueJson);
                        value = new Vector3(v.x, v.y, v.z);
                        return true;
                    }
                    case Node68StateValueType.Color:
                    {
                        var c = JsonConvert.DeserializeObject<StateColor>(entry.ValueJson);
                        value = new Color(c.r, c.g, c.b, c.a);
                        return true;
                    }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        internal static bool TryApplyEntry(
            Asset asset,
            Node68StateEntryData entry,
            Node68StateApplyOptions options,
            out string message
        )
        {
            message = null;
            options ??= new Node68StateApplyOptions();

            if (asset == null)
            {
                message = "Asset is empty.";
                return false;
            }

            var path = entry?.Path?.Trim();
            if (string.IsNullOrWhiteSpace(path))
            {
                message = "Path is empty.";
                return false;
            }

            if (!options.RestoreEnabled && string.Equals(path, "Enabled", StringComparison.OrdinalIgnoreCase))
            {
                message = "Enabled skipped.";
                return true;
            }

            if (!TryDeserializeValue(entry, out var value, out var valueType))
            {
                message = $"Value deserialize failed: {path}";
                return false;
            }

            if (valueType == Node68StateValueType.AssetReferenceAsString)
            {
                if (!options.RestoreAssetReferences)
                {
                    message = $"Asset reference skipped: {path}";
                    return true;
                }

                if (!TryResolveAssetReference((string)value, out var referenced))
                {
                    message = $"Asset reference not found: {path}";
                    return false;
                }

                value = referenced;
            }

            try
            {
                ApplyTypedValue(asset, path, valueType, value, options.Broadcast);
                return true;
            }
            catch (Exception ex)
            {
                message = $"Apply failed: {path} ({ex.Message})";
                return false;
            }
        }

        internal static bool TryFindEntry(
            Node68StateData state,
            string keyOrPath,
            out Node68StateEntryData entry
        )
        {
            entry = null;
            if (state?.Entries == null || string.IsNullOrWhiteSpace(keyOrPath))
                return false;

            var trimmed = keyOrPath.Trim();
            entry = state.Entries.FirstOrDefault(e =>
                e != null
                && (
                    string.Equals(e.Key?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(e.Path?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase)
                )
            );
            return entry != null;
        }

        internal static string BuildSummary(Node68StateData state)
        {
            if (state?.Entries == null || state.Entries.Count == 0)
                return "0 entries";

            var paths = state.Entries
                .Where(e => e != null && !string.IsNullOrWhiteSpace(e.Path))
                .Select(e => e.Path.Trim())
                .ToList();
            return $"{paths.Count} entries\n{string.Join(", ", paths)}";
        }

        private static void ApplyTypedValue(
            Asset asset,
            string path,
            Node68StateValueType valueType,
            object value,
            bool broadcast
        )
        {
            if (TryApplyTransformValue(asset, path, value, broadcast))
                return;

            switch (valueType)
            {
                case Node68StateValueType.Boolean:
                    asset.SetDataInput(path, (bool)value, broadcast);
                    break;
                case Node68StateValueType.Integer:
                    asset.SetDataInput(path, (int)value, broadcast);
                    break;
                case Node68StateValueType.Float:
                    asset.SetDataInput(path, (float)value, broadcast);
                    break;
                case Node68StateValueType.String:
                    asset.SetDataInput(path, (string)value, broadcast);
                    break;
                case Node68StateValueType.Vector2:
                    asset.SetDataInput(path, (Vector2)value, broadcast);
                    break;
                case Node68StateValueType.Vector3:
                    asset.SetDataInput(path, (Vector3)value, broadcast);
                    break;
                case Node68StateValueType.Color:
                    asset.SetDataInput(path, (Color)value, broadcast);
                    break;
                case Node68StateValueType.EnumAsString:
                    ApplyEnumAsString(asset, path, (string)value, broadcast);
                    break;
                case Node68StateValueType.AssetReferenceAsString:
                    ApplyAssetReference(asset, path, (Asset)value, broadcast);
                    break;
                case Node68StateValueType.SerializedDataInput:
                    if (!asset.DataInputPortCollection.SetSerializedValueAtPath(
                            path,
                            (string)value,
                            broadcast
                        ))
                        throw new InvalidOperationException("Serialized data input apply failed.");
                    break;
                default:
                    asset.SetDataInput(path, value, broadcast);
                    break;
            }
        }

        private static bool TryApplyTransformValue(
            Asset asset,
            string path,
            object value,
            bool broadcast
        )
        {
            if (!(asset is GameObjectAsset gameObjectAsset) || gameObjectAsset.Transform == null)
                return false;

            if (!(value is Vector3 vector))
                return false;

            switch (path)
            {
                case "Transform.Position":
                    gameObjectAsset.Transform.Position = vector;
                    break;
                case "Transform.Rotation":
                    gameObjectAsset.Transform.Rotation = vector;
                    break;
                case "Transform.Scale":
                    gameObjectAsset.Transform.Scale = vector;
                    break;
                default:
                    return false;
            }

            if (broadcast)
                gameObjectAsset.BroadcastDataInput(nameof(GameObjectAsset.Transform));
            return true;
        }

        private static void ApplyEnumAsString(
            Asset asset,
            string path,
            string value,
            bool broadcast
        )
        {
            if (asset is CameraAsset camera)
            {
                if (
                    path == nameof(CameraAsset.ControlMode)
                    && Enum.TryParse(value, true, out CameraAsset.CameraControlMode controlMode)
                )
                {
                    camera.ControlMode = controlMode;
                    camera.SetDataInput(path, controlMode, broadcast);
                    return;
                }

                if (
                    path == nameof(CameraAsset.ControllerIndex)
                    && Enum.TryParse(value, true, out ControllerIndex controllerIndex)
                )
                {
                    camera.ControllerIndex = controllerIndex;
                    camera.SetDataInput(path, controllerIndex, broadcast);
                    return;
                }
            }

            asset.SetDataInput(path, value ?? string.Empty, broadcast);
        }

        private static void ApplyAssetReference(
            Asset asset,
            string path,
            Asset referenced,
            bool broadcast
        )
        {
            if (asset is CameraAsset camera && path == nameof(CameraAsset.FocusCharacter))
            {
                camera.FocusCharacter = referenced as CharacterAsset;
                camera.SetDataInput(path, camera.FocusCharacter, broadcast);
                return;
            }

            asset.SetDataInput(path, referenced, broadcast);
        }

        private static bool TryResolveAssetReference(string formatted, out Asset asset)
        {
            asset = null;
            if (string.IsNullOrWhiteSpace(formatted))
                return true;

            var idText = formatted;
            var separator = formatted.LastIndexOf('|');
            if (separator >= 0 && separator + 1 < formatted.Length)
                idText = formatted[(separator + 1)..];

            var scene = Context.OpenedScene;
            if (scene != null && Guid.TryParse(idText.Trim(), out var id))
            {
                asset = scene.GetAsset(id);
                return asset != null;
            }

            if (scene == null)
                return false;

            asset = scene
                .GetAssetList()
                .FirstOrDefault(a => a != null && string.Equals(a.Name, formatted, StringComparison.Ordinal));
            return asset != null;
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

    internal static class Node68StateFieldCatalog
    {
        private const string CategoryAsset = "Asset";
        private const string CategoryTransform = "Transform";
        private const string CategoryControls = "Controls";
        private const string CategoryBasicProperties = "Basic Properties";
        private const string CategoryEffects = "Effects";

        internal const string GroupAsset = CategoryAsset;
        internal const string GroupTransform = CategoryTransform;
        internal const string GroupControls = CategoryControls;
        internal const string GroupBasicProperties = CategoryBasicProperties;
        internal const string GroupEffects = CategoryEffects;

        private static readonly KnownField[] Fields =
        {
            new KnownField("Enabled", "Enabled", Node68StateValueType.Boolean, CategoryAsset),
            new KnownField("DefaultIdleAnimation", "Character > Animation > Idle Animation", Node68StateValueType.String, "Character Animation"),
            new KnownField("OverrideHandPoses", "Character > Animation > Override Hand Poses", Node68StateValueType.SerializedDataInput, "Character Animation"),
            new KnownField("BreathingEnabled", "Character > Animation > Breathing Enabled", Node68StateValueType.Boolean, "Character Animation"),
            new KnownField("BreathingExertion", "Character > Animation > Breathing Exertion", Node68StateValueType.Float, "Character Animation"),
            new KnownField("BreathingRate", "Character > Animation > Breathing Rate", Node68StateValueType.Float, "Character Animation"),
            new KnownField("SwayingEnabled", "Character > Animation > Swaying Enabled", Node68StateValueType.Boolean, "Character Animation"),
            new KnownField("SwayingIntensity", "Character > Animation > Swaying Intensity", Node68StateValueType.Float, "Character Animation"),
            new KnownField("OverlappingAnimationTransitionTime", "Character > Animation > Overlapping Transition Time", Node68StateValueType.Float, "Character Animation"),
            new KnownField("AdditionalBoneOffsets", "Character > Animation > Additional Bone Offsets", Node68StateValueType.SerializedDataInput, "Character Animation"),
            new KnownField("ApplyFootIK", "Character > Look At IK > Apply Foot IK", Node68StateValueType.Boolean, "Character Look At IK"),
            new KnownField("DisableUnityRetargeting", "Character > Look At IK > Disable Unity Retargeting", Node68StateValueType.Boolean, "Character Look At IK"),
            new KnownField("LookAtEnabled", "Character > Look At IK > Look At Enabled", Node68StateValueType.Boolean, "Character Look At IK"),
            new KnownField("LookAtTarget", "Character > Look At IK > Look At Target", Node68StateValueType.AssetReferenceAsString, "Character Look At IK"),
            new KnownField("LookAtWeight", "Character > Look At IK > Look At Weight", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtEyesWeight", "Character > Look At IK > Eyes Weight", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtHeadWeight", "Character > Look At IK > Head Weight", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtBodyWeight", "Character > Look At IK > Body Weight", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtClampEyesWeight", "Character > Look At IK > Clamp Eyes", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtClampHeadWeight", "Character > Look At IK > Clamp Head", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("LookAtClampBodyWeight", "Character > Look At IK > Clamp Body", Node68StateValueType.Float, "Character Look At IK"),
            new KnownField("SpineIK", "Character > Look At IK > Spine IK", Node68StateValueType.SerializedDataInput, "Character Look At IK"),
            new KnownField("LeftHandIK", "Character > Look At IK > Left Hand IK", Node68StateValueType.SerializedDataInput, "Character Look At IK"),
            new KnownField("RightHandIK", "Character > Look At IK > Right Hand IK", Node68StateValueType.SerializedDataInput, "Character Look At IK"),
            new KnownField("LeftFootIK", "Character > Look At IK > Left Foot IK", Node68StateValueType.SerializedDataInput, "Character Look At IK"),
            new KnownField("RightFootIK", "Character > Look At IK > Right Foot IK", Node68StateValueType.SerializedDataInput, "Character Look At IK"),
            new KnownField("Transform.Position", "Transform > Position", Node68StateValueType.Vector3, CategoryTransform),
            new KnownField("Transform.Rotation", "Transform > Rotation", Node68StateValueType.Vector3, CategoryTransform),
            new KnownField("Transform.Scale", "Transform > Scale", Node68StateValueType.Vector3, CategoryTransform),
            new KnownField("ControlMode", "Control Mode", Node68StateValueType.EnumAsString, CategoryControls),
            new KnownField("ControlSensitivity", "Control Sensitivity", Node68StateValueType.Float, CategoryControls),
            new KnownField("ControllerInputEnabled", "Controller Input", Node68StateValueType.Boolean, CategoryControls),
            new KnownField("ControllerIndex", "Controller", Node68StateValueType.EnumAsString, CategoryControls),
            new KnownField("ControllerSensitivity", "Controller Sensitivity", Node68StateValueType.Float, CategoryControls),
            new KnownField("ControllerInvertXAxis", "Controller Invert X", Node68StateValueType.Boolean, CategoryControls),
            new KnownField("ControllerInvertYAxis", "Controller Invert Y", Node68StateValueType.Boolean, CategoryControls),
            new KnownField("FocusCharacter", "Focus Character", Node68StateValueType.AssetReferenceAsString, CategoryControls),
            new KnownField("FollowCharacter", "Follow Character", Node68StateValueType.Boolean, CategoryControls),
            new KnownField("FollowCharacterSpeed", "Follow Character Speed", Node68StateValueType.Float, CategoryControls),
            new KnownField("OrbitRotation", "Orbit Rotation", Node68StateValueType.Vector2, CategoryControls),
            new KnownField("OrbitOffset", "Orbit Offset", Node68StateValueType.Vector3, CategoryControls),
            new KnownField("FieldOfView", "Field Of View", Node68StateValueType.Float, CategoryBasicProperties),
            new KnownField("TransparentBackground", "Transparent Background", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("UseChromaKey", "Chroma Key", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("ChromaKeyColor", "Chroma Key Color", Node68StateValueType.Color, CategoryBasicProperties),
            new KnownField("RenderCharacters", "Render Characters", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("RenderProps", "Render Props", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("RenderEnvironment", "Render Environment", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("RenderScreen", "Render Screen", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("NearClipPlane", "Near Clip Plane", Node68StateValueType.Float, CategoryBasicProperties),
            new KnownField("FarClipPlane", "Far Clip Plane", Node68StateValueType.Float, CategoryBasicProperties),
            new KnownField("OrthographicProjection", "Orthographic Projection", Node68StateValueType.Boolean, CategoryBasicProperties),
            new KnownField("NoiseEnabled", "Noise Enabled", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("NoiseAmplitude", "Noise Amplitude", Node68StateValueType.Float, CategoryEffects),
            new KnownField("NoiseFrequency", "Noise Frequency", Node68StateValueType.Float, CategoryEffects),
            new KnownField("ACESTonemapping", "ACES Tonemapping", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("Vibrance", "Vibrance", Node68StateValueType.Float, CategoryEffects),
            new KnownField("Contrast", "Contrast", Node68StateValueType.Float, CategoryEffects),
            new KnownField("Brightness", "Brightness", Node68StateValueType.Float, CategoryEffects),
            new KnownField("Tint", "Tint", Node68StateValueType.Color, CategoryEffects),
            new KnownField("EnableBloom", "Bloom", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("BloomIntensity", "Bloom Intensity", Node68StateValueType.Float, CategoryEffects),
            new KnownField("BloomThreshold", "Bloom Threshold", Node68StateValueType.Float, CategoryEffects),
            new KnownField("BloomTint", "Bloom Tint", Node68StateValueType.Color, CategoryEffects),
            new KnownField("EnableDepthOfField", "Depth Of Field", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("DepthOfFieldFocusDistance", "Depth Of Field Focus Distance", Node68StateValueType.Float, CategoryEffects),
            new KnownField("DepthOfFieldFocusSpeed", "Depth Of Field Focus Speed", Node68StateValueType.Float, CategoryEffects),
            new KnownField("DepthOfFieldFocalLength", "Depth Of Field Focal Length", Node68StateValueType.Float, CategoryEffects),
            new KnownField("DepthOfFieldAperture", "Depth Of Field Aperture", Node68StateValueType.Float, CategoryEffects),
            new KnownField("EnableChromaticAberration", "Chromatic Aberration", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("ChromaticAberrationIntensity", "Chromatic Aberration Intensity", Node68StateValueType.Float, CategoryEffects),
            new KnownField("EnableVignetting", "Vignetting", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("VignettingColor", "Vignetting Color", Node68StateValueType.Color, CategoryEffects),
            new KnownField("VignettingFadeOut", "Vignetting Fade Out", Node68StateValueType.Float, CategoryEffects),
            new KnownField("EnableBlur", "Blur", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("BlurIntensity", "Blur Intensity", Node68StateValueType.Float, CategoryEffects),
            new KnownField("EnablePixelate", "Pixelate", Node68StateValueType.Boolean, CategoryEffects),
            new KnownField("PixelateIntensity", "Pixelate Intensity", Node68StateValueType.Integer, CategoryEffects),
        };

        internal static AutoCompleteList BuildAutoCompleteList()
        {
            return new AutoCompleteList
            {
                categories = Fields
                    .GroupBy(f => f.Category)
                    .Select(
                        group =>
                            new AutoCompleteCategory
                            {
                                title = group.Key,
                                entries = group
                                    .Select(
                                        field =>
                                            new AutoCompleteEntry
                                            {
                                                label = field.Label,
                                                value = field.Path,
                                            }
                                    )
                                    .ToList(),
                            }
                    )
                    .ToList(),
            };
        }

        internal static bool TryGetValueType(string path, out Node68StateValueType valueType)
        {
            foreach (var field in Fields)
            {
                if (string.Equals(field.Path, path?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    valueType = field.ValueType;
                    return true;
                }
            }

            valueType = Node68StateValueType.String;
            return false;
        }

        internal static IEnumerable<Node68StateEntryConfig> CreateEntriesForGroups(
            params string[] groups
        )
        {
            var groupSet = new HashSet<string>(
                groups ?? Array.Empty<string>(),
                StringComparer.OrdinalIgnoreCase
            );

            foreach (var field in Fields)
            {
                if (!groupSet.Contains(field.Category))
                    continue;

                yield return CreateEntry(field.Path);
            }
        }

        internal static Node68StateEntryConfig CreateEntry(string path)
        {
            TryGetValueType(path, out var valueType);
            return new Node68StateEntryConfig
            {
                Enabled = true,
                DataPath = path,
                ValueType = valueType,
                Key = "",
            };
        }

        internal static string GetLabel(string path)
        {
            foreach (var field in Fields)
            {
                if (string.Equals(field.Path, path?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return field.Label;
            }

            return path;
        }

        private readonly struct KnownField
        {
            internal readonly string Path;
            internal readonly string Label;
            internal readonly Node68StateValueType ValueType;
            internal readonly string Category;

            internal KnownField(
                string path,
                string label,
                Node68StateValueType valueType,
                string category
            )
            {
                Path = path;
                Label = label;
                ValueType = valueType;
                Category = category;
            }
        }
    }
}
