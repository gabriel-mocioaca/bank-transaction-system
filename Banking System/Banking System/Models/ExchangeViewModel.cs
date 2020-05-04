using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class ExchangeViewModel
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal Amount { get; set; }

        public decimal Rate { get; set; }
      
        public string Currency { get; set; }
        public List<SelectListItem> Currencies { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "USD", Text = "USD" },
            new SelectListItem { Value = "EUR", Text = "EUR" },
            new SelectListItem { Value = "RON", Text = "RON"  },
        };
    }
}

