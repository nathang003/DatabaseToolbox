
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SchemaController : ControllerBase
{
    private ISchemaData _schemaData;
    public SchemaController(ISchemaData schemaData)
    {
        _schemaData = schemaData;
    }
    

    // GET api/Schema/GetAllSchemas
    [Route("GetAllSchemas", Name = "GetAllSchemas")]
    [HttpGet]
    public Task<List<SchemaModel>> GetAllSchemas()
    {
        return _schemaData.GetAllSchemas();
    }


    // GET api/Schema/GetAllSchemasByServerId/{serverId}
    [Route("GetAllSchemasByServerId", Name = "GetAllSchemasByDatabaseId")]
    [HttpGet]
    public Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId)
    {
        return _schemaData.GetAllSchemasByDatabaseId(databaseId);
    }
}
