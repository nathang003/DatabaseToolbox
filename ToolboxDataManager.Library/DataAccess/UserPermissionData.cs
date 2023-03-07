using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class UserPermissionData
    {
        public List<UserPermissionModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { Id = Id };
            var output = sql.LoadData<UserPermissionModel, dynamic>("dbo.spUserPermissionsLookup", parameters, "ToolboxData");

            return output;
        }
    }
}
