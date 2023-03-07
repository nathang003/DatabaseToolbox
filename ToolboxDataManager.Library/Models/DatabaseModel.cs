using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.Models
{
    public class DatabaseModel
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string DatabaseName { get; set; }
        public string Purpose { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
