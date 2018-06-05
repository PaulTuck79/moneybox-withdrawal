using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App.DataAccess;
using Moneybox.App.Features;

namespace Moneybox.App.Test
{
    [TestClass]
    public class TransferMoneyTests
    {
        [TestMethod]
        public void TransferMoney_SufficientFunds_UnderPayInLimit()
        {
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), Name = "User", Email = "" };
            var fromAccount = new Account { Id = fromAccountId, Balance = 100, User = user };
            var toAccount = new Account { Id = toAccountId, Balance = 0, User = user };

            IAccountRepository accounts = new MockAccountRepository();
            accounts.Add(fromAccount);
            accounts.Add(toAccount);

            var notificationService = new MockNotificationService();

            TransferMoney transferMoneyFeature = new TransferMoney(accounts, notificationService);

            transferMoneyFeature.Execute(fromAccountId, toAccountId, 100);

            fromAccount = accounts.GetAccountById(fromAccountId);
            toAccount = accounts.GetAccountById(toAccountId);

            Assert.AreEqual(0, fromAccount.Balance);
            Assert.AreEqual(-100, fromAccount.Withdrawn);
            Assert.AreEqual(100, toAccount.Balance);
            Assert.AreEqual(100, toAccount.PaidIn);
        }

        [TestMethod]
        public void TransferMoney_SufficientFunds_OverPayInLimit()
        {
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), Name = "User", Email = "" };
            var fromAccount = new Account { Id = fromAccountId, Balance = Account.PayInLimit + 1, User = user };
            var toAccount = new Account { Id = toAccountId, Balance = 0, User = user };

            IAccountRepository accounts = new MockAccountRepository();
            accounts.Add(fromAccount);
            accounts.Add(toAccount);

            var notificationService = new MockNotificationService();

            TransferMoney transferMoneyFeature = new TransferMoney(accounts, notificationService);
            try
            {
                transferMoneyFeature.Execute(fromAccountId, toAccountId, fromAccount.Balance);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Account pay in limit reached")
                {
                    return;
                }
            }
            Assert.Fail();
        }

        [TestMethod]
        public void TransferMoney_InsufficientFunds()
        {
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), Name = "User", Email = "" };
            var fromAccount = new Account { Id = fromAccountId, Balance = 0, User = user };
            var toAccount = new Account { Id = toAccountId, Balance = 0, User = user };

            IAccountRepository accounts = new MockAccountRepository();
            accounts.Add(fromAccount);
            accounts.Add(toAccount);

            var notificationService = new MockNotificationService();

            TransferMoney transferMoneyFeature = new TransferMoney(accounts, notificationService);
            try
            {
                transferMoneyFeature.Execute(fromAccountId, toAccountId, 100);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Insufficient funds to make transfer")
                {
                    return;
                }
            }
            Assert.Fail();
        }
    }
}
