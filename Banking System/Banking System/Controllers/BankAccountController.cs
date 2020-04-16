using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.ApplicationLogic.Services;
using BankingSystem.EFDataAccess;
using BankingSystem.Models;
using BankingSystemExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banking_System.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        
        private readonly UserService userService;
        private readonly PayService payService;
        private readonly ExchangeService exchangeService = new ExchangeService();

        public BankAccountController(UserManager<IdentityUser> userManager ,  UserService userService , PayService payService) 
        {
            this.userManager = userManager;
            
            this.userService = userService;
            this.payService = payService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentUSer = userManager.GetUserId(User).ToString();
            var allAccounts = userService.GetAllAccounts(userManager.GetUserId(User).ToString());
            return View();
        }

       

        [HttpGet]
        public ActionResult Exchange()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Exchange(ExchangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var amount = model.Amount;
            
            string fromCurrencyS = model.FromCurrency;
            string toCurrencyS = model.ToCurrency;
            var CurrentUserId = userManager.GetUserId(User).ToString();
           

            var transactionDate = DateTime.Now;

            var senderAccountId = userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), model.FromCurrency);
            var receiverAccountId = userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), model.ToCurrency);

            var currencyRate = 1;

            var fromAccount = userService.GetAccount(CurrentUserId , fromCurrencyS);
            var toAccount = userService.GetAccount(CurrentUserId, toCurrencyS);


            Currency fromCurrency = (Currency)Enum.Parse(typeof(Currency), model.FromCurrency, true);
            Currency toCurrency = (Currency)Enum.Parse(typeof(Currency), model.ToCurrency, true);

            var fromAmmount = fromAccount.Amount;
            var toAmmount = toAccount.Amount;

            var newAmount = exchangeService.Convert(fromCurrency, toCurrency , fromAmmount , toAmmount , fromAccount, toAccount , model.Amount , model.Rate);

            payService.Pay(senderAccountId, receiverAccountId, newAmount, currencyRate, transactionDate);

            return View();

        }
    }
}
