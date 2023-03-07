using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ForeignKeyController : ControllerBase
{
    private IForeignKeyData _foreignKeyData;
    public ForeignKeyController(IForeignKeyData foreignKeyData)
    {
        _foreignKeyData = foreignKeyData;
    }


    // GET api/ForeignKey/GetAllForeignKeys
    [Route("GetAllForeignKeys", Name = "GetAllForeignKeys")]
    [HttpGet]
    public async Task<List<ForeignKeyModel>> GetAllForeignKeys()
    {
        return await _foreignKeyData.GetAllForeignKeys();
    }
}
