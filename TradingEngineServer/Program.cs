using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HostBuilder = TradingEngine.Server.Core.HostBuilder;
using ServiceProvider = TradingEngine.Server.Core.ServiceProvider;

using var engine = HostBuilder.BuildTradingEngineServer();
ServiceProvider.IServiceProvider = engine.Services;
{
    using var scope = ServiceProvider.IServiceProvider.CreateScope();
    await engine.RunAsync().ConfigureAwait(false);
}
