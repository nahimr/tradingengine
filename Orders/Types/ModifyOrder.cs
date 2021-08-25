namespace TradingEngine.Shared.Orders.Types
{
    public class ModifyOrder : IOrderCore
    {
        private readonly IOrderCore _orderCore;
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;
        public long ModifyPrice { get; private set; }
        public uint ModifyQuantity { get; private set; }
        public bool IsBuySide { get; private set; }

        public ModifyOrder(IOrderCore orderCore, long modifyPrice, uint modifyQuantity, bool isBuySide)
        {
            _orderCore = orderCore;
            ModifyPrice = modifyPrice;
            ModifyQuantity = modifyQuantity;
            IsBuySide = isBuySide;
        }

        public CancelOrder ToCancelOrder()
        {
            return new CancelOrder(this);
        }

        public Order ToNewOrder()
        {
            return new Order(this);
        }
    }
}
