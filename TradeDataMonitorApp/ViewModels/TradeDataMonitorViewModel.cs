using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            _monitor.StartMonitor();

            //InitCommands();
            const string correctCsvString = "2013-5-20,30.16,30.39,30.02,30.17,1478200";
            var corectValues = correctCsvString.Split(',');
            var data = TradeData.Parse(corectValues);
            
            _tradeDataList.Add(data);
            
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
    }
}
