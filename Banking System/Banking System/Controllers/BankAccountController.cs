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
        private readonly PayService payService;
        private readonly ExchangeService exchangeService = new ExchangeService();

        public BankAccountController(UserManager<IdentityUser> userManager,
            UserTransactionsService userTransactionsService,
            UserService userService,
            PayService payService,
            ExchangeService exchangeService)
        {
            this.userManager = userManager;
            this.userTransactionsService = userTransactionsService;
            this.userService = userService;
            this.payService = payService;
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
            decimal newAmount;

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
            

            string currency = model.SenderAccountId;
            var senderAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);
            var receiverName = model.ReceiverName;

            var receiverUser = userService.GetUserByName(receiverName);
            if (receiverUser == null)
            {
                return BadRequest("User not Found");
            }

            var receiverUserId = userService.GetUserId(receiverUser);

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

            userService.UpdateAmount(userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency), newAmountOfSender);//sender

            userService.UpdateAmount(userService.GetAccountByCurrency(receiverUserId, currency), newAmountOfReceiver);//receiver

            userTransactionsService.AddTransaction(senderAccountId, receiverAccountId, amount, currencyRate, transactionDate);

            return Redirect(Url.Action("Send", "BankAccount"));
        }

        
        

       
        public ActionResult Transactions()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            var currentUSer = userManager.GetUserId(User).ToString();
            var allAccounts = userService.GetAllAccounts(userManager.GetUserId(User).ToString());

            //List<UserTransaction> currentUserTransactions = ctx.UserTransactions.Where(t => currentUserAccountIds.Contains(t.ToAccountId) || currentUserAccountIds.Contains(t.FromAccountId)).ToList();
            List<UserTransaction> userTransactions = new List<UserTransaction>();

            foreach (var item in allAccounts)
            {
                var accountId = item.AccountId;
                List<UserTransaction> transactions = userTransactionsService.getTransactionsByAccountId(accountId);
                userTransactions.AddRange(transactions);
            }

            List<TransactionViewModel> viewModel = new List<TransactionViewModel>();

            viewModel = userTransactions.Select(a => new TransactionViewModel
            {
                ToCurrency = a.ToAccount.Currency,
                Amount = a.Amount,
                DateTime = a.TransactionDate,
                Rate = a.CurrencyRate,
                FromCurrency = a.FromAccount.Currency,
               
            }).ToList();

            //var allExchanges = userService.GetAllExchanges();
            //var allTransactionsSend = userService.GetAllTransactionsSend();
            return View(viewModel);
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
            //var currencyRate = 2;


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

            List<CurrencyRate> rates = exchangeService.GetConversionRate(fromCurrency, new Currency[] { toCurrency });
            model.Rate = rates[0].Rate;

            var fromAmmount = fromAccount.Amount;
            var toAmmount = toAccount.Amount;


            var newExchangedAmount = exchangeService.Convert(fromCurrency, toCurrency , fromAmmount , toAmmount , fromAccount, toAccount , model.Amount , model.Rate);
            decimal oldAmount = userService.GetAccountAmount(userManager.GetUserId(User).ToString(), toCurrencyS);
            decimal newAmount ;

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
            
            userTransactionsService.AddTransaction(senderAccountId , toAccountId, newAmount, model.Rate, transactionDate);

            return Redirect(Url.Action("Exchange", "BankAccount"));
        }
    }
}
