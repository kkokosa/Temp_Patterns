using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cart.Application.Commands;
using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Product;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using Cart.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Sales.Shared.Events;
using Wolverine.Marten;

namespace Cart.Application.Tests
{
    public class ChangingOrders
    {
        [Fact]
        public async Task CanApplyCoupon_OnEmptyCart()
        {
            var cartId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var pricingService = new Mock<IPricingService>();
            pricingService.Setup(x => x.GetCurrentPricing(CustomerType.NormalSubscription))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                    new DiscountForSpecifiedProductsStrategy(0.5m, "Test message", new CartItemSpecification(ProductType.EBook))
                });
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var promotionsService = new Mock<IPromotionsService>();
            promotionsService.Setup(x => x.GetCouponByName("HALFPRICE")).ReturnsAsync(
                    new DiscountForSpecifiedProductsStrategy(0.5m, "Test message",
                        new CartItemSpecification(ProductType.Web)
                            .Or(new CartItemSpecification(ProductType.EBook)))
            );

            var factory = new CartFactory(pricingService.Object, new DhlDeliveryService(), customerRepository.Object);
            var cart = await factory.CreateCart(cartId, customerId);
            var handler = new ApplyCouponEndpoint();

            var command = new ApplyCoupon(cartId, "HALFPRICE");
            var (response, store, @event) = await handler.Handle(command, cartId, cart, promotionsService.Object);

            response.Should().BeOfType<ApplyCouponResponse>();
            var document = store.Should().BeOfType<StoreDoc<Domain.ShoppingCart.Cart>>()
                .Which.Document.Should().BeOfType<Domain.ShoppingCart.Cart>()
                .Which;
            document.Id.Should().Be(cartId);
            document.CustomerId.Should().Be(customerId);
            document.State.Should().Be(State.Unconfirmed);
            document.Items.Should().BeEmpty();
                
            @event.Should().BeOfType<CouponApplied>()
                .Which.CartId.Should().Be(cartId);
        }

        [Fact]
        public async Task CanApplyCoupon_OnProperBook()
        {
            var cartId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var pricingService = new Mock<IPricingService>();
            pricingService.Setup(x => x.GetCurrentPricing(CustomerType.NormalSubscription))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                    new DefaultPricingStrategy()
                });
            var promotionsService = new Mock<IPromotionsService>();
            promotionsService.Setup(x => x.GetCouponByName("HALFPRICE")).ReturnsAsync(
                new DiscountForSpecifiedProductsStrategy(0.5m, "Test message",
                    new CartItemSpecification(ProductType.Web)
                        .Or(new CartItemSpecification(ProductType.EBook)))
            );
            var factory = new CartFactory(pricingService.Object, new DhlDeliveryService(), customerRepository.Object);
            var cart = await factory.CreateCart(cartId, customerId);
            cart.AddItem(productId, ProductType.Web, 10.0m, 1);
            
            var handler = new ApplyCouponEndpoint();
            var command = new ApplyCoupon(cartId, "HALFPRICE");
            var (response, store, @event) = await handler.Handle(command, cartId, cart, promotionsService.Object);

            response.Should().BeOfType<ApplyCouponResponse>();
            var document = store.Should().BeOfType<StoreDoc<Domain.ShoppingCart.Cart>>()
                .Which.Document.Should().BeOfType<Domain.ShoppingCart.Cart>()
                .Which;
            document.Id.Should().Be(cartId);
            document.CustomerId.Should().Be(customerId);
            document.State.Should().Be(State.Unconfirmed);
            document.Items.Should().ContainSingle(item => item.ProductId == productId &&
                                                          item.Amount == 1 &&
                                                          item.FinalPrice == 5m &&
                                                          item.DiscountReason == "Test message");

            @event.Should().BeOfType<CouponApplied>()
                .Which.CartId.Should().Be(cartId);
        }
    }
}
