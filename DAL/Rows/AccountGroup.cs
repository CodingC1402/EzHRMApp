using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class AccountGroup : Row
    {
        public string TenNhomTaiKhoan { get; set; }
        public int QuyenHan { get; set; }

        public AccountGroup(AccountGroup ag)
        {
            TenNhomTaiKhoan = ag.TenNhomTaiKhoan;
            QuyenHan = ag.QuyenHan;
        }
        public AccountGroup() { }

        public override string Save(UnitOfWork uow)
        {
            return BoolToString(AccountGroupRepo.Instance.Update(new object[] { TenNhomTaiKhoan }, this, uow));
        }
    }
}
