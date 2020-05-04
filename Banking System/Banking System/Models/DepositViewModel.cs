
﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingSystem.Models
{
    public class DepositViewModel
    {
        [Range(1, 10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }


        [Display(Name = "Choose a currency: ")]
        public string Currency { get; set; }
        public List<SelectListItem> Currencies { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "USD", Text = "USD" },
            new SelectListItem { Value = "EUR", Text = "EUR" },
            new SelectListItem { Value = "RON", Text = "RON"  },
        };

    }

}