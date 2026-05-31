using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Graphs;
using Warudo.Plugins.Core.Assets.Character;

namespace Node68.ToolkitMods.Node68DevKit
{
    /// <summary>
    /// <see cref="CharacterAsset.Meshes"/> 전체의 <c>Visible</c> 를 한 번에 토글합니다.
    /// 인스펙터에서 메시 하나씩 누르는 노가다를 대체하기 위한 DevKit 편의 노드입니다.
    /// 포함/제외 필터(쉼표 구분, 부분일치)를 통해 일부 메시만 대상으로 삼을 수도 있습니다.
    /// </summary>
    [NodeType(
        Id = "b2e6f9c1-3a7d-4e5b-8a2c-7f1e9d3b4c50",
        Title = "Toggle All Character Meshes Node68",
        Category = BpToolkitFlavorEmbedded.ShareBuild
            ? BpToolkitCategories.Share
            : BpToolkitCategories.Toolkit,
        Width = 1.3f
    )]
    public sealed class ToggleAllCharacterMeshesNode68 : Node
    {
        [DataInput]
        [Label("캐릭터")]
        [Description("대상 캐릭터. Meshes 항목 전체가 일괄 변경됩니다.")]
        public CharacterAsset Character;

        [DataInput]
        [Label("포함 필터")]
        [Description(
            "쉼표(,)로 구분한 부분일치 토큰. 비우면 전체 메시가 대상. 예) Hair, Top"
        )]
        public string IncludeFilter = "";

        [DataInput]
        [Label("제외 필터")]
        [Description("쉼표(,)로 구분한 부분일치 토큰. 매칭되면 건너뜀. 예) base, body_base")]
        public string ExcludeFilter = "";

        [DataInput]
        [Label("인스펙터 동기화")]
        [Description("켜면 변경 후 캐릭터 인스펙터의 Visible 토글이 즉시 다시 그려집니다.")]
        public bool BroadcastInspectorRefresh = true;

        [Markdown]
        [Hidden]
        public string _note =
            "이 노드는 `Character.Meshes` 배열의 각 항목의 `Visible` 필드를 직접 토글하고 `UpdateVisibility()` 를 호출합니다. " +
            "캐릭터가 스폰되어 `Meshes` 가 채워진 뒤에만 동작합니다.";

        [FlowInput]
        [Label("전부 숨김 (No)")]
        public Continuation HideAll()
        {
            ApplyVisibility(VisibilityMode.ForceHide);
            return Exit;
        }

        [FlowInput]
        [Label("전부 보임 (Yes)")]
        public Continuation ShowAll()
        {
            ApplyVisibility(VisibilityMode.ForceShow);
            return Exit;
        }

        [FlowInput]
        [Label("반전")]
        public Continuation InvertAll()
        {
            ApplyVisibility(VisibilityMode.Invert);
            return Exit;
        }

        [FlowOutput]
        public Continuation Exit;

        [DataOutput]
        [Label("적용된 메시 수")]
        public int OutputAffectedCount() => _lastAffected;

        [DataOutput]
        [Label("전체 메시 수")]
        public int OutputTotalCount() => _lastTotal;

        [DataOutput]
        [Label("결과 메시지")]
        public string OutputLastResult() => _lastResult;

        private int _lastAffected;
        private int _lastTotal;
        private string _lastResult = "";

        private enum VisibilityMode
        {
            ForceHide,
            ForceShow,
            Invert,
        }

        private void ApplyVisibility(VisibilityMode mode)
        {
            if (Character == null)
            {
                _lastAffected = 0;
                _lastTotal = 0;
                _lastResult = "(캐릭터 없음)";
                Broadcast();
                return;
            }

            var meshes = Character.Meshes;
            if (meshes == null || meshes.Length == 0)
            {
                _lastAffected = 0;
                _lastTotal = 0;
                _lastResult = "(Meshes 비어있음 — 캐릭터 스폰 후 다시 시도)";
                Broadcast();
                return;
            }

            var includes = ParseTokens(IncludeFilter);
            var excludes = ParseTokens(ExcludeFilter);

            var affected = 0;

            for (var i = 0; i < meshes.Length; i++)
            {
                var m = meshes[i];
                if (m == null)
                    continue;

                var path = m.Path ?? "";
                if (includes.Count > 0 && !MatchAny(path, includes))
                    continue;
                if (excludes.Count > 0 && MatchAny(path, excludes))
                    continue;

                var newValue = mode switch
                {
                    VisibilityMode.ForceHide => false,
                    VisibilityMode.ForceShow => true,
                    VisibilityMode.Invert => !m.Visible,
                    _ => m.Visible,
                };

                if (m.Visible == newValue)
                    continue;

                m.Visible = newValue;
                try
                {
                    m.UpdateVisibility();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[ToggleAllMeshes68] UpdateVisibility 실패: path={path}, err={ex.Message}"
                    );
                }

                affected++;
            }

            if (affected > 0 && BroadcastInspectorRefresh)
            {
                try
                {
                    Character.BroadcastDataInput(nameof(CharacterAsset.Meshes));
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[ToggleAllMeshes68] Meshes 브로드캐스트 실패: {ex.Message}"
                    );
                }
            }

            _lastAffected = affected;
            _lastTotal = meshes.Length;
            _lastResult = mode switch
            {
                VisibilityMode.ForceHide => $"Hide All: {affected}/{meshes.Length}개 숨김",
                VisibilityMode.ForceShow => $"Show All: {affected}/{meshes.Length}개 보임",
                VisibilityMode.Invert => $"Invert: {affected}/{meshes.Length}개 반전",
                _ => $"{affected}/{meshes.Length}",
            };

            Broadcast();
        }

        private static List<string> ParseTokens(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return new List<string>(0);

            return raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => t.Length > 0)
                .ToList();
        }

        private static bool MatchAny(string path, List<string> tokens)
        {
            if (string.IsNullOrEmpty(path) || tokens == null || tokens.Count == 0)
                return false;

            for (var i = 0; i < tokens.Count; i++)
            {
                if (path.IndexOf(tokens[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
    }
}
