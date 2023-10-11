using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain;

namespace Cart.Infrastructure.Services
{
    public class DhlDeliveryService : IDeliveryService
    {
        public decimal GetShippingCost(decimal weight, string address)
        {
            return 10.0m * weight;
        }
    }
}
