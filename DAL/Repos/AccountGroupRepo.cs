using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class AccountGroupRepo : Repo<AccountGroup, string>
    {
        public static AccountGroupRepo Instance { get; private set; } = new AccountGroupRepo();
        public override string IDColName => "TenNhomTaiKhoan";
        public override string TableName => "NHOMTAIKHOAN";
    }
}
