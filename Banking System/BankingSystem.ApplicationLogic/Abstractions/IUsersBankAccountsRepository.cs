﻿using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Abstractions
{
    public interface IUsersBankAccountsRepository : IRepository<UserBankAccount>
    {
        CurrencyType GetCurrencyType(UserBankAccount userBankAccount);
    }
}
