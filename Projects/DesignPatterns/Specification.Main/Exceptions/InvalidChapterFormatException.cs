using System;
using System.Runtime.Serialization;

namespace ConsoleApp
{
    [Serializable]
    internal class InvalidChapterFormatException : Exception
    {
        public InvalidChapterFormatException(string message) : base(message)
        {
        }
    }
}