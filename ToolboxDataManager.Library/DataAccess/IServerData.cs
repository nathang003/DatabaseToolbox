using System.Collections.Generic;

using ToolboxDataManager.Library.Models;

namespace ToolboxDataManager.Library.DataAccess
{
    public interface IServerData
    {
        List<ServerModel> GetAllNonDevServers();
        List<ServerModel> GetAllServers();
    }
}