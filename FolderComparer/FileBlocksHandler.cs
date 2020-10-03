using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Blocks;
using FolderComparer.Files;
using FolderComparer.Folders;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class FileBlocksHandler
    {
        public readonly BlockingCollection<FileBlock> Blocks;
        private readonly BlockingCollection<HashedFileBlock> HashedBlocks = new(new ConcurrentQueue<HashedFileBlock>());

        public FileBlocksHandler(BlockingCollection<FileBlock> blocks)
        {
            Blocks = blocks;
        }

        public void HandleBlocks(CancellationToken cancellation)
        {
            try
            {
                InternalHandleBlocks(cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void InternalHandleBlocks(CancellationToken cancellation)
        {
            List<Task> hashTasks = new();
            while (!Blocks.IsCompleted)
            {
                cancellation.ThrowIfCancellationRequested();
                FileBlock currentBlock;
                try
                {
                    currentBlock = Blocks.Take(cancellation);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                hashTasks.Add(Task.Run(() => HashBlock(currentBlock)));
            }

            Task.WaitAll(hashTasks.ToArray());
        }
        private void HashBlock(FileBlock block)
        {
            HashedFileBlock hashedBlock = block.HashBlock(SHA512.Create());
            HashedBlocks.Add(hashedBlock);
        }

        #region НеНадоСюдаСмотреть...
        public Dictionary<Guid, HashedLocalFolder> BuildFolders() 
            => HashedBlocks
                .ToList()
                .GroupBy(k => k.LocalFileInfo.FolderId)
                .ToDictionary(e => e.Key, x => x
                    .GroupBy(y => y.LocalFileInfo.FileId)
                    .ToDictionary(y => y.Key, j => j
                        .OrderBy(f => f.BlockNumber)
                        .ToList())
                    .Select(f => MergeBlocksHash(f.Value))
                    .OrderBy(f => Encoding.Default.GetString(f.Hash)) // TODO : Это костыль т.к. хеш у меня считается через жопу. Мне нужно гарантировать одинаковый порядок, а это единственный способ.
                    .ToHashedFolder());
        #endregion

        public static HashedLocalFile MergeBlocksHash(List<HashedFileBlock> blocks)
        {
            LocalFileInfo info = blocks[0].LocalFileInfo;
            Byte[] hash = blocks[0].Hash;

            if (blocks.Count == 1)
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
    }
}