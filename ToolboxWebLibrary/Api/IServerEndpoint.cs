
namespace ToolboxWebLibrary.Api
{
    public interface IServerEndpoint
    {
        Task<List<ServerModel>> GetAllNonDevServers();
        Task<List<ServerModel>> GetAllServers();
    }
}