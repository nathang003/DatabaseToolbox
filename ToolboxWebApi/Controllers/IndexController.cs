using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private IIndexData _indexData;
        public IndexController(IIndexData constraintData)
        {
            _indexData = constraintData;
        }


        // GET api/Index/GetAllIndexes
        [Route("GetAllIndexes", Name = "GetAllIndexes")]
        [HttpGet]
        public async Task<List<IndexModel>> GetAllIndexes()
        {
            return await _indexData.GetAllIndexes();
        }


        // GET api/Index/GetAllIndexes
        [Route("GetIndexesByTableId", Name = "GetIndexesByTableId")]
        [HttpGet]
        public async Task<List<IndexModel>> GetIndexesByTableId(int tableId)
        {
            return await _indexData.GetIndexesByTableId(tableId);
        }
    }
}
