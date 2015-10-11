using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using TradeDataMonitoring;

namespace XmlFileTradeData
{
    /// <summary>
    /// TradeDataLoader for .xml files
    /// </summary>
    public class XmlFileTradeDataLoader : ITradeDataLoader
    {
        /// <summary>
        /// Checking that file is supported and trade data could be loaded
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>true if able to load data</returns>
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == ".xml"; // checking the appropriate extension
        }

        /// <summary>
        /// Loading trade data from  .xml file
        /// </summary>
        /// <param name="file">.xml file to read</param>
        /// <returns>single package of trade data that has been loaded</returns>
        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            using (FileStream fs = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            {
                return LoadTradeData(bs);
            }
        }

        /// <summary>
        /// Reading a stream by lines
        /// trying to parse TradeData
        /// <remarks>method allocated to decouple from filesteams and to provide testability on memorystreams</remarks>
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <returns>Single package of trade data that has been loaded</returns>
        public TradeDataPackage LoadTradeData(Stream stream)
        {
            var dataList = new List<TradeData>();
            using (XmlReader reader = XmlReader.Create(stream))
            {
                try
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
                catch
                {
                    // suppress any exceptions when trying to read corrupted data from file or different xml schema
                    // disputable decision
                    // TODO: possibly revise exception suppressing later
                }
            }

            return new TradeDataPackage(dataList);
        }
    }
}