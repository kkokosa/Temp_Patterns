using System;
using System.Runtime.Serialization;

namespace ConsoleApp
{
    [Serializable]
    internal class InvalidBookStageException : Exception
    {
        public InvalidBookStageException(BookStage expected, BookStage actual)
            : base($"State {expected} was expected but was {actual}.")
        {
        }

        public InvalidBookStageException(BookStage actual)
            : base($"State was {actual}.")
        {
        }
    }
}