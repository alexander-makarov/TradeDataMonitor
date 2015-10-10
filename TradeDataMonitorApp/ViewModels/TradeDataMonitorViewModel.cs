using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TradeDataMonitorApp.MvvmHelpers;
using TradeDataMonitoring;

namespace TradeDataMonitorApp.ViewModels
{
    public class TradeDataMonitorViewModel : ViewModelBase
    {
        private readonly TradeDataMonitor _monitor;
        public TradeDataMonitorViewModel(TradeDataMonitor monitor)
        {
            _monitor = monitor;
            _monitor.TradeDataUpdate += MonitorOnTradeDataUpdate;

            InitCommands();
        }

        private void MonitorOnTradeDataUpdate(TradeDataPackage tradeDataPackage)
        {
            Application.Current.Dispatcher.Invoke(() => tradeDataPackage.Package.ForEach(data => _tradeDataList.Add(data)));
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
            get { return _monitor.MonitoringDirectory; }
        }

        private const string MonitoringStartText = "Start monitoring directory";
        private const string MonitoringStopText = "Stop monitoring directory";
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

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private string _busyContent = "";
        public string BusyContent
        {
            get { return _busyContent; }
            set { _busyContent = value; OnPropertyChanged(); }
        }

        #region Commands

        public ICommand MonitoringStartStopCommand { get; private set; }


        protected void InitCommands()
        {
            MonitoringStartStopCommand = new RelayCommand(MonitoringStartStop);
        }

        public void MonitoringStartStop(object o)
        {
            if (_monitor.IsMonitoringStarted) // если уже соединены
            {
                _monitor.PropertyChanged += (obj, e) =>
                {
                    if (e.PropertyName == "IsMonitoringStarted" && _monitor.IsMonitoringStarted == false)
                    {
                        MonitoringStartStopButtonContent = MonitoringStartText;
                        MonitoringStartStopButtonBackground = MonitoringStartBackground;
                        IsBusy = false;
                    }
                };
                BusyContent = "Await on active file reading operations...";
                IsBusy = true;
                _monitor.StopMonitor();
            }
            else
            {
                _monitor.StartMonitor();
                MonitoringStartStopButtonContent = MonitoringStopText;
                MonitoringStartStopButtonBackground = MonitoringStopBackground;
            }
        }
        #endregion
    }
}
