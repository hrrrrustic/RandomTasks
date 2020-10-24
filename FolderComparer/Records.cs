using System;

namespace FolderComparer
{
    public record FileInfo(String FilePath, Guid FolderId, Guid FileId);
}