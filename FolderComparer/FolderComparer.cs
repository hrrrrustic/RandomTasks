using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class FolderComparer
    {
        private LocalFolder _firstFolder;
        private LocalFolder _secondFolder;

        public FolderComparer(String firstFolderPath, String secondFolderPath)
        {
            Task firstFolderInitialize = Task.Run(() => {_firstFolder = new LocalFolder(firstFolderPath);});
            Task secondFolderInitialize = Task.Run(() => { _secondFolder = new LocalFolder(secondFolderPath); });
            Task.WaitAll(firstFolderInitialize, secondFolderInitialize);
        }

        public void CompareFolders()
        {
            _secondFolder.
        }
    }
}