
using Newtonsoft.Json;

namespace ToolboxWebLibrary.Models;

public class ScrapeModel
{
    public Guid ScrapeId { get; set; }

    public string ScrapeScope { get; set; }

    public string DynamicScrapeObject { get; set; }

    public int DynamicScrapeObjectId { get; set; }

    public DateTime ScrapeScheduledDate { get; set; }

    public Guid ScrapeWorkerId { get; set; }

    public int ScrapeStatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string UpdatedBy { get; set; }

    public ScrapeModel()
    {
        ScrapeId = Guid.NewGuid();
        ScrapeScope = "";
        DynamicScrapeObject = "";
        DynamicScrapeObjectId = 0;
        ScrapeScheduledDate = DateTime.UtcNow;
        ScrapeWorkerId = Guid.Empty;
        ScrapeStatusId = 1;
        CreatedDate = DateTime.UtcNow;
        CreatedBy = "";
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "";
    }
}
