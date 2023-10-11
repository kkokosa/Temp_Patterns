using Cart.Domain.Product;
using Cart.Domain.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Sales.Shared.Events;
using Wolverine.Http;
using Wolverine.Marten;

namespace Cart.Application.Commands
{
    public record AddCartItem(Guid CartId, Guid ProductId, uint Amount);

    public record AddCartItemResponse();
    
    public class AddCartItemEndpoint
    {
        public static async Task<(Domain.ShoppingCart.Cart?, Product?, IResult )> 
            LoadAsync(Guid cartId,
                AddCartItem command,
                ICartRepository cartRepository,
                IProductRepository productRepository)
        {
            var cart = await cartRepository.GetById(cartId);
            var product = await productRepository.GetById(command.ProductId);
            return (cart is null || product is null)
                ? (null, null, Results.NotFound()) 
                : (cart, product, new WolverineContinue());
        }

        [WolverinePost("/carts/{cartId:Guid}/items")]
        public (AddCartItemResponse, StoreDoc<Domain.ShoppingCart.Cart>, CartItemAdded)
            Handle(AddCartItem command,
                Guid cartId,
                Domain.ShoppingCart.Cart cart,
                Product product)
        {
            cart.AddItem(command.ProductId, product.Type, product.Price, command.Amount);
            return (new AddCartItemResponse(),
                new StoreDoc<Domain.ShoppingCart.Cart>(cart),
                new CartItemAdded(command.CartId, command.ProductId, command.Amount));
        }
    }
}
