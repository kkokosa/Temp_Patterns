using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sales.Shared.Events;
using Wolverine.Attributes;
using Wolverine.Http;
using Wolverine.Marten;

namespace Cart.Application.Commands
{
    public record CartCheckout(Guid CartId);

    public class CartCheckoutValidator : AbstractValidator<CartCheckout>
    {
        public CartCheckoutValidator()
        {
            RuleFor(x => x.CartId).NotNull();
            RuleFor(x => x.CartId).NotEmpty();
        }
    }

    public record CartCheckoutResponse();
    
    public class CartCheckoutEndpoint
    {
        public static async Task<(Domain.ShoppingCart.Cart?, IResult)>
            LoadAsync(Guid cartId,
                ICartRepository cartRepository)
        {
            var cart = await cartRepository.GetById(cartId);
            return cart is null
                ? (null, Results.NotFound())
                : (cart, new WolverineContinue());
        }

        [Transactional]
        [WolverinePost("/carts/{cartId:Guid}/checkout")]
        public async Task<(CartCheckoutResponse, CartCheckedOut)>
            Handle(CartCheckout command,
                Guid cartId,
                Domain.ShoppingCart.Cart cart)
        {
            // cart ?
            return (new CartCheckoutResponse(),
                new CartCheckedOut(command.CartId));
        }
    }
}
