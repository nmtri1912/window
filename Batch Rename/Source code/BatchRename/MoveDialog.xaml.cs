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
    /// Interaction logic for MoveDialog.xaml
    /// </summary>
    public partial class MoveDialog : Window
    {
        public string From;
        public MoveDialog(MoveArgs args)
        {
            InitializeComponent();
            args.From = From;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHeadButton.IsChecked == true)
            {
                From = "Head";
            }
            if (MoveTailButton.IsChecked == true)
            {
                From = "Tail";
            }

            this.DialogResult = true;
        }
    }
}
