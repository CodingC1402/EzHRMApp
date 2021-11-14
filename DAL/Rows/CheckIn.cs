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

        public override string Save(UnitOfWork uow)
        {
            return BoolToString(CheckInRepo.Instance.Update(new object[] { ThoiGianVaoLam, IDNhanVien }, this, uow));
        }
    }
}
