﻿using System;
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
