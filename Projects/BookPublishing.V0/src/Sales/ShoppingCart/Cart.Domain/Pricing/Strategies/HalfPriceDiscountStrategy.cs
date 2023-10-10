using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Domain.ShoppingCart;

namespace Cart.Domain.Pricing.Strategies
{
    public class HalfPriceDiscountStrategy : IPricingStrategy
    {
        public List<CartItem> CalculatePrices(ICart cart)
        {
            var result = new List<CartItem>();
            foreach (var item in cart.Items)
            {
                result.Add(item.ProductType is ProductType.Web or ProductType.EBook
                    ? CartItem.WithNewDiscount(item,
                        item.Price * 0.5m,
                        "HALFPRICE coupon applied.")
                    : CartItem.WithoutNewDiscount(item));
            }
            return result;
        }
    }
}
