using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;
using FsCheck.Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FsCheck;

namespace Cart.Domain.Tests.Pricing
{
    public class DefaultPricingStrategyTests
    {
        [Theory]
        [InlineData(ProductType.EBook, 0.0)]
        [InlineData(ProductType.Web, 10.0)]
        [InlineData(ProductType.PrintedBook, 100.0)]
        public void GivenSingleItemCart_WhenUsingFixedPricing_ThenPriceIsFixed(ProductType type, decimal price)
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            DefaultPricingStrategy strategy = new DefaultPricingStrategy();
            var result = strategy.CalculatePrices(cart.Object);

            result.Should().ContainSingle(item => item.FinalPrice == price);
        }

        [Property(Verbose = false)]
        public Property PropertyThat_GivenSingleItemCart_WhenUsingFixedPricing_ThenPriceIsFixed(ProductType type, decimal price)
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            DefaultPricingStrategy strategy = new DefaultPricingStrategy();
            var result = strategy.CalculatePrices(cart.Object);

            return (result.Single().FinalPrice == price).ToProperty();
        }
    }
}
