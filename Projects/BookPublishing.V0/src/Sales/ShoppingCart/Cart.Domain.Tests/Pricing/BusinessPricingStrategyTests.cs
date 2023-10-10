using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using Cart.Infrastructure.Services;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Moq.Protected;
using Cart = Cart.Domain.ShoppingCart.Cart;

namespace Cart.Domain.Tests.Pricing
{
    public class BusinessPricingStrategyTests
    {
        [Fact]
        public async Task GivenDefaultCustomer_WhenUsingBusinessPricing_ThenPriceIsFixed()
        {
            // Pure mocks approach, heavily:
            // - solitary - BusinessPricingStrategy is isolated from its real dependencies
            // - interaction-based - the test checks how BusinessPricingStrategy interacts with its dependencies and needs to understand it a lot
            var cartId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer.Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var promotionService = new Mock<IPromotionsService>();
            promotionService.Setup(x => x.GetPromotions(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                });
            var fakeTimeProvider = new FakeTimeProvider();

            var factory = new CartFactory(new DhlDeliveryService(), customerRepository.Object, promotionService.Object, fakeTimeProvider);
            var cart = await factory.CreateCart(cartId, customerId);
            cart.AddItem(productId, ProductType.Web, 10.0m, 1);

            BusinessPricingStrategy strategy = new BusinessPricingStrategy(5.0m);
            var result = strategy.CalculatePrices(cart);

            result.Should().ContainSingle(item => item.FinalPrice == 5.0m);
        }

        [Theory]
        [InlineData(ProductType.EBook, 0.0, 0.0)]
        [InlineData(ProductType.Web, 10.0, 5.0)]
        [InlineData(ProductType.PrintedBook, 100.0, 5.0)]
        public void GivenSingleItemCart_WhenUsingBusinessPricing_ThenPriceIsFixed(ProductType type, decimal price, decimal expectedPrice)
        {
            // Pure stubs approach
            // - we needed to make ICart, and CalculatePrices accepting ICart, just for such tests...
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            BusinessPricingStrategy strategy = new BusinessPricingStrategy(5.0m);
            var result = strategy.CalculatePrices(cart.Object);
            
            result.Should().ContainSingle(item => item.FinalPrice == expectedPrice);
        }

        [Property(Verbose = false)]
        public Property PropertyThat_GivenSingleItemCart_WhenUsingBusinessPricing_ThenPriceIsFixed(ProductType type, decimal price, decimal expectedPrice)
        {
            // Pure stubs approach
            // - we needed to make ICart, and CalculatePrices accepting ICart, just for such tests...
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            BusinessPricingStrategy strategy = new BusinessPricingStrategy(expectedPrice);
            var result = strategy.CalculatePrices(cart.Object);

            return (result.Single().FinalPrice == expectedPrice).ToProperty();
        }
    }
}
