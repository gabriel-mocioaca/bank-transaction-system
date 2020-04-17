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
        
        private readonly UserService userService;
        private readonly PayService payService;
       

        public BankAccountController(UserManager<IdentityUser> userManager ,  UserService userService , PayService payService) 
        {
            this.userManager = userManager;
           
            this.userService = userService;
            this.payService = payService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

       
        [HttpGet]
        public ActionResult Send()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Send(SendViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string currency = model.SenderAccountId;
            var senderAccountId = userService.GetAccountByCurrency(userManager.GetUserId(User).ToString(), currency);
            var receiverName = model.ReceiverName;
            var receiverUserId = userService.GetUserByName(receiverName).ToString();
            var receiverAccountId = userService.GetAccountByCurrency(receiverUserId, currency);
            var currencyRate = 1;
            DateTime transactionDate = DateTime.Now;
            var amount = model.Amount;
            payService.Pay(senderAccountId, receiverAccountId, amount , currencyRate , transactionDate);
            return View();
        }

        
        

       
    }
}
