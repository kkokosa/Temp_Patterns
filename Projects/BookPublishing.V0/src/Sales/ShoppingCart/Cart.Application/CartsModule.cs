using Cart.Application.Commands;
using Cart.Application.Repositories;
using Cart.Domain;
using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Product;
using Cart.Domain.Promotions;
using Cart.Domain.ShoppingCart;
using Cart.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.Application
{
    public static class CartsModule
    {
        public static IServiceCollection RegisterCartsModule(this IServiceCollection services, 
            IConfiguration? configuration = default)
        {
            services.AddSingleton(TimeProvider.System);
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartFactory, CartFactory>();

            services.AddScoped<IPromotionsService, PromotionsService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddSingleton<IDeliveryService, DhlDeliveryService>();
            return services;
        }
    }
}
