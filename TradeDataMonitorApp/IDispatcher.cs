using System;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Interface to mock Application.Current.Dispatcher
    /// </summary>
    public interface IDispatcher
    {
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }
}