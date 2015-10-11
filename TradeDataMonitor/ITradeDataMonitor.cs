using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TradeDataMonitoring
{
    public interface ITradeDataMonitor
    {
        /// <summary>
        /// Event to notify about detected trade data updates
        /// </summary>
        event Action<TradeDataPackage> TradeDataUpdate;

        void StartMonitoring();

        Task StopMonitoringAsync();

        bool IsMonitoringStarted { get; }

        string MonitoringDirectory { get; }
    }
}