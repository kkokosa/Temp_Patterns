using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public class DefaultPricingStrategy : IPricingStrategy
{
    public List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        return cart.Items
            .Select(CartItem.WithoutNewDiscount)
            .ToList();
    }
}