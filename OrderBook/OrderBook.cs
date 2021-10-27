using System.Collections.Generic;
using System.Linq;
using TradingEngine.Shared.Instrument;
using TradingEngine.Shared.Orders;
using TradingEngine.Shared.Orders.Types;

namespace TradingEngine.Shared.OrderBook
{
    public class OrderBook : IRetrievalOrderbook
    {
        private readonly Security _instrument;
        private readonly Dictionary<long, OrderbookEntry> _orders
            = new Dictionary<long, OrderbookEntry>();
        private readonly SortedSet<Limit> _askLimits 
            = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits 
            = new SortedSet<Limit>(BidLimitComparer.Comparer);

        public int Count => _orders.Count;

        public OrderBook(Security instrument)
        {
            _instrument = instrument;
        }

        public bool ContainsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);
        }

        public OrderbookSpread GetSpread()
        {
            long? bestAsk = null;
            long? bestBid = null;

            if (_askLimits.Any() && !_askLimits.Min.IsEmpty)
            {
                bestAsk = _askLimits.Min.Price;
            }

            if (_bidLimits.Any() && !_bidLimits.Max.IsEmpty)
            {
                bestBid = _bidLimits.Max.Price;
            }

            return new OrderbookSpread(bestBid, bestAsk);
        }

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            AddOrder(order, baseLimit, 
                order.IsBuySide ? _bidLimits : _askLimits, 
                _orders);
        }

        private static void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limitLevels,
            IDictionary<long, OrderbookEntry> internalBook)
        {
            var orderbookEntry = new OrderbookEntry(order, baseLimit);
            if (limitLevels.TryGetValue(baseLimit, out var limit))
            {
                if (limit.Head == null)
                {
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                }
                else
                {
                    var tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    orderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;
                }

            }
            else
            {
                limitLevels.Add(baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
            }

            internalBook.Add(order.OrderId, orderbookEntry);
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            if (!_orders.TryGetValue(modifyOrder.OrderId, out var obe)) return;
            RemoveOrder(modifyOrder.ToCancelOrder());
            AddOrder(modifyOrder.ToNewOrder(), obe.ParentLimit, 
                modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            if (_orders.TryGetValue(cancelOrder.OrderId, out var obe))
            {
                RemoveOrder(cancelOrder.OrderId, obe, _orders);
            }
        }

        private static void RemoveOrder(long orderId, OrderbookEntry obe,
            IDictionary<long, OrderbookEntry> internalBook)
        {
            // Deal with the location of OBE within the LinkedList
            if (obe.Previous != null && obe.Next != null)
            {
                obe.Next.Previous = obe.Previous;
                obe.Previous.Next = obe.Next;
            }
            else if (obe.Previous != null)
            {
                obe.Previous.Next = null;
            }
            else if (obe.Next != null)
            {
                obe.Next.Previous = null;
            }

            // Deal with OBE on Limit-level
            if (obe.ParentLimit.Head == obe && obe.ParentLimit.Tail == obe)
            {
                obe.ParentLimit.Head = null;
                obe.ParentLimit.Tail = null;
            }
            else if (obe.ParentLimit.Head == obe)
            {
                obe.ParentLimit.Head = obe.Next;
            }
            else if (obe.ParentLimit.Tail == obe)
            {
                obe.ParentLimit.Tail = obe.Previous;
            }

            internalBook.Remove(orderId);

        }

        public List<OrderbookEntry> GetAskOrders()
        {
            var orderbookEntries = new List<OrderbookEntry>();
            foreach (var askLimit in _askLimits)
            {
                if (askLimit.IsEmpty) continue;
                var askLimitPointer = askLimit.Head;
                while (askLimitPointer != null)
                {
                    orderbookEntries.Add(askLimitPointer);
                    askLimitPointer = askLimitPointer.Next;
                }
            }

            return orderbookEntries;
        }

        public List<OrderbookEntry> GetBidOrder()
        {
            var orderbookEntries = new List<OrderbookEntry>();
            foreach (var bidLimit in _bidLimits)
            {
                if (bidLimit.IsEmpty) continue;
                var bidLimitPointer = bidLimit.Head;
                while (bidLimitPointer != null)
                {
                    orderbookEntries.Add(bidLimitPointer);
                    bidLimitPointer = bidLimitPointer.Next;
                }
            }

            return orderbookEntries;
        }
    }
}
