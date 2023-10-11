namespace Cart.Domain.Customer;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CustomerType Type { get; set; }
}

public enum CustomerType
{
    NormalSubscription,
    BusinessSubscription,
    WebSubscription
}
