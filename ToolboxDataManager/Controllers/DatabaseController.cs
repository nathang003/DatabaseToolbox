using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Database")]
    public class DatabaseController : ApiController
    {
        List<DatabaseModel> databases = new List<DatabaseModel>();
        List<DatabaseModel> externalDatabases = new List<DatabaseModel>();
        DatabaseData data = new DatabaseData();

        public DatabaseController()
        {
            databases = data.GetAllDatabases();
        }

        // GET api/Database/GetAllDatabases
        [Route("GetAllDatabases", Name = "GetAllDatabases")]
        [HttpGet]
        public List<DatabaseModel> GetAllDatabases()
        {
            return databases;
        }

        // GET api/Database/GetAllDatabasesByServerId
        [Route("GetAllDatabasesByServerId/{serverId:int}", Name = "GetAllDatabasesByServerId")]
        [HttpGet]
        public List<DatabaseModel> GetAllDatabasesByServerId(int serverId)
        {
            return databases.Where(x => x.ServerId == serverId).ToList();
        }

        // GET api/Database/FindAllDatabases
        [Route("FindAllDatabases/{serverName}", Name = "FindAllDatabases")]
        [HttpGet]
        public List<DatabaseModel> FindAllDatabases(string serverName)
        {
            return data.FindAllDatabases(serverName);
        }
    }
}