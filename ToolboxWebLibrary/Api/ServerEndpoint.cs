
namespace ToolboxWebLibrary.Api;

public class ServerEndpoint : IServerEndpoint
{
    private IApiHelper _apiHelper;

    public ServerEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<ServerModel>> GetAllServers()
    {
        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Server/GetAllServers"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ServerModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ServerModel>> GetAllNonDevServers()
    {
        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Server/GetAllNonDevServers"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ServerModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
