using DAL.Others;
using DAL.Repos;
using System;

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

        public Timetable() { }
        public Timetable(Timetable t)
        {
            ThoiDiemTao = t.ThoiDiemTao;
            GioVaoLamCacNgayTrongTuan = t.GioVaoLamCacNgayTrongTuan;
            GioVaoLamThuBay = t.GioVaoLamThuBay;
            GioVaoLamChuNhat = t.GioVaoLamChuNhat;
            GioTanLamCacNgayTrongTuan = t.GioTanLamCacNgayTrongTuan;
            GioTanLamThuBay = t.GioTanLamThuBay;
            GioTanLamChuNhat = t.GioTanLamChuNhat;
        }
    }
}
