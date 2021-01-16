using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Linq;
using System.IO;

using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Data;
using NavisionObjectImporterExporterTool.Data.Models;
using NavisionObjectImporterExporterTool.Enum;

namespace NavisionObjectImporterExporterTool.Services
{
    public class NavObjectService : INavObjectService
    {
        private INavisionObjectsRepository _repository;

        public NavObjectService(INavisionObjectsRepository DBrepository)
        {
            _repository = DBrepository;
        }

        public List<NavisionObjectType> GetTypeList()
        {
            //navision 2013 R2 objects

            //TableData = 0,
            //Table = 1,
            //Form = 2,
            //Report = 3,
            //Dataport = 4,
            //Codeunit = 5,
            //XMLport = 6,
            //MenuSuite = 7,
            //Page = 8,
            //Query = 9,
            //System = 10,
            //FieldNumber = 11


            List<NavisionObjectType>  returndata = new List<NavisionObjectType>();

            returndata.Add(new NavisionObjectType { ObjectType = 1, ObjectTypeName = "Table (Solo Backup)" });
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
            var objects = _repository.GetObjects(objecttype, connection);

            List<NavisionObject> navobjects = new List<NavisionObject>();

            foreach(NavDB_Object obj in objects)
            {
                navobjects.Add(
                    new NavisionObject()
                    {
                        ObjectId = obj.ID,
                        ObjectType = obj.Type,
                        Name = obj.Name,
                        ObjectTypeName =((ENUM_NavObjetType)obj.Type).ToString()
                    }); 
            }           

            return navobjects;
        }
    }
}
