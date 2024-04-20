using Assignment;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        public bool CanContinue { get; set; }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginDto = new LoginDto
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password,
            };

            var loginDAO = new LoginDAO();
            CanContinue = loginDAO.LoginUser(loginDto);
            if (CanContinue)
            {
                this.Close();
                return;
            }

            BIZ.Utils.ShowMessageBox("WrongPassword");
        }
    }
}
