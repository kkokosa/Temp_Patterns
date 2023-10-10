using Cart.Domain.Pricing.Strategies;

namespace Cart.Domain.ShoppingCart;

public interface ICart
{
    Guid Id { get; }
    State State { get; }
    Guid CustomerId { get; }
    IReadOnlyCollection<CartItem> Items { get; }
    void AddItem(Guid productId, ProductType type, decimal price, uint amount);
    decimal CalculateShippingCost();
    void AddPricing(IPricingStrategy pricing);
    void ApplyPricing(List<IPricingStrategy> pricing);
}