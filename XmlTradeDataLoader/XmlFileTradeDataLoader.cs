using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using TradeDataMonitoring;

namespace XmlFileTradeData
{
    public class XmlFileTradeDataLoader : ITradeDataLoader
    {
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == ".xml";
        }

        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var dataList = new List<TradeData>();

            using (FileStream fs = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (XmlReader reader = XmlReader.Create(bs))
            {
                while (reader.ReadToFollowing("value"))
                {
                    var valuesArray = new string[6];
                    reader.MoveToFirstAttribute();
                    valuesArray[0] = reader.Value;

                    int i = 1;
                    while (reader.MoveToNextAttribute())
                    {
                        valuesArray[i++] = reader.Value;
                    }

                    var data = TradeData.Parse(valuesArray);
                    dataList.Add(data);
                }
            }
            Thread.Sleep(15000);
            return new TradeDataPackage(dataList);
        }
    }
}