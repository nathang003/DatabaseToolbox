using System;
using System.Collections;
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
    [System.ComponentModel.ComplexBindingProperties("DataContext","SearchText")]
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        public object DataContext
        {
            get
            {
                return dataGrid.DataContext;
            }
            set
            {
                dataGrid.DataContext = value;
            }
        }

        public string SearchText
        {
            get
            {
                return SearchTextBox.Text.ToString();
            }
            set
            {
                SearchTextBox.Text = value;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGrid.Visibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            dataGrid.Visibility = Visibility.Hidden;
        }

        public event EventHandler<EventArgs> SearchTextChanged;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            if (SearchTextChanged != null)
                SearchTextChanged(this, EventArgs.Empty);
        }
    }
}
