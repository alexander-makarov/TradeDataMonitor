using System.Collections.Generic;

namespace TradeDataMonitoring
{
    public class TradeDataPackage
    {
        public List<TradeData> Package { get; private set; }

        public TradeDataPackage()
        {
            Package = new List<TradeData>();
        }

        public TradeDataPackage(List<TradeData> dataList)
        {
            Package = dataList;
        }
    }
}