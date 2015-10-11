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
        /// 
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directoryPath"/> is null. </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="directoryPath"/> contains invalid characters such as ", &lt;, &gt;, or |. </exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. 
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. 
        /// The specified path, file name, or both are too long.</exception>
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