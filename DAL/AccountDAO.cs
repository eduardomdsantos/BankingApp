using System.Data.SqlClient;
using System.Data;
using System;

namespace Assignment
{
    public class AccountDAO : DAO
    {
        public DataTable GetAllAccounts()
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = OpenCon().CreateCommand())
            {
                cmd.CommandText = "GetAllAccounts";
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
                finally
                {
                    CloseCon();
                }
            }

            return dt;
        }
    }
}
