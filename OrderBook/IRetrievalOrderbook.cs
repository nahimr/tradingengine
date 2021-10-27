using System.Collections.Generic;
using TradingEngine.Shared.Orders;

namespace TradingEngine.Shared.OrderBook
{
    public interface IRetrievalOrderbook : IOrderEntryOrderbook
    {
        List<OrderbookEntry> GetAskOrders();
        List<OrderbookEntry> GetBidOrder();
    }
}
