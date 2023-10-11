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
using Moq;
using Moq.Protected;
using Cart = Cart.Domain.ShoppingCart.Cart;

namespace Cart.Domain.Tests.Pricing
{
    public class FixedPricingStrategyTests
    {
        [Fact]
        public async Task GivenDefaultCustomer_WhenUsingFixedPricing_ThenPriceIsFixed()
        {
            // Pure mocks approach, heavily:
            // - solitary - FixedPricingStrategy is isolated from its real dependencies
            // - interaction-based - the test checks how FixedPricingStrategy interacts with its dependencies and needs to understand it a lot
            var cartId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer.Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var pricingService = new Mock<IPricingService>();
            pricingService.Setup(x => x.GetCurrentPricing(CustomerType.NormalSubscription))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                    new DefaultPricingStrategy()
                });
            var factory = new CartFactory(pricingService.Object, new DhlDeliveryService(), customerRepository.Object);
            var cart = await factory.CreateCart(cartId, customerId);
            cart.AddItem(productId, ProductType.Web, 10.0m, 1);

            FixedPricingStrategy strategy = new FixedPricingStrategy(5.0m, CartItemSpecification.Any);
            var result = strategy.CalculatePrices(cart);

            result.Should().ContainSingle(item => item.FinalPrice == 5.0m);
        }

        [Theory]
        [InlineData(ProductType.EBook, 0.0, 0.0)]
        [InlineData(ProductType.Web, 10.0, 5.0)]
        [InlineData(ProductType.PrintedBook, 100.0, 5.0)]
        public void GivenSingleItemCart_WhenUsingFixedPricing_ThenPriceIsFixed(ProductType type, decimal price, decimal expectedPrice)
        {
            // Pure stubs approach
            // - we needed to make ICart, and CalculatePrices accepting ICart, just for such tests...
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            FixedPricingStrategy strategy = new FixedPricingStrategy(5.0m, CartItemSpecification.Any);
            var result = strategy.CalculatePrices(cart.Object);
            
            result.Should().ContainSingle(item => item.FinalPrice == expectedPrice);
        }

        [Property(Verbose = false)]
        public Property PropertyThat_GivenSingleItemCart_WhenUsingFixedPricing_ThenPriceIsFixed(ProductType type, decimal price, decimal expectedPrice)
        {
            // Pure stubs approach
            // - we needed to make ICart, and CalculatePrices accepting ICart, just for such tests...
            var cart = new Mock<ShoppingCart.ICart>();
            cart.SetupGet(x => x.Items).Returns(new[]
            {
                new CartItem(Guid.NewGuid(), type, 1, price)
            });

            FixedPricingStrategy strategy = new FixedPricingStrategy(expectedPrice, CartItemSpecification.Any);
            var result = strategy.CalculatePrices(cart.Object);

            return (result.Single().FinalPrice == expectedPrice).ToProperty();
        }
    }
}
