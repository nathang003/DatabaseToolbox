
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DataTableLineageController : ControllerBase
{
    private IDataTableLineageData _dataTableLineageData;
    public DataTableLineageController(IDataTableLineageData dataTableLineageData)
    {
        _dataTableLineageData = dataTableLineageData;
    }


    // GET api/DataTableLineage/GetAllDataTableLineages
    [Route("GetAllDataTableLineages", Name = "GetAllDataTableLineages")]
    [HttpGet]
    public async Task<List<DataTableLineageModel>> GetAllDataTableLineages()
    {
        return await _dataTableLineageData.GetAllDataTableLineages();
    }
}
