using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class PaymentMethod : Row
    {
        public string Ten { get; set; }
        public int KyHanTraLuongTheoNgay { get; set; }
        public DateTime LanTraLuongCuoi { get; set; }
        public DateTime NgayTinhLuongThangNay { get; set; }

        public PaymentMethod(PaymentMethod cs)
        {
            Ten = cs.Ten;
            KyHanTraLuongTheoNgay = cs.KyHanTraLuongTheoNgay;
            LanTraLuongCuoi = cs.LanTraLuongCuoi;
            NgayTinhLuongThangNay = cs.NgayTinhLuongThangNay;
        }
        public PaymentMethod() { }
    }
}
