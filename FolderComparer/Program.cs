using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class Program
    {
        public static void Main()
        {
            var res = new FolderComparer().Compare("D:\\VSProjects\\MyRandomProjects\\JustR\\JustR\\JustR.Core\\Extensions", "D:\\VSProjects\\MyRandomProjects\\JustR\\JustR\\JustR.Core");

            Console.WriteLine(res);
        }
    }
}
