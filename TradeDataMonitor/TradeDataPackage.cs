using System;
using System.Collections.Generic;

namespace TradeDataMonitoring
{
    /// <summary>
    /// TradeDataPackage simlpy keeps colection of TradeData objects
    /// Used to represent a single update of trade data obtained from ITradeDataLoader
    /// <remarks>derive from EventArgs just for unit-testing (to allow FakeItEasy raise events from ITradeDataMonitor)</remarks>
    /// </summary>
    public class TradeDataPackage : EventArgs
    {
        /// <summary>
        /// List of TradeData objects
        /// </summary>
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