using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class ServerEndpoint : IServerEndpoint
    {
        private IAPIHelper _apiHelper;

        public ServerEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<ServerModel>> GetAllServers()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Server/GetAllServers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ServerModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<ServerModel>> GetAllNonDevServers()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Server/GetAllNonDevServers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ServerModel>>();
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
