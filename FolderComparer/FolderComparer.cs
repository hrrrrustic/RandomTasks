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
            SingleThreadFileBlocksReader singleThreadFileBlocksReader = SingleThreadFileBlocksReader.GetInstance();

            FillFilesToReader(firstFolder.GetFiles(), singleThreadFileBlocksReader);
            FillFilesToReader(secondFolder.GetFiles(), singleThreadFileBlocksReader);
        }

        private void FillFilesToReader(LocalFile[] files, SingleThreadFileBlocksReader reader)
        {
            foreach (LocalFile file in files)
                Task.Run(() => reader.QueuedFiles.Add(file));
        }

        private void InitializeComparing() { }
    }
}