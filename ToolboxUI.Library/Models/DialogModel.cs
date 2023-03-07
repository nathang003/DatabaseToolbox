using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Models
{
    public class DialogModel<TResponse>
    {
        public DialogTypeModel DialogType { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public IEnumerable<TResponse> PossibleResponses { get; protected set; }
        public TResponse GivenResponse { get; set; }
        public bool IsResponseGiven { get; private set; }
    }
}
