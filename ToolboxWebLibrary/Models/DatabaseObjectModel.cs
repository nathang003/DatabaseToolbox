using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxWebLibrary.Models
{
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
}
