

namespace ToolboxWebLibrary.Models;

public class BasicConstraintModel
{
    public int ConstraintId { get; set; }
    public int ServerId { get; set; }
    public int DatabaseId { get; set; }
    public int SchemaId { get; set; }
    public int TableId { get; set; }
    public string ConstraintName { get; set; }
    public string ConstraintDefinition { get; set; }
    public string Purpose { get; set; }
    public string ConstraintType { get; set; }
    public string ConstraintTypeDescription { get; set; }
    public bool IsSystemNamed { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
