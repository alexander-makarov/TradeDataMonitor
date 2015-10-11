using System.IO;

namespace TradeDataMonitoring.TradeDataLoaders
{
    /// <summary>
    /// Abstract class for any TradeDataLoader classes that based on
    /// one file type to load. 
    /// <remarks>
    /// Done, for sake of duplicated code elimination, according to The Rule of Three
    /// </remarks> 
    /// </summary>
    public abstract class FileBasedTradeDataLoader : ITradeDataLoader
    {
        /// <summary>
        /// Checking that file is supported and trade data could be loaded
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>true if able to load data</returns>
        public bool CouldLoad(FileInfo file)
        {
            // checking the appropriate extension:
            return file.Extension.TrimStart('.') == SupportedFileTypeExtension.TrimStart('.');
        }

        /// <summary>
        /// Loading trade data from a file
        /// </summary>
        /// <param name="file">file to read</param>
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
        /// <remarks>method allocated to decouple from filestreams and to provide testability on memorystreams</remarks>
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <returns>Single package of trade data that has been loaded</returns>
        public abstract TradeDataPackage LoadTradeData(Stream stream);

        /// <summary>
        /// Supported file type extension (e.g. ".xml")
        /// </summary>
        protected abstract string SupportedFileTypeExtension { get; }
    }
}
