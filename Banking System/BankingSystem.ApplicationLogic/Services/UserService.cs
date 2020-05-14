using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.ApplicationLogic.Services
{
    public class UserService
    {
        private IUserRepository userRepository;
        private IUsersBankAccountRepository userBankAccountRepository;
        public UserService (IUserRepository userRepository , IUsersBankAccountRepository userBankAccountRepository)
        {
            this.userRepository = userRepository;
            this.userBankAccountRepository = userBankAccountRepository;
        }
        public UserBankAccounts GetAccount(string UserId, string currency)
        {
            var account = userRepository.GetAccount(UserId, currency);
            return account;
        }

        public int GetAccountIdByCurrency(string userId, string currency)
        {
            var userBankAccount = userRepository.GetAccountIdByCurrency(userId, currency);

            return userBankAccount;
        }

        public List<User> getAllUsers()
        {
             List < User > userList = userRepository.getAllUsers();
            return userList;
        }

        public UserBankAccounts GetAccountByCurrency(string userId, string currency)
        {
            var userBankAccount = userRepository.GetAccountByCurrency(userId, currency);

            return userBankAccount;
        }

        public IList<UserBankAccounts> GetAllAccounts(string userId)
        {
            //return userRepository.GetAll().Where(x => x.UserBankAccounts.UserId == userId);
            var userBankAccounts = userRepository.GetAllAccounts(userId);
            return userBankAccounts;
        }


        public void AddAccount(string userId, decimal amount , string currency  )
        {
            userBankAccountRepository.Add(new UserBankAccounts() { UserId = userId , Amount = amount, Currency = currency }) ;
        }

        public User GetUserByName(string receiverName)
        {
            var user = userRepository.GetUserByName(receiverName);

            return user;
        }

        

        public void AddUser(string userId , string userName )
        {
            userRepository.Add(new User() {UserId = userId , UserName = userName});
        }

        public bool FirstTimeUser(string v)
        {
            
            return userRepository.FirstTimeUser(v);
        }

        public decimal GetAccountAmount(string userId , string currency)
        {
            return userBankAccountRepository.GetAccountAmount( userId,  currency);
        }

        public void SetAddress(string userId, string address)
        {

            userRepository.SetAddress(userId, address);

        }

        public void UpdateAccount(UserBankAccounts userAccount)
        {
            userBankAccountRepository.Update(userAccount);
        }
       
        public void UpdateAmount( UserBankAccounts userAccount , decimal amount )
        {
            userAccount.Amount = amount;
            userBankAccountRepository.Update(userAccount);
        }

        public string GetUserId(User receiverUser)
        {
            string userId = userRepository.GetUserId(receiverUser);
            return userId;
        }

        public int GetAccountIdByUserIdAndCurrency(string receiverUserId, string currency)
        {
            throw new NotImplementedException();
        }

        public object GetAllDeposits(IList<UserBankAccounts> allAccounts)
        {
            throw new NotImplementedException();
        }

        public List<UserBankAccounts> GetCurrentUserAccounts(string userId)
        {
            return userBankAccountRepository.GetCurrentUserAccounts(userId);
        }
    }
}
