namespace TradingEngine.Shared.OrderBook
{
    public interface IReadOnlyOrderbook
    {
        bool ContainsOrder(long orderId);
        OrderbookSpread GetSpread();
        int Count { get; }
    }
}
