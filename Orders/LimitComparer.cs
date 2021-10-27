using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Shared.Orders
{
    class BidLimitComparer : IComparer<Limit>
    {
        public static IComparer<Limit> Comparer { get; } = new BidLimitComparer();
        public int Compare(Limit x, Limit y)
        {
            if (x.Price == y.Price) return 0;
            if (x.Price > y.Price) return -1;
            return 1;
        }
    }    
    
    class AskLimitComparer : IComparer<Limit>
    {
        public static IComparer<Limit> Comparer { get; } = new AskLimitComparer();
        public int Compare(Limit x, Limit y)
        {
            if (x.Price == y.Price) return 0;
            if (x.Price < y.Price) return -1;
            return 1;
        }
    }
}
