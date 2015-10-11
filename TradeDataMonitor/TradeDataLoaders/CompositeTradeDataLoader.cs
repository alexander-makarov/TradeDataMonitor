using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeDataMonitoring.TradeDataLoaders
{
    /// <summary>
    /// CompositeTradeDataLoader encapsulates collection of loaders 
    /// and let clients of the class to work with them as with a single ITradeDataLoader instance
    /// </summary>
    public class CompositeTradeDataLoader : ITradeDataLoader
    {
        /// <summary>
        /// Collection of ITradeDataLoader
        /// </summary>
        private readonly IEnumerable<ITradeDataLoader> _loaders;

        /// <summary>
        /// Creates composite object from a collection of ITradeDataLoader
        /// </summary>
        /// <param name="loaders">collection of ITradeDataLoader</param>
        public CompositeTradeDataLoader(IEnumerable<ITradeDataLoader> loaders)
        {
            _loaders = loaders;
        }

        /// <summary>
        /// Checking that file is supported and trade data could be loaded
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>true if able to load data</returns>
        public bool CouldLoad(FileInfo file)
        {
            return _loaders.Any(l => l.CouldLoad(file)); // if at least one of the loaders in collection could load
        }

        /// <summary>
        /// Loading trade data from  a file
        /// </summary>
        /// <param name="file">file to read</param>
        /// <returns>single package of trade data that has been loaded</returns>
        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var loader = _loaders.First(l => l.CouldLoad(file)); // get appropriate loader from a collection
            return loader.LoadTradeData(file); // load data by appropriate loader
        }
    }
}