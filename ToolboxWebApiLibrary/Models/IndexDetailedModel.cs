

namespace ToolboxWebApiLibrary.Models;

public class IndexDetailedModel : IndexModel
{
    public string ServerName { get; set; }
    public string DatabaseName { get; set; }
    public string SchemaName { get; set; }
    public string TableName { get; set; }

    public IndexModel ToIndexModel()
    {
        return (IndexModel)this;
    }
}
