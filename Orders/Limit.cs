using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Shared.Orders
{
    public class Limit
    {
        public long Price { get; set; }
        public OrderbookEntry Head { get; set; }
        public OrderbookEntry Tail { get; set; }

        public bool IsEmpty => Head == null && Tail == null;

        public Side Side
        {
            get
            {
                if (IsEmpty) return Side.Unknown;
                return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask;
            }
        }


        public uint GetLevelOrderCount()
        {
            uint orderCount = 0U;
            OrderbookEntry headPointer = Head;
            if (headPointer == null) return 0U;

            do
            {
                if (headPointer.CurrentOrder.CurrentQuantity == 0U) continue;
                orderCount++;
            } while ((headPointer = headPointer.Next) != null);

            return orderCount;
        }

        public uint GetLevelOrderQuantity()
        {
            uint orderQuantity = 0U;
            OrderbookEntry headPointer = Head;
            if (headPointer == null) return 0U;

            do
            {
                orderQuantity += headPointer.CurrentOrder.CurrentQuantity;
            } while ((headPointer = headPointer.Next) != null);

            return orderQuantity;
        }

        public List<OrderRecord> GetLevelOrderRecords()
        {
            List<OrderRecord> orderRecords = new List<OrderRecord>();
            OrderbookEntry headPointer = Head;
            if (headPointer == null) return orderRecords;
            uint theoreticalQueuePosition = 0U;

            do
            {
                var currentOrder = headPointer.CurrentOrder;
                if (currentOrder.CurrentQuantity == 0U) continue;
                orderRecords.Add(new OrderRecord(
                    currentOrder.OrderId, currentOrder.CurrentQuantity,
                    Price, currentOrder.IsBuySide,
                    currentOrder.Username, currentOrder.SecurityId,
                    theoreticalQueuePosition));
                theoreticalQueuePosition++;
            } while ((headPointer = headPointer.Next) != null);

            return orderRecords;
        }
    }
}
