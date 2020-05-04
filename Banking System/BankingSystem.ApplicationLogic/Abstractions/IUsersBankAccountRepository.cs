using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Abstractions
{
    public interface IUsersBankAccountRepository : IRepository<UserBankAccounts> 
    {
        decimal GetAccountAmount(string userId, string currency);
    }
}
