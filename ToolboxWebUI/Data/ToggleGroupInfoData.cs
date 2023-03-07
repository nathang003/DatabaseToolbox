
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToolboxWebUI.Data;

public class ToggleGroupInfoData : INotifyPropertyChanged
{
    private String _selectedOption;
    public String[] Options { get; set; }
    public String SelectedOption
    {
        get => _selectedOption;
        set
        {
            if (value == _selectedOption) return;
            _selectedOption = value;
            NotifyPropertyChanged();
        }
    }

    public String GetActive(String option)
    {
        return option == SelectedOption ? "active" : "";
    }

    #region Interface

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion
}
