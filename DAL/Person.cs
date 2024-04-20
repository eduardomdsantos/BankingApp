using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public Person (string firstName, string surName, string email, string phone, string address1, string address2, string city, string county)
        {
            FirstName = firstName;
            Surname = surName;
            Email = email;
            Phone = phone;
            Address1 = address1;
            Address2 = address2;
            City = city;
            County = county;
        }
    }
}
