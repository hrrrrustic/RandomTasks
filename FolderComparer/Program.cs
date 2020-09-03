using System;
using System.IO;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var folder = new LocalFolder(@"D:\VSProjects");
            folder.GetFileNames();
        }
    }
}
