using System.Collections.Generic;
using System.Threading.Tasks;

using ToolboxUI.Library.Models;

namespace ToolboxUI.Library.Api
{
    public interface ITableEndpoint
    {
        Task<List<TableModel>> GetAllTables();
        Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId);
        Task<TableModel> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName);
    }
}