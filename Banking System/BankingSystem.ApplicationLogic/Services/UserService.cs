using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSystem.ApplicationLogic.Services
{
    public class UserService
    {
        private IUserRepository userRepository;
        private IUsersBankAccountRepository userBankAccountRepository;
        public UserService (IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.userBankAccountRepository = userBankAccountRepository;
        }
        public UserBankAccounts GetAccount(string UserId, string currency)
        {
            var account = userRepository.GetAccount(UserId, currency);
            return account;
        }

        public int GetAccountByCurrency(string userId, string currency)
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
            userBankAccountRepository.Add(new UserBankAccounts() { Amount = amount , Currency = currency });
        }

        public User GetUserByName(string receiverName)
        {
            var user = userRepository.GetUserByName(receiverName);

            return user;
        }
    }
}
