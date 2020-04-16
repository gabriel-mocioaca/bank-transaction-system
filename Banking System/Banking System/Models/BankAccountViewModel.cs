using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class BankAccountViewModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal CurrencyRate { get; set; }
    }
}
