using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class TableEndpoint : ITableEndpoint
    {
        private IAPIHelper _apiHelper;

        public TableEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<TableModel>> GetAllTables()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Table/GetAllTables"))
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
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Table/GetAllTables/{ schemaId }"))
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

        public async Task<TableModel> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
		{

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Table/GetTableByNames/{ serverName }/{ databaseName }/{ schemaName }/{ tableName }"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<TableModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
