using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.EFDataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {

        }



        public UserBankAccounts GetAccount(string UserId, string currency)
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

        public string GetUserId(User receiverUser)
        {
            return receiverUser.UserId;
        }

        public string GetUserIdByName(string receiverName)
        {
            throw new NotImplementedException();
        }

        object IUserRepository.GetUserIdByName(string receiverName)
        {
            throw new NotImplementedException();
        }

        public List<User> getAllUsers()
        {
            return dbContext.Users.ToList();
        }

        public bool FirstTimeUser(string v)
        {
            if (dbContext.Users.FirstOrDefault(user => user.UserId == v) != null)
            {
                return true;
            }
            return false;
        }

        /*public string GetAddress(string user)
        {
            var user1 = dbContext.Users.Where(x => x.UserId == user).SingleOrDefault();
            return user1.Address;
        }*/

        public void SetAddress(string userId, string address)
        {
            var user = dbContext.Users.Where(x => x.UserId == userId).SingleOrDefault();
            user.Address = address;
            this.Update(user);
        }

        public Task<string> GetAddress(string user)
        {


            var user1 = dbContext.Users.Where(x => x.UserId == user).SingleOrDefault();

            return Task.FromResult<string>(user1.Address);
        }

        public async Task<string> GetAsynsAdress(string user)
        {
            string adress = await GetAddress(user);
            return adress;
        }
    }
}
