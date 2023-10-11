using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;
using Moq;

namespace Cart.Domain.Tests.Shipping
{
    public class CartCreation
    {
        [Fact]
        public async Task Given_InitialOrder_When_CheckingState_Then_ShouldBeCreatedAndEmpty()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var deliveryService = new Mock<IDeliveryService>();
            var pricingService = new Mock<IPricingService>();
            pricingService.Setup(x => x.GetCurrentPricing(CustomerType.NormalSubscription))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                    new DefaultPricingStrategy()
                });
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer.Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var factory = new CartFactory(pricingService.Object, deliveryService.Object, customerRepository.Object);

            var cart = await factory.CreateCart(id,
                customerId);

            Assert.Equal(id, cart.Id);
            Assert.Equal(customerId, cart.CustomerId);
            Assert.Equal(State.Unconfirmed, cart.State);
            Assert.Equal(0, cart.Items.Count);
        }
    }
}