namespace TradingEngine.Shared.Orders.Types
{
    public class CancelOrder : IOrderCore
    {
        private readonly IOrderCore _orderCore;
        public CancelOrder(IOrderCore orderCore)
        {
            _orderCore = orderCore;
        }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;
    }
}
