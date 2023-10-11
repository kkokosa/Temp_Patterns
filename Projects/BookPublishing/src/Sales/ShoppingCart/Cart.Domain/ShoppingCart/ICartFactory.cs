using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.ShoppingCart
{
    public interface ICartFactory
    {
        public Task<Cart> CreateCart(
            Guid? id,
            Guid customerId);
    }
}
