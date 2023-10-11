using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainOfResponsibility.Tree
{
    public class PrintingUnit : IPrintHandler
    {
        private readonly string name;
        private readonly List<IPrintHandler> subunits;
        public string Name => name;
        public Guid Id { get; private set; }
        
        public PrintingUnit(Guid id, string name, List<IPrintHandler> subunits)
        {
            this.Id = id;
            this.name = name;
            this.subunits = subunits;
        }

        public Guid Print(PrintOrder order)
        {
            return this.subunits.First(x => x.CanHandle(order)).Print(order);
        }

        public bool CanHandle(PrintOrder order)
        {
            return this.subunits.Any(x => x.CanHandle(order));
        }
    }
}