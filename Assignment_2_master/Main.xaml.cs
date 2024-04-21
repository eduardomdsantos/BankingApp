using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Assignment;
using System.Windows.Forms;

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            ShowLoginForm();
            Activated += MainWindow_Activated;

        }

        private void MenuUser_Click(object sender, RoutedEventArgs e)
        {
            var fu = new FormUser();
            fu.ShowDialog();

        }
        private void MenuTransaction_Click(object sender, RoutedEventArgs e)
        {
            var ft = new FormTransaction();
            ft.ShowDialog();

        }
        private void MenuViewTransaction_Click(object sender, RoutedEventArgs e)
        {
            var fvt = new FormViewTranscation();
            fvt.ShowDialog();

        }
        private void ShowLoginForm()
        {
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            if (loginForm.CanContinue == false) { CloseForm(); }
        }
        private void CloseForm()
        {
            this.Close();
        }
        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginForm();

        }
        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            CloseForm();

        }
        private void MenuAccounts_Click(object sender, RoutedEventArgs e)
        {
            var fa = new FormAccounts();
            fa.ShowDialog();

        }
        private void LoadAccounts()
        {
            var accountDAO = new AccountDAO();
            DataTable dt = accountDAO.GetAllAccounts();

            dgAccounts.ItemsSource = dt.DefaultView;
        }
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            LoadAccounts();
        }
    }
}
