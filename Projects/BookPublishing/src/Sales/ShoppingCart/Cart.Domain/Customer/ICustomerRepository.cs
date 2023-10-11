using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Customer
{
    public interface ICustomerRepository
    {
        Task<Customer> GetById(Guid id);
    }
}
