using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FieldController : ControllerBase
{
    private IFieldData _fieldData;
    public FieldController(IFieldData fieldData)
    {
        _fieldData = fieldData;
    }


    // GET api/Field/GetAllFields
    [Route("GetAllFields", Name = "GetAllFields")]
    [HttpGet]
    public async Task<List<FieldModel>> GetAllFields()
    {
        return await _fieldData.GetAllFields();
    }


    // GET api/Field/GetAllFieldsByTableId/{ tableId }
    [Route("GetAllFieldsByTableId", Name = "GetAllFieldsByTableId")]
    [HttpGet]
    public async Task<List<FieldModel>> GetAllFieldsByTableId(int tableId)
    {
        return await _fieldData.GetAllFieldsByTableId(tableId);
    }
}
