using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountID { get; set; } // Property for the source account ID
        public int DestinationAccountID { get; set; } // Property for the destination account ID
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNumber{ get; set; }
        public string RecipientBankName { get; set; }
        public string RecipientAccountNumber { get; set; }
        public string RecipientAccountHolderName { get; set; }
        public string IntermediaryBankName { get; set; }
        public string IntermediaryBankAccountNumber { get; set; }

        // Constructor without TransactionId parameter
        public Transaction(int AccountId, int destinationAccountId, string transactionType, decimal amount, string referenceNumber, string recipientBankName, string recipientAccountNumber, string recipientAccountHolderName, string intermediaryBankName, string intermediaryBankAccountNumber)
        {
            AccountID = AccountId;
            DestinationAccountID = destinationAccountId;
            TransactionType = transactionType;
            Amount = amount;
            ReferenceNumber = referenceNumber;
            RecipientBankName = recipientBankName;
            RecipientAccountNumber = recipientAccountNumber;
            RecipientAccountHolderName = recipientAccountHolderName;
            IntermediaryBankName = intermediaryBankName;
            IntermediaryBankAccountNumber = intermediaryBankAccountNumber;
        }
    }
}
