using System.IO;

namespace TradeDataMonitoring
{
    /// <summary>
    /// Common interface for trade data loaders
    /// </summary>
    public interface ITradeDataLoader
    {
        /// <summary>
        /// Checking that file is supported and trade data could be loaded
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>true if able to load data</returns>
        bool CouldLoad(FileInfo file);

        /// <summary>
        /// Loading trade data from  a file
        /// </summary>
        /// <param name="file">file to read</param>
        /// <returns>single package of trade data that has been loaded</returns>
        TradeDataPackage LoadTradeData(FileInfo file);
    }
}