using Cart.Domain.Customer;
using Cart.Domain.Product;
using Cart.Domain.ShoppingCart;
using JasperFx.CodeGeneration.Frames;
using Marten;
using Marten.Schema;

public class PopulateTestingData : IInitialData
{
    private readonly object[] _initialData;

    public PopulateTestingData(params object[] initialData)
    {
        _initialData = initialData;
    }

    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();
        session.Store(_initialData);
        await session.SaveChangesAsync();
    }
}

public static class InitialDatasets
{
    public static readonly Customer[] Customers =
    {
        new Customer()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Type = CustomerType.NormalSubscription,
            Name = "John Normals"
        }
    };

    public static readonly Product[] Products =
    {
        new Product()
        {
            Id = Guid.Parse("21a3c24e-f317-1237-31cd-34413d4ecadb"),
            Type = ProductType.PrintedBook,
            Price = 100.0m,
            Name = "Pro .NET Memory Management"
        },
        new Product()
        {
            Id = Guid.Parse("31a3c24e-f317-1237-31cd-34413d4ecadb"),
            Type = ProductType.EBook,
            Price = 100.0m,
            Name = "Pro .NET Memory Management"
        }
    };
}