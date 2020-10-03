using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Blocks;
using FolderComparer.Files;
using FolderComparer.Folders;
using FolderComparer.Tools;
using ThreadState = System.Diagnostics.ThreadState;

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

            var allFiles = firstFolder
                .GetFiles()
                .Concat(secondFolder.GetFiles())
                .ToArray();

            SingleThreadFileBlocksReader singleThreadFileBlocksReader = new(allFiles);

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
                throw new CompareFolderException("One of folder was changed");
            }

            handleThread.Join();


            Dictionary<Guid, HashedLocalFolder> folders = handler.BuildFolders();
           

            HashedLocalFolder firstHashedFolder = folders[firstFolder.FolderId];
            HashedLocalFolder secondHashedFolder = folders[secondFolder.FolderId];
            
            if(firstHashedFolder == secondHashedFolder)
                return FolderCompareResult.IdenticalFoldersResult;

            List<(String, String)> matches = new();
            List<String> differences = new();

            firstHashedFolder
                .HashedFiles
                .Union(secondHashedFolder.HashedFiles)
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
    }
}