using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public interface IPricingStrategy
{
    List<CartItem> CalculatePrices(ShoppingCart.ICart cart);
}