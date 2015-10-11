using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using TradeDataMonitorApp.Configuration;
using TradeDataMonitoring;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Class loads application setting from app.config
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// True when application settings has been loaded succesfully by <see cref="AppSettings.Load"/>
        /// </summary>
        private static bool _isLoaded = false;
        private static readonly List<ITradeDataLoader> _tradeDataLoaders = new List<ITradeDataLoader>();
        private static int _monitoringPeriodSeconds = -1;
        private static string _monitoringDirectoryPath = null;

        /// <summary>
        /// Collection of ITradeDataLoader instances constucted at runtime from configuration file
        /// </summary>
        public static IReadOnlyList<ITradeDataLoader> TradeDataLoaders
        {
            get
            {
                if (!_isLoaded)
                {
                    throw new InvalidOperationException("Application settings have not been loaded");
                }
                return _tradeDataLoaders;
            }
        }

        /// <summary>
        /// Period in seconds of how frequently directory should be checked for updates
        /// </summary>
        public static int MonitoringPeriodSeconds
        {
            get 
            {
                if (!_isLoaded)
                {
                    throw new InvalidOperationException("Application settings have not been loaded");
                }
                return _monitoringPeriodSeconds; 
            }
        }

        /// <summary>
        /// Path to directory to be monitored for trade data updates
        /// </summary>
        public static string MonitoringDirectoryPath
        {
            get 
            {
                if (!_isLoaded)
                {
                    throw new InvalidOperationException("Application settings have not been loaded");
                }
                return _monitoringDirectoryPath; 
            }
        }

        /// <summary>
        /// Load and validate application configuration
        /// </summary>
        public static void Load()
        {
            try
            {
                #region load all specified TradeDataLoaders at runtime from app.config
                var loaders = ConfigurationManager.GetSection("TradeDataLoadersSection") as TradeDataLoadersSection; // get app.config section
                if (loaders == null)
                {
                    throw new ConfigurationErrorsException("Can't retrieve <section name=\"TradeDataLoadersSection\".../> from 'TradeDataMonitorApp.exe.config'");
                }
                if (loaders.TradeDataLoaders.Count == 0)
                {
                    throw new ConfigurationErrorsException("At least one TradeDataLoader element must be specified within <TradeDataLoaders>...</TradeDataLoaders>");
                }

                foreach (TradeDataLoaderElement loaderTypeElement in loaders.TradeDataLoaders) // get through section
                {
                    // try to load and instantiate all TradeDataLoaders
                    var loader = AssemblyHelpers.LoadClassInstanceFromAssembly<ITradeDataLoader>(loaderTypeElement.Assembly, loaderTypeElement.Class);
                    _tradeDataLoaders.Add(loader); // add to the list
                }
                #endregion

                // load other settings:
                _monitoringPeriodSeconds = Int32.Parse(ConfigurationManager.AppSettings["UpdatesMonitoringPeriodSeconds"]);
                if (_monitoringPeriodSeconds < 0 )
                {
                    throw new ConfigurationErrorsException(String.Format("Incorrect value - {0} for 'UpdatesMonitoringPeriodSeconds'", _monitoringPeriodSeconds));
                }

                _monitoringDirectoryPath = ConfigurationManager.AppSettings["MonitoringDirectoryPath"];
                if (!Directory.Exists(_monitoringDirectoryPath))
                {
                    throw new ConfigurationErrorsException(String.Format("Directory 'MonitoringDirectoryPath' doesn't exists - {0}", _monitoringDirectoryPath));
                }

                _isLoaded = true; // succesfully loaded
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception exc)
            {
                throw new ConfigurationErrorsException("Can't load AppSettings from 'TradeDataMonitorApp.exe.config'", exc);
            }

        }
    }
}