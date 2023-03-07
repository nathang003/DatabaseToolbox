

namespace ToolboxWebLibrary.Api;

public class SchemaEndpoint : ISchemaEndpoint
{
    private IApiHelper _apiHelper;

    public SchemaEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<SchemaModel>> GetAllSchemas()
    {
        Console.WriteLine("Entering SchemaEndpoint.GetAllSchemas()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Schema/GetAllSchemas"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<SchemaModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId)
    {
        Console.WriteLine("Entering SchemaEndpoint.GetAllSchemasByDatabaseId(int databaseId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Schema/GetAllSchemasByDatabaseId/{databaseId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<SchemaModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
