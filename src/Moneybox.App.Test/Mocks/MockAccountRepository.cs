using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moneybox.App.DataAccess
{
    public class MockAccountRepository : IAccountRepository
    {
        public List<Account> Accounts { get; set; }

        public MockAccountRepository()
        {
            Accounts = new List<Account>();
        }

        public void Add(Account account)
        {
            Accounts.Add(account);
        }

        public Account GetAccountById(Guid accountId)
        {
            return Accounts.Where(a => a.Id == accountId).FirstOrDefault();
        }

        public void Update(Account account)
        {
            var oldAccountRecord = GetAccountById(account.Id);
            if(oldAccountRecord != null)
            {
                Accounts.Remove(oldAccountRecord);
                Accounts.Add(account);
            }
        }
    }
}
