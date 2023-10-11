using Cart.Domain.Customer;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing;

public class PricingService : IPricingService
{
    private readonly IPromotionsService _promotionsService;
    private readonly TimeProvider _timeProvider;

    public PricingService(IPromotionsService promotionsService, TimeProvider timeProvider)
    {
        _promotionsService = promotionsService;
        _timeProvider = timeProvider;
    }

    public async Task<List<IPricingStrategy>> GetCurrentPricing(CustomerType customerType)
    {
        var baseCustomerPricing = GetBaseCustomerPricing(customerType);
        var additionalPricing = await _promotionsService.GetPromotions(_timeProvider.GetUtcNow());
        additionalPricing.Add(baseCustomerPricing);
        return additionalPricing;
    }

    public IPricingStrategy GetBaseCustomerPricing(CustomerType customerType) =>
        customerType switch
        {
            CustomerType.NormalSubscription 
                => new DefaultPricingStrategy(),
            CustomerType.BusinessSubscription 
                => new FixedPricingStrategy(5.0m, new CartItemSpecification(ProductType.Web)
                    .Or(new CartItemSpecification(ProductType.EBook))
                    .Or(new CartItemSpecification(ProductType.PrintedBook))),
            CustomerType.WebSubscription
                => new PricingStrategyBuilder()
                    .AddFixedPricing(0.0m, new CartItemSpecification(ProductType.Web))
                    .AddDiscount(0.1m, "Web subscription!", new CartItemSpecification(ProductType.PrintedBook))
                    .Result,
            _ => throw new ArgumentOutOfRangeException(nameof(customerType), customerType, null)
        };
}