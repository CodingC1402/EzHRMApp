using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class CalculateSalary : Row
    {
        public string Ten { get; set; }
        public int KyHanTraLuongTheoNgay { get; set; }
        public DateTime LanTraLuongCuoi { get; set; }
        public DateTime NgayTinhLuongThangNay { get; set; }

        public CalculateSalary(CalculateSalary cs)
        {
            Ten = cs.Ten;
            KyHanTraLuongTheoNgay = cs.KyHanTraLuongTheoNgay;
            LanTraLuongCuoi = cs.LanTraLuongCuoi;
            NgayTinhLuongThangNay = cs.NgayTinhLuongThangNay;
        }
        public CalculateSalary() { }
    }
}
