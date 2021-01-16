using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPoco;

namespace NavisionObjectImporterExporterTool.Data.Models
{

    [TableName("Object")]
    [PrimaryKey("Type,Company Name,ID")]
    public class NavDB_Object
    {
        //public Byte[] timestamp { get; set; }
        public Int32 Type { get; set; }

        [Column("Company Name")]
        public string CompanyName { get; set; }

        public Int32 ID { get; set; }

        public string Name { get; set; }

        public Int16 Modified { get; set; }

        public Int16 Compiled { get; set; }

        //public Int32 BLOBSize { get; set; }

        //public Int32 DBMTableNo_ { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        [Column("Version List")]
        public string VersionList { get; set; }

        public Int16 Locked { get; set; }

        [Column("Locked By")]
        public string LockedBy { get; set; }

    }
}
