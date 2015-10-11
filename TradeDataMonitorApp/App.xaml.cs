using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Ninject;
using Ninject.Parameters;
using TradeDataMonitorApp.ViewModels;
using TradeDataMonitoring;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Fatal(e.Exception.ToString); // log any unhandled exceptions
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                AppSettings.Load(); // load settings from app.config
                
                IKernel kernel = new StandardKernel();
                kernel.Bind<IFileSystemManager>().To<FileSystemManager>();
                kernel.Bind<ITimer>().To<TimerAdaper>();
                kernel.Bind<ITradeDataMonitor>().To<TradeDataMonitor>();
                
                // bundle all loaders within one CompositeTradeDataLoader to transfer into TradeDataMonitor
                var compositeLoader = new CompositeTradeDataLoader(AppSettings.TradeDataLoaders);

                // create TradeDataMonitor class - the model
                var model = kernel.Get<ITradeDataMonitor>(
                     new ConstructorArgument("timerPeriodSeconds", AppSettings.MonitoringPeriodSeconds),
                     new ConstructorArgument("monitoringDirectory", AppSettings.MonitoringDirectoryPath),
                     new ConstructorArgument("tradeDataLoader", compositeLoader));

                var viewModel = new TradeDataMonitorViewModel(model); // initialize ViewModel with TradeDataMonitor
                var mw = new MainWindow(viewModel);
                mw.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Error during application execution", MessageBoxButton.OK, MessageBoxImage.Error);
                NLog.LogManager.GetCurrentClassLogger().Error(exc.ToString);
            }
        }
    }
}
