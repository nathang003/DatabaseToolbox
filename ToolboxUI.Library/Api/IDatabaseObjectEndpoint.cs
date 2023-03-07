using ToolboxUI.Library.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public interface IDatabaseObjectEndpoint
    {
        Task<List<DatabaseObjectModel>> GetAllDatabaseObjects();
        Task<List<DatabaseObjectModel>> GetAllNonDevDatabaseObjects();
    }
}