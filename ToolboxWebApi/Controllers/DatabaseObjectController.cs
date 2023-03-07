using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseObjectController : ControllerBase
    {
        private IDatabaseObjectData _databaseObjectData;
        public DatabaseObjectController(IDatabaseObjectData databaseObjectData)
        {
            _databaseObjectData = databaseObjectData;
        }


        // GET api/DatabaseObject/GetAllDatabaseObjects
        [Route("GetAllDatabaseObjects", Name = "GetAllDatabaseObjects")]
        [HttpGet]
        public async Task<List<DatabaseObjectModel>> GetAllDatabaseObjects()
        {
            return await _databaseObjectData.GetAllDatabaseObjects();
        }


        // GET api/DatabaseObject/GetExactMatches
        [Route("GetExactMatches", Name = "GetExactMatches")]
        [HttpGet]
        public async Task<List<DatabaseObjectModel>> GetExactMatches(string searchValue)
        {
            return await _databaseObjectData.GetExactMatches(searchValue);
        }


        // GET api/DatabaseObject/GetExactMatches
        [Route("GetPartialMatches", Name = "GetPartialMatches")]
        [HttpGet]
        public async Task<List<DatabaseObjectModel>> GetPartialMatches(string searchValue)
        {
            return await _databaseObjectData.GetPartialMatches(searchValue);
        }
    }
}
