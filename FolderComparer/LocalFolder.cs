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

       public LocalFolder(String path)
        {
            _path = path;
            Task initFiles = Task.Run(InitializeFiles);
            Task initFolders = Task.Run(InitializeSubFolders);
            Task.WaitAll(initFiles, initFolders);
        }

        private void InitializeFiles()
        {
            String[] files = Directory.GetFiles(_path);
            _files = files
                .Select(k => new LocalFile(k))
                .ToArray();
        }

        private void InitializeSubFolders()
        {
            String[] directories = Directory.GetDirectories(_path);
            _innerFolders = directories
                .Select(k => new LocalFolder(k))
                .ToArray();
        }

        public void GetFileNames()
        {
            _files.ToList().ForEach(k => Console.WriteLine(k.FilePath));
            _innerFolders.ToList().ForEach(k => k.GetFileNames());
        }
    }
}