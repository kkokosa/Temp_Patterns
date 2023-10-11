using Cart.Domain.ShoppingCart;
using Shared.Domain;

namespace Cart.Domain.Pricing.Strategies;

public class FixedPricingStrategy : DecoratingPricingStrategy
{
    public const string BusinessPriceDescription = "Fixed pricing rule.";
    private readonly Specification<CartItem> _pricingSpecification;

    private decimal _negotiatedPrice { get; init; }

    public FixedPricingStrategy(decimal negotiatedPrice,
        Specification<CartItem> pricingSpecification) : this(negotiatedPrice,
        pricingSpecification,
        null)
    {
    }

    public FixedPricingStrategy(decimal negotiatedPrice,
        Specification<CartItem> pricingSpecification,
        IPricingStrategy decoratedStrategy) : base(decoratedStrategy)
    {
        _negotiatedPrice = negotiatedPrice;
        _pricingSpecification = pricingSpecification;
    }

    public override List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        var items =
            _decoratedStrategy is not null 
                ? _decoratedStrategy.CalculatePrices(cart) 
                : cart.Items;

        return items
            .Select(item => item.Price != 0 && _pricingSpecification.IsSatisfiedBy(item) ?
                CartItem.WithNewDiscount(item,
                _negotiatedPrice,
                BusinessPriceDescription) 
                : CartItem.WithoutNewDiscount(item))
            .ToList();
    }
}