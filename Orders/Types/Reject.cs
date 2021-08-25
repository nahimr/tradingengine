using TradingEngine.Shared.Orders.Enumerations;

namespace TradingEngine.Shared.Orders.Types
{
    public class Reject : IOrderCore
    {
        private readonly IOrderCore _orderCore;
        public Reject(IOrderCore rejectedOrder, RejectionReason reason)
        {
            _orderCore = rejectedOrder;
            RejectionReason = reason;
        }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;
        public RejectionReason RejectionReason { get; private set; }
    }
}
