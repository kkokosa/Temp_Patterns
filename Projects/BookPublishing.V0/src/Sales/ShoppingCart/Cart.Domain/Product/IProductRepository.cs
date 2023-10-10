using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Product
{
    public interface IProductRepository
    {
        Task<Product> GetById(Guid id);
    }
}
