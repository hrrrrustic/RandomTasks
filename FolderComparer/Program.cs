using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class Program
    {
        static void Main(string[] args)
        {
            new FolderComparer().Compare(@"D:\VSProjects", "");
        }
    }
}
