using FolderComparer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IDirectoryComparer<TFirstDirectory, TFirstFile, TSecondDirectory, TSecondFile>
        where TFirstDirectory : IDirectory<TFirstFile>
        where TFirstFile : IFile
        where TSecondDirectory : IDirectory<TSecondFile>
        where TSecondFile : IFile
    {
        DirectoryCompareResult Compare(TFirstDirectory x, TSecondDirectory y);
    }
}
