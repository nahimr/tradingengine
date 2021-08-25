using System;
using Microsoft.Extensions.Logging;

namespace TradingEngine.Shared.Logging.Utilities
{
    public record LogInformation(LogLevel LogLevel, string Module, string Message, DateTime Now, int ThreadId, string ThreadName);
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { };
}