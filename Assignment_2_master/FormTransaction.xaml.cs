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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment_2_master
{
    /// <summary>
    /// Interaction logic for FormTransaction.xaml
    /// </summary>
    public partial class FormTransaction : Window
    {
        public FormTransaction()
        {
            InitializeComponent();
            CleanFields();
            ControlButton(false);
        }
        AddData add = new AddData();     
        Account sourceAccount = null;
        Account destinationAccount = null;
        private void cmbTransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var transactionType = cmbTransactionType.SelectedItem?.ToString();

            bool enableInternalTransfer = transactionType == "Internal_Transfer";
            bool enableExternalTransfer = transactionType == "External_Transfer";

            tabInternalTransfer.IsEnabled = enableInternalTransfer;
            tabExternalTransfer.IsEnabled = enableExternalTransfer;

            txtDestinationAccountID.IsEnabled = enableInternalTransfer;
            txtInternalTransferReferenceNumber.IsEnabled = enableInternalTransfer;

            txtDestinationAccountID.IsEnabled = enableInternalTransfer;
            txtInternalTransferReferenceNumber.IsEnabled = enableInternalTransfer;
            btnFindDestinationAccountID.IsEnabled = enableInternalTransfer;
            txtDestinationAccountID.Clear();
            txtDestinationFirstName.Clear();
            txtDestinationSurname.Clear();
            txtDestinationAccountNumber.Clear();
            txtInternalTransferReferenceNumber.Clear();

            txtRecipientAccountNumber.IsEnabled = enableExternalTransfer;
            txtRecipientAccountHolderName.IsEnabled = enableExternalTransfer;
            txtIntermediaryBankName.IsEnabled = enableExternalTransfer;
            txtIntermediaryBankAccountNumber.IsEnabled = enableExternalTransfer;
            txtExternalTransferReferenceNumber.IsEnabled = enableExternalTransfer;
            txtRecipientBankName.IsEnabled = enableExternalTransfer;

            txtRecipientAccountNumber.Clear();
            txtRecipientAccountHolderName.Clear();
            txtIntermediaryBankName.Clear();
            txtIntermediaryBankAccountNumber.Clear();
            txtExternalTransferReferenceNumber.Clear();
            txtRecipientBankName.Clear();

            if (enableInternalTransfer)
            {
                tabControl.SelectedItem = tabInternalTransfer;
            }
            else if (enableExternalTransfer)
            {
                tabControl.SelectedItem = tabExternalTransfer;
            }

        }





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbTransactionType.ItemsSource = Enum.GetValues(typeof(DAL.TransactionType));
        }
        private void CleanFields()
        {
            cmbTransactionType.SelectedIndex = 0;

            txtFindAccountID.Clear();
            txtAmount.Clear();
            txtDestinationAccountID.Clear();
            txtInternalTransferReferenceNumber.Clear();

            txtRecipientAccountNumber.Clear();
            txtRecipientAccountHolderName.Clear();
            txtIntermediaryBankName.Clear();
            txtIntermediaryBankAccountNumber.Clear();

            txtFirstName.Clear();
            txtSurname.Clear();
            txtAccountNumber.Clear();
            txtBalance.Clear();

            txtDestinationFirstName.Clear();
            txtDestinationSurname.Clear();
            txtDestinationAccountNumber.Clear();

            txtExternalTransferReferenceNumber.Clear();
            txtRecipientBankName.Clear();
        }
    
        private void ControlButton(bool editMode)
        {
            btnAdd.IsEnabled = !editMode;

        }

        private void btnFindDestinationAccountID_Click(object sender, RoutedEventArgs e)
        {
            destinationAccount = null;
            if (!string.IsNullOrWhiteSpace(txtDestinationAccountID.Text))
            {
                int accountID;
                if (int.TryParse(txtDestinationAccountID.Text, out accountID))
                {
                    destinationAccount = FindAccount(accountID);

                    if (destinationAccount != null)
                    {
                        txtDestinationFirstName.Text = destinationAccount.FirstName;
                        txtDestinationSurname.Text = destinationAccount.Surname;
                        txtDestinationAccountNumber.Text = destinationAccount.AccountNum.ToString();
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
            sourceAccount = null;
            if (!string.IsNullOrWhiteSpace(txtFindAccountID.Text))
            {
                int accountID;
                if (int.TryParse(txtFindAccountID.Text, out accountID))
                {
                    sourceAccount = FindAccount(accountID);

                    if (sourceAccount != null)
                    {
                        txtFirstName.Text = sourceAccount.FirstName;
                        txtSurname.Text = sourceAccount.Surname;
                        txtAccountNumber.Text = sourceAccount.AccountNum.ToString();
                        txtBalance.Text = sourceAccount.Balance.ToString();
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
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string transactionType = cmbTransactionType.SelectedItem?.ToString();
            decimal amount = decimal.Parse(txtAmount.Text);
            string referenceNumber = tabExternalTransfer.IsEnabled ? txtExternalTransferReferenceNumber.Text : txtInternalTransferReferenceNumber.Text;
            string recipientBankName = txtRecipientBankName.Text;
            string recipientAccountNumber = txtRecipientAccountNumber.Text;
            string recipientAccountHolderName = txtRecipientAccountHolderName.Text;
            string intermediaryBankName = txtIntermediaryBankName.Text;
            string intermediaryBankAccountNumber = txtIntermediaryBankAccountNumber.Text;
            int sournceAccountId = sourceAccount.AccountId;
            int destinationAccountId = destinationAccount?.AccountId ?? 0;
            var newTransaction = new DAL.Transaction(sournceAccountId, destinationAccountId, transactionType, amount, referenceNumber, recipientBankName, recipientAccountNumber, recipientAccountHolderName, intermediaryBankName, intermediaryBankAccountNumber);

            add.PerformTransaction(newTransaction);

            CleanFields();
        }
    }
}
