using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public class FirstBookFreePricingStrategy : IPricingStrategy
{
    public const string FreeBookDescription = "Your subscription's the first free book in this month!";

    public List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        // Check if promotion used already in this month
        if (cart.Items.Count() == 1)
        {
            var onlyItem = cart.Items.First();
            return new List<CartItem>() { CartItem.WithOriginalPrice(onlyItem) };
        }

        var freePrintedBook = cart.Items
            .Where(item => item.ProductType == ProductType.PrintedBook)
            .MinBy(x => x.Price);

        return cart.Items.Select(item => item == freePrintedBook
                ? CartItem.WithNewDiscount(item, 0.0m, FreeBookDescription)
                : CartItem.WithoutNewDiscount(item)).ToList();
    }
}