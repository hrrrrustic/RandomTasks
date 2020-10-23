using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IHashedFile
    {
        byte[] Hash { get; }
        Guid FolderId { get; }
    }
}
