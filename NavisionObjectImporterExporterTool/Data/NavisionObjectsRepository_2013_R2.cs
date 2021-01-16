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
    public class NavisionObjectsRepository_2013_R2 : INavisionObjectsRepository
    {
        public List<NavDB_Object> GetObjects(ENUM_NavObjetType objecttype,NavisionConnection connection)
        {
            List<NavDB_Object> NavobjectList;

            using (IDatabase db = DBFactory.CreateDatabase(connection))
            {
                string sql = string.Format("select * from [dbo].[Object] where [Type] = {0}", (int)objecttype);

                NavobjectList = db.Fetch<NavDB_Object>(sql).ToList();

                return NavobjectList;
            }       
           
        }

    }
}
