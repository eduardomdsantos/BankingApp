using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;

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
        public Account() : base("", "", "", "", "", "", "", "")
        {
        }

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
        public void ExportToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Account));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files|*.xml",
                Title = "Save Account Details XML File",
                FileName = "account_details.xml" 
            };

            var dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK) 
               
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, this);
                    }
                    File.WriteAllText(saveFileDialog.FileName, stringWriter.ToString());
                }
            }
        }
    }
}
