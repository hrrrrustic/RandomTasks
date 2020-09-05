using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class FolderComparer
    {
        //TODO : Сравнивать просто по хешам и количеству файлов
        public void Compare(String firstFolderPath, String secondFolderPath) 
            => Compare(new LocalFolder(firstFolderPath), new LocalFolder(secondFolderPath));

        public void Compare(LocalFolder firstFolder, LocalFolder secondFolder)
        {
            FileBlockPool pool = new FileBlockPool();
            SingleThreadFileBlocksReader singleThreadFileBlocksReader = new SingleThreadFileBlocksReader(pool.Blocks);

            FillFilesToReader(firstFolder.GetFileNames(), singleThreadFileBlocksReader);
            FillFilesToReader(secondFolder.GetFileNames(), singleThreadFileBlocksReader);
        }

        private void FillFilesToReader(String[] filePaths, SingleThreadFileBlocksReader reader)
        {
            foreach (String filePath in filePaths)
                Task.Run(() => reader.FilePaths.Add(filePath));
        }

        private void InitializeComparing() { }
    }
}