using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class SchemaEndpoint : ISchemaEndpoint
    {
        private IAPIHelper _apiHelper;

        public SchemaEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<SchemaModel>> GetAllSchemas()
        {
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
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Schema/GetAllSchemas/{ databaseId }"))
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
}
