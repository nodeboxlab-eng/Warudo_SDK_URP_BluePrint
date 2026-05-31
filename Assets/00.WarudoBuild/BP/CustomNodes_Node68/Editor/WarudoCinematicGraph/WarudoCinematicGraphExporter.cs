using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// 생성된 JSON을 UTF-8(무 BOM)으로 저장합니다.
    /// </summary>
    public static class WarudoCinematicGraphExporter
    {
        public static void ExportToFile(string absolutePath, string json)
        {
            if (string.IsNullOrEmpty(absolutePath))
                throw new ArgumentException("경로가 비어 있습니다.", nameof(absolutePath));

            var utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            File.WriteAllText(absolutePath, json ?? string.Empty, utf8NoBom);
            AssetDatabase.Refresh();
        }
    }
}
