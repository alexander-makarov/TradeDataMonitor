﻿using System;
using System.Collections.Generic;
using System.IO;

namespace TradeDataMonitoring
{
    public interface IFileSystemManager
    {
        IEnumerable<FileInfo> GetNewFilesFromDirectory(DateTime createdLater, string directoryPath);
    }
}