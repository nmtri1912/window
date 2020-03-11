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
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for ReplaceDialog.xaml
    /// </summary>
    public partial class ReplaceDialog : Window
    {
        public string From;
        public string To;
        public ReplaceDialog(ReplaceArgs args)
        {
            InitializeComponent();
            if (args != null)
            {
                fromTextBox.Text = args.From;
                toTextBox.Text = args.To;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            From = fromTextBox.Text;
            To = toTextBox.Text;

            this.DialogResult = true;
        }
    }
}
