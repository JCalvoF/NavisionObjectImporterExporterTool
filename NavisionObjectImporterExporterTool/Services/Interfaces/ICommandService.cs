using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using NavisionObjectImporterExporterTool.Services;
using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Enum;

namespace NavisionObjectImporterExporterTool.Services
{
    public interface ICommandService
    {
        List<string> GenerateCommands(
            NavisionConnection SourceConnection, 
            NavisionObjectType ObjectType, 
            string ObjectID, 
            List<NavisionConnection> DestinationConnections, 
            string BackupPath, 
            string WorkPath, 
            bool IncludeTimestamp, 
            bool MakeDestinationBackup, 
            bool Backup_FOB,
            bool Backup_TXT, 
            bool InsertPauseBeforeImport);
    }
}
