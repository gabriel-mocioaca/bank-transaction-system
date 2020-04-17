using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Services
{
    public class PayService
    {
        private IPayRepository payREpository;
        public PayService (IPayRepository payREpository)
        {
            this.payREpository = payREpository;
        }
        public void Pay(int senderAccountId,int receiverAccountId,decimal amount, int currencyRate , DateTime transactionDate)
        {
            payREpository.Add(new UserTransaction() { FromAccountId = senderAccountId, ToAccountId = receiverAccountId , Amount = amount , CurrencyRate = currencyRate , TransactionDate = transactionDate });
        }
    }
}
