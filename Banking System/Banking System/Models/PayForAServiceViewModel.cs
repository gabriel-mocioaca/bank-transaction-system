using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class PayForAServiceViewModel
    {
        
        [Display(Name = "Choose an account:")]
        public string SenderAccountId { get; set; }
        
        
        [Display(Name = "Choose a service:")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Enter your client code:")]
        public string ClientCode { get; set; }

        [Required]
        [Display(Name = "Enter your bill number:")]
        public string BillNr { get; set; }

        [Display(Name = "The amount of money you wish to transfer:")]
        public decimal Amount { get; set; }

        public List<SelectListItem> SenderAccounts { get; set; } = new List<SelectListItem>();

        public string Currency { get; set; }
        public List<SelectListItem> Services { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "electrica@electrica.com", Text = "Electrica" },
            new SelectListItem { Value = "telekom@telekom.com", Text = "Telekom" },
            new SelectListItem { Value = "digi@digi.com", Text = "Digi"  },
            new SelectListItem { Value = "vodafone@vodafone.com", Text = "Vodafone"  },
            new SelectListItem { Value = "orange@orange.com", Text = "Orange"  },
            new SelectListItem { Value = "gaz@gaz.com", Text = "Gaz"  },
            new SelectListItem { Value = "apa@apa.com", Text = "Apa"  },
        };
        public List<SelectListItem> Currencies { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "USD", Text = "USD" },
            new SelectListItem { Value = "EUR", Text = "EUR" },
            new SelectListItem { Value = "RON", Text = "RON"  },
        };
    }
}
