using Cart.Application;
using Cart.Domain.Customer;
using Marten;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Oakton;
using Oakton.Resources;
using Orders.Application;
using Orders.Application.Handlers;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.Marten;

// TODO:
// - use https://fluentassertions.com/ in tests
// - use https://github.com/oskardudycz/Ogooreck
// - use https://github.com/bchavez/Bogus

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

CartsModule.RegisterCartsModule(builder.Services);
OrdersModule.RegisterOrdersModule(builder.Services);

builder.Services.CustomizeMarten(builder
    .Configuration
    .GetConnectionString("postgres"));

builder.Services.AddResourceSetupOnStartup();
if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith(
        new PopulateTestingData(InitialDatasets.Customers),
        new PopulateTestingData(InitialDatasets.Products));
}

builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.Policies.AutoApplyTransactions();
    opts.Policies.UseDurableLocalQueues();

    opts.Discovery.IncludeAssembly(typeof(CartsModule).Assembly);
    opts.Discovery.IncludeAssembly(typeof(OrdersModule).Assembly);

    // This *temporary* line of code will write out a full report about why or 
    // why not Wolverine is finding this handler and its candidate handler messages
    //Console.WriteLine(opts.DescribeHandlerMatch(typeof(ShoppingCartCheckedOutHandler)));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

#if false
    var store = app.Services.GetRequiredService<IDocumentStore>();
    await store.Advanced.ResetAllData();
#endif
}


// Anti-pattern from Jeremy's POV - using Wolverine just as mediator
// app.MapPost("/orders", async (CreateOrder body, IMessageBus bus) => await bus.InvokeAsync<Guid>(body));
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapWolverineEndpoints(opts =>
{
    opts.UseFluentValidationProblemDetailMiddleware();
    //opts.UseNewtonsoftJsonForSerialization(settings => settings.TypeNameHandling = TypeNameHandling.All);
});

return await app.RunOaktonCommands(args);

public static class MartenConfiguration
{
    public static void CustomizeMarten(this IServiceCollection services, string connectionString)
    {
        services.AddMarten(opts =>
            {
                opts.Connection(connectionString);
                opts.DatabaseSchemaName = "orders";
                opts.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.All);
            })
            // Optionally add Marten/Postgresql integration
            // with Wolverine's outbox
            .IntegrateWithWolverine();
    }
}