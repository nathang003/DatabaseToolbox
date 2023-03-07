

namespace ToolboxWebLibrary.Models;

internal class ConstraintDetailedModel : ConstraintModel
{
    public string ServerName { get; set; }
    public string DatabaseName { get; set; }
    public string SchemaName { get; set; }
    public string TableName { get; set; }

    public ConstraintModel ToConstraintModel()
    {
        return (ConstraintModel)this;
    }
}
