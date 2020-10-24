using FolderComparer.Tools;
using System.Collections.Generic;

namespace FolderComparer
{
    public interface IHashedDirectory
    {
        byte[] Hash { get; }
        IReadOnlyCollection<IHashedFile> HashedFiles { get; }
        DirectoryCompareResult Compare(IHashedDirectory another);
    }
}
