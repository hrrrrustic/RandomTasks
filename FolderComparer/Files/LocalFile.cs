using System.IO;

namespace FolderComparer.Files
{
    public class LocalFile : ILocalFile
    {
        private readonly FileInfo _fileInfo;
        public string Path => _fileInfo.FullName;
        public LocalFile(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }
    }
}
