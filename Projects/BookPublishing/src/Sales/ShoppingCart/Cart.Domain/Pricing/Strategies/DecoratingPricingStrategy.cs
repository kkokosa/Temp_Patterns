using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public abstract class DecoratingPricingStrategy : IPricingStrategy
{
    protected readonly IPricingStrategy? _decoratedStrategy;

    protected DecoratingPricingStrategy(IPricingStrategy? decoratedStrategy)
    {
        _decoratedStrategy = decoratedStrategy;
    }

    public abstract List<CartItem> CalculatePrices(ICart cart);
}