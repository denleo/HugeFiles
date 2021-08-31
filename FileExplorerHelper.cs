using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HugeFiles
{
    internal static class FileExplorerHelper
    {
        public static FileInfo[] StartSearch(string rootDir, int count = 10)
        {
            if (!Directory.Exists(rootDir))
            {
                throw new ArgumentException("The path does not exist!", nameof(rootDir));
            }

            var filesList = FileCollector.GetFiles(rootDir);
            return filesList.AsParallel<FileInfo>()  // PLINQ
                .OrderByDescending(x => x.Length)
                .Take(count).ToArray();
        }


        public static Task<FileInfo[]> StartSearchAsync(string rootDir, int count = 10)
        {
            var tcs = new TaskCompletionSource<FileInfo[]>();
            var factory = new TaskFactory();
            factory.StartNew(() =>
            {
                try
                {
                    var result = StartSearch(rootDir, count);
                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }
    }
}
