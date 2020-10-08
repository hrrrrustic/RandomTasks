using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Blocks;
using FolderComparer.Folders;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class FileBlocksHandler : IDisposable
    {
        public readonly BlockingCollection<FileBlock> Blocks;
        private readonly BlockingCollection<HashedFileBlock> HashedBlocks = new(new ConcurrentQueue<HashedFileBlock>());
        private readonly HashAlgorithm _hashAlgorithm;
        public FileBlocksHandler(BlockingCollection<FileBlock> blocks, HashAlgorithm hashAlgorithm) => (Blocks, _hashAlgorithm) = (blocks, hashAlgorithm);

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
            block.Dispose();
            HashedBlocks.Add(hashedBlock);
        }

        #region НеНадоСюдаСмотреть...
        public Dictionary<Guid, HashedLocalFolder> BuildFolders()
        {
            var hashedFiles = HashedBlocks
                        .GroupBy(k => k.LocalFileInfo.FolderId)
                        .ToDictionary(k => k.Key, x => x
                            .GroupBy(y => y.LocalFileInfo.FileId)
                            .ToDictionary(x => x.Key, y => y
                                .OrderBy(e => e.BlockNumber)
                                .ToList()
                                .ToHashedFile(_hashAlgorithm)));

            return hashedFiles
                .ToDictionary(k => k.Key, x => x
                    .Value
                    .Select(k => k.Value)
                    .GroupBy(x => x.Hash)
                    .ToDictionary(x => new DictionaryKeyHash(x.Key), y => y
                        .OrderBy(e => Encoding.Default.GetString(e.Hash)) // TODO : Это костыль т.к. хеш у меня считается через жопу. Мне нужно гарантировать одинаковый порядок, а больше никак и не засортировать
                        .ToList())
                    .ToHashedFolder(_hashAlgorithm));
                  
        }
        #endregion

        public static HashedLocalFile MergeBlocksHash(IReadOnlyList<HashedFileBlock> blocks, HashAlgorithm hashAlgorithm)
        {
            LocalFileInfo info = blocks[0].LocalFileInfo;
            Byte[] hash = new Byte[hashAlgorithm.HashSize];

            foreach (HashedFileBlock block in blocks)
                hash = hashAlgorithm.ComputeHash(hash.Concat(block.Hash).ToArray());

            return new HashedLocalFile(info, hash);
        }

        public static HashedLocalFolder MergeFilesHash(Dictionary<DictionaryKeyHash, List<HashedLocalFile>> files, HashAlgorithm hashAlgorithm)
        {
            Byte[] hash = new Byte[hashAlgorithm.HashSize];

            foreach (var file in files)
                for (int i = 0; i < file.Value.Count; i++)
                    hash = hashAlgorithm.ComputeHash(hash.Concat(file.Key.ByteHash).ToArray());

            return new HashedLocalFolder(files, hash);
        }

        public void Dispose()
        {
            HashedBlocks?.Dispose();
            Blocks?.Dispose();
            _hashAlgorithm.Dispose();
        }
    }
}