using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

                    // Add Singleton Object
                    services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
                    
                    // Add Hosted Service
                    services.AddHostedService<TradingEngineServer>();

                    #if DEBUG
                        services.AddLogging(l =>
                            l.AddConsole().SetMinimumLevel(LogLevel.Debug));
                    #endif
                }).Build();
    }
}
