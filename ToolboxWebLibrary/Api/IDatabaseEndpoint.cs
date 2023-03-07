
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolboxWebLibrary.Api
{
    public interface IDatabaseEndpoint
    {
        Task<List<DatabaseModel>> GetAllDatabases();

        Task<List<DatabaseModel>> GetAllDatabasesByServerId(int serverId);

        Task<List<DatabaseModel>> FindAllDatabases(string serverName);
    }
}