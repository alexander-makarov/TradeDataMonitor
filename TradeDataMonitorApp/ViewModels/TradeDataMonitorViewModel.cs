using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using TradeDataMonitorApp.MvvmHelpers;
using TradeDataMonitoring;

namespace TradeDataMonitorApp.ViewModels
{
    public class TradeDataMonitorViewModel : ViewModelBase
    {
        private readonly ITradeDataMonitor _tradeDataMonitor;
        private readonly IDispatcher _dispatcher;

        public TradeDataMonitorViewModel(ITradeDataMonitor tradeDataMonitor, IDispatcher dispatcher)
        {
            _tradeDataMonitor = tradeDataMonitor;
            _dispatcher = dispatcher;
            _tradeDataMonitor.TradeDataUpdate += TradeDataMonitorOnTradeDataUpdate;

            InitCommands();
        }

        private void TradeDataMonitorOnTradeDataUpdate(object sender, TradeDataPackage tradeDataPackage)
        {
            _dispatcher.Invoke(() => tradeDataPackage.TradeDataList.ForEach(data => _tradeDataList.Add(data)));
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly ObservableCollection<TradeData> _tradeDataList = new ObservableCollection<TradeData>();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TradeData> TradeDataList
        {
            get { return _tradeDataList; }
        }

        public string MonitoringDirectory
        {
            get { return _tradeDataMonitor.MonitoringDirectory; }
        }

        private const string MonitoringStartText = "Start monitoring the directory for updates";
        private const string MonitoringStopText = "Stop monitoring the directory for updates";
        private string _monitoringStartStopButtonContent = MonitoringStartText;
        public string MonitoringStartStopButtonContent
        {
            get { return _monitoringStartStopButtonContent; }
            set
            {
                if (value == _monitoringStartStopButtonContent) return;
                _monitoringStartStopButtonContent = value;
                OnPropertyChanged();
            }
        }


        private readonly static SolidColorBrush MonitoringStartBackground = new SolidColorBrush(Colors.PaleGreen);
        private readonly static SolidColorBrush MonitoringStopBackground = new SolidColorBrush(Colors.PaleVioletRed);
        private SolidColorBrush _monitoringStartStopButtonBackground = MonitoringStartBackground;

        public SolidColorBrush MonitoringStartStopButtonBackground
        {
            get { return _monitoringStartStopButtonBackground; }
            set
            {
                if (Equals(value, _monitoringStartStopButtonBackground)) return;
                _monitoringStartStopButtonBackground = value;
                OnPropertyChanged();
            }
        }


        private bool _monitoringStartStopButtonEnabled = true;
        public bool MonitoringStartStopButtonEnabled
        {
            get { return _monitoringStartStopButtonEnabled; }
            set { _monitoringStartStopButtonEnabled = value; OnPropertyChanged(); }
        }

        #region Commands

        /// <summary>
        /// MonitoringStartStopCommand exposed for binding 
        /// </summary>
        public ICommand MonitoringStartStopCommand { get; private set; }

        /// <summary>
        /// Create all the commands for view-model
        /// </summary>
        protected void InitCommands()
        {
            MonitoringStartStopCommand = new RelayCommand(MonitoringStartStop);
        }

        /// <summary>
        /// MonitoringStartStop command
        /// Either start or stop monitoring in depence of current model state (ITradeDataMonitor.IsMonitoringStarted property)
        /// <remarks>
        /// Monitoring stop executes asynchronously, awaiting any background file readings currently happenned in ITradeDataMonitor.
        /// Prevent situation when UI is in monitring-stop state, 
        /// but it happens to refresh because of deffered/long-running updates being processed by ITradeMonitor  
        /// </remarks>
        /// </summary>
        /// <param name="o">not used</param>
        public async void MonitoringStartStop(object o)
        {
            if (_tradeDataMonitor.IsMonitoringStarted) // 
            {
                MonitoringStartStopButtonContent = "Will stop monitoring. Await on active file reading operations...";
                MonitoringStartStopButtonEnabled = false;

                await _tradeDataMonitor.StopMonitoringAsync();
                MonitoringStartStopButtonContent = MonitoringStartText;
                MonitoringStartStopButtonBackground = MonitoringStartBackground;
                MonitoringStartStopButtonEnabled = true;
            }
            else
            {
                _tradeDataMonitor.StartMonitoring();
                MonitoringStartStopButtonContent = MonitoringStopText;
                MonitoringStartStopButtonBackground = MonitoringStopBackground;
            }
        }

        #endregion
    }
}
