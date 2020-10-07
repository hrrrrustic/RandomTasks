using System;
using System.Collections.Generic;
using System.Linq;
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
            if (firstFolder.IsEmpty && secondFolder.IsEmpty)
                return FolderCompareResult.IdenticalFoldersResult;

            var allFiles = GetAllFiles(firstFolder, secondFolder);
            Dictionary<Guid, HashedLocalFolder> folders = PrepareFolders(allFiles);

            HashedLocalFolder firstHashedFolder = folders[firstFolder.FolderId];
            HashedLocalFolder secondHashedFolder = folders[secondFolder.FolderId];

            if (firstHashedFolder == secondHashedFolder)
                return FolderCompareResult.IdenticalFoldersResult;

            List<(String, String)> matches = new();
            List<String> differences = new();

            firstHashedFolder
                .Union(secondHashedFolder)
                .GroupBy(k => k.Hash)
                .ToList()
                .ForEach(k =>
                {
                    List<HashedLocalFile> files = k.ToList();

                    if (files.Count == 2)
                        matches.Add((files[0].LocalFile.FilePath, files[1].LocalFile.FilePath));
                    else
                        differences.Add(files[0].LocalFile.FilePath);
                });

            return new FolderCompareResult(matches, differences);
        }

        private Dictionary<Guid, HashedLocalFolder> PrepareFolders(IReadOnlyCollection<LocalFile> files)
        {
            SingleThreadFileBlocksReader singleThreadFileBlocksReader = new(files);

            Thread readThread = new(singleThreadFileBlocksReader.StartReading);
            readThread.Start();

            FileBlocksHandler handler = new(singleThreadFileBlocksReader.ReadedBlocks);

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