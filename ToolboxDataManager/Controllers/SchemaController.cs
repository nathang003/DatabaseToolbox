using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    public class SchemaController : ApiController
    {
        List<SchemaModel> schemas = new List<SchemaModel>();

        public SchemaController()
        {
            SchemaData data = new SchemaData();

            schemas = data.GetAllSchemas();
        }

        // GET api/Schema/GetAllSchemas
        [Route("api/Schema/GetAllSchemas", Name = "GetAllSchemas")]
        [HttpGet]
        public List<SchemaModel> GetAllSchemas()
        {
            return schemas;
        }

        // GET api/Schema/GetAllSchemasByDatabaseId
        [Route("api/Schema/GetAllSchemasByDatabaseId/{databaseId:int}", Name = "GetAllSchemasByDatabaseId")]
        [HttpGet]
        public List<SchemaModel> GetAllSchemasByDatabaseId(int databaseId)
        {
            return schemas.Where(x => x.DatabaseId == databaseId).ToList();
        }
    }
}