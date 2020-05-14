using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.ApplicationLogic.Abstractions
{
    public interface IUserRepository :IRepository<User>
    {
        int GetAccountIdByCurrency(String userId, string currency);
        UserBankAccounts GetAccountByCurrency(string userId, string currency);
        User GetUserByName(string receiverName);
        IList<UserBankAccounts> GetAllAccounts(string userId);
        UserBankAccounts GetAccount(string UserId, string currency);
        string GetUserId(User receiverUser);
        object GetUserIdByName(string receiverName);
        List<User> getAllUsers();
        bool FirstTimeUser(string v);
        void SetAddress(string userId, string address);
    }
}
