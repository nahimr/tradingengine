namespace TradingEngine.Shared.Orders.Enumerations
{
    public enum RejectionReason
    {
        Unknown,
        OrderNotFound,
        InstrumentNotFound,
        AttemptingToModifyWrongSide,
    }
}
