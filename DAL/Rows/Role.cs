using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Role : Row
    {
        public string TenChucVu { get; set; }
        public string CachTinhLuong { get; set; }
        public float TienLuongMoiGio { get; set; }
        public float TienLuongMoiThang { get; set; }
        public float PhanTramLuongNgoaiGio { get; set; }
        public int DaXoa { get; set; }

        public Role() { }
        public Role(Role role)
        {
            TenChucVu = role.TenChucVu;
            CachTinhLuong = role.CachTinhLuong;
            TienLuongMoiGio = role.TienLuongMoiGio;
            TienLuongMoiThang = role.TienLuongMoiThang;
            PhanTramLuongNgoaiGio = role.PhanTramLuongNgoaiGio;
            DaXoa = role.DaXoa;
        }

    }
}
