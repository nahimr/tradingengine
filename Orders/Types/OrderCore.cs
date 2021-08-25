namespace TradingEngine.Shared.Orders.Types
{
    public class OrderCore : IOrderCore
    {
        public OrderCore(long orderId, string username, int securityId)
        {
            OrderId = orderId;
            Username = username;
            SecurityId = securityId;
        }

        public long OrderId { get; }
        public string Username { get; }
        public int SecurityId { get; }
    }
}
