using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class OpenAccountViewModel
    {
        [Display(Name = "What type of account do you wish to open?")]
        public string Currency { get; set; }
        public List<SelectListItem> Currencies { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "USD", Text = "USD" },
            new SelectListItem { Value = "EUR", Text = "EUR" },
            new SelectListItem { Value = "RON", Text = "RON"  },
        };
    }
}
