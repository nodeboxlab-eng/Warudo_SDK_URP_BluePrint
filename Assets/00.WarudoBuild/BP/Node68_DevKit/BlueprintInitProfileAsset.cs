using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// 블루프린트 초기화에 넣어야 할 값을 규칙으로 등록하고,
    /// Markdown 체크리스트로 ON_ENABLE_GRAPH 연결 가이드를 표시합니다 (실행은 하지 않음).
    /// </summary>
    [AssetType(
        Id = "e7f8a9b0-1c2d-4e3f-8a7b-6c5d4e3f2a10",
        Title = "Blueprint Init Profile Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        CategoryOrder = -55
    )]
    public sealed class BlueprintInitProfileAsset : Asset
    {
        [DataInput]
        [Section("프로필")]
        [Label("표시 이름")]
        [Description("체크리스트 제목에 사용됩니다. 비우면 에셋 이름을 씁니다.")]
        public string ProfileTitle = "";

        [DataInput]
        [Label("설명")]
        [MultilineInput]
        [Description("체크리스트 상단에 표시할 메모 (연출 BP 목적 등).")]
        public string Description = "";

        [DataInput]
        [Section("블루프린트")]
        [Label("연결 BP")]
        [Description("스캔·Graph Variable 검증에 사용합니다. 비우면 규칙만으로 체크리스트를 만듭니다.")]
        [AutoComplete(nameof(AutoCompleteTargetGraph))]
        public string TargetGraphId = "";

        [DataInput]
        [Section("규칙")]
        [Label("초기화 규칙")]
        [Description(
            "ON_ENABLE_GRAPH 에 넣어야 할 항목을 등록하세요. SortOrder 가 작을수록 먼저 실행하는 것을 권장합니다."
        )]
        public InitRuleItem[] Rules = Array.Empty<InitRuleItem>();

        [Section("체크리스트")]
        [Markdown]
        public string _checklistDisplay =
            "*「체크리스트 갱신」을 눌러 ON_ENABLE_GRAPH 가이드를 생성하세요.*";

        [Trigger]
        [Label("체크리스트 갱신")]
        [Description("등록된 규칙과 (있으면) 마지막 BP 스캔 결과로 Markdown 을 다시 만듭니다.")]
        public void RefreshChecklist()
        {
            UpdateChecklistDisplay();
        }

        [Trigger]
        [Label("BP 스캔 → 제안 반영")]
        [Description(
            "연결 BP에서 SET_* / TOGGLE_* 등 변경 노드를 찾아 제안 목록을 갱신하고 체크리스트를 다시 만듭니다."
        )]
        public void ScanBlueprintAndRefresh()
        {
            var graph = FindLinkedGraph();
            if (graph == null)
            {
                _lastScanSuggestions = Array.Empty<BlueprintInitGraphScanner.ScanSuggestion>();
                UpdateChecklistDisplay(
                    "**오류**: 연결 BP 가 없습니다. 「연결 BP」를 선택하거나 규칙만 수동 등록하세요."
                );
                return;
            }

            _lastScanSuggestions = BlueprintInitGraphScanner.Scan(graph).ToArray();
            UpdateChecklistDisplay();
            Debug.Log(
                "[Init Profile] BP 스캔 완료 · "
                    + graph.Name
                    + " · 제안 "
                    + _lastScanSuggestions.Length
                    + "건"
            );
        }

        [Trigger]
        [Label("스캔 제안 → 규칙에 추가")]
        [Description("마지막 BP 스캔 제안 중, 아직 없는 Kind 를 규칙 배열 끝에 추가합니다.")]
        public void AppendScanSuggestionsToRules()
        {
            if (_lastScanSuggestions == null || _lastScanSuggestions.Length == 0)
            {
                UpdateChecklistDisplay(
                    "**안내**: 먼저 「BP 스캔 → 제안 반영」을 실행하세요."
                );
                return;
            }

            var list = new List<InitRuleItem>(Rules ?? Array.Empty<InitRuleItem>());
            var existingKinds = new HashSet<BlueprintInitRuleKind68>(
                list.Where(r => r != null && r.Enabled).Select(r => r.Kind)
            );

            var added = 0;
            var nextSort =
                list.Count > 0 ? list.Where(r => r != null).Max(r => r.SortOrder) + 10 : 0;

            foreach (var suggestion in _lastScanSuggestions)
            {
                if (existingKinds.Contains(suggestion.SuggestedKind))
                    continue;

                var rule = StructuredData.Create<InitRuleItem, BlueprintInitProfileAsset>(
                    this,
                    r =>
                    {
                        r.Enabled = true;
                        r.Kind = suggestion.SuggestedKind;
                        r.SortOrder = nextSort;
                        r.Note = suggestion.Summary + " · `" + suggestion.SourceNodeName + "`";
                        ApplySuggestionDefaults(r, suggestion);
                    }
                );

                list.Add(rule);
                existingKinds.Add(suggestion.SuggestedKind);
                nextSort += 10;
                added++;
            }

            if (added == 0)
            {
                UpdateChecklistDisplay(
                    "**안내**: 추가할 새 Kind 가 없습니다 (이미 규칙에 포함됨)."
                );
                return;
            }

            SetDataInput(nameof(Rules), list.ToArray(), broadcast: true);
            UpdateChecklistDisplay();
            Debug.Log("[Init Profile] 스캔 제안 " + added + "건 규칙에 추가");
        }

        private BlueprintInitGraphScanner.ScanSuggestion[] _lastScanSuggestions =
            Array.Empty<BlueprintInitGraphScanner.ScanSuggestion>();

        protected override void OnCreate()
        {
            base.OnCreate();
            SetActive(true);
            Watch(nameof(Rules), OnRulesChanged);
            Watch(nameof(TargetGraphId), OnGraphLinkChanged);
            Watch(nameof(ProfileTitle), OnRulesChanged);
            Watch(nameof(Description), OnRulesChanged);
        }

        private void OnRulesChanged() => RefreshChecklist();

        private void OnGraphLinkChanged()
        {
            _lastScanSuggestions = Array.Empty<BlueprintInitGraphScanner.ScanSuggestion>();
            RefreshChecklist();
        }

        private Graph FindLinkedGraph()
        {
            if (string.IsNullOrEmpty(TargetGraphId))
                return null;

            if (!Guid.TryParse(TargetGraphId, out var graphGuid))
                return null;

            return Context.OpenedScene?.GetGraph(graphGuid);
        }

        private void UpdateChecklistDisplay(string overrideMarkdown = null)
        {
            if (!string.IsNullOrEmpty(overrideMarkdown))
            {
                SetDataInput(nameof(_checklistDisplay), overrideMarkdown, broadcast: true);
                return;
            }

            var markdown = BlueprintInitChecklistFormatter.Format(
                this,
                FindLinkedGraph(),
                _lastScanSuggestions
            );
            SetDataInput(nameof(_checklistDisplay), markdown, broadcast: true);
        }

        private static void ApplySuggestionDefaults(
            InitRuleItem rule,
            BlueprintInitGraphScanner.ScanSuggestion suggestion
        )
        {
            switch (suggestion.SuggestedKind)
            {
                case BlueprintInitRuleKind68.GameObjectEnabled:
                    rule.BoolValue = false;
                    break;

                case BlueprintInitRuleKind68.PropBlendShape:
                    rule.FloatValue = 0f;
                    break;

                case BlueprintInitRuleKind68.TextDisplayReset:
                    rule.StringValue = "";
                    rule.BoolValue = true;
                    break;

                case BlueprintInitRuleKind68.CameraRestoreFromVariables:
                    rule.StringValue = "PreOrbitX";
                    rule.StringValue2 = "PreOrbitY";
                    rule.StringValue3 = "PreOrbitOffset";
                    rule.StringValue4 = "PreFOV";
                    break;

                case BlueprintInitRuleKind68.CharacterLookAtAndFov:
                    rule.BoolValue = true;
                    rule.StringValue = "PreLookAtCamera";
                    break;
            }
        }

        public async UniTask<AutoCompleteList> AutoCompleteTargetGraph()
        {
            await UniTask.CompletedTask;
            var scene = Context.OpenedScene;
            if (scene == null)
                return AutoCompleteList.Message("씬이 열려있지 않습니다");

            var graphs = scene.GetGraphs();
            if (graphs == null || graphs.Count == 0)
                return AutoCompleteList.Message("블루프린트가 없습니다");

            var entries = graphs
                .Values.Select(g => new AutoCompleteEntry
                {
                    label = g.Name + "  (노드 " + g.GetNodes().Count + "개)",
                    value = g.Id.ToString(),
                })
                .ToList();

            return new AutoCompleteList
            {
                categories = new List<AutoCompleteCategory>
                {
                    new AutoCompleteCategory { title = "블루프린트", entries = entries },
                },
            };
        }

        public sealed class InitRuleItem
            : StructuredData<BlueprintInitProfileAsset>,
                ICollapsibleStructuredData
        {
            [DataInput]
            [Label("활성")]
            public bool Enabled = true;

            [DataInput]
            [Label("실행 순서")]
            [Description("작을수록 먼저. 비활성화 → 위치 이동 순서 등을 숫자로 조정하세요.")]
            public int SortOrder;

            [DataInput]
            [Label("종류")]
            public BlueprintInitRuleKind68 Kind = BlueprintInitRuleKind68.ManualNote;

            [DataInput]
            [Label("GameObject / Prop")]
            [HiddenIf(nameof(HideTargetAsset))]
            public GameObjectAsset TargetAsset;

            [DataInput]
            [Label("TextDisplay")]
            [HiddenIf(nameof(HideTextDisplay))]
            public GameObjectAsset TextDisplay;

            [DataInput]
            [Label("캐릭터")]
            [HiddenIf(nameof(HideCharacter))]
            public CharacterAsset Character;

            [DataInput]
            [Label("스킨 메시 키")]
            [HiddenIf(nameof(HidePropFields))]
            public string SkinnedMeshKey = "";

            [DataInput]
            [Label("블렌드쉐이프 이름")]
            [HiddenIf(nameof(HidePropFields))]
            public string BlendShapeName = "";

            [DataInput]
            [Label("Bool 값")]
            [Description("활성/비활성, LookAt, 숨김 등.")]
            [HiddenIf(nameof(HideBoolValue))]
            public bool BoolValue;

            [DataInput]
            [Label("Float 값")]
            [HiddenIf(nameof(HideFloatValue))]
            public float FloatValue;

            [DataInput]
            [Label("Vector3 (위치 등)")]
            [HiddenIf(nameof(HideVector3))]
            public Vector3 Vector3Value;

            [DataInput]
            [Label("문자열 A")]
            [Description("변수명, 텍스트, PreOrbitX 등.")]
            [HiddenIf(nameof(HideStringA))]
            public string StringValue = "PreOrbitX";

            [DataInput]
            [Label("문자열 B")]
            [HiddenIf(nameof(HideStringB))]
            public string StringValue2 = "PreOrbitY";

            [DataInput]
            [Label("문자열 C")]
            [HiddenIf(nameof(HideStringC))]
            public string StringValue3 = "PreOrbitOffset";

            [DataInput]
            [Label("문자열 D")]
            [HiddenIf(nameof(HideStringD))]
            public string StringValue4 = "PreFOV";

            [DataInput]
            [Label("메모")]
            [MultilineInput]
            public string Note = "";

            public string GetHeader()
            {
                var kind = Kind switch
                {
                    BlueprintInitRuleKind68.GameObjectEnabled => "GO 활성",
                    BlueprintInitRuleKind68.GameObjectTransform => "GO 트랜스폼",
                    BlueprintInitRuleKind68.PropBlendShape => "BlendShape",
                    BlueprintInitRuleKind68.TextDisplayReset => "TextDisplay",
                    BlueprintInitRuleKind68.CameraRestoreFromVariables => "카메라",
                    BlueprintInitRuleKind68.CharacterLookAtAndFov => "LookAt/FOV",
                    BlueprintInitRuleKind68.GraphVariable => "Variable",
                    _ => "메모",
                };

                var target =
                    TargetAsset?.Name
                    ?? TextDisplay?.Name
                    ?? Character?.Name
                    ?? "";

                if (string.IsNullOrEmpty(target) && !string.IsNullOrWhiteSpace(Note))
                {
                    var preview = Note.Trim();
                    if (preview.Length > 24)
                        preview = preview.Substring(0, 24) + "…";
                    target = preview;
                }

                return (Enabled ? "✓ " : "○ ")
                    + kind
                    + (string.IsNullOrEmpty(target) ? "" : " · " + target)
                    + " (#"
                    + SortOrder
                    + ")";
            }

            private bool HideTargetAsset() =>
                Kind
                    is not BlueprintInitRuleKind68.GameObjectEnabled
                        and not BlueprintInitRuleKind68.GameObjectTransform
                        and not BlueprintInitRuleKind68.PropBlendShape;

            private bool HideTextDisplay() => Kind != BlueprintInitRuleKind68.TextDisplayReset;

            private bool HideCharacter() => Kind != BlueprintInitRuleKind68.CharacterLookAtAndFov;

            private bool HidePropFields() => Kind != BlueprintInitRuleKind68.PropBlendShape;

            private bool HideBoolValue() =>
                Kind
                    is not BlueprintInitRuleKind68.GameObjectEnabled
                        and not BlueprintInitRuleKind68.TextDisplayReset
                        and not BlueprintInitRuleKind68.CharacterLookAtAndFov;

            private bool HideFloatValue() =>
                Kind
                    is not BlueprintInitRuleKind68.PropBlendShape
                        and not BlueprintInitRuleKind68.GraphVariable;

            private bool HideVector3() => Kind != BlueprintInitRuleKind68.GameObjectTransform;

            private bool HideStringA() =>
                Kind
                    is not BlueprintInitRuleKind68.TextDisplayReset
                        and not BlueprintInitRuleKind68.CameraRestoreFromVariables
                        and not BlueprintInitRuleKind68.CharacterLookAtAndFov
                        and not BlueprintInitRuleKind68.GraphVariable;

            private bool HideStringB() =>
                Kind != BlueprintInitRuleKind68.CameraRestoreFromVariables;

            private bool HideStringC() =>
                Kind != BlueprintInitRuleKind68.CameraRestoreFromVariables;

            private bool HideStringD() =>
                Kind != BlueprintInitRuleKind68.CameraRestoreFromVariables;
        }
    }
}
