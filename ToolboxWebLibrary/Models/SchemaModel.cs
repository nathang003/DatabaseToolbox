using System;

namespace ToolboxWebLibrary.Models;

public class SchemaModel
{
    public int SchemaId { get; set; }
    public int ServerId { get; set; }
    public int DatabaseId { get; set; }
    public string SchemaName { get; set; }
    public string Purpose { get; set; }
    public DateTime RemovalDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
