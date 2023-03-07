namespace ToolboxWebLibrary.Api
{
    public interface IScrapeEndpoint
    {
        Task<List<ScrapeModel>> GetAllScrapes();
        Task<List<ScrapeDetailedModel>> GetAllScrapesDetailed();
        Task<List<ScrapeModel>> GetAllUnassignedScrapes();
        Task<List<ScrapeSuggestionModel>> GetSuggestedScrapes();
        Task<List<ScrapeSuggestionModel>> GetTopSuggestedScrapes();
        Task<int> GetSuggestionCount();
        Task<List<ScrapeModel>> GetScrapeById(Guid scrapeId);
        Task<List<ScrapeDetailedModel>> GetMyAssignedScrapes(string userId);
        Task AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime scrapeScheduledDateTime);
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