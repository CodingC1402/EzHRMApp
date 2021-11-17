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

        public override string Add(UnitOfWork uow = null)
        {
            string result = CheckForError();

            if (result != "")
                return result;

            if (AccountGroupRepo.Instance.FindByID(new object[] { TenNhomTaiKhoan }) != null)
                return "Account group name is already taken.";

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    AccountGroupRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (AccountGroupRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            string result = CheckForError();

            if (result == "")
            {
                if (uow == null)
                {
                    using (var uowNew = new UnitOfWork())
                    {
                        AccountGroupRepo.Instance.Update(new object[] { TenNhomTaiKhoan }, this, uowNew);
                        return ExecuteAndReturn(uowNew);
                    }
                }

                if (AccountGroupRepo.Instance.Update(new object[] { TenNhomTaiKhoan }, this, uow))
                    result = "";
                else
                    result = "Failed!";
            }
            return result;
        }

        public override string CheckForError()
        {
            if (String.IsNullOrWhiteSpace(TenNhomTaiKhoan))
                return "Ten nhom tai khoan khong duoc trong";
            if (QuyenHan == 0)
                return "Quyen han khong the trong";

            return "";
        }
    }
}
