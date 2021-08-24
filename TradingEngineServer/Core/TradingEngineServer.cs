using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TradingEngineServer.Core.Configuration;

namespace TradingEngineServer.Core
{
    internal sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {
        private readonly ILogger<TradingEngineServer> _logger;
        private readonly TradingEngineServerConfiguration _config;
        public TradingEngineServer(ILogger<TradingEngineServer> logger, 
            IOptions<TradingEngineServerConfiguration> config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
