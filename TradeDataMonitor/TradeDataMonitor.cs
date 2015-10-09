using System;
using System.Threading;

namespace TradeDataMonitoring
{
    public class TradeDataMonitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITradeDataLoader _tradeDataLoader;
        private DateTime _lastCheckUpdates = DateTime.MinValue;
        private readonly ITimer _timer;
        private readonly int _timerPeriodSeconds = 5;
        public bool IsMonitoringStarted { get; private set; }

        public TradeDataMonitor(IFileSystem fileSystem, ITradeDataLoader tradeDataLoader, int timerPeriodSeconds)
            : this(fileSystem, tradeDataLoader, timerPeriodSeconds, new TimerAdaper())
        {
        }
        public TradeDataMonitor(IFileSystem fileSystem, ITradeDataLoader tradeDataLoader, int timerPeriodSeconds, ITimer timer)
        {
            _fileSystem = fileSystem;
            _tradeDataLoader = tradeDataLoader;
            _timerPeriodSeconds = timerPeriodSeconds;
            _timer = timer;
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

        public void CheckUpdates()
        {
            var now = DateTime.UtcNow;
            var files = _fileSystem.GetNewFiles(_lastCheckUpdates);
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
