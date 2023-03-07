

namespace ToolboxWebLibrary.Api;

public class DatabaseObjectEndpoint : IDatabaseObjectEndpoint
{
    private IApiHelper _apiHelper;

    public DatabaseObjectEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<DatabaseObjectModel>> GetAllDatabaseObjects()
    {
        Console.WriteLine("Entering DatabaseObjectEndpoint.GetAllDatabaseObjects()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/DatabaseObject/GetAllDatabaseObjects"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<DatabaseObjectModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<DatabaseObjectModel>> GetExactMatches(string searchValue)
    {
        Console.WriteLine("Entering DatabaseObjectEndpoint.GetExactMatches(string searchValue)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/DatabaseObject/GetExactMatches/{ searchValue }"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<DatabaseObjectModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<DatabaseObjectModel>> GetPartialMatches(string searchValue)
    {
        Console.WriteLine("Entering DatabaseObjectEndpoint.GetPartialMatches(string searchValue)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/DatabaseObject/GetPartialMatches/{ searchValue }"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<DatabaseObjectModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
