using Cart.Domain.Pricing;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.Shipping;

namespace Cart.Domain.ShoppingCart
{
    public class Cart : ICart
    {
        private List<CartItem> _items = new();

        public Guid Id { get; private set; }
        public State State { get; private set; }

        public Guid CustomerId { get; private set; }

        public IReadOnlyCollection<CartItem> Items => _items;

        private List<IPricingStrategy> _pricing = new();

        private IShippingCostStrategy _costStrategy;

        private Cart()
        {
        }

        internal Cart(Guid orderId,
            Guid customerId,
            IShippingCostStrategy costStrategy)
        {
            Id = orderId;
            CustomerId = customerId;
            State = State.Unconfirmed;
            _costStrategy = costStrategy;
        }

        internal Cart(Guid orderId,
            Guid customerId,
            IShippingCostStrategy costStrategy,
            List<CartItem> items)
        {
            Id = orderId;
            CustomerId = customerId;
            State = State.Unconfirmed;
            _costStrategy = costStrategy;
            _items = items;
        }

        public void AddItem(Guid productId, ProductType type, decimal price, uint amount)
        {
            var existingItem  = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Amount += amount;
                return;
            }
            var item = new CartItem(productId, type, amount, price);
            _items.Add(item);
            RecalculatePrices();
        }

        public decimal CalculateShippingCost()
        {
            return _costStrategy.CalculateShippingCost(this);
        }

        public void AddPricing(IPricingStrategy pricing)
        {
            _pricing.Add(pricing);
            RecalculatePrices();
        }

        public void ApplyPricing(List<IPricingStrategy> pricing)
        {
            _pricing = pricing;
            RecalculatePrices();
        }

        private void RecalculatePrices()
        {
            List<CartItem>? winningItems = null;
            var minimumCost = decimal.MaxValue;
            foreach (var pricing in _pricing)
            {
                var pricedItems = pricing.CalculatePrices(this);
                var totalCost = pricedItems.Sum(item => item.FinalPrice);
                if (totalCost < minimumCost)
                {
                    winningItems = pricedItems;
                    minimumCost = totalCost;
                }
            }
            _items = winningItems ?? _items;
        }

    }
}
