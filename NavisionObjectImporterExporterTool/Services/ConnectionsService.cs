using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;


using NavisionObjectImporterExporterTool.Models;

namespace NavisionObjectImporterExporterTool.Services
{
    public class ConnectionsService : IConnectionsService
    {
        public List<NavisionConnection> GetConectionsList()
        {
            List<NavisionConnection> developerConnectionList = new List<NavisionConnection>();

            string xml = NavisionObjectImporterExporterTool.Properties.Settings.Default.ConnectionFile;

#if DEBUG
            xml = string.Format(@"{0}\Connections.xml",
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    )
                );
#endif

            if (!File.Exists(xml))
            {
                throw new Exception(string.Format("No existe el fichero de conexiones {0}", xml));
            }

            foreach (XElement xelement in XElement.Load(xml).Elements((XName)"connection").Select<XElement, XElement>((Func<XElement, XElement>)(conn => conn)))
            {
                NavisionConnection developerConnection = new NavisionConnection();
                developerConnection.Name = xelement.Element((XName)"name").Value;
                developerConnection.Server = xelement.Element((XName)"server").Value;
                developerConnection.Database = xelement.Element((XName)"database").Value;
                if (xelement.Element((XName)"ntauthentication").Value.ToLower() == "true")
                {
                    developerConnection.ntauthentication = true;
                }
                else
                {
                    developerConnection.ntauthentication = false;
                    developerConnection.username = xelement.Element((XName)"username").Value;
                    developerConnection.password = xelement.Element((XName)"password").Value;
                }
                developerConnectionList.Add(developerConnection);
            }
            return developerConnectionList;
        }
    }
}
