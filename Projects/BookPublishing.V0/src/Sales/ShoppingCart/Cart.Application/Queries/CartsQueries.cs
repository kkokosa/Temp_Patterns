using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.ShoppingCart;
using Wolverine.Http;

namespace Cart.Application.Queries
{
    public class CartsQueries
    {
#if DEBUG
        // On production we don't need such heavyweight operation
        [WolverineGet("/carts")]
        public static async Task<IReadOnlyList<Domain.ShoppingCart.Cart>> Get(IQuerySession session)
        {
            return await session.Query<Domain.ShoppingCart.Cart>().ToListAsync();
        }
#endif

        [WolverineGet("/carts/{cartId:Guid}")]
        public static async Task<Domain.ShoppingCart.Cart?> GetTodo(Guid cartId, 
            ICartRepository cartRepository)
        {
            return await cartRepository.GetById(cartId);
        }
    }
}
