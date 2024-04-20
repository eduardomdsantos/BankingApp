using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Assignment
{
    public class UpdateData : DAO
    {
        public void UpdateUser(User updateUser)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "UpdateUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", updateUser.UserId);
            cmd.Parameters.AddWithValue("@FirstName", updateUser.FirstName);
            cmd.Parameters.AddWithValue("@Surname", updateUser.Surname);
            cmd.Parameters.AddWithValue("@Email", updateUser.Email);
            cmd.Parameters.AddWithValue("@Phone", updateUser.Phone);
            cmd.Parameters.AddWithValue("@AddressLine1", updateUser.Address1);
            cmd.Parameters.AddWithValue("@AddressLine2", updateUser.Address2);
            cmd.Parameters.AddWithValue("@City", updateUser.City);
            cmd.Parameters.AddWithValue("@County", updateUser.County);
            cmd.Parameters.AddWithValue("@Username", updateUser.Username);
            cmd.Parameters.AddWithValue("@Password", updateUser.Password);

            cmd.ExecuteNonQuery();
            CloseCon();
        }
    }
}
