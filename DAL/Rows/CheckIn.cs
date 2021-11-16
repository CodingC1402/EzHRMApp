using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class CheckIn : Row
    {
        public DateTime ThoiGianVaoLam { get; set; }
        public string IDNhanVien { get; set; }
        public DateTime? ThoiGianTanLam { get; set; }

        public CheckIn(CheckIn checkIn)
        {
            this.ThoiGianVaoLam = checkIn.ThoiGianVaoLam;
            this.IDNhanVien = checkIn.IDNhanVien;
            this.ThoiGianTanLam = checkIn.ThoiGianTanLam;
        }

        public CheckIn() { }

        public override string Save(UnitOfWork uow)
        {
            return BoolToString(CheckInRepo.Instance.Update(new object[] { ThoiGianVaoLam, IDNhanVien }, this, uow));
        }
    }
}
