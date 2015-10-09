using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeDataMonitoring
{
    public class CompositeTradeDataLoader : ITradeDataLoader
    {
        private readonly List<ITradeDataLoader> _loaders;

        public CompositeTradeDataLoader(List<ITradeDataLoader> loaders)
        {
            _loaders = loaders;
        }

        public bool CouldLoad(FileInfo file)
        {
            return _loaders.Any(l => l.CouldLoad(file));
        }

        public TradeDataPackage LoadTradeData(FileInfo file)
        {
            var loader = _loaders.First(l => l.CouldLoad(file));
            return loader.LoadTradeData(file);
        }
    }
}