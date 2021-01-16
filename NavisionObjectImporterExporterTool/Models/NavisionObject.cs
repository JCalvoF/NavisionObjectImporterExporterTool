using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMvvmToolkit;

namespace NavisionObjectImporterExporterTool.Models
{
    public class NavisionObject
    {
        public int ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string ObjectTypeName { get; set; }
        public string Name { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1} {2}",ObjectType,ObjectTypeName,Name); }
        }

        public string IdName
        {
            get { return string.Format("{0}-{1} {2}", ObjectId, Name, ObjectTypeName); }
        }
    }
}
