using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/DatabaseObject")]
    public class DatabaseObjectController : ApiController
    {
        DatabaseObjectData data = new DatabaseObjectData();

        public DatabaseObjectController()
        {
            Console.WriteLine("Entering DatabaseObjectController.");
        }

        // GET api/DatabaseOject/GetAllDatabaseObjects
        [Route("GetAllDatabaseObjects", Name = "GetAllDatabaseObjects")]
        [HttpGet]
        public List<DatabaseObjectModel> GetAllDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectController.GetAllDatabaseObjects.");

            return data.GetAllDatabaseObjects();
        }

        // GET api/DatabaseOject/GetAllNonDevDatabaseObjects
        [Route("GetAllNonDevDatabaseObjects", Name = "GetAllNonDevDatabaseObjects")]
        [HttpGet]
        public List<DatabaseObjectModel> GetAllNonDevDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectController.GetAllNonDevDatabaseObjects.");

            return data.GetAllNonDevDatabaseObjects();
        }
    }
}