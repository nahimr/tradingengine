using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HostBuilder = TradingEngineServer.Core.HostBuilder;
using ServiceProvider = TradingEngineServer.Core.ServiceProvider;

using var engine = HostBuilder.BuildTradingEngineServer();
ServiceProvider.IServiceProvider = engine.Services;
{
    using var scope = ServiceProvider.IServiceProvider.CreateScope();
    await engine.RunAsync().ConfigureAwait(false);
}
