using TradingEngine.Shared.Orders.Enumerations;
using TradingEngine.Shared.Orders.Types;

namespace TradingEngine.Shared.Orders
{
    public sealed class RejectFactory
    {
        public static Reject GenerateOrderCoreReject(IOrderCore rejectedOrder, RejectionReason reason)
        {
            return new Reject(rejectedOrder, reason);
        }
    }
}
