using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitoring
{
    /// <summary>
    /// TradeDataMonitor performs periodic check for trade data updates
    /// in a monitoring directory.
    /// 
    /// <remarks>Current implementation based on System.Threading.Timer,
    /// however for any further extensive development on files monitoring functionality,
    /// one might find useful to look at FileSystemWatcher class:
    /// https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher(v=vs.110).aspx </remarks>
    /// </summary>
    public class TradeDataMonitor : ITradeDataMonitor
    {
        private readonly object _syncObj = new object();
        private readonly IFileSystemManager _fileSystemManager;
        private readonly ITradeDataLoader _tradeDataLoader;
        private readonly ITimer _timer;
        /// <summary>
        /// Last time when monitoring directory has been checked for updates
        /// </summary>
        private DateTime _lastCheckUpdates = DateTime.MinValue;

        /// <summary>
        /// Directory which is gonna be tracked for any trade data updates
        /// </summary>
        public string MonitoringDirectory { get; private set; }

        /// <summary>
        /// Flag to determine if monitoring for updates has been started
        /// </summary>
        public bool IsMonitoringStarted { get; private set; }

        /// <summary>
        /// Monitoring time period in seconds
        /// </summary>
        public int TimerPeriodSeconds { get; private set; }

        public TradeDataMonitor(IFileSystemManager fileSystemManager, ITimer timer, ITradeDataLoader tradeDataLoader,  int timerPeriodSeconds, string monitoringDirectory)
        {
            _fileSystemManager = fileSystemManager;
            _tradeDataLoader = tradeDataLoader;
            _timer = timer;
            _timer.Init(OnTimerTick, null, Timeout.Infinite, Timeout.Infinite);
            TimerPeriodSeconds = timerPeriodSeconds;
            MonitoringDirectory = monitoringDirectory;
            IsMonitoringStarted = false;
        }

        /// <summary>
        /// Start monitoring trade data updates
        /// </summary>
        public void StartMonitoring()
        {
            lock (_syncObj)
            {
                if (!IsMonitoringStarted)
                {
                    IsMonitoringStarted = true;
                    _timer.Change(TimerPeriodSeconds*1000, Timeout.Infinite);
                }
            }
        }
        
        /// <summary>
        /// Trying to stop monitoring trade data updates
        /// let await while not stopped (all files has been processed in a background)
        /// </summary>
        /// <returns>task to await on</returns>
        public Task StopMonitoringAsync()
        {
            return Task.Run(() =>
            {
                lock (_syncObj) // wait untill OnTimerTick wil be finished
                {
                    IsMonitoringStarted = false; // stop monitoring
                }
            });
        }

        /// <summary>
        /// Periodic timer tick to check monitoring deirectory for updates
        /// </summary>
        /// <param name="state">not used</param>
        private void OnTimerTick(object state)
        {
            lock (_syncObj) // force StopMonitoringAsync method to wait while process updates 
            {
                if (!IsMonitoringStarted) // check if monitoring was stopped, before start
                    return;

                CheckUpdates(); // check monitoring directory for updates 

                // only when done schedule the next Tick (to prevent repeated calls)
                _timer.Change(TimerPeriodSeconds * 1000, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Check for data updates (new files) in monitoring directory
        /// <remarks>
        /// For now, we track new files by creation time, and don't delete any of them,
        /// another option might be to delete files once they have been processed</remarks>
        /// </summary>
        private void CheckUpdates()
        {
            var now = DateTime.UtcNow; // save the 'now' time
            var files = _fileSystemManager.GetNewFilesFromDirectory(_lastCheckUpdates, MonitoringDirectory); // check directory for new files
            _lastCheckUpdates = now; // update last checked time

            // go for parallel file processing:
            Parallel.ForEach(files,
                (file) =>
                {
                    // for any new file check if we could load data from it
                    if (_tradeDataLoader.CouldLoad(file))
                    {
                        var data = _tradeDataLoader.LoadTradeData(file); // load data
                        OnTradeDataUpdate(data); // notify about update
                    }
                });
        }

        /// <summary>
        /// Event to notify about detected trade data updates
        /// </summary>
        public event EventHandler<TradeDataPackage> TradeDataUpdate;
        protected virtual void OnTradeDataUpdate(TradeDataPackage obj)
        {
            EventHandler<TradeDataPackage> handler = TradeDataUpdate;
            if (handler != null) handler(this, obj);
        }
    }
}
