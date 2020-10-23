using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Old
{
    public record LocalFileInfo(String FilePath, Guid FolderId, Guid FileId);
}