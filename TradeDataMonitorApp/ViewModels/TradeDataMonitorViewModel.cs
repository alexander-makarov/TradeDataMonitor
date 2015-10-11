using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using TradeDataMonitorApp.MvvmHelpers;
using TradeDataMonitoring;

namespace TradeDataMonitorApp.ViewModels
{
    /// <summary>
    /// Simple ViewModel on ITradeDataMonitor as Model
    /// </summary>
    public class TradeDataMonitorViewModel : ViewModelBase
    {
        public TradeDataMonitorViewModel(ITradeDataMonitor tradeDataMonitor, IDispatcher dispatcher)
        {
            if (tradeDataMonitor == null)
            {
                throw new ArgumentNullException("tradeDataMonitor");
            }
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            _tradeDataMonitor = tradeDataMonitor;
            _dispatcher = dispatcher;
            _tradeDataMonitor.TradeDataUpdate += TradeDataMonitorOnTradeDataUpdate; // subscribe for trade data updates

            InitCommands();
        }

        /// <summary>
        /// On any update of trade data
        /// </summary>
        /// <param name="sender">ITradeDataMonitor</param>
        /// <param name="tradeDataPackage">update trade data package</param>
        private void TradeDataMonitorOnTradeDataUpdate(object sender, TradeDataPackage tradeDataPackage)
        {
            if(tradeDataPackage == null)
                return;

            // adding to TradeDataList to let UI display new data
            _dispatcher.Invoke(() => tradeDataPackage.TradeDataList.ForEach(data => _tradeDataList.Add(data)));
        }

        #region consts
        private readonly static SolidColorBrush MonitoringStartBackground = new SolidColorBrush(Colors.PaleGreen);
        private readonly static SolidColorBrush MonitoringStopBackground = new SolidColorBrush(Colors.PaleVioletRed);
        private const string MonitoringStopAwaitText = "Will stop monitoring. Await on active file reading operations...";
        private const string MonitoringStartText = "Start monitoring the directory for updates";
        private const string MonitoringStopText = "Stop monitoring the directory for updates";
        #endregion

        #region fields
        private readonly ITradeDataMonitor _tradeDataMonitor;
        private readonly IDispatcher _dispatcher;
        private string _monitoringStartStopButtonContent = MonitoringStartText;
        private SolidColorBrush _monitoringStartStopButtonBackground = MonitoringStartBackground;
        private readonly ObservableCollection<TradeData> _tradeDataList = new ObservableCollection<TradeData>();
        private bool _monitoringStartStopButtonEnabled = true;
        #endregion

        #region properties
        /// <summary>
        /// Trade data
        /// </summary>
        public ObservableCollection<TradeData> TradeDataList
        {
            get { return _tradeDataList; }
        }

        /// <summary>
        /// Directory which tracked by ITradeDataMonitor for any trade data updates
        /// </summary>
        public string MonitoringDirectory
        {
            get { return _tradeDataMonitor.MonitoringDirectory; }
        }
        
        /// <summary>
        /// Binding Content property for MonitoringStartStopButton in a View
        /// </summary>
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

        /// <summary>
        /// Binding Background property for MonitoringStartStopButton in a View
        /// </summary>
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

        /// <summary>
        /// Binding Enabled property for MonitoringStartStopButton in a View
        /// </summary>
        public bool MonitoringStartStopButtonEnabled
        {
            get { return _monitoringStartStopButtonEnabled; }
            set { _monitoringStartStopButtonEnabled = value; OnPropertyChanged(); }
        }
        #endregion

        #region commands

        /// <summary>
        /// MonitoringStartStopCommand exposed for binding in a View
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
            if (_tradeDataMonitor.IsMonitoringStarted) // monitoring started
            {
                // will try to stop:
                // in case of any file procesing in background, we might await 
                // before change UI state
                MonitoringStartStopButtonContent = MonitoringStopAwaitText; // display await message within a button content
                MonitoringStartStopButtonEnabled = false; // disable button to prevent repeated calls

                await _tradeDataMonitor.StopMonitoringAsync(); // await on trying to stop monitoring process
                // when done:
                MonitoringStartStopButtonContent = MonitoringStartText; // display start monitring mesage within a button content
                MonitoringStartStopButtonBackground = MonitoringStartBackground; // chagne the background color of the button
                MonitoringStartStopButtonEnabled = true; // enable button
            }
            else // monitoring stopped
            {
                _tradeDataMonitor.StartMonitoring(); // start
                MonitoringStartStopButtonContent = MonitoringStopText; // display stop monitoring message within a button content
                MonitoringStartStopButtonBackground = MonitoringStopBackground; // change the background color of the button
            }
        }

        #endregion
    }
}
