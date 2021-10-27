using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Shared.Orders
{
    /// <summary>
    /// Read-only representation of an order.
    /// </summary>
    public record OrderRecord(long OrderId, uint Quantity, long Price, 
        bool IsBuySide, string Username, int SecurityId, uint TheoreticalQueuePosition);
}

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This is here temporarily to enable record types in C#9 due to a Visual Studio 2019 bug
    /// </summary>
    internal static class IsExternalInit {};
}