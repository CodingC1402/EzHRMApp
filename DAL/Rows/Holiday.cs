using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Holiday : Row
    {
        public int ID { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int SoNgayNghi { get; set; }
        public string TenDipNghiLe { get; set; }

        public Holiday() { }
        public Holiday(Holiday h)
        {
            ID = h.ID;
            Ngay = h.Ngay;
            Thang = h.Thang;
            SoNgayNghi = h.SoNgayNghi;
            TenDipNghiLe = h.TenDipNghiLe;
        }
    }
}
