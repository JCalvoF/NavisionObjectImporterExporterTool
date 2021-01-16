using System;
using System.Collections.Generic;
using NavisionObjectImporterExporterTool.Enum;
using NavisionObjectImporterExporterTool.Models;

namespace NavisionObjectImporterExporterTool.Services
{
    public class Mock_NavObjectService : INavObjectService
    {
        public List<NavisionObjectType> GetTypeList()
        {
            List<NavisionObjectType> returndata = new List<NavisionObjectType>();

            returndata.Add(new NavisionObjectType { ObjectType = 1, ObjectTypeName = "Table" });
            returndata.Add(new NavisionObjectType { ObjectType = 3, ObjectTypeName = "Report" });
            returndata.Add(new NavisionObjectType { ObjectType = 5, ObjectTypeName = "Codeunit" });
            returndata.Add(new NavisionObjectType { ObjectType = 6, ObjectTypeName = "XMLport" });
            returndata.Add(new NavisionObjectType { ObjectType = 7, ObjectTypeName = "MenuSuite" });
            returndata.Add(new NavisionObjectType { ObjectType = 8, ObjectTypeName = "Page" });
            returndata.Add(new NavisionObjectType { ObjectType = 9, ObjectTypeName = "Query" });

            return returndata;
        }

        public List<NavisionObject> GetObjectList(ENUM_NavObjetType objecttype, NavisionConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
