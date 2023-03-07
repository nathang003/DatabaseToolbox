

namespace ToolboxWebLibrary.Models;

public class IndexModel
{
    public int IndexId { get; set; }
    public int ServerId { get; set; }
    public int DatabaseId { get; set; }
    public int SchemaId { get; set; }
    public int TableId { get; set; }
    public string IndexName
    {
        get; set;
    }
    public string Purpose
    {
        get; set;
    }
    public string IndexType
    {
        get; set;
    }
    public bool IsUnique { get; set; }
    public IndexFieldModel[] Fields { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy
    {
        get; set;
    }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy
    {
        get; set;
    }
}
