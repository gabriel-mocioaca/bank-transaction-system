using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class UserBankAccount
    {
        public UserBankAccount()
        {
            FromTransactions = new List<UserTransaction>();
            ToTransactions = new List<UserTransaction>();
        }

       //User user = new User();

        [Key]
        public int AccountId { get; set; }
        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public virtual CurrencyType Currency { get; set; }

        public virtual ICollection<UserTransaction> FromTransactions { get; set; }
        public virtual ICollection<UserTransaction> ToTransactions { get; set; }
    }
}
