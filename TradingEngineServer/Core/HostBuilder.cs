using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.Configuration;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace TradingEngineServer.Core
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
                    services.Configure<LoggingConfiguration>(ctx.Configuration.GetSection(nameof(LoggingConfiguration)));

                    // Add Singleton Object
                    services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
                    services.AddSingleton<ITextLogger, TextLogger>();
                    
                    // Add Hosted Service
                    services.AddHostedService<TradingEngineServer>();

                    #if DEBUG
                        services.AddLogging(l =>
                            l.AddConsole().SetMinimumLevel(LogLevel.Debug));
                    #endif
                }).Build();
    }
}
