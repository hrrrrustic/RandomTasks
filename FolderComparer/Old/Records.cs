using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public record LocalFileInfo(String FilePath, Guid FolderId, Guid FileId);
    public record HashedLocalFile(LocalFileInfo LocalFile, Byte[] Hash);
}
