

namespace ToolboxWebLibrary.Models;

public class ForeignKeyModel
{
    public int ForeignKeyId { get; set; }
    public int ServerId { get; set; }
    public int DatabaseId { get; set; }
    public int SchemaId { get; set; }
    public int TableId { get; set; }
    public string ForeignKeyName { get; set; }
    public string Purpose { get; set; }
    public TableModel ConstraintTable { get; set; }
    public FieldModel ConstraintField { get; set; }
    public TableModel ReferencedTable { get; set; }
    public FieldModel ReferencedField { get; set; }
    public bool IsDisabled { get; set; }
    public bool IsNotTrusted { get; set; }
    public string DeleteReferentialActionDescription { get; set; }
    public string UpdateReferentialActionDescription { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
