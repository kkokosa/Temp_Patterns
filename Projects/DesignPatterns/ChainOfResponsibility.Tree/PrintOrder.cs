using System;

namespace ChainOfResponsibility.Tree
{
    public class PrintOrder
    {
        public Guid Id { get; private set; }

        public PrintOrder(Guid id)
        {
            Id = id;
        }
    }
}