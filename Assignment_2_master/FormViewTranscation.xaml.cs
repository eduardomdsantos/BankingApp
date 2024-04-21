using Assignment;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for FormViewTranscation.xaml
    /// </summary>
    public partial class FormViewTranscation : Window
    {
        public FormViewTranscation()
        {
            InitializeComponent();
            Activated += MainWindow_Activated;
        }
        private void LoadTranscations()
        {
            var transcationDAO = new TransactionDAO();
            DataTable dt = transcationDAO.GetAllTransactions();

            dgTranscations.ItemsSource = dt.DefaultView;
        }
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            LoadTranscations();
        }
    }
}
