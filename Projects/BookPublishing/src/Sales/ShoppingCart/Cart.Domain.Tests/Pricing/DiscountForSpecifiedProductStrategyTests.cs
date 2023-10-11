using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Moq;

namespace Cart.Domain.Tests.Pricing
{
    public class DiscountForSpecifiedProductStrategyTests
    {
        [Theory]
        [InlineData(ProductType.EBook, 0.0, 0.0, 0.0)]
        [InlineData(ProductType.Web, 10.0, 0.1, 9.0)]
        [InlineData(ProductType.PrintedBook, 100.0, 0.1, 90.0)]
        public void GivenProperSingleItemCart_WhenUsingDiscountPricing_ThenPriceIsDiscounted(
            ProductType type, decimal price, decimal discountByPercent, decimal expectedPrice)
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            var strategy = new DiscountForSpecifiedProductsStrategy(discountByPercent, 
                "Test message",
                new CartItemSpecification(type));
            var result = strategy.CalculatePrices(cart.Object);

            result.Should()
                .ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    FinalPrice = expectedPrice,
                    DiscountReason = "Test message"
                });
        }

        [Property(Verbose = true)]
        public Property PropertyThat_GivenProperSingleItemCart_WhenUsingDiscountPricing_ThenPriceIsDiscounted(
            ProductType type, decimal price, decimal discountByPercent)
        {
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            var strategy = new DiscountForSpecifiedProductsStrategy(
                discountByPercent, 
                "Test message",
                new CartItemSpecification(type));
            var result = strategy.CalculatePrices(cart.Object);

            return (result.Single().FinalPrice == (price * (1 - discountByPercent))).ToProperty();
        }

        [Theory]
        [InlineData(ProductType.EBook)]
        [InlineData(ProductType.PrintedBook)]
        [InlineData(ProductType.Web)]
        public void GivenMultipleItemsCart_WhenUsingDiscountPricing_ThenPriceIsDiscountedForProperItemOnly(
            ProductType type)
        {
            decimal price = 10.0m;
            decimal discountByPercent = 0.5m;
            decimal expectedPrice = 5.0m;
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), ProductType.Web, 1, price),
                new CartItem(Guid.NewGuid(), ProductType.EBook, 1, price),
                new CartItem(Guid.NewGuid(), ProductType.PrintedBook, 1, price)
            });

            var strategy = new DiscountForSpecifiedProductsStrategy(discountByPercent, 
                "Test message", 
                new CartItemSpecification(type));
            var result = strategy.CalculatePrices(cart.Object);

            result.Should().ContainSingle(x => 
                x.ProductType == type && x.FinalPrice == expectedPrice && x.DiscountReason == "Test message");
            result.Should()
                .Match(items => items.Count(x => x.ProductType != type && x.FinalPrice == price) == 2);

        }
    }
}
