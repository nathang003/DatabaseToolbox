using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class FieldEndpoint : IFieldEndpoint
    {
        private IAPIHelper _apiHelper;

        public FieldEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<FieldModel>> GetAllFields()
        {
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
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Field/GetAllFields/{ tableId }"))
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
}
