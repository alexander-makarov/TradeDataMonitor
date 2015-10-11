using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring.TradeDataLoaders
{
    /// <summary>
    /// TradeDataLoader for .txt files
    /// </summary>
    public class TxtFileTradeDataLoader : FileBasedTradeDataLoader
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
            using (var sr = new StreamReader(stream))
            {
                string line;
                sr.ReadLine(); // skip the first line with columns names
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        var arr = line.Split(';'); // split by value separator symbol
                        var data = TradeData.Parse(arr); // parse values into TradeData object
                        dataList.Add(data);
                    }
                    catch
                    {
                        // suppress any exceptions when trying to read corrupted data from file
                        // disputable decision
                        // TODO: possibly revise exception suppressing later
                    }
                }
            }

            return new TradeDataPackage(dataList);
        }

        /// <summary>
        /// Supported file type extension (e.g. ".xml")
        /// </summary>
        protected override string SupportedFileTypeExtension
        {
            get { return ".txt"; }
        }
    }
}