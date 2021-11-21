using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class AccountModel : Account
    {
        public static bool IsAccountExist(string accountName)
        {
            var result = AccountRepo.Instance.FindByID(new object[] { accountName });
            return result != null;
        }

        public static AccountModel GetAccount(string accountName)
        {
            return new AccountModel(AccountRepo.Instance.FindByID(new object[] { accountName }));
        }

        public AccountModel() { }
        public AccountModel(Account account) : base(account) { }
    }
}
