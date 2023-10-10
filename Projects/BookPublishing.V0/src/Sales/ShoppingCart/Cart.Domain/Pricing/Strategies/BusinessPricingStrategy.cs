using Cart.Domain.ShoppingCart;
using Shared.Domain;

namespace Cart.Domain.Pricing.Strategies;

public class BusinessPricingStrategy : IPricingStrategy
{
    public const string BusinessPriceDescription = "Fixed pricing rule.";

    private decimal _negotiatedPrice { get; init; }

    public BusinessPricingStrategy(decimal negotiatedPrice)
    {
        _negotiatedPrice = negotiatedPrice;
    }

    public List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        return cart.Items
            .Select(item => item.Price != 0 
                ? CartItem.WithNewDiscount(item,_negotiatedPrice,BusinessPriceDescription) 
                : CartItem.WithoutNewDiscount(item))
            .ToList();
    }
}