using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public class WebPricingStrategy : IPricingStrategy
{
    public WebPricingStrategy()
    {
    }

    public List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        var discount = 0.1m;
        var result = new List<CartItem>();
        foreach (var item in cart.Items)
        {
            switch (item.ProductType)
            {
                case ProductType.EBook: result.Add(CartItem.WithoutNewDiscount(item)); break;
                case ProductType.PrintedBook:
                    result.Add(CartItem.WithNewDiscount(item, (1 - discount) * item.Price, "Web subscription!"));
                    break;
                case ProductType.Web:
                    result.Add(CartItem.WithNewDiscount(item, 0.0m, "Fixed pricing rule."));
                    break;
            }
        }
        return result;
    }
}