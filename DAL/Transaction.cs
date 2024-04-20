using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransferReference { get; set; }
        public string RecipientBankName { get; set; }
        public string RecipientAccountNumber { get; set; }
        public string RecipientAccountHolderName { get; set; }
        public string IntermediaryBankName { get; set; }
        public string IntermediaryBankAccountNumber { get; set; }
        public Transaction(int transactionId, string transactionType, decimal amount, DateTime transactionDate, string transferReference, string recipientBankName, string recipientAccountNumber, string recipientAccountHolderName, string intermediaryBankName, string intermediaryBankAccountNumber)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
            Amount = amount;
            TransactionDate = transactionDate;
            TransferReference = transferReference;
            RecipientBankName = recipientBankName;
            RecipientAccountNumber = recipientAccountNumber;
            RecipientAccountHolderName = recipientAccountHolderName;
            IntermediaryBankName = intermediaryBankName;
            IntermediaryBankAccountNumber = intermediaryBankAccountNumber;
        }
    }
}
