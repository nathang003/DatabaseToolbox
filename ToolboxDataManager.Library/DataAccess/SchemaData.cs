using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class SchemaData
    {
        public List<SchemaModel> GetAllSchemas()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<SchemaModel, dynamic>("dbo.spSchemas_GetAll", new { }, "ToolboxData");

            return output;
        }
        
        public List<SchemaModel> GetAllSchemasByDatabaseId(int databaseId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<SchemaModel, dynamic>("dbo.spSchemas_GetByDatabaseId", databaseId, "ToolboxData");
            

            return output;
        }
    }
}
