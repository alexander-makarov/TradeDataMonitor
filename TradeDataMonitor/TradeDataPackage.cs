using System.Collections.Generic;

namespace TradeDataMonitoring
{
    /// <summary>
    /// TradeDataPackage simlpy keeps colection of TradeData elements
    /// Used to represent a single update of trade data obtained from ITradeDataLoader
    /// </summary>
    public class TradeDataPackage
    {
        public List<TradeData> TradeDataList { get; private set; }

        /// <summary>
        /// Creates an empty package of trade data
        /// </summary>
        public TradeDataPackage()
        {
            TradeDataList = new List<TradeData>();
        }

        /// <summary>
        /// Creates package of trade data
        /// </summary>
        /// <param name="dataList">colection of TradeData elements</param>
        public TradeDataPackage(List<TradeData> dataList) : this()
        {
            TradeDataList = dataList;
        }
    }
}