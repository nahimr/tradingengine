using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngine.Shared.Logging.Configuration;
using TradingEngine.Shared.Logging.Utilities;

namespace TradingEngine.Shared.Logging.Loggers
{
    public sealed class DatabaseLogger : AbstractLogger
    {
        private readonly LoggingConfiguration _config;
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly SqlConnection _connection;
        private readonly object _lock = new object();
        private bool _isDisposed = false;

        public DatabaseLogger(IOptions<LoggingConfiguration> config) : base()
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));

            if (_config.LoggerType != LoggerType.Database)
            {
                throw new InvalidOperationException(
                    $"{nameof(DatabaseLogger)} doesn't match LoggerType of {_config.LoggerType}");
            }

            _connection = new SqlConnection(_config.DatabaseLoggerConfiguration.ConnectionString);

            _ = Task.Run(() => LogAsync(_logQueue, _tokenSource.Token));
        }

        ~DatabaseLogger()
        {
            Dispose(false);
        }

        protected override void Log(LogLevel level, string module, string message)
        {
            // Here push to Queue Log

            _logQueue.Post(new LogInformation(level, module, message, DateTime.Now,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        private async Task LogAsync(BufferBlock<LogInformation> logQueue, CancellationToken token)
        {
            // Open Connection Here !
            await _connection.OpenAsync(token);
            var sqCommand = _connection.CreateCommand();
            sqCommand.CommandText = $"CREATE TABLE IF NOT EXISTS {_config.DatabaseLoggerConfiguration.TableName}(" +
                                    "id INTEGER PRIMARY KEY," +
                                    "message TEXT)";
            await sqCommand.ExecuteNonQueryAsync(token);

            try
            {
                while (true)
                {
                    var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    // Push to DB
                    var logItemQuery = _connection.CreateCommand();
                    logItemQuery.CommandText = $"INSERT INTO {_config.DatabaseLoggerConfiguration.TableName}(message) VALUES({logItem.Message})";
                    await logItemQuery.ExecuteNonQueryAsync(token);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (_isDisposed) return;

            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                // Kill Connection if there is one here
                _connection.Close();
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.


            // Note disposing has been done.
            _isDisposed = true;

        }
    }
}
