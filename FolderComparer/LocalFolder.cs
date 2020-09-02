using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalFolder
    {
        private LocalFile[] _files;
        private LocalFolder[] _innerFolders;

        private readonly String _path;

        private LocalFolder(String path)
        {
            _path = path;
        }

        public static async Task<LocalFolder> Initialize(String path)
        {
            LocalFolder folder = new LocalFolder(path);
            Task initFiles = Task.Run(() => folder.InitializeFiles());
            Task initFolders = Task.Run(() => folder.InitializeSubFolders());
            await Task.WhenAll(initFiles, initFolders);

            return folder;
        }

        private void InitializeFiles()
        {
            String[] files = Directory.GetFiles(_path);
            _files = files
                .AsParallel()
                .Select(k => new LocalFile(k))
                .ToArray();
        }

        private async Task InitializeSubFolders()
        {
            String[] directories = Directory.GetDirectories(_path);
            IEnumerable<Task<LocalFolder>> initializeInnerFolders = directories.Select(Initialize);
            _innerFolders = await Task.WhenAll(initializeInnerFolders);
        }

        public void GetFileNames()
        {
            _files.ToList().ForEach(k => Console.WriteLine(k.FilePath));
            _innerFolders.ToList().ForEach(k => k.GetFileNames());
        }
    }
}