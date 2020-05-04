using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Abstractions
{
    public interface ITransactionsRepository : IRepository<UserTransaction>
    {
        //UserTransaction SetAmount(UserTransaction userTransaction);
        List<UserTransaction> getTransactionsByAccountId(int accountId);
    }
}
