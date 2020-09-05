using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalFolder
    //TODO : хешить папки
    {
        private LocalFile[] _files;
        private LocalFolder[] _innerFolders;
        public Byte[] FolderHash;
        private readonly String _path;

       public LocalFolder(String path)
        {
            _path = path;
            InitializeFiles();
            InitializeSubFolders();
        }

        private void InitializeFiles()
        {
            String[] files = Directory.GetFiles(_path);
            _files = files
                .Select(k => new LocalFile(k))
                .ToArray();
        }

        private void InitializeSubFolders()
        {
            String[] directories = Directory.GetDirectories(_path);
            _innerFolders = directories
                .Select(k => new LocalFolder(k))
                .ToArray();
        }

        public String[] GetFileNames()
        {
            // TODO : убрать жесть
            return _files
                .Select(k => k.FilePath)
                .Union(
                    _innerFolders
                        .Select(k => k.GetFileNames())
                        .SelectMany(k => k))
                .ToArray();
        }
    }
}