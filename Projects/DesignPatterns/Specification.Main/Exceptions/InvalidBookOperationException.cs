using System;
using System.Runtime.Serialization;

namespace ConsoleApp
{
    [Serializable]
    internal class InvalidBookOperationException : Exception
    {
        public InvalidBookOperationException(string message) : base(message)
        {
        }
    }
}