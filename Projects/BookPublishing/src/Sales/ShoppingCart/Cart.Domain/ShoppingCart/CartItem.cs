namespace Cart.Domain.ShoppingCart;

public class CartItem(Guid productId, ProductType productType, uint amount, decimal price)
{
    public Guid ProductId { get; init; } = productId;
    public ProductType ProductType { get; init; } = productType;
    public decimal Price { get; init; } = price;
    public uint Amount { get; set; } = amount;
    public decimal FinalPrice { get; set; }
    public string DiscountReason { get; set; }

    public static CartItem WithOriginalPrice(CartItem item)
        => new CartItem(item.ProductId, item.ProductType, item.Amount, item.Price)
        {
            FinalPrice = item.Price,
            DiscountReason = string.Empty
        };

    public static CartItem WithNewDiscount(CartItem item, decimal finalPrice, string reason)
        => new CartItem(item.ProductId, item.ProductType, item.Amount, item.Price)
        {
            FinalPrice = finalPrice,
            DiscountReason = reason
        };

    public static CartItem WithoutNewDiscount(CartItem item)
        => new CartItem(item.ProductId, item.ProductType, item.Amount, item.Price)
        {
            FinalPrice = string.IsNullOrEmpty(item.DiscountReason) ? item.Price : item.FinalPrice,
            DiscountReason = item.DiscountReason ?? string.Empty
        };
}

public enum ProductType
{
    PrintedBook,
    EBook,
    Web
}