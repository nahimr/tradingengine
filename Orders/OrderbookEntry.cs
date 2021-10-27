using System;
using TradingEngine.Shared.Orders.Types;

namespace TradingEngine.Shared.Orders
{
    public class OrderbookEntry
    {
        public Order CurrentOrder { get; private set; }
        public Limit ParentLimit { get; private set; }
        public DateTime CreationTime { get; private set; }
        public OrderbookEntry Next { get; set; }
        public OrderbookEntry Previous { get; set; }
        public OrderbookEntry(Order currentOrder, Limit parentLimit)
        {
            CreationTime = DateTime.UtcNow;
            CurrentOrder = currentOrder;
            ParentLimit = parentLimit;
        }

    }
}
