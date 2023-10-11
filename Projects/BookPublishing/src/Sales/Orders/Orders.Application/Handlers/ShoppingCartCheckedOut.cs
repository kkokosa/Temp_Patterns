using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sales.Shared.Events;
using Wolverine.Attributes;

namespace Orders.Application.Handlers
{
    public class ShoppingCartCheckedOutHandler
    {
        [Transactional]
        public void Handle(CartCheckedOut cartCheckedOut)
        {
            Console.WriteLine("Handle!");
        }
    }
}
