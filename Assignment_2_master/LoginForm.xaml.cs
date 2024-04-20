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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginDto = new LoginDto
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password,
            };

            var loginDAO = new LoginDAO();
            
            if(loginDAO.LoginUser(loginDto))
            {
                this.Close();
            }

            MessageBox.Show("Wrong Password", "WrongPassword", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
