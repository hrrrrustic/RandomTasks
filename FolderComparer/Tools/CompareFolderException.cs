using System;

namespace FolderComparer.Tools
{
    public class CompareFolderException : Exception
    {
        public CompareFolderException(String message) : base(message)
        {
        }
    }
}