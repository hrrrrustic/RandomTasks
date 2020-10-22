using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Files
{
    public class LocalFile : ILocalFile
    {
        private readonly FileInfo _fileInfo;
        public LocalFile(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }
        public IHashedFlie HashFile()
        {
            throw new NotImplementedException();
        }
    }
}
