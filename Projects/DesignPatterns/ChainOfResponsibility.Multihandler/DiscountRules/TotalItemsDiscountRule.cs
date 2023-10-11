using System;

namespace ChainOfResponsibility.Multihandler.DiscountRules
{
    public class TotalItemsDiscountRule : IDiscountRule
    {
        private readonly int itemsCountThreshold;
        private readonly decimal discount;

        public TotalItemsDiscountRule(int itemsCountThreshold, decimal discount)
        {
            this.itemsCountThreshold = itemsCountThreshold;
            this.discount = discount;
        }

        public Guid Id { get; }

        public bool Apply(Order order)
        {
            order.CurrentPrice *= discount;
            order.TallyDiscount(Id);
            return true;
        }

        public bool CanApply(Order order)
        {
            return order.TotalItems > itemsCountThreshold;
        }
    }
}