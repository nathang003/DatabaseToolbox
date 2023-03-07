

namespace ToolboxWebLibrary.Api;

public class TableEndpoint : ITableEndpoint
{
    private IApiHelper _apiHelper;

    public TableEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<TableModel>> GetAllTables()
    {
        Console.WriteLine("Entering TableEndpoint.GetAllTables()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Table/GetAllTables"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<TableModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId)
    {
        Console.WriteLine("Entering TableEndpoint.GetAllTablesBySchemaId(int schemaId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Table/GetAllTablesBySchemaId/{schemaId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<TableModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<TableModel>> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
    {
        Console.WriteLine("Entering TableEndpoint.GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)");

        string[] parameters = new string[] { serverName, databaseName, schemaName, tableName };

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Table/GetTableByNames/{parameters}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<TableModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
