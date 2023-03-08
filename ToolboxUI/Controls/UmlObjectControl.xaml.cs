using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolboxUI.Controls
{
    /// <summary>
    /// Interaction logic for UmlObjectControl.xaml
    /// </summary>
    public partial class UmlObjectControl : UserControl
    {
        public UmlObjectControl()
        {
            InitializeComponent();
        }

        public string Title
        {
            get; set;
        }
        public List<string> Values
        {
            get; set;
        }
    }
}
