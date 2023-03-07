
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TableController : ControllerBase
{
    private ITableData _tableData;
    public TableController(ITableData tableData)
    {
        _tableData = tableData;
    }


    // GET api/Table/GetAllTables
    [Route("GetAllTables", Name = "GetAllTables")]
    [HttpGet]
    public async Task<List<TableModel>> GetAllTables()
    {
        return await _tableData.GetAllTables();
    }


    // GET api/Table/GetAllTablesBySchemaId/{ schemaId }
    [Route("GetAllTablesBySchemaId", Name = "GetAllTablesBySchemaId")]
    [HttpGet]
    public async Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId)
    {
        return await _tableData.GetAllTablesBySchemaId(schemaId);
    }


    // GET api/Table/GetTableByNames/{ serverName, databaseName, schemaName, tableName }
    [Route("GetTableByNames", Name = "GetTableByNames")]
    [HttpGet]
    public async Task<List<TableModel>> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
    {
        return await _tableData.GetTableByNames(serverName, databaseName, schemaName, tableName);
    }
}
