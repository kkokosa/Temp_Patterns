using Cart.Domain.Pricing;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Sales.Shared.Events;
using Wolverine.Http;
using Wolverine.Marten;

namespace Cart.Application.Commands
{
    public record ApplyCoupon(Guid CartId, string Name);

    public class ApplyCouponValidator : AbstractValidator<ApplyCoupon>
    {
        public ApplyCouponValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public record ApplyCouponResponse();


    public class ApplyCouponEndpoint
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


        [WolverinePost("/carts/{cartId:Guid}/coupon")]
        public async Task<(ApplyCouponResponse, StoreDoc<Domain.ShoppingCart.Cart>, CouponApplied)>
            Handle(ApplyCoupon command,
                Guid cartId,
                Domain.ShoppingCart.Cart cart,
                IPromotionsService promotionsService)
        {
            var couponPricing = await promotionsService.GetCouponByName(command.Name);
            cart.AddPricing(couponPricing); 
            return (new ApplyCouponResponse(),
                new StoreDoc<Domain.ShoppingCart.Cart>(cart),
                new CouponApplied(command.CartId));
        }
    }
}
