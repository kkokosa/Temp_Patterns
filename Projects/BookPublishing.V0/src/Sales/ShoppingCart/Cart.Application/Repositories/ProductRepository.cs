using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.Customer;
using Cart.Domain.Product;
using Marten;
using Sales.Shared.Exceptions;

namespace Cart.Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDocumentSession _session;

        public ProductRepository(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Product> GetById(Guid id)
        {
            var product = await _session.LoadAsync<Product>(id);
            if (product == null)
                throw new ObjectDoesNotExistException<Product>(id);
            return product;
        }
    }
}
