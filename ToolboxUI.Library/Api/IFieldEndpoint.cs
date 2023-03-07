using System.Collections.Generic;
using System.Threading.Tasks;

using ToolboxUI.Library.Models;

namespace ToolboxUI.Library.Api
{
    public interface IFieldEndpoint
    {
        Task<List<FieldModel>> GetAllFields();
        Task<List<FieldModel>> GetAllFieldsByTableId(int tableId);
    }
}