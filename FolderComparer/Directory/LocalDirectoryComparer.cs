using FolderComparer.Files;
using FolderComparer.Pipes;
using FolderComparer.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalDirectoryComparer : IDirectoryComparer<LocalDirectory, ILocalFile, LocalDirectory, ILocalFile>
    {
        public DirectoryCompareResult Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return Compare(new LocalDirectory(x), new LocalDirectory(y));
        }

        public DirectoryCompareResult Compare(LocalDirectory x, LocalDirectory y)
        {
            //var reader = new FileEnumator();
            //PipeAction<ILocalFile> pipeAction = new PipeAction<ILocalFile>(reader);
            //var pipeline = Pipeline
            //    .Start<ILocalFile>(pipeAction);

            throw new Exception();
        }
    }
}