using FolderComparer.Tools;

namespace FolderComparer
{
    public interface IDirectoryComparer<TFirstDirectory, TSecondDirectory>
        where TFirstDirectory : IDirectory
        where TSecondDirectory : IDirectory
    {
        DirectoryCompareResult Compare(TFirstDirectory x, TSecondDirectory y);
    }
}
