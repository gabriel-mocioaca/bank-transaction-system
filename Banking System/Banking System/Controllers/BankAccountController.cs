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
    public class BankAccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserTransactionsService userTransactionsService;
        private readonly UserService userService;
       

        public BankAccountController(UserManager<IdentityUser> userManager, UserTransactionsService userTransactionsService, UserService userService)
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
        public ActionResult Deposit()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Deposit(DepositViewModel model)
        {
            if (ModelState.IsValid)
            {

                string currency = "EUR";

                int FromAccountId = userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency);
                if (FromAccountId == null)
                {
                    userService.AddAccount(userManager.GetUserId(User).ToString(), model.Amount, currency);
                    FromAccountId = userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency);
                }
                //eurAccount.Amount += viewModel.Amount;

                int ToAccountId = FromAccountId;
                decimal Amount = model.Amount;
                var CurrencyRate = 1;
                var TransactionDate = DateTime.Now;
                userTransactionsService.AddTransaction(FromAccountId, ToAccountId, Amount, CurrencyRate, TransactionDate);

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

        }
    }
}
