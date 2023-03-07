

namespace ToolboxWebLibrary.Api;

public class FieldEndpoint : IFieldEndpoint
{
    private IApiHelper _apiHelper;

    public FieldEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<FieldModel>> GetAllFields()
    {
        Console.WriteLine("Entering FieldEndpoint.GetAllFields()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Field/GetAllFields"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<FieldModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<FieldModel>> GetAllFieldsByTableId(int tableId)
    {
        Console.WriteLine("Entering FieldEndpoint.GetAllFieldsByTableId(int tableId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Field/GetAllFieldsByTableId/{tableId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<FieldModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
