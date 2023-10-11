using System;
using System.Collections.Generic;

namespace ChainOfResponsibility.Multihandler
{
    public class DiscountService
    {
        private readonly List<IDiscountRule> discountRules;

        public DiscountService(List<IDiscountRule> discountRules)
        {
            this.discountRules = discountRules;
        }

        public void CalculateDiscount(Order order)
        {
            // Process order through all the rules, until one terminates (all all if none will do)
            foreach (var discountRule in discountRules)
            {
                bool continueProcessing = true;
                if (discountRule.CanApply(order))
                {
                    continueProcessing = discountRule.Apply(order);
                }
                if (!continueProcessing)
                    break;
            }
        }
    }
}
