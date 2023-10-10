using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.Customer;
using Marten;
using Sales.Shared.Exceptions;

namespace Cart.Application.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDocumentSession _session;

        public CustomerRepository(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Customer> GetById(Guid id)
        {
            var customer = await _session.LoadAsync<Customer>(id);
            if (customer == null)
                throw new ObjectDoesNotExistException<Customer>(id);
            return customer;
        }
    }
}
