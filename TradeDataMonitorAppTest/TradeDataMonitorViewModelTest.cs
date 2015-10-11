using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private const string MonitoringStartText = "Start monitoring the directory for updates";

        [TestMethod]
        public void ConstructorDefaultValues_NoException()
        {
            // arrange
            const string expectedDirectory = "X:\\some-expected-path\\dir1\\";
            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>();
            mockedTradeDataMonitor.CallsTo(m => m.MonitoringDirectory).Returns(expectedDirectory);
            // act
            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, A.Fake<IDispatcher>()); // inject

            // assert
            Assert.AreEqual(MonitoringStartText, viewModel.MonitoringStartStopButtonContent);
            Assert.AreEqual(true, viewModel.MonitoringStartStopButtonEnabled);
            Assert.AreEqual(Colors.PaleGreen, viewModel.MonitoringStartStopButtonBackground.Color);
            Assert.AreEqual(1.0, viewModel.MonitoringStartStopButtonBackground.Opacity);
            Assert.AreEqual(expectedDirectory, viewModel.MonitoringDirectory);
        }

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
            Assert.AreEqual(true, viewModel.MonitoringStartStopButtonEnabled);
            Assert.AreEqual(Colors.PaleVioletRed, viewModel.MonitoringStartStopButtonBackground.Color);
            Assert.AreEqual(1.0, viewModel.MonitoringStartStopButtonBackground.Opacity);
            Assert.AreEqual("Stop monitoring the directory for updates", viewModel.MonitoringStartStopButtonContent);
        }

        [TestMethod]
        public void MonitoringStartStop_MonitoringStartedAndCurrentFileReading_NoException()
        {
            // arrange
            var mockedTradeDataMonitor = A.Fake<ITradeDataMonitor>(); // setup mocked model ITradeDataMonitor
            mockedTradeDataMonitor.CallsTo(m => m.IsMonitoringStarted).Returns(true); // initially monitoring started

            Action longtimeReturnTask = () => { while (true); }; // simulating longtime monitoring stop like some large files were reading in background
            mockedTradeDataMonitor.CallsTo(m => m.StopMonitoringAsync()).Returns(Task.Run(longtimeReturnTask)); // allegedly background file reading, so awaiting
            
            var viewModel = new TradeDataMonitorViewModel(mockedTradeDataMonitor, A.Fake<IDispatcher>()); // inject

            // act
            viewModel.MonitoringStartStop(null); // await on stop monitoring (since it's started and there are file reading involved)

            // assert
            Assert.AreEqual(false, viewModel.MonitoringStartStopButtonEnabled);
            Assert.AreEqual("Will stop monitoring. Await on active file reading operations...", viewModel.MonitoringStartStopButtonContent);
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
            Assert.AreEqual(expectedTradeDataList.Count, viewModel.TradeDataList.Count);
            for (int i = 0; i < expectedTradeDataList.Count; i++)
            {
                Assert.AreEqual(expectedTradeDataList[i], viewModel.TradeDataList[i]);
            }
        }
    }
}
