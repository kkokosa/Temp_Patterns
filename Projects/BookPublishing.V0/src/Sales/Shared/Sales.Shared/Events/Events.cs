namespace Sales.Shared.Events
{
    public record CartCreated(Guid Id);
    public record CartItemAdded(Guid CartId, Guid ProductId, uint Amount);
    public record CartCheckedOut(Guid CartId);

    public record CouponApplied(Guid CartId);

}
