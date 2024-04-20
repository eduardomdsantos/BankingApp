using System.Data.SqlClient;
using System.Data;
using System;

namespace Assignment
{

    public class LoginDto
    {
        public string Username { get; set; } 
        public string Password { get; set; } 
    }

    public class LoginDAO : DAO
    {
        public bool LoginUser(LoginDto login)
        {
            SqlCommand cmd = OpenCon().CreateCommand();
            cmd.CommandText = "LoginUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", login.Username);
            cmd.Parameters.AddWithValue("@Password", login.Password);

            try
            {
                var dr = cmd.ExecuteReader();
                var result = dr["Authenticated"].ToString();

                if(result == "1")
                    return true;
                return false; 
               
            } catch(Exception ex)
            {
            }
            finally
            {
                CloseCon();
            }

            return false;
        }
    }
}
