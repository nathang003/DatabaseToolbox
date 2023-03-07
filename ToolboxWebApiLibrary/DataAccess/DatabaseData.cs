
using Microsoft.Data.SqlClient;
using System.Reflection;
using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess
{
    public class DatabaseData : IDatabaseData
    {
        private ISqlDataAccess _sqlDataAccess;
        private IConverterHelper _converterHelper;

        public DatabaseData(ISqlDataAccess sqlDataAccess, IConverterHelper converterHelper)
        {
            _sqlDataAccess = sqlDataAccess;
            _converterHelper = converterHelper;
        }

        public async Task<List<DatabaseModel>> GetAllDatabases()
        {
            var output = await _sqlDataAccess.LoadData<DatabaseModel, dynamic>("dbo.spDatabases_GetAll", new { }, "ToolboxData");

            return output;

        }

        public async Task<List<DatabaseModel>> GetAllDatabasesByServerId(int serverId)
        {
            var output = await _sqlDataAccess.LoadData<DatabaseModel, dynamic>("dbo.spDatabases_GetByServerId", new { ServerId = serverId }, "ToolboxData");

            return output;

        }


        public async Task<DatabaseDetailedModel> GetDatabaseDetailsByDatabaseId(int DatabaseId)
        {
            var output = await _sqlDataAccess.LoadData<DatabaseDetailedModel, dynamic>("dbo.spDatabases_GetDatabaseDetailsByDatabaseId", new { databaseId = DatabaseId }, "ToolboxData");

            return output.FirstOrDefault();

        }



        public Task<List<DatabaseModel>> FindAllDatabases(string serverName)
        {
            Console.WriteLine("Entering DatabaseData.FindAllDatabases(string serverName)");

            string connectionString = _sqlDataAccess.GetConnectionString(serverName);
            List<DatabaseModel> databases = new List<DatabaseModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlScript = @"Select distinct [name] database_name 
                    from sys.databases 
                    where [name] not in ('master','tempdb','msdb','SSISDB','model','TruncateTest1')";

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {

                            while (reader.Read())
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
            return Task.FromResult(databases);

        }



        public async Task DatabaseUpsert(List<DatabaseModel> databases, int ServerId)
        {
            Console.WriteLine("Entering DatabaseData.DatabaseUpsert(List<DatabaseModel> databases)");

            try
            {
                var parameters = new { databases = _converterHelper.ConvertModelToDataTable<DatabaseModel>(databases), serverId = ServerId };
                await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spDatabases_Upsert", parameters, "ToolboxData");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception encountered in DatabaseUpsert: {ex.Message}");

            }
        }

    }

}
