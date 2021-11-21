using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Leave : Row
    {
        public string IDNhanVien { get; set; }
        public DateTime NgayBatDauNghi { get; set; }
        public int SoNgayNghi { get; set; }
        public string LyDoNghi { get; set; }
        public int CoPhep { get; set; } = 1;

        public Leave() { }
        public Leave(Leave leave) {
            IDNhanVien = leave.IDNhanVien;
            NgayBatDauNghi = leave.NgayBatDauNghi;
            SoNgayNghi = leave.SoNgayNghi;
            LyDoNghi = leave.LyDoNghi;
        }

        public override string Add(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    LeaveRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (LeaveRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    LeaveRepo.Instance.Update(new object[] { IDNhanVien, NgayBatDauNghi }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (LeaveRepo.Instance.Update(new object[] { IDNhanVien, NgayBatDauNghi }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public string Delete(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    LeaveRepo.Instance.Remove(new object[] { IDNhanVien, NgayBatDauNghi }, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (LeaveRepo.Instance.Remove(new object[] { IDNhanVien, NgayBatDauNghi }, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            Employee employee = EmployeeRepo.Instance.FindByID(new object[] { IDNhanVien });
            if (employee == null)
                return "Employee id doesn't exists";

            if (NgayBatDauNghi < DateTime.Today)
                return "You can't edit or add a leave on a date that is in the past!";

            if (string.IsNullOrEmpty(LyDoNghi))
                return "You can't leave the reason empty!";

            return "";
        }
    }
}
