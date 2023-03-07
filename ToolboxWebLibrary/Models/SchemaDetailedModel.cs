

using System.ComponentModel.DataAnnotations;

namespace ToolboxWebLibrary.Models;

public class SchemaDetailedModel : SchemaModel
{
    [Required()]
    public string ServerName { get; set; }

    [Required()]
    public string DatabaseName { get; set; }
}
