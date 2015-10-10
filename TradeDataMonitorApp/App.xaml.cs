using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using TradeDataMonitorApp.Configuration;
using TradeDataMonitorApp.ViewModels;
using TradeDataMonitoring;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(e.Exception.ToString);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // load all specified TradeDataLoaders at runtime
            var loadersList = new List<ITradeDataLoader>();
            var loaders = ConfigurationManager.GetSection("TradeDataLoadersSection") as TradeDataLoadersSection;
            foreach (TradeDataLoaderElement loaderTypeElement in loaders.TradeDataLoaders)
            {
                var loader = LoadClassInstanceFromAssembly<ITradeDataLoader>(loaderTypeElement.Assembly, loaderTypeElement.Class);
                loadersList.Add(loader);
            }
            // bundle all loaders within one CompositeTradeDataLoader
            var compositeLoader = new CompositeTradeDataLoader(loadersList);

            // read other settings:
            var monitoringPeriodSeconds = Int32.Parse(ConfigurationManager.AppSettings["UpdatesMonitoringPeriodSeconds"]);
            var monitoringDirectoryPath = ConfigurationManager.AppSettings["MonitoringDirectoryPath"];

            var m = new TradeDataMonitor(compositeLoader, monitoringPeriodSeconds, monitoringDirectoryPath);
            var viewModel = new TradeDataMonitorViewModel(m);

            var mw = new MainWindow(viewModel);
            mw.Show();
        }


        /// <summary>
        /// Загрузка типа из сборки
        /// </summary>
        /// <param name="fileName">Имя файла сборки</param>
        /// <param name="className">Имя класса</param>
        /// <returns>Загруженный тип</returns>
        public static Type LoadTypeFromAssembly(string fileName, string className)
        {
            Assembly a = Assembly.LoadFrom(fileName);
            var type = a.GetType(className);
            if (type == null)
            {
                throw new Exception(String.Format("Класс '{0}' в сборке '{1}' не найден", className, fileName));
            }
            return type;
        }

        /// <summary>
        /// Загрузка экземпляра класса из сборки
        /// </summary>
        /// <typeparam name="T">Тип, к которому нужно привести результат. Как правило это интерфейс.</typeparam>
        /// <param name="fileName">Имя файла сборки</param>
        /// <param name="className">Имя класса</param>
        /// <returns>Экземпляр класса, приведённый к типу T</returns>
        public static T LoadClassInstanceFromAssembly<T>(string fileName, string className)
        {
            try
            {
                Type t = LoadTypeFromAssembly(fileName, className);
                T obj = (T)Activator.CreateInstance(t);
                return obj;
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Error on creating instance of class '{0}' from '{1}' : {2}", className, fileName, exc.Message),
                                    exc);
            }
        }
    }
}
