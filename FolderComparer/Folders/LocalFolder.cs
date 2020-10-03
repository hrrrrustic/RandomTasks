using System;
using System.IO;
using System.Linq;
using FolderComparer.Files;

namespace FolderComparer.Folders
{
    public class LocalFolder
    {
        public readonly LocalFile[] Files;
        public readonly LocalFolder[] InnerFolders;
        public readonly String Path;
        public readonly Guid FolderId;
        public Boolean IsEmpty => InnerFolders.Length == 0 && Files.Length == 0;
        public LocalFolder(String path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory {path} doesn't exist");
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