using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using TradeDataMonitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitorTest
{
    [TestClass]
    public class TradeDataMonitorTest
    {
        [TestMethod]
        public void Constructor_NoException()
        {
            // arrange
            var mockedFileSystem = A.Fake<IFileSystemManager>();
            var mockedTimer = A.Fake<ITimer>();
            var loader = A.Fake<ITradeDataLoader>();
            const int periodSeconds = 5;
            const string monitoringDirectory = "X:\\folder1\\folder2\\";

            // act
            var monitor = new TradeDataMonitor(mockedFileSystem, mockedTimer, loader, periodSeconds, monitoringDirectory);

            // assert
            Assert.AreEqual(periodSeconds, monitor.TimerPeriodSeconds);
            Assert.AreEqual(monitoringDirectory, monitor.MonitoringDirectory);
            Assert.AreEqual(false, monitor.IsMonitoringStarted);
        }

        [TestMethod]
        public void StartMonitoring_NoException()
        {
            // arrange
            var mockedFileSystem = A.Fake<IFileSystemManager>();
            mockedFileSystem.CallsTo(m => m.GetNewFilesFromDirectory(DateTime.MinValue, null))
                .WithAnyArguments()
                .Returns(new List<FileInfo>());

            // setup mocked timer to immediate callback call (Timer_Tick) on ITimer.Change regardless of periods
            var mockedTimer = A.Fake<ITimer>();
            TimerCallback callbackTimerTick = null; // preserve callback on Timer.Init call
            mockedTimer.CallsTo(t => t.Init(null, null, 0, 0)).WithAnyArguments().
                Invokes(conf => callbackTimerTick = (conf.Arguments[0] as TimerCallback));

            bool wasCallbackCalledYet = false;
            mockedTimer.CallsTo(t => t.Change(0, 0)).WithAnyArguments().
                Invokes(conf =>
                {
                    if (!wasCallbackCalledYet)
                    {
                        wasCallbackCalledYet = true;
                        callbackTimerTick.Invoke(null);
                    }
                }); // call callback regardless of dueTimes periods, but only once to avoid infinite loop

            var loader = A.Fake<ITradeDataLoader>();
            const int periodSeconds = 5;
            const string monitoringDirectory = "X:\\folder1\\folder2\\";
            var monitor = new TradeDataMonitor(mockedFileSystem, mockedTimer, loader, periodSeconds, monitoringDirectory); // inject

            // act
            monitor.StartMonitoring();

            // assert
            Assert.AreEqual(true, monitor.IsMonitoringStarted);
            // assert timer starts:
            mockedTimer.CallsTo(m => m.Change(periodSeconds * 1000, Timeout.Infinite)).MustHaveHappened();
            // assert timer callback happens and we trying to check directory for a new files:
            mockedFileSystem.CallsTo(m => m.GetNewFilesFromDirectory(DateTime.MinValue, monitoringDirectory)).MustHaveHappened(); 
        }

        [TestMethod]
        public void StopMonitoringAsync_NoException()
        {
            // arrange
            var mockedFileSystem = A.Fake<IFileSystemManager>();
            var mockedTimer = A.Fake<ITimer>();
            var loader = A.Fake<ITradeDataLoader>();
            const int periodSeconds = 5;
            const string monitoringDirectory = "X:\\folder1\\folder2\\";
            var monitor = new TradeDataMonitor(mockedFileSystem, mockedTimer, loader, periodSeconds, monitoringDirectory); // inject
            monitor.StartMonitoring();

            // act
            monitor.StopMonitoringAsync().Wait();

            // assert
            Assert.AreEqual(false, monitor.IsMonitoringStarted);
        }

        [TestMethod]
        public void TradeDataUpdateEventFired_NewCorrectFilesInDirectory_NoException()
        {
            // arrange
            var someNewFile = new FileInfo("someNewFile.csv");
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
            #endregion
            var tradeDataPackageFromSomeNewFile = new TradeDataPackage(new List<TradeData> { data1, data2, data3 });

            var mockedFileSystem = A.Fake<IFileSystemManager>();
            mockedFileSystem.CallsTo(m => m.GetNewFilesFromDirectory(DateTime.MinValue, null))
                .WithAnyArguments()
                .Returns(new List<FileInfo> {someNewFile});

            // setup mocked timer to immediate callback call (Timer_Tick) on ITimer.Change regardless of periods
            var mockedTimer = A.Fake<ITimer>();
            TimerCallback callbackTimerTick = null; // preserve callback on Timer.Init call
            mockedTimer.CallsTo(t => t.Init(null, null, 0, 0)).WithAnyArguments().
                Invokes(conf => callbackTimerTick = (conf.Arguments[0] as TimerCallback));

            bool wasCallbackCalledYet = false;
            mockedTimer.CallsTo(t => t.Change(0, 0)).WithAnyArguments().
                Invokes(conf =>
                {
                    if (!wasCallbackCalledYet)
                    {
                        wasCallbackCalledYet = true;
                        callbackTimerTick.Invoke(null);
                    }
                }); // call callback regardless of dueTimes periods, but only once to avoid infinite loop

            var loader = A.Fake<ITradeDataLoader>();
            loader.CallsTo(l => l.CouldLoad(someNewFile)).Returns(true);
            loader.CallsTo(l => l.LoadTradeData(someNewFile)).Returns(tradeDataPackageFromSomeNewFile);
            const int periodSeconds = 5;
            const string monitoringDirectory = "X:\\folder1\\folder2\\";
            var monitor = new TradeDataMonitor(mockedFileSystem, mockedTimer, loader, periodSeconds, monitoringDirectory); // inject

            var mockedTradeDataUpdateEventHandler = A.Fake<EventHandler<TradeDataPackage>>();
            monitor.TradeDataUpdate += mockedTradeDataUpdateEventHandler;
            
            // act
            monitor.StartMonitoring();

            // assert TradeDataUpdateEvent has been fired with apropriate params
            A.CallTo(() => mockedTradeDataUpdateEventHandler.Invoke(monitor, tradeDataPackageFromSomeNewFile)).MustHaveHappened();
        }
    }
}
