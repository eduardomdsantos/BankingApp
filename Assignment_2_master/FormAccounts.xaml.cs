using Assignment;
using DAL;
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

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for FormAccounts.xaml
    /// </summary>
    public partial class FormAccounts : Window
    {
        public FormAccounts()
        {
            InitializeComponent();
            CleanFields();
            ControlButton(false);
        }

        AddData add = new AddData();
        UpdateData update = new UpdateData();
        Account foundAccount = null;
        private void CleanFields()
        {
            txtFindAccountID.Clear();
            txtFirstname.Clear();
            txtSurname.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            cboCounty.SelectedIndex = 0;
            cboAccountType.SelectedIndex = 0;
            txtAccountNum.Clear();
            txtSortCode.Clear();
            txtOverdraftLimit.Clear();
        }
        private void ControlButton(bool editMode)
        {
            btnAdd.IsEnabled    = !editMode;
            btnCancel.IsEnabled = editMode;
            btnEdit.IsEnabled   = editMode && foundAccount != null;
            btnExport.IsEnabled = editMode && foundAccount != null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboCounty.ItemsSource = Enum.GetValues(typeof(DAL.Counties));
            cboAccountType.ItemsSource = Enum.GetValues(typeof (DAL.AccountType));
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
        private bool TestRequiredFields()
        {
            if (IsFieldEmptyOrWhitespace(txtAccountNum.Text, txtSortCode.Text, txtFirstname.Text, txtSurname.Text, txtEmail.Text, txtPhone.Text))
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
            int accountNum = int.Parse(txtAccountNum.Text);
            int sortCode = int.Parse(txtSortCode.Text);
            string accountType = cboAccountType.SelectedItem.ToString();
            decimal balance = 0; 
            decimal overdraftLimit = decimal.Parse(txtOverdraftLimit.Text);

            var newAccount = new DAL.Account(firstName, surName, email, phone, address1, address2, city, county,
                                           accountNum, sortCode, accountType, balance, overdraftLimit);

            add.AddAccount(newAccount);

            CleanFields();
            ControlButton(false);
        }
        private void cboAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboAccountType.SelectedItem != null)
            {
                string selectedAccountType = cboAccountType.SelectedItem.ToString();

                if (selectedAccountType == "Current")
                {
                    txtOverdraftLimit.IsEnabled = true;
                }
                else
                {
                    txtOverdraftLimit.IsEnabled = false;
                    txtOverdraftLimit.Text = "0"; 
                }
            }
        }

        private Account FindAccount(int accountID)
        {
            Account accountFound = null;
            DAO dao = new DAO();
            SqlDataReader dr;

            SqlCommand cmd = dao.OpenCon().CreateCommand();
            cmd.CommandText = "GetAccountByID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AccountID", accountID);

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
                    int accountNum = int.Parse(dr["AccountNumber"].ToString());
                    int sortCode = int.Parse(dr["SortCode"].ToString());
                    string accountType = dr["AccountType"].ToString();
                    decimal balance = decimal.Parse(dr["Balance"].ToString());
                    decimal overdraftLimit = decimal.Parse(dr["OverdraftLimit"].ToString());
                    int accountId = int.Parse(dr["AccountID"].ToString());

                    accountFound = new Account(
                        firstName,
                        surname,
                        email,
                        phone,
                        addressLine1,
                        addressLine2,
                        city,
                        county,
                        accountNum,
                        sortCode,
                        accountType,
                        balance,
                        overdraftLimit,
                        accountId
                    );
                }
            }
            finally
            {
                dao.CloseCon();
            }

            return accountFound;
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            foundAccount = null;
            if (!string.IsNullOrWhiteSpace(txtFindAccountID.Text))
            {
                int accountID;
                if (int.TryParse(txtFindAccountID.Text, out accountID))
                {
                    foundAccount = FindAccount(accountID);

                    if (foundAccount != null)
                    {
                        txtFirstname.Text = foundAccount.FirstName;
                        txtSurname.Text = foundAccount.Surname;
                        txtEmail.Text = foundAccount.Email;
                        txtPhone.Text = foundAccount.Phone;
                        txtAddress1.Text = foundAccount.Address1;
                        txtAddress2.Text = foundAccount.Address2;
                        txtCity.Text = foundAccount.City;
                        BIZ.Utils.PopulateComboBox(cboCounty, foundAccount.County);
                        txtAccountNum.Text = foundAccount.AccountNum.ToString();
                        txtSortCode.Text = foundAccount.SortCode.ToString();
                        BIZ.Utils.PopulateComboBox(cboAccountType, foundAccount.AccountType);                        
                        txtOverdraftLimit.Text = foundAccount.OverdraftLimit.ToString();
                        ControlButton(true);
                        return;
                    }
                    else
                    {
                        BIZ.Utils.ShowMessageBox("Account not found!");
                    }
                }
                else
                {
                    BIZ.Utils.ShowMessageBox("Please enter a valid Account ID!");
                }
            }
            else
            {
                BIZ.Utils.ShowMessageBox("Please enter an Account ID!");
            }
            CleanFields();
            ControlButton(false);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TestRequiredFields() == false) { return; }

            int accountID;
            if (!int.TryParse(txtFindAccountID.Text, out accountID))
            {
                BIZ.Utils.ShowMessageBox("Please enter a valid Account ID!");
                return;
            }

            foundAccount.Email = txtEmail.Text;
            foundAccount.Phone = txtPhone.Text;
            foundAccount.Address1 = txtAddress1.Text;
            foundAccount.Address2 = txtAddress2.Text;
            foundAccount.City = txtCity.Text;
            foundAccount.County = cboCounty.SelectedItem.ToString();
            decimal overdraftLimit;
            if (!decimal.TryParse(txtOverdraftLimit.Text, out overdraftLimit))
            {
                BIZ.Utils.ShowMessageBox("Please enter a valid Overdraft Limit!");
                return;
            }
            foundAccount.OverdraftLimit = overdraftLimit;

            
            update.UpdateAccount(foundAccount);

            CleanFields();
            ControlButton(false);
        }
    }
}
