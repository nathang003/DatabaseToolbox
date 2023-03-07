using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Table")]
    public class TableController : ApiController
    {
        List<TableModel> tables = new List<TableModel>();

        public TableController()
        {
        }

        // GET api/Table/GetAllTables
        [Route("GetAllTables", Name = "GetAllTables")]
        [HttpGet]
        public List<TableModel> GetAllTables()
        {
            TableData data = new TableData();

            tables = data.GetAllTables();
            return tables;
        }

        // GET api/Table/GetAllTablesBySchemaId
        [Route("GetAllTablesBySchemaId/{schemaId:int}", Name = "GetAllTablesByShemasId")]
        [HttpGet]
        public List<TableModel> GetBySchemaId(int schemaId)
        {
            TableData data = new TableData();
            tables = data.GetAllTablesBySchemaId(schemaId);
            return tables;
        }

        // GET api/Table/GetTableByNames
        [Route("GetTableByNames/{serverName}/{databaseName}/{schemaName}/{tableName}", Name = "GetTableByNames")]
        [HttpGet]
        public TableModel GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
        {
            TableData data = new TableData();
            TableModel table = data.GetTableByNames(serverName, databaseName, schemaName, tableName);

            return table;
        }
    }
}