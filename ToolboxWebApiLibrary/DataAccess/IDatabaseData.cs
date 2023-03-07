
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IDatabaseData
    {
        Task<List<DatabaseModel>> FindAllDatabases(string serverName);
        Task<List<DatabaseModel>> GetAllDatabases();
        Task<List<DatabaseModel>> GetAllDatabasesByServerId(int serverId);
        Task<DatabaseDetailedModel> GetDatabaseDetailsByDatabaseId(int databaseId);
        Task DatabaseUpsert(List<DatabaseModel> databases, int ServerId);
    }
}