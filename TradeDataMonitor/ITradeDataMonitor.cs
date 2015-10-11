using System;
using System.ComponentModel;

namespace TradeDataMonitoring
{
    public interface ITradeDataMonitor : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify about detected trade data updates
        /// </summary>
        event Action<TradeDataPackage> TradeDataUpdate;

        void StartMonitoring();

        void StopMonitoring();

        bool IsMonitoringStarted { get; }

        string MonitoringDirectory { get; }
    }
}