

namespace ToolboxWebApiLibrary.Models;

public class DatabaseObjectModel
{
    public int DatabaseObjectId { get; set; }
    public string DatabaseObjectName { get; set; }
    public string DatabaseFullAddress { get; set; }
    public string DatabaseObjectType { get; set; }
    public string Purpose { get; set; }

    public int SortByNameAscending(string name1, string name2)
    {
        return name1.CompareTo(name2);
    }
}
