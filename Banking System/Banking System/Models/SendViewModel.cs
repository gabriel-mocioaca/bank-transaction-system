using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class SendViewModel
    {
        [Range(1, 100000)]
        [MaxLength(64)]
        [Display(Name = "From Account:")]
        public string SenderAccountId { get; set; }

        [Required]
        [MaxLength(64)]
        [Display(Name = "To Account:")]
        public string ReceiverName { get; set; }

        [Range(1, 10000)]
        [Display(Name = "The amount of money you wish to transfer")]
        public decimal Amount { get; set; }

        public List<SelectListItem> SenderAccounts { get; set; } = new List<SelectListItem>();

        public string Currency { get; set; }
        public List<SelectListItem> Currencies { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "USD", Text = "USD" },
            new SelectListItem { Value = "EUR", Text = "EUR" },
            new SelectListItem { Value = "RON", Text = "RON"  },
        };

    }
}
