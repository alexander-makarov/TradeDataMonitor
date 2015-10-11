using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
    public class TradeDataMonitor : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private readonly IFileSystemManager _fileSystemManager;
        private readonly ITradeDataLoader _tradeDataLoader;
        private DateTime _lastCheckUpdates = DateTime.MinValue;
        private readonly ITimer _timer;
        private readonly int _timerPeriodSeconds = 5;
        private readonly string _monitoringDirectory;
        /// <summary>
        /// 
        /// </summary>
        private bool _stopMonitoringRequestToken = false;
        private bool _isMonitoringStarted;
        private readonly object _syncObj = new object();

        public string MonitoringDirectory { get { return _monitoringDirectory; } }


        public bool IsMonitoringStarted
        {
            get { return _isMonitoringStarted; }
            private set { _isMonitoringStarted = value; OnPropertyChanged();}
        }

        public TradeDataMonitor(ITradeDataLoader tradeDataLoader, int timerPeriodSeconds, string monitoringDirectory)
            : this(new FileSystemManager(), tradeDataLoader, timerPeriodSeconds, new TimerAdaper(), monitoringDirectory)
        {
        }
        public TradeDataMonitor(IFileSystemManager fileSystemManager, ITradeDataLoader tradeDataLoader, int timerPeriodSeconds, ITimer timer, string monitoringDirectory)
        {
            _fileSystemManager = fileSystemManager;
            _tradeDataLoader = tradeDataLoader;
            _timerPeriodSeconds = timerPeriodSeconds;
            _timer = timer;
            _timer.Init(OnTimerTick, null, Timeout.Infinite, Timeout.Infinite);
            _monitoringDirectory = monitoringDirectory;
            IsMonitoringStarted = false;
        }

        public void StartMonitoring()
        {
            if (!IsMonitoringStarted)
            {
                IsMonitoringStarted = true;
                _timer.Change(_timerPeriodSeconds*1000, Timeout.Infinite);
            }
        }
        
        public void StopMonitoring()
        {
            bool acquired = false;
            try
            {
                acquired = Monitor.TryEnter(_syncObj);
                if (acquired)
                {
                    IsMonitoringStarted = false;
                }
                else
                {
                    _stopMonitoringRequestToken = true;
                }
            }
            finally
            {
                if (acquired)
                {
                    Monitor.Exit(_syncObj);
                }
            }
        }

        private void OnTimerTick(object state)
        {
            lock (_syncObj)
            {
                if (!IsMonitoringStarted)
                    return;

                CheckUpdates();

                if (!_stopMonitoringRequestToken)
                {
                    _timer.Change(_timerPeriodSeconds*1000, Timeout.Infinite);
                }
                else
                {
                    IsMonitoringStarted = false;
                    _stopMonitoringRequestToken = false;
                }
            }
        }




        /// <summary>
        /// Event to notify about trade data updates
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
            var files = _fileSystemManager.GetNewFilesFromDirectory(_lastCheckUpdates, _monitoringDirectory); // check directory for new files
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
