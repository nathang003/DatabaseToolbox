
using System.Collections.Generic;

namespace ToolboxWebApiLibrary.DataAccess;

public class ServerData : IServerData
{
    ISqlDataAccess _sql;

    public ServerData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<ServerModel>> GetAllServers()
    {
        var output = await _sql.LoadData<ServerModel, dynamic>("dbo.spServers_GetAll", new { }, "ToolboxData");

        return output;
    }

    public async Task<List<ServerModel>> GetAllNonDevServers()
    {
        var output = await _sql.LoadData<ServerModel, dynamic>("dbo.spServers_GetAllNonDev", new { }, "ToolboxData");

        return output;
    }

    public async Task<ServerModel> GetByServerId(int serverId)
    {
        var parameters = new { serverId = serverId };
        var output = await _sql.LoadData<ServerModel, dynamic>("dbo.spServers_GetByServerId", parameters, "ToolboxData");

        return output.FirstOrDefault();
    }
}
