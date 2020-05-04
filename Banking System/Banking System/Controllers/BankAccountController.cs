using System;
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

        [HttpPost]
        public ActionResult Send([FromForm]SendViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            string currency = model.SenderAccountId;
            var senderAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);
            var receiverName = model.ReceiverName;

            var receiverUser = userService.GetUserByName(receiverName);
            if (receiverUser == null)
            {
                return BadRequest("User not Found");
            }

            var receiverUserId = userService.GetUserId(receiverUser);//???


            int receiverAccountId = userService.GetAccountIdByCurrency(receiverUserId, currency);
      
            if (receiverAccountId == 0)
            {
                return BadRequest("User does not have account");
            }

            var currencyRate = 1;
            DateTime transactionDate = DateTime.Now;
            var amount = model.Amount;


            decimal oldAmountOfReceiver = userService.GetAccountAmount(receiverUserId, currency);
            decimal newAmountOfReceiver = amount + oldAmountOfReceiver;

            decimal oldAmountOfSender = userService.GetAccountAmount(userManager.GetUserId(User).ToString(), currency);
            decimal newAmountOfSender = oldAmountOfSender - amount;

            if(newAmountOfSender < 0)
            {
                return BadRequest("Not enought money");
            }



            userTransactionsService.AddTransaction(senderAccountId, receiverAccountId, amount, currencyRate, transactionDate);

            userService.UpdateAmount(userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency), newAmountOfSender);//sender

            userService.UpdateAmount(userService.GetAccountByCurrency(receiverUserId, currency), newAmountOfReceiver);//receiver

            return Redirect(Url.Action("Send", "BankAccount"));
        }
    }
}
