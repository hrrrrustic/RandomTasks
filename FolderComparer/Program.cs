using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using FolderComparer.Folders;
using FolderComparer.Pipes;
using FolderComparer.Tools;

namespace FolderComparer
{
    public class Program
    {
        public static void Main()
        {
            TestPipeline();
            return;
            String? firstPath = Console.ReadLine();
            String? secondPath = Console.ReadLine();

            var validateResult = new DirectoryPathValidator().Validate(firstPath, secondPath);

            if (!validateResult.IsValid)
            {
                Console.WriteLine(validateResult.Message);
                return;
            }
            DirectoryInfo firstDirectory = new DirectoryInfo(firstPath);
            DirectoryInfo secondDirectory = new DirectoryInfo(secondPath);
            
            try
            {
                var result = new LocalDirectoryComparer().Compare(firstDirectory, secondDirectory);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void TestPipeline()
        {
            var pipeline = Pipeline
                .Start<int>(output => new NumberGenerator(output))
                .ContinueWith<int>(SquareNumbers)
                .ContinueWith<string>((input, output) => new Converter(input, output))
                .FinishWith(GetOneString);

            var res = pipeline.GetResult();
            Console.WriteLine(res);
        }

        public static void GenerateNumbers(IAddingCompletableCollection<int> destination)
        {
          
        }

        public static void SquareNumbers(IAddingCompletableCollection<int> source, IAddingCompletableCollection<int> destination)
        {
            while(!source.IsEmpty)
            {
                var item = source.GetItem();
                destination.PushItem(item * item);
            }

            destination.CompletePushing();
        }

        public static void ConvertToReverseString(IAddingCompletableCollection<int> source, IAddingCompletableCollection<string> destination)
        {
            string res = string.Empty;

            while (!source.IsEmpty)
            {
                var strinItem = source.GetItem().ToString();
                var item = new string(strinItem.Reverse().ToArray());
                destination.PushItem(item);
            }

            destination.CompletePushing();
        }

        public static string GetOneString(IAddingCompletableCollection<string> source)
        {
            string res = string.Empty;

            while(!source.IsEmpty)
            {
                res += source.GetItem();
                res += Environment.NewLine;
            }

            return res;
        }
    }

    public class NumberGenerator : IPipeStartItem<int>
    {
        public IAddingCompletableCollection<int> Output { get; private set; }

        public NumberGenerator(IAddingCompletableCollection<int> output)
        {
            Output = output;
        }

        public void Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                Output.PushItem(i);
            }

            Output.CompletePushing();
        }
    }

    public class Squarer : IPipeMiddleItem<int, int>
    {
        public IAddingCompletableCollection<int> Input { get; private set; }

        public IAddingCompletableCollection<int> Output { get; private set; }

        public Squarer(IAddingCompletableCollection<int> input, IAddingCompletableCollection<int> output)
        {
            Input = input;
            Output = output;
        }
        public void Execute()
        {
            while (!Input.IsEmpty)
            {
                var item = Input.GetItem();
                Output.PushItem(item * item);
            }

            Output.CompletePushing();
        }
    }

    public class Converter : IPipeMiddleItem<int, string>
    {
        public IAddingCompletableCollection<int> Input {get; init;}

        public IAddingCompletableCollection<string> Output { get; init; }

        public Converter(IAddingCompletableCollection<int> input, IAddingCompletableCollection<string> output)
        {
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            string res = string.Empty;

            while (!Input.IsEmpty)
            {
                var strinItem = Input.GetItem().ToString();
                var item = new string(strinItem.Reverse().ToArray());
                Output.PushItem(item);
            }

            Output.CompletePushing();
        }
    }
}