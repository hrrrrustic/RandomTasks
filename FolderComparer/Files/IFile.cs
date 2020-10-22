using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IFile
    {
    }

    public interface ILocalFile : IFile 
    { 
        String Path { get; }
    }
}
