using System.Collections.Generic;
using System.Threading.Tasks;

using ToolboxUI.Library.Models;

namespace ToolboxUI.Library.Api
{
    public interface IServerEndpoint
    {
        Task<List<ServerModel>> GetAllServers();
        Task<List<ServerModel>> GetAllNonDevServers();
    }
}