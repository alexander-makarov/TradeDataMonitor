using System;
using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring
{
    /// <summary>
    /// Interface to decouple classes from I/O opreations, for sake of testability
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// Returns list of files from specified directory
        /// that have been created later a given time
        /// </summary>
        /// <param name="createdLaterUtc">creation time later to filter files</param>
        /// <param name="directoryPath">path to a directory</param>
        /// <returns>list of files</returns>
        IEnumerable<FileInfo> GetNewFilesFromDirectory(DateTime createdLaterUtc, string directoryPath);
    }
}