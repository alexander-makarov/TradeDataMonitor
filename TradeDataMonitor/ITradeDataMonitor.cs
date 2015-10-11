using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TradeDataMonitoring
{
    public interface ITradeDataMonitor
    {
        /// <summary>
        /// Event to notify about detected trade data updates 
        /// <remarks>Use of EventHandler just for unit-testing (to allow FakeItEasy raise events from ITradeDataMonitor)</remarks>
        /// </summary>
        event EventHandler<TradeDataPackage> TradeDataUpdate;

        void StartMonitoring();

        Task StopMonitoringAsync();

        bool IsMonitoringStarted { get; }

        string MonitoringDirectory { get; }
    }
}