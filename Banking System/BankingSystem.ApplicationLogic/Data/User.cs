
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class User 
    {
        public User()
        {
            UserBankAccounts = new List<UserBankAccounts>();
        }
        public string UserId { get; set; } 
        
        public string UserName { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }
        public virtual ICollection<UserBankAccounts> UserBankAccounts { get; set; }
    }
}
