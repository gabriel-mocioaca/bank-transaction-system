﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.ApplicationLogic.Data;
using BankingSystem.ApplicationLogic.Services;
using BankingSystem.EFDataAccess;
using BankingSystem.Models;
using BankingSystemExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses.ResultOperators;

namespace Banking_System.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserTransactionsService userTransactionsService;
        private readonly UserService userService;
        
        private readonly ExchangeService exchangeService = new ExchangeService();

        public BankAccountController(UserManager<IdentityUser> userManager,
            UserTransactionsService userTransactionsService,
            UserService userService,
            
            ExchangeService exchangeService)
        {
            this.userManager = userManager;
            this.userTransactionsService = userTransactionsService;
            this.userService = userService;
           
            this.exchangeService = exchangeService;
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string currency = "EUR";

            int FromAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);
            if (FromAccountId == 0)
            {
                userService.AddUser(userManager.GetUserId(User).ToString(), userManager.GetUserName(User));
                userService.AddAccount(userManager.GetUserId(User).ToString(), model.Amount, currency);
                FromAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);
            }

            int ToAccountId = FromAccountId;
            decimal Amount = model.Amount;
            var CurrencyRate = 1;
            var TransactionDate = DateTime.Now;

            decimal oldAmount = userService.GetAccountAmount(userManager.GetUserId(User).ToString(), currency);
            decimal newAmount = 0;

            if (oldAmount == 0)
            {
                newAmount = Amount;
            }
            else
            {
                newAmount = Amount + oldAmount;
            }

            userService.UpdateAmount(userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency) , newAmount);
            
            userTransactionsService.AddTransaction(FromAccountId, ToAccountId, newAmount, CurrencyRate, TransactionDate);

            return View(model);
        }
        [HttpGet]
        public ActionResult Send()
        {
            var model = new SendViewModel();

            return View(model);
        }

     
        [HttpGet]
        public ActionResult Exchange()
        {
            var model = new ExchangeViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Exchange([FromForm]ExchangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var amount = model.Amount;
            string fromCurrencyS = model.FromCurrency.ToString();
            string toCurrencyS = model.ToCurrency.ToString();
            var CurrentUserId = userManager.GetUserId(User).ToString();
            var transactionDate = DateTime.Now;
            var currencyRate = 2;


            var senderAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), model.FromCurrency.ToString());
            

            int toAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), toCurrencyS);
            if (toAccountId == 0)
            {
                
                userService.AddAccount(userManager.GetUserId(User).ToString(), model.Amount, toCurrencyS);
                toAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), toCurrencyS);
            }


            var fromAccount = userService.GetAccount(CurrentUserId , fromCurrencyS);
            var toAccount = userService.GetAccount(CurrentUserId, toCurrencyS);
            
            
            Currency fromCurrency = (Currency)Enum.Parse(typeof(Currency), model.FromCurrency.ToString(), true);
            Currency toCurrency = (Currency)Enum.Parse(typeof(Currency), model.ToCurrency.ToString(), true);


            var fromAmmount = fromAccount.Amount;
            var toAmmount = toAccount.Amount;


            var newExchangedAmount = exchangeService.Convert(fromCurrency, toCurrency , fromAmmount , toAmmount , fromAccount, toAccount , model.Amount , model.Rate);
            decimal oldAmount = userService.GetAccountAmount(userManager.GetUserId(User).ToString(), toCurrencyS);
            decimal newAmount = 0;
            if (oldAmount == 0)
            {
                newAmount = newExchangedAmount;
            }
            else
            {
                newAmount = newExchangedAmount + oldAmount;
            }
            
            if(newAmount < 0)
            {
                return BadRequest("Not enought money");
            }
            
            userTransactionsService.AddTransaction(senderAccountId , toAccountId, newAmount, currencyRate, transactionDate);

            return Redirect(Url.Action("Exchange", "BankAccount"));
        }
    }
}
