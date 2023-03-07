using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxWebLibrary.Models;

public class SearchModeModel
{
    public String[] Options { get; set; }
    public string SelectedOption { get; set; }

    public String GetActive(String option)
    {
        return option == SelectedOption ? "active" : "";
    }
}
