using BankingSystem.ApplicationLogic.Abstractions;
using BankingSystem.ApplicationLogic.Data;
using BankingSystem.ApplicationLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using BankingSystem.ApplicationLogic.Exceptions;

namespace BankingSystem.ApplicationLogic.Tests.Services
{
    [TestClass]
    public class UserServicesTests
    {
        [TestMethod]
        public void GetAccount_ThrowsException_WhenUserIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string badUserId = null;
            string goodUserId = "userId";
            var existingCurrency = "EUR";
            Assert.ThrowsException<Exception>(() =>{
                userService.GetAccount(badUserId , existingCurrency);
            });
        }
        [TestMethod]
        public void GetAccount_ThrowsException_WhenCurrencyIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string UserId = "dsfdsfdsfds";
            string nullCurrency = null;
            string existingCurrency = "EUR";

            Assert.ThrowsException<Exception>(() => {
                userService.GetAccount(UserId, nullCurrency);
            });
        }

        [TestMethod]
        public void GetAccount_ThrowsEnittyNotFound_WhenUserDoesNotExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            var nonExistingUserId = "2cb5f5c9-30d9-4886-a84e-df7aa66b37bf";
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            var existingCurrency = "EUR";

            var userBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = existingCurrency
            };
            userRepoMock.Setup(userRepo => userRepo.GetAccount(existingUserId, existingCurrency)).Returns(userBankAccount);

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.GetAccount(nonExistingUserId, existingCurrency);
            });
        }
        
       
        
       
        [TestMethod]
        public void GetAccountByCurrency_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullUserId = null;
            string userId = "userId";
            string Currency = "EUR";
            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountByCurrency(nullUserId, Currency);
            });
        }

        [TestMethod]
        public void GetAccountByCurrency_ThrowsException_WhenCurrencyIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string UserId = "userId";
            string nullCurrency = null;
            string existingCurrency = "EUR";
            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountByCurrency(UserId, nullCurrency);
            });
        }
        [TestMethod]
        public void GetAccountByCurrency_EntityNotFoundException_WhenAccountDoesNotExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            var nonExistingUserId = "2cb5f5c9-30d9-4886-a84e-df7aa66b37bf";
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            var existingCurrency = "EUR";
            var nonExistingCurrency = "asdsad";
            var userBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = existingCurrency
            };
            userRepoMock.Setup(userRepo => userRepo.GetAccountByCurrency(existingUserId, existingCurrency)).Returns(userBankAccount);

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.GetAccountByCurrency(existingUserId, nonExistingCurrency);
            });
        }
        [TestMethod]
        public void GetAllAccounts_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullUserId = null;
            string userId = "userId";
            
            Assert.ThrowsException<Exception>(() => {
                userService.GetAllAccounts(nullUserId);
            });
        }

        [TestMethod]
        public void GetAllAccounts_EntityNotFoundException_WhenUserHasNoAccounts()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            var nonExistingUserId = "2cb5f5c9-30d9-4886-a84e-df7aa66b37bf";
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            List<UserBankAccounts> nonExistingUserList = null;
            List<UserBankAccounts> existingUserList = new List<UserBankAccounts>();

            var userBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = "EUR"
            };

            existingUserList.Add(userBankAccount);
            userRepoMock.Setup(userRepo => userRepo.GetAllAccounts(existingUserId)).Returns(existingUserList);

            userRepoMock.Setup(userRepo => userRepo.GetAllAccounts(nonExistingUserId)).Returns(nonExistingUserList);

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            

            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.GetAllAccounts(nonExistingUserId);
            });
        }

       

    }
}
