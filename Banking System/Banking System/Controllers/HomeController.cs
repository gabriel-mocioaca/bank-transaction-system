using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Banking_System.Models;
using BankingSystemExchange;
using BankingSystem.Models;

namespace Banking_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ExchangeService exchangeService = new ExchangeService();
            List<CurrencyRate> rates = new ExchangeService().GetConversionRate(Currency.EUR, new Currency[] { Currency.GBP, Currency.USD, Currency.BTC, Currency.RON });
            List<CurrencyRateViewModel> viewModel = rates.Select(a => new CurrencyRateViewModel
            {
                Currency = a.Currency.ToString(),
                Rate = a.Rate
            }).ToList();
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
