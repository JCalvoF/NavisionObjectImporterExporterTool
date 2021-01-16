using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

using NavisionObjectImporterExporterTool.Models;

using NPoco;

namespace NavisionObjectImporterExporterTool.Data
{
    public static class DBFactory
    {      
        public static Database CreateDatabase(NavisionConnection navconnection)
        {
            DatabaseFactory DbFactory;

            try
            {
                string ConnectionString = string.Empty;

                ConnectionString += string.Format("Server = {0}",navconnection.Server);
                ConnectionString += string.Format(";Database = {0}", navconnection.Database);

                if (navconnection.ntauthentication)
                {
                    //Server = myServerAddress; Database = myDataBase; Trusted_Connection = True;
                    ConnectionString += string.Format(";Trusted_Connection = True");

                }
                else
                {
                    //Server = myServerAddress; Database = myDataBase; User Id = myUsername; Password = myPassword;
                    ConnectionString += string.Format("; User Id = {0}",navconnection.username);
                    ConnectionString += string.Format("; Password = {0}", navconnection.password);
                }
                            

                DbFactory = DatabaseFactory.Config(x =>
                {
                    x.UsingDatabase(() => new Database(ConnectionString,DatabaseType.SqlServer2012));
                });

                return DbFactory.GetDatabase();
            }
            catch (Exception excep)
            {
                throw new Exception("Error creating the sql database object" + navconnection.Server  + ". " + excep.Message);
            }
        }
    }
}
