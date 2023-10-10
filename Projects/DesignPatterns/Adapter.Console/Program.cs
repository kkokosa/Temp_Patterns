using Autofac;
using Serilog;

var serilogLogger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Register services
var builder = new ContainerBuilder();

// We can't register Serilog.ILogger as ILogger
// builder.RegisterInstance(serilogLogger).As<ILogger>();
builder.RegisterInstance(serilogLogger).As<Serilog.ILogger>();

//builder.RegisterType<SerilogLoggerAdapter>().As<ILogger>();
builder.RegisterAdapter<Serilog.ILogger, ILogger>(sl => new SerilogLoggerAdapter(sl));

builder.RegisterType<SampleHandler>().As<IHandler>();
var container = builder.Build();

// Simulate HTTP request
using (var scope = container.BeginLifetimeScope())
{
    var handler = scope.Resolve<IHandler>();
    handler.HandleRequest($"Request {Guid.NewGuid()}");
}