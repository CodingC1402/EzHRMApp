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
        public bool DaXoa { get; set; }

        public AccountGroup(AccountGroup ag)
        {
            TenNhomTaiKhoan = ag.TenNhomTaiKhoan;
            QuyenHan = ag.QuyenHan;
            DaXoa = ag.DaXoa;
        }
        public AccountGroup() { }

        public override string Add(UnitOfWork uow = null)
        {
            string result = CheckForError();

            if (result != "")
                return result;

            result = CheckForDuplicate();
            if (result == "group-deleted")
                return this.Save();

            if (result != "")
                return result;

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
                return "Account group name cannot be empty.";
            if (QuyenHan == 0)
                return "Permission cannot be empty.";

            return "";
        }

        private string CheckForDuplicate()
        {
            var item = AccountGroupRepo.Instance.FindByID(new object[] { TenNhomTaiKhoan });
            if (item != null && item.DaXoa)
                return "group-deleted";
            else if (item != null)
                return "Account group name is already taken.";

            return "";
        }
    }
}
