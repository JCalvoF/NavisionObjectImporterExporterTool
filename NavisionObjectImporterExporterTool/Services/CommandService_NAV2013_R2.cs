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
    public class CommandService_NAV2013_R2 : ICommandService
    {

        /*
         * finsql.exe 
                command=exportobjects, 
                file=<exportfile>, 
                [servername=<server>,] 
                [database=<database>,] 
                [logfile=<path and filename>,] 
                [filter=<filter>,] 
                [username=<username>,] 
                [password=<password>,] 
                [ntauthentication=<yes|no|1|0>]
        */

        public List<string> GenerateCommands(
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
            bool InsertPauseBeforeImport
            )
        {
            List<string> ExportCommand = new List<string>();

            //export filter
            string _filter = string.Format("filter={0}Type={1};ID={2}{0}", '"', ObjectType.ObjectTypeName, ObjectID);

            //backup path
            string _backuppath = BackupPath.EndsWith(@"\") ? BackupPath : string.Format(@"{0}\", BackupPath);

            //working path
            string _workpath = WorkPath;
            _workpath = _workpath.EndsWith(@"\") ? _workpath : string.Format(@"{0}\", _workpath);

            //include timestamp in filenames
            string timestamp = string.Empty;
            if (IncludeTimestamp)
                timestamp = String.Format("_{0:yyyyMMddHHmmssfff}", System.DateTime.Now);

            string cleanobjectid = ObjectID.Replace("|", "_").Replace("..", "_");

            //nombre del archivo sin extension ni ruta
            string objname;
            objname = string.Format("{0}_{1}_{2}_{3}{4}",
                    SourceConnection.Server.Replace(@"\", "_"),
                    SourceConnection.Database,
                    ObjectType.ObjectTypeName,
                    cleanobjectid,
                    timestamp);

            //FOB Full path completa
            string fobexportedobjectpath = string.Format("{0}{1}.{2}", _workpath, objname, "fob");

            string command = string.Empty;
            ExportCommand.Add(string.Format("REM {0} {1}", ObjectType.ObjectTypeName.ToUpper(), ObjectID.ToUpper()));
            ExportCommand.Add(Environment.NewLine);

            ExportCommand.Add(string.Format("REM Export ORIGEN -FOB-"));
            command = string.Empty;
            command += string.Format("finsql.exe command=exportobjects, ");
            command += string.Format("servername={0}, ", SourceConnection.Server);
            command += string.Format("database={0}, ", SourceConnection.Database);
            command += string.Format("ntauthentication=1, ");
            command += string.Format("file={0}{1}.{2}, ", _workpath, objname, "fob");
            command += string.Format("{0}, ", _filter);
            command += string.Format("logfile={0}LOG_{1}.txt", _workpath, objname);
            command += Environment.NewLine;
 
            ExportCommand.Add(command);
            ExportCommand.Add(string.Format("REM Resultado export origen"));
            ExportCommand.Add(string.Format("type {0}navcommandresult.txt", _workpath));
            ExportCommand.Add(Environment.NewLine);

            //backup de destino
            if (MakeDestinationBackup)
            {
                List<string> exporttype = new List<string>();
                if (Backup_TXT)
                    exporttype.Add("txt");
                if (Backup_FOB)
                    exporttype.Add("fob");

                foreach (var conn in DestinationConnections)
                {
                    command = string.Empty;
                    objname = string.Format("{0}_{1}_{2}_{3}{4}", conn.Server.Replace(@"\", "_"), conn.Database, ObjectType.ObjectTypeName, cleanobjectid, timestamp);

                    foreach (string ty in exporttype)
                    {
                        ExportCommand.Add(string.Format("REM Backup Destino {0} {1} {2}", conn.Server, conn.Database, ty.ToUpper()));
                        command = string.Empty;
                        command += string.Format("finsql.exe command=exportobjects, ");
                        command += string.Format("servername={0}, ", conn.Server);
                        command += string.Format("database={0}, ", conn.Database);
                        command += string.Format("ntauthentication=1, ");
                        command += string.Format("file={0}{1}.{2}, ", _backuppath, objname, ty);
                        command += string.Format("{0}, ", _filter);
                        command += string.Format("logfile={0}LOG_{1}.txt", _workpath, objname);
                        command += Environment.NewLine;
                        ExportCommand.Add(command);
                        ExportCommand.Add(string.Format("REM Resultado Backup"));
                        ExportCommand.Add(string.Format("type {0}navcommandresult.txt", _workpath));
                        ExportCommand.Add(Environment.NewLine);
                    }
                }

            }

            if (ObjectType.ObjectType != (int)ENUM_NavObjetType.Table)
            {

                //IMPORT EN DESTINO
                foreach (var conn in DestinationConnections)
                {
                    ExportCommand.Add(string.Format("REM IMPORT {0} {1}", conn.Server, conn.Database));

                    if (InsertPauseBeforeImport)
                    {
                        ExportCommand.Add(string.Format("REM Se importará el objeto de origen en {0} {1} {2}. Pulse cualquier tecla para continuar", conn.Server, conn.Database, fobexportedobjectpath));
                        ExportCommand.Add(string.Format("PAUSE"));
                    }

                    //finsql.exe command=importobjects, file=<importfile>, [servername=<server>,] [database=<database>,] [logfile=<path and filename>,] [importaction=<default|overwrite|skip|0|1|2>,] [username=<username>,] [password=<password>,] [ntauthentication=<yes|no|1|0>,] [synchronizeschemachanges=<yes|no|force>,] [navservername=<server name>,] [navserverinstance=<instance>,] [navservermanagementport=<port>,] [tenant=<tenant ID>]
                    //finsql.exe command=importobjects, file=C:\NewObjects.fob, servername=TestComputer01, database="Demo Database NAV (9-0)", ImportAction=overwrite

                    command = string.Empty;
                    objname = string.Format("{0}_{1}_{2}_{3}{4}", conn.Server.Replace(@"\", "_"), conn.Database, ObjectType.ObjectTypeName, cleanobjectid, timestamp);

                    //ExportCommand.Add(string.Format("REM IMPORTANDO EN {0} {1}", conn.Connection.Server, conn.Connection.Database));
                    command = string.Empty;
                    command += string.Format("finsql.exe command=importobjects, ");
                    command += string.Format("servername={0}, ", conn.Server);
                    command += string.Format("database={0}, ", conn.Database);
                    command += string.Format("ntauthentication=1, ");
                    command += string.Format("file={0},", fobexportedobjectpath);
                    //if ((SynchronizeSchemaChanges) && (SelectedObjectTypeList.ObjectType == (int)ENUM_NavObjetType.Table))
                    //    command += string.Format("synchronizeschemachanges=yes,");
                    command += string.Format("ImportAction=overwrite,");
                    command += string.Format("logfile={0}LOG_IMPORT_{1}.txt", _workpath, objname);
                    command += Environment.NewLine;
                    ExportCommand.Add(command);
                    ExportCommand.Add(string.Format("REM Resultado importacion"));
                    ExportCommand.Add(string.Format("type {0}LOG_IMPORT_{1}.txt", _workpath, objname));
                    ExportCommand.Add(Environment.NewLine);

                }

            }
            ExportCommand.Add(string.Format("REM Proceso finalizado"));
            ExportCommand.Add(string.Format("PAUSE"));

            return ExportCommand;
        }
    }
}
