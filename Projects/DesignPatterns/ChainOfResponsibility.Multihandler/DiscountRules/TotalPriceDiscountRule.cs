using System;
using System.Collections.Generic;
using System.Text;

namespace ChainOfResponsibility.Multihandler.DiscountRules
{
    public class TotalPriceDiscountRule : IDiscountRule
    {
        private readonly decimal goodPriceThreshold;
        private readonly decimal discount;

        public TotalPriceDiscountRule(decimal goodPriceThreshold, decimal discount)
        {
            this.goodPriceThreshold = goodPriceThreshold;
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
            return order.CurrentPrice >= goodPriceThreshold;
        }
    }
}
