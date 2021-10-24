using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class AccountGroupRepo : Repo<AccountGroup>
    {
        public static AccountGroupRepo Instance { get; private set; } = new AccountGroupRepo();
        private AccountGroupRepo()
        {
            TableName = "NHOMTAIKHOAN";
            PKColsName = new string[]
            {
                "TenNhomTaiKhoan"
            };
        }
    }
}
