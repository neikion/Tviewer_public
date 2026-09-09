using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using Tviewer.model;

namespace Tviewer
{
    public static class FileUtil
    {
        private static object DebugLogObj=new object();
        
        /// <summary>
        /// return first image of directory
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetTitleFilePath(string fullPath)
        {
            var e = Directory.EnumerateFileSystemEntries(fullPath, "*", new EnumerationOptions { AttributesToSkip = FileAttributes.None });
            var list = e.ToImmutableList().Sort(NaturalCompare.CompareOrdinal);
            for (int i = 0; i < list.Count; i++)
            {
                if (WorkSpaceScanner.s_imageRegex.IsMatch(list[i]))
                {
                    return list[i];
                }
            }
            return string.Empty;
        }

        private static OpenFolderDialog FolderDialog = new OpenFolderDialog()
        {
            Multiselect = true,
            InitialDirectory = Environment.CurrentDirectory,
            ShowHiddenItems = true,
            DereferenceLinks = true
        };

        public static void Log(string text)
        {
            lock (DebugLogObj)
            {
                using FileStream fileStream = new FileStream(Environment.CurrentDirectory + "/Log.txt", FileMode.OpenOrCreate);
                fileStream.Seek(0, SeekOrigin.End);
                using StreamWriter streamWriter = new StreamWriter(fileStream);
                DateTime time = DateTime.UtcNow;
                streamWriter.WriteLine();
                streamWriter.WriteLine($"Time : {time.ToString("HH:mm:ss")} ({time.Ticks})");
                streamWriter.WriteLine($"OS : {Environment.OSVersion}");
                streamWriter.WriteLine(text);
                streamWriter.Flush();
            }
        }

        public static List<string> OpenDirectoryBrowser()
        {
            List<string>? data=null;
            if (FolderDialog.ShowDialog() == true)
            {
                if (FolderDialog.FolderNames.Length > 0)
                {
                    data = new List<string>(FolderDialog.FolderNames.Length);
                    for (int i = 0; i < FolderDialog.FolderNames.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(FolderDialog.FolderNames[i]))
                        {
                            data.Add(FolderDialog.FolderNames[i]);
                        }
                    }
                }
            }
            if (data == null) data = new List<string>();
            return data;
        }


        public static string FileSizeToString(long value)
        {
            string suffix;
            double readable;
            switch (Math.Abs(value))
            {
                case >= 0x1000000000000000:
                    suffix = "EiB";
                    readable = value >> 50;
                    break;
                case >= 0x4000000000000:
                    suffix = "PiB";
                    readable = value >> 40;
                    break;
                case >= 0x10000000000:
                    suffix = "TiB";
                    readable = value >> 30;
                    break;
                case >= 0x40000000:
                    suffix = "GiB";
                    readable = value >> 20;
                    break;
                case >= 0x100000:
                    suffix = "MiB";
                    readable = value >> 10;
                    break;
                case >= 0x400:
                    suffix = "KiB";
                    readable = value;
                    break;
                default:
                    return value.ToString("0 B");
            }

            return (readable / 1024).ToString("0.## ", CultureInfo.InvariantCulture) + suffix;
        }
    }
}
