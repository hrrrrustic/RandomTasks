using System;
using System.IO;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var folder = await LocalFolder.Initialize(@"D:\VSProjects");
            folder.GetFileNames();
        }
    }
}
