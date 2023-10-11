using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.ShoppingCart;
using Marten;
using Sales.Shared.Exceptions;

namespace Cart.Application.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IPricingService _pricingService;
        private readonly IDocumentSession _session;
        private readonly ICustomerRepository _customerRepository;

        public CartRepository(IDocumentSession session,
            IPricingService pricingService,
            ICustomerRepository customerRepository)
        {
            _session = session;
            _pricingService = pricingService;
            _customerRepository = customerRepository;
        }

        public async Task<Domain.ShoppingCart.Cart> GetById(Guid cartId)
        {
            var cart = await _session.LoadAsync<Domain.ShoppingCart.Cart>(cartId);
            if (cart == null)
                throw new ObjectDoesNotExistException<Domain.ShoppingCart.Cart>(cartId);

            var customer = await _customerRepository.GetById(cart.CustomerId);
            var pricing = await _pricingService.GetCurrentPricing(customer.Type);
            cart.ApplyPricing(pricing);
            return cart;
        }
    }
}
