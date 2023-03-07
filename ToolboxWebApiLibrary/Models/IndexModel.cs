

namespace ToolboxWebApiLibrary.Models;

public class IndexModel : BasicIndexModel
{
    public IndexFieldModel[] Fields { get; set; }

    public BasicIndexModel ToBasicIndexModel()
    {
        return (BasicIndexModel)this;
    }
}
