using System;
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
    public class TradeDataMonitor
    {
        private readonly IFileSystemManager _fileSystemManager;
        private readonly ITradeDataLoader _tradeDataLoader;
        private DateTime _lastCheckUpdates = DateTime.MinValue;
        private readonly ITimer _timer;
        private readonly int _timerPeriodSeconds = 5;
        private readonly string _monitoringDirectory;
        public bool IsMonitoringStarted { get; private set; }

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
            //var t = new Task(() =>
            //{
            //    const string correctCsvString = "2000-5-20,30.16,30.39,30.02,30.17,1478200";
            //    var corectValues = correctCsvString.Split(',');
            //    var data1 = TradeData.Parse(corectValues);
            //    var p = new TradeDataPackage();
            //    for (int i = 0; i < 3; i++)
            //    {
            //        p.Package.Add(data1);
            //        data1 = new TradeData(data1.Date.AddDays(1), data1.Open, data1.High, data1.Low, data1.Close,
            //            data1.Volume);

            //    }
            //    OnTradeDataUpdate(p);
            //});
            //var t2 = new Task(() =>
            //{
            //    const string correctCsvString = "1999-5-20,30.16,30.39,30.02,30.17,1478200";
            //    var corectValues = correctCsvString.Split(',');
            //    var data1 = TradeData.Parse(corectValues);
            //    var p = new TradeDataPackage();
            //    for (int i = 0; i < 3; i++)
            //    {
            //        p.Package.Add(data1);
            //        data1 = new TradeData(data1.Date.AddDays(1), data1.Open, data1.High, data1.Low, data1.Close,
            //            data1.Volume);

            //    }
            //    OnTradeDataUpdate(p);
            //});
            //t.Start();
            //t2.Start();
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
            ////test
            //const string correctCsvString = "2013-5-20,30.16,30.39,30.02,30.17,1478200";
            //var corectValues = correctCsvString.Split(',');
            //var data1 = TradeData.Parse(corectValues);
            //var p = new TradeDataPackage();
            //for (int i = 0; i < 3; i++)
            //{
            //    p.Package.Add(data1);
            //    data1 = new TradeData(data1.Date.AddDays(1), data1.Open, data1.High, data1.Low, data1.Close,
            //        data1.Volume);

            //}
            //OnTradeDataUpdate(p);
            //return;
            ////test

            var now = DateTime.UtcNow;
            var files = _fileSystemManager.GetNewFilesFromDirectory(_lastCheckUpdates, _monitoringDirectory);
            _lastCheckUpdates = now;
            
            Parallel.ForEach(files, 
                (file) =>
                {
                    if (_tradeDataLoader.CouldLoad(file))
                    {
                        var data = _tradeDataLoader.LoadTradeData(file);
                        OnTradeDataUpdate(data);
                    }
                });
        }
    }
}
