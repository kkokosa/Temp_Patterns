using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.Pricing.Strategies;
using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing
{
    public class PricingStrategyBuilder
    {
        private IPricingStrategy _strategy;
        public PricingStrategyBuilder() { }

        public IPricingStrategy Result => _strategy;

        public PricingStrategyBuilder AddFixedPricing(decimal price, CartItemSpecification cartItemSpecification)
        {
            _strategy = new FixedPricingStrategy(price, cartItemSpecification, _strategy);
            return this;
        }

        public PricingStrategyBuilder AddDiscount(decimal discountByPerce, string reasonMessage, CartItemSpecification cartItemSpecification)
        {
            _strategy = new DiscountForSpecifiedProductsStrategy(discountByPerce, reasonMessage,
                cartItemSpecification, _strategy);
            return this;
        }

        public PricingStrategyBuilder AddFirstBookFree()
        {
            _strategy = new FirstBookFreePricingStrategy(_strategy);
            return this;
        }
    }
}
