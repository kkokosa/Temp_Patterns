using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Promotions;
using Cart.Domain.Shipping;
using Sales.Shared.Exceptions;
using System;

namespace Cart.Domain.ShoppingCart;

public class CartFactory : ICartFactory
{
    private readonly IDeliveryService _deliveryService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPromotionsService _promotionsService;
    private readonly TimeProvider _timeProvider;

    public CartFactory(IDeliveryService deliveryService,
        ICustomerRepository customerRepository, 
        IPromotionsService promotionsService, 
        TimeProvider timeProvider)
    {
        _deliveryService = deliveryService;
        _customerRepository = customerRepository;
        _promotionsService = promotionsService;
        _timeProvider = timeProvider;
    }

    public async Task<Domain.ShoppingCart.Cart> CreateCart(
        Guid? id,
        Guid customerId)
    {
        var customer = await _customerRepository.GetById(customerId);
        if (customer is null)
            throw new ObjectDoesNotExistException<Customer.Customer>(id);

        var customerType = customer.Type;

        IShippingCostStrategy shipping = customerType switch
        {
            CustomerType.NormalSubscription => new RegularShippingCost(_deliveryService),
            CustomerType.BusinessSubscription => new DiscountedShippingCost(1.0m, _deliveryService),
            CustomerType.WebSubscription => new RegularShippingCost(_deliveryService),
            _ => throw new ArgumentOutOfRangeException(nameof(customerType), customerType, null)
        };
        var pricing = await GetCurrentPricing(customerType);
        var cart = new Domain.ShoppingCart.Cart( id ?? Guid.NewGuid(), customerId, shipping);
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
        };
        var additionalPricing = await _promotionsService.GetPromotions(_timeProvider.GetUtcNow());
        additionalPricing.Add(baseCustomerPricing);
        return additionalPricing;
    }
}