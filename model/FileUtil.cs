using ImageMagick;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_Practice.Interfaces;
using WPF_Practice.model;
using WinForms = System.Windows.Forms;

namespace WPF_Practice
{
    public static class FileUtil
    {

        public static BitmapImage CreateBitmapPath(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnDemand;
            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        /// <summary>
        /// bitmapimage class with stream
        /// </summary>
        public static async Task getImageFilesAsync_BitmapImage(List<string> directory, Action<List<BitmapImage>> callback)
        {
            List<BitmapImage> result = new List<BitmapImage>();
            for (int i = 0; i < directory.Count; i++)
            {
                using (FileStream fs = new FileStream(directory[i], FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    byte[] source = new byte[fs.Length];
                    Memory<byte> sourceView = source;
                    await fs.ReadExactlyAsync(source);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnDemand;
                    bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                    //gdi+의 api를 호출하는데 gdi+의 LoadImageFromStream은 이미지를 사용하는 동안 stream이 열려 있어야 한다.
                    bitmap.StreamSource = new MemoryStream(source);
                    bitmap.EndInit();
                    bitmap.Freeze();
                    result.Add(bitmap);
                }
            }
            callback(result);
        }

        public static string GetTitleFilePath(string fullPath)
        {
            var e = Directory.EnumerateFileSystemEntries(fullPath, "*", new EnumerationOptions { AttributesToSkip = FileAttributes.None });
            var list =e.ToImmutableList().Sort(NaturalCompare.CompareOrdinal);
            for(int i= 0; i < list.Count; i++)
            {
                if (WorkSpaceScanner.s_imageRegex.IsMatch(list[i]))
                {
                    return list[i];
                }
            }
            return string.Empty;
        }


        #region imagemagick
        public static async Task getTunmnail(List<string> directory, Action<ImageSource[]> callback)
        {
            ImageSource[] result = new ImageSource[directory.Count];
            Task[] tasks = new Task[directory.Count];
            for (int index = 0; index < directory.Count; index++)
            {
                tasks[index] = readFileCallBack_Thumnail(directory[index], result, index);
            }
            await Task.WhenAll(tasks);
            callback(result);

        }
        private static async Task readFileCallBack_Thumnail(string path, ImageSource[] sources, int index)
        {
            using var image = new MagickImage();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                await image.ReadAsync(fs);
                image.Format = MagickFormat.Bmp;
                IMagickGeometry geometry = new MagickGeometry();
                image.Thumbnail(200, 200);
                ImageSource t = ImageMagick.IMagickImageExtentions.ToBitmapSource(image);
                t.Freeze();
                sources[index] = t;
            }
        }
        #endregion


        public class UsedWinform : IWinformUsing
        {
            public static string? openDirectoryBrowser()
            {
                WinForms.FolderBrowserDialog open = new WinForms.FolderBrowserDialog();
                open.InitialDirectory = Environment.CurrentDirectory;
                if (open.ShowDialog() != WinForms.DialogResult.OK)
                {
                    return null;
                }
                return open.SelectedPath;
            }
        }
    }
}
