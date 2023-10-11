using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.Pricing;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

namespace Cart.Domain.Tests.Pricing
{
    public class WebPricingStrategyTests
    {
        [Theory]
        [InlineData(ProductType.EBook, 10.0, 10.0)]
        [InlineData(ProductType.Web, 10.0, 0.0)]
        [InlineData(ProductType.PrintedBook, 10.0, 9.0)]
        public void GivenSingleItemCart_WhenUsingWebPricing_ThenPriceIsDiscountedProperly(ProductType type, decimal price, decimal expectedPrice)
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            var strategy =
                new FixedPricingStrategy(0.0m, new CartItemSpecification(ProductType.Web),
                    new DiscountForSpecifiedProductsStrategy(0.1m, 
                        "Test message",
                        new CartItemSpecification(ProductType.PrintedBook)));
            var result = strategy.CalculatePrices(cart.Object);

            result.Should().ContainSingle(item => item.FinalPrice == expectedPrice);
        }

        [Fact]
        public void GivenMultipleItemsCart_WhenUsingWebPricing_ThenPriceIsDiscountedForProperItemOnly()
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), ProductType.Web, 1, 10.0m),
                new CartItem(Guid.NewGuid(), ProductType.EBook, 1, 10.0m),
                new CartItem(Guid.NewGuid(), ProductType.PrintedBook, 1, 10.0m)
            });

            var strategy =
                new FixedPricingStrategy(0.0m, new CartItemSpecification(ProductType.Web),
                    new DiscountForSpecifiedProductsStrategy(0.1m, "Test message", new CartItemSpecification(ProductType.PrintedBook)));
            var result = strategy.CalculatePrices(cart.Object);

            result.Should().BeEquivalentTo(new[]
            {
                new { ProductType = ProductType.Web, FinalPrice = 0.0m, DiscountReason = "Fixed pricing rule." },
                new { ProductType = ProductType.EBook, FinalPrice = 10.0m, DiscountReason = "" },
                new { ProductType = ProductType.PrintedBook, FinalPrice = 9.0m, DiscountReason = "Test message" }
            });
        }

        [Fact]
        public void GivenMultipleItemsCart_WhenUsingWebPricingAndCorrespondingBuilder_ResultsAreTheSame()
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), ProductType.Web, 1, 10.0m),
                new CartItem(Guid.NewGuid(), ProductType.EBook, 1, 10.0m),
                new CartItem(Guid.NewGuid(), ProductType.PrintedBook, 1, 10.0m)
            });

            var strategy =
                new FixedPricingStrategy(0.0m, new CartItemSpecification(ProductType.Web),
                    new DiscountForSpecifiedProductsStrategy(0.1m, "Test message", new CartItemSpecification(ProductType.PrintedBook)));

            var strategy2 = new PricingStrategyBuilder()
                .AddFixedPricing(0.0m, new CartItemSpecification(ProductType.Web))
                .AddDiscount(0.1m, "Test message", new CartItemSpecification(ProductType.PrintedBook))
                .Result;

            var result = strategy.CalculatePrices(cart.Object);
            var result2 = strategy2.CalculatePrices(cart.Object);

            result.Should().BeEquivalentTo(result2);
        }
    }
}
