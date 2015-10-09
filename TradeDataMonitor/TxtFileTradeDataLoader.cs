using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring
{
    public class TxtFileTradeDataLoader : ITradeDataLoader
    {
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == "txt";
        }

        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var dataList = new List<TradeData>();

            using (FileStream fs = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (var sr = new StreamReader(bs))
            {
                string line;
                sr.ReadLine(); // skip the first line with columns names
                while ((line = sr.ReadLine()) != null)
                {
                    var arr = line.Split(',');
                    var data = TradeData.Parse(arr);
                    dataList.Add(data);
                }
            }

            return new TradeDataPackage(dataList);
        }
    }
}