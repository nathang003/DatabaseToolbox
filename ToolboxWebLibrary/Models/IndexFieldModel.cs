
namespace ToolboxWebLibrary.Models;

public class IndexFieldModel
{
    public int IndexFieldId { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; }
    public int DatabaseId { get; set; }
    public string DatabaseName { get; set; }
    public int SchemaId { get; set; }
    public string SchemaName { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; }
    public int IndexId { get; set; }
    public string IndexName { get; set; }
    public int FieldId { get; set; }
    public string FieldName { get; set; }
    public int KeyOrdinal { get; set; }
    public int PartialOrdinal { get; set; }
    public bool IsDescendingKey { get; set; }
    public bool IsIncludedColumn { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }

    public IndexFieldModel()
    {
    }

    public IndexFieldModel(IndexDetailedModel index)
    {
        ServerId = index.ServerId;
        ServerName = index.ServerName;
        DatabaseId = index.DatabaseId;
        DatabaseName = index.DatabaseName;
        SchemaId = index.SchemaId;
        SchemaName = index.SchemaName;
        TableId = index.TableId;
        TableName = index.TableName;
        IndexId = index.IndexId;
        IndexName = index.IndexName;
    }
}
