
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private IDatabaseData _databaseData;
        public DatabaseController(IDatabaseData databaseData)
        {
            _databaseData = databaseData;
        }


        // GET api/Database/GetAllDatabases
        [Route("GetAllDatabases", Name = "GetAllDatabases")]
        [HttpGet]
        public Task<List<DatabaseModel>> GetAllDatabases()
        {
            return _databaseData.GetAllDatabases();
        }


        // GET api/Database/GetAllDatabasesByServerId/{serverId}
        [Route("GetAllDatabasesByServerId", Name = "GetAllDatabasesByServerId")]
        [HttpGet]
        public Task<List<DatabaseModel>> GetAllDatabasesByServerId(int serverId)
        {
            return _databaseData.GetAllDatabasesByServerId(serverId);
        }


        // GET api/Database/FindAllDatabases/{serverName}
        [Route("FindAllDatabases", Name = "FindAllDatabases")]
        [HttpGet]
        public Task<List<DatabaseModel>> FindAllDatabases(string serverName)
        {
            return _databaseData.FindAllDatabases(serverName);
        }
    }
}
