using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NavisionObjectImporterExporterTool.Models;

namespace NavisionObjectImporterExporterTool.Services
{
    public interface IConnectionsService
    {
        List<NavisionConnection> GetConectionsList();
    }
}
