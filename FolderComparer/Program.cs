using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class Program
    {
        public static void Main()
        { 
            FolderCompareResult res = new FolderComparer()
                .Compare("D:\\VSProjects\\OpenSource\\runtime\\src\\coreclr\\src\\jit", "D:\\VSProjects\\OpenSource\\runtime\\src\\coreclr\\src\\jit");

            Console.WriteLine(res);
        }
    }
}
