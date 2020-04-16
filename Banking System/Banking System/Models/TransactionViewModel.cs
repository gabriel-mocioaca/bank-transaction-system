using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class TransactionViewModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }

        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        public DateTime DateTime { get; set; }

        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal RateNow { get; set; }
    }
}
