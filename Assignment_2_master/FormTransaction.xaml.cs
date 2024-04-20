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

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for FormTransaction.xaml
    /// </summary>
    public partial class FormTransaction : Window
    {
        public FormTransaction()
        {
            InitializeComponent();
        }
        private void cmbTransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTransactionType.SelectedItem != null)
            {
                string selectedTransactionType = (cmbTransactionType.SelectedItem as ComboBoxItem).Content.ToString();

                if (selectedTransactionType == "Internal Transfer")
                {
                    tabInternalTransfer.IsEnabled = true;
                    tabExternalTransfer.IsEnabled = false;
                }
                else if (selectedTransactionType == "External Transfer")
                {
                    tabInternalTransfer.IsEnabled = false;
                    tabExternalTransfer.IsEnabled = true;
                }
                else
                {
                    tabInternalTransfer.IsEnabled = false;
                    tabExternalTransfer.IsEnabled = false;
                }
            }
        }
    }
}
