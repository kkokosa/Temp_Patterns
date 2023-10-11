using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainOfResponsibility.Tree
{
    public interface IPrintHandler
    {
        Guid Id { get; }
        
        Guid Print(PrintOrder order);
        bool CanHandle(PrintOrder order);
    }
}
