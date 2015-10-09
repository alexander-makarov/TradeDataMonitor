using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TradeDataMonitoring
{
    public class XmlFileTradeDataLoader : ITradeDataLoader
    {
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == "xml";
        }

        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var dataList = new List<TradeData>();

            using (FileStream fs = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (XmlReader reader = XmlReader.Create(bs))
            {
                reader.ReadToFollowing("value");
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

            return new TradeDataPackage { Package = dataList };
        }
    }
}