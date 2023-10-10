public class SampleHandler : IHandler
{
    private readonly ILogger _logger;

    public SampleHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void HandleRequest(string request)
    {
        _logger.LogInfo($"Handling request {request}");
    }
}