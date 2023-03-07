using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class FieldData
    {
        public List<FieldModel> GetAllFields()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<FieldModel, dynamic>("dbo.spFields_GetAll", new { }, "ToolboxData");

            return output;
        }
        public List<FieldModel> GetAllFieldsByTableId(int tableId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<FieldModel, dynamic>("dbo.spFields_GetByTableId @TableId", new { TableId = tableId }, "ToolboxData");

            return output;
        }
    }
}
