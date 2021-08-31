using System;
using System.Collections.Generic;
using System.IO;

namespace HugeFiles
{
    internal static class FileCollector
    {
        private const int Capacity = 150000;
        private static readonly EnumerationOptions EnumerationOptions = new()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false
        };

        public static IEnumerable<FileInfo> GetFiles(string rootDir)
        {
            if (!Directory.Exists(rootDir))
            {
                throw new ArgumentException("The path does not exist!", nameof(rootDir));
            }

            var list = new List<FileInfo>(Capacity);
            var rootDirInfo = new DirectoryInfo(rootDir);
            RecursiveAdd(rootDirInfo, list);
            return list;
        }

        private static void RecursiveAdd(DirectoryInfo directoryInfo, List<FileInfo> fileList)
        {
            var files = directoryInfo.GetFiles("*.*", EnumerationOptions);
            fileList.AddRange(files);

            var catalogs = directoryInfo.GetDirectories("*", EnumerationOptions);

            if (catalogs.Length == 0)
            {
                return;
            }
            else
            {
                foreach (var catalog in catalogs)
                {
                    RecursiveAdd(catalog, fileList);
                }
            }
        }
    }
}
