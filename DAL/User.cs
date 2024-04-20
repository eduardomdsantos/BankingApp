using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class User:Person
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public User(string firstName, string surName, string email, string phone, string address1,string address2,
            string city,string county,string username, string password, int userId = 0):
            base(firstName,surName,email,phone,address1,address2,city,county)
            
        {
            UserId = userId;
            Username = username;
            Password = password;
        }
    }
}
