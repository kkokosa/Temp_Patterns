using System;

namespace State.Main.Exceptions
{
    public class InvalidBookOperationException : InvalidOperationException
    {
        public InvalidBookOperationException(string method, string state) 
            : base($"Method {method} invoked in state {state}")
        {
        }
    }
}