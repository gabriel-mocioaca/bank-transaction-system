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

        [TestMethod]
        public void AddAccount_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullUserId = null;
            string existingUserId = "userId";
            decimal amount = 1;
            string currency = "EUR";

            Assert.ThrowsException<Exception>(() => {
                userService.AddAccount(nullUserId, amount, currency);
            });
        }

        [TestMethod]
        public void AddAccount_ThrowsException_WhenCurrencyIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string userId = "dfasdasdsa";
            decimal amount = 1;
            string nullCurrency = null;
            string existingCurrency = "EUR";

            Assert.ThrowsException<Exception>(() => {
                userService.AddAccount(userId, amount, nullCurrency);
            });
        }


        [TestMethod]
        public void AddUser_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string userName = "userName";
            string nullUserId = null;
            string existingUserId = "userId";

            Assert.ThrowsException<Exception>(() => {
                userService.AddUser(nullUserId, userName);
            });
        }

        [TestMethod]
        public void AddUser_ThrowsException_WhenUserNameIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullUserName = null;
            string existingUserName = "userName";
            string userId = "userId";

            Assert.ThrowsException<Exception>(() => {
                userService.AddUser(userId, nullUserName);
            });
        }

        [TestMethod]
        public void UpdateAccount_ThrowsException_WhenUserAccountNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            var existingCurrency = "EUR";

            UserBankAccounts nonExistingUserBankAccount = null;
            var existingUserBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = existingCurrency
            };

            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.UpdateAccount(nonExistingUserBankAccount);
            });
        }


        [TestMethod]
        public void UpdateAmount_ThrowsException_WhenUserAccountNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            var existingCurrency = "EUR";
            UserBankAccounts nonExistingUserBankAccount = null;
            var existingUserBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = existingCurrency
            };

            decimal amount = 1;

            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.UpdateAmount(nonExistingUserBankAccount, amount);
            });
        }

        [TestMethod]
        public void GetUserId_ThrowsException_WhenUserDoesNotExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            User nonExistingReceiverUser = null;
            User existingReceiverUser = new User
            {
                UserId = "useruseruseruser",
                UserName = "useruser",
                PhoneNo = "64738",
                Address = "user"
            };

            string existingUserId = "das";
            string nonExistingUserId = null;

            userRepoMock.Setup(userRepo => userRepo.GetUserId(nonExistingReceiverUser)).Returns(nonExistingUserId);
            userRepoMock.Setup(userRepo => userRepo.GetUserId(existingReceiverUser)).Returns(existingUserId);

            Assert.ThrowsException<Exception>(() => {
                userService.GetUserId(nonExistingReceiverUser);
            });
        }
        [TestMethod]
        public void FirstTimeUser_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string userId = null;

            Assert.ThrowsException<Exception>(() => {
                userService.FirstTimeUser(userId);
            });
        }

        [TestMethod]
        public void GetAccountAmount_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string userId = null;
            string currency = "asdsa";

            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountAmount(userId, currency);
            });
        }

        [TestMethod]
        public void GetAccountAmount_ThrowsException_WhenCurrencyIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string userId = "ASdasd";
            string currency = null;

            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountAmount(userId, currency);
            });
        }

        [TestMethod]
        public void SetAddress_ThrowsException_WhenUserIdNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string userId = null;
            string address = "sadas";

            Assert.ThrowsException<Exception>(() => {
                userService.SetAddress(userId, address);
            });
        }
        [TestMethod]
        public void SetAddress_ThrowsException_WhenAdressNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string userId = "sadas";
            string address = null;

            Assert.ThrowsException<Exception>(() => {
                userService.SetAddress(userId, address);
            });
        }

        [TestMethod]
        public void GetAccountIdByCurrency_ThrowsException_WhenCurrencyIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string UserId = "userId";
            string nullCurrency = null;
            string existingCurrency = "EUR";
            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountIdByCurrency(UserId, nullCurrency);
            });
        }

        [TestMethod]
        public void GetAccountIdByCurrency_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullUserId = null;
            string existingUserId = "userId";
            string nullCurrency = "EUR";
            Assert.ThrowsException<Exception>(() => {
                userService.GetAccountIdByCurrency(nullUserId, nullCurrency);
            });
        }

        [TestMethod]
        public void GetAllUsers_ThrowsException_WhenUsersListIsEmpty()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            List<User> usersList = null;
            userRepoMock.Setup(userRepo => userRepo.getAllUsers())
                .Returns(usersList);

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            Assert.ThrowsException<Exception>(() => {
                userService.getAllUsers();
            });
        }

        [TestMethod]
        public void GetUserByName_ThrowsException_WhenReceiverNameIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            string nullReceiverName = null;
            string existingReceiverName = "receiverName";

            Assert.ThrowsException<Exception>(() => {
                userService.GetUserByName(nullReceiverName);
            });
        }

        [TestMethod]
        public void GetUserByName_EntityNotFoundException_WhenUserDoesNotExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();
            string existingReceiverName = "receiverName";


            User existingUser = new User();
            User nonExistingUser = null;


            userRepoMock.Setup(userRepo => userRepo.GetUserByName(existingReceiverName)).Returns(nonExistingUser);

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);
            Assert.ThrowsException<EntityNotFoundException>(() => {
                userService.GetUserByName(existingReceiverName);
            });
        }

        [TestMethod]
        public void GetCurrentUserAccounts_ThrowsException_WhenUserIdIsNull()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);


            string existingUserId = "userId";
            string nonExistingUserId = null;

            Assert.ThrowsException<Exception>(() => {
                userService.GetCurrentUserAccounts(nonExistingUserId);
            });
        }


        [TestMethod]
        public void GetAccount_Returns_WhenUserExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            var nonExistingUserId = "2cb5f5c9-30d9-4886-a84e-df7aa66b37bf";
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";

            var existingCurrency = "EUR";

            Exception throwException = null;
            var userBankAccount = new UserBankAccounts
            {
                AccountId = 1,
                UserId = existingUserId,
                Amount = 1,
                Currency = existingCurrency
            };
            userRepoMock.Setup(userRepo => userRepo.GetAccount(existingUserId, existingCurrency)).Returns(userBankAccount);
            UserService userService = new UserService(userRepoMock.Object, userBankAccountRepoMock.Object);

            UserBankAccounts userBank = null;
            try
            {
                userBank = userService.GetAccount(existingUserId, existingCurrency);
            }
            catch (Exception e)
            {
                throwException = e;
            }
            Assert.IsNull(throwException, $"Exception was thrown");
            Assert.IsNotNull(userBank);

        }



        [TestMethod]
        public void GetAccountByCurrency_Return_WhenAccountExist()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IUsersBankAccountRepository> userBankAccountRepoMock = new Mock<IUsersBankAccountRepository>();

            var nonExistingUserId = "2cb5f5c9-30d9-4886-a84e-df7aa66b37bf";
            var existingUserId = "2cb5f5c9-30d9-1986-a84e-df7aa66b37bf";
            Exception throwException = null;
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


            UserBankAccounts userBank = null;
            try
            {
                userBank = userService.GetAccountByCurrency(existingUserId, existingCurrency);
            }
            catch (Exception e)
            {
                throwException = e;
            }
            Assert.IsNull(throwException, $"Exception was thrown");
            Assert.IsNotNull(userBank);
        }



    }
}
