
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IServerData
    {
        Task<List<ServerModel>> GetAllNonDevServers();
        Task<List<ServerModel>> GetAllServers();
        Task<ServerModel> GetByServerId(int serverId);
    }
}