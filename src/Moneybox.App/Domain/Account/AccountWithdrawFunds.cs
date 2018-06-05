using Moneybox.App.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App
{
    public class AccountWithdrawFunds
    {
        public static void WithdrawFunds(Account account, decimal amount, INotificationService notificationService)
        {
            var fromBalance = account.Balance - amount;
            if (fromBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (fromBalance < 500m)
            {
                notificationService.NotifyFundsLow(account.User.Email);
            }

            account.Balance = account.Balance - amount;
            account.Withdrawn = account.Withdrawn - amount;
        }
    }
}
