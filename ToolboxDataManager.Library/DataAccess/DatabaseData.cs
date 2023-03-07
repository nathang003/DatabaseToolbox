using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class DatabaseData
    {
        public List<DatabaseModel> GetAllDatabases()
        {

            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseModel, dynamic>("dbo.spDatabases_GetAll", new { }, "ToolboxData");

            return output;

        }

        public List<DatabaseModel> GetAllDatabasesByServerId(int serverId)
        {

            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseModel, dynamic>("dbo.spDatabases_GetByServerId", new { ServerId = serverId }, "ToolboxData");

            return output;

        }

        public List<DatabaseModel> FindAllDatabases(string serverName)
        {

            string connectionString;
            List<DatabaseModel> databases = new List<DatabaseModel>();

            switch(serverName)
            {

                case "mncportalprod-sql.database.windows.net":
					connectionString = @"Server=" + serverName + "; Authentication=Active Directory Password; User Id=svcASQLapi@mynexuscare.onmicrosoft.com; Password=myNEXUS2021";
					break;
                case "AZE1PSQLME01":
                case "AZE1SSQLME02":
                default:
                    connectionString = @"Data Source=" + serverName + "; Integrated Security=true; Trusted_Connection=Yes";
                    break;

            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using(SqlCommand command = new SqlCommand())
                {

                    command.CommandText = "Select distinct [name] database_name from master.sys.databases where [name] not in ('master','tempdb','msdb','SSISDB','model','TruncateTest1')";
                    command.Connection = connection;
                    
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {

                            while(reader.Read())
                            {

                                DatabaseModel database = new DatabaseModel();
                                database.DatabaseName = reader.GetString(reader.GetOrdinal("database_name"));
                                Console.WriteLine(database.DatabaseName);
                                databases.Add(database);

                            }

                        }

                    }

                }

            }

            return databases;

        }

    }

}
