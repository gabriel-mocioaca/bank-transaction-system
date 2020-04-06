using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking_System.Controllers
{
    public class BankAccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Transactions()
        {

            using (CryptoWalletDBContext ctx = new CryptoWalletDBContext())
            {
                User currentUser = ctx.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

                List<UserBankAccount> currentUserBankAccouts = ctx.UserBankAccounts.Where(u => u.UserId == currentUser.UserId).ToList();

                List<int> currentUserAccountIds = currentUserBankAccouts.Select(b => b.AccountId).ToList();

                List<UserTransaction> currentUserTransactions = ctx.UserTransactions.Where(t => currentUserAccountIds.Contains(t.ToAccountId) || currentUserAccountIds.Contains(t.FromAccountId)).ToList();

                List<TransactionViewModel> viewModel = currentUserTransactions.Select(a => new TransactionViewModel
                {
                    Amount = a.Amount,
                    DateTime = a.TransactionDate,
                    Rate = a.CurrencyRate,
                    FromUser = a.FromAccount.User.Email,
                    ToUser = a.ToAccount.User.Email,
                    FromCurrency = a.FromAccount.Currency,
                    ToCurrency = a.ToAccount.Currency
                }).ToList();
                ExchangeService exchangeService = new ExchangeService();

                foreach (var item in viewModel)
                {
                    Currency fromCurrency = (Currency)Enum.Parse(typeof(Currency), item.FromCurrency, true);
                    Currency toCurrency = (Currency)Enum.Parse(typeof(Currency), item.ToCurrency, true);
                    List<CurrencyRate> rates = exchangeService.GetConversionRate(fromCurrency, new Currency[] { toCurrency });
                    item.RateNow = rates[0].Rate;
                }
                return View(viewModel);
            }

        }
    }
}
