using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;

namespace Assignment.Models
{
    internal class Perform : DAO
    {
        public void loginUser(User loginUser)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "LoginUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", loginUser.Username);
            cmd.Parameters.AddWithValue("@Password", loginUser.Password);

            cmd.ExecuteNonQuery();
            CloseCon();
        }
        public void perfomTransaction(Transaction performTransaction)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "LoginUser";
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@SourceAccountID" performTransaction.A)
        }
    }
}
