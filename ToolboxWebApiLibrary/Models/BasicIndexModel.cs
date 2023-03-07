

namespace ToolboxWebApiLibrary.Models;

public class BasicIndexModel
{
    public int IndexId { get; set; }
    public int ServerId { get; set; }
    public int DatabaseId { get; set; }
    public int SchemaId { get; set; }
    public int TableId { get; set; }
    public string IndexName { get; set; }
    public string Purpose { get; set; }
    public int IndexType { get; set; }
    public string IndexTypeDescription { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsUniqueConstraint { get; set; }
    public bool IsUnique { get; set; }
    public bool IsDisabled { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
