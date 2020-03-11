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
    /// Interaction logic for FullnameNormalizeDialog.xaml
    /// </summary>
    public partial class FullnameNormalizeDialog : Window
    {
        public string From;
        public FullnameNormalizeDialog(FullnameNormalizeArgs args)
        {
            InitializeComponent();
            args.From = From;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoneSpace.IsChecked == true)
            {
                From = NoneSpace.Name;

            }
            if (Standard.IsChecked == true)
            {
                From = Standard.Name;
            }
            if (OneSpace.IsChecked == true)
            {
                From = OneSpace.Name;
            }
            this.DialogResult = true;
        }
    }
}
