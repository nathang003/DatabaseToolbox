

namespace ToolboxWebLibrary.Api;

public class ForeignKeyEndpoint : IForeignKeyEndpoint
{
    private IApiHelper _apiHelper;

    public ForeignKeyEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<ForeignKeyModel>> GetAllForeignKeys()
    {
        Console.WriteLine("Entering ForeignKeyEndpoint.GetAllForeignKeys()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/ForeignKey/GetAllForeignKeys"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ForeignKeyModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
