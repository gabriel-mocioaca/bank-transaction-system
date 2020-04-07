/*using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingSystem.ApplicationLogic.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            UserBankAccounts = new List<UserBankAccount>();
        }
        [Required]
        public int UserId { get; set; }

        [Required]
        public override string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public virtual ICollection<UserBankAccount> UserBankAccounts { get; set; }
    }
}*/
