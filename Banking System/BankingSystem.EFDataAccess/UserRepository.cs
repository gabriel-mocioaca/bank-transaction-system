using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {
            
        }

        public UserBankAccounts GetAccount(string UserId , string currency)
        {
            UserBankAccounts account = dbContext.UserBankAccounts.FirstOrDefault(a => a.UserId == UserId && a.Currency == currency);
                return account;
        }
        public int GetAccountIdByCurrency(string userId, string currency)
        {
            
            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            foreach (var item in currentUserBankAccouts)
            {
                if (item.Currency == currency)
                    return item.AccountId;
            }
            return 0;
        }
        
        public UserBankAccounts GetAccountByCurrency(string userId, string currency)
        {

            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            foreach (var item in currentUserBankAccouts)
            {
                if (item.Currency == currency)
                    return item;
            }
            return null;
        }
        public IList<UserBankAccounts> GetAllAccounts(string userId)
        {
            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            return currentUserBankAccouts;
        }

        public User GetUserByName(string userName)
        {
            return dbContext.Users.Where(user => user.UserName == userName).SingleOrDefault(); ;
        }

        


    }
}
