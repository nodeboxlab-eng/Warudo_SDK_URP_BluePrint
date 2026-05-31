using System;
using UnityEngine;

namespace Node68.CustomAssets
{
    internal static class Node68TypewriterText68
    {
        public static int VisibleCharsReveal(
            string body,
            float p01,
            TextDisplayTypewriterGranularity68 g
        )
        {
            if (string.IsNullOrEmpty(body))
                return 0;
            p01 = Mathf.Clamp01(p01);
            if (g == TextDisplayTypewriterGranularity68.Character)
            {
                var n = body.Length;
                return Mathf.Clamp(Mathf.CeilToInt(p01 * n), 0, n);
            }

            if (g == TextDisplayTypewriterGranularity68.Word)
            {
                var words = body.Split(
                    new[] { ' ', '\t', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                );
                if (words.Length == 0)
                    return 0;
                var nWord = Mathf.Clamp(Mathf.CeilToInt(p01 * words.Length), 0, words.Length);
                return PrefixCharCountForWordIndex(body, nWord);
            }

            {
                var nLines = CountLines(body);
                if (nLines <= 0)
                    return 0;
                var nLine = Mathf.Clamp(Mathf.CeilToInt(p01 * nLines), 0, nLines);
                return PrefixCharCountForLineIndex(body, nLine);
            }
        }

        public static int VisibleCharsHide(
            string body,
            float p01,
            TextDisplayTypewriterGranularity68 g
        )
        {
            if (string.IsNullOrEmpty(body))
                return 0;
            p01 = Mathf.Clamp01(p01);
            var q = 1f - p01;
            if (g == TextDisplayTypewriterGranularity68.Character)
            {
                var n = body.Length;
                return Mathf.Clamp(Mathf.FloorToInt(q * n + 1e-4f), 0, n);
            }

            if (g == TextDisplayTypewriterGranularity68.Word)
            {
                var words = body.Split(
                    new[] { ' ', '\t', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                );
                if (words.Length == 0)
                    return 0;
                var nWord = Mathf.Clamp(
                    Mathf.FloorToInt(q * words.Length + 1e-4f),
                    0,
                    words.Length
                );
                return PrefixCharCountForWordIndex(body, nWord);
            }

            {
                var nLines = CountLines(body);
                if (nLines <= 0)
                    return 0;
                var nLine = Mathf.Clamp(Mathf.FloorToInt(q * nLines + 1e-4f), 0, nLines);
                return PrefixCharCountForLineIndex(body, nLine);
            }
        }

        private static int CountLines(string body)
        {
            if (string.IsNullOrEmpty(body))
                return 0;
            var n = 1;
            for (var i = 0; i < body.Length; i++)
            {
                if (body[i] == '\n' || body[i] == '\r')
                {
                    n++;
                    if (body[i] == '\r' && i + 1 < body.Length && body[i + 1] == '\n')
                        i++;
                }
            }
            return n;
        }

        private static int PrefixCharCountForLineIndex(string body, int lineCount)
        {
            if (lineCount <= 0 || string.IsNullOrEmpty(body))
                return 0;
            var line = 1;
            var i = 0;
            while (i < body.Length && line < lineCount)
            {
                if (body[i] == '\n')
                {
                    line++;
                    i++;
                }
                else if (body[i] == '\r')
                {
                    line++;
                    i++;
                    if (i < body.Length && body[i] == '\n')
                        i++;
                }
                else
                {
                    i++;
                }
            }

            if (line < lineCount)
                return body.Length;

            var end = i;
            while (end < body.Length && body[end] != '\r' && body[end] != '\n')
                end++;
            return end;
        }

        private static int PrefixCharCountForWordIndex(string body, int wordCount)
        {
            if (wordCount <= 0 || string.IsNullOrEmpty(body))
                return 0;
            var seen = 0;
            for (var i = 0; i < body.Length; i++)
            {
                if (char.IsWhiteSpace(body[i]))
                    continue;
                seen++;
                while (i < body.Length && !char.IsWhiteSpace(body[i]))
                    i++;
                if (seen >= wordCount)
                    return i;
                i--;
            }

            return body.Length;
        }
    }
}
