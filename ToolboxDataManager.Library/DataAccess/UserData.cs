using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{

    public class UserData
    {

        public List<UserModel> GetUserById(string Id)
        {

            Console.WriteLine("Entering ToolboxDataManager.Libary.DataAccess.UserData.GetUserById");

            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { Id = Id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "ToolboxData");

            return output;

        }

    }

}
