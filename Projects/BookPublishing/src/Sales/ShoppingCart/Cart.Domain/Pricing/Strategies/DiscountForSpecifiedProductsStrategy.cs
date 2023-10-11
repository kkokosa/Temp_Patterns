using Cart.Domain.ShoppingCart;
using Shared.Domain;

namespace Cart.Domain.Pricing.Strategies;

public class DiscountForSpecifiedProductsStrategy : DecoratingPricingStrategy
{
    private readonly Specification<CartItem> _discountSpecification;
    private readonly decimal _discountByPercent;
    private readonly string _reasonMessage;

    public DiscountForSpecifiedProductsStrategy(decimal discountByPercent,
        string reasonMessage,
        Specification<CartItem> discountSpecification) 
        : this(discountByPercent, reasonMessage, discountSpecification, null)
    {
    }

    public DiscountForSpecifiedProductsStrategy(decimal discountByPercent,
        string reasonMessage,
        Specification<CartItem> discountSpecification,
        IPricingStrategy decoratedStrategy) : base(decoratedStrategy)
    {
        _discountByPercent = discountByPercent;
        _discountSpecification = discountSpecification;
        _reasonMessage = reasonMessage;
    }

    public override List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        var items = 
            _decoratedStrategy is not null ?
            _decoratedStrategy.CalculatePrices(cart) :
            cart.Items;
        var result = new List<CartItem>();
        foreach (var item in items)
        {
            result.Add(_discountSpecification.IsSatisfiedBy(item)
                ? CartItem.WithNewDiscount(item, 
                    item.Price * (1.0m - _discountByPercent), 
                    _reasonMessage)
                : CartItem.WithoutNewDiscount(item));
        }
        return result;
    }
}