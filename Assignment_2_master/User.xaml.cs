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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : MainForm
    {
        public User()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            // Get values from input boxes
            string firstName = txtFirstname.Text;
            string surName = txtSurname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address1 = txtAddress1.Text;
            string address2 = txtAddress2.Text;
            string city = txtCity.Text;
            string county = cboCounty.SelectedItem.ToString();
            string username = txtUsername.Text;
            string password = txtPassword.Text;

           // User newUser = new User(firstName, surName, email, phone, address1, address2, city, county, username, password);
        }
    }
}
