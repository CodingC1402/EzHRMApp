using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Timetable : Row
    {
        public DateTime ThoiDiemTao { get; set; }
        public TimeSpan GioVaoLamCacNgayTrongTuan { get; set; }
        public TimeSpan GioVaoLamThuBay { get; set; }
        public TimeSpan GioVaoLamChuNhat { get; set; }
        public TimeSpan GioTanLamCacNgayTrongTuan { get; set; }
        public TimeSpan GioTanLamThuBay { get; set; }
        public TimeSpan GioTanLamChuNhat { get; set; }

        public override bool Save(UnitOfWork uow)
        {
            ThoiDiemTao = DateTime.Now;
            bool res = TimetableRepo.Instance.Add(this, uow);
            TimetableRepo.Instance.FindLatestTimetable();
            return res;
        }
    }
}
