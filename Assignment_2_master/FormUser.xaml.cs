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
        UpdateData update = new UpdateData();
        User foundUser = null;
        private void CleanFields()
        {
            txtFindUserID.Clear();
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
            if (TestRequiredFields() == false) { return; }
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
                    int userId = int.Parse(dr["UserID"].ToString());
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
                        "",
                        userId

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
                        BIZ.Utils.PopulateComboBox(cboCounty,foundUser.County);
                        txtUsername.Text = foundUser.Username;                      
                        ControlButton(true);
                        return;
                    }
                    else
                    {
                        BIZ.Utils.ShowMessageBox("User not found!");
                    }
                }
                else
                {
                    BIZ.Utils.ShowMessageBox("Please enter a valid User ID!");

                }
            }
            else
            {
                BIZ.Utils.ShowMessageBox("Please enter a User ID!");

            }
            CleanFields();
            ControlButton(false);

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TestRequiredFields() == false) { return; }

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

            var updateUser = new DAL.User(firstName, surName, email, phone, address1, address2, city, county, username, password, foundUser.UserId);

            update.UpdateUser(updateUser);

            CleanFields();
            ControlButton(false);
        }
        private bool TestRequiredFields()
        {
            if (IsFieldEmptyOrWhitespace(txtUsername.Text, txtPassword.Password, txtFirstname.Text, txtSurname.Text, txtEmail.Text, txtPhone.Text))
            {
                BIZ.Utils.ShowMessageBox("All fields are required.");
                return false;
            }
            return true;
        }
        private bool IsFieldEmptyOrWhitespace(params string[] fields)
        {
            foreach (string field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    return true; 
                }
            }
            return false; 
        }
    }
}
