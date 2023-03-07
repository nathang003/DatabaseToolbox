

namespace ToolboxWebLibrary.Api;

public class IndexEndpoint : IIndexEndpoint
{
    private IApiHelper _apiHelper;

    public IndexEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<IndexModel>> GetAllIndexes()
    {
        Console.WriteLine("Entering IndexEndpoint.GetAllIndexes()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Index/GetAllIndexes"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<IndexModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<IndexModel>> GetIndexesByTableId(int tableId)
    {
        Console.WriteLine("Entering IndexEndpoint.GetIndexesByTableId(int tableId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Index/GetIndexesByTableId/{tableId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<IndexModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
