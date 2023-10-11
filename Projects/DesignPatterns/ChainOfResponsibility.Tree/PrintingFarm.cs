using System;
using System.Collections.Generic;

namespace ChainOfResponsibility.Tree
{
    public class PrintingFarm : IPrintHandler
    {
        private List<SinglePrinter> printers;
        public Guid Id { get; private set; }


        public PrintingFarm(Guid id, List<SinglePrinter> printers)
        {
            this.Id = id;
            this.printers = printers;
        }

        public bool CanHandle(PrintOrder order)
        {
            return printers.Count > 0 && printers.Exists(x => x.CanHandle(order));
        }

        public Guid Print(PrintOrder order)
        {
            return printers.Find(x => x.CanHandle(order))
                .Print(order);
        }

        public void RegisterPrinter(SinglePrinter printer)
        {
            bool alreadyExists = this.printers.Exists(x => x.Id == printer.Id);
            if (alreadyExists)
                throw new InvalidOperationException();
            this.printers.Add(printer);
        }

        public void UnregisterPrinter(SinglePrinter printer)
        {
            var alreadyExists = this.printers.Exists(x => x.Id == printer.Id);
            if (!alreadyExists)
                throw new InvalidOperationException();
            this.printers.RemoveAll(x => x.Id == printer.Id);
        }
    }
}