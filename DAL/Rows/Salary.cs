using DAL.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Salary : Row
    {
        public DateTime NgayTinhLuong { get; set; }
        public string IDNhanVien { get; set; }
        public float TienLuong { get; set; }
        public float TienTruLuong { get; set; }
        public float TienThuong { get; set; }
        public float TongTienLuong { get; set; }

        public Salary() { }
        public Salary(Salary s)
        {
            NgayTinhLuong = s.NgayTinhLuong;
            IDNhanVien = s.IDNhanVien;
            TienLuong = s.TienLuong;
            TienTruLuong = s.TienTruLuong;
            TienThuong = s.TienThuong;
            TongTienLuong = s.TongTienLuong;
        }
    }
}
