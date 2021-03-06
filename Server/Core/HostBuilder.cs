using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradingEngine.Shared.Logging.Configuration;
using TradingEngine.Shared.Logging.Loggers;
using TradingEngine.Shared.Logging.Utilities;
using ILogger = TradingEngine.Shared.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace TradingEngine.Server.Core
{
    public sealed class HostBuilder
    {
        public static IHost BuildTradingEngineServer()
            => Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    // Start with configuration
                    services.AddOptions();
                    services.Configure<Configuration.GlobalConfiguration>(ctx.Configuration.GetSection(nameof(Configuration.GlobalConfiguration)));
                    var loggingConfig = ctx.Configuration.GetSection(nameof(LoggingConfiguration));
                    services.Configure<LoggingConfiguration>(loggingConfig);
                    var loggerType = loggingConfig.GetValue<LoggerType>("LoggerType");
                    var forwardToConsole = loggingConfig.GetValue<bool>("ForwardToConsole");

                    // Add Singleton Object

                    services.AddSingleton<ITradingEngineServer, TradingEngineServer>();

                    switch (loggerType)
                    {
                        case LoggerType.Text:
                            services.AddSingleton<ILogger, TextLogger>();
                            break;
                        case LoggerType.Database:
                            services.AddSingleton<ILogger, DatabaseLogger>();
                            break;
                        default:
                            if (forwardToConsole) break;
                            services.AddSingleton<ILogger, ConsoleLogger>();
                            break;
                    }

                    // Add Hosted Service
                    services.AddHostedService<TradingEngineServer>();

                    #if DEBUG
                        services.AddLogging(l =>
                            l.AddConsole().SetMinimumLevel(LogLevel.Debug));
                    #endif
                }).Build();
    }
}
