using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Models
{
    public class ServerModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string Purpose { get; set; }
        public int IsDevelopmentServer { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
