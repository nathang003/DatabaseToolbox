

namespace ToolboxWebApiLibrary.Models;

public class DataTableLineageModel
{
    public int DataTableLineageId {get; set;}
    public int ParentTableId {get; set;}
    public int ChildTableId {get; set;}
    public DateTime LineageStartDate {get; set;}
    public DateTime LineageEndDate {get; set;}
    public DateTime CreatedDate {get; set;}
    public string CreatedBy {get; set;}
    public DateTime UpdatedDate {get; set;}
    public string UpdatedBy {get; set;}
}
