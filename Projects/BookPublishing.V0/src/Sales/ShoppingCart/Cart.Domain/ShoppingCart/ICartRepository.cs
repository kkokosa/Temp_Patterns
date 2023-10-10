namespace Cart.Domain.ShoppingCart;

public interface ICartRepository
{
    Task<Cart?> GetById(Guid cartId);
}