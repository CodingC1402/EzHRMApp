using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class UnitOfWorkInADay : Row
    {
        public DateTime Ngay { get; set; }
        public string IDNhanVien { get; set; }
        public int SoGioLamTrongGio { get; set; }
        public int SoGioLamNgoaiGio { get; set; }
        public int SoSanPhamLamTrongGio { get; set; }
        public int SoSanPhamNgoaiGio { get; set; }
        public int SoHopDongThoaThuanTrongGio { get; set; }
        public int SoHopDongThoaThuanNgoaiGio { get; set; }
    }
}
