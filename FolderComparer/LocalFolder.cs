using System;
using System.IO;
using System.Linq;

namespace FolderComparer
{
    public class LocalFolder
    {
        public readonly LocalFile[] Files;
        public readonly LocalFolder[] InnerFolders;
        public readonly String Path;
        public readonly Guid FolderId;

        public LocalFolder(String path)
        {
            Path = path;
            FolderId = Guid.NewGuid();
            Files = InitializeFiles();
            //_innerFolders = InitializeSubFolders();
            InnerFolders = Array.Empty<LocalFolder>();
        }

        private LocalFile[] InitializeFiles() =>
            Directory
                .GetFiles(Path)
                .Select(k => new LocalFile(k, FolderId))
                .ToArray();

        private LocalFolder[] InitializeSubFolders() =>
            Directory.GetDirectories(Path)
                .Select(k => new LocalFolder(k))
                .ToArray();


        public LocalFile[] GetFiles()
        {
            return Files
                .Union(InnerFolders
                    .SelectMany(k => k.Files))
                .ToArray();
        }
    }
}