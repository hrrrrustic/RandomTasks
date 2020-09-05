using System;

namespace FolderComparer
{
    public class MultithreadAccessException : Exception
    {
        public MultithreadAccessException(String message) : base(message)
        {
        }
    }
}