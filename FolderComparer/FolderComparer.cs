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
    public static class HashExtensions
    {
        public static HashedLocalFolder ToHashedFolder(this IEnumerable<HashedLocalFile> files) => FolderComparer.MergeFilesHash(files.ToList());
    }
    public class FolderComparer
    {
        public FolderCompareResult Compare(String firstFolderPath, String secondFolderPath) 
            => Compare(new LocalFolder(firstFolderPath), new LocalFolder(secondFolderPath));

        public FolderCompareResult Compare(LocalFolder firstFolder, LocalFolder secondFolder)
        {
            FileBlockPool pool = new FileBlockPool();
            Task handleTask = Task.Run(() => pool.HandleBlocks());

            SingleThreadFileBlocksReader singleThreadFileBlocksReader = SingleThreadFileBlocksReader.GetInstance();
            Task.Run(() => singleThreadFileBlocksReader.StartReading());

            var allFiles = firstFolder
                .GetFiles()
                .Concat(secondFolder.GetFiles())
                .ToArray();

            FillFilesToReader(allFiles, singleThreadFileBlocksReader);

            handleTask.Wait();

            var folders = pool
                .HashedBlocks
                .ToList()
                .GroupBy(k => k.LocalFileInfo.FolderId)
                .ToDictionary(e => e.Key, x => x
                    .GroupBy(y => y.LocalFileInfo.FileId)
                    .ToDictionary(y => y.Key, j => j
                        .OrderBy(f => f.BlockNumber)
                        .ToList())
                    .Select(f => MergeBlocksHash(f.Value))
                    .OrderBy(f => Encoding.Default.GetString(f.Hash))
                    .ToHashedFolder());

            var folder1 = folders[firstFolder.FolderId];
            var folder2 = folders[secondFolder.FolderId];
            
            if(folder1 == folder2)
                return FolderCompareResult.IdenticalFoldersResult;

            List<(String, String)> matches = new List<(String, String)>();
            List<String> differences = new List<String>();

            folder1
                .HashedFiles
                .Union(folder2.HashedFiles)
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

        public static HashedLocalFile MergeBlocksHash(List<HashedFileBlock> blocks)
        {
            LocalFileInfo info = blocks[0].LocalFileInfo;
            Byte[] hash = blocks[0].Hash;

            if(blocks.Count == 1)
                return new HashedLocalFile(info, hash);

            foreach (HashedFileBlock block in blocks.Skip(1))
                hash = SHA512.HashData(hash.Concat(block.Hash).ToArray());

            return new HashedLocalFile(info, hash);
        }

        public static HashedLocalFolder MergeFilesHash(List<HashedLocalFile> files)
        {
            Byte[] hash = files[0].Hash;

            foreach (HashedLocalFile file in files.Skip(1))
                hash = SHA512.HashData(hash.Concat(file.Hash).ToArray());

            return new HashedLocalFolder(files.ToArray(), hash);
        }

        private void FillFilesToReader(LocalFile[] files, SingleThreadFileBlocksReader reader)
        {
            foreach (LocalFile file in files)
                reader.QueuedFiles.Add(file);

            reader.QueuedFiles.CompleteAdding();
        }
    }
}