using System;

namespace State.Main.Exceptions
{
    [Serializable]
    public class InvalidBookStateTransitionException : InvalidOperationException
    {
        public InvalidBookStateTransitionException(State fromState, State toState) 
            : base($"Invalid try to move from state {fromState} to {toState}") { }
    }
}