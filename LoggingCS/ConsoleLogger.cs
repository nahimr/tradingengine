using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace TradingEngineServer.Logging
{
    public class ConsoleLogger : AbstractLogger
    {
        private readonly ILogger<ConsoleLogger> _logger;
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new object();
        private bool _disposed = false;

        public ConsoleLogger(ILogger<ConsoleLogger> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ = LogAsync(_tokenSource.Token);
        }

        protected override void Log(LogLevel level, string module, string message)
        {
            _logQueue.Post(new LogInformation(level, module, message, DateTime.Now,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
            
        }

        private async Task LogAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var logItem = await _logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    var formattedMessage = FormatLogItem(logItem);
                    _logger.Log(logItem.LogLevel, formattedMessage);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string FormatLogItem(LogInformation logItem)
        {
            var msgRegex = Regex.Replace(logItem.Message, "(\\n|\\t)+", string.Empty);
            return $"[{logItem.Now:yyyy-MM-dd HH-mm-ss.fffffff}][{logItem.Module}][{logItem.ThreadId:000}] {msgRegex}\n";
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
            }

            if (!disposing) return;

            // Get rid of managed resources
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            // Get rid of unmanaged resources

        }

        ~ConsoleLogger()
        {
            Dispose(false);
        }
    }
}
