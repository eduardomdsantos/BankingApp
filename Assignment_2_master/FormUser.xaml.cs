using Assignment;
using System;
using System.Windows;

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for FormUser.xaml
    /// </summary>
    public partial class FormUser : Window
    {
        public FormUser()
        {
            InitializeComponent();
        }
        AddData add = new AddData();

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

            var newUser = new DAL.User(firstName, surName, email, phone, address1, address2, city, county, username, password);

            add.AddUser(newUser);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboCounty.ItemsSource = Enum.GetValues(typeof(DAL.Counties));
        }
    }
}
