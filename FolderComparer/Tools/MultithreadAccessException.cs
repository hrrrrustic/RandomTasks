using System;

namespace FolderComparer.Tools
{
    public class MultithreadAccessException : Exception
    {
        public MultithreadAccessException(String message) : base(message)
        {
        }
    }
}