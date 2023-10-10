public class SerilogLoggerAdapter : ILogger // ILogger is the Target
{ 
    private Serilog.ILogger _logger; // Adaptee
    public SerilogLoggerAdapter(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message)
    {
        _logger.Information(message);
    }
}