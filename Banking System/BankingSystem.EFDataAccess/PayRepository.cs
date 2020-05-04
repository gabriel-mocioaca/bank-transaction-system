using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class PayRepository : BaseRepository<UserTransaction> , IPayRepository
    {
        public PayRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {

        }

        
    }
}
