using System.IO;

namespace TradeDataMonitoring
{
    public interface ITradeDataLoader
    {
        bool CouldLoad(FileInfo file);
        TradeDataPackage LoadTradeData(FileInfo file);
    }
}