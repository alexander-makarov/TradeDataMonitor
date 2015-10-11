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

                TradeDataMonitorAppSettings.Load(); // load settings from app.config
                
                IKernel kernel = new StandardKernel(new TradeDataMonitorAppModule()); // ninject bindings
                
                // bundle all loaders within one CompositeTradeDataLoader to transfer into TradeDataMonitor
                var compositeLoader = new CompositeTradeDataLoader(TradeDataMonitorAppSettings.TradeDataLoaders);

                // create TradeDataMonitor class - the model
                var model = kernel.Get<ITradeDataMonitor>(
                     new ConstructorArgument("timerPeriodSeconds", TradeDataMonitorAppSettings.MonitoringPeriodSeconds),
                     new ConstructorArgument("monitoringDirectory", TradeDataMonitorAppSettings.MonitoringDirectoryPath),
                     new ConstructorArgument("tradeDataLoader", compositeLoader));

                // initialize ViewModel with TradeDataMonitor
                var viewModel = kernel.Get<TradeDataMonitorViewModel>(new ConstructorArgument("tradeDataMonitor", model));
                    
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
