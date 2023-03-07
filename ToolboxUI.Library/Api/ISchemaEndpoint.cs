using ToolboxUI.Library.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public interface ISchemaEndpoint
    {
        Task<List<SchemaModel>> GetAllSchemas();
        Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId);
    }
}