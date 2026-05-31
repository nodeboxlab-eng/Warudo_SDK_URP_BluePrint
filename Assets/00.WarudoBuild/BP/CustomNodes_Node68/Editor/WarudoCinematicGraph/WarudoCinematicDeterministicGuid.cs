using System;
using System.Security.Cryptography;
using System.Text;

namespace Node68.CustomNodes.Editor.WarudoCinematicGraph
{
    /// <summary>
    /// 동일 입력에 대해 항상 같은 GUID를 생성 (그래프 재생성 시 diff 최소화·에셋 참조 안정성).
    /// </summary>
    internal static class WarudoCinematicDeterministicGuid
    {
        private const string NamespaceV1 = "node68.warudo.cinematic-graph/v1";

        public static Guid From(params string[] parts)
        {
            var sb = new StringBuilder(NamespaceV1.Length + 32);
            sb.Append(NamespaceV1);
            foreach (var p in parts)
            {
                sb.Append('\u001e');
                sb.Append(p ?? string.Empty);
            }

            byte[] hash;
            using (var md5 = MD5.Create())
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            return new Guid(hash);
        }
    }
}
