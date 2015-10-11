using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitoring
{
    /// <summary>
    /// 
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
        private DateTime _lastCheckUpdates = DateTime.MinValue;
        public string MonitoringDirectory { get; private set; }
        public bool IsMonitoringStarted { get; private set; }
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
        
        public Task StopMonitoringAsync()
        {
            return Task.Run(() =>
            {
                lock (_syncObj)
                {
                    IsMonitoringStarted = false;
                }
            });
        }

        private void OnTimerTick(object state)
        {
            lock (_syncObj)
            {
                if (!IsMonitoringStarted)
                    return;

                CheckUpdates();

                _timer.Change(TimerPeriodSeconds * 1000, Timeout.Infinite);
            }
        }




        /// <summary>
        /// Event to notify about detected trade data updates
        /// </summary>
        public event Action<TradeDataPackage> TradeDataUpdate;
        protected virtual void OnTradeDataUpdate(TradeDataPackage obj)
        {
            Action<TradeDataPackage> handler = TradeDataUpdate;
            if (handler != null) handler(obj);
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
    }
}
