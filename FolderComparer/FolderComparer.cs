using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FolderComparer.Blocks;
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
            SingleThreadFileBlocksReader singleThreadFileBlocksReader = SingleThreadFileBlocksReader.GetInstance();

            FileBlocksHandler handler = new(singleThreadFileBlocksReader.ReadedBlocks);
            Task handleTask = Task.Run(() => handler.HandleBlocks());

            Task.Run(() => singleThreadFileBlocksReader.StartReading());

            var allFiles = firstFolder
                .GetFiles()
                .Concat(secondFolder.GetFiles())
                .ToArray();

            FillFilesToReader(allFiles, singleThreadFileBlocksReader);

            handleTask.Wait();
            
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

       

        private void FillFilesToReader(LocalFile[] files, SingleThreadFileBlocksReader reader)
        {
            foreach (LocalFile file in files)
                reader.QueuedFiles.Add(file);

            reader.QueuedFiles.CompleteAdding();
        }
    }
}