using System.Linq.Expressions;
using Shared.Domain;

namespace Cart.Domain.ShoppingCart
{
    public class CartItemSpecification : Specification<CartItem>
    {
        private readonly ProductType _type;

        public CartItemSpecification(ProductType Type)
        {
            _type = Type;
        }

        public override Expression<Func<CartItem, bool>> ToExpression() 
            => item => item.ProductType == _type;

        public static Specification<CartItem> Any 
            => new AnySpecification<CartItem>();
    }
}
