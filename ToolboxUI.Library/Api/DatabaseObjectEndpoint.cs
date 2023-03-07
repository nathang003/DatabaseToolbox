using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class DatabaseObjectEndpoint : IDatabaseObjectEndpoint
    {
        private IAPIHelper _apiHelper;

        public DatabaseObjectEndpoint(IAPIHelper apiHelper)
        {
            Console.WriteLine("Entering DatabaseObjectEndpoint.");

            _apiHelper = apiHelper;
        }

        public async Task<List<DatabaseObjectModel>> GetAllDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectEndpoint.GetAllDatabaseObjects().");

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

        public async Task<List<DatabaseObjectModel>> GetAllNonDevDatabaseObjects()
        {
            Console.WriteLine("Entering DatabaseObjectEndpoint.GetAllNonDevDatabaseObjects().");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/DatabaseObject/GetAllNonDevDatabaseObjects"))
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
}
