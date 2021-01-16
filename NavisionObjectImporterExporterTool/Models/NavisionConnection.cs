using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisionObjectImporterExporterTool.Models
{
    public class NavisionConnection
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool ntauthentication { get; set; }                    

        public string FullName
        {
            get { return string.Format("{0} {1} {2}", Name, Server, Database); }
        }

    }
}
