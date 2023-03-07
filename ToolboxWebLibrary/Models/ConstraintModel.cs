

namespace ToolboxWebLibrary.Models;

public class ConstraintModel : BasicConstraintModel
{
    public ConstraintFieldModel[] Fields
    {
        get; set;
    }

    public BasicConstraintModel ToBasicConstraintModel()
    {
        return (BasicConstraintModel)this;
    }
}
