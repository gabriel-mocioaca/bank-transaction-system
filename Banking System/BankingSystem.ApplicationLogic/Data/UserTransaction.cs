using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class UserTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrencyRate { get; set; }

        public DateTime TransactionDate { get; set; }

        public virtual UserBankAccount FromAccount { get; set; }
        public virtual UserBankAccount ToAccount { get; set; }
    }
}
