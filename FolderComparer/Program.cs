using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using FolderComparer.Folders;
using Pipelines.Pipes;
using FolderComparer.Tools;
using System.Threading;

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
                .ContinueParallelWith<int>(SquareNumbers)
                .ContinueParallelWith<string>((input, output) => new Converter(input, output))
                .FinishParallelWith(GetOneString);

            var res = pipeline.GetResult();
            Console.WriteLine(res);
        }

        public static void SquareNumbers(BlockingCollection<int> source, BlockingCollection<int> destination)
        {
            while(!source.IsCompleted || source.Count != 0)
            {
                try
                {
                    var item = source.Take();
                    destination.Add(item * item);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            Console.WriteLine("FINISH SQUARE");
            destination.CompleteAdding();
        }

        public static void ConvertToReverseString(BlockingCollection<int> source, BlockingCollection<string> destination)
        {
            string res = string.Empty;

            while (!source.IsCompleted || source.Count != 0)
            {
                try
                {
                    var strinItem = source.Take().ToString();
                    var item = new string(strinItem.Reverse().ToArray());
                    destination.Add(item);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            destination.CompleteAdding();
        }

        public static string GetOneString(BlockingCollection<string> source)
        {
            string res = string.Empty;

            while(!source.IsCompleted || source.Count != 0)
            {
                try
                {
                    res += source.Take();
                }
                catch (Exception)
                {
                    continue;
                }
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
                Output.Add(i);
            }

            Output.CompleteAdding();
            Thread.Sleep(2500);
            Console.WriteLine("FINISH GENERATOR");
        }

        public void Dispose()
        {
            Output.Dispose();
            Console.WriteLine("Generator disposed");
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
                try
                {
                    var item = Input.Take();
                    Output.Add(item * item);
                }
                catch (Exception)
                {
                    continue;
                }
        
            }

            Console.WriteLine("FINISH SQUARER");
            Output.CompleteAdding();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Console.WriteLine("Squarer disposed");
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
                try
                {
                    var strinItem = Input.Take().ToString();
                    var item = new string(strinItem.Reverse().ToArray());
                    Output.Add(item);
                }
                catch (Exception)
                {
                    continue;
                }
          
            }

            Output.CompleteAdding();

            Thread.Sleep(1000);
            Console.WriteLine("FINISH CONVERTER");
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Console.WriteLine("Converter disposed");
        }
    }
}