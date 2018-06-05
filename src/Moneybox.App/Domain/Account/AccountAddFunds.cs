using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App
{
    public class AccountAddFunds
    {
        public static void AddFunds(Account account, decimal amount, INotificationService notificationService)
        {
            var paidIn = account.Balance + amount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (Account.PayInLimit - paidIn < 500m)
            {
                notificationService.NotifyApproachingPayInLimit(account.User.Email);
            }

            account.Balance = account.Balance + amount;
            account.PaidIn = account.PaidIn + amount;
        }
    }
}
