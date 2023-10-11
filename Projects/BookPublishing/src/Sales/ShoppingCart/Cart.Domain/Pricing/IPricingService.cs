using Cart.Domain.Customer;
using Cart.Domain.Pricing.Strategies;

namespace Cart.Domain.Pricing
{
    public interface IPricingService
    {
        Task<List<IPricingStrategy>> GetCurrentPricing(CustomerType customerType);
        IPricingStrategy GetBaseCustomerPricing(CustomerType customerType);
    }
}
