using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;

namespace Cart.Domain.Promotions;

public interface IPromotionsService
{
    Task<List<IPricingStrategy>> GetPromotions(DateTimeOffset time);
    Task<IPricingStrategy> GetCouponByName(string name);
}