using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.EventModels
{
    public sealed class NavigateToViewEvent
    {
        public NavigateToViewEvent(string viewModelName)
        {
            ViewModel = viewModelName;
        }

        public string ViewModel { get; }
    }
}
