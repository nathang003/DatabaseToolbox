using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.Models
{
    public class UserPermissionModel
    {
        public int Id { get; set; }
        public string Permission { get; set; }
        public string Description { get; set; }
        public int Granted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
