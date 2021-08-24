using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TradingEngine.Server.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngine.Server.Core
{
    internal sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {
        private readonly ILogger _logger;
        private readonly GlobalConfiguration _config;
        public TradingEngineServer(ILogger logger,
            IOptions<GlobalConfiguration> config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Debug(GetType().Namespace, _config.ToString());
            _logger.Information(GetType().Namespace, $"Started {nameof(TradingEngineServer)} !");
            UInt16 test = 0;
            
            while (!stoppingToken.IsCancellationRequested)
            {
                if (test > 500) Dispose();
                test++;
                _logger.Information(nameof(TradingEngineServer), $"test: {test}");
            }

            _logger.Information(GetType().Namespace,$"Stopped {nameof(TradingEngineServer)} !");

            return Task.CompletedTask;
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);
    }
}
