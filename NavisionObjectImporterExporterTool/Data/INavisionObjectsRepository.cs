using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPoco;

using NavisionObjectImporterExporterTool.Data.Models;
using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Enum;

namespace NavisionObjectImporterExporterTool.Data
{
    public interface INavisionObjectsRepository
    {
        List<NavDB_Object> GetObjects(ENUM_NavObjetType objecttype, NavisionConnection connection);
    }
}
