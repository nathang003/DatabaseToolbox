

namespace ToolboxWebLibrary.Api;

public class DataTableLineageEndpoint : IDataTableLineageEndpoint
{
    private IApiHelper _apiHelper;

    public DataTableLineageEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<DataTableLineageModel>> GetAllDataTableLineages()
    {
        Console.WriteLine("Entering DataTableLineageEndpoint.GetAllDataTableLineages()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/DataTableLineage/GetAllDataTableLineages"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<DataTableLineageModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
