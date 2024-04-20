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
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public void ControlGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Background = Brushes.Yellow;
            }
            else if (sender is ComboBox comboBox)
            {
                comboBox.Background = Brushes.Yellow;
            }
        }

        public void ControlLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Background = Brushes.White; 
            }
            else if (sender is ComboBox comboBox)
            {
                comboBox.Background = Brushes.White; 
            }
        }



    }
}
