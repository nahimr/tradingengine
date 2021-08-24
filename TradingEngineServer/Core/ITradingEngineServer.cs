using System.Threading;
using System.Threading.Tasks;

namespace TradingEngine.Server.Core
{
    interface ITradingEngineServer
    {
        Task Run(CancellationToken token);
    }
}
