namespace TradingEngine.Shared.Logging.Utilities
{
    public enum LoggerType
    {
        Text,
        Database, // Not implementing
        Trace, // Not implementing
        Console,
    }

    public delegate ILogger ServiceResolver(LoggerType serviceType);

}
