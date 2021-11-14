using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class WorkhoursInDay : Row
    {
        public DateTime Ngay { get; set; }
        public string IDNhanVien { get; set; }
        public int SoGioLamTrongGio { get; set; }
        public int SoGioLamNgoaiGio { get; set; }

        public override string Save(UnitOfWork uow)
        {
            return BoolToString(WorkhoursInDayRepo.Instance.Update(new object[] { Ngay, IDNhanVien }, this, uow));
        }
    }
}
