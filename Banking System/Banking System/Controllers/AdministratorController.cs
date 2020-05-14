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
        public IActionResult AccountsList(string id)
        {
            var model = new AccountsListViewModel();
            model.UserName = id;

            var user = userService.GetUserByName(id);

            string userId = userService.GetUserId(user);

            IList<UserBankAccounts> AccountsList = userService.GetAllAccounts(userId);

            //model.accountsList = AccountsList;

            return View(AccountsList);



        }

    }
}