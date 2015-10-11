using System.Collections.Generic;
using System.IO;
using System.Xml;
using TradeDataMonitoring;
using TradeDataMonitoring.TradeDataLoaders;

namespace XmlFileTradeData
{
    /// <summary>
    /// TradeDataLoader for .xml files
    /// </summary>
    public class XmlFileTradeDataLoader : FileBasedTradeDataLoader
    {
        /// <summary>
        /// Reading a stream by lines
        /// trying to parse TradeData
        /// <remarks>method allocated to decouple from filesteams and to provide testability on memorystreams</remarks>
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <returns>Single package of trade data that has been loaded</returns>
        public override TradeDataPackage LoadTradeData(Stream stream)
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

        /// <summary>
        /// Supported file type extension (e.g. ".xml")
        /// </summary>
        protected override string SupportedFileTypeExtension
        {
            get { return ".xml"; }
        }
    }
}