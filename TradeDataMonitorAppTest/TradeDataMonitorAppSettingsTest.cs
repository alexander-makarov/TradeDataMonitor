using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitorApp;
using TradeDataMonitorApp.Configuration;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitorAppTest
{
    [TestClass]
    public class TradeDataMonitorAppSettingsTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MonitoringDirectoryPath_NotLoadedSettings_InvalidOperationException()
        {
            // act
            var tryReadBeforeLoad = TradeDataMonitorAppSettings.MonitoringDirectoryPath;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MonitoringPeriodSeconds_NotLoadedSettings_InvalidOperationException()
        {
            // act
            var tryReadBeforeLoad = TradeDataMonitorAppSettings.MonitoringPeriodSeconds;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TradeDataLoaders_NotLoadedSettings_InvalidOperationException()
        {
            // act
            var tryReadBeforeLoad = TradeDataMonitorAppSettings.TradeDataLoaders;
        }

        [TestMethod]
        public void AllProperties_LoadedSettingsCorrect_NoException()
        {
            // arrange
            // all expected values that we gonna mock into further
            const int expectedPeriod = 5;
            string expectedDirectory = Environment.CurrentDirectory;
            var expectedLoaders = new List<ITradeDataLoader>() { new CsvFileTradeDataLoader() };

            var mockedConfigManager = A.Fake<IConfigurationManager>(); // mock System.Configuration.Manager

            #region mock fake values for AppSettings elements of app.config of System.Configuration.Manager
            var fakeAppSettings = new NameValueCollection
            {
                { "UpdatesMonitoringPeriodSeconds", expectedPeriod.ToString() },
                { "MonitoringDirectoryPath", expectedDirectory }
            };
            mockedConfigManager.CallsTo(cm => cm.AppSettings).Returns(fakeAppSettings);
            #endregion

            #region mock TradeDataLoadersSection of app.config of System.Configuration.Manager
            var appConfigSection = A.Fake<TradeDataLoadersSection>();
            var fakeTradeDataLoaderElementCollection = new TradeDataLoaderElementCollection();
            var tradeDataLoaderElement = new TradeDataLoaderElement
            {
                Assembly = "TradeDataMonitoring.dll",
                Class = "TradeDataMonitoring.TradeDataLoaders.CsvFileTradeDataLoader"
            };
            fakeTradeDataLoaderElementCollection.Add(tradeDataLoaderElement);
            appConfigSection.CallsTo(section => section.TradeDataLoaders).Returns(fakeTradeDataLoaderElementCollection);
            mockedConfigManager.CallsTo(cm => cm.GetSection("TradeDataLoadersSection")).Returns(appConfigSection);
            #endregion


            // call Load with all the mocked stuff
            TradeDataMonitorAppSettings.Load(mockedConfigManager);

            // act
            var tryReadPeriod = TradeDataMonitorAppSettings.MonitoringPeriodSeconds;
            var tryReadDirectory = TradeDataMonitorAppSettings.MonitoringDirectoryPath;
            var tryReadLoaders = TradeDataMonitorAppSettings.TradeDataLoaders;

            // assert
            Assert.AreEqual(tryReadPeriod, expectedPeriod);
            Assert.AreEqual(tryReadDirectory, expectedDirectory);
            Assert.IsTrue(tryReadLoaders.Count == expectedLoaders.Count
                && tryReadLoaders.First().GetType() == expectedLoaders.First().GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Load_UpdatesMonitoringPeriodSecondsIncorrectValue_ConfigurationErrorsException()
        {
            // arrange
            // all expected values that we gonna mock into further
            const int expectedPeriod = -5; // INCORRECT VALUE
            string expectedDirectory = Environment.CurrentDirectory;

            var mockedConfigManager = A.Fake<IConfigurationManager>(); // mock System.Configuration.Manager

            #region mock fake values for AppSettings elements of app.config of System.Configuration.Manager
            var fakeAppSettings = new NameValueCollection
            {
                { "UpdatesMonitoringPeriodSeconds", expectedPeriod.ToString() },
                { "MonitoringDirectoryPath", expectedDirectory }
            };
            mockedConfigManager.CallsTo(cm => cm.AppSettings).Returns(fakeAppSettings);
            #endregion

            #region mock TradeDataLoadersSection of app.config of System.Configuration.Manager
            var appConfigSection = A.Fake<TradeDataLoadersSection>();
            var fakeTradeDataLoaderElementCollection = new TradeDataLoaderElementCollection();
            var tradeDataLoaderElement = new TradeDataLoaderElement
            {
                Assembly = "TradeDataMonitoring.dll",
                Class = "TradeDataMonitoring.TradeDataLoaders.CsvFileTradeDataLoader"
            };
            fakeTradeDataLoaderElementCollection.Add(tradeDataLoaderElement);
            appConfigSection.CallsTo(section => section.TradeDataLoaders).Returns(fakeTradeDataLoaderElementCollection);
            mockedConfigManager.CallsTo(cm => cm.GetSection("TradeDataLoadersSection")).Returns(appConfigSection);
            #endregion

            // act
            TradeDataMonitorAppSettings.Load(mockedConfigManager); // call Load with all the mocked stuff
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Load_MonitoringDirectoryPathIncorrectValue_ConfigurationErrorsException()
        {
            // arrange
            // all expected values that we gonna mock into further
            const int expectedPeriod = 5;
            string expectedDirectory = "not-existent-directory-path"; // INCORRECT VALUE

            var mockedConfigManager = A.Fake<IConfigurationManager>(); // mock System.Configuration.Manager

            #region mock fake values for AppSettings elements of app.config of System.Configuration.Manager
            var fakeAppSettings = new NameValueCollection
            {
                { "UpdatesMonitoringPeriodSeconds", expectedPeriod.ToString() },
                { "MonitoringDirectoryPath", expectedDirectory }
            };
            mockedConfigManager.CallsTo(cm => cm.AppSettings).Returns(fakeAppSettings);
            #endregion

            #region mock TradeDataLoadersSection of app.config of System.Configuration.Manager
            var appConfigSection = A.Fake<TradeDataLoadersSection>();
            var fakeTradeDataLoaderElementCollection = new TradeDataLoaderElementCollection();
            var tradeDataLoaderElement = new TradeDataLoaderElement
            {
                Assembly = "TradeDataMonitoring.dll",
                Class = "TradeDataMonitoring.TradeDataLoaders.CsvFileTradeDataLoader"
            };
            fakeTradeDataLoaderElementCollection.Add(tradeDataLoaderElement);
            appConfigSection.CallsTo(section => section.TradeDataLoaders).Returns(fakeTradeDataLoaderElementCollection);
            mockedConfigManager.CallsTo(cm => cm.GetSection("TradeDataLoadersSection")).Returns(appConfigSection);
            #endregion

            // act
            TradeDataMonitorAppSettings.Load(mockedConfigManager); // call Load with all the mocked stuff
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Load_TradeDataLoadersSectionIsNotSpecified_ConfigurationErrorsException()
        {
            // arrange
            // all expected values that we gonna mock into further
            const int expectedPeriod = 5;
            string expectedDirectory = Environment.CurrentDirectory;

            var mockedConfigManager = A.Fake<IConfigurationManager>(); // mock System.Configuration.Manager

            #region mock fake values for AppSettings elements of app.config of System.Configuration.Manager
            var fakeAppSettings = new NameValueCollection
            {
                { "UpdatesMonitoringPeriodSeconds", expectedPeriod.ToString() },
                { "MonitoringDirectoryPath", expectedDirectory }
            };
            mockedConfigManager.CallsTo(cm => cm.AppSettings).Returns(fakeAppSettings);
            #endregion

            #region mock TradeDataLoadersSection of app.config of System.Configuration.Manager
            TradeDataLoadersSection appConfigSection = null; // INCORRECT VALUE
            mockedConfigManager.CallsTo(cm => cm.GetSection("TradeDataLoadersSection")).Returns(appConfigSection);
            #endregion

            // act
            TradeDataMonitorAppSettings.Load(mockedConfigManager); // call Load with all the mocked stuff
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Load_TradeDataLoadersSectionSpecifiedButEmpty_ConfigurationErrorsException()
        {
            // arrange
            // all expected values that we gonna mock into further
            const int expectedPeriod = 5;
            string expectedDirectory = Environment.CurrentDirectory;

            var mockedConfigManager = A.Fake<IConfigurationManager>(); // mock System.Configuration.Manager

            #region mock fake values for AppSettings elements of app.config of System.Configuration.Manager
            var fakeAppSettings = new NameValueCollection
            {
                { "UpdatesMonitoringPeriodSeconds", expectedPeriod.ToString() },
                { "MonitoringDirectoryPath", expectedDirectory }
            };
            mockedConfigManager.CallsTo(cm => cm.AppSettings).Returns(fakeAppSettings);
            #endregion

            #region mock TradeDataLoadersSection of app.config of System.Configuration.Manager
            var appConfigSection = A.Fake<TradeDataLoadersSection>();
            var fakeTradeDataLoaderElementCollection = new TradeDataLoaderElementCollection(); // INCORRECT VALUE
            appConfigSection.CallsTo(section => section.TradeDataLoaders).Returns(fakeTradeDataLoaderElementCollection);
            mockedConfigManager.CallsTo(cm => cm.GetSection("TradeDataLoadersSection")).Returns(appConfigSection);
            #endregion

            // act
            TradeDataMonitorAppSettings.Load(mockedConfigManager); // call Load with all the mocked stuff
        }
    }
}
