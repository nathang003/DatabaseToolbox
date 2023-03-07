
using ToolboxWebAppLibrary.Models;

namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IScrapeData
    {
        Task<List<ScrapeModel>> GetAllScrapes();
        Task<List<ScrapeDetailedModel>> GetAllScrapesDetailed();
        Task<List<ScrapeDetailedModel>> GetAllUnassignedScrapes();
        Task<List<ScrapeSuggestionModel>> GetSuggestedScrapes();
        Task<List<ScrapeSuggestionModel>> GetTopSuggestedScrapes();
        Task<int> GetSuggestionCount();
        Task<int> GetUnassignedScrapeCount();
        Task<ScrapeModel> GetScrapeById(Guid scrapeId);
        Task AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime? scrapeScheduleTime);
        Task UpdateScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteServerScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteDatabaseScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteSchemaScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteTableScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteFieldScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteIndexScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteConstraintScrape(ScrapeDetailedModel scrape);
        Task<bool> ExecuteForeignKeyScrape(ScrapeDetailedModel scrape);
    }
}