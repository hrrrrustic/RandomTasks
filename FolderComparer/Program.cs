using System;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class Program
    {
        public static void Main()
        {
            String firstPath = Console.ReadLine();
            String secondPath = Console.ReadLine();

            try
            {
                FolderCompareResult res = new FolderComparer()
                    .Compare(firstPath, secondPath);

                Console.WriteLine(res);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
