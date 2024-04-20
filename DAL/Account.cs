using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Account:Person
    {
        public int AccountId {  get; set; }
        public int AccountNum { get; set; }
        public int SortCode { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public Account(string firstName, string surName,string email, string phone, string address1, string address2, string city, string county,
            int accountNum, int sortCode, string accountType, decimal balance, decimal overdraftLimit, int accountId = 0):
            base(firstName,surName,email,phone,address1,address2,city,county)
        {
            AccountNum = accountNum;
            SortCode = sortCode;
            AccountType = accountType;
            Balance = balance;
            OverdraftLimit = overdraftLimit;
            AccountId = accountId;
        }
    }
}
