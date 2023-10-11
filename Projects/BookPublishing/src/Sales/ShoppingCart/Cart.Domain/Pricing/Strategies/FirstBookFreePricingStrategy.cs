using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies;

public class FirstBookFreePricingStrategy : DecoratingPricingStrategy
{
    public const string FreeBookDescription = "Your subscription's the first free book in this month!";

    public FirstBookFreePricingStrategy() : this(null)
    {
    }

    public FirstBookFreePricingStrategy(IPricingStrategy decoratedStrategy) : base(decoratedStrategy)
    {
    }

    public override List<CartItem> CalculatePrices(ShoppingCart.ICart cart)
    {
        var items =
            _decoratedStrategy?.CalculatePrices(cart) ?? cart.Items;

        // Check if promotion used already in this month
        if (items.Count() == 1)
        {
            var onlyItem = items.First();
            return new List<CartItem>() { CartItem.WithOriginalPrice(onlyItem) };
        }

        var freePrintedBook = items
            .Where(item => item.ProductType == ProductType.PrintedBook)
            .MinBy(x => x.Price);

        return items.Select(item => item == freePrintedBook
                ? CartItem.WithNewDiscount(item, 0.0m, FreeBookDescription)
                : CartItem.WithoutNewDiscount(item)).ToList();
    }
}