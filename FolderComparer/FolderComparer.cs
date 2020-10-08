using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using FolderComparer.Files;
using FolderComparer.Folders;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class FolderComparer
    {
        public FolderCompareResult Compare(String firstFolderPath, String secondFolderPath)
            => Compare(new LocalFolder(firstFolderPath), new LocalFolder(secondFolderPath));

        public FolderCompareResult Compare(LocalFolder firstFolder, LocalFolder secondFolder)
        {
            if (firstFolder.IsEmpty || secondFolder.IsEmpty)
            {
                if (firstFolder.IsEmpty && secondFolder.IsEmpty)
                    return FolderCompareResult.IdenticalFoldersResult;

                var notEmptyFolder = firstFolder;

                if (!firstFolder.IsEmpty)
                    notEmptyFolder = secondFolder;

                var firstFolderFiles = notEmptyFolder
                    .GetFiles()
                    .Select(k => k.FileInfo.FilePath)
                    .ToList();

                return new FolderCompareResult(new List<(String, String)>(0), firstFolderFiles);
            }

            var allFiles = GetAllFiles(firstFolder, secondFolder);
            Dictionary<Guid, HashedLocalFolder> folders = PrepareFolders(allFiles);

            HashedLocalFolder firstHashedFolder = folders[firstFolder.FolderId];
            HashedLocalFolder secondHashedFolder = folders[secondFolder.FolderId];

            if (firstHashedFolder == secondHashedFolder)
                return FolderCompareResult.IdenticalFoldersResult;

            return firstHashedFolder.CompareTo(secondHashedFolder);
        }

        private Dictionary<Guid, HashedLocalFolder> PrepareFolders(IReadOnlyCollection<LocalFile> files)
        {
            SingleThreadFileBlocksReader singleThreadFileBlocksReader = new(files);

            Thread readThread = new(singleThreadFileBlocksReader.StartReading);
            readThread.Start();

            FileBlocksHandler handler = new(singleThreadFileBlocksReader.ReadedBlocks, SHA512.Create());

            CancellationTokenSource cancellationSource = new();

            Thread handleThread = new Thread(() => handler.HandleBlocks(cancellationSource.Token));
            handleThread.Start();
            readThread.Join();

            if (!singleThreadFileBlocksReader.IsSuccessfullyFinished)
            {
                cancellationSource.Cancel();
                throw new CompareFolderException("One of folders was changed");
            }

            handleThread.Join();

            return handler.BuildFolders();
        }

        IReadOnlyCollection<LocalFile> GetAllFiles(LocalFolder firstFolder, LocalFolder secondFolder) 
            => firstFolder
                .GetFiles()
                .Concat(secondFolder.GetFiles())
                .ToList();
    }
}