using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.Models
{
	public class PackageDataLineageModel
	{

		public string SubPackageName { get; set; }
		public string ParentServer { get; set; }
		public string ParentDatabase { get; set; }
		public string ParentSchema { get; set; }
		public string ParentTable { get; set; }
		public string ChildServer { get; set; }
		public string ChildDatabase { get; set; }
		public string ChildSchema { get; set; }
		public string ChildTable { get; set; }

	}
}
