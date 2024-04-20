using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Xml.Linq;
using DAL;

namespace Assignment
{
    class AddData : DAO
    {
        public void AddUser(User newUser)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "AddUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@Surname", newUser.Surname);
            cmd.Parameters.AddWithValue("@Email", newUser.Email);
            cmd.Parameters.AddWithValue("@Phone", newUser.Phone);
            cmd.Parameters.AddWithValue("@AddressLine1", newUser.Address1);
            cmd.Parameters.AddWithValue("@AddressLine2", newUser.Address2);
            cmd.Parameters.AddWithValue("@City", newUser.City);
            cmd.Parameters.AddWithValue("@County", newUser.County);
            cmd.Parameters.AddWithValue("@Username", newUser.Username);
            cmd.Parameters.AddWithValue("@Password", newUser.Password);

            cmd.ExecuteNonQuery();
            CloseCon();
        }
        public void AddAccount(Account newAccount)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "AddAccount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", newAccount.FirstName);
            cmd.Parameters.AddWithValue("@Surname", newAccount.Surname);
            cmd.Parameters.AddWithValue("@Email", newAccount.Email);
            cmd.Parameters.AddWithValue("@Phone", newAccount.Phone);
            cmd.Parameters.AddWithValue("@AddressLine1", newAccount.Address1);
            cmd.Parameters.AddWithValue("@AddressLine2", newAccount.Address2);
            cmd.Parameters.AddWithValue("@City", newAccount.City);
            cmd.Parameters.AddWithValue("@County", newAccount.County);
            cmd.Parameters.AddWithValue("@AccountNumber", newAccount.AccountNum);
            cmd.Parameters.AddWithValue("@SortCode", newAccount.SortCode);
            cmd.Parameters.AddWithValue("@AccountType", newAccount.AccountType);
            cmd.Parameters.AddWithValue("@Balance", newAccount.Balance);
            cmd.Parameters.AddWithValue("@OverdraftLimit", newAccount.OverdraftLimit);

            cmd.ExecuteNonQuery();
            CloseCon();
        }
    }
}
