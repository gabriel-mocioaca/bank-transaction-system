using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using BankingSystem.ApplicationLogic.Exceptions;
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
            
            if (UserId == null)
                throw new Exception("Empty UserID");

            if (currency == null)
                throw new Exception("Empty Currency");
            
            var account = userRepository.GetAccount(UserId, currency);
            if (account == null)
                throw new EntityNotFoundException(UserId);

            return account;
        }//

        public int GetAccountIdByCurrency(string userId, string currency)
        {
            if (userId == null)
                throw new Exception("Empty UserID");

            if (currency == null)
                throw new Exception("Empty Currency");

            var userBankAccount = userRepository.GetAccountIdByCurrency(userId, currency);
            

            return userBankAccount;
        }

        public List<User> getAllUsers()
        {
             List < User > userList = userRepository.getAllUsers();
             if (userList == null)
                throw new Exception("Users list empty");
            return userList;
        }
        //
        public UserBankAccounts GetAccountByCurrency(string userId, string currency)
        {
            if (userId == null)
                throw new Exception("Empty UserID");

            if (currency == null)
                throw new Exception("Empty Currency");

            var userBankAccount = userRepository.GetAccountByCurrency(userId, currency);
            if (userBankAccount == null)
                throw new EntityNotFoundException(userId);

            return userBankAccount;
        }
        //
        public IList<UserBankAccounts> GetAllAccounts(string userId)
        {
            if (userId == null)
                throw new Exception("Empty UserID");


            //return userRepository.GetAll().Where(x => x.UserBankAccounts.UserId == userId);
            var userBankAccounts = userRepository.GetAllAccounts(userId);
            if (userBankAccounts == null)
                throw new EntityNotFoundException(userId);
            return userBankAccounts;
        }

        //
        public void AddAccount(string userId, decimal amount , string currency  )
        {
            if (userId == null)
                throw new Exception("Empty UserID");

            if (currency == null)
                throw new Exception("Empty Currency");

           
            userBankAccountRepository.Add(new UserBankAccounts() { UserId = userId , Amount = amount, Currency = currency }) ;

        }
        //
        public User GetUserByName(string receiverName)
        {
            if (receiverName == null)
                throw new Exception("Empty Name");

            var user = userRepository.GetUserByName(receiverName);
            if (user == null)
                throw new EntityNotFoundException(receiverName);
            return user;
        }
        //
        

        public void AddUser(string userId , string userName )
        {
            if (userId == null)
                throw new Exception("Empty UserID");

            if (userName == null)
                throw new Exception("Empty Name");

            userRepository.Add(new User() {UserId = userId , UserName = userName});
        }
        //
        public bool FirstTimeUser(string userId)
        {

            if (userId == null)
                throw new Exception("Empty UserID");


            return userRepository.FirstTimeUser(userId);
        }
        //
        public decimal GetAccountAmount(string userId , string currency)
        {
            if (userId == null)
                throw new Exception("Empty UserID");

            if (currency == null)
                throw new Exception("Empty currency");

            return userBankAccountRepository.GetAccountAmount( userId,  currency);
        }
        //
        public void SetAddress(string userId, string address)
        {

            if (userId == null)
                throw new Exception("Empty UserID");

            if (address == null)
                throw new Exception("Empty address");

            userRepository.SetAddress(userId, address);

        }
        //
        public void UpdateAccount(UserBankAccounts userAccount)
        {
            if (userAccount == null)
                throw new EntityNotFoundException("userAccount null");

            userBankAccountRepository.Update(userAccount);
        }
        
        public void UpdateAmount( UserBankAccounts userAccount , decimal amount )
        {
            if (userAccount == null)
                throw new EntityNotFoundException("userAccount null");

            userAccount.Amount = amount;
            userBankAccountRepository.Update(userAccount);
        }

        public string GetUserId(User receiverUser)
        {
               

            string userId = userRepository.GetUserId(receiverUser);
            if (userId == null)
                throw new Exception("UserId not found");

            return userId;
        }


        public List<UserBankAccounts> GetCurrentUserAccounts(string userId)
        {
            if (userId == null)
                throw new Exception("UserId not found");

            return userBankAccountRepository.GetCurrentUserAccounts(userId);
        }
    }
}
