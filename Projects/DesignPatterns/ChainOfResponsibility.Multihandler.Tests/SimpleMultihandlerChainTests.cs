using System;
using System.Collections.Generic;
using ChainOfResponsibility.Multihandler.DiscountRules;
using Xunit;

namespace ChainOfResponsibility.Multihandler.Tests
{
    public class SimpleMultihandlerChainTests
    {
        [Fact]
        public void GivenSampleOrderAndDiscounts_WhenBothShouldApply_ThenProperDiscountShouldBeCalculated()
        {
            Order order = new Order();
            order.AddOrderItem("X", 10.0m, 100);
            order.AddOrderItem("Y", 10_000m, 1);

            DiscountService service = new DiscountService(new List<IDiscountRule>()
            {
                new TotalItemsDiscountRule(10, 0.9m),
                new TotalPriceDiscountRule(5_000m, 0.8m)
            });

            service.CalculateDiscount(order);

            Assert.Equal(11_000m * 0.8m * 0.9m, order.CurrentPrice);
        }
    }
}
