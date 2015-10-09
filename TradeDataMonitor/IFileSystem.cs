using System;
using System.Collections.Generic;

namespace TradeDataMonitoring
{
    public interface IFileSystem
    {
        List<FileMetadata> GetNewFiles(DateTime createdLater);
    }
}