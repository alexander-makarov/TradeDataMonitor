using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeDataMonitoring
{
    public class FileSystemManager : IFileSystemManager
    {
        public IEnumerable<FileInfo> GetNewFilesFromDirectory(DateTime createdLater, string directoryPath)
        {
            var info = new DirectoryInfo(directoryPath);
            var files = info.GetFiles().Where(f => f.CreationTime > createdLater);
            return files;
        }
    }
}