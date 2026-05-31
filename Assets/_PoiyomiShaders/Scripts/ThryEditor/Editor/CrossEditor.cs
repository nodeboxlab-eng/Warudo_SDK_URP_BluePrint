using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Thry.ThryEditor
{
    public class CrossEditor : EditorWindow
    {
        public static CrossEditor GetInstance()
        {
            CrossEditor window = GetWindow(typeof(CrossEditor)) as CrossEditor;
            window.name = "Cross Shader Editor";

            return window;
        }

        [MenuItem("Assets/Thry/Materials/Add to Cross Shader Editor", false , 400)]
        private static void OpenInCrossShaderEditor()
        {
            List<Material> materials = ShaderOptimizer.FindMaterials(ShaderOptimizer.GetSelectedFolders());
            materials.AddRange(Selection.objects.Where(o => o is Material).Cast<Material>());

            GetInstance().UpdateTargets(materials, true);
        }

        [MenuItem("Assets/Thry/Materials/Add to Cross Shader Editor", true, 400)]
        private static bool OpenInCrossShaderEditorValidation()
        {
            return Selection.objects.Any(o => o is Material) || ShaderOptimizer.GetSelectedFolders().Any();
        }

        [MenuItem("GameObject/Thry/Materials/Open All in Cross Shader Editor", false, 10)]
        private static void OpenAllInCrossShaderEditor()
        {
            GetInstance().UpdateTargets(Selection.gameObjects.SelectMany(o => o.GetComponentsInChildren<Renderer>(true)).SelectMany(r => r.sharedMaterials));
        }

        List<Material> _materialList = new List<Material>();
        List<Material> _targets = new List<Material>();
        Dictionary<Material,Shader> _targetShaders = new Dictionary<Material, Shader>();
        ShaderEditor _shaderEditor = null;
        MaterialEditor _materialEditor = null;
        MaterialProperty[] _materialProperties = null;
        Vector2 _scrollPosition = Vector2.zero;
        bool _showMaterials = true;

        public void UpdateTargets(IEnumerable<Material> materials, bool add = false)
        {
            _materialList = (add ?
                _materialList.Concat(materials) : // add
                materials) // replace
                .Distinct().ToList(); // deduplicate

            UpdateTargets();
        }

        private void UpdateTargets()
        {
            _targets = _materialList.Where(t => t != null && !t.shader.IsBroken()).ToList();
            foreach(Material m in _materialList.Where(t => t != null && t.shader.IsBroken()))
                Debug.LogWarning("Material " + m.name + " has no shader assigned");

            _shaderEditor = null;
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true));
            _showMaterials = EditorGUILayout.Foldout(_showMaterials, "Materials");

            EditorGUI.BeginChangeCheck();
            DrawMaterials();

            // Check if targets have changed
            bool didShadersChange = EditorGUI.EndChangeCheck();
            foreach (Material m in _materialList)
            {
                if (m == null || // Material is null
                    _targetShaders.ContainsKey(m) && _targetShaders[m] == m.shader) // Shader hasn't changed
                    continue;

                didShadersChange = true;
                _targetShaders[m] = m.shader;
            }

            if (didShadersChange) UpdateTargets();

            DrawShaderEditor();

            EditorGUILayout.EndScrollView();
        }

        // List of materials, remove button next to each
        // Add and Remove All buttons at bottom
        private void DrawMaterials()
        {
            if (!_showMaterials) return;

            for (int i = 0; i < _materialList.Count; i++) DrawMaterial(i);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(15);
                if (GUILayout.Button("Add", GUILayout.Width(100))) _materialList.Add(null);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Remove All", GUILayout.Width(100))) _materialList.Clear();
            }
        }

        private void DrawMaterial(int i)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(15);
                Material material = (Material)EditorGUILayout.ObjectField(_materialList[i], typeof(Material), false);

                if (material != _materialList[i])
                {
                    if (_materialList.Contains(material)) material = null;

                    _materialList[i] = material;
                }

                if (GUILayout.Button("Remove", GUILayout.Width(60))) _materialList.RemoveAt(i);
            }
        }

        private void DrawShaderEditor()
        {
            if (_targets.Count == 0) return;

            // Create shader editor
            CreateShaderEditor();

            // Seperator
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            bool wideMode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = true;
            _shaderEditor.OnGUI(_materialEditor, _materialProperties);
            EditorGUIUtility.wideMode = wideMode;
        }

        private void CreateShaderEditor()
        {
            if (_shaderEditor != null) return;

            _shaderEditor = new ShaderEditor(){ IsCrossEditor = true };
            _materialEditor = Editor.CreateEditor(_targets.ToArray()) as MaterialEditor;

            // group targets by shader, take one material per shader
            IEnumerable<Material> materialsToSearchProperties = _targets.GroupBy(t => t.shader).Select(g => g.First());
            // get properties for each shader (preserving order and duplicates)
            Dictionary<Shader, string[]> shaderProperties = materialsToSearchProperties.ToDictionary(
                m => m.shader,
                m => MaterialEditor.GetMaterialProperties(new UnityEngine.Object[] { m }).Select(p => p.name).ToArray());

            // Build ordered property list that preserves duplicate names.
            // Track how many times each name has appeared so far to distinguish duplicates.
            List<string> propertiesOrdered = new List<string>();

            // Seed with the first shader's full property list
            string[][] allPropertyArrays = shaderProperties.Values.ToArray();
            if (allPropertyArrays.Length > 0)
                propertiesOrdered.AddRange(allPropertyArrays[0]);

            // Merge each subsequent shader's properties, inserting missing ones after their predecessor
            for (int s = 1; s < allPropertyArrays.Length; s++)
            {
                string[] properties = allPropertyArrays[s];
                // Track occurrence counts in propertiesOrdered to handle duplicates
                Dictionary<string, int> seenInOrdered = new Dictionary<string, int>();

                for (int i = 0; i < properties.Length; i++)
                {
                    string property = properties[i];
                    // Count how many times this name appeared before index i in this shader's list
                    int occurrenceIndex = 0;
                    for (int j = 0; j < i; j++)
                        if (properties[j] == property) occurrenceIndex++;

                    // Find the nth occurrence in propertiesOrdered
                    int foundCount = 0;
                    int posInOrdered = -1;
                    for (int k = 0; k < propertiesOrdered.Count; k++)
                    {
                        if (propertiesOrdered[k] == property)
                        {
                            if (foundCount == occurrenceIndex)
                            {
                                posInOrdered = k;
                                break;
                            }
                            foundCount++;
                        }
                    }

                    if (posInOrdered == -1)
                    {
                        // Not found — insert after predecessor
                        if (i == 0)
                        {
                            propertiesOrdered.Insert(0, property);
                        }
                        else
                        {
                            string pred = properties[i - 1];
                            // Find the predecessor's matching occurrence
                            int predOccurrence = 0;
                            for (int j = 0; j < i - 1; j++)
                                if (properties[j] == pred) predOccurrence++;

                            int predFoundCount = 0;
                            int predPos = 0;
                            for (int k = 0; k < propertiesOrdered.Count; k++)
                            {
                                if (propertiesOrdered[k] == pred)
                                {
                                    if (predFoundCount == predOccurrence)
                                    {
                                        predPos = k;
                                        break;
                                    }
                                    predFoundCount++;
                                }
                            }
                            propertiesOrdered.Insert(predPos + 1, property);
                        }
                    }
                }
            }

            // Build a lookup: for each (name, occurrenceIndex), which materials have that occurrence?
            // First, build per-shader occurrence counts
            Dictionary<Shader, Dictionary<string, int>> shaderPropertyCounts = new Dictionary<Shader, Dictionary<string, int>>();
            foreach (var kvp in shaderProperties)
            {
                var counts = new Dictionary<string, int>();
                foreach (string p in kvp.Value)
                {
                    if (!counts.ContainsKey(p)) counts[p] = 0;
                    counts[p]++;
                }
                shaderPropertyCounts[kvp.Key] = counts;
            }

            // For each entry in propertiesOrdered, find materials whose shader has enough occurrences of that property
            List<Material[]> propertyMaterialsList = new List<Material[]>();
            Dictionary<string, int> orderedOccurrences = new Dictionary<string, int>();
            foreach (string property in propertiesOrdered)
            {
                if (!orderedOccurrences.ContainsKey(property)) orderedOccurrences[property] = 0;
                int requiredCount = orderedOccurrences[property] + 1;

                propertyMaterialsList.Add(
                    _targets.Where(t => shaderPropertyCounts[t.shader].ContainsKey(property) &&
                                        shaderPropertyCounts[t.shader][property] >= requiredCount).ToArray());

                orderedOccurrences[property]++;
            }

            // Get MaterialProperties of all materials, handling duplicate property names.
            // MaterialEditor.GetMaterialProperty(mats, name) always returns the same property for a given name,
            // so we must use GetMaterialProperties to get the full list and pick by occurrence index.
            _materialProperties = new MaterialProperty[propertiesOrdered.Count];
            // Cache GetMaterialProperties per unique material set (keyed by sorted instance IDs)
            Dictionary<string, MaterialProperty[]> matPropsCache = new Dictionary<string, MaterialProperty[]>();
            Dictionary<string, int> occurrenceTracker = new Dictionary<string, int>();

            for (int i = 0; i < propertiesOrdered.Count; i++)
            {
                Material[] mats = propertyMaterialsList[i];
                if (mats.Length == 0) continue;

                string property = propertiesOrdered[i];
                if (!occurrenceTracker.ContainsKey(property)) occurrenceTracker[property] = 0;
                int occurrence = occurrenceTracker[property];
                occurrenceTracker[property]++;

                // For the first occurrence, GetMaterialProperty works fine
                if (occurrence == 0)
                {
                    _materialProperties[i] = MaterialEditor.GetMaterialProperty(mats, property);
                    continue;
                }

                // For subsequent occurrences, get the full property array and find the nth match
                string cacheKey = string.Join(",", mats.Select(m => m.GetInstanceID()).OrderBy(id => id));
                if (!matPropsCache.TryGetValue(cacheKey, out MaterialProperty[] allProps))
                {
                    allProps = MaterialEditor.GetMaterialProperties(mats.Cast<UnityEngine.Object>().ToArray());
                    matPropsCache[cacheKey] = allProps;
                }

                int found = 0;
                foreach (MaterialProperty mp in allProps)
                {
                    if (mp.name == property)
                    {
                        if (found == occurrence)
                        {
                            _materialProperties[i] = mp;
                            break;
                        }
                        found++;
                    }
                }
            }
            Debug.Log(propertyMaterialsList[propertiesOrdered.IndexOf("_EnableGrabpass")].Length);
            MaterialProperty test = _materialProperties.Where(p => p != null && p.name == "_EnableGrabpass").First();
            Debug.Log(test.displayName);
            Debug.Log(test.GetPropertyType());
            Debug.Log(test.GetPropertyFlags());
            Shader s2 = (test.targets[0] as Material).shader;
            Debug.Log(string.Join(",", s2.GetPropertyAttributes(s2.FindPropertyIndex(test.name))));
        }
    }
}