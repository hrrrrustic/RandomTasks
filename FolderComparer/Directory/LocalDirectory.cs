using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalDirectory : IDirectory<ILocalFile>
    {
        private readonly DirectoryInfo _directoryInfo;
        public LocalDirectory(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }
        public bool IsEmpty => throw new NotImplementedException();
        public string Path => _directoryInfo.FullName;

        public IEnumerable<ILocalFile> Enumerate()
        {
            foreach (var file in _directoryInfo.EnumerateFiles())
                yield return default;

            foreach (var subDirectory in _directoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
                foreach (var file in subDirectory.EnumerateFiles())
                    yield return default;
        }
    }
}
