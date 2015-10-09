using System;
using System.Threading;

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
    public class TradeDataMonitor
    {
        private readonly IFileSystemManager _fileSystemManager;
        private readonly ITradeDataLoader _tradeDataLoader;
        private DateTime _lastCheckUpdates = DateTime.MinValue;
        private readonly ITimer _timer;
        private readonly int _timerPeriodSeconds = 5;
        private readonly string _monitoringDirectory;
        public bool IsMonitoringStarted { get; private set; }

        public TradeDataMonitor(IFileSystemManager fileSystemManager, ITradeDataLoader tradeDataLoader, int timerPeriodSeconds, string monitoringDirectory)
            : this(fileSystemManager, tradeDataLoader, timerPeriodSeconds, new TimerAdaper(), monitoringDirectory)
        {
        }
        public TradeDataMonitor(IFileSystemManager fileSystemManager, ITradeDataLoader tradeDataLoader, int timerPeriodSeconds, ITimer timer, string monitoringDirectory)
        {
            _fileSystemManager = fileSystemManager;
            _tradeDataLoader = tradeDataLoader;
            _timerPeriodSeconds = timerPeriodSeconds;
            _timer = timer;
            _monitoringDirectory = monitoringDirectory;
            IsMonitoringStarted = false;
        }

        public void StartMonitor()
        {
            if (!IsMonitoringStarted)
            {
                IsMonitoringStarted = true;
                _timer.Init(OnTimerTick, null, Timeout.Infinite, Timeout.Infinite);
                _timer.Change(_timerPeriodSeconds*1000, Timeout.Infinite);
            }
        }

        private void OnTimerTick(object state)
        {
            CheckUpdates();

            _timer.Change(_timerPeriodSeconds*1000, Timeout.Infinite);
        }

        public event Action<TradeDataPackage> TradeDataUpdate;

        protected virtual void OnTradeDataUpdate(TradeDataPackage obj)
        {
            Action<TradeDataPackage> handler = TradeDataUpdate;
            if (handler != null) handler(obj);
        }

        private void CheckUpdates()
        {
            var now = DateTime.UtcNow;
            var files = _fileSystemManager.GetNewFilesFromDirectory(_lastCheckUpdates, _monitoringDirectory);
            _lastCheckUpdates = now;

            // TODO: parallel loading of trade data from files
            foreach (var file in files)
            {
                if (_tradeDataLoader.CouldLoad(file))
                {
                    var data = _tradeDataLoader.LoadTradeData(file);
                    OnTradeDataUpdate(data);
                }
            }
        }
    }
}
