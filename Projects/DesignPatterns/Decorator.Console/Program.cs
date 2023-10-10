using Autofac;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

// Register services
var builder = new ContainerBuilder();

// We can't register Serilog.ILogger as ILogger
// builder.RegisterInstance(serilogLogger).As<ILogger>();
builder.RegisterInstance(serilogLogger).As<Serilog.ILogger>();

builder.RegisterType<SerilogLoggerAdapter>().As<ILogger>();
//builder.RegisterAdapter<Serilog.ILogger, ILogger>(sl => new SerilogLoggerAdapter(sl));

builder.RegisterType<SampleHandler>().As<IHandler>();
builder.RegisterDecorator<DiagnosticDecorator, IHandler>();
builder.RegisterDecorator<CacheDecorator, IHandler>();

builder.RegisterType<MemoryCacheProvider>().As<ICacheProvider>();
var container = builder.Build();

// Simulate HTTP request
using (var scope = container.BeginLifetimeScope())
{
    var guid = Guid.NewGuid();
    var handler = scope.Resolve<IHandler>();

    handler.HandleRequest($"Request {guid}");
    handler.HandleRequest($"Request {Guid.NewGuid()}");
    handler.HandleRequest($"Request {guid}");
}

public class DiagnosticDecorator : IHandler
{
    private readonly IHandler _handler;
    private readonly ILogger _logger;

    public DiagnosticDecorator(IHandler handler, ILogger logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public void HandleRequest(string request)
    {
        _logger.LogDebug($"Before request {request}");
        _handler.HandleRequest(request);
        _logger.LogDebug($"After request {request}");
    }
}

public class CacheDecorator : IHandler
{
    private readonly IHandler _handler;
    private readonly ILogger _logger;
    private readonly ICacheProvider _cacheProvider;

    public CacheDecorator(IHandler handler, ILogger logger, ICacheProvider cacheProvider)
    {
        _handler = handler;
        _logger = logger;
        _cacheProvider = cacheProvider;
    }

    public void HandleRequest(string request)
    {
        _logger.LogDebug($"Cache check for request {request}");
        var cachedResponse = _cacheProvider.Get(request);
        if (cachedResponse is not null)
        {
            _logger.LogDebug($"Cache FOUND for request {request}");
            return;
        }
        _handler.HandleRequest(request);
        _cacheProvider.Add(request, request);
        _logger.LogDebug($"Cache stored for request {request}");
    }
}

public interface ICacheProvider
{
    void Add(string key, object value);
    object? Get(string key);
}

public class MemoryCacheProvider : ICacheProvider
{
    private readonly Dictionary<string, object> _cache = new();

    public void Add(string key, object value)
    {
        _cache[key] = value;
    }

    public object? Get(string key) 
        => _cache.TryGetValue(key, out var value) ? value : null;
}
