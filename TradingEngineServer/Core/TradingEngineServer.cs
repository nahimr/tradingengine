using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;

namespace TradingEngineServer.Core
{
    internal sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {
        private readonly ILogger<TradingEngineServer> _logger;
        private readonly GlobalConfiguration _config;
        public TradingEngineServer(ILogger<TradingEngineServer> logger, 
            IOptions<GlobalConfiguration> config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(_config.ToString());
            _logger.LogInformation($"Started {nameof(TradingEngineServer)} !");

            while (!stoppingToken.IsCancellationRequested)
            {
                
            }

            _logger.LogInformation($"Stopped {nameof(TradingEngineServer)} !");

            return Task.CompletedTask;
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);
    }
}
