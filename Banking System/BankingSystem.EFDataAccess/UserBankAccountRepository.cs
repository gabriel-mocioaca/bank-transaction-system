using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class UserBankAccountRepository : BaseRepository<UserBankAccounts> , IUsersBankAccountRepository
    {
        public UserBankAccountRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {
        }
        
    }
}
