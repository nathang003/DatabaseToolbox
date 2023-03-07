using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class DatabaseEndpoint : IDatabaseEndpoint
    {
        private IAPIHelper _apiHelper;

        public DatabaseEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<DatabaseModel>> GetAllDatabases()
        {
            Console.WriteLine("Entering DatabaseEndpoint.GetAllDatabases()");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Database/GetAllDatabases"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DatabaseModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<DatabaseModel>> GetAllDatabasesByServerId(int serverId)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Database/GetAllDatabases/{ serverId }"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DatabaseModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<DatabaseModel>> FindAllDatabases(string serverName)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Database/FindAllDatabases/{ serverName }"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DatabaseModel>>();
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
