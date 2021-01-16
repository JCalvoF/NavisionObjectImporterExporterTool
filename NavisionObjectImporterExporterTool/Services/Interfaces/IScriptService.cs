using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisionObjectImporterExporterTool.Services
{
    public interface IScriptService
    {
        void Save2File(string workingpath, string ObjectTypeName, string ObjectID, List<string> Commands, bool showfolder, bool execute);
    }
}
