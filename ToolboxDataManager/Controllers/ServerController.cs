using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    //[Authorize]
    public class ServerController : ApiController
    {
        List<ServerModel> servers = new List<ServerModel>();
        ServerData data = new ServerData();

        public ServerController()
        {

        }

        // GET api/Server/GetAllServers
        [Route("api/Server/GetAllServers", Name = "GetAllServers")]
        [HttpGet]
        public List<ServerModel> GetAllServers()
        {
            return data.GetAllServers();
        }

        // GET api/Server/GetAllNonDevServers
        [Route("api/Server/GetAllNonDevServers", Name = "GetAllNonDevServers")]
        [HttpGet]
        public List<ServerModel> GetAllNonDevServers()
        {
            return data.GetAllNonDevServers();
        }
    }
}