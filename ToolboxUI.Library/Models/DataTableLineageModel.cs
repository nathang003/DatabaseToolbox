using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Models
{
    public class DataTableLineageModel
    {
        public int Id { get; set; }
        public int ParentTableId { get; set; }
        public int ChildTableId { get; set; }
        public DateTime LineageStartDate { get; set; }
        public DateTime LineageEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public override string ToString()
        {
            return $"Id: { Id }, ParentTableId: { ParentTableId }, ChildTableId: { ChildTableId }, LineageStartDate: { LineageStartDate }, LineageEndDate: { LineageEndDate }, CreatedBy: { CreatedBy }, CreatedDate: { CreatedDate }, UpdatedBy: { UpdatedBy }, UpdatedDate: { UpdatedDate }";
        }
    }
}
