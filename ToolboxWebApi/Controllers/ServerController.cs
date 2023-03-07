
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ServerController : ControllerBase
{
    private IServerData _serverData;
    public ServerController(IServerData serverData)
    {
        _serverData = serverData;
    }


    // GET api/Server/GetAllServers
    [Route("GetAllServers", Name = "GetAllServers")]
    [HttpGet]
    public async Task<List<ServerModel>> GetAllServers()
    {
        return await _serverData.GetAllServers();
    }


    // Get api/Server/GetAllNonDevServers
    [Route("GetAllNonDevServers", Name = "GetAllNonDevServers")]
    [HttpGet]
    public async Task<List<ServerModel>> GetAllNonDevServers()
    {
        return await _serverData.GetAllNonDevServers();
    }
}
