using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    public class FieldController : ApiController
    {
        List<FieldModel> fields = new List<FieldModel>();
        FieldData data = new FieldData();

        public FieldController()
        {
            fields = data.GetAllFields();
        }

        // GET api/Field/GetAllFields
        [Route("api/Field/GetAllFields", Name = "GetAllFields")]
        [HttpGet]
        public List<FieldModel> GetAllFields()
        {
            return fields;
        }

        // GET api/Database/GetAllFieldsByTableId
        [Route("api/Field/GetAllFieldsByTableId/{tableId:int}", Name = "GetAllFieldsByTableId")]
        [HttpGet]
        public List<FieldModel> GetAllFieldsByTableId(int tableId)
        {
            return fields.Where(x => x.TableId == tableId).ToList();
        }
    }
}