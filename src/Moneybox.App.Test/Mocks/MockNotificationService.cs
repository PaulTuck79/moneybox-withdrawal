using Moneybox.App.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneybox.App.Test
{
    public class MockNotificationService : INotificationService
    {
        public void NotifyApproachingPayInLimit(string emailAddress)
        {
            
        }

        public void NotifyFundsLow(string emailAddress)
        {
            
        }
    }
}
