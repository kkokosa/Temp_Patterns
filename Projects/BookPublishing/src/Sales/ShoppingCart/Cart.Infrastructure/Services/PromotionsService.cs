using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using Sales.Shared.Exceptions;

namespace Cart.Infrastructure.Services
{
    public class PromotionsService : IPromotionsService
    {
        public Task<List<IPricingStrategy>> GetPromotions(DateTimeOffset time)
        {
            // TODO: get from somewhere
            return Task.FromResult(new List<IPricingStrategy>()
            {
                // new SeasonalDiscount() { From = "2023-09-01", To = "2023-10-15"}
            });
        }

        public async Task<IPricingStrategy> GetCouponByName(string name)
        {
            var reasonMessage = $"{name} coupon applied.";
            return name switch
            {
                "HALFPRICE" => new DiscountForSpecifiedProductsStrategy(0.5m, reasonMessage,
                    new CartItemSpecification(ProductType.Web)
                        .Or(new CartItemSpecification(ProductType.EBook))),
                _ => throw new ObjectDoesNotExistException<IPricingStrategy>(name)
            };
        }
    }
}
