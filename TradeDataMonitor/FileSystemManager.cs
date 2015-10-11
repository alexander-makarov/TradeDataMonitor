using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeDataMonitoring
{
    public class FileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// Returns list of files from specified directory
        /// that have been created later a given time
        /// </summary>
        /// <param name="createdLaterUtc">creation time later to filter files</param>
        /// <param name="directoryPath">path to a directory</param>
        /// <returns>list of files</returns>
        public IEnumerable<FileInfo> GetNewFilesFromDirectory(DateTime createdLaterUtc, string directoryPath)
        {
            var info = new DirectoryInfo(directoryPath);
            var files = info.GetFiles().Where(f => f.CreationTimeUtc > createdLaterUtc);
            return files;
        }
    }
}