using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/DataTableLineage")]
    public class DataTableLineageController : ApiController
    {

        DataTableLineageData data = new DataTableLineageData();


        // GET api/DataTableLineage/FindDataTableLineages
        [Route("FindDataTableLineages", Name = "FindDataTableLineages")]
        [HttpGet]
        public List<DataTableLineageModel> FindDataTableLineages()
        {
            Console.WriteLine("Entering DataTableLineageController.FindDataTableLineages.");

            return data.FindDataTableLineages();
        }


        // GET api/DataTableLineage/GetAllDataTableLineages
        [Route("GetAllDataTableLineages", Name = "GetAllDataTableLineages")]
        [HttpGet]
        public List<DataTableLineageModel> GetAllDataTableLineages()
        {
            Console.WriteLine("Entering DataTableLineageController.GetAllDataTableLineages.");

            return data.GetAllDataTableLineages();
        }


        // GET api/DataTableLineage/GetAllDataTableLineageParentsByTableId
        [Route("GetAllDataTableLineageParentsByTableId", Name = "GetAllDataTableLineageParentsByTableId")]
        [HttpGet]
        public List<DatabaseModel> GetAllDataTableLineageParentsByTableId(int tableId)
        {
            Console.WriteLine("Entering DataTableLineageController.GetAllDataTableLineageParentsByTableId.");

            return data.GetAllDataTableLineageParentsByTableId(tableId);
        }


        // GET api/DataTableLineage/GetAllDataTableLineageChildrenByTableId
        [Route("GetAllDataTableLineageChildrenByTableId", Name = "GetAllDataTableLineageChildrenByTableId")]
        [HttpGet]
        public List<DatabaseModel> GetAllDataTableLineageChildrenByTableId(int tableId)
        {
            Console.WriteLine("Entering DataTableLineageController.GetAllDataTableLineageChildrenByTableId.");

            return data.GetAllDataTableLineageChildrenByTableId(tableId);
        }


        // POST api/DataTableLineage/AddDataTableLineage
        [Route("AddDataTableLineage", Name = "AddDataTableLineage")]
        [HttpPost]
        public void AddDataTableLineage(DataTableLineageModel dataTableLineageModel)
        {
            Console.WriteLine("Entering DataTableLineageController.UpdateDataTableLineage.");

            data.AddDataTableLineage(dataTableLineageModel);
        }


        // POST api/DataTableLineage/UpdateDataTableLineage
        [Route("UpdateDataTableLineage", Name = "UpdateDataTableLineage")]
        [HttpPost]
        public void UpdateDataTableLineage(DataTableLineageModel dataTableLineageModel)
        {
            Console.WriteLine("Entering DataTableLineageController.UpdateDataTableLineage.");

            data.UpdateDataTableLineage(dataTableLineageModel);
		}
    }
}