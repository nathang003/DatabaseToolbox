

namespace ToolboxWebLibrary.Models;

public class ScrapeSuggestionModel
{
    public string ScrapeScope { get; set; }
    public string DynamicScrapeObject { get; set; }
    public int DynamicScrapeObjectId { get; set; }
    public string DynamicScrapeObjectName { get; set; }
    public DateTime ScrapeObjectCreatedDate { get; set; }
    public DateTime ScrapeObjectUpdatedDate { get; set; }
    public int DaysSinceCreation { get; set; }
    public int DaysSinceLastUpdate { get; set; }
    public int ScrapePriorityLevel { get; set; }
    public int SuggestionRank { get; set; }
}
