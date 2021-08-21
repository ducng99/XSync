using System;
using System.Collections.Generic;
using System.IO;

namespace XSync
{
    class App
    {
        private readonly List<string> FoldersPaths;

        public App(List<string> paths)
        {
            FoldersPaths = paths;
        }

        public void Sync()
        {
            for (int i = 1; i < FoldersPaths.Count; i++)
            {
                Sync2Folders(FoldersPaths[0], FoldersPaths[i]);
            }
        }

        private static void Sync2Folders(string folder1, string folder2)
        {
            Dictionary<string, (DateTime, byte)> LatestFiles = new();

            var filesFolder1 = Directory.EnumerateFiles(folder1, "*", SearchOption.AllDirectories);
            
            foreach (string file in filesFolder1)
            {
                string relaPath = Path.GetRelativePath(folder1, file);
                DateTime modifiedTime = File.GetLastWriteTime(file);
                LatestFiles.Add(relaPath, (modifiedTime, 0));
            }

            var filesFolder2 = Directory.EnumerateFiles(folder2, "*", SearchOption.AllDirectories);

            foreach (string file in filesFolder2)
            {
                string relaPath = Path.GetRelativePath(folder2, file);
                DateTime modifiedTime = File.GetLastWriteTime(file);

                if (!LatestFiles.ContainsKey(relaPath) || DateTime.Compare(LatestFiles[relaPath].Item1, modifiedTime) < 0)
                {
                    LatestFiles[relaPath] = (modifiedTime, 1);
                }
            }

            foreach ((var relaPath, (var date, var index)) in LatestFiles)
            {
                if (index == 0)
                {
                    var destFile = Path.Combine(folder2, relaPath);
                    new FileInfo(destFile).Directory.Create();

                    File.Copy(Path.Combine(folder1, relaPath), destFile, true);
                }
                else if (index == 1)
                {
                    var destFile = Path.Combine(folder1, relaPath);
                    new FileInfo(destFile).Directory.Create();

                    File.Copy(Path.Combine(folder2, relaPath), destFile, true);
                }
            }
        }
    }
}
