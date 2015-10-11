using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitorApp;
using TradeDataMonitorApp.ViewModels;
using TradeDataMonitoring;

namespace TradeDataMonitorAppTest
{
    [TestClass]
    public class TradeDataMonitorViewModelTest
    {
        [TestMethod]
        public void MonitoringStartStop_MonitoringStopped_NoException()
        {
            // arrange
            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>(); // setup mocked model ITradeDataMonitor
            mockedTradeDataMonitor.CallsTo(m => m.IsMonitoringStarted).Returns(false); // initially monitoring stopped
            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, A.Fake<IDispatcher>()); // inject

            // act
            viewModel.MonitoringStartStop(null); // start monitoring (since it's stopped)
            
            // assert
            Assert.AreEqual(viewModel.MonitoringStartStopButtonEnabled, true);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonBackground.Color, Colors.PaleVioletRed);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonBackground.Opacity, 1.0);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonContent, "Stop monitoring the directory for updates");
        }

        [TestMethod]
        public void MonitoringStartStop_MonitoringStartedNoCurrentFileReading_NoException()
        {
            // arrange
            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>(); // setup mocked model ITradeDataMonitor
            mockedTradeDataMonitor.CallsTo(m => m.IsMonitoringStarted).Returns(true); // initially monitoring started
            mockedTradeDataMonitor.CallsTo(m => m.StopMonitoringAsync()).Returns(Task.Run(()=>{})); // no background file reading, so stops immediately
            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, A.Fake<IDispatcher>()); // inject

            // act
            viewModel.MonitoringStartStop(null); // stop monitoring (since it's started)

            // assert
            Assert.AreEqual(viewModel.MonitoringStartStopButtonEnabled, true);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonBackground.Color, Colors.PaleGreen);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonBackground.Opacity, 1.0);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonContent, "Start monitoring the directory for updates");
        }

        [TestMethod]
        public void MonitoringStartStop_MonitoringStartedAndCurrentFileReading_NoException()
        {
            // arrange
            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>(); // setup mocked model ITradeDataMonitor
            mockedTradeDataMonitor.CallsTo(m => m.IsMonitoringStarted).Returns(true); // initially monitoring started
            mockedTradeDataMonitor.CallsTo(m => m.StopMonitoringAsync()).Returns(Task.Run(() => { while (true) ; })); // allegedly background file reading, so awaiting
            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, A.Fake<IDispatcher>()); // inject

            // act
            viewModel.MonitoringStartStop(null); // await on stop monitoring (since it's started and there are file reading involved)

            // assert
            Assert.AreEqual(viewModel.MonitoringStartStopButtonEnabled, false);
            Assert.AreEqual(viewModel.MonitoringStartStopButtonContent, "Will stop monitoring. Await on active file reading operations...");
        }

        [TestMethod]
        public void TradeDataList_OnTradeDataUpdate_NoException()
        {
            // arrange
            #region prepare trade data updates (TradeDataPackage)
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date.AddDays(1), open, high, low, close, volume);
            var data3 = new TradeData(date.AddDays(2), open, high, low, close, volume);
            var updatePackage1 = new TradeDataPackage(new List<TradeData> { data1, data2, data3 });
            var data4 = new TradeData(date.AddDays(3), open, high, low, close, volume);
            var data5 = new TradeData(date.AddDays(4), open, high, low, close, volume);
            var data6 = new TradeData(date.AddDays(5), open, high, low, close, volume);
            var updatePackage2 = new TradeDataPackage(new List<TradeData> { data4, data5, data6 });
            var expectedTradeDataList = new List<TradeData> {data1, data2, data3, data4, data5, data6};
            #endregion

            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>(); // setup mocked model ITradeDataMonitor

            var mockedAppDispatcher = A.Fake<IDispatcher>(); // setup mocked Application.Current.Dispatcher.Invoke()
            mockedAppDispatcher.CallsTo(d => d.Invoke(null)).WithAnyArguments().Invokes((conf) => (conf.Arguments.First() as Action).Invoke());

            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, mockedAppDispatcher); // inject

            // act
            mockedTradeDataMonitor.TradeDataUpdate += Raise.With(updatePackage1); // send first update to viewModel
            mockedTradeDataMonitor.TradeDataUpdate += Raise.With(updatePackage2); // send second update to viewModel

            // assert
            Assert.IsTrue(viewModel.TradeDataList.Count == expectedTradeDataList.Count);
            for (int i = 0; i < expectedTradeDataList.Count; i++)
            {
                Assert.AreEqual(viewModel.TradeDataList[i],  expectedTradeDataList[i]);
            }
        }
    }
}
