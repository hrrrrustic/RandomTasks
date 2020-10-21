using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using FolderComparer.Folders;
using Pipelines.Pipes;
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


        public static void SquareNumbers(BlockingCollection<int> source, BlockingCollection<int> destination)
        {
            while(!source.IsCompleted || source.Count != 0)
            {
                var item = source.Take();
                destination.Add(item * item);
            }

            destination.CompleteAdding();
        }

        public static void ConvertToReverseString(BlockingCollection<int> source, BlockingCollection<string> destination)
        {
            string res = string.Empty;

            while (!source.IsCompleted || source.Count != 0)
            {
                var strinItem = source.Take().ToString();
                var item = new string(strinItem.Reverse().ToArray());
                destination.Add(item);
            }

            destination.CompleteAdding();
        }

        public static string GetOneString(BlockingCollection<string> source)
        {
            string res = string.Empty;

            while(!source.IsCompleted || source.Count != 0)
            {
                res += source.Take();
                res += Environment.NewLine;
            }

            return res;
        }
    }

    public class NumberGenerator : IPipeStartItem<int>
    {
        public BlockingCollection<int> Output { get; private set; }

        public NumberGenerator(BlockingCollection<int> output)
        {
            Output = output;
        }

        public void Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                Output.TryAdd(i);
            }

            Output.CompleteAdding();
        }
    }

    public class Squarer : IPipeMiddleItem<int, int>
    {
        public BlockingCollection<int> Input { get; private set; }

        public BlockingCollection<int> Output { get; private set; }

        public Squarer(BlockingCollection<int> input, BlockingCollection<int> output)
        { 
            Input = input;
            Output = output;
        }
        public void Execute()
        {
            while (!Input.IsCompleted || Input.Count != 0)
            {
                var item = Input.Take();
                Output.Add(item * item);
            }

            Output.CompleteAdding();
        }
    }

    public class Converter : IPipeMiddleItem<int, string>
    {
        public BlockingCollection<int> Input {get; init;}

        public BlockingCollection<string> Output { get; init; }

        public Converter(BlockingCollection<int> input, BlockingCollection<string> output)
        {
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            string res = string.Empty;

            while (!Input.IsCompleted || Input.Count != 0)
            {
                var strinItem = Input.Take().ToString();
                var item = new string(strinItem.Reverse().ToArray());
                Output.Add(item);
            }

            Output.CompleteAdding();
        }
    }
}