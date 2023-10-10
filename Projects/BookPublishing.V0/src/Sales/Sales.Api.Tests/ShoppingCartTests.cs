using Alba;
using Cart.Application.Commands;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Oakton;
using Wolverine.Tracking;

namespace Sales.Api.Tests
{
    public class ShoppingCartTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task GivenHost_WhenGetCarts_ThenReceivesSomeCarts()
        {
            OaktonEnvironment.AutoStartHost = true;
            await using var host = await AlbaHost.For<global::Program>(builder =>
            {
                builder.ConfigureServices(services => {
                    //    services.AddSingleton<IAccountService, StubAccountService>()
                });
            });            

            var carts = await host.GetAsJson<Cart.Domain.ShoppingCart.Cart[]>("/carts");

        }
    }
}