using System;
using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring
{
    public class CsvFileTradeDataLoader : ITradeDataLoader
    {
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == "csv";
        }

        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var dataList = new List<TradeData>();

            using (FileStream fs = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (var sr = new StreamReader(bs))
            {
                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var arr = line.Split(',');
                    var data = TradeData.Parse(arr);
                    dataList.Add(data);
                }
            }

            return new TradeDataPackage { Package = dataList };
        }
    }
}