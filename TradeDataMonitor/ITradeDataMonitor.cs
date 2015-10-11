using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TradeDataMonitoring
{
    /// <summary>
    /// ITradeDataMonitor interface for monitoring trade data updates in a directory.
    /// </summary>
    public interface ITradeDataMonitor
    {
        /// <summary>
        /// Event to notify about detected trade data updates 
        /// <remarks>Use of EventHandler just for unit-testing (to allow FakeItEasy raise events from ITradeDataMonitor)</remarks>
        /// </summary>
        event EventHandler<TradeDataPackage> TradeDataUpdate;

        /// <summary>
        /// Start monitoring trade data updates
        /// </summary>
        void StartMonitoring();

        /// <summary>
        /// Trying to stop monitoring trade data updates
        /// let await while not stopped (all files has been processed in a background)
        /// </summary>
        /// <returns>task to await on</returns>
        Task StopMonitoringAsync();

        /// <summary>
        /// Flag to determine if monitoring for updates has been started
        /// </summary>
        bool IsMonitoringStarted { get; }

        /// <summary>
        /// Directory which is gonna be tracked for any trade data updates
        /// </summary>
        string MonitoringDirectory { get; }
    }
}