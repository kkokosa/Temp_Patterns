using Cart.Domain.Customer;
using Cart.Domain.Pricing;
using Cart.Domain.Shipping;
using Sales.Shared.Exceptions;

namespace Cart.Domain.ShoppingCart;

public class CartFactory : ICartFactory
{
    private readonly IPricingService _pricingService;
    private readonly IDeliveryService _deliveryService;
    private readonly ICustomerRepository _customerRepository;

    public CartFactory(IPricingService pricingService,
        IDeliveryService deliveryService,
        ICustomerRepository customerRepository)
    {
        _pricingService = pricingService;
        _deliveryService = deliveryService;
        _customerRepository = customerRepository;
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
        var pricing = await _pricingService.GetCurrentPricing(customerType);
        var cart = new Domain.ShoppingCart.Cart( id ?? Guid.NewGuid(), customerId, shipping);
        cart.ApplyPricing(pricing);
        return cart;
    }
}