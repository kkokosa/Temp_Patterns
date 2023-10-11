using System;

namespace ChainOfResponsibility.Tree
{
    public class SinglePrinter : IPrintHandler
    {
        private readonly State state;
        private readonly string name;

        public Guid Id { get; private set; }

        public SinglePrinter(Guid id, string name, State state)
        {
            this.Id = id;
            this.name = name;
            this.state = state;
        }

        public Guid Print(PrintOrder order)
        {
            Console.WriteLine($"Printing in printer {name} (Id:{Id}!");
            return Id;
        }

        public bool CanHandle(PrintOrder order)
        {
            return this.state == State.Waiting || this.state == State.Sleep;
        }

        public enum State
        {
            Busy,
            Queued,
            Waiting,
            Sleep
        }
    }
}