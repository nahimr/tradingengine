using System;

namespace TradingEngine.Shared.Orders.Types
{
    public class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide)
        {
            Price = price;
            IsBuySide = isBuySide;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;

            _orderCore = orderCore;
        }

        public Order(ModifyOrder modifyOrder) : 
            this(modifyOrder, modifyOrder.ModifyPrice, modifyOrder.ModifyQuantity, modifyOrder.IsBuySide)
        {
        }

        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;
        public long Price { get; }
        public uint InitialQuantity { get; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; }

        private readonly IOrderCore _orderCore;

        public void IncreaseQuantity(uint quantityDelta) => CurrentQuantity += quantityDelta;

        public void DecreaseQuantity(uint quantityDelta)
        {
            if (quantityDelta > CurrentQuantity)
            {
                throw new InvalidOperationException($"Quantity Delta > Current Quantity for OrderId={OrderId}");
            }

            CurrentQuantity -= quantityDelta;
        }

    }
}
