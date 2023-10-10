using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.Application
{
    public static class OrdersModule
    {
        public static IServiceCollection RegisterOrdersModule(this IServiceCollection services,
            IConfiguration? configuration = default)
        {
            //services.AddSingleton(TimeProvider.System);
            return services;
        }
    }
}
