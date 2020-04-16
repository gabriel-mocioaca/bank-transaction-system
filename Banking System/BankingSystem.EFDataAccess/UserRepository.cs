﻿using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BankingSystemDbContext dbContext) : base(dbContext)
        {
            
        }

        public int GetAccountByCurrency(string userId, string currency)
        {
            
            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            foreach (var item in currentUserBankAccouts)
            {
                if (item.Currency == currency)
                    return item.AccountId;
            }
            throw new Exception("");
        }

        public IList<UserBankAccounts> GetAllAccounts(string userId)
        {
            List<UserBankAccounts> currentUserBankAccouts = dbContext.UserBankAccounts.Where(u => u.UserId == userId).ToList();
            return currentUserBankAccouts;
        }

        public User GetUserByName(string userName)
        {
            return dbContext.Users.Where(user => user.UserName == userName).SingleOrDefault(); ;
        }


    }
}
