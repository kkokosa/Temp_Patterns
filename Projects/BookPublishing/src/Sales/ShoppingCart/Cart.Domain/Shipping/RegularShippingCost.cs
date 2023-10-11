﻿using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Shipping;

public class RegularShippingCost(IDeliveryService DeliveryService) : IShippingCostStrategy
{
    public decimal CalculateShippingCost(ShoppingCart.Cart cart)
    {
        // TODO: Calculate total weight...
        var totalWeight = 0.0m;
        var isPhysicaldelivery = cart.Items.Any(x => x.ProductType == ProductType.PrintedBook);
        if (!isPhysicaldelivery)
            return 0.0m;
        var cost = DeliveryService.GetShippingCost(totalWeight, "Chmielna 73");
        return cost;
    }
}