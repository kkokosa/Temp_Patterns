using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.ShoppingCart;
using FluentValidation;
using Sales.Shared.Events;
using Wolverine.Http;
using Wolverine.Marten;

namespace Cart.Application.Commands
{
    public record CreateCart(Guid Id, Guid CustomerId);

    public class CreateCartValidator : AbstractValidator<CreateCart>
    {
        public CreateCartValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }

    public record CreateCartResponse(Guid Id)
        : CreationResponse("/carts/" + Id);

    public class CreateCartEndpoint
    {
        [WolverinePost("/carts")]
        public async Task<(CreateCartResponse, StoreDoc<Domain.ShoppingCart.Cart>, CartCreated)> Handle(
            CreateCart command,
            ICartFactory cartFactory)
        {
            var cart = await cartFactory.CreateCart(command.Id,
                command.CustomerId);

            return (new CreateCartResponse(command.Id),
                    new StoreDoc<Domain.ShoppingCart.Cart>(cart),
                    new CartCreated(command.Id));
        }
    }
}
