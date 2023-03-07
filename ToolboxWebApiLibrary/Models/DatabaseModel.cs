using System;

namespace ToolboxWebApiLibrary.Models;

public class DatabaseModel
{
    public int DatabaseId { get; set; }
    public int ServerId { get; set; }
    public string DatabaseName { get; set; }
    public string Purpose { get; set; }
    public DateTime RemovalDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
