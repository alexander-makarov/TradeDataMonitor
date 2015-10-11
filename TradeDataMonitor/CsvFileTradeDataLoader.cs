using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring
{
    /// <summary>
    /// TradeDataLoader for .csv files
    /// </summary>
    public class CsvFileTradeDataLoader : ITradeDataLoader
    {
        /// <summary>
        /// Checking that file is supported and trade data could be loaded
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>true if able to load data</returns>
        public bool CouldLoad(FileInfo file)
        {
            return file.Extension == ".csv"; // checking the appropriate extension
        }

        /// <summary>
        /// Loading trade data from  .csv file
        /// </summary>
        /// <param name="file">.csv file to read</param>
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
            using (var sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        var arr = line.Split(','); // split by value separator symbol
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
    }
}