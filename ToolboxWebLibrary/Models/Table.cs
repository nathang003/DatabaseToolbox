using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace ToolboxWebLibrary.Models;

public class Table : NodeModel
{
    public Table(Point position = null) : base(position, RenderLayer.HTML)
    {
        Columns = new List<Column>
        {
        };
    }

    public string Name { get; set; }
    public List<Column> Columns { get; } 
    public bool HasPrimaryColumn => Columns.Any(c => c.Primary);

    public ColumnPort GetPort(Column column) => Ports.Cast<ColumnPort>().FirstOrDefault(p => p.Column == column);

    public void AddPort(Column column, PortAlignment alignment) => AddPort(new ColumnPort(this, column, alignment));
}
