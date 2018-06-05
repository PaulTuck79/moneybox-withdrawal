using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Features;

namespace Moneybox.App.Test
{
    [TestClass]
    public class WithdrawMoneyTests
    {
        [TestMethod]
        public void WithdrawMoney_SufficientFundsTest()
        {
            var accountId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), Name = "User", Email = "" };
            var account = new Account { Id = accountId, Balance = 100, User = user };

            IAccountRepository accounts = new MockAccountRepository();
            accounts.Add(account);

            var notificationService = new MockNotificationService();

            WithdrawMoney withdrawMoneyFeature = new WithdrawMoney(accounts, notificationService);
            withdrawMoneyFeature.Execute(accountId, 100);

            account = accounts.GetAccountById(accountId);

            Assert.AreEqual(0, account.Balance);
            Assert.AreEqual(-100, account.Withdrawn);
        }

        [TestMethod]
        public void WithdrawMoney_InsufficientFundsTest()
        {
            var accountId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), Name = "User", Email = "" };
            var account = new Account { Id = accountId, Balance = 0, User = user };

            IAccountRepository accounts = new MockAccountRepository();
            accounts.Add(account);

            var notificationService = new MockNotificationService();

            WithdrawMoney withdrawMoneyFeature = new WithdrawMoney(accounts, notificationService);
            try
            {
                withdrawMoneyFeature.Execute(accountId, 100);
            }
            catch(InvalidOperationException ex)
            {
                if(ex.Message == "Insufficient funds to make transfer")
                {
                    return;
                }
            }
            Assert.Fail();
        }
    }
}
