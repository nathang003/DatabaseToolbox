

namespace ToolboxWebLibrary.Models;

public class ConstraintFieldModel
{
    public int ConstraintFieldId { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; }
    public int DatabaseId { get; set; }
    public string DatabaseName { get; set; }
    public int SchemaId { get; set; }
    public string SchemaName { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; }
    public int ConstraintId { get; set; }
    public string ConstraintName { get; set; }
    public int FieldId { get; set; }
    public string FieldName { get; set; }
    public bool IsNullable { get; set; }
    public bool IsAnsiPadded { get; set; }
    public bool IsRowGuidColumn { get; set; }
    public bool IsIdentity { get; set; }
    public int GeneratedAlwaysType { get; set; }
    public string GeneratedAlwaysTypeDescription { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set;
    }
}
