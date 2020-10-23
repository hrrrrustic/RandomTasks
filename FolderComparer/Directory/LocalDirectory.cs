using FolderComparer.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FolderComparer
{
    public class LocalDirectory : IDirectory<ILocalFile>, IEnumerable<ILocalFile>
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
                yield return new LocalFile(file);

            foreach (var subDirectory in _directoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                foreach (var file in subDirectory.EnumerateFiles())
                {
                    yield return new LocalFile(file);
                }
            }
        }

        public IEnumerator<ILocalFile> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
