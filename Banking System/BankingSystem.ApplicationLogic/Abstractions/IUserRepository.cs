using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Abstractions
{
    public interface IUserRepository :IRepository<User>
    {
        int GetAccountByCurrency(String userId, string currency);
        User GetUserByName(string receiverName);

        IList<UserBankAccounts> GetAllAccounts(string userId);
    }
}
