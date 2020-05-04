using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.ApplicationLogic.Data;
using BankingSystem.ApplicationLogic.Services;
using BankingSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class AdministratorController : Controller
    {
        UserService userService;
        public AdministratorController(UserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public IActionResult Admin()
        {

            List<User> userList = userService.getAllUsers();

            return View(userList);

        }
        [HttpGet]
        public ActionResult AccountsList()
        {
            var model = new AccountsListViewModel();

            return View(model);
        }
        [HttpPost]
        public IActionResult AccountsList(AccountsListViewModel model)
        {
            string userName = model.UserName;

            var user = userService.GetUserByName(userName);

            string userId = userService.GetUserId(user);

            IList<UserBankAccounts> AccountsList = userService.GetAllAccounts(userId);

            return View(AccountsList);
        }

    }
}