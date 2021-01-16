using System;
using System.Collections.Generic;
using NavisionObjectImporterExporterTool.Models;

namespace NavisionObjectImporterExporterTool.Services
{
    public class Mock_ConnectionsService : IConnectionsService
    {
        public List<NavisionConnection> GetConectionsList()
        {
            List<NavisionConnection> returndata = new List<NavisionConnection>();

            returndata.Add(new NavisionConnection { Name = "NAVISION_ENV_DESARROLLO", Database = "NAVISION_ENV_DESARROLLO", Server = @"servidorDesarrollo" });
            returndata.Add(new NavisionConnection { Name = "NAVISION_ENV_PREPRODUCCION", Database = "NAVISION_ENV_PREPRODUCCION", Server = @"servidorPreproduccion" });
            returndata.Add(new NavisionConnection { Name = "NAVISION_ENV_PRODUCCION", Database = "NAVISION_ENV_PRODUCCION", Server = @"servidorProduccion" });

            return returndata;
        }
    }
}
