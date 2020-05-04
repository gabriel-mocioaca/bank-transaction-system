using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class CurrencyType
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Currency { get; set; }

        public int AccountId { get; set; }

        public virtual ICollection<UserBankAccount> Accounts { get; set; }
    }
}
