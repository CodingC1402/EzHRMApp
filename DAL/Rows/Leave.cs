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
                    LeaveRepo.Instance.Update(new object[] { NgayBatDauNghi }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (LeaveRepo.Instance.Update(new object[] { NgayBatDauNghi }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            if (NgayBatDauNghi < DateTime.Today)
                return "You can't edit or add a leave on a date that is in the past!";

            if (string.IsNullOrEmpty(LyDoNghi))
                return "You can't leave the reason empty!";

            return "";
        }
    }
}
