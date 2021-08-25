using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Shared.Orders
{
    public sealed class OrderStatusFactory
    {
        public static CancelOrderStatus GenerateCancelOrderStatus(CancelOrder co)
        {
            return new CancelOrderStatus();
        }

        public static NewOrderStatus GenerateNewOrderStatus(Order order)
        {
            return new NewOrderStatus();
        }

        public static ModifyOrderStatus GenerateModifyOrderStatus(ModifyOrder order)
        {
            return new ModifyOrderStatus();
        }
    }
}
