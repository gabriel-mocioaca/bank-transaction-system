using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class UserTransaction
    {
        public int TransactionId { get; set; }
 
        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrencyRate { get; set; }
        public DateTime TransactionDate { get; set; }
        public virtual UserBankAccounts FromAccount { get; set; }
        public virtual UserBankAccounts ToAccount { get; set; }
    }
}
