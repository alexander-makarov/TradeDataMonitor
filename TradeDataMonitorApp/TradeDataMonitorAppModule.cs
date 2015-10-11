using Ninject.Modules;
using TradeDataMonitoring;

namespace TradeDataMonitorApp
{
    public class TradeDataMonitorAppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileSystemManager>().To<FileSystemManager>();
            Bind<ITimer>().To<TimerAdaper>();
            Bind<ITradeDataMonitor>().To<TradeDataMonitor>();
        }
    }
}