

using System.ComponentModel.DataAnnotations;

namespace ToolboxWebApiLibrary.Models;

public class ScrapeSuggestionModel
{
    [Required()]
    public string ScrapeScope { get; set; }

    [Required()]
    public string DynamicScrapeObject { get; set; }

    [Required()]
    public int DynamicScrapeObjectId { get; set; }
        
    [Required()]
	public string DynamicScrapeObjectName { get; set; }
    public DateTime ScrapeObjectCreatedDate { get; set; }
    public DateTime ScrapeObjectUpdatedDate { get; set; }
    public int DaysSinceCreation { get; set; }
    public int DaysSinceLastUpdate { get; set; }
    public int ScrapePriorityLevel { get; set; }
    public int SuggestionRank { get; set; }

    public ScrapeSuggestionModel()
    {
        ScrapeScope = String.Empty;
        DynamicScrapeObject = String.Empty;
        DynamicScrapeObjectId = 0;
        DynamicScrapeObjectName = String.Empty;
    }
}
