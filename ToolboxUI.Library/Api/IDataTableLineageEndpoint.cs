using ToolboxUI.Library.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public interface IDataTableLineageEndpoint
    {
        Task<List<DataTableLineageModel>> FindDataTableLineages();
        Task<List<DataTableLineageModel>> GetAllDataTableLineages();
        Task<List<DataTableLineageModel>> GetDataTableLineageChildren(int tableId);
        Task<List<DataTableLineageModel>> GetDataTableLineageParents(int tableId);
        Task InsertDataTableLineage(DataTableLineageModel dataTableLineage);
        Task UpdateDataTableLineage(DataTableLineageModel dataTableLineage);
    }
}