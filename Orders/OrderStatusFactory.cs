using TradingEngine.Shared.Orders.Status;
using TradingEngine.Shared.Orders.Types;

namespace TradingEngine.Shared.Orders
{
    public sealed class OrderStatusFactory
    {
        public static CancelOrderStatus GenerateCancelOrderStatus(CancelOrder cancelOrder)
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
