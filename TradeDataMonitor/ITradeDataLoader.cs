namespace TradeDataMonitoring
{
    public interface ITradeDataLoader
    {
        bool CouldLoad(FileMetadata file);
        TradeDataPackage LoadTradeData(FileMetadata file);
    }
}