using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingEngineServer.Core.Configuration;

namespace TradingEngineServer.Core
{
    public sealed class TradingEngineServerHostBuilder
    {
        public static IHost BuildTradingEngineServer()
            => Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    // Start with configuration
                    services.AddOptions();
                    services.Configure<TradingEngineServerConfiguration>(ctx.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));

                    // Add Singleton Object
                    services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
                    
                    // Add Hosted Service
                    services.AddHostedService<TradingEngineServer>();
                }).Build();
    }
}
