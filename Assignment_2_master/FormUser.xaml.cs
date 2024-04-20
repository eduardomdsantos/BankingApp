using Assignment;
using DAL;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Xml.Linq;

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
            CleanFields();
            ControlButton(false);
        }
        AddData add = new AddData();
        User foundUser = null;
        private void CleanFields()
        {
            txtFirstname.Clear();
            txtSurname.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            cboCounty.SelectedIndex = 0;
            txtUsername.Clear();
            txtPassword.Clear();
        }
        private void ControlButton(bool editMode)
        {
            btnAdd.IsEnabled = !editMode;
            btnCancel.IsEnabled = editMode;
            btnEdit.IsEnabled = editMode && foundUser != null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstname.Text;
            string surName = txtSurname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address1 = txtAddress1.Text;
            string address2 = txtAddress2.Text;
            string city = txtCity.Text;
            string county = cboCounty.SelectedItem.ToString();
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var newUser = new DAL.User(firstName, surName, email, phone, address1, address2, city, county, username, password);

            add.AddUser(newUser);

            CleanFields();
            ControlButton(false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboCounty.ItemsSource = Enum.GetValues(typeof(DAL.Counties));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CleanFields();
            ControlButton(false);
        }
        private User FindUser(int userID)
        {
            User userFound = null;
            DAO dao = new DAO();
            SqlDataReader dr;

            SqlCommand cmd = dao.OpenCon().CreateCommand();
            cmd.CommandText = "GetUserByID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userID);

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string firstName = dr["FirstName"].ToString();
                    string surname = dr["Surname"].ToString();
                    string email = dr["Email"].ToString();
                    string phone = dr["Phone"].ToString();
                    string addressLine1 = dr["AddressLine1"].ToString();
                    string addressLine2 = dr["AddressLine2"].ToString();
                    string city = dr["City"].ToString();
                    string county = dr["County"].ToString();
                    string username = dr["Username"].ToString();

                    userFound = new DAL.User
                    (
                        firstName,
                        surname,
                        email,
                        phone,
                        addressLine1,
                        addressLine2,
                        city,
                        county,
                        username,
                        ""
                    );
                }
            }
            finally
            {
                dao.CloseCon();
            }

            return userFound;
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            foundUser = null;
            if (!string.IsNullOrWhiteSpace(txtFindUserID.Text))
            {
                int userID;
                if (int.TryParse(txtFindUserID.Text, out userID))
                {
                    foundUser = FindUser(userID);

                    if (foundUser != null)
                    {
                        txtFirstname.Text = foundUser.FirstName;
                        txtSurname.Text = foundUser.Surname;
                        txtEmail.Text = foundUser.Email;
                        txtPhone.Text = foundUser.Phone;
                        txtAddress1.Text = foundUser.Address1;
                        txtAddress2.Text = foundUser.Address2;
                        txtCity.Text = foundUser.City;
                        cboCounty.SelectedItem = foundUser.County;
                        txtUsername.Text = foundUser.Username;                      
                        ControlButton(true);
                    }
                    else
                    {
                        BIZ.Utils.ShowMessageBox("Person not found!");
                        CleanFields();
                    }
                }
                else
                {
                    BIZ.Utils.ShowMessageBox("Please enter a valid Person ID!");
                    CleanFields();

                }
            }
            else
            {
                BIZ.Utils.ShowMessageBox("Please enter a Person ID!");
                CleanFields();

            }


        }
    }
}
