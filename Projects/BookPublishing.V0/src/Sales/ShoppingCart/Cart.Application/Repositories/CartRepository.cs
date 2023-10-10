using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;
using Marten;
using Sales.Shared.Exceptions;
using System;
using Cart.Domain.Promotions;

namespace Cart.Application.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IPromotionsService _promotionsService;
        private readonly IDocumentSession _session;
        private readonly ICustomerRepository _customerRepository;
        private readonly TimeProvider _timeProvider;

        public CartRepository(IDocumentSession session,
            ICustomerRepository customerRepository, 
            IPromotionsService promotionsService, 
            TimeProvider timeProvider)
        {
            _session = session;
            _customerRepository = customerRepository;
            _promotionsService = promotionsService;
            _timeProvider = timeProvider;
        }

        public async Task<Domain.ShoppingCart.Cart> GetById(Guid cartId)
        {
            var cart = await _session.LoadAsync<Domain.ShoppingCart.Cart>(cartId);
            if (cart == null)
                throw new ObjectDoesNotExistException<Domain.ShoppingCart.Cart>(cartId);

            var customer = await _customerRepository.GetById(cart.CustomerId);
            var pricing = await GetCurrentPricing(customer.Type);
            cart.ApplyPricing(pricing);
            return cart;
        }

        private async Task<List<IPricingStrategy>> GetCurrentPricing(CustomerType customerType)
        {
            IPricingStrategy baseCustomerPricing = customerType switch
            {
                CustomerType.NormalSubscription
                    => new DefaultPricingStrategy(),
                CustomerType.BusinessSubscription
                    => new BusinessPricingStrategy(5.0m),
                CustomerType.WebSubscription
                    => new WebPricingStrategy(),
                _ => throw new ArgumentOutOfRangeException(nameof(customerType), customerType, null)
            }; ;
            var additionalPricing = await _promotionsService.GetPromotions(_timeProvider.GetUtcNow());
            additionalPricing.Add(baseCustomerPricing);
            return additionalPricing;
        }
    }
}
