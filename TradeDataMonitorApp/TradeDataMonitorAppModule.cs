using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using TradeDataMonitoring;

namespace TradeDataMonitorApp
{
    public class TradeDataMonitorAppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileSystemManager>().To<FileSystemManager>();
            Bind<ITimer>().To<TimerWrapper>().InParentScope(); // using parent scope cause IDisposable
            Bind<ITradeDataMonitor>().To<TradeDataMonitor>();
            Bind<IDispatcher>().To<DispatcherWrapper>();
        }
    }
}