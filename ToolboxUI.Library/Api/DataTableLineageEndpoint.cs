using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace ToolboxUI.Library.Api
{
    public class DataTableLineageEndpoint : IDataTableLineageEndpoint
    {
        private IAPIHelper _apiHelper;

        public DataTableLineageEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<DataTableLineageModel>> FindDataTableLineages()
        {
            Console.WriteLine("Entering DataTableLineageEndpoint.FindDataTableLineages()");

            using(HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/DataTableLineage/FindDataTableLineages"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DataTableLineageModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<DataTableLineageModel>> GetAllDataTableLineages()
        {
            Console.WriteLine("Entering DataTableLineageEndpoint.GetAllDataTableLineages()");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/DataTableLineage/GetAllDataTableLineages"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DataTableLineageModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<DataTableLineageModel>> GetDataTableLineageParents(int tableId)
        {
            Console.WriteLine("Entering DataTableLineageEndpoint.GetDataTableLineageParents(tableId: {0})", tableId.ToString());

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/DataTableLineage/GetDataTableLineageParents/{ tableId }"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DataTableLineageModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<DataTableLineageModel>> GetDataTableLineageChildren(int tableId)
        {
            Console.WriteLine("Entering DataTableLineageEndpoint.GetDataTableLineageChildren(tableId: {0})", tableId.ToString());

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/DataTableLineage/GetDataTableLineageChildren/{ tableId }"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DataTableLineageModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public async Task InsertDataTableLineage(DataTableLineageModel dataTableLineage)
		{
            Console.WriteLine($"Entering ToolboxUI.Library.Api.DataTableLineageEndpoint.InsertDataTableLineage({ dataTableLineage.ToString() })");

            string json = JsonConvert.SerializeObject(dataTableLineage);

            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string,string>("Id", dataTableLineage.Id.ToString()),
            //    new KeyValuePair<string,string>("ParentTableId", dataTableLineage.ParentTableId.ToString()),
            //    new KeyValuePair<string,string>("ChildTableId", dataTableLineage.ChildTableId.ToString()),
            //    new KeyValuePair<string,string>("LineageStartDate", dataTableLineage.LineageStartDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("LineageEndDate", dataTableLineage.LineageEndDate.Year < 2017 ? String.Empty : dataTableLineage.LineageEndDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("CreatedBy", dataTableLineage.CreatedBy.ToString()),
            //    new KeyValuePair<string,string>("CreatedDate", dataTableLineage.CreatedDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("UpdatedBy", dataTableLineage.UpdatedBy.ToString()),
            //    new KeyValuePair<string,string>("UpdatedDate", dataTableLineage.UpdatedDate.ToLongDateString())
            //});
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync($"/api/DataTableLineage/AddDataTableLineage", content))
			{
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
				}
			}
        }


        public async Task UpdateDataTableLineage(DataTableLineageModel dataTableLineage)
        {
            Console.WriteLine("Entering ToolboxUI.Library.Api.DataTableLineageEndpoint.UpdateDataTableLineage(parentTableId: {0}, childTableId: {1}", dataTableLineage.ParentTableId.ToString(), dataTableLineage.ChildTableId.ToString());

            string json = JsonConvert.SerializeObject(dataTableLineage);

            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string,string>("Id", dataTableLineage.Id.ToString()),
            //    new KeyValuePair<string,string>("ParentTableId", dataTableLineage.ParentTableId.ToString()),
            //    new KeyValuePair<string,string>("ChildTableId", dataTableLineage.ChildTableId.ToString()),
            //    new KeyValuePair<string,string>("LineageStartDate", dataTableLineage.LineageStartDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("LineageEndDate", dataTableLineage.LineageEndDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("CreatedBy", dataTableLineage.CreatedBy.ToString()),
            //    new KeyValuePair<string,string>("CreatedDate", dataTableLineage.CreatedDate.ToLongDateString()),
            //    new KeyValuePair<string,string>("UpdatedBy", dataTableLineage.UpdatedBy.ToString()),
            //    new KeyValuePair<string,string>("UpdatedDate", dataTableLineage.UpdatedDate.ToLongDateString())
            //});

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync($"/api/DataTableLineage/UpdateDataTableLineage", content))
            {

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }
    }
}
