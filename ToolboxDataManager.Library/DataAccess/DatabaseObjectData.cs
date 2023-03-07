using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class DatabaseObjectData
    {
        public List<DatabaseObjectModel> GetAllDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectData.GetAllDatabaseObjects.");

            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetAll", new { }, "ToolboxData");

            return output;
        }

        public List<DatabaseObjectModel> GetAllNonDevDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectData.GetAllNonDevDatabaseObjects.");

            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetAllNonDev", new { }, "ToolboxData");

            return output;
        }
    }
}
