namespace Cart.Domain.Shipping;

public interface IShippingCostStrategy
{
    decimal CalculateShippingCost(ShoppingCart.Cart cart);
}