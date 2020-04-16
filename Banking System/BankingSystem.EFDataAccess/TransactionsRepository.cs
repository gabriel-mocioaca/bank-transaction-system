using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class TransactionsRepository : BaseRepository<UserTransaction>, ITransactionsRepository
    {
        public TransactionsRepository(BankingSystemDbContext dbContext):base (dbContext)
        {

        }
       /* public UserTransaction SetAmount(UserTransaction userTransaction)
        {
            return dbContext.UserTransactions.Add(userTransaction);
        }*/
    }
}
