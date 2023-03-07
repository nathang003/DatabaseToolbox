using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class ServerData : IServerData
    {
        public List<ServerModel> GetAllServers()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ServerModel, dynamic>("dbo.spServers_GetAll", new { }, "ToolboxData");

            return output;
        }

        public List<ServerModel> GetAllNonDevServers()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ServerModel, dynamic>("dbo.spServers_GetAllNonDev", new { }, "ToolboxData");

            return output;
        }
    }
}
