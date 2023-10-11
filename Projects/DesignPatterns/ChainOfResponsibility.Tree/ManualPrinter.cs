using System;

namespace ChainOfResponsibility.Tree
{
    public class ManualPrinter : IPrintHandler
    {
        private Employee owner;

        public ManualPrinter(Guid id, Employee owner)
        {
            this.Id = id;
            this.owner = owner;
        }

        public Guid Id { get; private set; }

        public Guid Print(PrintOrder order)
        {
            Console.WriteLine($"Print order will be printed by {owner.Name}");
            return Id;
        }

        public bool CanHandle(PrintOrder order) => owner?.IsOnDuty ?? false;
    }
}