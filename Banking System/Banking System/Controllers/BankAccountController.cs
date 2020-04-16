using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.ApplicationLogic.Services;
using BankingSystem.EFDataAccess;
using BankingSystem.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banking_System.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserTransactionsService userTransactionsService;
        private readonly UserService userService;
        

        public BankAccountController(UserManager<IdentityUser> userManager , UserTransactionsService userTransactionsService , UserService userService ) 
        {
            this.userManager = userManager;
            this.userTransactionsService = userTransactionsService;
            this.userService = userService;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentUSer = userManager.GetUserId(User).ToString();
            var allAccounts = userService.GetAllAccounts(userManager.GetUserId(User).ToString());
            return View();
        }

     

        [HttpGet]
        public ActionResult Transactions()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var currentUSer = userManager.GetUserId(User).ToString();
            var allAccounts = userService.GetAllAccounts(userManager.GetUserId(User).ToString());
            
            return View();
        }

       
    }
}
