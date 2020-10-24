using System;
using System.IO;

namespace FolderComparer
{
    public static class Program
    {
        public static void Main()
        {
            String? firstPath = Console.ReadLine();
            String? secondPath = Console.ReadLine();

            var validateResult = new DirectoryPathValidator().Validate(firstPath, secondPath);

            if (!validateResult.IsValid)
            {
                Console.WriteLine(validateResult.Message);
                return;
            }
            DirectoryInfo firstDirectory = new DirectoryInfo(firstPath!);
            DirectoryInfo secondDirectory = new DirectoryInfo(secondPath!);
            try
            {
                var result = new LocalDirectoryComparer().Compare(firstDirectory, secondDirectory);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}