using System.Collections.Generic;
using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Enum;

namespace NavisionObjectImporterExporterTool.Services
{
    public interface INavObjectService
    {
        List<NavisionObjectType> GetTypeList();

        List<NavisionObject> GetObjectList(ENUM_NavObjetType objecttype, NavisionConnection connection);        
    }
}
