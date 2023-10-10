using FluentAssertions;
using Cart.Application.Commands;
using Moq;
using Wolverine.Marten;
using Cart.Domain.Pricing;
using Cart.Domain.ShoppingCart;
using Cart.Infrastructure.Services;
using Cart.Domain.Customer;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Promotions;
using Castle.Core.Resource;
using Microsoft.Extensions.Time.Testing;
using Sales.Shared.Events;

namespace Cart.Application.Tests
{
    public class CreatingCarts
    {
        [Fact]
        public async Task CreateCartCommand()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new CreateCart(id, customerId);
            var handler = new CreateCartEndpoint();
            var promotionsService = new Mock<IPromotionsService>();
            promotionsService.Setup(x => x.GetPromotions(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new List<IPricingStrategy>
                {
                });
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer()
            {
                Type = CustomerType.NormalSubscription
            });
            var fakeTimeProvider = new FakeTimeProvider();

            var factory = new CartFactory(new DhlDeliveryService(), customerRepository.Object, promotionsService.Object, fakeTimeProvider);

            var (response, store, @event) = await handler.Handle(command, factory);

            response.Should().BeOfType<CreateCartResponse>()
                .Which.Id.Should().Be(id);
            store.Should().BeOfType<StoreDoc<Domain.ShoppingCart.Cart>>()
                .Which.Document.Should().BeOfType<Domain.ShoppingCart.Cart>()
                .Which.Id.Should().Be(id);
            @event.Should().BeOfType<CartCreated>()
                .Which.Id.Should().Be(id);
        }

        [Fact]
        public void CreateCartValidator()
        {
            var validator = new CreateCartValidator();

            var result = validator.Validate(new CreateCart(Guid.Empty, Guid.Empty));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Id");
        }

        [Fact]
        public async Task CartForWebSubscription()
        {
            var customerId = Guid.NewGuid();
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetById(customerId)).ReturnsAsync(new Customer()
            {
                Type = CustomerType.WebSubscription
            });
            
            var promotionsService = new Mock<IPromotionsService>();
            promotionsService.Setup(x => x.GetPromotions(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new List<IPricingStrategy>());
            var fakeTime = new FakeTimeProvider();

            var factory = new CartFactory(new DhlDeliveryService(), customerRepository.Object, promotionsService.Object, fakeTime);
            var cart = await factory.CreateCart(Guid.NewGuid(), customerId);
            cart.AddItem(Guid.NewGuid(), ProductType.Web, 10.0m, 1);
            cart.AddItem(Guid.NewGuid(), ProductType.EBook, 10.0m, 1);
            cart.AddItem(Guid.NewGuid(), ProductType.PrintedBook, 10.0m, 1);

            cart.Items.Should().BeEquivalentTo(new[]
            {
                new { ProductType = ProductType.Web, FinalPrice = 0.0m, DiscountReason = "Fixed pricing rule." },
                new { ProductType = ProductType.EBook, FinalPrice = 10.0m, DiscountReason = "" },
                new { ProductType = ProductType.PrintedBook, FinalPrice = 9.0m, DiscountReason = "Web subscription!" }
            });
        }
    }
}