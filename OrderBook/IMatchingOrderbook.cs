namespace TradingEngine.Shared.OrderBook
{
    public interface IMatchingOrderbook : IRetrievalOrderbook
    {
        MatchResult Match();
    }
}
