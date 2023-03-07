

using ToolboxWebAppLibrary.Models;

namespace ToolboxWebApiLibrary.Models;

public class ScrapeDetailedModel : ScrapeModel
{
    public string DynamicScrapeObjectName
    {
        get; set;
    }
    public string ScrapeStatusName
    {
        get; set;
    }

    public ScrapeDetailedModel()
    {
        ScrapeId = Guid.NewGuid();
        ScrapeScope = "";
        DynamicScrapeObject = "";
        DynamicScrapeObjectId = 0;
        DynamicScrapeObjectName = "";
        ScrapeScheduledDate = DateTime.UtcNow;
        ScrapeWorkerId = Guid.Empty;
        ScrapeStatusId = 2;
        ScrapeStatusName = "Scheduled";
        CreatedDate = DateTime.UtcNow;
        CreatedBy = "";
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "";
    }

    public ScrapeDetailedModel(ScrapeSuggestionModel scrapeSuggestion)
    {
        ScrapeId = Guid.NewGuid();
        ScrapeScope = scrapeSuggestion.ScrapeScope;
        DynamicScrapeObject = scrapeSuggestion.DynamicScrapeObject;
        DynamicScrapeObjectId = scrapeSuggestion.DynamicScrapeObjectId;
        DynamicScrapeObjectName = scrapeSuggestion.DynamicScrapeObjectName;
        ScrapeScheduledDate = DateTime.UtcNow;
        ScrapeWorkerId = Guid.Empty;
        ScrapeStatusId = 2;
        ScrapeStatusName = "Scheduled";
        CreatedDate = DateTime.UtcNow;
        CreatedBy = "";
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "";
    }
}
