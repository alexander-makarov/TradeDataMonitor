using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TradeDataMonitorApp.MvvmHelpers;
using TradeDataMonitoring;

namespace TradeDataMonitorApp.ViewModels
{
    public class TradeDataMonitorViewModel : ViewModelBase
    {
        private readonly ITradeDataMonitor _tradeDataMonitor;
        public TradeDataMonitorViewModel(ITradeDataMonitor tradeDataMonitor)
        {
            _tradeDataMonitor = tradeDataMonitor;
            _tradeDataMonitor.TradeDataUpdate += TradeDataMonitorOnTradeDataUpdate;

            InitCommands();
        }

        private void TradeDataMonitorOnTradeDataUpdate(TradeDataPackage tradeDataPackage)
        {
            Application.Current.Dispatcher.Invoke(() => tradeDataPackage.TradeDataList.ForEach(data => _tradeDataList.Add(data)));
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

        public ICommand MonitoringStartStopCommand { get; private set; }


        protected void InitCommands()
        {
            MonitoringStartStopCommand = new RelayCommand(MonitoringStartStop);
        }

        public async void MonitoringStartStop(object o)
        {
            if (_tradeDataMonitor.IsMonitoringStarted) // 
            {
                //_tradeDataMonitor.PropertyChanged += WaitTillIsMonitoringStartedIsFalse; 
                MonitoringStartStopButtonContent = "Will stop monitoring. Await on active file reading operations...";
                MonitoringStartStopButtonEnabled = false;
                await _tradeDataMonitor.StopMonitoringAsync();
                MonitoringStartStopButtonContent = MonitoringStartText;
                MonitoringStartStopButtonBackground = MonitoringStartBackground;
                //_tradeDataMonitor.PropertyChanged -= WaitTillIsMonitoringStartedIsFalse;
                MonitoringStartStopButtonEnabled = true;
            }
            else
            {
                _tradeDataMonitor.StartMonitoring();
                MonitoringStartStopButtonContent = MonitoringStopText;
                MonitoringStartStopButtonBackground = MonitoringStopBackground;
            }
        }

        //private void WaitTillIsMonitoringStartedIsFalse(object o, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsMonitoringStarted" && _tradeDataMonitor.IsMonitoringStarted == false)
        //    {
        //        MonitoringStartStopButtonContent = MonitoringStartText;
        //        MonitoringStartStopButtonBackground = MonitoringStartBackground;
        //        _tradeDataMonitor.PropertyChanged -= WaitTillIsMonitoringStartedIsFalse;
        //        MonitoringStartStopButtonEnabled = true;
        //    }
        //}

        #endregion
    }
}
