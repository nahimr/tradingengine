using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using TradingEngine.Shared.Logging.Configuration;
using TradingEngine.Shared.Logging.Utilities;

namespace TradingEngine.Shared.Logging.Loggers
{
    public sealed class TextLogger : AbstractLogger
    {
        private readonly LoggingConfiguration _config;
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new object();
        private bool _disposed = false;
        public TextLogger(IOptions<LoggingConfiguration> config) : base()
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
            if (_config.LoggerType != LoggerType.Text)
            {
                throw new InvalidOperationException(
                    $"{nameof(TextLogger)} doesn't match LoggerType of {_config.LoggerType}");
            }

            var now = DateTime.Now;

            var logDirectory = Path.Combine(_config.TextLoggerConfiguration.Directory, $"{now:yyyy-MM-dd}");
            var uniqueLogName = $"{_config.TextLoggerConfiguration.FileName}-{now:HH_mm_ss}";
            var baseLogName = $"{uniqueLogName}{_config.TextLoggerConfiguration.FileExtension}";
            var filePath = Path.Combine(logDirectory, baseLogName);

            Directory.CreateDirectory(logDirectory);
            _ = Task.Run(() => LogAsync(filePath, _logQueue, _tokenSource.Token));
        }

        ~TextLogger()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
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

        private static async Task LogAsync(string filepath, BufferBlock<LogInformation> logQueue, CancellationToken token)
        {
            await using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            await using var sr = new StreamWriter(fs) { AutoFlush = true };

            try
            {
                while (true)
                {
                    var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    var formattedMessage = FormatLogItem(logItem);
                    await sr.WriteAsync(formattedMessage).ConfigureAwait(false);
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
            return $"[{logItem.Now:yyyy-MM-dd HH-mm-ss.fffffff}][{logItem.Module}][{logItem.ThreadName,-30}:{logItem.ThreadId:000}] [{logItem.LogLevel}] {msgRegex}\n";
        }

        protected override void Log(LogLevel level, string module, string message)
        {
            _logQueue.Post(new LogInformation(level, module, message, DateTime.Now, 
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        public override string ToString()
        {
            return "TextLoggerConfiguration:" +
                   $"\n\tDirectory: {_config.TextLoggerConfiguration.Directory}" +
                   $"\n\tFileName: {_config.TextLoggerConfiguration.FileName}" +
                   $"\n\tFileExtension: {_config.TextLoggerConfiguration.FileExtension}";
        }

    }
}
