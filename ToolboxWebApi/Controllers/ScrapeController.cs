
using Microsoft.AspNetCore.Mvc;
using ToolboxWebAppLibrary.Models;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScrapeController : ControllerBase
{
    private IScrapeData _scrapeData;
    public ScrapeController(IScrapeData scrapeData)
    {
        _scrapeData = scrapeData;
    }


    // GET api/Scrape/GetAllScrapes
    [Route("GetAllScrapes", Name = "GetAllScrapes")]
    [HttpGet]
    public async Task<List<ScrapeModel>> GetAllScrapes()
    {
        return await _scrapeData.GetAllScrapes();
    }


    // GET api/Scrape/GetAllUnassignedScrapes
    [Route("GetAllUnassignedScrapes", Name = "GetAllUnassignedScrapes")]
    [HttpGet]
    public async Task<List<ScrapeDetailedModel>> GetAllUnassignedScrapes()
    {
        return await _scrapeData.GetAllUnassignedScrapes();
    }


    // GET api/Scrape/GetAllScrapesDetailed
    [Route("GetAllScrapesDetailed", Name = "GetAllScrapesDetailed")]
    [HttpGet]
    public async Task<List<ScrapeDetailedModel>> GetAllScrapesDetailed()
    {
        return await _scrapeData.GetAllScrapesDetailed();
    }


    // GET api/Scrape/GetSuggestedScrapes
    [Route("GetSuggestedScrapes", Name = "GetSuggestedScrapes")]
    [HttpGet]
    public async Task<List<ScrapeSuggestionModel>> GetSuggestedScrapes()
    {
        return await _scrapeData.GetSuggestedScrapes();
    }


    // GET api/Scrape/GetTopSuggestedScrapes
    [Route("GetTopSuggestedScrapes", Name = "GetTopSuggestedScrapes")]
    [HttpGet]
    public async Task<List<ScrapeSuggestionModel>> GetTopSuggestedScrapes()
    {
        return await _scrapeData.GetTopSuggestedScrapes();
    }


    // GET api/Scrape/GetSuggestionCount
    [Route("GetSuggestionCount", Name = "GetSuggestionCount")]
    [HttpGet]
    public async Task<int> GetSuggestionCount()
    {
        return await _scrapeData.GetSuggestionCount();
    }


    // GET api/Scrape/GetScrapeById/{ scrapeId }
    [Route("GetScrapeById", Name = "GetScrapeById")]
    [HttpGet]
    public async Task<ScrapeModel> GetScrapeById(Guid scrapeId)
    {
        Console.WriteLine("Entering ScrapeController.GetScrapeById(Guid scrapeId)");

        return await _scrapeData.GetScrapeById(scrapeId);
    }


    // POST api/Scrape/AddScrape
    [Route("AddScrape", Name = "AddScrape")]
    [HttpPost]
    public async Task AddScrape(ScrapeModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.AddScrape(ScrapeModel scrape)");

        await _scrapeData.AddScrape(scrape.ScrapeScope, scrape.DynamicScrapeObject, scrape.DynamicScrapeObjectId, scrape.ScrapeScheduledDate);
    }


    // POST api/Scrape/UpdateScrape
    [Route("UpdateScrape", Name = "UpdateScrape")]
    [HttpPut]
    public async Task UpdateScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.UpdateScrape(ScrapeDetailedModel scrape)");

        await _scrapeData.UpdateScrape(scrape);
    }


    // POST api/Scrape/ExecuteServerScrape
    [Route("ExecuteServerScrape", Name = "ExecuteServerScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteServerScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteServerScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteServerScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteDatabaseScrape
    [Route("ExecuteDatabaseScrape", Name = "ExecuteDatabaseScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteDatabaseScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteDatabaseScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteDatabaseScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteSchemaScrape
    [Route("ExecuteSchemaScrape", Name = "ExecuteSchemaScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteSchemaScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteSchemaScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteSchemaScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteTableScrape
    [Route("ExecuteTableScrape", Name = "ExecuteTableScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteTableScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteTableScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteTableScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteFieldScrape
    [Route("ExecuteFieldScrape", Name = "ExecuteFieldScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteFieldScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteFieldScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteFieldScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteIndexScrape
    [Route("ExecuteIndexScrape", Name = "ExecuteIndexScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteIndexScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteIndexScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteIndexScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteConstraintScrape
    [Route("ExecuteConstraintScrape", Name = "ExecuteConstraintScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteConstraintScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteConstraintScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteConstraintScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // POST api/Scrape/ExecuteForeignKeyScrape
    [Route("ExecuteForeignKeyScrape", Name = "ExecuteForeignKeyScrape")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeController.ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)");

        bool result = await _scrapeData.ExecuteForeignKeyScrape(scrape);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}
