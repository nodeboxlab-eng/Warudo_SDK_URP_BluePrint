using UnityEngine;
using UnityEditor;
using System.IO;

namespace MaskCreationTool.Editor
{
    /// <summary>
    /// マスクのImport/Export処理
    /// </summary>
    public static class MaskImportExporter
    {
        /// <summary>
        /// マスクをPNGファイルとしてエクスポート
        /// </summary>
        public static void ExportMask(MaskCanvasModel model, string path)
        {
            if (model == null || string.IsNullOrEmpty(path))
            {
                // Debug.LogError("ExportMask: Invalid parameters");
                return;
            }

            var texture = new Texture2D(model.Width, model.Height, TextureFormat.R8, false);
            var pixels = new Color[model.Width * model.Height];

            for (int y = 0; y < model.Height; y++)
            {
                for (int x = 0; x < model.Width; x++)
                {
                    float value = model.GetValue(x, y);
                    int index = y * model.Width + x;
                    pixels[index] = new Color(value, value, value, 1f);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);

            Object.DestroyImmediate(texture);

            // Unityにリフレッシュさせる
            AssetDatabase.Refresh();

            // Debug.Log($"Mask exported to: {path}");
        }

        /// <summary>
        /// PNGファイルからマスクをインポート
        /// </summary>
        public static bool ImportMask(MaskCanvasModel model, string path)
        {
            if (model == null || string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                // Debug.LogError("ImportMask: Invalid parameters or file not found");
                return false;
            }

            byte[] bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);

            if (!texture.LoadImage(bytes))
            {
                // Debug.LogError("ImportMask: Failed to load image");
                Object.DestroyImmediate(texture);
                return false;
            }

            // サイズチェック
            if (texture.width != model.Width || texture.height != model.Height)
            {
                // Debug.LogWarning($"ImportMask: Size mismatch. Expected {model.Width}x{model.Height}, got {texture.width}x{texture.height}. Resizing...");

                // リサイズ
                var resizedTexture = ResizeTexture(texture, model.Width, model.Height);
                Object.DestroyImmediate(texture);
                texture = resizedTexture;
            }

            var pixels = texture.GetPixels();

            for (int y = 0; y < model.Height; y++)
            {
                for (int x = 0; x < model.Width; x++)
                {
                    int index = y * model.Width + x;
                    float value = pixels[index].grayscale;
                    model.SetValue(x, y, value);
                }
            }

            model.SyncCpuToGpu();

            Object.DestroyImmediate(texture);

            // Debug.Log($"Mask imported from: {path}");
            return true;
        }

        /// <summary>
        /// テクスチャをリサイズ
        /// </summary>
        private static Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            var renderTexture = RenderTexture.GetTemporary(targetWidth, targetHeight);
            Graphics.Blit(source, renderTexture);

            RenderTexture.active = renderTexture;
            var result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            result.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);

            return result;
        }

        /// <summary>
        /// Export用のファイルダイアログを表示
        /// </summary>
        public static string ShowExportDialog(string defaultName = "mask")
        {
            return EditorUtility.SaveFilePanel(
                "Export Mask",
                "Assets",
                defaultName,
                "png"
            );
        }

        /// <summary>
        /// Import用のファイルダイアログを表示
        /// </summary>
        public static string ShowImportDialog()
        {
            return EditorUtility.OpenFilePanel(
                "Import Mask",
                "Assets",
                "png"
            );
        }
    }
}
