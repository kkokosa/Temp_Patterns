using Alba;
using Cart.Application.Commands;
using FluentAssertions;
using Oakton;
using Testcontainers.PostgreSql;

namespace Sales.Api.Tests
{
    public class ShoppingCartTests : IClassFixture<WebAppFixture>
    {
        public ShoppingCartTests(WebAppFixture app)
        {
            _host = app.AlbaHost;
        }
        private readonly IAlbaHost _host;

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GivenHost_WhenGetCarts_ThenReceivesSomeCarts()
        {
            var result = await _host.Scenario(_ =>
            {
                _.Get.Url("/carts");
                _.StatusCodeShouldBeOk();
            });
            var carts = await result.ReadAsJsonAsync<Cart.Domain.ShoppingCart.Cart[]>();
            carts.Should().NotBeNull();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GivenHost_WhenAddNewCart_ThenWeCanReceiveIt()
        {
            var cartId = Guid.NewGuid();
            var customerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var request = new CreateCart(cartId, customerId);

            var createResult = await _host.Scenario(_ =>
            {
                _.Post.Json(request).ToUrl("/carts");
                _.StatusCodeShouldBe(201);
            });

            var createCartResponse = await createResult.ReadAsJsonAsync<CreateCartResponse>();
            createCartResponse.Id.Should().Be(cartId);
            createCartResponse.Url.Should().Be($"/carts/{cartId}");

            var getResult = await _host.Scenario(_ =>
            {
                _.Get.Url($"/carts/{cartId}");
                _.StatusCodeShouldBeOk();
            });
            var cart = await getResult.ReadAsJsonAsync<Cart.Domain.ShoppingCart.Cart>();
            cart.Should().NotBeNull();
            cart.Id.Should().Be(cartId);
            cart.CustomerId.Should().Be(customerId);
            cart.Items.Should().BeEmpty();
        }
    }

    public class WebAppFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithExposedPort("5432")
            .Build();

        public IAlbaHost AlbaHost = null!;

        public async Task InitializeAsync()
        {
            // Will give us +4s one-time cost for tests execution
            await _postgres.StartAsync();

            OaktonEnvironment.AutoStartHost = true;
            AlbaHost = await Alba.AlbaHost.For<global::Program>(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.CustomizeMarten(_postgres.GetConnectionString());
                });
            });
        }

        public async Task DisposeAsync()
        {
            await AlbaHost.DisposeAsync();

            await _postgres.DisposeAsync();
        }
    }

}