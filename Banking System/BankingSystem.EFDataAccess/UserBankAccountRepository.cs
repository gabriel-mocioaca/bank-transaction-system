using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class UserBankAccountRepository : BaseRepository<UserBankAccounts> , IUsersBankAccountRepository
    {
        public UserBankAccountRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {

        }

        public decimal GetAccountAmount(string userId, string currency)
        {
            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            foreach (var item in currentUserBankAccouts)
            {
                if (item.Currency == currency)
                    return item.Amount;
            }
            return 0;
        }

        public List<UserBankAccounts> GetCurrentUserAccounts(string userId)
        {
            return dbContext.UserBankAccounts.Where(b => b.UserId == userId).ToList();
        }
    }
}
