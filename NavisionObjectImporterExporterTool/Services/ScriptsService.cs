using System;
using System.Collections.Generic;
using System.IO;

namespace NavisionObjectImporterExporterTool.Services
{
    public class ScriptsService : IScriptService
    {

        public void Save2File(string workingpath,string ObjectTypeName,string ObjectID, List<String> Commands,bool showfolder,bool execute )
        {
            try
            {

                string _workpath = workingpath;

                if (!Directory.Exists(_workpath))
                    Directory.CreateDirectory(_workpath);

                _workpath = _workpath.EndsWith(@"\") ? _workpath : string.Format(@"{0}\", _workpath);

                string filename = string.Format("{0}_{1}", ObjectTypeName, ObjectID);

                string filepath;

                filepath = string.Format("{0}{1}.bat", _workpath, filename);

                if (File.Exists(filepath))
                    File.Delete(filepath);

                System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);

                foreach (var i in Commands)
                {
                    file.WriteLine(i);
                }

                file.Close();

                if (showfolder)
                    System.Diagnostics.Process.Start("explorer.exe", _workpath);

                if (execute)
                    System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
