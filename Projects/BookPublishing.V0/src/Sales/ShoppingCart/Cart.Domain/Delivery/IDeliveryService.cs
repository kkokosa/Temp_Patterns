namespace Cart.Domain;

public interface IDeliveryService
{
    public decimal GetShippingCost(decimal weight, string address);
}