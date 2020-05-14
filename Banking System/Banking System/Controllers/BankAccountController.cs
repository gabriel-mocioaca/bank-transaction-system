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
        public ActionResult OpenAccount()
        {
            var model = new OpenAccountViewModel();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult OpenAccount(OpenAccountViewModel model)
        {
            string currency = model.Currency;
            decimal Amount = 0;
            int FromAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);

            if (FromAccountId != 0)
            {
                return BadRequest("This account already exists!");
            }

            if( !userService.FirstTimeUser(userManager.GetUserId(User).ToString()) )
            {
                userService.AddUser(userManager.GetUserId(User).ToString(), userManager.GetUserName(User));
            }

         
            userService.AddAccount(userManager.GetUserId(User).ToString(), Amount, currency);

            ViewBag.message = "Account opened successfully!";

            return View(model);
            
        }


        [HttpGet]
        public ActionResult Deposit()
        {
            var model = new DepositViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Deposit(DepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string currency = model.Currency;

            int FromAccountId = userService.GetAccountIdByCurrency(userManager.GetUserId(User).ToString(), currency);
            if (FromAccountId == 0)
            {
                return BadRequest("The account does not exist!");
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

            ViewBag.message = "Transaction successful!";

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

            ViewBag.message = "Transaction successful!";

            return View(model);

        }

        
        public ActionResult Transactions()
        {
            
            var currentUSer = userManager.GetUserId(User).ToString();
            var allAccounts = userService.GetAllAccounts(userManager.GetUserId(User).ToString());

            
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

            if (fromCurrencyS == toCurrencyS)
            {
                return BadRequest("Same currency");
            }

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

            ViewBag.message = "Transaction successful!";

            return Redirect(Url.Action("Exchange", "BankAccount"));
        }
        
        [HttpGet]
        public ActionResult PayService()
        {
            var model = new PayForAServiceViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PayService([FromForm]PayForAServiceViewModel model)
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

            if (newAmountOfSender < 0)
            {
                return BadRequest("Not enought money");
            }

            userService.UpdateAmount(userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency), newAmountOfSender);//sender

            userService.UpdateAmount(userService.GetAccountByCurrency(receiverUserId, currency), newAmountOfReceiver);//receiver

            userTransactionsService.AddTransaction(senderAccountId, receiverAccountId, amount, currencyRate, transactionDate);

            ViewBag.message = "Transaction successful!";

            return View(model);

        }
        
          
        public ActionResult AccountsList()
        {


            List<CurrencyRate> rates = exchangeService.GetConversionRate(Currency.EUR, new Currency[] { Currency.EUR, Currency.BTC, Currency.GBP, Currency.USD, Currency.RON });

            List<CurrencyRateViewModel> viewModel = rates.Select(a => new CurrencyRateViewModel
            {
                Currency = a.Currency.ToString(),
                Rate = a.Rate
            }).ToList();

            var userId = userManager.GetUserId(User).ToString();

            List<UserBankAccounts> currentUserAccounts = userService.GetCurrentUserAccounts(userId);


            List<BankAccountViewModel> accountsViewModels = new List<BankAccountViewModel>();

            accountsViewModels = currentUserAccounts.Select(a => new BankAccountViewModel
            {
                AccountId = a.AccountId,
                Amount = a.Amount,
                Currency = a.Currency
            }).ToList();
            foreach (var item in accountsViewModels)
            {
                item.CurrencyRate = viewModel.FirstOrDefault(s => s.Currency == item.Currency).Rate;
               

            }

            return View(accountsViewModels);

        }
    }
}
