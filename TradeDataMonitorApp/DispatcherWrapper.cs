using System;
using System.Windows;
using System.Windows.Threading;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Wrapper around Application.Current.Dispatcher
    /// </summary>
    public class DispatcherWrapper : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherWrapper()
        {
            _dispatcher = Application.Current.Dispatcher;
        }
        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}