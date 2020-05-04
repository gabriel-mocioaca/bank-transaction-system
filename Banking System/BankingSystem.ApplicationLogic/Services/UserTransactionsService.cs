using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Services
{
    public class UserTransactionsService
    {
        private ITransactionsRepository transactionsRepository;

        public UserTransactionsService(ITransactionsRepository transactionsRepository)
        {
            this.transactionsRepository = transactionsRepository;
        }

        public void  AddTransaction( int fromAccountId , int toAccountId , decimal amount , decimal currencyRate , DateTime transactionDate)
        {
            transactionsRepository.Add(new UserTransaction() {  FromAccountId = fromAccountId , ToAccountId = toAccountId , Amount = amount, CurrencyRate = currencyRate, TransactionDate = transactionDate });
        }

    }

}
