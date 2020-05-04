using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class UserBankAccounts
    {
        public UserBankAccounts()
        {
            FromTransactions = new List<UserTransaction>();
            ToTransactions = new List<UserTransaction>();
        }

       //User user = new User();
       
        public int AccountId { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public User User { get; set; }

        public User Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }

        public virtual ICollection<UserTransaction> FromTransactions { get; set; }
        public virtual ICollection<UserTransaction> ToTransactions { get; set; }
    }
}
