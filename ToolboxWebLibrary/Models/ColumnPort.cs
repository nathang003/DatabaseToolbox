using Blazor.Diagrams.Core.Models;
using System.Text.Json.Serialization;

namespace ToolboxWebLibrary.Models;

public class ColumnPort : PortModel
{
    public ColumnPort(NodeModel parent, Column column, PortAlignment alignment = PortAlignment.Right) : base(parent, alignment, null, null)
    {
        Column = column;
    }

    [JsonIgnore]
    public Column Column { get; set; }

    public override bool CanAttachTo(PortModel port)
    {
        // Avoid attaching to self port/node
        if (!base.CanAttachTo(port)) return false;

        var targetPort = port as ColumnPort;
        var targetColumn = targetPort.Column;

        /*************
         Default Rules
        **************/
        // if (Column.Type != targetColumn.Type) return false;

        // if (Column.Primary && targetColumn.Primary) return false;

        // if (Column.Primary && targetPort.Links.Count > 0 || targetColumn.Primary && Links.Count > 1) return false;

        return true;
    }
}
