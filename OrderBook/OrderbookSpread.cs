﻿namespace TradingEngine.Shared.OrderBook
{
    public class OrderbookSpread
    {
        public long? Bid { get; private set; }
        public long? Ask { get; private set; }

        public long? Spread
        {
            get
            {
                if (Bid.HasValue && Ask.HasValue) return (Ask.Value - Bid.Value);
                return null;
            }
        }
        public OrderbookSpread(long? bid, long? ask)
        {
            Bid = bid;
            Ask = ask;
        }
    }
}
