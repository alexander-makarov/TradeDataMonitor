using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using TradeDataMonitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradeDataMonitorTest
{
    [TestClass]
    public class TradeDataMonitorTest
    {
        //[TestMethod]
        //public void TradeDataUpdateEventFired_NewFile_NoException()
        //{
        //    // arrange
        //    var someNewFile = new FileInfo("someNewFile.csv");
            
        //    var mockFileSystem = A.Fake<IFileSystemManager>();
        //    mockFileSystem.CallsTo(system => system.GetNewFiles(DateTime.MinValue)).Returns(new List<FileInfo> {someNewFile});
            
        //    var mockTradeDataLoader = A.Fake<ITradeDataLoader>();
        //    mockTradeDataLoader.CallsTo(loader => loader.CouldLoad(someNewFile)).Returns(true);
        //    mockTradeDataLoader.CallsTo(loader => loader.LoadTradeData(someNewFile)).Returns(new TradeDataPackage());

        //    var mockTimer = A.Fake<ITimer>(); // mock the timer
            
        //    // preserve TimerCallback initialized internally within TradeDataMonitor:
        //    TimerCallback timerCallback = null;
        //    mockTimer.CallsTo(timer => timer.Init(null, null, 0, 0)).WithAnyArguments()
        //        .Invokes((TimerCallback callback, object o, int due, int per) => timerCallback = callback); // save callback on Init call

        //    // override Change call to produce immediate timer tick by invoking preserved callback:
        //    mockTimer.CallsTo(timer => timer.Change(5000, Timeout.Infinite)).WithAnyArguments().Invokes(
        //        () =>
        //        {
        //            if (timerCallback != null)
        //            {
        //                var tmp = timerCallback; // need temp variable to avoid infinite calls
        //                timerCallback = null; // reset to null, cause need to invoke callback just once here
        //                tmp(new object()); // invoke callback
        //            }
        //        });
            
        //    var monitor = new TradeDataMonitor(mockFileSystem, mockTradeDataLoader, 5, mockTimer);
        //    bool updateEventHasBeenFired = false;
        //    monitor.TradeDataUpdate += delegate(TradeDataPackage dataPackage)
        //    {
        //        updateEventHasBeenFired = true;
        //    };

        //    // act
        //    monitor.StartMonitor();

        //    // assert
        //    Assert.IsTrue(updateEventHasBeenFired, "TradeDataUpdateEvent has not been fired");
        //}

        //[TestMethod]
        //public void TradeDataUpdateEventNotFired_NoNewFile_NoException()
        //{
        //    // arrange
        //    var mockFileSystem = A.Fake<IFileSystemManager>();
        //    mockFileSystem.CallsTo(system => system.GetNewFiles(DateTime.MinValue)).Returns(new List<FileInfo> {});
        //    var mockTradeDataLoader = A.Fake<ITradeDataLoader>();

        //    var monitor = new TradeDataMonitor(mockFileSystem, mockTradeDataLoader, 5);
        //    bool updateEventHasBeenFired = false;
        //    monitor.TradeDataUpdate += delegate(TradeDataPackage dataPackage)
        //    {
        //        updateEventHasBeenFired = true;
        //    };

        //    // act
        //    monitor.StartMonitor();

        //    // assert
        //    Assert.IsFalse(updateEventHasBeenFired, "TradeDataUpdateEvent has been fired");
        //}
    }
}
